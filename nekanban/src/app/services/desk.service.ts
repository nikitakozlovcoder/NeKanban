import {Injectable} from "@angular/core";
import {HttpClient, HttpHeaders, HttpParams} from "@angular/common/http";
import {ActivatedRoute, Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {BaseHttpService} from "./base_http.service";
import {DeskUsers} from "../models/deskusers";
import {Desk} from "../models/desk";

@Injectable()
export class DeskService {

  name ='';
  constructor(private http: HttpClient, private route: ActivatedRoute, private router: Router, public dialog: MatDialog,
              private http_service: BaseHttpService) { }
  getDesks() {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    console.log(httpOptions.headers.get('Authorization'));
    //let desks: Desk[] = [];
    return this.http.get<Desk[]>(this.http_service.base_url + "Desks/GetForUser", httpOptions);
    //console.log(desks);
    //return desks;
  }
  addDesk(name: string) {
    const body = {name: name};
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    return this.http.post<Desk>(this.http_service.base_url + "Desks/CreateDesk", body, httpOptions);
  }
  addPreference(id: number, preference: number)  {
    const body = {preference: preference};
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    return this.http.put<Desk[]>(this.http_service.base_url + "DesksUsers/SetPreferenceType/" + id, body, httpOptions);
  }
  updateDesk(id: number, name: string) {
    const body = {name: name};
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    /*this.http.put(this.http_service.base_url + "Desks/UpdateDesk/" + id, body, httpOptions).subscribe({
      next: data => {

      },
      error: error => {
        console.error('There was an error!', error.message);
      }
    })*/
    return this.http.put<Desk>(this.http_service.base_url + "Desks/UpdateDesk/" + id, body, httpOptions);
  }
  getDesk(id: number) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    return this.http.get<Desk>(this.http_service.base_url + "Desks/GetDesk/" + id, httpOptions);
  }
}
