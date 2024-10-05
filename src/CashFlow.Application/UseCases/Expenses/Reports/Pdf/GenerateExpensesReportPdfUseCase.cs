using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using CashFlow.Domain.Interface.Expenses;
using CashFlow.Domain.Reports.MessageResource;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
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

            var document = CreateDocument(month);
            var page = CreatePage(document);

            var table = page.AddTable();
            table.AddColumn();
            table.AddColumn("300");

            var row = table.AddRow();
            row.Cells[0].AddImage("C:\\Users\\Eugenio\\Downloads\\foto-perfil_red.jpg");
            row.Cells[1].AddParagraph("Olá, Eugênio Bandeira");
            row.Cells[1].Format.Font = new Font
            {
                Name = FontHelper.RALEWAY_BLACK, 
                Size = 16
            };
            row.Cells[1].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;

            var paragraph = page.AddParagraph();
            paragraph.Format.SpaceBefore = "40";
            paragraph.Format.SpaceAfter = "40";

            var title = string.Format(ReportGenerationMessagesResource.TOTAL_SPENT_IN, month.ToString("Y"));

            paragraph.AddFormattedText(title, new Font { 
                Name = FontHelper.RALEWAY_REGULAR, 
                Size = 15
            });

            paragraph.AddLineBreak();

            var totalSpent = expenses.Sum(value => value.Amount);

            paragraph.AddFormattedText($"{totalSpent} {CURRENCY_SYMBOL}", new Font
            {
                Name = FontHelper.WORKSANS_BLACK,
                Size = 50
            });

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
        #endregion
    }
}
