import {User} from "./user";
import {Token} from "./token";

export interface UserWithToken extends User {
  token: Token;
}
