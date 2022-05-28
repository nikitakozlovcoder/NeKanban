import {Injectable} from "@angular/core";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {ActivatedRoute, Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {BaseHttpService} from "./base_http.service";
import {DeskRole} from "../models/deskrole";
import {Column} from "../models/column";
import {DeskUsers} from "../models/deskusers";

@Injectable()
export class RolesService {

  constructor(private http: HttpClient, private route: ActivatedRoute, private router: Router, public dialog: MatDialog,
              private http_service: BaseHttpService) {
  }
  rolesAndPermissions : DeskRole[] = [];
  initRoles() {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    this.http.get<DeskRole[]>(this.http_service.base_url + "Roles/GetRolesAndPermissions/", httpOptions).subscribe( result => {
      this.rolesAndPermissions = result;
      console.log(this.rolesAndPermissions);
    })
  }
  userHasPermission(deskUser : DeskUsers, permissionName: string) : boolean {
    if (this.rolesAndPermissions.find(el => el.role === deskUser.role && el.permissions.find(perm => perm.permissionName === permissionName) != undefined)) {
      return true;
    }
    return false;
  }
  getRolesAndPermissions() {
    return this.rolesAndPermissions;
  }
}
