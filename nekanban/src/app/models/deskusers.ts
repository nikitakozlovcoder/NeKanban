import {User} from "./user";
import {Desk} from "./desk";

export class DeskUsers {
  id: number;
  user: User;
  role: number;
  roleName: string;
  preference: number;
  preferenceName: string;
  constructor(user: User, role: number, roleName: string, id: number, preference: number, preferenceName: string) {
    this.user = user;
    this.role = role;
    this.roleName = roleName;
    this.id = id;
    this.preference = preference;
    this.preferenceName = preferenceName;
  }
}
