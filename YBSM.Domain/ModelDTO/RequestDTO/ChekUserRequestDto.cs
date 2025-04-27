using System;
using Core.Domain.Enum;

namespace Core.Domain.ModelDTO
{
    public class ChekUserRequestDto

    {
        public string? nid { get; set; }
        public string? phone { get; set; }
        public string? passportNumber { get; set; }
    }
}