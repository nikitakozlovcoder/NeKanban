import { Component, OnInit } from '@angular/core';
import {FormGroup, UntypedFormControl, ValidationErrors, ValidatorFn, Validators} from '@angular/forms';
import { UserService } from '../../../services/user.service';
import {BehaviorSubject} from "rxjs";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  busy = new BehaviorSubject(false);
  constructor(private userService: UserService) { }

  ngOnInit(): void {
  }

  hide = true;
  hideConfirm = true;

  email = new UntypedFormControl('', [Validators.required, Validators.email]);
  name = new UntypedFormControl('', [Validators.required]);
  surname = new UntypedFormControl('', [Validators.required]);
  password = new UntypedFormControl('', [Validators.required, Validators.minLength(8)]);
  password_confirmation = new UntypedFormControl('', [Validators.required, this.passwordMatchValidator(this.password)]);
  userAgreement = new UntypedFormControl(false, [Validators.requiredTrue]);


  getEmailErrorMessage() {
    if (this.email.hasError('required')) {
      return 'Поле не должно быть пустым!';
    }

    return this.email.hasError('email') ? 'Некорректный email' : '';
  }
  passwordMatchValidator (password: UntypedFormControl) : ValidatorFn {
    return (passwordConfirmation) => {
      if (password.value === passwordConfirmation.value)
        return null;
      else
        return {passwordMismatch: true};
    }
  };

  registerUser() {
    if (this.email.invalid || this.name.invalid || this.surname.invalid || this.password.invalid ||
      this.password_confirmation.invalid || this.userAgreement.invalid) {
      this.name.markAsTouched();
      this.surname.markAsTouched();
      this.email.markAsTouched();
      this.password.markAsTouched();
      this.password_confirmation.markAsTouched();
      this.userAgreement.markAsTouched();
    } else {
      this.busy.next(true);
      this.userService.addUser(this.name.value, this.surname.value, this.email.value, this.password.value, this.userAgreement.value).subscribe({
        next: () => this.busy.next(false),
        error: () => this.busy.next(false)
      });
    }
  }
}
