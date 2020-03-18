using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PokeApiNet;
using Pokemonsieur.Shakespeare.Model;
using Refit;

namespace Pokemonsieur.Shakespeare.Service
{
    /// <summary>
    /// Pokemon Service - Call pokemon.co api to get all pokemon details
    /// Uses PokeApiNet nuget package as wrapper - only for pokemon defined classes for pokemon.co api service
    /// Using Refit HTTP client to keep it real
    /// </summary>
    public class PokemonService : IPokemonService
    {
        /// <summary>
        /// Usual log the logger
        /// </summary>
        public readonly ILogger<PokemonService> _logger;

        /// <summary>
        /// Refit Client to call pokeApi service, configured in middleware
        /// </summary>
        public readonly IRestApiClient<PokemonSpecies, PokemonQueryParams, string> _client;

        /// <summary>
        /// Appsetting - for PokeAPI configurations 
        /// </summary>
        private readonly AppSettings _appSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="PokemonService"/> class.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="client">Refit HTTP Client</param>
        /// <param name="options">AppSettting - API Configurations</param>
        public PokemonService(ILogger<PokemonService> logger, IRestApiClient<PokemonSpecies, PokemonQueryParams, string> client, IOptions<AppSettings> options)
        {
            _logger = logger;
            _client = client;
            _appSettings = options.Value;
        }

        /// <summary>
        /// Get pokemon species details - only place which some kind of description
        ///   {pokeApi}/pokemon-species/{pokemonName}/
        /// Extract Flavor text by language - configured via AppSetting
        /// </summary>
        /// <param name="pokemonName">Pokemon name or yours, if you are one.</param>
        /// <returns>Pokemon Details with error, if any</returns>
        public async Task<PokemonDetails> GetPokemonDetailsAsync(string pokemonName)
        {
            try
            {
                _logger.LogDebug("Calling {methodName}, Getting details for {pokemon}", nameof(GetPokemonDetailsAsync), pokemonName);

                PokemonSpecies species = await _client.Get($"{_appSettings.PokeApi.Key}/{pokemonName}", new PokemonQueryParams());

                if (!(species is null) && species.FlavorTextEntries?.Count > 0)
                {
                    _logger.LogInformation("{methodName} call successfully returned species data", nameof(GetPokemonDetailsAsync));

                    PokemonSpeciesFlavorTexts flavorTexts = species.FlavorTextEntries.Where(x => x.Language.Name == _appSettings.PokeApi.DefaultLanguage).FirstOrDefault();

                    if (flavorTexts is null || string.IsNullOrEmpty(flavorTexts?.FlavorText))
                    {
                        _logger.LogError("Error in {methodName} - Empty Flavor text for language {lang}", nameof(GetPokemonDetailsAsync), _appSettings.PokeApi.DefaultLanguage);
                        return GetErrorResponse(500, "API Response Error");
                    }

                    return new PokemonDetails
                    {
                        Name = species.Name,
                        Description = flavorTexts.FlavorText.Trim().Replace('\n', ' ')
                    };
                }
                else
                {
                    _logger.LogError("Error in {methodName} - Null/Empty response from PokeAPI", nameof(GetPokemonDetailsAsync));
                    return GetErrorResponse(500, "API Response Error");
                }
            }
            catch (ValidationApiException validationApiException)
            {
                _logger.LogError(validationApiException, "HttpRequestException occurred while calling PokeApi - {Details}", validationApiException.Message);
                return GetErrorResponse((int)validationApiException.StatusCode, validationApiException?.Message);
            }
            catch (ApiException exception)
            {
                _logger.LogError(exception, "Exception occurred while calling PokeApi - {Details}", exception?.Message);
                return GetErrorResponse(500, exception?.Message);
            }
        }

        /// <summary>
        /// Get Error response for PokeMone Details
        /// </summary>
        /// <param name="code">Error Code</param>
        /// <param name="message">Error Message</param>
        /// <returns></returns>
        private PokemonDetails GetErrorResponse(int code, string message) => new PokemonDetails
        {
            Error = new Error
            {
                Code = code,
                Message = message
            }
        };


    }
}