using System;
using Core.Domain.Enum;

namespace Core.Domain.ModelDTO
{
    public class UserInfoResponseDto
    {
        public UserInfoResponseDto()
        {
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserRole { get; set; }
        public string State { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }
    }
}