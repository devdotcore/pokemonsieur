using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Pokemonsieur.Shakespeare.Controllers;
using Pokemonsieur.Shakespeare.Model;
using Pokemonsieur.Shakespeare.Service;
using Xunit;

namespace Pokemonsieur.Shakespeare.Tests
{
    public class PokemonsieurController_IsTranslation
    {
        private PokemonsieurController _pokemonsieurController;
        private readonly Mock<ILogger<PokemonsieurController>> _mockLogger;

        private readonly Mock<IPokemonsieurService> _mockPokemonsieurService;

        public PokemonsieurController_IsTranslation()
        {
            _mockPokemonsieurService = new Mock<IPokemonsieurService>();
            _mockLogger = new Mock<ILogger<PokemonsieurController>>();
        }

        [Fact]
        public async Task Invalid_PokemonName_ReturnBadRequest()
        {
            //Arrange
            _mockPokemonsieurService.Setup(p => p.GetErrorDetails(It.IsAny<int>())).Returns(new Error
            {
                Code = 400
            });
            _pokemonsieurController = new PokemonsieurController(_mockLogger.Object, _mockPokemonsieurService.Object);

            //Act
            var output = await _pokemonsieurController.GetAsync("dsds$$£$%%%");

            var status = output.Result as ObjectResult;

            //Assert
            Assert.Equal(400, status.StatusCode);
        }

        [Fact]
        public async Task Valid_PokemonName_ReturnSystemException()
        {
            //Arrange
            _mockPokemonsieurService.Setup(p => p.GetErrorDetails(It.IsAny<int>())).Returns(new Error
            {
                Code = 500
            });
            _mockPokemonsieurService.Setup(p => p.GetDetailsAndTranslateAsync(It.IsAny<string>())).Throws<Exception>();

            _pokemonsieurController = new PokemonsieurController(_mockLogger.Object, _mockPokemonsieurService.Object);

            //Act
            var output = await _pokemonsieurController.GetAsync(TestData._mockName);

            var status = output.Result as ObjectResult;

            //Assert
            Assert.Equal(500, status.StatusCode);
        }

        [Fact]
        public async Task Valid_PokemonName_ReturnSuccess()
        {
            //Arrange
            _mockPokemonsieurService.Setup(p => p.GetDetailsAndTranslateAsync(It.IsAny<string>())).Returns(Task.FromResult(
                new Model.Pokemonsieur
                {
                    Name = TestData._mockName,
                    Description = TestData._mockTranslation
                }
            ));

            _pokemonsieurController = new PokemonsieurController(_mockLogger.Object, _mockPokemonsieurService.Object);

            //Act
            var output = await _pokemonsieurController.GetAsync(TestData._mockName);

            //Assert
            Assert.Equal(TestData._mockName, output.Value.Name);
            Assert.Equal(TestData._mockTranslation, output.Value.Description);
        }

        [Fact]
        public async Task Valid_PokemonName_ReturnApiError()
        {
            //Arrange
            _mockPokemonsieurService.Setup(p => p.GetErrorDetails(It.IsAny<int>())).Returns(new Error
            {
                Code = 404
            });
            _mockPokemonsieurService.Setup(p => p.GetDetailsAndTranslateAsync(It.IsAny<string>())).Returns(Task.FromResult(
                new Model.Pokemonsieur
                {
                    Error = new Error
                    {
                        Code = 404
                    }
                }
            ));

            _pokemonsieurController = new PokemonsieurController(_mockLogger.Object, _mockPokemonsieurService.Object);

            //Act
            var output = await _pokemonsieurController.GetAsync(TestData._mockName);
            var status = output.Result as ObjectResult;

            //Assert
            Assert.Equal(404, status.StatusCode);

        }


    }
}