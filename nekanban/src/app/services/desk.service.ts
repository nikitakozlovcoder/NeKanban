import {Injectable} from "@angular/core";
import {HttpClient, HttpHeaders, HttpParams} from "@angular/common/http";
import {BaseHttpService} from "./baseHttp.service";

import {Desk} from "../models/desk";

@Injectable()
export class DeskService {

  constructor(private http: HttpClient, private httpService: BaseHttpService) {
  }

  getDesks() {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }

    return this.http.get<Desk[]>(this.httpService.baseUrl + "Desks/GetForUser", httpOptions);
  }

  addDesk(name: string) {
    const body = {name: name};
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    return this.http.post<Desk>(this.httpService.baseUrl + "Desks/CreateDesk", body, httpOptions);
  }

  addPreference(id: number, preference: number) {
    const body = {preference: preference};
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    return this.http.put<Desk[]>(this.httpService.baseUrl + "DesksUsers/SetPreferenceType/" + id, body, httpOptions);
  }

  updateDesk(id: number, name: string) {
    const body = {name: name};
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    /*this.http.put(this.httpService.base_url + "Desks/UpdateDesk/" + id, body, httpOptions).subscribe({
      next: data => {

      },
      error: error => {
        console.error('There was an error!', error.message);
      }
    })*/
    return this.http.put<Desk>(this.httpService.baseUrl + "Desks/UpdateDesk/" + id, body, httpOptions);
  }

  getDesk(id: number) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    return this.http.get<Desk>(this.httpService.baseUrl + "Desks/GetDesk/" + id, httpOptions);
  }

  setLink(deskId: number) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    const body = {action: 1};
    return this.http.put<Desk>(this.httpService.baseUrl + "Desks/InviteLink/" + deskId, body, httpOptions);
  }

  inviteByLink(guid: number) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    const body = {uid: guid};
    return this.http.put<Desk>(this.httpService.baseUrl + "DesksUsers/AddUserByLink/", body, httpOptions);
  }

  removeUserFromDesk(usersId: number[], deskId: number) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    const body = {usersToRemove: usersId};
    return this.http.put<Desk>(this.httpService.baseUrl + "DesksUsers/RemoveUsers/" + deskId, body, httpOptions);
  }

  removeDesk(deskId: number) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    return this.http.delete(this.httpService.baseUrl + "Desks/Delete?id=" + deskId, httpOptions);
  }

}

