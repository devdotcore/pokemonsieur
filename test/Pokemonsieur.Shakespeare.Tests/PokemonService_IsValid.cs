using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PokeApiNet;
using Pokemonsieur.Shakespeare.Model;
using Pokemonsieur.Shakespeare.Service;
using Refit;
using Xunit;

namespace Pokemonsieur.Shakespeare.Tests
{
    public class PokemonService_IsValid
    {
        private IPokemonService _pokemonService;

        private readonly Mock<ILogger<PokemonService>> _mockLogger = new Mock<ILogger<PokemonService>>();

        private Mock<IRestApiClient<PokemonSpecies, PokemonQueryParams, string>> _mockClient = new Mock<IRestApiClient<PokemonSpecies, PokemonQueryParams, string>>();

        public readonly Mock<IOptions<AppSettings>> _mockOptions = new Mock<IOptions<AppSettings>>();

        public PokemonService_IsValid()
        {
            _mockOptions.Setup(o => o.Value).Returns(new AppSettings
            {
                PokeApi = new PokeApi
                {
                    DefaultLanguage = "en",
                    Key = "testKey",
                    Url = "http://test"
                }
            });
        }

        [Fact]
        public async void Valid_PokemonName_ReturnSuccess()
        {
            //Arrange
            _mockClient.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<PokemonQueryParams>())).ReturnsAsync(new PokemonSpecies
            {
                Name = TestData._mockName,
                FlavorTextEntries = new List<PokemonSpeciesFlavorTexts> {
                    new PokemonSpeciesFlavorTexts {
                        FlavorText = TestData._mockDetails,
                        Language = new NamedApiResource<Language> { Name = "en" }
                    }
                }
            });
            _pokemonService = new PokemonService(_mockLogger.Object, _mockClient.Object, _mockOptions.Object);

            //Act
            var output = await _pokemonService.GetPokemonDetailsAsync(TestData._mockName);

            //Assert
            Assert.Equal(TestData._mockName, output.Name);
            Assert.Equal(TestData._mockDetails, output.Description);
            Assert.Null(output.Error);
        }

        [Fact]
        public async void Valid_PokemonName_ReturnsErrorNullSpecies()
        {
            //Arrange
            _mockClient.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<PokemonQueryParams>())).ReturnsAsync(new PokemonSpecies
            {
                Name = TestData._mockName,
                FlavorTextEntries = null
            });
            _pokemonService = new PokemonService(_mockLogger.Object, _mockClient.Object, _mockOptions.Object);

            //Act
            var output = await _pokemonService.GetPokemonDetailsAsync(TestData._mockName);

            //Assert
            Assert.NotNull(output.Error);
            Assert.Equal(500, output.Error.Code);
        }

        [Fact]
        public async void Valid_PokemonName_ReturnsErrorLangugaeMismatch()
        {
            //Arrange
            _mockClient.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<PokemonQueryParams>())).ReturnsAsync(new PokemonSpecies
            {
                Name = TestData._mockName,
                FlavorTextEntries = new List<PokemonSpeciesFlavorTexts> {
                    new PokemonSpeciesFlavorTexts {
                        Language = new NamedApiResource<Language> {
                            Name = "jk"
                        }
                    }
                }
            });
            _pokemonService = new PokemonService(_mockLogger.Object, _mockClient.Object, _mockOptions.Object);

            //Act
            var output = await _pokemonService.GetPokemonDetailsAsync(TestData._mockName);

            //Assert
            Assert.NotNull(output.Error);
            Assert.Equal(500, output.Error.Code);
        }


    }
}