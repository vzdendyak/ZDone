import {Component, EventEmitter, OnInit, Output, Input} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {LoginModel} from '../../../models/auth/login-model';
import {RegisterModel} from '../../../models/auth/register-model';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {
  registerReactiveForm: FormGroup;
  @Output() registerSubmit = new EventEmitter<RegisterModel>();
  @Input() isLoading: boolean;

  constructor(private fb: FormBuilder) {
  }

  get emailGet() {
    return this.registerReactiveForm.get('email');
  }

  get passwordGet() {
    return this.registerReactiveForm.get('password');
  }

  get usernameGet() {
    return this.registerReactiveForm.get('username');
  }

  get confirmedPasswordGet() {
    return this.registerReactiveForm.get('confirmedPassword');
  }

  ngOnInit() {
    this.initForm();
  }

  initForm() {
    this.registerReactiveForm = this.fb.group(
      {
        username: ['', [Validators.required, Validators.pattern('^(?!\\s*$).+')]],
        email: ['', [Validators.required, Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$')]],
        password: ['', [Validators.required, Validators.pattern('^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$')]],
        confirmedPassword: ['', [Validators.required]]
      },
      {validator: this.passwordConfirming}
    );
  }

  passwordConfirming(frm: FormGroup) {
    return frm.controls.password.value === frm.controls.confirmedPassword.value ? null : {mismatch: true};
  }

  onSubmit() {
    const model: RegisterModel = {
      email: this.registerReactiveForm.get('email').value,
      password: this.registerReactiveForm.get('password').value,
      name: this.registerReactiveForm.get('username').value,
      confirmedPassword: this.registerReactiveForm.get('confirmedPassword').value
    };
    this.registerSubmit.emit(model);
  }
}
