using RickMort.Backend.Api.Abstractions.Interfaces;
using RickMort.Backend.Api.Models;
using System.Text.Json;

namespace RickMort.Backend.Api.Services
{
    public class RickAndMortyApiClient : IRickAndMortyApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public RickAndMortyApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiUrl = "https://rickandmortyapi.com/api/episode";
        }

        public async Task<EpisodeResult> GetEpisodesAsync(int page = 1)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrl}?page={page}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<EpisodeResult>(content);
                }
                else
                {
                    var errorMessage = $"API returned {(int)response.StatusCode} - {response.StatusCode}";
                    throw new Exception(errorMessage);
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to fetch episodes: {ex.Message}";
                throw new Exception(errorMessage);
            }
        }
    }
}
