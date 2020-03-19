using Refit;

namespace Pokemonsieur.Shakespeare.Model
{
    /// <summary>
    /// Pokemon Details - Populated via pokeapi.co call
    /// </summary>
    public class PokemonDetails : BaseModel
    {
        /// <summary>
        /// Pokemon Name
        /// </summary>
        /// <value></value>
        public string Name { get; set; }

        /// <summary>
        /// Pokemon Description
        /// </summary>
        /// <value></value>
        public string Description { get; set; }

    }

    public class PokemonQueryParams {}
}