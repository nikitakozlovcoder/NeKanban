import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-task-creation',
  templateUrl: './task-creation.component.html',
  styleUrls: ['./task-creation.component.css']
})
export class TaskCreationComponent implements OnInit {

  constructor() { }

  employess : string[] = ['ivan', 'petr', 'konstantin', 'adam', 'ivan', 'petr', 'konstantin', 'adam']
  ngOnInit(): void {
  }

}
