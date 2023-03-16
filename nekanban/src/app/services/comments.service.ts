import {Injectable} from "@angular/core";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {AppHttpService} from "./app-http.service";
import {Comment} from "../models/comment";
import {Column} from "../models/column";
import {environment} from "../../environments/environment";

@Injectable()
export class CommentsService {
  constructor(private httpService: AppHttpService, private testHttp: HttpClient) { }

  getComments(toDoId: number) {
    return this.httpService.get<Comment[]>("Comments/GetComments/" + toDoId);
  }

  getDraft(toDoId: number) {
    return this.httpService.post<Comment>(`Comments/GetDraft/${toDoId}`, {});
  }

  applyDraft(commentId: number) {
    return this.httpService.put<Comment[]>(`Comments/ApplyDraft/${commentId}`, {});
  }

  updateDraft(commentId: number, body: string) {
    return this.httpService.put<Comment>(`Comments/UpdateDraft/${commentId}`, {body});
  }

  createComment(toDoId: number, body: string) {
    return this.httpService.post<Comment[]>("Comments/Create/" + toDoId, {body});
  }

  updateComment(commentId: number, body: string) {
    return this.httpService.put<Comment[]>( "Comments/Update/" + commentId, {body});
  }

  deleteOwnComment(commentId: number) {
    return this.httpService.delete<Comment[]>("Comments/DeleteOwn/" + commentId);
  }

  deleteComment(commentId: number) {
    return this.httpService.delete<Comment[]>("Comments/Delete/" + commentId);
  }

  attachFile(commentId: number, formData: any) {
    return this.testHttp.put(environment.baseUrl + `Comments/AttachFile/${commentId}`, formData, {responseType: 'text' as const});
  }
}
