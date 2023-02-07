﻿import {Injectable} from "@angular/core";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {AppHttpService} from "./app-http.service";
import {DeskRole} from "../models/deskrole";
import {DeskUser} from "../models/deskUser";
import {Role} from "../models/Role";
import {Permission} from "../models/permission";

@Injectable()
export class RolesService {

  constructor(private httpService: AppHttpService) {
  }
  permissions: Permission[] = [];
  initPermissions() {
    this.getAllPermissions().subscribe(result => {
      this.permissions = result;
    })
    /*this.httpService.get<DeskRole[]>("Roles/GetRolesAndPermissions/").subscribe(result => {
      this.rolesAndPermissions = result;
    })*/
  }

  userHasPermission(roles: Role[], deskUser : DeskUser, permissionName: string) : boolean {
    if (deskUser.isOwner) {
      return true;
    }
    return roles.some(el => el.id === deskUser.role.id && el.permissions.some(perm => perm.permissionName === permissionName));
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
