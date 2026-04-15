import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';

import { ApiService } from '../../services/api-service';

@Component({
  selector: 'app-create-movie-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './create-movie-page.html',
})
export class CreateMoviePage {
  private fb = inject(FormBuilder);
  private api = inject(ApiService);
  private router = inject(Router);

  isSubmitting = false;
  error: string | null = null;

  form = this.fb.nonNullable.group({
    title: ['', [Validators.required]],
    year: [new Date().getFullYear(), [Validators.required, Validators.min(1888)]],
    description: [''],
  });

  submit() {
    this.error = null;

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;

    const payload = this.form.getRawValue(); // { title, year, description }

    this.api.createMovie(payload).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.router.navigateByUrl('/');
      },
      error: (err) => {
        this.isSubmitting = false;
        this.error = err?.error?.message ?? 'Kunne ikke oprette movie.';
      },
    });
  }
}
