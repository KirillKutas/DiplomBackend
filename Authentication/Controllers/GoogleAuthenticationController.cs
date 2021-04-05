using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Authentication.Helpers;
using DbProject.Helpers;
using ViewModels.Authentication;

namespace Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomExceptionFilter]
    [AllowAnonymous]
    public class GoogleAuthenticationController : ControllerBase
    {
        private readonly DbProject.Interfaces.IAuthenticationService _service;

        public GoogleAuthenticationController(DbProject.Interfaces.IAuthenticationService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("token")]
        public IActionResult Token()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet]
        [Route("google-response")]
        public async Task<string> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var data = AuthResultParser.Parse(result);

            var user = await _service.CheckExistUser(data.Email);
            if (user == null)
            {
                user = await _service.Registration(new RegistrationViewModel
                {
                    Email = data.Email,
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    Password = data.FirstName + data.LastName + data.UserName,
                    UserName = data.UserName
                });
            }

            var token = Helpers.Token.Create(user);

            var response = new
            {
                access_token = token,
                username = user.UserName
            };

            return ResponseCreator.Create(response);
        }
    }
}
