using Microsoft.Extensions.Logging;
using Moq;
using Pokemonsieur.Shakespeare.Controllers;
using Xunit;

namespace Pokemonsieur.Shakespeare.Tests
{
    public class PokemonsieurController_IsTranslation
    {
        private readonly PokemonsieurController _pokemonsieurController;
        private readonly Mock<ILogger<PokemonsieurController>> _mockLogger;

        public PokemonsieurController_IsTranslation()
        {
            _mockLogger = new Mock<ILogger<PokemonsieurController>>();
            _pokemonsieurController = new PokemonsieurController(_mockLogger.Object);
        }

        [Theory]
        [InlineData("pikachu", "Pikachu is an electric-type pokémon did introduce in generation i. Pikachu is famous for being the most well-known and recognizable pokémon.")]
        public void IsTranslation_Success(string pokemonName, string expectedTranslation)
        {

            var output = _pokemonsieurController.Get(pokemonName);
            
            Assert.Equal(expectedTranslation, output.Description);
            Assert.Equal(pokemonName, output.Name);
        }


    }
}