import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {DialogComponent} from "../dialog/dialog.component";
import {BaseHttpService} from "./base_http.service";
import {catchError, tap, throwError} from "rxjs";
import ErrorTypes from "../constants/ErrorTypes";
import {ErrorCodes} from "../constants/ErrorCodes";

@Injectable()
export class UserService {

  openDialog(err?: string): void {
    let errorType = ErrorTypes.Unknown;
    switch (err){
      case ErrorCodes.DuplicateEmail:
        errorType = ErrorTypes.DuplicateEmail;
        break;
      case ErrorCodes.ValidationError:
        errorType = ErrorTypes.ValidationError;
        break;
    }

    this.dialog.open(DialogComponent, {
      width: '250px',
      data: {errorType: errorType}
    });
  }

  constructor(private http: HttpClient, private router: Router, public dialog: MatDialog,
              private http_service: BaseHttpService) { }

    addUser(name: string, surname: string, email: string, password: string) {
      const body = {"email": email, "password": password, "name": name, "surname": surname};
      return this.http.post<any>(this.http_service.base_url + "Users/Register", body).pipe(tap(x => {
        localStorage.setItem("currentUser", JSON.stringify(x));
        localStorage.setItem("token", x.token.tokenValue);
        this.router.navigate(['']).then();
      }), catchError((err : HttpErrorResponse) => {
        this.openDialog(err.error)
        return throwError(() => err);
      }))
    }

    loginUser(email: string, password: string) {
      const body = {email: email, password: password};
      return this.http.post<any>(this.http_service.base_url + "Users/Login", body).pipe(tap(x => {
        localStorage.setItem("currentUser", JSON.stringify(x));
        localStorage.setItem("token", x.token.tokenValue);
        this.router.navigate(['']).then();
      }), catchError((err : HttpErrorResponse) => {
        this.openDialog(ErrorCodes.ValidationError)
        return throwError(() => err);
      }))
    }

    logoutUser() {
      localStorage.removeItem("token");
      localStorage.removeItem("currentUser");
    }
}
