using System.Text.Json.Serialization;

namespace Pokemonsieur.Shakespeare.Model
{
    public class BaseModel
    {
        [JsonPropertyName("error")]
        public Error Error { get; set; }
    }
}