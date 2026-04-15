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


  canRate = signal<boolean>(false);

  apiService: ApiService = inject(ApiService);
  featureService: FeatureService = inject(FeatureService);

  async ngOnInit() {
    this.apiService.getAverageRating(this.movie()!.id).subscribe({
      next: result => this.rating.set(result.average),
      error: err => console.error(err),
    });

    // 2. Set the signal value
    const enabled = await this.featureService.isRatingEnabled();
    this.canRate.set(enabled);
    console.log(this.canRate);
  }

  sendRatingRequest(value: string) {
    const score = Number(value);

    this.rating.set(score);

    this.apiService.sendRating(this.movie()!.id, score).subscribe({
      next: result => console.log(result),
      error: err => console.error(err),
    });
  }

  displayRating() {
    return Math.round(this.rating() / 2);
  }

  rate(star: number) {
    const value = star * 2;
    this.sendRatingRequest(value.toString());
  }
}
