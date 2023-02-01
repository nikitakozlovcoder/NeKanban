import {Injectable} from "@angular/core";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {BaseHttpService} from "./baseHttp.service";

import {DeskUser} from "../models/deskUser";

@Injectable()
export class DeskUserService {

  constructor(private http: HttpClient, private httpService: BaseHttpService) {
  }
  changeRole(deskUserId: number, role: number) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    const body = {role: role};
    return this.http.put<DeskUser[]>(this.httpService.baseUrl + "DesksUsers/ChangeRole/" + deskUserId, body, httpOptions);
  }
}
