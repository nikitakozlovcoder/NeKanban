export class User {
  name: string;
  surname: string;
  email: string;
  password: string;
  constructor(name: string, surname: string, email: string, password: string) {
    this.name = name;
    this.email = email;
    this.surname = surname;
    this.password = password;
  }
}
