import { Component, inject, signal, DestroyRef } from '@angular/core';
import { RouterLink } from '@angular/router';

import { AuthService } from '../../core/services/AuthService';

@Component({
  selector: 'app-header',
  imports: [RouterLink],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header {
  // Inject
  AService = inject(AuthService);
  destroyRef = inject(DestroyRef);

  // Variables
  isSidebarVisible = signal<Boolean>(false);
  isLoggedIn = this.AService.isLoggedIn;

  toggleSidebar() {
    this.isSidebarVisible.update(s => !s);
  }

  // DOESNT WORK
  logout() {
    const subscription = this.AService.tryLogout()
      .subscribe();
    this.destroyRef.onDestroy(() => {
      subscription.unsubscribe();
    });
  }
}
