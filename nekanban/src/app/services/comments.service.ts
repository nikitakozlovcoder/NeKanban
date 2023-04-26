import {Injectable} from "@angular/core";
import {AppHttpService} from "./app-http.service";
import {Comment} from "../models/comment";
import {HttpEvent} from "@angular/common/http";
import {map, mergeMap, Observable, toArray} from "rxjs";

@Injectable()
export class CommentsService {
  constructor(private httpService: AppHttpService) { }

  getComments(toDoId: number) {
    return this.mapComments(this.httpService.get<Comment[]>("Comments/GetComments/" + toDoId));
  }

  getDraft(toDoId: number) {
    return this.httpService.post<Comment>(`Comments/GetDraft/${toDoId}`, {});
  }

  applyDraft(commentId: number) {
    return this.mapComments(this.httpService.put<Comment[]>(`Comments/ApplyDraft/${commentId}`, {}));
  }

  updateDraft(commentId: number, body: string) {
    return this.httpService.put<Comment>(`Comments/UpdateDraft/${commentId}`, {body});
  }

  updateComment(commentId: number, body: string) {
    return this.mapComments(this.httpService.put<Comment[]>( `Comments/Update/${commentId}`, {body}));
  }

  deleteOwnComment(commentId: number) {
    return this.mapComments(this.httpService.delete<Comment[]>(`Comments/DeleteOwn/${commentId}`));
  }

  deleteComment(commentId: number) {
    return this.mapComments(this.httpService.delete<Comment[]>(`Comments/Delete/${commentId}`));
  }

  attachFile(commentId: number, formData: any) {
    return this.httpService.put<HttpEvent<string>>(`Comments/AttachFile/${commentId}`, formData,
      {responseType: 'text' as const,
        reportProgress: true,
        observe: "events"});
  }

  private mapComments(comments: Observable<Comment[]>) {
    return comments.pipe(
      mergeMap(data => data),
      map(({id, body, deskUser, createdAtUtc}) => {
        return <Comment>{
          id: id,
          body: body,
          deskUser: deskUser,
          createdAtUtc: new Date(createdAtUtc)};
      }),
      toArray())
  }
}
