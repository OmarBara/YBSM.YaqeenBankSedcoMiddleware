using System;

namespace Core.Domain.ModelDTO
{
    public class UserRequestDto
    {
        public UserRequestDto()
        {
        }

        public long Id { get; set; }
        public string UserName { get; set; }
        public string Pin { get; set; }

        public string Name { get; set; }
        public string FatherName { get; set; }
        public string Surname { get; set; }
        public string PlaceOfBirth { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string NationalId { get; set; }
        public string IdentityId { get; set; }
        public byte Gender { get; set; }
    }
}