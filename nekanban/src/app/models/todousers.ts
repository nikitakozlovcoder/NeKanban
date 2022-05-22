import {DeskUsers} from "./deskusers";

export class ToDoUsers {
  id: number;
  deskUser: DeskUsers;
  toDoUserType: number;
  toDoUserTypeName: string;
  constructor(deskUser: DeskUsers, toDoUserType: number, toDoUserTypeName: string, id: number) {
    this.deskUser = deskUser;
    this.toDoUserType = toDoUserType;
    this.toDoUserTypeName = toDoUserTypeName;
    this.id = id;
  }
}
