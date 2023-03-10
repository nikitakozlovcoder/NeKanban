import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {Todo} from "../../../models/todo";
import {FormControl, FormGroup, UntypedFormControl, Validators} from "@angular/forms";
import {TodoService} from "../../../services/todo.service";
import {BehaviorSubject, Subject} from "rxjs";

@Component({
  selector: 'app-todo-editing',
  templateUrl: './todo-editing.component.html',
  styleUrls: ['./todo-editing.component.css']
})
export class TodoEditingComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: {deskId: number, toDo: Todo},
              private todoService: TodoService,
              public dialogRef: MatDialogRef<TodoEditingComponent>) { }

  todoFormGroup = new FormGroup({
    id: new FormControl<number>(0),
    name: new FormControl<string>('', [Validators.required, Validators.minLength(3)]),
    body: new FormControl<string>('', [Validators.required, Validators.minLength(10)])
  })

  isLoaded = new BehaviorSubject(false);

  updateLoaded = new BehaviorSubject(true);

  ngOnInit(): void {
    this.getToDo();
  }


  getToDo() {
    this.isLoaded.next(false);
    this.todoService.getToDo(this.data.toDo.id).subscribe({
      next: (data) => {
        this.isLoaded.next(true);
        this.todoFormGroup.patchValue(data);
      }
    })
  }

  updateToDo(): void {
    if (this.todoFormGroup.invalid) {
      this.todoFormGroup.markAsTouched();
    }
    else {
      this.updateLoaded.next(false);
      this.todoService.updateToDo(this.todoFormGroup.getRawValue() as Todo).subscribe({
        next: data => {
          this.updateLoaded.next(true);
          this.dialogRef.close(data);
        },
        error: () => {
        }
      })
    }
  }
}
