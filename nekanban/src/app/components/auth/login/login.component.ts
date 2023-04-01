import { Component, OnInit } from '@angular/core';
import {FormControl, FormGroup, UntypedFormControl, ValidationErrors, Validators} from '@angular/forms';
import {UserService} from "../../../services/user.service";
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
  loginFormGroup = new FormGroup({
    email: new FormControl<string>('', [Validators.required, Validators.email]),
    password: new FormControl<string>('', [Validators.required])
  });


  getEmailErrorMessage() {
    if (this.loginFormGroup.controls.email.hasError('required')) {
      return 'Поле не должно быть пустым!';
    }

    return this.loginFormGroup.controls.email.hasError('email') ? 'Некорректный email' : '';
  }

  makeLogin() {
    this.busy.next(true);
    this.userService.loginUser(this.loginFormGroup.value.email!, this.loginFormGroup.value.password!).subscribe({
      next: () => this.busy.next(false),
      error: () => this.busy.next(false)
    });
  }
}
