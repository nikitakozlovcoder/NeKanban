import {DeskUsers} from "./deskusers";

export class ToDoUsers {
  deskUser: DeskUsers;
  toDoUserType: number;
  toDoUserTypeName: string;
  constructor(deskUser: DeskUsers, toDoUserType: number, toDoUserTypeName: string) {
    this.deskUser = deskUser;
    this.toDoUserType = toDoUserType;
    this.toDoUserTypeName = toDoUserTypeName;
  }
}
