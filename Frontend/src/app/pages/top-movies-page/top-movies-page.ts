import { Component } from '@angular/core';
import {TopMoviesGrid} from '../../components/top-movies-grid/top-movies-grid';

@Component({
  selector: 'app-top-movies-page',
  imports: [
    TopMoviesGrid
  ],
  templateUrl: './top-movies-page.html',
  styleUrl: './top-movies-page.css',
})
export class TopMoviesPage {

}
