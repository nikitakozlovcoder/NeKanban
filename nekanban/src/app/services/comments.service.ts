import {Injectable} from "@angular/core";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {BaseHttpService} from "./baseHttp.service";
import {Comment} from "../models/comment";
import {Column} from "../models/column";

@Injectable()
export class CommentsService {
  constructor(private http: HttpClient, private httpService: BaseHttpService) { }

  getComments(toDoId: number) {
    let httpOptions = this.getHttpHeaders();
    return this.http.get<Comment[]>(this.httpService.baseUrl + "Comments/GetComments/" + toDoId, httpOptions);
  }
  createComment(toDoId: number, body: string) {
    let httpOptions = this.getHttpHeaders();
    const requestBody = {body: body};
    return this.http.post<Comment[]>(this.httpService.baseUrl + "Comments/Create/" + toDoId, requestBody, httpOptions);
  }
  updateComment(commentId: number, body: string) {
    let httpOptions = this.getHttpHeaders();
    const requestBody = {body: body};
    return this.http.put<Comment[]>(this.httpService.baseUrl + "Comments/Update/" + commentId, requestBody, httpOptions);
  }
  deleteOwnComment(commentId: number) {
    let httpOptions = this.getHttpHeaders();
    return this.http.delete<Comment[]>(this.httpService.baseUrl + "Comments/DeleteOwn/" + commentId, httpOptions);
  }
  deleteComment(commentId: number) {
    let httpOptions = this.getHttpHeaders();
    return this.http.delete<Comment[]>(this.httpService.baseUrl + "Comments/Delete/" + commentId, httpOptions);
  }
  private getHttpHeaders() {
    return {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    };
  }
}
