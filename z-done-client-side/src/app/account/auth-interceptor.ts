import {Injectable} from '@angular/core';
import {HttpClient, HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse} from '@angular/common/http';
import {Observable} from 'rxjs';
import {tap} from 'rxjs/operators';
import {Router} from '@angular/router';
import {CookieService} from 'ngx-cookie-service';

// import {Observable} from 'rxjs/Observable';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(public http: HttpClient, private router: Router, private cookieService: CookieService) {
  }


  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    req = req.clone({
      withCredentials: true,
      responseType: 'json'
    });

    console.log('INTERCEPTED');
    return next.handle(req).pipe(tap(
      event => {
        if (event instanceof HttpResponse) { console.log('Server OK response'); }
      },
      err => {
        if (err instanceof HttpErrorResponse) {
          if (err.status == 401) console.log('Unauthorized interceptor');
          this.router.navigate(['/account/login']);
        }
      }
    ));
  }
}
