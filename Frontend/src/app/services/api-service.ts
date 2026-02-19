import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {UserDtoInterface} from '../interfaces/user-dto-interface';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private http = inject(HttpClient);

  private registerUrl = 'http://localhost:5102/api/Auth/register';
  private loginUrl = 'http://localhost:5102/api/Auth/login';

  register(request: UserDtoInterface): Observable<any> {
    return this.http.post(this.registerUrl, request);
  }

  login(request: UserDtoInterface): Observable<any> {
    return this.http.post(this.loginUrl, request);
  }
}
