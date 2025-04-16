using System;
using Core.Domain.Enum;

namespace Core.Domain.ModelDTO
{
    public class AddUserRequestDto
    {
        public AddUserRequestDto()
        {
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
        public Roles UserRole { get; set; }
    }
}