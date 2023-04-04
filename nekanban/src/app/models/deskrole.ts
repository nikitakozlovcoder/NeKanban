import {Permission} from "./permission";

export  interface DeskRole {
  role: number;
  roleName: string;
  permissions: Permission[];
}
