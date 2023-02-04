import {Injectable} from "@angular/core";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {AppHttpService} from "./app-http.service";
import {Comment} from "../models/comment";
import {Column} from "../models/column";

@Injectable()
export class CommentsService {
  constructor(private httpService: AppHttpService) { }

  getComments(toDoId: number) {
    return this.httpService.get<Comment[]>("Comments/GetComments/" + toDoId);
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
}
