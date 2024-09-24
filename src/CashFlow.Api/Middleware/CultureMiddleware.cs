using System.Globalization;

namespace CashFlow.Api.Middleware
{
    /// <summary>
    /// Middleware responsible for identifying the language
    /// </summary>
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Delegate
        /// </summary>
        /// <param name="next"></param>
        public CultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var supportedLanguages = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();

            var requestedCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();

            var cultureInfo = new CultureInfo("en");

            if (!string.IsNullOrWhiteSpace(requestedCulture) 
                && supportedLanguages
                .Exists(lang => lang.Name
                .Equals(requestedCulture)))
            {
                cultureInfo = new CultureInfo(requestedCulture);
            }
                

            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;

            await _next(context);
        }
            
    }
}
