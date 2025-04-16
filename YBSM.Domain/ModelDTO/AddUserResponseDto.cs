using System;
using Core.Domain.Enum;

namespace Core.Domain.ModelDTO
{
    public class AddUserResponseDto
    {
        public AddUserResponseDto()
        {
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Roles UserRole { get; set; }
        public State State { get; set; } = State.Active;
        public DateTime CreatedDate { get; set; }

    }
}