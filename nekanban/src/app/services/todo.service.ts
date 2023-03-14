import {Injectable} from "@angular/core";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {AppHttpService} from "./app-http.service";
import {Todo} from "../models/todo";
import {environment} from "../../environments/environment";

@Injectable()
export class TodoService {
  constructor(private httpService: AppHttpService, private testHttp: HttpClient) {
  }

  getToDos(deskId: number) {
    return this.httpService.get<Todo[]>(`ToDos/GetToDos/${deskId}`);
  }

  getToDo(todoId: number) {
    return this.httpService.get<Todo>(`ToDos/GetTodo/${todoId}`);
  }

  getDraft(deskId: number) {
    return this.httpService.post<Todo>(`ToDos/GetDraft/${deskId}`, {});
  }

  updateDraft(todo: Todo) {
    return this.httpService.put<Todo>(`ToDos/UpdateDraft/${todo.id}`, {name: todo.name, body: todo.body});
  }

  applyDraft(todoId: number) {
    return this.httpService.put<Todo>(`ToDos/ApplyDraft/${todoId}`, {});
  }

  addToDo(deskId: number, todo: Todo) {
    return this.httpService.post<Todo[]>("ToDos/CreateToDo/" + deskId, {name: todo.name, body: todo.body});
  }

  assignUser(todoId: number, deskUserId: number) {
    return this.httpService.put<Todo>("ToDos/AssignUser/" + todoId, {deskUserId});
  }

  removeUser(toDoUserId: number) {
    return this.httpService.delete<Todo>("ToDos/RemoveUser/", {toDoUserId});
  }

  updateToDo(todo: Todo) {
    return this.httpService.put<Todo>("ToDos/UpdateToDo/" + todo.id, {name: todo.name, body: todo.body});
  }

  moveToDo(todoId: number, columnId: number, order: number) {
    return this.httpService.put<Todo[]>("ToDos/MoveToDo/" + todoId, {columnId, order});
  }

  attachFile(todoId: number, formData: any) {
    let headers = new HttpHeaders();
    headers.append('Content-Type', 'multipart/form-data');
    headers.append('Accept', 'application/json');
    let options = { headers: headers, responseType: 'text' as const };
    return this.testHttp.put(environment.baseUrl + `ToDos/AttachFile/${todoId}`, formData, options);
  }
}
