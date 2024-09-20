using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses.Error;
using CashFlow.Communication.Responses.Register;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers
{
    [Route("v1/api/expenses")]
    [ApiController]
    public class ExpensesController(IRegisterExpenseUseCase registerExpenseUseCase) : ControllerBase
    {
        private readonly IRegisterExpenseUseCase _useCase = registerExpenseUseCase;

        [HttpPost]
        [ProducesResponseType(typeof(RegisteredExpenseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] InsertExpenseRequest req)
        {
            var response = await _useCase.Execute(req);

            return Created(string.Empty, response);
        }
    }
}
