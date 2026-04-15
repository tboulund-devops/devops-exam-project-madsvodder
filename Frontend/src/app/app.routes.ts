import { Routes } from '@angular/router';
import { HomePage } from './pages/home-page/home-page';
import { LoginPage } from './pages/login-page/login-page';
import { RegisterPage } from './pages/register-page/register-page';
import { CreateMoviePage } from './pages/create-movie-page/create-movie-page';

export const routes: Routes = [
  { path: '', component: HomePage, title: 'Home' },
  { path: 'login', component: LoginPage, title: 'Login' },
  { path: 'register', component: RegisterPage, title: 'Register' },
  { path: 'movies/create', component: CreateMoviePage, title: 'Create Movie' },
];
