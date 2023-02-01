import {DeskUser} from "./deskUser";

export class ToDoUsers {
  id: number;
  deskUser: DeskUser;
  toDoUserType: number;
  toDoUserTypeName: string;
  constructor(deskUser: DeskUser, toDoUserType: number, toDoUserTypeName: string, id: number) {
    this.deskUser = deskUser;
    this.toDoUserType = toDoUserType;
    this.toDoUserTypeName = toDoUserTypeName;
    this.id = id;
  }
}
