using Microsoft.AspNetCore.Mvc;
using RickMort.Backend.Api.Abstractions.Interfaces;

namespace RickMort.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EpisodesController : ControllerBase
    {
        private readonly IRickAndMortyApiClient _rickAndMortyApiClient;

        public EpisodesController(IRickAndMortyApiClient rickAndMortyApiClient)
        {
            _rickAndMortyApiClient = rickAndMortyApiClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetEpisodes(int page = 1)
        {
            try
            {
                var episodes = await _rickAndMortyApiClient.GetEpisodesAsync(page);
                return Ok(episodes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }

}
