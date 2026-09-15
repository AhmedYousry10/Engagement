import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./pages/public-site/public-site/public-site').then((m) => m.PublicSite)
  },
  {
    path: 'admin/login',
    loadComponent: () =>
      import('./pages/dashboard/dashboard-login/dashboard-login').then((m) => m.DashboardLogin)
  },
  {
    path: 'admin',
    loadComponent: () =>
      import('./pages/dashboard/dashboard-layout/dashboard-layout').then((m) => m.DashboardLayout),
    canActivate: [authGuard],
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'basics' },
      {
        path: 'basics',
        loadComponent: () =>
          import('./pages/dashboard/dashboard-basics/dashboard-basics').then((m) => m.DashboardBasics)
      },
      {
        path: 'details',
        loadComponent: () =>
          import('./pages/dashboard/dashboard-details/dashboard-details').then((m) => m.DashboardDetails)
      },
      {
        path: 'photos',
        loadComponent: () =>
          import('./pages/dashboard/dashboard-photos/dashboard-photos').then((m) => m.DashboardPhotos)
      },
      {
        path: 'settings',
        loadComponent: () =>
          import('./pages/dashboard/dashboard-settings/dashboard-settings').then((m) => m.DashboardSettings)
      }
    ]
  },
  { path: '**', redirectTo: '' }
];
