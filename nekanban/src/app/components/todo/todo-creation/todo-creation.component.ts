import {Component, Inject, OnInit} from '@angular/core';

import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {FormControl, FormGroup, UntypedFormControl, Validators} from "@angular/forms";
import {TodoService} from "../../../services/todo.service";
import {Todo} from "../../../models/todo";
import {BehaviorSubject, combineLatest, debounce, debounceTime, interval, map, Subject, switchMap} from "rxjs";
import tinymce, {Editor} from "tinymce";
import {DomSanitizer} from "@angular/platform-browser";

@Component({
  selector: 'app-todo-creation',
  templateUrl: './todo-creation.component.html',
  styleUrls: ['./todo-creation.component.css']
})
export class TodoCreationComponent implements OnInit {

  draftSubject = new Subject();
  formLoaded = new BehaviorSubject(false);
  formSubmitLoaded = new BehaviorSubject(true);
  editorLoaded = new BehaviorSubject(false);

  get isLoaded() {
    return combineLatest([this.formLoaded])
      .pipe(map(x => x.every(isLoaded => isLoaded)));
  }

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
    body: new FormControl<string>('')
  })

  setLoaded() {
    this.editorLoaded.next(true);
  }

  applyDraft() {
    if (this.todoFormGroup.invalid) {
      this.todoFormGroup.markAsTouched();
    }
    else {
      this.formSubmitLoaded.next(false);
      this.todoService.updateDraft(this.todoFormGroup.getRawValue() as Todo).subscribe({
        next: (data: Todo) => {
          this.todoFormGroup.patchValue(data, {emitEvent : false});
        },
        complete: () => {
          this.todoService.applyDraft(this.todoFormGroup.controls.id.value!).subscribe({
            next: (data: Todo) => {
              this.formSubmitLoaded.next(true);
              this.dialogRef.close(
                new Todo(data.id, data.name, data.column, data.toDoUsers, data.order)
              );
            }
          })
        }
      })
    }
  }

  private getDraft() {
    this.formLoaded.next(false);
    this.todoService.getDraft(this.data.deskId).subscribe( {
      next: (data: Todo) => {
        this.formLoaded.next(true);
        this.todoFormGroup.patchValue(data, {emitEvent : false});
      }
    });
  }

  private initDebounce() {
    this.draftSubject.pipe(debounceTime(1500),
      switchMap(() => this.todoService.updateDraft(this.todoFormGroup.getRawValue() as Todo))).subscribe({
        next: (data: Todo) => {
          this.todoFormGroup.controls.name.patchValue(data.name, {emitEvent: false});
          this.todoFormGroup.controls.id.patchValue(data.id, {emitEvent: false});
          //this.todoFormGroup.patchValue(data, {emitEvent : false});
        },
    });
  }

  private setFormListeners() {
    this.todoFormGroup.controls.name.valueChanges.subscribe({
      next: () => {
        this.draftSubject.next(1);
      }
    })
  }
}
