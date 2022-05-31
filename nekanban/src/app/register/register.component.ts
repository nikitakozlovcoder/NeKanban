import { Component, OnInit } from '@angular/core';
import {FormControl, ValidationErrors, ValidatorFn, Validators} from '@angular/forms';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  constructor(private userService: UserService) { }

  ngOnInit(): void {
  }

  hide = true;
  hideConfirm = true;
  email = new FormControl('', [Validators.required, Validators.email]);
  name = new FormControl('', [Validators.required]);
  surname = new FormControl('', [Validators.required]);
  password = new FormControl('', [Validators.required, Validators.minLength(8)]);
  password_confirmation = new FormControl('', [Validators.required, this.passwordMatchValidator(this.password)]);
  getEmailErrorMessage() {
    if (this.email.hasError('required')) {
      return 'Поле не должно быть пустым!';
    }

    return this.email.hasError('email') ? 'Некорректный email' : '';
  }
  getRequiredErrorMessage() {
    return 'Поле не должно быть пустым!';

  }
  passwordMatchValidator (password: FormControl) : ValidatorFn {
    return (passwordConfirmation) => {
      if (password.value === passwordConfirmation.value)
        return null;
      else
        return {passwordMismatch: true};
    }

  };
  registerUser() {
    if (this.email.invalid || this.name.invalid || this.surname.invalid || this.password.invalid || this.password_confirmation.invalid) {
      console.log("Unable to register");
      this.name.markAsTouched();
      this.surname.markAsTouched();
      this.email.markAsTouched();
      this.password.markAsTouched();
      this.password_confirmation.markAsTouched();
    } else {
      this.userService.addUser(this.name.value, this.surname.value, this.email.value, this.password.value);
      console.log("success");
    }
  }
}
