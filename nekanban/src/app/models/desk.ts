import {DeskUser} from "./deskUser";

export interface Desk {
  id: number;
  name: string;
  inviteLink: string;
  deskUsers: DeskUser[];
  deskUser: DeskUser;
}
