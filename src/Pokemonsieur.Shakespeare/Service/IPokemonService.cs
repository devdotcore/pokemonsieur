using System.Threading.Tasks;
using Pokemonsieur.Shakespeare.Model;

namespace Pokemonsieur.Shakespeare.Service
{
    /// <summary>
    /// Pokemon Service - Call pokemon.co api to get all pokemon details
    /// Uses PokeApiNet nuget package as wrapper - only for pokemon defined classes for pokemon.co api service
    /// Using Refit HTTP client to keep it real
    /// </summary>  
    public interface IPokemonService
    {
        /// <summary>
        /// Get pokemon species details - only place which some kind of description
        ///   {pokeApi}/pokemon-species/{pokemonName}/
        /// Extract Flavor text by language - configured via AppSetting
        /// </summary>
        /// <param name="pokemonName">Pokemon name or yours, if you are one.</param>
        /// <returns>Pokemon Details with error, if any</returns>
        Task<PokemonDetails> GetPokemonDetailsAsync(string pokemonName);
    }
}