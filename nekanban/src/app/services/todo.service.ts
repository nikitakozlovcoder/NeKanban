import {Injectable} from "@angular/core";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {AppHttpService} from "./app-http.service";
import {Todo} from "../models/todo";

@Injectable()
export class TodoService {
  constructor(private httpService: AppHttpService) {
  }

  getToDos(deskId: number) {
    return this.httpService.get<Todo[]>("ToDos/GetToDos/" + deskId);
  }

  addToDo(deskId: number, name: string, body: string) {
    return this.httpService.post<Todo[]>("ToDos/CreateToDo/" + deskId, {name, body});
  }

  assignUser(todoId: number, deskUserId: number) {
    return this.httpService.put<Todo>("ToDos/AssignUser/" + todoId, {deskUserId});
  }

  removeUser(toDoUserId: number) {
    return this.httpService.delete<Todo>("ToDos/RemoveUser/", {toDoUserId});
  }

  updateToDo(todoId: number, name: string, body: string) {
    return this.httpService.put<Todo>("ToDos/UpdateToDo/" + todoId, {name, body});
  }

  moveToDo(todoId: number, columnId: number, order: number) {
    return this.httpService.put<Todo[]>("ToDos/MoveToDo/" + todoId, {columnId, order});
  }
}
