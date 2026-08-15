namespace Netflix.Configurations
{
    /// <summary>
    /// Represents the configuration options for connecting to the TMDB API.
    /// Contains the API key and base URL required for making authenticated API calls.
    /// </summary>
    /// <remarks>
    /// This class is typically bound to the <c>Tmdb</c> section in appsettings.json.
    /// </remarks>
    public class TmdbOptions
    {
        public const string SectionName = "Tmdb";
        public string ApiKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
    }
}
