import React, { useState } from 'react';
import { Genre } from '../types';

interface SearchFiltersProps {
  onSearch: (
    searchText: string,
    contentType: number,
    minRate: number,
    releaseYear: number | null,
    genresId: number[]
  ) => void;
  genres: Genre[];
  isLoading: boolean;
}

export const SearchFilters: React.FC<SearchFiltersProps> = ({ onSearch, genres, isLoading }) => {
  const [searchText, setSearchText] = useState('');
  const [contentType, setContentType] = useState(0);
  const [minRate, setMinRate] = useState(0);
  const [releaseYear, setReleaseYear] = useState('');
  const [selectedGenres, setSelectedGenres] = useState<number[]>([]);
  const [showGenreDropdown, setShowGenreDropdown] = useState(false);

  const currentYear = new Date().getFullYear();
  const years = Array.from({ length: 80 }, (_, i) => currentYear - i);

  const handleGenreToggle = (genreId: number) => {
    setSelectedGenres((prev) =>
      prev.includes(genreId) ? prev.filter((id) => id !== genreId) : [...prev, genreId]
    );
  };

  const handleSearch = (e: React.FormEvent) => {
    e.preventDefault();
    onSearch(
      searchText,
      contentType,
      minRate,
      releaseYear ? parseInt(releaseYear) : null,
      selectedGenres
    );
  };

  const handleClear = () => {
    setSearchText('');
    setContentType(0);
    setMinRate(0);
    setReleaseYear('');
    setSelectedGenres([]);
  };

  return (
    <div className="bg-white rounded-2xl shadow-lg p-6 md:p-8 mb-8">
      <h2 className="text-2xl font-bold text-slate-900 mb-6">Find Your Next Movie</h2>
      
      <form onSubmit={handleSearch} className="space-y-6">
        {/* Search Text Input */}
        <div>
          <label className="block text-sm font-semibold text-slate-700 mb-2">
            Search by Title
          </label>
          <div className="relative">
            <input
              type="text"
              value={searchText}
              onChange={(e) => setSearchText(e.target.value)}
              placeholder="Enter movie or TV series name..."
              className="w-full px-4 py-3 border border-slate-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-transparent transition"
              disabled={isLoading}
            />
            <svg className="absolute right-3 top-3 w-5 h-5 text-slate-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
            </svg>
          </div>
        </div>

        {/* Grid for filters */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {/* Content Type */}
          <div>
            <label className="block text-sm font-semibold text-slate-700 mb-2">
              Content Type
            </label>
            <select
              value={contentType}
              onChange={(e) => setContentType(parseInt(e.target.value))}
              className="w-full px-4 py-3 border border-slate-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-transparent transition"
              disabled={isLoading}
            >
              <option value={0}>All Content</option>
              <option value={1}>Movies Only</option>
              <option value={2}>TV Series Only</option>
            </select>
          </div>

          {/* Minimum Rating */}
          <div>
            <label className="block text-sm font-semibold text-slate-700 mb-2">
              Minimum Rating: {minRate.toFixed(1)}
            </label>
            <input
              type="range"
              min="0"
              max="10"
              step="0.5"
              value={minRate}
              onChange={(e) => setMinRate(parseFloat(e.target.value))}
              className="w-full h-2 bg-gradient-to-r from-yellow-400 to-red-500 rounded-lg appearance-none cursor-pointer"
              disabled={isLoading}
            />
            <div className="flex justify-between text-xs text-slate-500 mt-1">
              <span>0</span>
              <span>10</span>
            </div>
          </div>

          {/* Release Year */}
          <div>
            <label className="block text-sm font-semibold text-slate-700 mb-2">
              Release Year
            </label>
            <select
              value={releaseYear}
              onChange={(e) => setReleaseYear(e.target.value)}
              className="w-full px-4 py-3 border border-slate-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-transparent transition"
              disabled={isLoading}
            >
              <option value="">Any Year</option>
              {years.map((year) => (
                <option key={year} value={year}>
                  {year}
                </option>
              ))}
            </select>
          </div>
        </div>

        {/* Genres */}
        <div>
          <label className="block text-sm font-semibold text-slate-700 mb-2">
            Genres ({selectedGenres.length} selected)
          </label>
          <div className="relative">
            <button
              type="button"
              onClick={() => setShowGenreDropdown(!showGenreDropdown)}
              disabled={isLoading}
              className="w-full px-4 py-3 border border-slate-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-transparent transition text-left flex justify-between items-center bg-white hover:bg-slate-50"
            >
              <span className="text-slate-700">
                {selectedGenres.length > 0 ? `${selectedGenres.length} genre(s) selected` : 'Select genres...'}
              </span>
              <svg
                className={`w-5 h-5 text-slate-400 transition-transform ${showGenreDropdown ? 'rotate-180' : ''}`}
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 14l-7 7m0 0l-7-7m7 7V3" />
              </svg>
            </button>

            {showGenreDropdown && (
              <div className="absolute top-full left-0 right-0 mt-2 bg-white border border-slate-300 rounded-lg shadow-xl z-10 max-h-64 overflow-y-auto">
                <div className="p-4 grid grid-cols-2 gap-3">
                  {genres.map((genre) => (
                    <label key={genre.id} className="flex items-center gap-2 cursor-pointer hover:bg-purple-50 p-2 rounded transition">
                      <input
                        type="checkbox"
                        checked={selectedGenres.includes(genre.id)}
                        onChange={() => handleGenreToggle(genre.id)}
                        className="w-4 h-4 rounded border-slate-300 text-purple-600 focus:ring-purple-500 cursor-pointer"
                      />
                      <span className="text-sm text-slate-700">{genre.name}</span>
                    </label>
                  ))}
                </div>
              </div>
            )}
          </div>

          {/* Selected Genres Display */}
          {selectedGenres.length > 0 && (
            <div className="flex flex-wrap gap-2 mt-3">
              {selectedGenres.map((genreId) => {
                const genre = genres.find((g) => g.id === genreId);
                return (
                  <div
                    key={genreId}
                    className="inline-flex items-center gap-2 bg-purple-100 text-purple-700 px-3 py-1 rounded-full text-sm"
                  >
                    {genre?.name}
                    <button
                      type="button"
                      onClick={() => handleGenreToggle(genreId)}
                      className="hover:text-purple-900 font-bold"
                    >
                      ×
                    </button>
                  </div>
                );
              })}
            </div>
          )}
        </div>

        {/* Action Buttons */}
        <div className="flex gap-3 pt-4">
          <button
            type="submit"
            disabled={isLoading}
            className="flex-1 bg-gradient-to-r from-purple-600 to-pink-600 hover:from-purple-700 hover:to-pink-700 disabled:from-slate-400 disabled:to-slate-400 text-white font-semibold py-3 px-6 rounded-lg transition transform hover:scale-105 disabled:hover:scale-100 flex items-center justify-center gap-2"
          >
            {isLoading ? (
              <>
                <svg className="animate-spin w-5 h-5" fill="none" viewBox="0 0 24 24">
                  <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4" />
                  <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z" />
                </svg>
                Searching...
              </>
            ) : (
              <>
                <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
                </svg>
                Search Movies
              </>
            )}
          </button>
          <button
            type="button"
            onClick={handleClear}
            disabled={isLoading}
            className="bg-slate-200 hover:bg-slate-300 disabled:bg-slate-100 text-slate-700 font-semibold py-3 px-6 rounded-lg transition"
          >
            Clear
          </button>
        </div>
      </form>
    </div>
  );
};
