using System.Text.Json.Serialization;

namespace Pokemonsieur.Shakespeare.Model
{

    public class Pokemonsieur : BaseModel
    {
        /// <summary>
        /// Pokemon Name
        /// </summary>
        /// <value></value>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Pokemon Description
        /// </summary>
        /// <value></value>
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}