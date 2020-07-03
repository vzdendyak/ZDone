import { Component, OnInit } from '@angular/core';
import {RegisterModel} from '../../../models/auth/register-model';
import {IdentityService} from '../../services/identity.service';
import {Router} from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-registration-functionality',
  templateUrl: './registration-functionality.component.html',
  styleUrls: ['./registration-functionality.component.css']
})
export class RegistrationFunctionalityComponent implements OnInit {
  private invalidRegistration: boolean;
  isLoading: boolean;

  constructor(private identityService: IdentityService, private router: Router,  private snackBar: MatSnackBar) { }

  ngOnInit() {
  }

  register(model: RegisterModel) {
    console.log('GOT+ ' + model);
    this.isLoading = true;

    this.identityService.register(model).subscribe(result => {
      this.router.navigate(['/account/login']);
    }, error => {
      this.invalidRegistration = true;
      this.isLoading = false;
      let message = '';
      if ((error as HttpErrorResponse).error[0] == undefined) {
        let err = error as HttpErrorResponse;
        if (err.error.errors != undefined){
          message += err.error.errors.Email != undefined ? err.error.errors.Email[0] : '\n';
          message += err.error.errors.Name != undefined ? err.error.errors.Name[0] : '\n';
          message += err.error.errors.Password != undefined ? err.error.errors.Password[0] : '\n';
        }
        this.snackBar.open(message, 'Ok', {duration: 5000});
      } else {
         message = (error as HttpErrorResponse).error[0];
         this.snackBar.open(message, 'Ok', {duration: 5000});
      }
    });
  }
}
