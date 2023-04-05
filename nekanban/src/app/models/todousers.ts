import {DeskUser} from "./deskUser";

export interface ToDoUsers {
  id: number;
  deskUser: DeskUser;
  toDoUserType: number;
  toDoUserTypeName: string;
}
