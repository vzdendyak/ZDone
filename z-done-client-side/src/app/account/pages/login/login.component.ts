import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {LoginModel} from '../../../models/auth/login-model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginReactiveForm: FormGroup;
  @Output() loginSubmit = new EventEmitter<LoginModel>();
  constructor(private fb: FormBuilder) {
  }

  get emailGet() {
    return this.loginReactiveForm.get('email');
  }

  get passwordGet() {
    return this.loginReactiveForm.get('password');
  }

  ngOnInit() {
    this.initForm();
  }

  initForm() {
    this.loginReactiveForm = this.fb.group({
      email: ['', [Validators.required, Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$')]],
      password: ['', [Validators.required, Validators.pattern('^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$')]]
    });
  }

  onSubmit() {
   const model: LoginModel = {email: this.loginReactiveForm.get('email').value, password: this.loginReactiveForm.get('password').value };
   this.loginSubmit.emit(model);
  }
}
