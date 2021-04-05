using System.Threading.Tasks;
using Authentication.Helpers;
using DbProject.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Mvc;
using ViewModels.Authentication;

namespace Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacebookAuthenticationController : ControllerBase
    {
        private readonly DbProject.Interfaces.IAuthenticationService _service;

        public FacebookAuthenticationController(DbProject.Interfaces.IAuthenticationService service)
        {
            _service = service;
        }


        [HttpGet]
        [Route("SignIn")]
        public IActionResult SignIn()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("FacebookResponse") };
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        [HttpGet]
        [Route("facebook-response")]
        public async Task<string> FacebookResponse()
        {
            var result = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);


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

            var token = Token.Create(user);

            var response = new
            {
                access_token = token,
                username = user.UserName
            };

            return ResponseCreator.Create(response);
        }
    }
}
