using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers
{
    [Route("v1/api/expenses")]
    [ApiController]
    public class ExpensesController(IRegisterExpenseUseCase registerExpenseUseCase) : ControllerBase
    {
        private readonly IRegisterExpenseUseCase _useCase = registerExpenseUseCase;

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] InsertExpenseRequest req)
        {
            var response = await _useCase.Execute(req);

            return Created(string.Empty, response);
        }
    }
}
