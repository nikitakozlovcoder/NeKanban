import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor, HttpErrorResponse
} from '@angular/common/http';
import {BehaviorSubject, catchError, filter, Observable, switchMap, take, throwError} from 'rxjs';
import {UserStorageService} from "../../services/userStorage.service";
import {UserService} from "../../services/user.service";
import {Router} from "@angular/router";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private userService: UserService,
              private userStorageService: UserStorageService,
              private router: Router) {}

  private isTokenRefreshing = false;
  private refreshTokenSubject = new BehaviorSubject<any>(null);

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (!this.UrlRequiresAuth(request)) {
      return next.handle(request);
    }
    return next.handle(this.addAuthHeaders(request)).pipe(
      catchError((requestError: HttpErrorResponse) => {
        if (requestError && requestError.status === 401) {
          if (!this.isTokenRefreshing) {
            this.isTokenRefreshing = true;
            this.refreshTokenSubject.next(null);
            return this.userService.refreshToken().pipe(
              catchError(() => this.router.navigate(['authorization'])),
              switchMap(() => {
                this.isTokenRefreshing = false;
                this.refreshTokenSubject.next(1);
                return next.handle(this.addAuthHeaders(request));
              })
            )
          }
          else {
            return this.refreshTokenSubject.pipe(
              filter(token => (token != null)),
              take(1),
              switchMap(() => {
                return next.handle(this.addAuthHeaders(request))
              })
            );
          }
        }
        return throwError(() => requestError);
      })
    )
  }

  private addAuthHeaders(request: HttpRequest<any>) {
    return request.clone({
      headers: request.headers.set('Authorization', 'Bearer ' + this.userStorageService.getAccessToken()),
    })
  }

  private UrlRequiresAuth(request: HttpRequest<any>) {
    return !(request.url.includes('/Users/LogIn') || request.url.includes('/Users/Refresh'));
  }
}
