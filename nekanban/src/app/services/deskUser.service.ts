import {Injectable} from "@angular/core";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {AppHttpService} from "./app-http.service";

import {DeskUser} from "../models/deskUser";
import {Desk} from "../models/desk";
import {UserStorageService} from "./userStorage.service";

@Injectable()
export class DeskUserService {

  constructor(private httpService: AppHttpService,
              private readonly userStorageService: UserStorageService) {
  }
  changeRole(deskUserId: number, roleId: number) {
    return this.httpService.put<DeskUser[]>(`DesksUsers/ChangeRole/${deskUserId}`, {roleId});
  }

  getCurrentDeskUser(desk?: Desk) {
    return desk?.deskUsers.find(el => el.user.id === this.userStorageService.getUserFromStorage().id);
  }

  exitFromDesk(deskId: number) {
    return this.httpService.delete(`DesksUsers/Exit/${deskId}`);
  }
}
