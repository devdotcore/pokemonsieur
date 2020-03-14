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

        [Fact]
        public void IsTranslation_Success()
        {
            //Given

            //When

            //Then
        }
    }
}