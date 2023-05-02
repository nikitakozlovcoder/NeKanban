import {DeskUser} from "./deskUser";

export interface ToDoUser {
  id: number;
  deskUser: DeskUser;
  toDoUserType: number;
  toDoUserTypeName: string;
}
