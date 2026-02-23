import { Component } from '@angular/core';
import {MovieCard} from '../../components/movie-card/movie-card';
import {MoviesGrid} from '../../components/movies-grid/movies-grid';

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
