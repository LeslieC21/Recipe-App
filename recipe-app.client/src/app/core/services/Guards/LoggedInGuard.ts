import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';

import { AuthService } from '../../services/AuthService';

export const loggedInGuard: CanActivateFn = (route, state) => {
  var AService = inject(AuthService);
  var router = inject(Router);

  // If User is logged in
  if (AService.isLoggedIn()) {
    return true;
  } else {
    return router.navigate(['/Login']);
  }
};
