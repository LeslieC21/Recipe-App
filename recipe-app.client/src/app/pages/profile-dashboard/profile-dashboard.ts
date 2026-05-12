import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { form, required, email, pattern, FormField } from '@angular/forms/signals';
import { FormsModule } from '@angular/forms';

import { AuthService } from '../../core/services/AuthService';
import { RecipeService } from '../../core/services/RecipeService';
import { UserModel } from '../../core/models/UserModel';
import { UpdateUserInfo } from '../../core/models/RequestModels/UpdateUserInfoModel';

@Component({
  selector: 'app-profile-dashboard',
  imports: [FormField, FormsModule],
  templateUrl: './profile-dashboard.html',
  styleUrl: './profile-dashboard.css',
})
export class ProfileDashboard implements OnInit{
  // Injects
  AService = inject(AuthService);
  RService = inject(RecipeService);
  destroyRef = inject(DestroyRef);

  // Variables
  showModal = signal<number>(0);
  showEditPage = signal<boolean>(false);
  updatedUserItem = signal<string>('');
  wasUsernameTaken = signal<boolean>(false);
  user = signal<UserModel>({
    username: '',
    firstName: '',
    lastName: '',
    phoneNumber: '',
    email: ''
  });
  userForm = form(this.user, (schemaPath) => {
    required(schemaPath.firstName, { message: 'This field is required.' });
    required(schemaPath.lastName, { message: 'This field is required.' });

    email(schemaPath.email, { message: 'Invalid Email. Format: example@gmail.com' });
    pattern(schemaPath.phoneNumber, /^\d{3}-\d{3}-\d{4}$/, { message: 'Invalid phone number. Format: XXX-XXX-XXXX' });
  })

  getUserInfo() {
    const subscription = this.AService.getUserInfo().subscribe(x => {
      console.log(x);
      this.user.set(x);
    });
    this.destroyRef.onDestroy(() => {
      subscription.unsubscribe();
    })
  }

  updateUserProfile(event: Event) {
    event.preventDefault();

    if (this.userForm().invalid())
      return;

    this.showEditPage.set(false);
    var updateReq: UpdateUserInfo = {
      email: this.user().email,
      phone: this.user().phoneNumber,
      firstName: this.user().firstName,
      lastName: this.user().lastName
    }

    const subscription = this.AService.updateUserInfo(updateReq).subscribe(x => {
      catchError: {
        // If there was an error we want the signal to reupdate to whats in the db
        this.getUserInfo();
      }
    });
    this.destroyRef.onDestroy(() => {
      subscription.unsubscribe();
    })
  }

  handleUpdateUsernameOrPassword() {
    this.wasUsernameTaken.set(false);
    // Changing username
    if (this.showModal() == 1) {
      const subscription = this.AService.checkUsernameAvaliability(this.updatedUserItem()).subscribe(x => {
        if (x) {
          // If true - then username is taken
          this.wasUsernameTaken.set(true);
        } else
          this.updateUsername();
      });
      this.destroyRef.onDestroy(() => {
        subscription.unsubscribe();
      });
    } else if (this.showModal() == 2) {
      this.updatePassword();
    }
  }

  updateUsername() {
    const subscription = this.AService.updateUsername(this.updatedUserItem()).subscribe(x => {
      this.getUserInfo();
      this.showModal.set(0);
    });
    this.destroyRef.onDestroy(() => {
      subscription.unsubscribe();
    });
  }

  updatePassword() {
    const subscription = this.AService.updatePassword(this.updatedUserItem()).subscribe(x => {
      this.showModal.set(0);
    });
    this.destroyRef.onDestroy(() => {
      subscription.unsubscribe();
    });
  }

  ngOnInit() {
    // Get User information
    this.getUserInfo();
  }
}
