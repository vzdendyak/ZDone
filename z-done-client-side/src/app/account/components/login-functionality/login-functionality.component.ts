import {Component, OnInit} from '@angular/core';
import {LoginModel} from '../../../models/auth/login-model';
import {Router} from '@angular/router';
import {IdentityService} from '../../services/identity.service';
import {ItemService} from '../../../items/services/item.service';
import {UserService} from '../../services/user.service';
import { HttpErrorResponse } from '@angular/common/http';
import {MatSnackBar} from '@angular/material/snack-bar';

@Component({
  selector: 'app-login-functionality',
  templateUrl: './login-functionality.component.html',
  styleUrls: ['./login-functionality.component.css']
})
export class LoginFunctionalityComponent implements OnInit {
  private invalidLogin: boolean;
  isLoading: boolean;
  constructor(private identityService: IdentityService,
              private router: Router,
              private userService: UserService,
              private itemService: ItemService, private snackBar: MatSnackBar) {
  }

  ngOnInit() {
    this.identityService.logOut().subscribe(r => {
      console.log('Logouted');
    });
  }

  loginSubmit(loginModel: LoginModel) {
    console.log('GOT+ ' + loginModel);
    this.isLoading = true;
    this.identityService.login(loginModel).subscribe(result => {
      this.invalidLogin = false;
      let dec = JSON.parse(atob(result.token.split('.')[1]));
      // we need to get current user by id from jwt
      localStorage.setItem('sub', dec.sub);
      this.router.navigate(['/windows']);

    }, error => {
      this.invalidLogin = true;
      let message = '';
      this.isLoading=false;
      if ((error as HttpErrorResponse).error[0] == undefined) {
        let err = error as HttpErrorResponse;
        if (err.error.errors != undefined){
          message += err.error.errors.Email != undefined ? err.error.errors.Email[0] : '\n';
          message += err.error.errors.Password != undefined ? err.error.errors.Password[0] : '\n';
        }
        this.snackBar.open(message, 'Ok', {duration: 5000});
      } else {
         message = (error as HttpErrorResponse).error[0];
         this.snackBar.open(message, 'Ok', {duration: 5000});
      }

    });
  }


  logOut() {
    localStorage.removeItem('jwt');
  }
}
