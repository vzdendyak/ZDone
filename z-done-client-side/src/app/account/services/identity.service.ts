import {Injectable} from '@angular/core';
import {environment} from '../../../environments/environment';
import {HttpClient, HttpResponse} from '@angular/common/http';
import {LoginModel} from '../../models/auth/login-model';
import {RegisterModel} from '../../models/auth/register-model';
import {AuthResponse} from '../../models/auth/auth-response';
import {Observable} from 'rxjs';
import {map} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class IdentityService {
  url: string = environment.basicUrl + '/auth';
  requestOptions: object = {
    // headers: new HttpHeaders().append('Authorization', 'Bearer <yourtokenhere>'),
    // responseType: 'json',
    withCredentials: true
  };

  constructor(private http: HttpClient) {
  }

  login(loginModel: LoginModel) {
    return this.http.post<AuthResponse>(this.url + '/login', loginModel, this.requestOptions);
  }

  register(registerModel: RegisterModel) {
    return this.http.post(this.url + '/register', registerModel, this.requestOptions);
  }

  isLogin() {
    return this.http.get(this.url + '/isLogined');
  }

  logOut() {
    return this.http.post(this.url + '/logout', this.requestOptions);
  }
}
