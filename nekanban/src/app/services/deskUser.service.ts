import {Injectable} from "@angular/core";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {ActivatedRoute, Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {BaseHttpService} from "./base_http.service";
import {Desk} from "../models/desk";
import {DeskUsers} from "../models/deskusers";

@Injectable()
export class DeskUserService {

  constructor(private http: HttpClient, private route: ActivatedRoute, private router: Router, public dialog: MatDialog,
              private http_service: BaseHttpService) {
  }
  changeRole(deskUserId: number, role: number) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    const body = {role: role};
    return this.http.put<DeskUsers[]>(this.http_service.base_url + "DesksUsers/ChangeRole/" + deskUserId, body, httpOptions);
  }
}
