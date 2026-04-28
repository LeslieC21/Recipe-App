import { Component, signal } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-header',
  imports: [RouterLink],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header {
  isSidebarVisible = signal<Boolean>(false);

  toggleSidebar() {
    this.isSidebarVisible.update(s => !s);
  }
}
