import {Column} from "./column";
import {ToDoUsers} from "./todousers";

export class Todo {
  id: number;
  name: string;
  body?: string;
  column: Column;
  order: number;
  toDoUsers: ToDoUsers[];
  code: number;
  constructor(id: number, name: string, column: Column, toDoUsers: ToDoUsers[], order: number, code: number, body?: string) {
    this.id = id;
    this.name = name;
    this.body = body;
    this.column = column;
    this.toDoUsers = toDoUsers;
    this.order = order;
    this.code = code;
  }
}
