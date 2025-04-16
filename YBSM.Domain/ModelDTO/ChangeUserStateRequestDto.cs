using System;
using Core.Domain.Enum;

namespace Core.Domain.ModelDTO
{
    public class ChangeUserStateRequestDto
    {
        public ChangeUserStateRequestDto()
        {
        }

        public Guid UserId { get; set; }
        public State State { get; set; }
        public string Code { get; set; }
    }
}