using Netflix.DTOs.Stats;

namespace Netflix.Services.Interfaces
{
    public interface IGetMediaSample
    {
        Task<List<MediaSampleDto>> GetMediaSampleAsync(int count);
    }
}
