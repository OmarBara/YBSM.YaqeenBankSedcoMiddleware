using System;

namespace Core.Domain.ModelDTO
{
    public class AuthenticationResponseModels
    {
        public class LoginResponseModel
        {
            public long AccountId { get; set; }
            public string Username { get; set; }
            public string AccountEmail { get; set; }
            public string AccountPhoneNumber { get; set; }
            // public AccessTokenResponseModel AccessToken { get; set; }
        }
        
    }
}