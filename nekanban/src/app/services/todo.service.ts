import {Injectable} from "@angular/core";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {BaseHttpService} from "./baseHttp.service";
import {Todo} from "../models/todo";

@Injectable()
export class TodoService {
  constructor(private http: HttpClient, private httpService: BaseHttpService) {
  }
  getToDos(deskId: number) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    return this.http.get<Todo[]>(this.httpService.baseUrl + "ToDos/GetToDos/" + deskId, httpOptions);
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
    return this.http.post<Todo[]>(this.httpService.baseUrl + "ToDos/CreateToDo/" + deskId, requestBody, httpOptions);
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
    return this.http.put<Todo>(this.httpService.baseUrl + "ToDos/AssignUser/" + todoId, requestBody, httpOptions);
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
    return this.http.delete<Todo>(this.httpService.baseUrl + "ToDos/RemoveUser/", httpOptions);
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
    return this.http.put<Todo>(this.httpService.baseUrl + "ToDos/UpdateToDo/" + todoId, requestBody, httpOptions);
  }
  moveToDo(todoId: number, columnId: number, order: number) {
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
    return this.http.put<Todo[]>(this.httpService.baseUrl + "ToDos/MoveToDo/" + todoId, requestBody, httpOptions);
  }
}
