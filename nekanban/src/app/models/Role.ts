import {Permission} from "./permission";

export interface Role {
  id: number;
  name: string;
  isDefault: boolean;
  permissions: Permission[];
}
