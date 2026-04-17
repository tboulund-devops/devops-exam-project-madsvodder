import {Component, inject, OnInit} from '@angular/core';
import {ApiService} from '../../services/api-service';
import {BehaviorSubject, combineLatest, map, Observable} from 'rxjs';
import {Movie} from '../../interfaces/movie';
import {MovieCard} from '../movie-card/movie-card';
import {AsyncPipe} from '@angular/common';
import {FormsModule} from '@angular/forms';

@Component({
  selector: 'app-movies-grid',
  imports: [
    MovieCard,
    AsyncPipe,
    FormsModule
  ],
  templateUrl: './movies-grid.html',
  styleUrl: './movies-grid.css',
})
export class MoviesGrid implements OnInit {

  movies$!: Observable<Movie[]>;
  searchQuery$ = new BehaviorSubject<string>('');
  searchQuery = '';

  apiService: ApiService = inject(ApiService);

  ngOnInit() {
    const allMovies$ = this.apiService.getAllMovies().pipe(
      map(movies => movies.sort((a, b) => b.rating - a.rating))
    );

    this.movies$ = combineLatest([allMovies$, this.searchQuery$]).pipe(
      map(([movies, query]) =>
        movies.filter(movie =>
          movie.title.toLowerCase().includes(query.toLowerCase())
        )
      )
    );
  }

  onSearch() {
    this.searchQuery$.next(this.searchQuery);
  }
}
