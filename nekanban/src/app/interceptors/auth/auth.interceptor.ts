import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor, HttpErrorResponse
} from '@angular/common/http';
import { BehaviorSubject, catchError, filter, Observable, switchMap, take, tap, throwError } from 'rxjs';
import {UserStorageService} from "../../services/userStorage.service";
import {UserService} from "../../services/user.service";
import {Router} from "@angular/router";
import { environment } from "../../../environments/environment";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  private readonly isTokenRefreshing = new BehaviorSubject<boolean | undefined>(false);
  constructor(private readonly userService: UserService,
              private readonly userStorageService: UserStorageService,
              private readonly router: Router) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (!this.urlRequiresAuth(request)) {
      return next.handle(request);
    }

    return next.handle(this.addAuthHeaders(request)).pipe(
      catchError((requestError: HttpErrorResponse) => {
        if (!requestError || requestError.status !== 401) {
          return throwError(() => requestError);
        }

        if (!this.isTokenRefreshing.value) {
          this.isTokenRefreshing.next(true);
          this.userService.refreshToken().subscribe({
            next: () => this.isTokenRefreshing.next(false),
            error: () => {
              this.isTokenRefreshing.next(undefined);
              this.router.navigate(['authorization']).then()
            }
          });
        }

        return this.isTokenRefreshing.pipe(
          filter(x => !x),
          take(1),
          switchMap((x) => {
            if (x === undefined){
              return throwError(() => new Error());
            }

            return next.handle(this.addAuthHeaders(request))
          })
        );
      })
    )
  }

  private addAuthHeaders(request: HttpRequest<any>) {
    return request.clone({
      headers: request.headers.set('Authorization', 'Bearer ' + this.userStorageService.getAccessToken()),
    })
  }

  private urlRequiresAuth(request: HttpRequest<any>) {
    return !environment.urlsToDisableAuthInterceptor.some(x => request.url.includes(x));
  }
}
