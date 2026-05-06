import { Component, signal, inject } from '@angular/core';
import { form, required, debounce, FormField, pattern, email } from '@angular/forms/signals';

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
  // Login Model and Login Form
  loginModel = signal<LoginModel>({
    username: '',
    password: ''
  })

  loginForm = form(this.loginModel, (schemaPath) => {
    required(schemaPath.username, { message: "Please enter a username."});
    required(schemaPath.password, { message: "Please enter a password." });
  })

  tryLogin(event: Event) {
    event.preventDefault();

    if (this.loginForm().invalid())
      return;


  }
}
