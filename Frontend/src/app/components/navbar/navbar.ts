import {Component, inject} from '@angular/core';
import {RouterLink} from '@angular/router';
import {ApiService} from '../../services/api-service';

@Component({
  selector: 'app-navbar',
  imports: [
    RouterLink
  ],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar {
  protected readonly localStorage = localStorage;

  logOut(): void {
    this.localStorage.clear();
  }

  getUsername(): string {
    return <string>this.localStorage.getItem('username')
  }
}
