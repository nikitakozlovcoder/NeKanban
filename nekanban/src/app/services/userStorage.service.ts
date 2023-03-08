import {Injectable} from "@angular/core";
import {UserWithToken} from "../models/user-with-token";
import {Token} from "../models/token";

@Injectable()
export class UserStorageService {
  addUserToStorage(user: UserWithToken) {
    localStorage.setItem("currentUser", JSON.stringify(user));
    this.setTokens(user.token);
  }

  setTokens(token: Token) {
    localStorage.setItem("accessToken", token.accessToken);
    localStorage.setItem("refreshToken", token.refreshToken);
  }

  getAccessToken() {
    return localStorage.getItem("accessToken");
  }

  getRefreshToken() {
    return localStorage.getItem("refreshToken");
  }

  removeUserFromStorage() {
    localStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
    localStorage.removeItem("currentUser");
  }
}
