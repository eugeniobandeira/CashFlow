using PdfSharp.Fonts;
using System.Reflection;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts
{
    public class ExpensesReportFontsResolver : IFontResolver
    {
        public byte[]? GetFont(string faceName)
        {
            var stream = ReadFontFile(faceName) ?? ReadFontFile(FontHelper.DEFAULT);

            var lenght = (int)stream!.Length;

            var data = new byte[lenght];

            stream.Read(buffer: data, offset: 0, count: lenght);

            return data;
        }

        public FontResolverInfo? ResolveTypeface(string familyName, bool bold, bool italic)
        {
            return new FontResolverInfo(familyName);
        }

        private static Stream? ReadFontFile(string faceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            return assembly.GetManifestResourceStream($"CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts.{faceName}.ttf");
        }
    }
}
