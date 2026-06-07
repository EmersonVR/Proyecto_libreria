import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';

interface NavItem {
  path: string;
  label: string;
  icon: string;
}

@Component({
  selector: 'app-layout',
  imports: [
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    MatIconModule,
    MatListModule,
    MatSidenavModule,
    MatToolbarModule
  ],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss'
})
export class LayoutComponent {
  readonly navItems: NavItem[] = [
    { path: '/books', label: 'Books', icon: 'menu_book' },
    { path: '/authors', label: 'Authors', icon: 'person_edit' },
    { path: '/categories', label: 'Categories', icon: 'category' },
    { path: '/readers', label: 'Readers', icon: 'badge' },
    { path: '/loans', label: 'Loans', icon: 'assignment_return' },
    { path: '/external-content', label: 'External Content', icon: 'public' }
  ];
}
