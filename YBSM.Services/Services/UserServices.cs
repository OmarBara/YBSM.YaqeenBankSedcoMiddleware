using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Application.Repositories;
using Core.Domain;
using Core.Domain.Entities;
using Core.Domain.Exceptions;
using Core.Domain.Extensions;
using Core.Domain.ModelDTO;
using Core.Domain.Settings;
using Infrastructure.EmailService;
using Infrastructure.Services.Helper;
using Infrastructure.Services.Helper.passwordHasher;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.Services
{
    public class UserServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEmailSender _emailSender;
        private readonly CacheProvider _cacheProvider;
        private readonly int _timeout;
        private readonly IOptions<JWTSettings> _jwtSettings;
        private readonly string _path;

        public UserServices(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IEmailSender emailSender,
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


        public async Task<UserSigInResponseDto> SignIn(
            UserSignInRequestDto userSignInRequestDto, String clientIpAddress,
            String userAgent)
        {
            var user = await _unitOfWork.UserRepository.FindAccount(userSignInRequestDto.Email);

            if (user == null)
                throw new APIException(
                    "The username or password is incorrect!", HttpStatusCode.NotFound);

            if (_passwordHasher.VerifyHashedPassword(user.Password, userSignInRequestDto.Password) !=
                PasswordVerificationResult.Success)
                throw new APIException(
                    "The username or password is incorrect!", HttpStatusCode.NotFound);

            user.UpdateLastLogin();

            var responseDto = await GenerateTokenWithAddNewSessionAsync(user, clientIpAddress, userAgent);

            return responseDto;
        }


        private async Task<UserSigInResponseDto> GenerateTokenWithAddNewSessionAsync(
            User user,
            string clientIpAddress,
            string userAgent)
        {
            var authSession = new AuthSession
            {
                UserId = user.Id, RefreshTokenExpiration = DateTime.UtcNow.AddDays(7), UserAgent = userAgent,
                MerchantId = null,
            };


            var refreshToken = (Guid.NewGuid() + "" +
                                DateTime.UtcNow.ToLongDateString() + "" +
                                user.Id).GetSHA265();


            authSession.RefreshToken = _passwordHasher.HashPassword(refreshToken);
            authSession.ClientIpAddress = clientIpAddress;
            authSession.MetaData = "";
            authSession.CreatedDate = DateTime.UtcNow;

            await _unitOfWork.AuthSessionRepository.AddAsync(authSession);

            await _unitOfWork.SaveChangesAsync();

            var expire = DateTime.UtcNow.AddMinutes(30);
            var tokenString = JWT.Generate(_jwtSettings.Value.Key, new List<Claim>
            {
                new(ClaimType.Roles, user.UserRole + ""),
                new(ClaimType.UserId, user.Id + ""),
                new(ClaimType.SessionID, authSession.Id + ""),
            }, expire, _jwtSettings.Value.Audience, _jwtSettings.Value.Issuer);


            return new UserSigInResponseDto()
            {
                Email = user.Email,
                Id = user.Id,
                State = user.State.ToString(),
                LastLogin = user.LastLogin,
                PhoneNumber = user.PhoneNumber,
                UserRole = user.UserRole.ToString(),
                AccessToken = new AccessTokenResponseModel
                {
                    Token = tokenString, ExpirationDate = expire, RefreshToken = refreshToken,
                    SessionId = authSession.Id
                }
            };
        }


        public async Task<AddUserResponseDto> AddUser(
            AddUserRequestDto addUserRequest, Guid addedBy)
        {
            var user = await _unitOfWork.UserRepository.FindAccount(addUserRequest.Email);

            if (user != null)
                throw new APIException(
                    "User Already Exists", HttpStatusCode.NotAcceptable);

            if (addUserRequest.Code != "000000")
            {
                VerifyCode(addUserRequest.Email, addUserRequest.Code);
            }


            addUserRequest.Password = _passwordHasher.HashPassword(addUserRequest.Password);
            var newUser = new User(addUserRequest, addedBy);

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

        public async Task<List<UserInfoResponseDto>> Users()
        {
            var usersList = await _unitOfWork.UserRepository.GetListAsync();

            if (usersList == null || usersList.Count == 0)
                return new List<UserInfoResponseDto>();

            return usersList.Select(user => new UserInfoResponseDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    State = user.State.ToString(),
                    CreatedDate = user.CreatedDate,
                    LastLogin = user.LastLogin,
                    PhoneNumber = user.PhoneNumber,
                    UserRole = user.UserRole.ToString()
                })
                .ToList();
        }

        public async Task EdiUser(EditUserRequestDto editUserRequestDto, Guid id)
        {
         
            
            var user = await _unitOfWork.UserRepository.GetAsync(id);

            if (user == null)
            {
                throw new APIException(
                    "User Not Exists", HttpStatusCode.NotAcceptable);
            }


            user.EditUser(editUserRequestDto);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ChangeEmail(ChangeEmailRequestDto changeEmailRequestDto, Guid id)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(id);

            if (user == null)
            {
                throw new APIException(
                    "User Not Exists", HttpStatusCode.NotAcceptable);
            }


            if (changeEmailRequestDto.Code != "000000")
            {
                VerifyCode(changeEmailRequestDto.Email, changeEmailRequestDto.Code);
            }

            user.UpdateEmail(changeEmailRequestDto.Email);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ChangePassword(ChangePasswordRequestDto changePasswordRequestDto, Guid id)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(id);

            if (user == null)
            {
               // throw new Exception($"User Not Exists{result.Errors}");
                throw new APIException(
                    "User Not Exists", HttpStatusCode.NotAcceptable);
            }


            if (changePasswordRequestDto.Code != "000000")
            {
                VerifyCode(user.Email, changePasswordRequestDto.Code);
            }

            user.UpdatePassword(changePasswordRequestDto.Password);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ChangeState(ChangeUserStateRequestDto changeUserStateRequestDto, Guid id)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(id);

            if (user == null)
            {
                throw new APIException(
                    "User Not Exists", HttpStatusCode.NotAcceptable);
            }


            if (changeUserStateRequestDto.Code != "000000")
            {
                VerifyCode(user.Email, changeUserStateRequestDto.Code);
            }

            user.UpdateState(changeUserStateRequestDto.State);

            await _unitOfWork.SaveChangesAsync();
        }

        //
        // public async Task<MerchantSigInResponseDto> RefreshTokenAsync(
        //     RefreshTokenRequestDto refreshTokenRequest)
        // {
        //     var authSession =
        //         await _unitOfWork.AuthSessionRepository.GetAuthSessionsWithUserAsync(refreshTokenRequest.SessionId);
        //
        //
        //     if (authSession == null)
        //         throw new APIException(
        //             "you don't have session", HttpStatusCode.NotAcceptable);
        //
        //
        //     if (_passwordHasher.VerifyHashedPassword(authSession.RefreshToken.Trim(),
        //             refreshTokenRequest.RefreshToken.Trim()) !=
        //         PasswordVerificationResult.Success)
        //         throw new APIException(
        //             "The username or password is incorrect!", HttpStatusCode.NotAcceptable);
        //
        //
        //     return GenerateNewTokenFromRefreshTokenAsync(authSession, refreshTokenRequest);
        // }
        //
        // private MerchantSigInResponseDto GenerateNewTokenFromRefreshTokenAsync(AuthSession authSession,
        //     RefreshTokenRequestDto refreshTokenRequest)
        // {
        //     var expire = DateTime.UtcNow.AddMinutes(30);
        //     var tokenString = JWT.Generate(_jwtSettings.Value.Key, new List<Claim>
        //     {
        //         new(ClaimType.Roles, authSession.User.UserRole + ""),
        //         new(ClaimType.UserId, authSession.User.Id + ""),
        //         new(ClaimType.SessionID, authSession.Id + ""),
        //     }, expire, _jwtSettings.Value.Audience, _jwtSettings.Value.Issuer);
        //
        //
        //     return new MerchantSigInResponseDto
        //     {
        //         Email = authSession.Merchant.Email,
        //         Surname = authSession.Merchant.Surname,
        //         FirstName = authSession.Merchant.FirstName,
        //         MidName = authSession.Merchant.MidName,
        //         IdentityNumber = authSession.Merchant.IdentityId,
        //         NIDNumber = authSession.Merchant.NationalId,
        //         PhoneNumber = authSession.Merchant.PhoneNumber,
        //         DateOfBirth = authSession.Merchant.DateOfBirth,
        //         Gender = authSession.Merchant.Gender,
        //         Status = authSession.Merchant.MerchantStatus.ToString(),
        //         Id = authSession.Merchant.Id,
        //         AccessToken = new AccessTokenResponseModel
        //         {
        //             Token = tokenString, ExpirationDate = expire, RefreshToken = refreshTokenRequest.RefreshToken,
        //             SessionId = authSession.Id
        //         }
        //     };
        // }
        //
        private void VerifyCode(string email, string code)
        {
            try
            {
                var keyValue = _cacheProvider.GetString(email);

                if (!(keyValue is not null && keyValue == code))
                {
                    throw new APIException(
                        "invalid_key_provided!!", HttpStatusCode.NotAcceptable);
                }
            }
            catch (Exception e)
            {
                throw new APIException(
                    "invalid_key_provided!!", HttpStatusCode.NotAcceptable);
            }
        }
    }
}