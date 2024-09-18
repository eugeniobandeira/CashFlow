using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers
{
    [Route("v1/api/expenses")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly IRegisterExpenseUseCase _useCase;

        public ExpensesController(IRegisterExpenseUseCase registerExpenseUseCase)
        {
            _useCase = registerExpenseUseCase;
        }

        [HttpPost]
        public IActionResult Register([FromBody] InsertExpenseRequest req)
        {
            var response = _useCase.Execute(req);

            return Created(string.Empty, response);
        }
    }
}
