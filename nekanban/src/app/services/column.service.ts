﻿import {Injectable} from "@angular/core";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {BaseHttpService} from "./baseHttp.service";
import {Column} from "../models/column";

@Injectable()
export class ColumnService {
  constructor(private http: HttpClient, private httpService: BaseHttpService) { }
  getColumns(deskId: number) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    return this.http.get<Column[]>(this.httpService.baseUrl + "Columns/GetColumns/" + deskId, httpOptions);
  }
  addColumn(deskId: number, name: string) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    const body = {name: name};
    return this.http.post<Column[]>(this.httpService.baseUrl + "Columns/CreateColumn/" + deskId, body, httpOptions);
  }
  removeColumn(columnId: number) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    return this.http.delete<Column[]>(this.httpService.baseUrl + "Columns/DeleteColumn/" + columnId, httpOptions);
  }
  moveColumn(columnId: number, position: number) {

    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    const body = {position: position};
    return this.http.put<Column[]>(this.httpService.baseUrl + "Columns/MoveColumn/" + columnId, body, httpOptions);
  }
  updateColumn(columnId: number, name: string) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    const body = {name: name};
    return this.http.put<Column[]>(this.httpService.baseUrl + "Columns/UpdateColumn/" + columnId, body, httpOptions);
  }
}
