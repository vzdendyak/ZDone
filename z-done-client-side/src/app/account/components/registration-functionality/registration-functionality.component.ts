import { Component, OnInit } from '@angular/core';
import {RegisterModel} from '../../../models/auth/register-model';
import {IdentityService} from '../../services/identity.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-registration-functionality',
  templateUrl: './registration-functionality.component.html',
  styleUrls: ['./registration-functionality.component.css']
})
export class RegistrationFunctionalityComponent implements OnInit {
  private invalidRegistration: boolean;

  constructor(private identityService: IdentityService, private router: Router) { }

  ngOnInit() {
  }

  register(model: RegisterModel) {
    console.log('GOT+ ' + model);
    this.identityService.register(model).subscribe(result => {
      this.router.navigate(['/account/login']);
    }, err => {
      this.invalidRegistration = true;
    });
  }
}
