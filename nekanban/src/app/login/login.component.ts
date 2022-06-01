import { Component, OnInit } from '@angular/core';
import {FormControl, ValidationErrors, Validators} from '@angular/forms';
import { UserService } from '../services/user.service';
import {BehaviorSubject, mergeMap, of} from "rxjs";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  busy = new BehaviorSubject(false);
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
    this.busy.next(true);
    this.userService.loginUser(this.email.value, this.password.value).subscribe({
      next: () => this.busy.next(false),
      error: () => this.busy.next(false)
    });
  }
}
