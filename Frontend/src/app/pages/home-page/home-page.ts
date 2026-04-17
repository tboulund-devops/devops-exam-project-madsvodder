import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MoviesGrid } from '../../components/movies-grid/movies-grid';

@Component({
  selector: 'app-home-page',
  imports: [
    MoviesGrid
  ],
  templateUrl: './home-page.html',
  styleUrl: './home-page.css',
})
export class HomePage {

}
