using System;

namespace Core.Domain.ModelDTO
{
    public class UserSigInResponseDto
    {
        public UserSigInResponseDto()
        {
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserRole { get; set; }
        public string State { get; set; }
        public DateTime? LastLogin { get; set; }
        public AccessTokenResponseModel AccessToken { get; set; }
    }
}