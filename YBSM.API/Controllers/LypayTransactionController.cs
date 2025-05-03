using Core.Domain.ModelDTO;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBSM.Core.Domain.ModelDTO.RequestDTO;
using YBSM.Infrastructure.Services.Services;

namespace YBSM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LypayTransactionController : ControllerBase
    {
        private readonly LypayTransactionServices _lypayTransactionServices;
        public LypayTransactionController(LypayTransactionServices lypayTransactionServices)
        {
            _lypayTransactionServices = lypayTransactionServices;
        }



      /*  [HttpPost("inquired")]
        //[Authorize]
        public async Task<IActionResult> GetLypayTransaction(LypayTransRequestDto lypayTransRequest)
        {
            //var user = CurrentUserModel.GetUser(User);
            *//*if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*//*
            var lypayTrans = await _lypayTransactionServices.GetLypayTransaction(lypayTransRequest);
            return Ok(lypayTrans);
        }*/

        [HttpPost("inquired")]
        public async Task<IActionResult> ValidateLypayTransaction([FromBody] LypayTransRequestDto request, [FromServices] IValidator<LypayTransRequestDto> validator)
        {
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(error => new
                {
                    Code = (error.CustomState as dynamic)?.Code ?? "E0",
                    Message = (error.CustomState as dynamic)?.Message ?? error.ErrorMessage
                });

                return BadRequest(new { Result = errors });
            }

            var lypayTrans = await _lypayTransactionServices.GetLypayTransaction(request);
            return Ok(lypayTrans);
        }
    }
}
