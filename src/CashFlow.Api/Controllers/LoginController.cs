using CashFlow.Domain.Requests.Login;
using CashFlow.Domain.Responses.Users;
using Microsoft.AspNetCore.Mvc;
using CashFlow.Domain.Responses.Error;
using CashFlow.Application.UseCases.Login;

namespace CashFlow.Api.Controllers
{
    /// <summary>
    /// Controller responsible for managing login
    /// </summary>
    [Route("v1/api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        /// <summary>
        /// Do login
        /// </summary>
        /// <param name="useCase"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(RegisteredUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LoginAsync(
            [FromServices] IDoLoginUseCase useCase,
            [FromBody] LoginRequest req)
        {
            var response = await useCase.Execute(req);

            return Ok(response);
        }
    }
}
