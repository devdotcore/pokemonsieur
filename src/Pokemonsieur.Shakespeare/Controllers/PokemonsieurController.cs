using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pokemonsieur.Shakespeare.Model;

namespace Pokemonsieur.Shakespeare.Controllers
{
    [ApiController]
    [Produces(contentType: "application/json")]
    [Route("pokemon/")]
    public class PokemonsieurController : ControllerBase
    {

        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<PokemonsieurController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public PokemonsieurController(ILogger<PokemonsieurController> logger)
        {
            _logger = logger;
        }
        
        /// <summary>
        /// Get Pokemon Shakespearian description by passing pokemon name
        /// </summary>
        /// <param name="pokemonName"></param>
        /// <returns></returns>
        [HttpGet("pokemonName")]
        public PokemonShakespeare Get(string pokemonName)
        {
            return new PokemonShakespeare {
                Description = "Pikachu is an electric-type pokémon did introduce in generation i. Pikachu is famous for being the most well-known and recognizable pokémon.",
                Name = pokemonName
            };
        }
    }
}