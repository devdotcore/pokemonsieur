using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Pokemonsieur.Shakespeare.Model;
using Pokemonsieur.Shakespeare.Service;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Pokemonsieur.Shakespeare.Controllers
{
    [ApiController]
    [Produces(contentType: "application/json")]
    [Route("pokemon/")]
    public class PokemonsieurController : ControllerBase
    {

        /// <summary>
        /// The Logger
        /// </summary>
        private readonly ILogger<PokemonsieurController> _logger;

        /// <summary>
        /// Pokemonsieur Service
        /// </summary>
        private readonly IPokemonsieurService _pokemonsieurService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PokemonsieurController"/> class.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="pokemonsieurService">Pokemonsieur Service</param>
        public PokemonsieurController(ILogger<PokemonsieurController> logger, IPokemonsieurService pokemonsieurService)
        {
            _logger = logger;
            _pokemonsieurService = pokemonsieurService;
        }

        /// <summary>
        /// Get Pokemon Shakespearian description by passing pokemon name
        ///     - Call PokeAPI Service for Pokemon Details
        ///     - Call Translation Service for translation
        /// </summary>
        /// <param name="pokemonName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{pokemonName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Model.Pokemonsieur>> GetAsync([FromRoute]string pokemonName)
        {
            try
            {
                if (!Regex.IsMatch(pokemonName, @"^[a-zA-Z]+$"))
                {
                    _logger.LogError("Invalid input - {name}", pokemonName);
                    return StatusCode(StatusCodes.Status400BadRequest, _pokemonsieurService.GetErrorDetails(StatusCodes.Status400BadRequest));
                }

                Model.Pokemonsieur pokemonsieur = await _pokemonsieurService.GetDetailsAndTranslateAsync(pokemonName);

                if (pokemonsieur.Error is null)
                {
                    return pokemonsieur;
                }

                return StatusCode(pokemonsieur.Error.Code, pokemonsieur.Error);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Application error, check exception details.");
                return StatusCode(StatusCodes.Status500InternalServerError, _pokemonsieurService.GetErrorDetails(StatusCodes.Status500InternalServerError));
            }
        }
    }
}