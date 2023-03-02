import {Permission} from "./permission";

export class Role {
  id: number;
  name: string;
  isDefault: boolean;
  permissions: Permission[];
  constructor(id: number, name: string, isDefault: boolean, permissions: Permission[]) {
    this.id = id;
    this.name = name;
    this.isDefault = isDefault;
    this.permissions = permissions;
  }
}
