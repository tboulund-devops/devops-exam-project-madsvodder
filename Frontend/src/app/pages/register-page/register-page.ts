import {Component, inject} from '@angular/core';
import {RouterLink} from '@angular/router';
import {ApiService} from '../../services/api-service';
import {UserDtoInterface} from '../../interfaces/user-dto-interface';
import {FormControl, FormsModule, ReactiveFormsModule} from '@angular/forms';

@Component({
  selector: 'app-register-page',
  imports: [
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './register-page.html',
  styleUrl: './register-page.css',
})
export class RegisterPage {
  private apiService = inject(ApiService);

  username = new FormControl('');
  email = new FormControl('');
  password = new FormControl('');

  registerUser() {

    const request: UserDtoInterface = {
      email: this.email.value ?? '',
      username: this.username.value ?? '',
      password: this.password.value ?? '',
    }

    console.log(request);

    this.apiService.register(request).subscribe({
      next: response => console.log(response),
      error: err => console.log(err)
    });
  }
}
