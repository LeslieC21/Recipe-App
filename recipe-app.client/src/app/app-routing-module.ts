import { Routes } from '@angular/router';

import { Home } from './pages/home/home';
import { Recipes } from './pages/recipes/recipes';

export const routes: Routes = [
  {
    path: '',
    component: Home,
    data: { showNavbar: false }
  },
  {
    path: 'Home',
    component: Home,
    data: { showNavbar: false }
  },
  {
    path: 'Recipe-List',
    component: Recipes,
    data: { showNavbar: true }
  }
]
