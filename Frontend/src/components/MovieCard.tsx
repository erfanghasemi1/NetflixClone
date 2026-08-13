import React, { useState } from 'react';
import { Movie, Genre } from '../types';

interface MovieCardProps {
  movie: Movie;
  genres: Genre[];
}

const TMDB_IMAGE_BASE = 'https://image.tmdb.org/t/p/w500';

export const MovieCard: React.FC<MovieCardProps> = ({ movie, genres }) => {
  const [showDetails, setShowDetails] = useState(false);

  const movieGenres = movie.genresId
    .map((id) => genres.find((g) => g.id === id)?.name)
    .filter(Boolean);

  const year = new Date(movie.releaseDate).getFullYear();
  const mediaType = movie.mediaTypeId === 1 ? 'Movie' : 'TV Series';

  const getRatingColor = (rating: number) => {
    if (rating >= 8) return 'text-green-600 bg-green-100';
    if (rating >= 6) return 'text-yellow-600 bg-yellow-100';
    if (rating >= 4) return 'text-orange-600 bg-orange-100';
    return 'text-red-600 bg-red-100';
  };

  return (
    <div className="group bg-white rounded-xl shadow-md hover:shadow-2xl transition-all duration-300 overflow-hidden h-full flex flex-col">
      {/* Poster Image */}
      <div className="relative h-64 bg-gradient-to-br from-slate-200 to-slate-300 overflow-hidden">
        {movie.posterPath ? (
          <>
            <img
              src={`${TMDB_IMAGE_BASE}${movie.posterPath}`}
              alt={movie.title}
              className="w-full h-full object-cover group-hover:scale-110 transition-transform duration-300"
              onError={(e) => {
                (e.target as HTMLImageElement).src = 'data:image/svg+xml,%3Csvg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 500 750"%3E%3Crect fill="%23e2e8f0" width="500" height="750"/%3E%3Ctext x="50%25" y="50%25" font-size="32" fill="%2364748b" text-anchor="middle" dy=".3em"%3ENo Image%3C/text%3E%3C/svg%3E';
              }}
            />
            <div className="absolute inset-0 bg-gradient-to-t from-black/60 via-transparent to-transparent opacity-0 group-hover:opacity-100 transition-opacity duration-300" />
          </>
        ) : (
          <div className="w-full h-full flex items-center justify-center">
            <svg className="w-16 h-16 text-slate-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z" />
            </svg>
          </div>
        )}

        {/* Rating Badge */}
        <div className={`absolute top-3 right-3 ${getRatingColor(movie.voteAverage)} px-3 py-1 rounded-full text-sm font-bold`}>
          {movie.voteAverage.toFixed(1)}
        </div>

        {/* Media Type Badge */}
        <div className="absolute top-3 left-3 bg-purple-600 text-white px-3 py-1 rounded-full text-xs font-semibold">
          {mediaType}
        </div>
      </div>

      {/* Content */}
      <div className="p-4 flex-1 flex flex-col">
        <h3 className="font-bold text-slate-900 line-clamp-2 mb-2 group-hover:text-purple-600 transition">
          {movie.title}
        </h3>

        <div className="flex items-center justify-between mb-3">
          <span className="text-xs font-semibold text-slate-500">{year}</span>
          <div className="flex items-center gap-1 text-xs text-slate-500">
            <svg className="w-4 h-4 text-yellow-400" fill="currentColor" viewBox="0 0 20 20">
              <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z" />
            </svg>
            {movie.voteCount.toLocaleString()}
          </div>
        </div>

        {/* Genres */}
        {movieGenres.length > 0 && (
          <div className="flex flex-wrap gap-1 mb-3">
            {movieGenres.slice(0, 2).map((genre) => (
              <span key={genre} className="text-xs bg-purple-100 text-purple-700 px-2 py-1 rounded">
                {genre}
              </span>
            ))}
            {movieGenres.length > 2 && (
              <span className="text-xs bg-slate-100 text-slate-600 px-2 py-1 rounded">
                +{movieGenres.length - 2}
              </span>
            )}
          </div>
        )}

        {/* Overview Preview */}
        <p className="text-sm text-slate-600 line-clamp-3 flex-1 mb-4">
          {movie.overview}
        </p>

        {/* Details Button */}
        <button
          onClick={() => setShowDetails(!showDetails)}
          className="text-sm font-semibold text-purple-600 hover:text-purple-700 flex items-center gap-1 transition"
        >
          {showDetails ? 'Hide Details' : 'Read More'}
          <svg className={`w-4 h-4 transition-transform ${showDetails ? 'rotate-180' : ''}`} fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 14l-7 7m0 0l-7-7m7 7V3" />
          </svg>
        </button>

        {/* Full Details */}
        {showDetails && (
          <div className="mt-4 pt-4 border-t border-slate-200 space-y-2">
            <div>
              <p className="text-xs font-semibold text-slate-500 uppercase mb-1">Full Description</p>
              <p className="text-sm text-slate-600">{movie.overview}</p>
            </div>
            <div>
              <p className="text-xs font-semibold text-slate-500 uppercase mb-1">All Genres</p>
              <div className="flex flex-wrap gap-1">
                {movieGenres.map((genre) => (
                  <span key={genre} className="text-xs bg-purple-100 text-purple-700 px-2 py-1 rounded">
                    {genre}
                  </span>
                ))}
              </div>
            </div>
            <div className="grid grid-cols-2 gap-2 pt-2">
              <div>
                <p className="text-xs font-semibold text-slate-500 uppercase">Popularity</p>
                <p className="text-sm font-bold text-slate-700">{movie.popularity.toFixed(1)}</p>
              </div>
              <div>
                <p className="text-xs font-semibold text-slate-500 uppercase">Vote Count</p>
                <p className="text-sm font-bold text-slate-700">{movie.voteCount.toLocaleString()}</p>
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};
