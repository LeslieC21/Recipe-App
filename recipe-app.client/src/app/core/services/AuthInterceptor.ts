import { HttpErrorResponse, HttpEvent, HttpHandlerFn, HttpRequest } from "@angular/common/http"
import { inject } from '@angular/core';
import { Observable, throwError } from "rxjs";
import { catchError, switchMap } from 'rxjs/operators';

import { AuthService } from './AuthService';

export function AuthInterceptor(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> {
  const AService = inject(AuthService);

  // Clone the request to include creds (sends cookies automatically)
  // Dont forget to register it in app.config...
  const reqWithCreds = req.clone({ withCredentials: true });

  return next(reqWithCreds).pipe(
    catchError((error) => {
      if (error instanceof HttpErrorResponse && error.status === 401) {
        console.log("Cookie expired...");
        // Try to refresh the token
        return AService.refresh().pipe(
          switchMap(() => {
            console.log("Trying to refresh the token..");
            // Retry the orginal request
            return next(reqWithCreds);
          }),
          catchError(() => {
            // Refresh failed, redirect
            console.log("Failed to refresh the cookie...");
            AService.tryLogout();
            return throwError(() => error);
          })
          
        );
      }

      return throwError(() => error);
    })
  );
}
