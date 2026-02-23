import {Component, inject, input, OnInit} from '@angular/core';
import {Movie} from '../../interfaces/movie';
import {ApiService} from '../../services/api-service';

@Component({
  selector: 'app-movie-card',
  imports: [],
  templateUrl: './movie-card.html',
  styleUrl: './movie-card.css',
  standalone: true,
})
export class MovieCard implements OnInit {
  movie = input<Movie>();

  apiService: ApiService = inject(ApiService);

  ngOnInit() {
    console.log(this.movie);
  }

  sendRatingRequest(value: string) {

    // Create new movie with updated rating
    let movie: Movie = {
      title: this.movie()!.title,
      id: this.movie()!.id,
      rating: Number(value),
      year: this.movie()!.year,
      description: this.movie()!.description,
    }

    this.apiService.sendRating(movie).subscribe({
      next: result => console.log(result),
      error: err => console.error(err),
      complete: () => console.log('done')
    })
  }
}
