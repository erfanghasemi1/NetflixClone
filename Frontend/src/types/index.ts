export interface Movie {
  id: number;
  mediaTypeId: number;
  title: string;
  overview: string;
  posterPath: string;
  backdropPath: string;
  releaseDate: string;
  voteAverage: number;
  voteCount: number;
  popularity: number;
  genresId: number[];
}

export interface Genre {
  id: number;
  name: string;
  tmdbGenreId: number;
  mediaGenres: any[];
}

export interface SearchParams {
  searchText: string;
  contentType: number; // 0 = All, 1 = Movie, 2 = TV Series
  minRate: number;
  releaseYear: number | null;
  genresId: number[];
}
