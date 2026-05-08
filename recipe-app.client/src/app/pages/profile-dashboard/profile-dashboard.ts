import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';

import { AuthService } from '../../core/services/AuthService';
import { UserModel } from '../../core/models/UserModel';

@Component({
  selector: 'app-profile-dashboard',
  imports: [],
  templateUrl: './profile-dashboard.html',
  styleUrl: './profile-dashboard.css',
})
export class ProfileDashboard implements OnInit{
  // Injects
  AService = inject(AuthService);
  destroyRef = inject(DestroyRef);

  // Variables
  showEditPage = signal<boolean>(false);
  user = signal<UserModel>({
    username: '',
    firstName: '',
    lastName: '',
    phoneNumber: '',
    email: ''
  });

  getUserInfo() {
    const subscription = this.AService.getUserInfo().subscribe(x => {
      this.user.set(x);
    });
    this.destroyRef.onDestroy(() => {
      subscription.unsubscribe();
    })
  }

  updateUserProfile() {
    this.showEditPage.set(false);
  }

  ngOnInit() {
    // Get User information
    this.getUserInfo();
  }
}
