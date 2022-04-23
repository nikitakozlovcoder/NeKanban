import {Column} from "./column";
import {ToDoUsers} from "./todousers";

export class Todo {
  id: number;
  name: string;
  body: string;
  column: Column;
  toDoUsers: ToDoUsers;
  constructor(id: number, name: string, body: string, column: Column, toDoUsers: ToDoUsers) {
    this.id = id;
    this.name = name;
    this.body = body;
    this.column = column;
    this.toDoUsers = toDoUsers;
  }
}
