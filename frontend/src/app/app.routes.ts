import { Routes } from '@angular/router';
import { Login } from './features/auth/pages/login/login';
import MainLayout from './core/layouts/main-layout/main-layout';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: Login },
  {
    path: 'app',
    component: MainLayout,
    // Aquí adentro irán el dashboard, las citas y el mapa de SafeRoute
    children: [],
  },
];
