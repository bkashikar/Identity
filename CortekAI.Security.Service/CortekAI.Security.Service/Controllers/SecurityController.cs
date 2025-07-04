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

       

        [HttpPost(Name ="Login")]
        public async Task<TokenResponse> Login(LoginRequest loginRequest)
        {
            LoginService loginService = new LoginService();


            return await loginService.GenerateSession(loginRequest);
        }
    }
}
