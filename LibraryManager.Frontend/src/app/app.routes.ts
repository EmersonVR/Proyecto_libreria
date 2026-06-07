import { Routes } from '@angular/router';
import { LayoutComponent } from './layout/layout.component';

export const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'books' },
      {
        path: 'authors',
        loadComponent: () => import('./features/authors/authors-page.component').then(m => m.AuthorsPageComponent)
      },
      {
        path: 'categories',
        loadComponent: () => import('./features/categories/categories-page.component').then(m => m.CategoriesPageComponent)
      },
      {
        path: 'books',
        loadComponent: () => import('./features/books/books-page.component').then(m => m.BooksPageComponent)
      },
      {
        path: 'readers',
        loadComponent: () => import('./features/readers/readers-page.component').then(m => m.ReadersPageComponent)
      },
      {
        path: 'loans',
        loadComponent: () => import('./features/loans/loans-page.component').then(m => m.LoansPageComponent)
      },
      {
        path: 'external-content',
        loadComponent: () => import('./features/external-content/external-content-page.component').then(m => m.ExternalContentPageComponent)
      }
    ]
  },
  { path: '**', redirectTo: 'books' }
];
