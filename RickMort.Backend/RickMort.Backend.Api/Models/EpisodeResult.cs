using System.Text.Json.Serialization;

namespace RickMort.Backend.Api.Models
{
    public class EpisodeResult
    {
        [JsonPropertyName("info")]
        public Info Info { get; set; }

        [JsonPropertyName("results")]
        public List<Episode> Results { get; set; }
    }

}
