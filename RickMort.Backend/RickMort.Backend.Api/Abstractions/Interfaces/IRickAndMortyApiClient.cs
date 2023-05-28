using RickMort.Backend.Api.Models;

namespace RickMort.Backend.Api.Abstractions.Interfaces
{
    public interface IRickAndMortyApiClient
    {
        Task<EpisodeResult> GetEpisodesAsync(int page);
    }
}
