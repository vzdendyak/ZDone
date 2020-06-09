import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot} from '@angular/router';
import {JwtHelperService} from '@auth0/angular-jwt';
import {CookieService} from 'ngx-cookie-service';
import {IdentityService} from './services/identity.service';
import {Observable, of} from 'rxjs';
import {catchError, map} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private jwtHelper: JwtHelperService, private router: Router, private cookieService: CookieService, private identityService: IdentityService) {

  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.identityService.isLogin().pipe(
      map(res => {
        if (res == true) {
          console.log('guard - allowed');
          return true;
        } else {
          console.log('guard - forbidden');
          this.router.navigate(['/account/login']);
          return false;
        }
      }),
      catchError((err) => {
        console.log('guard - forbidden');
        this.router.navigate(['/account/login']);
        return of(false);
      })
    );
  }

}
