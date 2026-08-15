using Netflix.DTOs.Search;
using Netflix.Entities;

namespace Netflix.Services.Interfaces
{
    public interface ISearchMedia
    {
        Task<List<SearchResponseDto>> SearchAsync(SearchRequestDto request);
        IOrderedQueryable<Media> SortMediaAsync(IQueryable<Media> result , SearchRequestDto request);
    }
}
