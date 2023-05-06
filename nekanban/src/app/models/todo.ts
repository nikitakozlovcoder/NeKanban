import {Column} from "./column";
import {ToDoUser} from "./toDoUser";

export interface Todo {
  id: number;
  name: string;
  body?: string;
  column: Column;
  order: number;
  toDoUsers: ToDoUser[];
  code: number;
}
