import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './pages/login/login.component';
import { LoginFunctionalityComponent } from './components/login-functionality/login-functionality.component';
import { RegistrationComponent } from './pages/registration/registration.component';
import { RegistrationFunctionalityComponent } from './components/registration-functionality/registration-functionality.component';
import {RouterModule} from '@angular/router';



@NgModule({
  declarations: [LoginComponent, LoginFunctionalityComponent, RegistrationComponent, RegistrationFunctionalityComponent],
  imports: [
    CommonModule,
    RouterModule
  ]
})
export class LoginModule { }
