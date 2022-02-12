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
export class UserService {
    private users : User[] = [new User("Alex", "Alexov", "a@gmail.com", "123456789")]
    getUsers() : User[] {
        return this.users;
    }
    addUser(name: string, surname: string, email: string, password: string) {
        this.users.push(new User(name, surname, email, password));
    }
}