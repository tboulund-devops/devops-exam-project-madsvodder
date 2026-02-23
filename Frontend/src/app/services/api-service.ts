import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {UserDtoInterface} from '../interfaces/user-dto-interface';
import {Observable} from 'rxjs';
import {Movie} from '../interfaces/movie';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private http = inject(HttpClient);

  private url = 'http://localhost:5102/api';

  // User
  register(request: UserDtoInterface): Observable<any> {
    return this.http.post(`${this.url}/Auth/register`, request);
  }

  login(request: UserDtoInterface): Observable<any> {
    return this.http.post(`${this.url}/Auth/login`, request);
  }

  // Movies
  getAllMovies(): Observable<Movie[]> {
    return this.http.get<Movie[]>(`${this.url}/movies`)
  }

  sendRating(request: Movie) {
    return this.http.put(`${this.url}/movies/` + request.id, request);
  }
}
