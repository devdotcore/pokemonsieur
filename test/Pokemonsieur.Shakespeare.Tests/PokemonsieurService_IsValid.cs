using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Pokemonsieur.Shakespeare.Model;
using Pokemonsieur.Shakespeare.Service;
using Xunit;

namespace Pokemonsieur.Shakespeare.Tests
{
    public class PokemonsieurService_IsValid
    {
        private IPokemonsieurService _pokemonsieurService;

        private readonly Mock<ILogger<PokemonsieurService>> _mockLogger = new Mock<ILogger<PokemonsieurService>>();

        private readonly Mock<IPokemonService> _mockPokemonService = new Mock<IPokemonService>();

        private readonly Mock<ITranslationService> _mockTranslationService = new Mock<ITranslationService>();


        public PokemonsieurService_IsValid()
        {

        }

        [Fact]
        public async void Valid_PokemonName_ReturnTranslatedText()
        {
            //Arrange
            _mockPokemonService.Setup(p => p.GetPokemonDetailsAsync(It.IsAny<string>())).Returns(Task.FromResult(new PokemonDetails
            {
                Name = TestData._mockName,
                Description = TestData._mockDetails
            }));

            _mockTranslationService.Setup(p => p.GetTranslationAsync(It.IsAny<string>())).Returns(Task.FromResult(new Translation
            {
                Contents = new Contents
                {
                    Translated = TestData._mockTranslation
                }
            }));

            _pokemonsieurService = new PokemonsieurService(_mockLogger.Object, _mockTranslationService.Object, _mockPokemonService.Object);


            //Act
            var output = await _pokemonsieurService.GetDetailsAndTranslateAsync(TestData._mockName);

            //Assert
            Assert.Null(output.Error);
            Assert.Equal(TestData._mockTranslation, output.Description);
        }

        [Fact]
        public async void Valid_PokemonName_ReturnPokeApiError()
        {
            //Arrange
            _mockPokemonService.Setup(p => p.GetPokemonDetailsAsync(It.IsAny<string>())).Returns(Task.FromResult(new PokemonDetails
            {
                Error = new Error
                {
                    Code = 500
                }
            }));

            _pokemonsieurService = new PokemonsieurService(_mockLogger.Object, _mockTranslationService.Object, _mockPokemonService.Object);

            //Act
            var output = await _pokemonsieurService.GetDetailsAndTranslateAsync(TestData._mockName);

            //Assert
            Assert.NotNull(output.Error);
            Assert.Equal(500, output.Error.Code);
        }

        [Fact]
        public async void Valid_PokemonName_ReturnTranslationApiError()
        {
            //Arrange
            _mockPokemonService.Setup(p => p.GetPokemonDetailsAsync(It.IsAny<string>())).Returns(Task.FromResult(new PokemonDetails
            {
                Name = TestData._mockName,
                Description = TestData._mockDetails
            }));

            _mockTranslationService.Setup(p => p.GetTranslationAsync(It.IsAny<string>())).Returns(Task.FromResult(new Translation
            {
                Error = new Error
                {
                    Code = 400
                }
            }));

            _pokemonsieurService = new PokemonsieurService(_mockLogger.Object, _mockTranslationService.Object, _mockPokemonService.Object);

            //Act
            var output = await _pokemonsieurService.GetDetailsAndTranslateAsync(TestData._mockName);

            //Assert
            Assert.NotNull(output.Error);
            Assert.Equal(400, output.Error.Code);
        }

    }
}