import {DeskUser} from "./deskUser";

export interface Comment {
  id: number;
  body: string;
  deskUser: DeskUser;
  createdAtUtc: Date;
}
