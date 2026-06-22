import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./venues/venues.component').then(m => m.VenuesComponent)
  }
];
