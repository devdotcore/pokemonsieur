using System.Text.Json.Serialization;

namespace Pokemonsieur.Shakespeare.Model
{
    public class Error
    {
        /// <summary>
        /// Error Code
        /// </summary>
        /// <value></value>
        [JsonPropertyName("code")]
        public int Code { get; set; }

        /// <summary>
        /// Error Description
        /// </summary>
        /// <value></value>
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}