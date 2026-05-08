import { Component, inject, signal, DestroyRef } from '@angular/core';
import { form, FormField, required, email, pattern, minLength, validate, validateHttp, debounce } from '@angular/forms/signals';
import { RouterLink, Router } from '@angular/router';
import { catchError, EMPTY } from 'rxjs';

import { AuthService } from '../../core/services/AuthService';

interface RegisterModel {
  username: string;
  password: string;
  confirmPassword: string;
  firstName: string;
  lastName: string;
  phone: string;
  email: string;
}

@Component({
  selector: 'app-register',
  imports: [FormField, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  // Injects
  AService = inject(AuthService);
  destroyRef = inject(DestroyRef);
  router = inject(Router);

  // Variables
  errorMessage = signal<string | null>(null);

  // Register Form and Model
  registerModel = signal<RegisterModel>({
    username: '',
    password: '',
    confirmPassword: '',
    firstName: '',
    lastName: '',
    phone: '',
    email: ''
  })

  registerForm = form(this.registerModel, (schemaPath) => {
    // Required inputs to register
    required(schemaPath.username, { message: 'This field is required.' });
    required(schemaPath.password, { message: 'This field is required.' });
    required(schemaPath.confirmPassword, { message: 'This field is required.' });
    required(schemaPath.firstName, { message: 'This field is required.' });
    required(schemaPath.lastName, { message: 'This field is required.' });

    // Ensure email and password are in the correct formats
    email(schemaPath.email, { message: 'Please enter a valid email.' });
    pattern(schemaPath.phone, /^\d{3}-\d{3}-\d{4}$/, { message: 'Please enter a valid Phone Number.' });

    // Password must be AT LEAST 7 characters long, and confirmPassword and password must match.
    minLength(schemaPath.password, 7, { message: 'Password must be at least 7 characters.' })
    validate(schemaPath.confirmPassword, ({ value, valueOf }) => {
      const confirmPassword = value();
      const password = valueOf(schemaPath.password);

      if (confirmPassword !== password) {
        return {
          kind: 'passwordMismatch',
          message: 'Passwords do not match.'
        };
      }

      return null;
    });

    // Calls API to check if the username has been taken
    debounce(schemaPath.username, 300);
    validateHttp(schemaPath.username, {
      request: ({ value }) => `/Token/IsUsernameTaken/${value()}`,
      onSuccess: (response: boolean) => {
        console.log(response)
        if (response) {
          return {
            kind: 'usernameTaken',
            message: 'Username is already taken'
          };
        }
        return null;
      },
      onError: (error) => ({
        kind: 'networkError',
        message: 'Could not verify username availability',
      }),
    });
  });

  tryRegister(event: Event) {
    event.preventDefault();
    this.errorMessage.set(null);

    if (this.registerForm().invalid())
      return;

    // Deconstruct the form - we want to exclude the confirmPassword
    const { confirmPassword, ...registerReq } = this.registerForm().value();

    const subscription = this.AService.tryRegister(registerReq).pipe(
      catchError(error => {
        // Failed to register
        this.errorMessage.set(error.error);
        return EMPTY;
      }))
      .subscribe((x: boolean) => {
      if (x) {
        this.router.navigate(['/Recipe-List']);
      }
    });
    this.destroyRef.onDestroy(() => {
      subscription.unsubscribe();
    })
  }
}
