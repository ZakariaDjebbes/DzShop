import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { catchError, delay } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private toastr: ToastrService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError(baseError => {
        if (baseError) {
          if (baseError.status === 404) {
            this.router.navigateByUrl('/not-found');
          } else if (baseError.status === 500) {
            const navExtras: NavigationExtras = {state: {error: baseError.error}};
            this.router.navigateByUrl('/server-error', navExtras);
          } else if (baseError.status === 400) {
            if (baseError.error.errors)
            {
              throw baseError.error;
            }

            this.toastr.error(baseError.error.message, 'Error ' + baseError.error.statusCode);
          } else if (baseError.status === 401) {
            this.toastr.error(baseError.error.message, 'Error ' + baseError.error.statusCode);
          }
        }
        return throwError(baseError);
      })
    );
  }
}
