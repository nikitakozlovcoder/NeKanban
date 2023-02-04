import {Injectable} from "@angular/core";
import { environment } from "../../environments/environment";
import {HttpClient, HttpHeaders, HttpParams} from "@angular/common/http";
import {UserService} from "./user.service";
import {UserStorageService} from "./userStorage.service";

@Injectable()
export class AppHttpService {
  private readonly baseUrl = environment.baseUrl;
  constructor(private readonly http: HttpClient, private readonly userStorageService: UserStorageService) {}

  post<T>(action: string, body: {}|null) {
    return this.http.post<T>(`${this.baseUrl}${action}`, body, this.getHttpHeaders());
  }

  get<T>(action: string, query: HttpParams|undefined = undefined) {
    return this.http.get<T>(`${this.baseUrl}${action}`, {...this.getHttpHeaders(), params: query});
  }

  put<T>(action: string, body: {}|null) {
    return this.http.put<T>(`${this.baseUrl}${action}`, body, this.getHttpHeaders());
  }

  delete<T>(action: string, body: {}|null = null) {
    return this.http.delete<T>(`${this.baseUrl}${action}`, {...this.getHttpHeaders(), body: body});
  }

  private getHttpHeaders() {
    return {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + this.userStorageService.getToken()
      })
    };
  }
}
