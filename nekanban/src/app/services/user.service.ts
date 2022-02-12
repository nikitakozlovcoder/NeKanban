import {HttpClient} from "@angular/common/http";
import {Injectable} from "@angular/core";

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
@Injectable()
export class UserService {
  name = "";
  id = 0;
  base_url = "https://localhost:7146/";
  constructor(private http: HttpClient) { }
    private users : User[] = [new User("Alex", "Alexov", "a@gmail.com", "123456789")]
    getUsers() : User[] {
        return this.users;
    }
    addUser(name: string, surname: string, email: string, password: string) {
      const body = {"email": email, "password": password, "name": name, "surname": surname};
      const test_body = {title: "foo", body: "bar", userId: 1};
      console.log(body);
      /*this.http.post<any>("https://jsonplaceholder.typicode.com/posts", test_body).subscribe({
        next: data => {
          this.name = data.title;
          this.id = data.id;
          console.log(this);
        },
        error: error => {
          console.error('There was an error!', error.message);
        }
      })*/
      this.http.post<any>(this.base_url + "Users/Register", body).subscribe({
        next: data => {
          this.name = data.name;
          console.log(this);
        },
        error: error => {
          console.error('There was an error!', error.message);
        }
      })
        this.users.push(new User(name, surname, email, password));
    }
}
