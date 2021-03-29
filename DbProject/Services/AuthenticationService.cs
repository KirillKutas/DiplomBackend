using System;
using System.Threading.Tasks;
using DbProject.Entities.Authentication;
using DbProject.Helpers;
using DbProject.Interfaces;
using Microsoft.EntityFrameworkCore;
using ViewModels.Authentication;

namespace DbProject.Services
{
    public class AuthenticationService : IAuthenticationService
    {

        public async Task<User> Login(LoginViewModel user)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var existUser = await db.Users.FirstOrDefaultAsync(item => item.Email == user.Login || item.UserName == user.Login);

                if (existUser == null || !HashService.Verify(existUser.PasswordSalt, existUser.PasswordHash, user.Password))
                {
                    throw new Exception("Invalid user name or password");
                }
                return existUser;
            }
        }

        public async Task<User> Registration(RegistrationViewModel newUser)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var existUser = await db.Users.FirstOrDefaultAsync(item => item.Email == newUser.Email);
                if (existUser != null)
                {
                    throw new Exception("Such user already exists");
                }

                HashService.CreatePasswordHash(newUser.Password, out var passwordSalt, out var passwordHash);
                var user = new User()
                {
                    Id = Guid.NewGuid(),
                    UserName = newUser.UserName,
                    PasswordSalt = passwordSalt,
                    PasswordHash = passwordHash,
                    Email = newUser.Email,
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    Role = Role.User
                };

                await db.Users.AddAsync(user);
                await db.SaveChangesAsync();

                return user;
            }
        }

        public async Task<User> CheckExistUser(string email)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var existUser = await db.Users.FirstOrDefaultAsync(item => item.Email == email);
                return existUser;
            }
        }
    }
}
