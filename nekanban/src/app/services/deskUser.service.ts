import {Injectable} from "@angular/core";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {AppHttpService} from "./app-http.service";

import {DeskUser} from "../models/deskUser";

@Injectable()
export class DeskUserService {

  constructor(private httpService: AppHttpService) {
  }
  changeRole(deskUserId: number, role: number) {
    return this.httpService.put<DeskUser[]>("DesksUsers/ChangeRole/" + deskUserId, {role});
  }
}
