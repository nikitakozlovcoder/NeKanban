import {Injectable} from "@angular/core";
import {UserWithToken} from "../models/user-with-token";

@Injectable()
export class UserStorageService {
  addUserToStorage(user: UserWithToken) {
    localStorage.setItem("currentUser", JSON.stringify(user));
    localStorage.setItem("token", user.token.tokenValue);
  }

  getToken() {
    return localStorage.getItem("token");
  }

  removeUserFromStorage() {
    localStorage.removeItem("token");
    localStorage.removeItem("currentUser");
  }
}
