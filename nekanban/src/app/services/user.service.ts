import {HttpErrorResponse} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {Router} from "@angular/router";
import {AppHttpService} from "./app-http.service";
import {catchError, tap, throwError} from "rxjs";
import {ErrorCodes} from "../constants/ErrorCodes";
import {UserStorageService} from "./userStorage.service";
import {UserWithToken} from "../models/user-with-token";
import {Token} from "../models/token";
import {DialogService} from "./dialog.service";

@Injectable()
export class UserService {

  constructor(private readonly router: Router,
              private readonly dialogService: DialogService,
              private readonly httpService: AppHttpService,
              private userStorageService: UserStorageService) { }

  addUser(name: string, surname: string, email: string, password: string, personalDataAgreement: boolean) {
    const body = {email, password, name, surname, personalDataAgreement};
    return this.httpService.post<UserWithToken>("Users/Register", body).pipe(tap(x => {
      this.userStorageService.addUserToStorage(x);
      this.router.navigate(['']).then();
    }), catchError((err : HttpErrorResponse) => {
      this.dialogService.openToast(err.error)
      return throwError(() => err);
    }))
  }

  loginUser(email: string, password: string) {
    const body = { email, password};
    return this.httpService.post<UserWithToken>("Users/Login", body).pipe(tap(x => {
      this.userStorageService.addUserToStorage(x);
      this.router.navigate(['']).then();
    }), catchError((err : HttpErrorResponse) => {
      this.dialogService.openToast(ErrorCodes.ValidationError)
      return throwError(() => err);
    }))
  }

  logoutUser() {
    const body = {refreshToken: this.userStorageService.getRefreshToken()};
    return this.httpService.post(`Users/Logout`, body).pipe(tap( () => {
      this.userStorageService.removeUserFromStorage();
    }));
  }

  refreshToken() {
    const body = {refreshToken: this.userStorageService.getRefreshToken()};
    return this.httpService.post<Token>("Users/Refresh", body).pipe(tap(x => {
      this.userStorageService.setTokens(x);
    }))
  }
}
