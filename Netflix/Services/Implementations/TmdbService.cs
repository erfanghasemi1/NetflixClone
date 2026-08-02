using System.Text.Json;
using Microsoft.Extensions.Options;
using Netflix.Configurations;
using Netflix.DTOs.Tmdb;
using Netflix.Enums;
using Netflix.Services.Interfaces;

namespace Netflix.Services.Implementations
{
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

        public Task<GenreResponseDto> GetMovieGenresAsync()
            => GetAsync<GenreResponseDto>("/genre/movie/list");

        public Task<GenreResponseDto> GetTvGenresAsync()
            => GetAsync<GenreResponseDto>("/genre/tv/list");

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