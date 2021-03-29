using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
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

            string userName = result.Principal.Identities
                .FirstOrDefault().Claims.FirstOrDefault(item =>
                    item.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")).Value;
            string firstName = result.Principal.Identities
                .FirstOrDefault().Claims.FirstOrDefault(item =>
                    item.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")).Value;
            string lastName = result.Principal.Identities
                .FirstOrDefault().Claims.FirstOrDefault(item =>
                    item.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname")).Value;
            string email = result.Principal.Identities
                .FirstOrDefault().Claims.FirstOrDefault(item =>
                    item.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")).Value;

            var user = await _service.CheckExistUser(email);
            if (user == null)
            {
                user = await _service.Registration(new RegistrationViewModel
                {
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    Password = firstName + lastName + userName,
                    UserName = userName
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
