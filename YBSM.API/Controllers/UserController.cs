using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domain.ModelDTO;
using Infrastructure.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserServices _userServices;

        public UserController(UserServices userServices)
        {
            _userServices = userServices;
        }


        [HttpPost("sign-in")]
        public async Task<UserSigInResponseDto> SignIn(
            UserSignInRequestDto userSignInRequestDto)
        {
            var userAgent = Request.Headers["User-Agent"].ToString()?.Trim();
            var remoteIpAddress = "";

            if (Request.HttpContext.Connection.RemoteIpAddress != null)
            {
                remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            }

            return await _userServices.SignIn(userSignInRequestDto, remoteIpAddress, userAgent);
        }

        [HttpPost]
        [Authorize]
        public async Task<AddUserResponseDto> AddUser(
            AddUserRequestDto addUserRequest)
        {
            var user = CurrentUserModel.GetUser(User);

            return await _userServices.AddUser(addUserRequest, user.UserId);
        }

        [HttpGet]
        //[Authorize]
        public async Task<List<UserInfoResponseDto>> Users(
        )
        {
            return await _userServices.Users();
        }


        [HttpPut]
        [Authorize]
        public async Task<IActionResult> EditUser(EditUserRequestDto editUserRequestDt
        )
        {
            var user = CurrentUserModel.GetUser(User);

            await _userServices.EdiUser(editUserRequestDt, user.UserId);

            return Ok();
        }

        [HttpPut("change-email")]
        [Authorize]
        public async Task<IActionResult> ChangeEmail(ChangeEmailRequestDto changeEmailRequestDto
        )
        {
            var user = CurrentUserModel.GetUser(User);

            await _userServices.ChangeEmail(changeEmailRequestDto, user.UserId);

            return Ok();
        }


        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestDto changePasswordRequestDto
        )
        {
            var user = CurrentUserModel.GetUser(User);

            await _userServices.ChangePassword(changePasswordRequestDto, user.UserId);

            return Ok();
        }


        [HttpPut("update-state")]
        [Authorize]
        public async Task<IActionResult> UpdateState(ChangeUserStateRequestDto changeUserStateRequestDto
        )
        {
            await _userServices.ChangeState(changeUserStateRequestDto, changeUserStateRequestDto.UserId);

            return Ok();
        }
    }
}