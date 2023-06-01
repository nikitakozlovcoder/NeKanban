import {DeskUser} from "./deskUser";
import {ToDoUserType} from "../constants/ToDoUserType";

export interface ToDoUser {
  id: number;
  deskUser: DeskUser;
  toDoUserType: ToDoUserType;
  toDoUserTypeName: string;
}
