import {Component, OnInit} from '@angular/core';
import {LoginModel} from '../../../models/auth/login-model';
import {Router} from '@angular/router';
import {IdentityService} from '../../services/identity.service';
import {ItemService} from '../../../items/services/item.service';
import {UserService} from '../../services/user.service';

@Component({
  selector: 'app-login-functionality',
  templateUrl: './login-functionality.component.html',
  styleUrls: ['./login-functionality.component.css']
})
export class LoginFunctionalityComponent implements OnInit {
  private invalidLogin: boolean;

  constructor(private identityService: IdentityService,
              private router: Router,
              private userService: UserService,
              private itemService: ItemService) {
  }

  ngOnInit() {
    this.identityService.logOut().subscribe(r => {
      console.log('Logouted');
    });
  }

  loginSubmit(loginModel: LoginModel) {
    console.log('GOT+ ' + loginModel);
    this.identityService.login(loginModel).subscribe(result => {
      this.invalidLogin = false;
      let dec = JSON.parse(atob(result.token.split('.')[1]));
      // we need to get current user by id from jwt
      localStorage.setItem('sub', dec.sub);
      this.router.navigate(['/windows']);

    }, err => {
      this.invalidLogin = true;
    });
  }


  logOut() {
    localStorage.removeItem('jwt');
  }
}
