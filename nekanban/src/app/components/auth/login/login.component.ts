import { Component, OnInit } from '@angular/core';
import {FormControl, FormGroup} from '@angular/forms';
import {UserService} from "../../../services/user.service";
import {BehaviorSubject} from "rxjs";

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
    email: new FormControl<string>(''),
    password: new FormControl<string>('')
  });

  makeLogin() {
    this.busy.next(true);
    this.userService.loginUser(this.loginFormGroup.value.email!, this.loginFormGroup.value.password!).subscribe({
      next: () => this.busy.next(false),
      error: () => this.busy.next(false)
    });
  }
}
