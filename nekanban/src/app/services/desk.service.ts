import {Injectable} from "@angular/core";
import {HttpClient, HttpHeaders, HttpParams} from "@angular/common/http";
import {AppHttpService} from "./app-http.service";

import {Desk} from "../models/desk";
import {InviteLinkAction} from "../constants/InviteLinkAction";

@Injectable()
export class DeskService {

  constructor(private httpService: AppHttpService) {
  }

  getDesks() {
    return this.httpService.get<Desk[]>("Desks/GetForUser");
  }

  addDesk(name: string) {
    return this.httpService.post<Desk>("Desks/CreateDesk", {name});
  }

  addPreference(id: number, preference: number) {
    return this.httpService.put<Desk[]>("DesksUsers/SetPreferenceType/" + id, {preference});
  }

  updateDesk(id: number, name: string) {
    return this.httpService.put<Desk>("Desks/UpdateDesk/" + id, {name});
  }

  getDesk(id: number) {
    return this.httpService.get<Desk>("Desks/GetDesk/" + id);
  }

  setLink(deskId: number) {
    return this.httpService.put<Desk>("Desks/InviteLink/" + deskId, {action: InviteLinkAction.Generate});
  }

  removeLink(deskId: number) {
    return this.httpService.put<Desk>("Desks/InviteLink/" + deskId, {action: InviteLinkAction.Remove});
  }

  inviteByLink(guid: number) {
    return this.httpService.put<Desk>("DesksUsers/AddUserByLink/", {uid: guid});
  }

  removeUserFromDesk(usersId: number[], deskId: number) {
    return this.httpService.put<Desk>("DesksUsers/RemoveUsers/" + deskId, {usersToRemove: usersId});
  }

  removeDesk(deskId: number) {
    return this.httpService.delete("Desks/Delete?id=" + deskId);
  }
}

