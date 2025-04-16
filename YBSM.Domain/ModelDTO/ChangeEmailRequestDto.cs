using System;

namespace Core.Domain.ModelDTO
{
    public class ChangeEmailRequestDto
    {
        public ChangeEmailRequestDto()
        {
        }

        public string Email { get; set; }
        public string Code { get; set; }
    }
}