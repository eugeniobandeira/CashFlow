using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Colors;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using CashFlow.Domain.Extensions;
using CashFlow.Domain.Interface.Expenses;
using CashFlow.Domain.Reports.MessageResource;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using System.Reflection;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf
{
    internal class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
    {
        private readonly IExpensesReadOnlyRepository _repository;
        private const string CURRENCY_SYMBOL = "€";
        private const int HIGHT_ROW_EXPENSE_TABLE = 25;

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

            var document = CreateDocument(month);
            var page = CreatePage(document);

            CreateHeaderWithProfileAndName(page);

            var totalSpent = expenses.Sum(value => value.Amount);
            CreateTotalSpentSection(page, month, totalSpent);

            foreach (var expense in expenses)
            {
                var table = CreateTable(page);

                var row = table.AddRow();
                row.Height = HIGHT_ROW_EXPENSE_TABLE;

                AddExpenseTitle(row.Cells[0], expense.Title);
                AddHeaderForAmount(row.Cells[3]);

                row = table.AddRow();
                row.Height = HIGHT_ROW_EXPENSE_TABLE;

                row.Cells[0].AddParagraph(expense.Date.ToString("D"));
                SetStyleBaseForExpenseDescription(row.Cells[0]);
                row.Cells[0].Format.LeftIndent = 20;

                row.Cells[1].AddParagraph(expense.Date.ToString("t"));
                SetStyleBaseForExpenseDescription(row.Cells[1]);

                row.Cells[2].AddParagraph(expense.PaymentType.PaymentTypeToString());
                SetStyleBaseForExpenseDescription(row.Cells[2]);

                AddAmountForExpense(row.Cells[3], expense.Amount);
                if (!string.IsNullOrEmpty(expense.Description))
                {
                    var descriptionRow = table.AddRow();
                    descriptionRow.Height = HIGHT_ROW_EXPENSE_TABLE;

                    descriptionRow.Cells[0].AddParagraph(expense.Description);

                    descriptionRow.Cells[0].Format.Font = new Font
                    {
                        Name = FontHelper.WORKSANS_REGULAR,
                        Size = 10,
                        Color = ColorsHelper.BLACK
                    };
                    descriptionRow.Cells[0].Shading.Color = ColorsHelper.LIGHT_GREEN;
                    descriptionRow.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                    descriptionRow.Cells[0].MergeRight = 2;
                    descriptionRow.Cells[0].Format.LeftIndent = 20;

                    row.Cells[3].MergeDown = 1;
                }
                    

                AddWhiteSpace(table);
            }

            return RenderDocument(document);
        }

        #region Private methods
        private static Document CreateDocument(DateOnly month)
        {
            Document document = new();

            document.Info.Title = $"{ReportGenerationMessagesResource.EXPENSES_FOR} {month: Y}";
            document.Info.Author = "Eugênio Bandeira";

            var styles = document.Styles["Normal"];
            styles!.Font.Name = FontHelper.RALEWAY_REGULAR;

            return document;
        }

        private static Section CreatePage(Document document)
        {
            var section = document.AddSection();
            section.PageSetup = document.DefaultPageSetup.Clone();
            section.PageSetup.PageFormat = PageFormat.A4;
            section.PageSetup.LeftMargin = 40;
            section.PageSetup.RightMargin = 40;
            section.PageSetup.TopMargin = 80;
            section.PageSetup.BottomMargin = 80;

            return section;
        }

        private static byte[] RenderDocument(Document document)
        {
            var renderer = new PdfDocumentRenderer
            {
                Document = document
            };

            renderer.RenderDocument();

            using var file = new MemoryStream();
            renderer.PdfDocument.Save(file);

            return file.ToArray();
        }

        private static void CreateHeaderWithProfileAndName(Section page)
        {
            var table = page.AddTable();
            table.AddColumn();
            table.AddColumn("300");

            var row = table.AddRow();

            var assembly = Assembly.GetExecutingAssembly();
            var directoryName = Path.GetDirectoryName(assembly.Location);
            var pathFile = Path.Combine(directoryName!, "Logo", "foto-perfil_red.jpg");

            row.Cells[0].AddImage(pathFile);

            row.Cells[1].AddParagraph("Olá, Eugênio Bandeira");
            row.Cells[1].Format.Font = new Font
            {
                Name = FontHelper.RALEWAY_BLACK,
                Size = 16
            };
            row.Cells[1].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;
        }

        private static void CreateTotalSpentSection(Section page, DateOnly month, decimal totalExpenses)
        {
            var paragraph = page.AddParagraph();
            paragraph.Format.SpaceBefore = "40";
            paragraph.Format.SpaceAfter = "40";

            var title = string.Format(ReportGenerationMessagesResource.TOTAL_SPENT_IN, month.ToString("Y"));

            paragraph.AddFormattedText(title, new Font
            {
                Name = FontHelper.RALEWAY_REGULAR,
                Size = 15
            });

            paragraph.AddLineBreak();
                     
            paragraph.AddFormattedText($"{totalExpenses} {CURRENCY_SYMBOL}", new Font
            {
                Name = FontHelper.WORKSANS_BLACK,
                Size = 50
            });
        }

        private static Table CreateTable(Section page)
        {
            var table = page.AddTable();

            table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
            table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center; ;
            table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
            table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;

            return table;
        }

        private static void AddExpenseTitle(Cell cell, string expenseTitle)
        {
            cell.AddParagraph(expenseTitle);
            cell.Format.Font = new Font
            {
                Name = FontHelper.RALEWAY_BLACK,
                Size = 14,
                Color = ColorsHelper.BLACK
            };
            cell.Shading.Color = ColorsHelper.LIGHT_RED;
            cell.VerticalAlignment = VerticalAlignment.Center;
            cell.MergeRight = 2;
            cell.Format.LeftIndent = 20;
        }

        private static void AddHeaderForAmount(Cell cell)
        {
            cell.AddParagraph(ReportGenerationMessagesResource.AMOUNT);
            cell.Format.Font = new Font
            {
                Name = FontHelper.RALEWAY_BLACK,
                Size = 14,
                Color = ColorsHelper.WHITE
            };
            cell.Shading.Color = ColorsHelper.DARK_RED;
            cell.VerticalAlignment = VerticalAlignment.Center;
        }

        private static void SetStyleBaseForExpenseDescription(Cell cell)
        {
            cell.Format.Font = new Font
            {
                Name = FontHelper.WORKSANS_REGULAR,
                Size = 12,
                Color = ColorsHelper.BLACK
            };
            cell.Shading.Color = ColorsHelper.DARK_GREEN;
            cell.VerticalAlignment = VerticalAlignment.Center;
        }

        private static void AddAmountForExpense(Cell cell, decimal amount)
        {
            cell.AddParagraph($"-{amount} {CURRENCY_SYMBOL}");
            cell.Format.Font = new Font
            {
                Name = FontHelper.WORKSANS_REGULAR,
                Size = 14,
                Color = ColorsHelper.BLACK
            };
            cell.Shading.Color = ColorsHelper.WHITE;
            cell.VerticalAlignment = VerticalAlignment.Center;
        }

        private static void AddWhiteSpace(Table table)
        {
            var row = table.AddRow();
            row.Height = 30;
            row.Borders.Visible = false;
        }
        #endregion
    }
}
