import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { Router } from '@angular/router';

import { LoginRequest } from '../models/RequestModels/LoginRequest';
import { RegisterRequest } from '../models/RequestModels/RegisterRequest';
import { UserModel } from '../models/UserModel';


@Injectable({
  providedIn: 'root'
})

// Handles Login - logout - refresh
export class AuthService {
  private httpClient = inject(HttpClient);
  private router = inject(Router);
  private loggedIn = signal<Boolean>(this.checkCookie());

  readonly isLoggedIn = this.loggedIn.asReadonly();

  private checkCookie() {
    return document.cookie
      .split(';')
      .find(index => index.startsWith('logged_in'))
      ?.split('=')[1] === 'true';
  }

  tryLogin(request: LoginRequest): Observable<any> {
    console.log("Logging In..");
    return this.httpClient.post('/Token/Login', request).pipe(
      tap(() => {
        this.loggedIn.set(this.checkCookie());
        this.router.navigate(['/Favorite-Recipes']);
      })
    );
  }

  tryLogout(): Observable<any> {
    console.log("Logging Out..");
    return this.httpClient.post('/Token/Logout', {}).pipe(
      tap(() => {
        this.loggedIn.set(false);
        this.router.navigate(['/Home']);
        console.log(this.checkCookie());
      })
    )
  }

  tryRegister(request: RegisterRequest): Observable<any> {
    return this.httpClient.post<boolean>('/Token/Register', request);
  }

  refresh():Observable<any> {
    return this.httpClient.post('/Token/Refresh', {});
  }

  getUserInfo() {
    return this.httpClient.get<UserModel>('/Token/User');
  }
}
