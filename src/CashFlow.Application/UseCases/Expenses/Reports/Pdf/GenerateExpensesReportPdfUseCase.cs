
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using CashFlow.Domain.Interface.Expenses;
using PdfSharp.Fonts;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf
{
    internal class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
    {
        private readonly IExpensesReadOnlyRepository _repository;
        private const string CURRENCY_SYMBOL = "€";

        public GenerateExpensesReportPdfUseCase(IExpensesReadOnlyRepository expensesReadOnlyRepository)
        {
            _repository = expensesReadOnlyRepository;
            GlobalFontSettings.FontResolver = new ExpensesReportFontsResolver();
        }
        public async Task<byte[]> Execute(DateOnly month)
        {
            var expenses = await _repository.FilterByMonth(month);
            if (expenses.Count == 0)
                return [];

            return [];
        }
    }
}
