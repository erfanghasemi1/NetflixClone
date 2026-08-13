import React from 'react';
import { Movie, Genre } from '../types';
import { MovieCard } from './MovieCard';

interface MovieGridProps {
  movies: Movie[];
  genres: Genre[];
  isLoading: boolean;
  hasSearched: boolean;
}

export const MovieGrid: React.FC<MovieGridProps> = ({ movies, genres, isLoading, hasSearched }) => {
  if (isLoading) {
    return (
      <div className="flex flex-col items-center justify-center py-20">
        <div className="animate-spin mb-4">
          <svg className="w-12 h-12 text-purple-600" fill="none" viewBox="0 0 24 24">
            <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4" />
            <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z" />
          </svg>
        </div>
        <p className="text-slate-600 text-lg">Searching for amazing movies...</p>
      </div>
    );
  }

  if (!hasSearched) {
    return (
      <div className="flex flex-col items-center justify-center py-20">
        <div className="mb-4">
          <svg className="w-16 h-16 text-slate-300" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M14.828 14.828a4 4 0 01-5.656 0M21 12a9 9 0 11-18 0 9 9 0 0118 0zM9 9h.01M15 9h.01" />
          </svg>
        </div>
        <p className="text-slate-500 text-lg">Use the filters above to search for movies</p>
      </div>
    );
  }

  if (movies.length === 0) {
    return (
      <div className="flex flex-col items-center justify-center py-20">
        <div className="mb-4">
          <svg className="w-16 h-16 text-slate-300" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M9.172 16.172a4 4 0 015.656 0M9 10a4 4 0 018 0" />
          </svg>
        </div>
        <p className="text-slate-600 text-lg font-semibold">No movies found</p>
        <p className="text-slate-500">Try adjusting your search filters</p>
      </div>
    );
  }

  return (
    <div>
      <div className="mb-6 flex items-center justify-between">
        <div>
          <h2 className="text-2xl font-bold text-slate-900">Search Results</h2>
          <p className="text-slate-600 mt-1">Found {movies.length} movie{movies.length !== 1 ? 's' : ''}</p>
        </div>
      </div>

      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
        {movies.map((movie) => (
          <MovieCard key={`${movie.mediaTypeId}-${movie.id}`} movie={movie} genres={genres} />
        ))}
      </div>
    </div>
  );
};
