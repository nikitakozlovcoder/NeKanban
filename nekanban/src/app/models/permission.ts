export class Permission {
  id: number;
  permission: number
  permissionName: string
  constructor(id: number, permission: number, permissionName: string) {
    this.permission = permission;
    this.permissionName = permissionName;
    this.id = id;
  }
}
