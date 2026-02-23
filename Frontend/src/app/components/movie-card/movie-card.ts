import {Component, input, OnInit} from '@angular/core';
import {Movie} from '../../interfaces/movie';

@Component({
  selector: 'app-movie-card',
  imports: [],
  templateUrl: './movie-card.html',
  styleUrl: './movie-card.css',
  standalone: true,
})
export class MovieCard implements OnInit {
  movie = input<Movie>();

  ngOnInit() {
    console.log(this.movie);
  }
}
