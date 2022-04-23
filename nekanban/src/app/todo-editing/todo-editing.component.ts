import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA} from "@angular/material/dialog";
import {Todo} from "../models/todo";
import {FormControl, Validators} from "@angular/forms";

@Component({
  selector: 'app-todo-editing',
  templateUrl: './todo-editing.component.html',
  styleUrls: ['./todo-editing.component.css']
})
export class TodoEditingComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: {deskId: number, toDo: Todo}) { }

  ngOnInit(): void {
  }
  name = new FormControl(this.data.toDo.name, [Validators.required, Validators.minLength(8)]);
  body = new FormControl(this.data.toDo.body, [Validators.required, Validators.minLength(10)]);
  updateToDo(): void {

  }
}
