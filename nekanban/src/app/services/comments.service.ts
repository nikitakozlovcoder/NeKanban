import {Injectable} from "@angular/core";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {BaseHttpService} from "./baseHttp.service";
import {Comment} from "../models/comment";
import {Column} from "../models/column";

@Injectable()
export class CommentsService {
  constructor(private http: HttpClient, private httpService: BaseHttpService) { }

  getComments(toDoId: number) {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    return this.http.get<Comment[]>(this.httpService.baseUrl + "Comments/GetComments/" + toDoId, httpOptions);
  }
  createComment(toDoId: number, body: string) {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    const requestBody = {body: body};
    return this.http.post<Comment[]>(this.httpService.baseUrl + "Comments/Create/" + toDoId, requestBody, httpOptions);
  }
  updateComment(commentId: number, body: string) {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem("token")
      })
    }
    const requestBody = {body: body};
    return this.http.put<Comment[]>(this.httpService.baseUrl + "Comments/Update/" + commentId, requestBody, httpOptions);
  }
}
