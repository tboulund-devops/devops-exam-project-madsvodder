import {Component, inject, OnInit} from '@angular/core';
import {ApiService} from '../../services/api-service';
import {Observable} from 'rxjs';
import {Movie} from '../../interfaces/movie';
import {MovieCard} from '../movie-card/movie-card';
import {AsyncPipe} from '@angular/common';

@Component({
  selector: 'app-movies-grid',
  imports: [
    MovieCard,
    AsyncPipe
  ],
  templateUrl: './movies-grid.html',
  styleUrl: './movies-grid.css',
})
export class MoviesGrid implements OnInit {

  movies$!: Observable<Movie[]>;

  apiService: ApiService = inject(ApiService);

  ngOnInit() {
    this.movies$ = this.apiService.getAllMovies();
    console.log(this.movies$);
  }
}
