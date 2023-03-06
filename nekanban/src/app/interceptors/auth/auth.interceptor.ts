import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor, HttpErrorResponse
} from '@angular/common/http';
import {catchError, Observable, switchMap, throwError} from 'rxjs';
import {UserStorageService} from "../../services/userStorage.service";
import {UserService} from "../../services/user.service";
import {Router} from "@angular/router";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private userService: UserService,
              private userStorageService: UserStorageService,
              private router: Router) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(this.addAuthHeaders(request)).pipe(
      catchError((requestError: HttpErrorResponse) => {
        if (requestError && requestError.status === 401) {
          return this.userService.refreshToken().pipe(
            catchError(() => this.router.navigate(['authorization'])),
            switchMap(() => {
              return next.handle(this.addAuthHeaders(request));
            })
          )
        }
        return throwError(() => requestError);
      })
    )
  }

  addAuthHeaders(request: HttpRequest<any>) {
    return request.clone({
      headers: request.headers.set('Authorization', 'Bearer ' + this.userStorageService.getAccessToken()),
    })
  }
}
