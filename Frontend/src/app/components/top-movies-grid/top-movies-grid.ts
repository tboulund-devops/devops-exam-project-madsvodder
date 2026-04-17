import {Component, inject, OnInit} from '@angular/core';
import {Movie} from '../../interfaces/movie';
import {ApiService} from '../../services/api-service';
import {map, Observable} from 'rxjs';
import {AsyncPipe} from '@angular/common';
import {MovieCard} from '../movie-card/movie-card';

@Component({
  selector: 'app-top-movies-grid',
  imports: [
    AsyncPipe,
    MovieCard
  ],
  templateUrl: './top-movies-grid.html',
  styleUrl: './top-movies-grid.css',
})
export class TopMoviesGrid implements OnInit{
  movies$!: Observable<Movie[]>;

  apiService: ApiService = inject(ApiService);

  ngOnInit() {
    this.movies$ = this.apiService.getTopMovies().pipe(
      map(movies =>
        [...movies].sort((a, b) => b.rating - a.rating)
      )
    );
  }
}
