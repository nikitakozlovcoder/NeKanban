import {Injectable} from "@angular/core";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {ActivatedRoute, Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {BaseHttpService} from "./base_http.service";
import {Todo} from "../models/todo";

@Injectable()
export class TodoService {
  constructor(private http: HttpClient, private route: ActivatedRoute, private router: Router, public dialog: MatDialog,
              private http_service: BaseHttpService) {
  }
  getToDos(deskId: number) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    return this.http.get<Todo[]>(this.http_service.base_url + "ToDos/GetToDos/" + deskId, httpOptions);
  }
  addToDo(deskId: number, name: string, body: string) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    const requestBody = {
      name: name,
      body: body
    };
    return this.http.post<Todo[]>(this.http_service.base_url + "ToDos/CreateToDo/" + deskId, requestBody, httpOptions);
  }
}
