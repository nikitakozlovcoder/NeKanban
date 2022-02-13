import {User} from "./user";
import {Desk} from "./desk";

export class DeskUsers {
  user: User[];
  role: number;
  roleName: string;
  constructor(user: User[], role: number, roleName: string) {
    this.user = user;
    this.role = role;
    this.roleName = roleName;
  }
}
