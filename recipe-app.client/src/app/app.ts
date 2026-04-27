import { Component, signal } from '@angular/core';
import { RouterOutlet, Router, ActivatedRoute, NavigationEnd } from '@angular/router';

import { Header } from '../app/layout/header/header';
import { filter } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  imports: [Header, RouterOutlet],
  styleUrl: './app.css'
})
export class App {
  showNavbar = signal(false);

  constructor(private router: Router, private route: ActivatedRoute) {
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        const childRoute = this.getChild(this.route);
        this.showNavbar.set(childRoute.snapshot.data['showNavbar'] !== false);
      })
  }

  getChild(route: ActivatedRoute): ActivatedRoute {
    while (route.firstChild) {
      route = route.firstChild;
    }
    return route;
  }
}

