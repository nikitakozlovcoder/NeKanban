import {DeskUsers} from "./deskusers";

export class Desk {
  name: string;
  invite_link: string;
  deskUsers: DeskUsers[];
  constructor(name: string, invite_link: string, deskUsers: DeskUsers[]) {
    this.name = name;
    this.invite_link = invite_link;
    this.deskUsers = deskUsers;
  }
}
