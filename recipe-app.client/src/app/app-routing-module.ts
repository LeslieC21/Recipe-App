import { Routes } from '@angular/router';

import { Home } from './pages/home/home';
import { Recipes } from './pages/recipes/recipes';
import { FavoriteRecipes } from './pages/favorite-recipes/favorite-recipes';
import { CreateRecipe } from './pages/create-recipe/create-recipe';
import { Login } from './pages/login/login';
import { ProfileDashboard } from './pages/profile-dashboard/profile-dashboard';
import { Register } from './pages/register/register';

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
  },
  {
    path: 'Favorite-Recipes',
    component: FavoriteRecipes,
    data: { showNavbar: true }
  },
  {
    path: 'Create-Recipe',
    component: CreateRecipe,
    data: { showNavbar: true }
  },
  {
    path: 'Login',
    component: Login,
    data: { showNavbar: true }
  },
  {
    path: 'Register',
    component: Register,
    data: { showNavbar: true }
  },
  {
    path: 'Profile',
    component: ProfileDashboard,
    data: { showNavbar: true }
  },
  {
    path: '**',
    redirectTo: ''
  }
]
