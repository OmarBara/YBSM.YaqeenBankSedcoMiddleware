using System;

namespace Core.Domain.ModelDTO
{
    public class AuthorizationRequestDto
    {
        //Auth Info
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }


    }
}