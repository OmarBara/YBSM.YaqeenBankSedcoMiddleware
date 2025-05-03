using Core.Domain.ModelDTO;
using FluentValidation;

namespace HRM.API.Validatiors
{
    public class AddUserRequestDtoValidator: AbstractValidator<AddUserRequestDto> 
    {
        public AddUserRequestDtoValidator() {

            RuleFor(c => c.Email)
                //.EmailAddress().Length(3, 64)
                .NotEmpty()
                .WithMessage(" email is required! ");


            RuleFor(c => c.Password)
               .NotEmpty()
               .WithMessage(" UserRole is required! ");

           
        }
    }
}
