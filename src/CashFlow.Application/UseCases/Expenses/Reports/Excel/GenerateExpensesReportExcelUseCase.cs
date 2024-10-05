﻿using CashFlow.Domain.Enums;
using CashFlow.Domain.Interface.Expenses;
using CashFlow.Domain.Reports;
using ClosedXML.Excel;

namespace CashFlow.Application.UseCases.Expenses.Reports.Excel
{
    internal class GenerateExpensesReportExcelUseCase : IGenerateExpensesReportExcelUseCase
    {
        private readonly IExpensesReadOnlyRepository _repository;
        private const string CURRENCY_SYMBOL = "€";

        public GenerateExpensesReportExcelUseCase(IExpensesReadOnlyRepository expensesReadOnlyRepository)
        {
            _repository = expensesReadOnlyRepository;
        }
        public async Task<byte[]> Execute(DateOnly month)
        {
            var expenses = await _repository.FilterByMonth(month);

            if (expenses.Count == 0)
                return [];

            using var workbook = new XLWorkbook();

            workbook.Author = "Eugênio Bandeira";
            workbook.Style.Font.FontSize = 12;
            workbook.Style.Font.FontName = "Times New Roman";

            var worksheet = workbook.Worksheets.Add($"report {month.ToString("Y")}");

            InsertHeader(worksheet);

            var row = 2;
            foreach (var exp in expenses)
            {
                worksheet.Cell($"A{row}").Value = exp.Title;
                worksheet.Cell($"B{row}").Value = exp.Date;
                worksheet.Cell($"C{row}").Value = ConvertPaymentType(exp.PaymentType);

                worksheet.Cell($"D{row}").Value = exp.Amount;
                worksheet.Cell($"D{row}").Style.NumberFormat.Format = $"-{CURRENCY_SYMBOL} #,##0.00";

                worksheet.Cell($"E{row}").Value = exp.Description;

                row++;
            }

            worksheet.Columns().AdjustToContents();

            var file = new MemoryStream();
            workbook.SaveAs(file);

            return file.ToArray();
        }

        private static void InsertHeader(IXLWorksheet worksheet)
        {
            worksheet.Cell("A1").Value = ReportGenerationMessagesResource.TITLE;
            worksheet.Cell("B1").Value = ReportGenerationMessagesResource.DATE;
            worksheet.Cell("C1").Value = ReportGenerationMessagesResource.PAYMENT_TYPE;
            worksheet.Cell("D1").Value = ReportGenerationMessagesResource.AMOUNT;
            worksheet.Cell("E1").Value = ReportGenerationMessagesResource.DESCRIPTION;

            worksheet.Cells("A1:E1").Style.Font.Bold = true;

            worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#F5C2B6");

            worksheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell("B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell("C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

        }

        private static string ConvertPaymentType(PaymentTypeEnum payment)
        {
            return payment switch
            {
                PaymentTypeEnum.Cash => PaymentTypeMessageResource.CASH,
                PaymentTypeEnum.CreditCard => PaymentTypeMessageResource.CREDIT_CARD,
                PaymentTypeEnum.EletronicTransfer => PaymentTypeMessageResource.ELETRONIC_TARNSFER,
                PaymentTypeEnum.DebitCard => PaymentTypeMessageResource.DEBIT_CARD,
                _ => string.Empty
            };
        }
    }
}