using Domain.SeedWork;
using Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace Portal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IOrderService _orderService;
        private readonly IHttpClientFactory _httpClientFactory;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IOrderService orderService,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _orderService = orderService;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<ResultModel<WeatherForecast[]>> Get([Required] string name)
        {
            _orderService.Test();

            var httpClient = _httpClientFactory.CreateClient("default");
            HttpRequestMessage httpRequestMessage = new(HttpMethod.Get, "https://api.jsonserve.com/3jYJJ6");
            httpRequestMessage.Options.TryAdd("name", "bidianqing");
            await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead);

            var rng = new Random();
            var data = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            return ResultModel.Success(data);
        }
    }
}
