import {Injectable} from "@angular/core";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {AppHttpService} from "./app-http.service";
import {DeskRole} from "../models/deskrole";
import {DeskUser} from "../models/deskUser";

@Injectable()
export class RolesService {

  constructor(private httpService: AppHttpService) {
  }
  rolesAndPermissions : DeskRole[] = [];
  initRoles() {
    this.httpService.get<DeskRole[]>("Roles/GetRolesAndPermissions/").subscribe(result => {
      this.rolesAndPermissions = result;
    })
  }

  userHasPermission(deskUser : DeskUser, permissionName: string) : boolean {
    return this.rolesAndPermissions.some(el => el.role === deskUser.role && el.permissions.some(perm => perm.permissionName === permissionName));
  }
}
