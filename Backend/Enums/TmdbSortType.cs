namespace Netflix.Enums
{

    /// <summary>
    /// Defines the available sorting criteria used when querying the TMDB API, 
    /// allowing data to be ordered by popularity, average rating, total vote count, 
    /// or the most recent release date.
    /// </summary>


    public enum TmdbSortType
    {
        Popularity,
        TopRated,
        MostVoted,
        Newest
    }
}
