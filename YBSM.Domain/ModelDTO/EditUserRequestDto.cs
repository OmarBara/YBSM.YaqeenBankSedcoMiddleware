using Core.Domain.Enum;

namespace Core.Domain.ModelDTO
{
    public class EditUserRequestDto
    {
        public EditUserRequestDto()
        {
        }

        public string PhoneNumber { get; set; }
        public Roles UserRole { get; set; }
    }
}