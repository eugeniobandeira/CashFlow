using CashFlow.Application.UseCases.Expenses.Reports.Excel;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf;
using CashFlow.Domain.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CashFlow.Api.Controllers
{
    /// <summary>
    /// Controller resposible for reports
    /// </summary>
    [Route("v1/api/report")]
    [ApiController]
    //[Authorize(Roles = RolesHelper.ADMIN)]
    public class ReportsController : ControllerBase
    {

        /// <summary>
        /// Retuns an Excel file with expenses in a month
        /// </summary>
        /// <param name="useCase"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("excel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetExcelAsync(
            [FromServices] IGenerateExpensesReportExcelUseCase useCase,
            [FromQuery] int year,
            [FromQuery] int month)
        {
            var date = new DateOnly(year, month, 1);
            byte[] file = await useCase.Execute(date);

            if (file.Length > 0)
                return File(file, MediaTypeNames.Application.Octet, "report.xlsx");

            return NoContent();
        }

        /// <summary>
        /// Retuns a PDF file with expenses in a month
        /// </summary>
        /// <param name="useCase"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pdf")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetPdfAsync(
            [FromServices] IGenerateExpensesReportPdfUseCase useCase,
            [FromQuery] int year,
            [FromQuery] int month)
        {
            var date = new DateOnly(year, month, 1);
            byte[] file = await useCase.Execute(date);

            if (file.Length > 0)
                return File(file, MediaTypeNames.Application.Pdf, "report.pdf");

            return NoContent();
        }
    }
}
