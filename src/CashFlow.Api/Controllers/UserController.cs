using CashFlow.Application.UseCases.Users.Profile;
using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Domain.Requests.Users;
using CashFlow.Domain.Responses.Error;
using CashFlow.Domain.Responses.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers
{
    /// <summary>
    /// Controller responsible for managing users
    /// </summary>
    [Route("v1/api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// Register an user
        /// </summary>
        /// <param name="req"></param>
        /// <param name="useCase"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(RegisteredUserResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterAsync(
            [FromServices] IRegisterUserUseCase useCase,
            [FromBody] UserRequest req
            )
        {
            var response = await useCase.Execute(req);

            return Created(string.Empty, response);
        }


        /// <summary>
        /// Get user profile
        /// </summary>
        /// <param name="useCase"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
        [Authorize]
        public async Task<IActionResult> GetProfileAsync([FromServices] IGetUserProfileUseCase useCase)
        {
            var response = await useCase.Execute();

            return Ok(response);
        }
    }
}
