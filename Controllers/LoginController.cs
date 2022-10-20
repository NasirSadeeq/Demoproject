using Demoproject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demoproject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IwjtTokenManager tokenManager;

        public LoginController(IwjtTokenManager tokenManager)
        {
            this.tokenManager = tokenManager;
        }
            [HttpPost("Login")]
            public IActionResult Login([FromBody] UserLogin ulogin)
            {
                var Token = tokenManager.Authenticate(ulogin.Email.ToLower(), ulogin.Password);
                if (string.IsNullOrEmpty(Token))
                    return Unauthorized();

                return Ok(Token);
            }
        
    }
}
