import {User} from "./user";
import {Desk} from "./desk";
import {Role} from "./Role";

export interface DeskUser {
  id: number;
  user: User;
  preference: number;
  preferenceName: string;
  isOwner: boolean;
  role: Role;
}
