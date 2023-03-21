import {Injectable} from "@angular/core";
import {AppHttpService} from "./app-http.service";
import {Comment} from "../models/comment";

@Injectable()
export class CommentsService {
  constructor(private httpService: AppHttpService) { }

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

  updateComment(commentId: number, body: string) {
    return this.httpService.put<Comment[]>( `Comments/Update/${commentId}`, {body});
  }

  deleteOwnComment(commentId: number) {
    return this.httpService.delete<Comment[]>(`Comments/DeleteOwn/${commentId}`);
  }

  deleteComment(commentId: number) {
    return this.httpService.delete<Comment[]>(`Comments/Delete/${commentId}`);
  }

  attachFile(commentId: number, formData: any) {
    return this.httpService.put<string>(`Comments/AttachFile/${commentId}`, formData, {responseType: 'text' as const});
  }
}
