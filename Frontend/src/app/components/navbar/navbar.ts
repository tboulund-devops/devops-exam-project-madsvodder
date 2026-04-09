import {Component, inject, OnInit} from '@angular/core';
import {RouterLink} from '@angular/router';
import {ApiService} from '../../services/api-service';
import {FeatureService} from '../../services/feature-service';

@Component({
  selector: 'app-navbar',
  imports: [
    RouterLink
  ],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar implements OnInit {
  protected readonly localStorage = localStorage;

  loginEnabled = false;

  constructor(private featureService: FeatureService) {
  }

  async ngOnInit() {
    this.loginEnabled = await this.featureService.isLoginEnabled();
    console.log(this.loginEnabled);
  }

  logOut(): void {
    this.localStorage.clear();
  }

  getUsername(): string {
    return <string>this.localStorage.getItem('username')
  }
}
