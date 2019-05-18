using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service.Core;
using Service.Login.API.Models;
using Service.Login.BAL;
namespace Service.Login.API.Controllers
{
    using static APIEndpointConfiguration;

    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route(URI_Template_Action)]
    [Produces("application/json")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly TokenSettings _tokenSettings;
        private readonly ILoginService _loginService;
        public LoginController(IOptions<TokenSettings> tokenSettings, ILoginService loginService)
        {
            _loginService = loginService;
            _tokenSettings = tokenSettings.Value;
        }
        // GET api/ValidateLogin
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<string>> ValidateLogin([FromBody] LoginViewModel model)
        {
            return await _loginService.Authenticate(model.Email, model.Password);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [AllowAnonymous]
        [HttpPost]
        public async Task<string> RegisterUser([FromBody] RegisterViewModel input)
        {
            return await _loginService.Register(input.Email, input.Password);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
