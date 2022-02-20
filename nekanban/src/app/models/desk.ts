import {DeskUsers} from "./deskusers";

export class Desk {
  id: number;
  name: string;
  invite_link: string;
  deskUsers: DeskUsers[];
  deskUser: DeskUsers;
  constructor(id: number, name: string, invite_link: string, deskUser: DeskUsers, deskUsers: DeskUsers[]) {
    this.name = name;
    this.invite_link = invite_link;
    this.deskUser = deskUser;
    this.id = id;
    this.deskUsers = deskUsers;
  }
}
