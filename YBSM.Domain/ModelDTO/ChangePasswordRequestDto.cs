using System;

namespace Core.Domain.ModelDTO
{
    public class ChangePasswordRequestDto
    {
        public ChangePasswordRequestDto()
        {
        }

        public string Password { get; set; }
        public string Code { get; set; }
    }
}