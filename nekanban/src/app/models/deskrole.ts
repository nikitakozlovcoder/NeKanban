import {Permission} from "./permission";

export  class DeskRole {
  role: number;
  roleName: string;
  permissions: Permission[];
  constructor(role: number, roleName: string, permissions: Permission[]) {
    this.role = role;
    this.roleName = roleName;
    this.permissions = permissions;
  }
}
