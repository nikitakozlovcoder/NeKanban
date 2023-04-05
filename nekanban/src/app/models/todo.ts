import {Column} from "./column";
import {ToDoUsers} from "./todousers";

export interface Todo {
  id: number;
  name: string;
  body?: string;
  column: Column;
  order: number;
  toDoUsers: ToDoUsers[];
  code: number;
}
