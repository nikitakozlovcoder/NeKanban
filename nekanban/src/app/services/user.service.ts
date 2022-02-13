import {HttpClient} from "@angular/common/http";
import {Injectable} from "@angular/core";
import { User } from "../models/user";
import {ActivatedRoute, Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {LoginComponent} from "../login/login.component";
import {DialogComponent} from "../dialog/dialog.component";
import {BaseHttpService} from "./base_http.service";


@Injectable()
export class UserService {
  name = "";
  id = 0;

  openDialog(): void {
    const dialogRef = this.dialog.open(DialogComponent, {
      width: '250px',
    });

  }
  constructor(private http: HttpClient, private route: ActivatedRoute, private router: Router, public dialog: MatDialog,
              private http_service: BaseHttpService) { }
    addUser(name: string, surname: string, email: string, password: string) {
      const body = {"email": email, "password": password, "name": name, "surname": surname};
      this.http.post<any>(this.http_service.base_url + "Users/Register", body).subscribe({
        next: data => {
          localStorage.setItem("token", data.token.tokenValue);
        },
        error: error => {
          console.error('There was an error!', error.message);
        }
      })
    }
    loginUser(email: string, password: string) {
      const body = {email: email, password: password};
      this.http.post<any>(this.http_service.base_url + "Users/Login", body).subscribe( {
        next: data => {
          localStorage.setItem("token", data.token.tokenValue);
          this.router.navigate(['']);
        },
        error: error => {
          this.openDialog();
          console.error('There was an error!', error.message);
        }
      })
    }
    logoutUser() {
      localStorage.removeItem("token");
    }
}
