using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Pokemonsieur.Shakespeare.Model;
using Pokemonsieur.Shakespeare.Service;
using Xunit;

namespace Pokemonsieur.Shakespeare.Tests
{
    public class TranslationService_IsValid
    {

        private ITranslationService _pokemonService;

        private readonly Mock<ILogger<TranslationService>> _mockLogger = new Mock<ILogger<TranslationService>>();

        private Mock<IRestApiClient<Translation, TranslationQueryParams, string>> _mockClient = new Mock<IRestApiClient<Translation, TranslationQueryParams, string>>();

        public readonly Mock<IOptions<AppSettings>> _mockOptions = new Mock<IOptions<AppSettings>>();


        public TranslationService_IsValid()
        {
            _mockOptions.Setup(o => o.Value).Returns(new AppSettings
            {
                TranslationApi = new TranslationApi
                {
                    Type = "testKey",
                    Url = "http://test"
                }
            });
        }

        [Fact]
        public async void Valid_Text_ReturnSuccess()
        {
            //Arrange
            _mockClient.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<TranslationQueryParams>())).ReturnsAsync(new Translation
            {
                Contents = new Contents
                {
                    Text = TestData._mockDetails
                }
            });
            _pokemonService = new TranslationService(_mockLogger.Object, _mockOptions.Object, _mockClient.Object);

            //Act
            var output = await _pokemonService.GetTranslationAsync(TestData._mockDetails);

            //Assert
            Assert.Equal(TestData._mockDetails, output.Contents.Text);
            Assert.Null(output.Error);
        }






    }
}