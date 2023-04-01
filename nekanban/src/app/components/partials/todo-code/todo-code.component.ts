import {Component, Input, OnInit} from '@angular/core';

@Component({
  selector: 'app-todo-code',
  templateUrl: './todo-code.component.html',
  styleUrls: ['./todo-code.component.css']
})
export class TodoCodeComponent implements OnInit {

  @Input() todoCode?: number;
  constructor() { }

  ngOnInit(): void {
  }

}
