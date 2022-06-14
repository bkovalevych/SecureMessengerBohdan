import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { AuthService } from '../services/api/auth.service';
import { EmptyApiResponse } from '../models/values/api-response';
import { ActivatedRoute, Router } from '@angular/router';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService,
    private router: Router,
    private activatedRoute: ActivatedRoute) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return this.addHeader(request, next)
    .pipe(
      catchError((error : HttpErrorResponse) => {
        if(error.status === 401) {
          if (this.authService.accessToken) {
            return this.authService.refresh()
            .pipe(switchMap(
                () => this.addHeader(request, next)))
          } else {
            this.router.navigate(['/identity/login'], {
              queryParams: {
                returnUrl: this.activatedRoute.url
              },
              queryParamsHandling: 'merge'
            })
          }
        }
        let response: EmptyApiResponse = error.error as EmptyApiResponse || 
        {
          errors: [],
          isSucceeded: false
        };
        return throwError(() => response);
      })
    );
  }
  addHeader(req: HttpRequest<any>, next: HttpHandler) :  Observable<HttpEvent<any>> {
    const token = localStorage.getItem("secureMessengerAccessToken");
    if (!token) {
        return next.handle(req);
    }
    return next.handle(req.clone({
      setHeaders: {
        "Authorization": `Bearer ${token}`
      }
    }))
}
}
