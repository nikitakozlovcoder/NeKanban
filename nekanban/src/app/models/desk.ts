import {DeskUser} from "./deskUser";

export class Desk {
  id: number;
  name: string;
  inviteLink: string;
  deskUsers: DeskUser[];
  deskUser: DeskUser;
  constructor(id: number, name: string, invite_link: string, deskUser: DeskUser, deskUsers: DeskUser[]) {
    this.name = name;
    this.inviteLink = invite_link;
    this.deskUser = deskUser;
    this.id = id;
    this.deskUsers = deskUsers;
  }
}
