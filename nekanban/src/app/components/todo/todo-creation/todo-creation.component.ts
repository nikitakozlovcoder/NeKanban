import {Component, Inject, OnInit} from '@angular/core';

import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {FormControl, FormGroup, UntypedFormControl, Validators} from "@angular/forms";
import {TodoService} from "../../../services/todo.service";
import {Todo} from "../../../models/todo";
import {debounce, debounceTime, interval, Subject, switchMap} from "rxjs";

@Component({
  selector: 'app-todo-creation',
  templateUrl: './todo-creation.component.html',
  styleUrls: ['./todo-creation.component.css']
})
export class TodoCreationComponent implements OnInit {

  private draftSubject = new Subject();
  draft: Todo | undefined;
  constructor(@Inject(MAT_DIALOG_DATA) public data: {deskId: number},
              private todoService: TodoService,
              public dialogRef: MatDialogRef<TodoCreationComponent>) { }

  ngOnInit(): void {
    this.getDraft();
    this.initDebounce();
    this.setFormListeners();
  }

  todoFormGroup = new FormGroup({
    id: new FormControl<number>(0),
    name: new FormControl<string>('', [Validators.required, Validators.minLength(3)]),
    body: new FormControl<string>('', [Validators.required, Validators.minLength(10)])
  })

  isLoaded = true;

  createToDo() {
    if (this.todoFormGroup.invalid) {
      this.todoFormGroup.markAsTouched();
    }
    else {
      this.isLoaded = false;
      this.todoService.addToDo(this.data.deskId, this.todoFormGroup.getRawValue() as Todo).subscribe({
        next: (data: Todo[]) => {
          this.isLoaded = true;
          this.dialogRef.close(data);
        }
      });
    }
  }

  applyDraft() {
    if (this.todoFormGroup.invalid) {
      this.todoFormGroup.markAsTouched();
    }
    else {
      this.isLoaded = false;
      this.todoService.applyDraft(this.todoFormGroup.controls.id.value!).subscribe({
        next: (data: Todo) => {
          this.isLoaded = true;
          this.dialogRef.close(
            new Todo(data.id, data.name, data.column, data.toDoUsers, data.order)
          );
        }
      })
    }
  }

  private getDraft() {
    this.todoService.getDraft(this.data.deskId).subscribe( {
      next: (data: Todo) => {
        this.todoFormGroup.patchValue(data, {emitEvent : false});
      }
    });
  }

  private initDebounce() {
    this.draftSubject.pipe(debounceTime(1500),
      switchMap(() => this.todoService.updateDraft(this.todoFormGroup.getRawValue() as Todo))).subscribe({
        next: (data: Todo) => {
          this.todoFormGroup.patchValue(data, {emitEvent : false});
        },
    });
  }

  private setFormListeners() {
    this.todoFormGroup.valueChanges.subscribe({
      next: () => {
        this.draftSubject.next(1);
      }
    })
  }
}
