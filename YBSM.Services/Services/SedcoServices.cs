using Core.Application.Repositories;
using Core.Domain.Settings;
using Core.Domain;
using Infrastructure.Services.Helper;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Services.Helper.passwordHasher;
using Infrastructure.EmailService;
using Core.Domain.Entities;
using Core.Domain.Exceptions;
using Core.Domain.ModelDTO;
using System.Net;

namespace YBSM.Infrastructure.Services.Services
{
    public class SedcoServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEmailSender _emailSender;
        private readonly CacheProvider _cacheProvider;
        private readonly int _timeout;
        private readonly IOptions<JWTSettings> _jwtSettings;
        private readonly string _path;

        public SedcoServices(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IEmailSender emailSender,
            CacheProvider cacheProvider, IOptions<OtpOptions> options, IOptions<JWTSettings> jwtSettings,
            IHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _emailSender = emailSender;
            _cacheProvider = cacheProvider;
            _jwtSettings = jwtSettings;
            _timeout = options.Value.TimeoutInMinutes;
            _path = hostEnvironment.ContentRootPath;
        }


        public async Task<AddUserResponseDto> ChekCustomer(ChekUserRequestDto chekUserRequestDto, Guid addedBy)
        {
            var user = await _unitOfWork.UserRepository.FindAccount(addUserRequest.Email);

            if (user != null)
                throw new APIException(
                    "User Already Exists", HttpStatusCode.NotAcceptable);

            


            await _unitOfWork.UserRepository.AddAsync(newUser);

            await _unitOfWork.SaveChangesAsync();

            return new AddUserResponseDto
            {
                Id = newUser.Id,
                Email = newUser.Email,
                State = newUser.State,
                CreatedDate = newUser.CreatedDate,
                PhoneNumber = newUser.PhoneNumber,
                UserRole = newUser.UserRole
            };
        }


    }
}
