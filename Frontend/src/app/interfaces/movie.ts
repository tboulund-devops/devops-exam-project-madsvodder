export interface Movie {
  id: number;
  title: string;
  description: string;
  year: number;
  rating: number;
  posterUrl: string;
}

export type CreateMovieDto = Omit<Movie, 'id' | 'rating'>;
