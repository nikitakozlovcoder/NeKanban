import {Injectable} from "@angular/core";
import {AppHttpService} from "./app-http.service";
import {DeskUser} from "../models/deskUser";
import {Role} from "../models/Role";
import {Permission} from "../models/permission";
import {Permissions} from "../constants/Permissions";

@Injectable()
export class RolesService {

  constructor(private httpService: AppHttpService) {
  }
  permissions: Permission[] = [];
  permissionsTypes = Permissions;
  initPermissions() {
    this.getAllPermissions().subscribe(result => {
      this.permissions = result;
    })
  }

  userHasPermission(roles: Role[], deskUser : DeskUser, permission: Permissions) : boolean {
    if (deskUser.isOwner) {
      return true;
    }
    return roles.some(el => el.id === deskUser.role.id && el.permissions.some(perm => perm.permission === permission));
  }

  userHasAtLeastOnePermissionForAllSettings(roles: Role[], deskUser : DeskUser) {
    return this.userHasAtLeastOnePermissionForGeneralSettings(roles, deskUser) ||
      this.userHasAtLeastOnePermissionForUsersSettings(roles, deskUser) ||
      this.userHasAtLeastOnePermissionForRolesSettings(roles, deskUser);
  }

  userHasAtLeastOnePermissionForGeneralSettings(roles: Role[], deskUser : DeskUser) {
    return this.userHasPermission(roles, deskUser, Permissions.UpdateGeneralDesk) ||
      this.userHasPermission(roles, deskUser, Permissions.ManageInviteLink) ||
      this.userHasPermission(roles, deskUser, Permissions.DeleteDesk);
  }

  userHasAtLeastOnePermissionForUsersSettings(roles: Role[], deskUser : DeskUser) {
    return this.userHasPermission(roles, deskUser, Permissions.RemoveUsers) ||
      this.userHasPermission(roles, deskUser, Permissions.ChangeUserRole);
  }

  userHasAtLeastOnePermissionForRolesSettings(roles: Role[], deskUser : DeskUser) {
    return this.userHasPermission(roles, deskUser, Permissions.ManageRoles);
  }

  getRoles(deskId: number) {
    return this.httpService.get<Role[]>(`Roles/GetRoles/${deskId}`);
  }

  getAllPermissions() {
    return this.httpService.get<Permission[]>("ApplicationPermissions/GetPermissions/");
  }

  createRole(deskId: number, name: string) {
    return this.httpService.post<Role[]>(`Roles/CreateRole/${deskId}`, {name});
  }

  updateRole(roleId: number, name: string) {
    return this.httpService.put<Role[]>(`Roles/UpdateRole/${roleId}`, {name});
  }

  setAsDefault(roleId: number) {
    return this.httpService.put<Role[]>(`Roles/SetAsDefault/${roleId}`, {});
  }

  deleteRole(roleId: number) {
    return this.httpService.delete<Role[]>(`Roles/DeleteRole/${roleId}`);
  }

  grantPermission(roleId: number, permission: number) {
    return this.httpService.post<Role[]>(`Roles/GrantPermission/${roleId}`, {permission});
  }

  revokePermission(roleId: number, permission: number) {
    return this.httpService.post<Role[]>(`Roles/RevokePermission/${roleId}`, {permission});
  }
}
