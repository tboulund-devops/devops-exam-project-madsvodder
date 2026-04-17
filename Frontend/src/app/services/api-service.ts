import { inject, Injectable, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserDtoInterface } from '../interfaces/user-dto-interface';
import { Observable } from 'rxjs';
import { Movie } from '../interfaces/movie';

@Injectable({
  providedIn: 'root',
})
export class ApiService implements OnInit {
  private http = inject(HttpClient);

  private url = 'http://157.173.116.163:8000/api';
  private localUrl = 'http://localhost:5102/api';

  ngOnInit() {
    //this.url = this.localUrl;
  }

  // User
  register(request: UserDtoInterface): Observable<any> {
    return this.http.post(`${this.url}/Auth/register`, request);
  }

  login(request: UserDtoInterface): Observable<any> {
    return this.http.post(`${this.url}/Auth/login`, request);
  }

  // Movies
  getAllMovies(): Observable<Movie[]> {
    return this.http.get<Movie[]>(`${this.url}/movies`);
  }

  getTopMovies(): Observable<Movie[]> {
    return this.http.get<Movie[]>(`${this.url}/movies/top`);
  }

  sendRating(movieId: number, score: number, comment?: string) {
    return this.http.post(`${this.url}/movies/${movieId}/ratings`, {
      score,
      comment: comment ?? null
    });
  }

  createMovie(movie: { title: string; year: number; description: string }) {
    return this.http.post(`${this.url}/movies`, movie);
  }

  getAverageRating(movieId: number) {
    return this.http.get<{ average: number }>(`${this.url}/movies/${movieId}/ratings/average`);
  }
}
