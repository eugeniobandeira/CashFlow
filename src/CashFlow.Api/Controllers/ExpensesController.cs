using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Application.UseCases.Expenses.GetAll;
using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Application.UseCases.Expenses.Update;
using CashFlow.Domain.Requests.Expenses;
using CashFlow.Domain.Responses.Error;
using CashFlow.Domain.Responses.Register;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers
{
    /// <summary>
    /// Controller responsible for managing expenses
    /// </summary>
    [Route("v1/api/expenses")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {

        /// <summary>
        /// Register a new expense in the system
        /// </summary>
        /// <param name="req">Object containing the necessary data to create a new expense register</param>
        /// <param name="useCase">Service</param>
        /// <returns>Return the registered object or an error on validation</returns>
        /// <response code="201">Succeeded</response>
        /// <response code="400">Error while creating the register</response>
        [HttpPost]
        [ProducesResponseType(typeof(RegisteredExpenseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
            [FromServices] IRegisterExpenseUseCase useCase, 
            [FromBody] ExpenseRequest req)
        {
            var response = await useCase.Execute(req);

            return CreatedAtAction(nameof(GetExpenseById), new { id = response.Id }, response);
        }

        /// <summary>
        /// List all the expenses
        /// </summary>
        /// <param name="useCase"></param>
        /// <returns>List of expense entity</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ExpensesResponseList), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllExpenses([FromServices] IGetAllExpenseUseCase useCase)
        {
            var response = await useCase.Execute();

            if (response.RegisteredExpenses.Count != 0)
                return Ok(response);

            return NoContent();
        }

        /// <summary>
        /// Get an expense by its Id
        /// </summary>
        /// <param name="useCase"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(RegisteredExpenseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExpenseById(
            [FromServices] IGetExpenseByIdUseCase useCase, 
            long id)
        {
            var response = await useCase.Execute(id);

            if (response.Id > 0)
                return Ok(response);

            return BadRequest();
        }

        /// <summary>
        /// Updates an expense by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="req"></param>
        /// <param name="useCase"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(RegisteredExpenseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(
            [FromRoute] long id,
            [FromBody] ExpenseRequest req,
            [FromServices] IUpdateExpenseUseCase useCase)
        {
            var response = await useCase.Execute(id, req);

            return Ok(response);
        }

        /// <summary>
        /// Delete an expense by id
        /// </summary>
        /// <param name="useCase"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            [FromServices] IDeleteExpenseUseCase useCase, [FromRoute] long id)
        {
            await useCase.Execute(id);

            return NoContent();
        }
    }
}
