import {Injectable} from "@angular/core";
import {AppHttpService} from "./app-http.service";
import {Todo} from "../models/todo";

@Injectable()
export class TodoService {
  constructor(private httpService: AppHttpService) {
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

  assignUser(todoId: number, deskUserId: number) {
    return this.httpService.put<Todo>(`ToDos/AssignUser/${todoId}`, {deskUserId});
  }

  removeUser(toDoUserId: number) {
    return this.httpService.delete<Todo>("ToDos/RemoveUser/", {toDoUserId});
  }

  updateToDo(todo: Todo) {
    return this.httpService.put<Todo>(`ToDos/UpdateToDo/${todo.id}`, {name: todo.name, body: todo.body});
  }

  moveToDo(todoId: number, columnId: number, order: number) {
    return this.httpService.put<Todo[]>(`ToDos/MoveToDo/${todoId}`, {columnId, order});
  }

  attachFile(todoId: number, formData: any) {
    return this.httpService.put<string>(`ToDos/AttachFile/${todoId}`, formData, {responseType: 'text' as const});
  }
}
