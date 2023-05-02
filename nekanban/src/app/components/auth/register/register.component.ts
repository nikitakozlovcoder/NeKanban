import { Component, OnInit } from '@angular/core';
import {FormControl, FormGroup, UntypedFormControl, ValidatorFn, Validators} from '@angular/forms';
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
    this.registerFormGroup.controls.password_confirmation.addValidators(this.passwordMatchValidator(this.registerFormGroup.controls.password));
  }

  hide = true;
  hideConfirm = true;

  registerFormGroup = new FormGroup({
    email: new FormControl<string>('', [Validators.required, Validators.email]),
    name: new FormControl<string>('', [Validators.required]),
    surname: new FormControl<string>('', [Validators.required]),
    password: new FormControl<string>('', [Validators.required, Validators.minLength(6)]),
    password_confirmation: new FormControl<string>('', [Validators.required]),
    userAgreement: new FormControl<boolean>(false, [Validators.requiredTrue]),
  });



  getEmailErrorMessage() {
    if (this.registerFormGroup.controls.email.hasError('required')) {
      return 'Поле не должно быть пустым!';
    }

    return this.registerFormGroup.controls.email.hasError('email') ? 'Некорректный email' : '';
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
    if (this.registerFormGroup.invalid) {
      this.registerFormGroup.markAsTouched();
      this.registerFormGroup.controls.userAgreement.markAsTouched();
    } else {
      this.busy.next(true);
      this.userService.addUser(this.registerFormGroup.value.name!,this.registerFormGroup.value.surname!,
        this.registerFormGroup.value.email!, this.registerFormGroup.value.password!,
        this.registerFormGroup.value.userAgreement!).subscribe({
        next: () => this.busy.next(false),
        error: () => this.busy.next(false)
      });
    }
  }
}
