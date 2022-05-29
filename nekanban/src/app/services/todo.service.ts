import {Injectable} from "@angular/core";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {ActivatedRoute, Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {BaseHttpService} from "./base_http.service";
import {Todo} from "../models/todo";

@Injectable()
export class TodoService {
  constructor(private http: HttpClient, private http_service: BaseHttpService) {
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
  assignUser(todoId: number, deskUserId: number) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    const requestBody = {
      descUserId: deskUserId
    };
    return this.http.put<Todo>(this.http_service.base_url + "ToDos/AssignUser/" + todoId, requestBody, httpOptions);
  }
  removeUser(todoUserId: number) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      }),
      body: {
        toDoUserId: todoUserId
      }
    }
    /*const requestBody = {
      toDoUserId: todoUserId
    };*/
    return this.http.delete<Todo>(this.http_service.base_url + "ToDos/RemoveUser/", httpOptions);
  }
  updateToDo(todoId: number, name: string, body: string) {
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
    return this.http.put<Todo>(this.http_service.base_url + "ToDos/UpdateToDo/" + todoId, requestBody, httpOptions);
  }
  moveToDo(todoId: number, columnId: number, order: number) {
    console.log("Moving TODO");
    console.log(todoId);
    console.log("in column");
    console.log(columnId);
    console.log("To");
    console.log(order);
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    const requestBody = {
      columnId: columnId,
      order: order
    };
    return this.http.put<Todo[]>(this.http_service.base_url + "ToDos/MoveToDo/" + todoId, requestBody, httpOptions);
  }
}
