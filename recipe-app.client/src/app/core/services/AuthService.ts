import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { LoginRequest } from '../models/RequestModels/LoginRequest';
import { RegisterRequest } from '../models/RequestModels/RegisterRequest';


@Injectable({
  providedIn: 'root'
})

// Handles Login - logout - refresh
export class AuthService {
  private httpClient = inject(HttpClient);

  tryLogin(request: LoginRequest): Observable<any> {
    return this.httpClient.post('/Token/Login', request);
  }

  tryLogout(): Observable<any> {
    return this.httpClient.post('/Token/Logout', {});
  }

  tryRegister(request: RegisterRequest): Observable<any> {
    return this.httpClient.post('/Token/Register', request);
  }

  refresh():Observable<any> {
    return this.httpClient.post('Token/Refresh', {});
  }
}
