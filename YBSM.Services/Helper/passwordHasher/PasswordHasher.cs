using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services.Helper.passwordHasher
{
    public class PasswordHasher : Infrastructure.Services.Helper.passwordHasher.IPasswordHasher
    {
        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            var passwordHasher = new PasswordHasher<dynamic>();
            return passwordHasher.VerifyHashedPassword(null, hashedPassword,
                providedPassword);
        }

        public string HashPassword(string password)
        {
            var passwordHasher = new PasswordHasher<dynamic>();
            return passwordHasher.HashPassword(null, password);
        }
    }
}