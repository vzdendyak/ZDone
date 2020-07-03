import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {LoginComponent} from './pages/login/login.component';
import {LoginFunctionalityComponent} from './components/login-functionality/login-functionality.component';
import {RegistrationComponent} from './pages/registration/registration.component';
import {RegistrationFunctionalityComponent} from './components/registration-functionality/registration-functionality.component';
import {RouterModule} from '@angular/router';
import {ReactiveFormsModule} from '@angular/forms';
import {JwtModule} from '@auth0/angular-jwt';
import { MaterialAppsModule } from '../ngmaterial.module';

export function tokenGetter() {
  return localStorage.getItem('jwt');
}

@NgModule({
  declarations: [LoginComponent, LoginFunctionalityComponent, RegistrationComponent, RegistrationFunctionalityComponent],
  imports: [
    CommonModule,
    RouterModule,
    MaterialAppsModule,
    ReactiveFormsModule,
    JwtModule.forRoot({
      config: {
        tokenGetter,
        whitelistedDomains: ['localhost:5000'],
        blacklistedRoutes: []
      }
    })
  ]
})
export class LoginModule {
}
