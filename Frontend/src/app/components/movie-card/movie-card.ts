import {Component, inject, input, OnInit, signal} from '@angular/core';
import {Movie} from '../../interfaces/movie';
import {ApiService} from '../../services/api-service';
import {FeatureService} from '../../services/feature-service';

@Component({
  selector: 'app-movie-card',
  imports: [],
  templateUrl: './movie-card.html',
  styleUrl: './movie-card.css',
  standalone: true,
})
export class MovieCard implements OnInit {
  movie = input<Movie>();
  rating = signal<number>(1); // separate writable signal

  canRate: boolean = false;

  apiService: ApiService = inject(ApiService);
  featureService: FeatureService = inject(FeatureService);

  async ngOnInit() {
    this.apiService.getAverageRating(this.movie()!.id).subscribe({
      next: result => this.rating.set(result.average),
      error: err => console.error(err),
    });

    this.canRate = await this.featureService.isRatingEnabled();
    console.log(this.canRate);
  }

  sendRatingRequest(value: string) {
    this.rating.set(Number(value)); // update local signal immediately

    let movie: Movie = {
      ...this.movie()!,
      rating: Number(value),
    }

    this.apiService.sendRating(movie).subscribe({
      next: result => console.log(result),
      error: err => console.error(err),
    });
  }
}
