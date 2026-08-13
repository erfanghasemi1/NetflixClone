import { useEffect, useState } from 'react';
import { Header, SearchFilters, MovieGrid, LoadingOverlay } from './components';
import { checkDatabaseStatus, importAllData, searchMovies, getGenres } from './api/client';
import { Movie, Genre } from './types';

export default function App() {
  const [movies, setMovies] = useState<Movie[]>([]);
  const [genres, setGenres] = useState<Genre[]>([]);
  const [isInitializing, setIsInitializing] = useState(true);
  const [isSearching, setIsSearching] = useState(false);
  const [hasSearched, setHasSearched] = useState(false);
  const [error, setError] = useState<string | null>(null);

  // Initialize app on mount
  useEffect(() => {
    const initializeApp = async () => {
      try {
        setIsInitializing(true);
        
        // Load genres first
        const genresData = await getGenres();
        setGenres(genresData);

        // Check if database has data
        const hasDatabaseData = await checkDatabaseStatus();
        
        if (!hasDatabaseData) {
          // Database is empty, import data
          console.log('Database is empty, importing data...');
          await importAllData();
          console.log('Data imported successfully');
        }
        
        setIsInitializing(false);
      } catch (err) {
        console.error('Initialization error:', err);
        setError('Failed to initialize application. Please check your connection and try again.');
        setIsInitializing(false);
      }
    };

    initializeApp();
  }, []);

  const handleSearch = async (
    searchText: string,
    contentType: number,
    minRate: number,
    releaseYear: number | null,
    genresId: number[]
  ) => {
    try {
      setIsSearching(true);
      setError(null);
      setHasSearched(true);

      const results = await searchMovies(searchText, contentType, minRate, releaseYear, genresId);
      setMovies(results);
    } catch (err) {
      console.error('Search error:', err);
      setError('Failed to search movies. Please try again.');
      setMovies([]);
    } finally {
      setIsSearching(false);
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-slate-50 via-white to-purple-50">
      {isInitializing && <LoadingOverlay message="Initializing application and checking database..." />}
      
      <Header isLoading={isInitializing} />

      <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {/* Error Message */}
        {error && (
          <div className="mb-6 p-4 bg-red-50 border-l-4 border-red-500 rounded-lg flex items-start gap-3">
            <svg className="w-5 h-5 text-red-500 flex-shrink-0 mt-0.5" fill="currentColor" viewBox="0 0 20 20">
              <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clipRule="evenodd" />
            </svg>
            <div>
              <h3 className="font-semibold text-red-800">Error</h3>
              <p className="text-red-700 text-sm">{error}</p>
            </div>
          </div>
        )}

        {/* Search Filters */}
        {!isInitializing && (
          <SearchFilters
            onSearch={handleSearch}
            genres={genres}
            isLoading={isSearching}
          />
        )}

        {/* Movie Grid */}
        {!isInitializing && (
          <MovieGrid
            movies={movies}
            genres={genres}
            isLoading={isSearching}
            hasSearched={hasSearched}
          />
        )}
      </main>

      {/* Footer */}
      <footer className="bg-slate-900 text-slate-400 py-8 mt-16">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center">
            <p className="text-sm">
              Made with <span className="text-red-500">❤</span> using React, Vite & Tailwind CSS
            </p>
            <p className="text-xs mt-2">Movie data provided by TMDB</p>
          </div>
        </div>
      </footer>
    </div>
  );
}
