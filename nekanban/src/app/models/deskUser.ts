import {User} from "./user";
import {Desk} from "./desk";
import {Role} from "./Role";
import {DeletionReason} from "../constants/deletionReason";

export interface DeskUser {
  id: number;
  user: User;
  preference: number;
  preferenceName: string;
  isOwner: boolean;
  role: Role;
  deletionReason: DeletionReason;
}
