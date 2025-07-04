using CortekAI.Security.Service.Model;
using Microsoft.AspNetCore.Mvc;

namespace CortekAI.Security.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SecurityController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<SecurityController> _logger;

        public SecurityController(ILogger<SecurityController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost(Name ="Login")]
        public async Task<TokenResponse> Login(LoginRequest loginRequest)
        {
            LoginService loginService = new LoginService();


            return await loginService.GenerateSession(loginRequest);
        }
    }
}
