import {Injectable} from "@angular/core";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {AppHttpService} from "./app-http.service";

import {DeskUser} from "../models/deskUser";

@Injectable()
export class DeskUserService {

  constructor(private httpService: AppHttpService) {
  }
  changeRole(deskUserId: number, roleId: number) {
    return this.httpService.put<DeskUser[]>("DesksUsers/ChangeRole/" + deskUserId, {roleId});
  }
}
