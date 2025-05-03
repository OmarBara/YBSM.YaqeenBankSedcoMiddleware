using Core.Domain.ModelDTO;
using Infrastructure.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBSM.Infrastructure.Services.Services;

namespace YBSM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SedcoController : ControllerBase
    {
        private readonly SedcoServices _sedcoServices;

        public SedcoController(SedcoServices sedcoServices)
        {
            _sedcoServices = sedcoServices;
        }
/*
        [HttpPost]
        //[Authorize]
        public async Task<AddUserResponseDto> ChekCustomer(ChekUserRequestDto chekUserRequestDto)
        {
            var user = CurrentUserModel.GetUser(User);

            return await _sedcoServices.ChekCustomer(chekUserRequestDto, user.UserId);
        }*/

    }
}
