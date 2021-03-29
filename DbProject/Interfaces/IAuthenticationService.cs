using System.Threading.Tasks;
using DbProject.Entities.Authentication;
using ViewModels.Authentication;

namespace DbProject.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<User> Login(LoginViewModel user);
        public Task<User> Registration(RegistrationViewModel newUser);
        public Task<User> CheckExistUser(string email);
    }
}
