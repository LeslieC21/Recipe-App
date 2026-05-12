import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { Router } from '@angular/router';

import { LoginRequest } from '../models/RequestModels/LoginRequest';
import { RegisterRequest } from '../models/RequestModels/RegisterRequest';
import { UserModel } from '../models/UserModel';
import { UpdateUserInfo } from '../models/RequestModels/UpdateUserInfoModel';
import { UpdateUsername } from '../models/RequestModels/UpdateUsernameRequest';
import { UpdatePassword } from '../models/RequestModels/UpdatePasswordRequest';


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
        console.log("We are.." , this.checkCookie());
      })
    )
  }

  tryRegister(request: RegisterRequest): Observable<any> {
    return this.httpClient.post<boolean>('/Token/Register', request);
  }

  refresh(): Observable<any> {
    console.log("Refreshing...");
    return this.httpClient.post('/Token/Refresh', {}).pipe(
      tap(() => {
        this.loggedIn.set(this.checkCookie());
      })
    );
  }

  getUserInfo() {
    return this.httpClient.get<UserModel>('/Token/User');
  }

  updateUserInfo(updateReq: UpdateUserInfo) {
    return this.httpClient.put<boolean>('/Token/Update/UserInfo', updateReq);
  }

  checkUsernameAvaliability(username: string) {
    return this.httpClient.get<boolean>(`/Token/IsUsernameTaken/${username}`);
  }

  updateUsername(username: string) {
    var updateReq: UpdateUsername = {
      Username: username
    }

    return this.httpClient.put<boolean>('/Token/Update/Username', updateReq);
  }

  updatePassword(password: string) {
    var updateReq: UpdatePassword = {
      Password: password
    }
    return this.httpClient.put<boolean>('/Token/Update/Password', updateReq);
  }
}
