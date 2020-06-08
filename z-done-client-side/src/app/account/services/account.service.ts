import {Injectable} from '@angular/core';
import {environment} from '../../../environments/environment';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {LoginModel} from '../../models/auth/login-model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  url: string = environment.basicUrl + '/auth';
  requestOptions: object = {
    headers: new HttpHeaders().append('Authorization', 'Bearer <yourtokenhere>'),
    responseType: 'json'
  };

  constructor(private http: HttpClient) {
  }

  login(loginModel: LoginModel) {
    return this.http.post(this.url + '/login', loginModel, this.requestOptions);
  }
}
