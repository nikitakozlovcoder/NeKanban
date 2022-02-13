export class User {
  id: number;
  name: string;
  surname: string;
  email: string;
  password: string;
  constructor(name: string, surname: string, email: string, password: string, id: number) {
    this.name = name;
    this.email = email;
    this.surname = surname;
    this.password = password;
    this.id = id;
  }
}
