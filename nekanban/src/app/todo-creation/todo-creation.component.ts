import {Component, Inject, OnInit} from '@angular/core';

import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {FormControl, Validators} from "@angular/forms";
import {TodoService} from "../services/todo.service";
import {Todo} from "../models/todo";

@Component({
  selector: 'app-todo-creation',
  templateUrl: './todo-creation.component.html',
  styleUrls: ['./todo-creation.component.css']
})
export class TodoCreationComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: {deskId: number}, private todoService: TodoService, public dialogRef: MatDialogRef<TodoCreationComponent>) { }

  ngOnInit(): void {
  }

  name = new FormControl('', [Validators.required, Validators.minLength(3)]);
  body = new FormControl('', [Validators.required, Validators.minLength(10)]);

  createToDo() {
    if (this.name.invalid || this.body.invalid) {
      this.name.markAsTouched();
      this.body.markAsTouched();
    }
    else {
      this.todoService.addToDo(this.data.deskId, this.name.value, this.body.value).subscribe({
        next: (data: Todo[]) => {
          this.dialogRef.close(data);
        }
      });
    }
  }
}
