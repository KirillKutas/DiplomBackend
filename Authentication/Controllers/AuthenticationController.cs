using System;
using System.Threading.Tasks;
using Authentication.Helpers;
using DbProject.Entities.Authentication;
using DbProject.Helpers;
using DbProject.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViewModels.Authentication;

namespace Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomExceptionFilter]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _service;

        public AuthenticationController(IAuthenticationService service)
        {
            _service = service;
        }
        [HttpGet]
        [Route("login")]
        public async Task<string> Login([FromQuery] LoginViewModel model)
        {
            User user;

            if (ModelState.IsValid)
                user = await _service.Login(model);
            else
                throw new Exception("All fields must be filled in");

            var token = Token.Create(user);

            return ResponseCreator.Create(token);
        }

        [HttpPost]
        [Route("registration")]
        public async Task<string> Registration(RegistrationViewModel model)
        {

            if (ModelState.IsValid)
                await _service.Registration(model);
            else
                throw new Exception("All fields must be filled in");

            return ResponseCreator.Create(true);
        }
    }
}
