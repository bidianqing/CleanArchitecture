using Domain.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Portal.Controllers;

namespace CleanArchitecture.UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            var logger = new Mock<ILogger<WeatherForecastController>>();
            var orderService = new Mock<IOrderService>();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            httpClientFactory.Setup(s => s.CreateClient(It.IsAny<string>())).Returns(new HttpClient());

            var controller = new WeatherForecastController(logger.Object, orderService.Object, httpClientFactory.Object);

            await controller.Get("tom");

            Assert.True(true);
        }
    }
}