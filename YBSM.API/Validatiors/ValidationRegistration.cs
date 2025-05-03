using Core.Domain.ModelDTO;
using FluentValidation;
using HRM.API.Validatiors;
using YBSM.Core.Domain.ModelDTO.RequestDTO;

namespace Web.API.Validatiors
{
    public static class ValidationRegistration
    {
        public static void AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<AddUserRequestDto>, AddUserRequestDtoValidator>();            
            services.AddScoped<IValidator<LypayTransRequestDto>, AddLypayTransRequestDtoValidator>();            
            
            /*services.AddScoped<IValidator<AddActivityRequestDto>, AddActivityRequestDtoValidator>();
            services.AddScoped<IValidator<AddCategoryRequestDto>, AddCategoryRequestDtoValidator>();
            services.AddScoped<IValidator<MerchantSignUpRequestDto>, MerchantSignUpRequestDtoValidator>();*/
        }
    }
}