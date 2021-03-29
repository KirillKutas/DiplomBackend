using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Authentication
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer"; // Token publisher
        public const string AUDIENCE = "MyAuthClient"; // Token consumer
        const string KEY = "mysupersecret_secretkey!123";   // Encryption key
        public const int LIFETIME = 120; // Token lifetime - 1 min
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
