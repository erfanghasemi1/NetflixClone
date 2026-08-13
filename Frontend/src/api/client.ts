import axios from 'axios';
import { Movie, Genre } from '../types';

// Set to '/api' so requests are routed to Nginx, which proxies to https://netflix:8081/api
const API_BASE_URL = '/api';

const client = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

export const searchMovies = async (
  searchText: string,
  contentType: number,
  minRate: number,
  releaseYear: number | null,
  genresId: number[]
): Promise<Movie[]> => {
  try {
    const params = new URLSearchParams();

    if (searchText) {
      params.append('searchText', searchText);
    }
    
    params.append('contentType', contentType.toString());
    params.append('minRate', minRate.toString());

    if (releaseYear) {
      params.append('releaseYear', releaseYear.toString());
    }

    // Appends multiple `genresId` keys for ASP.NET array binding (e.g., ?genresId=1&genresId=3)
    genresId.forEach((id) => {
      params.append('genresId', id.toString());
    });

    const response = await client.get<Movie[]>('/search', { params });
    return response.data;
  } catch (error) {
    console.error('Error searching movies:', error);
    throw error;
  }
};

export const checkDatabaseStatus = async (): Promise<boolean> => {
  try {
    const response = await client.get<Movie[]>('/search');
    // Returns true if there is data, false if empty []
    return Array.isArray(response.data) && response.data.length > 0;
  } catch (error) {
    console.error('Error checking database status:', error);
    return false;
  }
};

export const importAllData = async (): Promise<void> => {
  try {
    await client.post('/import/all');
  } catch (error) {
    console.error('Error importing data:', error);
    throw error;
  }
};

export const getGenres = async (): Promise<Genre[]> => {
  return [
    { id: 1, name: 'Action', tmdbGenreId: 28, mediaGenres: [] },
    { id: 2, name: 'Adventure', tmdbGenreId: 12, mediaGenres: [] },
    { id: 3, name: 'Animation', tmdbGenreId: 16, mediaGenres: [] },
    { id: 4, name: 'Comedy', tmdbGenreId: 35, mediaGenres: [] },
    { id: 5, name: 'Crime', tmdbGenreId: 80, mediaGenres: [] },
    { id: 6, name: 'Documentary', tmdbGenreId: 99, mediaGenres: [] },
    { id: 7, name: 'Drama', tmdbGenreId: 18, mediaGenres: [] },
    { id: 8, name: 'Family', tmdbGenreId: 10751, mediaGenres: [] },
    { id: 9, name: 'Fantasy', tmdbGenreId: 14, mediaGenres: [] },
    { id: 10, name: 'History', tmdbGenreId: 36, mediaGenres: [] },
    { id: 11, name: 'Horror', tmdbGenreId: 27, mediaGenres: [] },
    { id: 12, name: 'Music', tmdbGenreId: 10402, mediaGenres: [] },
    { id: 13, name: 'Mystery', tmdbGenreId: 9648, mediaGenres: [] },
    { id: 14, name: 'Romance', tmdbGenreId: 10749, mediaGenres: [] },
    { id: 15, name: 'Science Fiction', tmdbGenreId: 878, mediaGenres: [] },
    { id: 16, name: 'TV Movie', tmdbGenreId: 10770, mediaGenres: [] },
    { id: 17, name: 'Thriller', tmdbGenreId: 53, mediaGenres: [] },
    { id: 18, name: 'War', tmdbGenreId: 10752, mediaGenres: [] },
    { id: 19, name: 'Western', tmdbGenreId: 37, mediaGenres: [] },
    { id: 20, name: 'Action & Adventure', tmdbGenreId: 10759, mediaGenres: [] },
    { id: 21, name: 'Kids', tmdbGenreId: 10762, mediaGenres: [] },
    { id: 22, name: 'News', tmdbGenreId: 10763, mediaGenres: [] },
    { id: 23, name: 'Reality', tmdbGenreId: 10764, mediaGenres: [] },
    { id: 24, name: 'Sci-Fi & Fantasy', tmdbGenreId: 10765, mediaGenres: [] },
    { id: 25, name: 'Soap', tmdbGenreId: 10766, mediaGenres: [] },
    { id: 26, name: 'Talk', tmdbGenreId: 10767, mediaGenres: [] },
    { id: 27, name: 'War & Politics', tmdbGenreId: 10768, mediaGenres: [] },
  ];
};