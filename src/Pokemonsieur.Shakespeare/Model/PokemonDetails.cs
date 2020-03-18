using Refit;

namespace Pokemonsieur.Shakespeare.Model
{
    /// <summary>
    /// Pokemon Details - Populated via pokeapi.co call
    /// </summary>
    public class PokemonDetails
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


        /// <summary>
        /// If any error while getting the pokemon details
        /// </summary>
        /// <value></value>
        public Error Error { get; set; }

    }

    public class PokemonQueryParams {}
}