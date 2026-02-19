import {Component, inject} from '@angular/core';
import {RouterLink} from '@angular/router';
import {FormControl, FormsModule, ReactiveFormsModule} from '@angular/forms';
import {ApiService} from '../../services/api-service';
import {UserDtoInterface} from '../../interfaces/user-dto-interface';

@Component({
  selector: 'app-login-page',
  imports: [
    RouterLink,
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './login-page.html',
  styleUrl: './login-page.css',
})
export class LoginPage {

  private apiService = inject(ApiService);

  email = new FormControl('');
  password = new FormControl('');

  loginUser() {

    const request: UserDtoInterface = {
      email: this.email.value ?? '',
      username: "", // Not used for logging in, but required by the interface
      password: this.password.value ?? '',
    }

    this.apiService.login(request).subscribe({
      next: response => {
        console.log(response);

        // Store the token in local storage
        localStorage.setItem('token', response.token);
        localStorage.setItem('username', JSON.stringify(response.username));
        localStorage.setItem('email', JSON.stringify(response.email));


      },
      error: error => console.log(error),
    })
  }
}
