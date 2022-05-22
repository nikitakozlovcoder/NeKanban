import {Column} from "./column";
import {ToDoUsers} from "./todousers";

export class Todo {
  id: number;
  name: string;
  body: string;
  column: Column;
  order: number;
  toDoUsers: ToDoUsers[];
  constructor(id: number, name: string, body: string, column: Column, toDoUsers: ToDoUsers[], order: number) {
    this.id = id;
    this.name = name;
    this.body = body;
    this.column = column;
    this.toDoUsers = toDoUsers;
    this.order = order;
  }
}
