using System.Text.Json;
using Microsoft.Extensions.Options;
using Netflix.Configurations;
using Netflix.DTOs.Tmdb;
using Netflix.Enums;
using Netflix.Services.Interfaces;

namespace Netflix.Services.Implementations
{

    /// <summary>
    /// A sealed service responsible for communicating with the TMDB API, providing methods 
    /// to retrieve genre lists and paginated media results for both movies and TV series,
    /// using configurable API credentials, sorting strategies, and minimum vote count filters.
    /// </summary>
    public sealed class TmdbService : ITmdbService
    {
        private readonly HttpClient _httpClient;
        private readonly TmdbOptions _tmdbOptions;
        private readonly ImportOptions _importOptions;

        public TmdbService(
            HttpClient httpClient,
            IOptions<TmdbOptions> tmdbOptions,
            IOptions<ImportOptions> importOptions)
        {
            _httpClient = httpClient;
            _tmdbOptions = tmdbOptions.Value;
            _importOptions = importOptions.Value;
        }

        /// <summary>
        /// Retrieves the complete list of movie genres available on the TMDB API.
        /// </summary>

        public Task<GenreResponseDto> GetMovieGenresAsync()
            => GetAsync<GenreResponseDto>("/genre/movie/list");

        /// <summary>
        /// Retrieves the complete list of TV series genres available on the TMDB API.
        /// </summary>
        public Task<GenreResponseDto> GetTvGenresAsync()
            => GetAsync<GenreResponseDto>("/genre/tv/list");

        /// <summary>
        /// Retrieves a paginated list of movies filtered by a specific genre, sorted by the 
        /// given sort type, and limited to entries meeting the configured minimum vote count.
        /// </summary>
        public Task<MediaResponseDto> GetMoviesByGenreAsync(
            int genreId,
            TmdbSortType sortType,
            int page)
        {
            var query =
                $"/discover/movie" +
                $"?with_genres={genreId}" +
                $"&sort_by={GetSortBy(sortType, isTv: false)}" +
                $"&vote_count.gte={_importOptions.MinimumVoteCount}" +
                $"&page={page}";

            return GetAsync<MediaResponseDto>(query);
        }

        /// <summary>
        /// Retrieves a paginated list of TV series filtered by a specific genre, sorted by the 
        /// given sort type, and limited to entries meeting the configured minimum vote count.
        /// </summary>

        public Task<MediaResponseDto> GetTvSeriesByGenreAsync(
            int genreId,
            TmdbSortType sortType,
            int page)
        {
            var query =
                $"/discover/tv" +
                $"?with_genres={genreId}" +
                $"&sort_by={GetSortBy(sortType, isTv: true)}" +
                $"&vote_count.gte={_importOptions.MinimumVoteCount}" +
                $"&page={page}";

            return GetAsync<MediaResponseDto>(query);
        }

        /// <summary>
        /// Retrieves a paginated list of discovered movies sorted by the given sort type 
        /// and limited to entries meeting the configured minimum vote count, without genre filtering.
        /// </summary>

        public Task<MediaResponseDto> DiscoverMoviesAsync(
            TmdbSortType sortType,
            int page)
        {
            var query =
                $"/discover/movie" +
                $"?sort_by={GetSortBy(sortType, isTv: false)}" +
                $"&vote_count.gte={_importOptions.MinimumVoteCount}" +
                $"&page={page}";

            return GetAsync<MediaResponseDto>(query);
        }

        /// <summary>
        /// Retrieves a paginated list of discovered TV series sorted by the given sort type 
        /// and limited to entries meeting the configured minimum vote count, without genre filtering.
        /// </summary>

        public Task<MediaResponseDto> DiscoverTvSeriesAsync(
            TmdbSortType sortType,
            int page)
        {
            var query =
                $"/discover/tv" +
                $"?sort_by={GetSortBy(sortType, isTv: true)}" +
                $"&vote_count.gte={_importOptions.MinimumVoteCount}" +
                $"&page={page}";

            return GetAsync<MediaResponseDto>(query);
        }

        /// <summary>
        /// Constructs the full TMDB API request URL by appending the API key to the given 
        /// relative URL, sends an HTTP GET request, deserializes the JSON response stream 
        /// into the specified type, and throws an exception if deserialization fails.
        /// </summary>

        private async Task<T> GetAsync<T>(string relativeUrl)
        {
            // Always append the API key
            var separator = relativeUrl.Contains('?') ? "&" : "?";
            var url = $"{_tmdbOptions.BaseUrl}{relativeUrl}{separator}api_key={_tmdbOptions.ApiKey}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync();

            var result = await JsonSerializer.DeserializeAsync<T>(
                stream,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (result is null)
                throw new Exception("Failed to deserialize TMDB response.");

            return result;
        }

        /// <summary>
        /// Converts a <see cref="TmdbSortType"/> enum value into the corresponding TMDB API 
        /// sort parameter string, applying TV-specific date sorting when applicable and 
        /// defaulting to popularity descending for unrecognized values.
        /// </summary>

        private static string GetSortBy(TmdbSortType sortType, bool isTv)
        {
            return sortType switch
            {
                TmdbSortType.Popularity => "popularity.desc",
                TmdbSortType.TopRated => "vote_average.desc",
                TmdbSortType.MostVoted => "vote_count.desc",
                TmdbSortType.Newest => isTv
                                            ? "first_air_date.desc"
                                            : "primary_release_date.desc",
                _ => "popularity.desc"
            };
        }
    }
}