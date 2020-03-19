using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
                _logger.LogDebug("Calling {methodName}, Getting translation for {text}", nameof(GetTranslationAsync), text);

                TranslationQueryParams queryParams = new TranslationQueryParams
                {
                    Query = text
                };

                Translation response = await _client.Get(_appSettings.TranslationApi.Type, queryParams);

                _logger.LogInformation("Translation Successful, Returning data");

                return response;
            }
            catch (ValidationApiException validationApiException)
            {
                _logger.LogError(validationApiException, "HttpRequestException occurred while calling translation api - {code} {Details}", (int)validationApiException.StatusCode, validationApiException.Message);
                return GetErrorResponse((int)validationApiException.StatusCode, validationApiException?.Message);
            }
            catch (ApiException exception)
            {
                _logger.LogError(exception, "Exception occurred while calling translation api - {code} {Details}", (int)exception.StatusCode, exception.Message);
                return GetErrorResponse((int)exception.StatusCode, "Api Error");

            }
        }

        /// <summary>
        /// Get Error response for translation service
        /// </summary>
        /// <param name="code">Error Code</param>
        /// <param name="message">Error Message</param>
        /// <returns></returns>
        private Translation GetErrorResponse(int code, string message) => new Translation
        {
            Error = new Error
            {
                Code = code,
                Message = message
            }
        };
    }
}