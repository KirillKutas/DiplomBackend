using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbProject.Entities.Authentication;
using Microsoft.AspNetCore.Authentication;

namespace Authentication.Helpers
{
    public static class AuthResultParser
    {
        public static User Parse(AuthenticateResult result)
        {
            return new User()
            {
                Email = result.Principal.Identities
                    .FirstOrDefault().Claims.FirstOrDefault(item =>
                        item.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")).Value,
                FirstName = result.Principal.Identities
                    .FirstOrDefault().Claims.FirstOrDefault(item =>
                        item.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")).Value,
                LastName = result.Principal.Identities
                    .FirstOrDefault().Claims.FirstOrDefault(item =>
                        item.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname")).Value,
                UserName = result.Principal.Identities
                    .FirstOrDefault().Claims.FirstOrDefault(item =>
                        item.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")).Value
            };
        }
    }
}
