import { Component, OnInit } from '@angular/core';
import {FormControl, ValidationErrors, Validators} from '@angular/forms';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {

  constructor(private userService : UserService) {  }

  ngOnInit(): void {
  }
  hide = true;
  email = new FormControl('', [Validators.required, Validators.email]);
  password = new FormControl('', [Validators.required]);

  getEmailErrorMessage() {
    if (this.email.hasError('required')) {
      return 'Поле не должно быть пустым!';
    }

    return this.email.hasError('email') ? 'Некорректный email' : '';
  }
  getRequiredErrorMessage() {
    return 'Поле не должно быть пустым!';

  }
  makeLogin() {
    /*console.log(localStorage.getItem("token"));
    let user  = this.userService.getUsers().find(el => el.email === this.email.value);
    if (user != undefined && user.password === this.password.value) {
      console.log("Logged in! Hello, " + user.name + " " + user.surname);
    }
    else {
      console.log("Error!");
    }*/
    this.userService.loginUser(this.email.value, this.password.value);
  }
}
