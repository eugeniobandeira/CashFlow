using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses.Error;
using CashFlow.Exception.ExceptionBase;
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
            try
            {
                var useCase = new RegisterExpenseUseCase();
                var response = useCase.Execute(req);

                return Created(string.Empty, response);
            }
            catch (ErrorOnValidationException ex)
            {
                var errorResponse = new ErrorResponse(ex.Errors); 

                return BadRequest(errorResponse);
            }
            catch
            {
                var errorResponse = new ErrorResponse("Unknown error");
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
    }
}
