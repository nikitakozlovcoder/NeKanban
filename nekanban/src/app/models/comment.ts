import {DeskUser} from "./deskUser";

export class Comment {
  id: number;
  body: string;
  deskUser: DeskUser;
  createdAtUtc: Date;
  constructor(id: number, body: string, user: DeskUser, createdAtUtc: Date) {
    this.id = id;
    this.body = body;
    this.deskUser = user;
    this.createdAtUtc = createdAtUtc;
  }
}
