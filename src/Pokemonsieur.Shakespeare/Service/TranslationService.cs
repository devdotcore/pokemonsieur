using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pokemonsieur.Shakespeare.Model;
using Refit;

namespace Pokemonsieur.Shakespeare.Service
{
    /// <summary>
    /// Translation Service - Calls "fun translation API" to translate 
    /// the given text required translation - configured via appsetting
    /// </summary>
    public class TranslationService : ITranslationService
    {
        /// <summary>
        /// The Logger
        /// </summary>
        private readonly ILogger<TranslationService> _logger;

        /// <summary>
        /// For API Settings
        /// </summary>
        private readonly AppSettings _appSettings;

        /// <summary>
        /// Refit Client
        /// </summary>
        private readonly IRestApiClient<Translation, TranslationQueryParams, string> _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationService"/> class.
        /// </summary>
        /// <param name="logger">The Logger</param>
        /// <param name="options">API Configuration</param>
        /// <param name="client">Refit Client</param>
        public TranslationService(ILogger<TranslationService> logger, IOptions<AppSettings> options, IRestApiClient<Translation, TranslationQueryParams, string> client)
        {
            _logger = logger;
            _appSettings = options.Value;
            _client = client;
        }

        /// <summary>
        /// Get the desired translation 
        ///     Configure via appsetting
        /// </summary>
        /// <param name="text">Text to be translated</param>
        /// <returns>Translated text</returns>
        public async Task<Translation> GetTranslationAsync(string text)
        {
            try
            {
                TranslationQueryParams queryParams = new TranslationQueryParams
                {
                    Query = text
                };

                var response = await _client.Get(_appSettings.TranslationApi.Type, queryParams);
                return response;
            }
            catch (ValidationApiException validationApiException)
            {
                _logger.LogError(validationApiException, "HttpRequestException occurred while calling PokeApi - {Details}", validationApiException.Message);
                return new Translation
                {
                    Error = new Error
                    {
                        Code = (int)validationApiException.StatusCode,
                        Message = validationApiException.Message
                    }
                };
            }
            catch (ApiException exception)
            {
                _logger.LogError(exception, "Exception occurred while calling PokeApi - {Details}", exception.Message);
                return new Translation
                {
                    Error = new Error
                    {
                        Code = 500,
                        Message = "System Error"
                    }
                };

            }
        }
    }
}