using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services.Helper.passwordHasher
{
    public interface IPasswordHasher
    {
        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword,
            string providedPassword);

        public  string HashPassword( string password);
    }
}