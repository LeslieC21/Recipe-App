import { Component, signal, inject, DestroyRef } from '@angular/core';
import { form, required, debounce, FormField } from '@angular/forms/signals';
import { catchError, EMPTY } from 'rxjs';
import { Router } from '@angular/router';

import { AuthService } from '../../core/services/AuthService';
import { Header } from '../../layout/header/header';

interface LoginModel {
  username: string;
  password: string;
}

@Component({
  selector: 'app-login',
  imports: [FormField],
  templateUrl: './login.html',
  styleUrl: './login.css',
})

export class Login {
  // Injects
  private AService = inject(AuthService);
  private destroyRef = inject(DestroyRef);

  // Variables
  errorMessage = signal<string | null>(null);
  isLoggedIn = signal<Boolean>(false);

  // Login Model and Login Form
  loginModel = signal<LoginModel>({
    username: '',
    password: ''
  })

  loginForm = form(this.loginModel, (schemaPath) => {
    required(schemaPath.username, { message: "Please enter a username."});
    required(schemaPath.password, { message: "Please enter a password." });

    debounce(schemaPath.username, 300);
    debounce(schemaPath.password, 300);
  })

  tryLogin(event: Event) {
    event.preventDefault();
    this.errorMessage.set(null);

    if (this.loginForm().invalid()) 
      return;

    const subscription = this.AService.tryLogin(this.loginModel()).pipe(
      catchError(error => {
        // Failed to log in
        this.errorMessage.set(error.error);
        return EMPTY;
      }))
      .subscribe();
    this.destroyRef.onDestroy(() => {
      subscription.unsubscribe();
    });
  }
}
