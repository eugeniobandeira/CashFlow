using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        [HttpPost]
        public IActionResult Register([FromBody] InsertExpenseRequest req)
        {
            var useCase = new RegisterExpenseUseCase();
            var response = useCase.Execute(req);

            return Created(string.Empty, response);
        }
    }
}
