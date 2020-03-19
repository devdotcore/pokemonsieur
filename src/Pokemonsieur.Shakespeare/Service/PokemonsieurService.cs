using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Pokemonsieur.Shakespeare.Model;

namespace Pokemonsieur.Shakespeare.Service
{
    public class PokemonsieurService : IPokemonsieurService
    {
        /// <summary>
        /// The Logger
        /// </summary>
        private readonly ILogger<PokemonsieurService> _logger;

        /// <summary>
        /// Translation Service
        /// </summary>
        private readonly ITranslationService _translationService;

        /// <summary>
        /// Pokemon Service
        /// </summary>
        private readonly IPokemonService _pokemonService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PokemonsieurService"/> class.
        /// </summary>
        /// <param name="logger">Logging</param>
        /// <param name="translationService">Translation Service</param>
        /// <param name="pokemonService">PokemonService</param>
        public PokemonsieurService(ILogger<PokemonsieurService> logger, ITranslationService translationService, IPokemonService pokemonService)
        {
            _logger = logger;
            _translationService = translationService;
            _pokemonService = pokemonService;
        }

        /// <summary>
        /// Get Pokemon details and translate as required
        /// </summary>
        /// <param name="name">Pokemon Name</param>
        /// <returns></returns>
        public async Task<Model.Pokemonsieur> GetDetailsAndTranslateAsync(string name)
        {
            _logger.LogInformation("Calling PokeAPI...");

            var error = new Error();

            PokemonDetails pokemonDetails = await _pokemonService.GetPokemonDetailsAsync(name);

            if (pokemonDetails.Error is null)
            {
                _logger.LogInformation("Calling TranslationAPI...");

                Translation translation = await _translationService.GetTranslationAsync(pokemonDetails.Description);

                if (translation.Error is null)
                {
                    return new Model.Pokemonsieur
                    {
                        Name = pokemonDetails.Name,
                        Description = translation.Contents.Translated
                    };
                }

                error = translation.Error;
            } 
            else
                error = pokemonDetails.Error;
            
            return new Model.Pokemonsieur
            {
                Error = GetErrorDetails(error.Code)
            };

        }

        /// <summary>
        /// Get Pokemon Error Details
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public Error GetErrorDetails(int statusCode)
        {
            Error errorDetails = new Error
            {
                Code = statusCode,
                Message = "Ooooops! Sooomeethhiingg Weennntt Wrrroonngg!!!!! Pllleaassee trrry aaaggaaainn!!"
            };

            switch (statusCode)
            {
                case StatusCodes.Status404NotFound:
                    errorDetails.Message = "The Pokemon you are looking does not exist in current world or any we are aware of.";
                    return errorDetails;
                case StatusCodes.Status400BadRequest:
                    errorDetails.Message = "That's a weird pokemon, never heard of it. Tell us more..";
                    return errorDetails;
                default:
                    return errorDetails;
            }
        }
    }
}