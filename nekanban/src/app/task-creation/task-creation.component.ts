import {Component, Inject, Input, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA} from "@angular/material/dialog";
import {Todo} from "../models/todo";

@Component({
  selector: 'app-task-creation',
  templateUrl: './task-creation.component.html',
  styleUrls: ['./task-creation.component.css']
})
export class TaskCreationComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: {todo: Todo, isEdit: boolean}) { }

  employess : string[] = ['ivan', 'petr', 'konstantin', 'adam', 'ivan', 'petr', 'konstantin', 'adam']

  ngOnInit(): void {
  }
  getToDoCreator() {
    return this.data.todo.toDoUsers.find(el => el.toDoUserType == 0);
  }

}
