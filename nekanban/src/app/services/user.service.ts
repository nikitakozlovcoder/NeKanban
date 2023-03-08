import {HttpErrorResponse} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {DialogComponent} from "../dialog/dialog.component";
import {AppHttpService} from "./app-http.service";
import {catchError, tap, throwError} from "rxjs";
import ErrorTypes from "../constants/ErrorTypes";
import {ErrorCodes} from "../constants/ErrorCodes";
import {UserStorageService} from "./userStorage.service";
import {UserWithToken} from "../models/user-with-token";
import {Token} from "../models/token";

@Injectable()
export class UserService {

  openDialog(err?: string): void {
    let errorType = ErrorTypes.Unknown;
    switch (err) {
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

  constructor(private router: Router,
              public dialog: MatDialog,
              private httpService: AppHttpService,
              private userStorageService: UserStorageService) { }

  addUser(name: string, surname: string, email: string, password: string, personalDataAgreement: boolean) {
    const body = {email, password, name, surname, personalDataAgreement};
    return this.httpService.post<UserWithToken>("Users/Register", body).pipe(tap(x => {
      this.userStorageService.addUserToStorage(x);
      this.router.navigate(['']).then();
    }), catchError((err : HttpErrorResponse) => {
      this.openDialog(err.error)
      return throwError(() => err);
    }))
  }

  loginUser(email: string, password: string) {
    const body = { email, password};
    return this.httpService.post<UserWithToken>("Users/Login", body).pipe(tap(x => {
      this.userStorageService.addUserToStorage(x);
      this.router.navigate(['']).then();
    }), catchError((err : HttpErrorResponse) => {
      this.openDialog(ErrorCodes.ValidationError)
      return throwError(() => err);
    }))
  }

  logoutUser() {
    this.userStorageService.removeUserFromStorage();
  }

  refreshToken() {
    const body = {refreshToken: this.userStorageService.getRefreshToken()};
    return this.httpService.post<Token>("Users/Refresh", body).pipe(tap(x => {
      this.userStorageService.setTokens(x);
    }))
  }
}
