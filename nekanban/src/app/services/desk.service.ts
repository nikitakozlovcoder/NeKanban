﻿import {Injectable} from "@angular/core";
import {HttpClient, HttpHeaders} from "@angular/common/http";
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
  getDesks() : Desk[] {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    console.log(httpOptions.headers.get('Authorization'));
    let desks: Desk[] = [];
    this.http.get<Desk[]>(this.http_service.base_url + "Desks/GetForUser", httpOptions).subscribe({
      next: (data: Desk[]) => {
        /*console.log(data);
        data.forEach(element => {
          console.log(element);
        })*/
        //desks = data;
        data.forEach(el => desks.push(Object.assign({}, el)));
      },
      error: error => {
        console.error('There was an error!', error.message);
      }
    })
    //console.log(desks);
    return desks;
  }
  addDesk(name: string) {
    const body = {name: name};
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    this.http.post(this.http_service.base_url + "Desks/CreateDesk", body, httpOptions).subscribe({
      next: data => {

      },
      error: error => {
        console.error('There was an error!', error.message);
      }
    })
  }
}