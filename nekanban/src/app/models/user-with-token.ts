import {User} from "./user";
import {Token} from "./token";

export class UserWithToken extends User {
  token: Token;
  constructor(name: string, surname: string, email: string, password: string, id: number, token: Token) {
    super(name, surname, email, password, id);
    this.token = token;
  }
}
