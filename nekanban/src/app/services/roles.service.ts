import {Injectable} from "@angular/core";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {BaseHttpService} from "./baseHttp.service";
import {DeskRole} from "../models/deskrole";
import {DeskUser} from "../models/deskUser";

@Injectable()
export class RolesService {

  constructor(private http: HttpClient, private httpService: BaseHttpService) {
  }
  rolesAndPermissions : DeskRole[] = [];
  initRoles() {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    this.http.get<DeskRole[]>(this.httpService.baseUrl + "Roles/GetRolesAndPermissions/", httpOptions).subscribe(result => {
      this.rolesAndPermissions = result;
    })
  }

  userHasPermission(deskUser : DeskUser, permissionName: string) : boolean {
    return !!this.rolesAndPermissions.find(el => el.role === deskUser.role && el.permissions.find(perm => perm.permissionName === permissionName) != undefined);

  }
  getRolesAndPermissions() {
    return this.rolesAndPermissions;
  }
}
