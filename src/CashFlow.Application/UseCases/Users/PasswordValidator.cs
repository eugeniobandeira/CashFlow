using CashFlow.Exception;
using FluentValidation;
using FluentValidation.Validators;
using System.Text.RegularExpressions;

namespace CashFlow.Application.UseCases.Users
{
    public partial class PasswordValidator<T> : PropertyValidator<T, string>
    {
        private const string ERROR_MESSAGE_KEY = "ErrorMessage";
        public override string Name => "PasswordValidator";

        public override bool IsValid(ValidationContext<T> context, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ErrorMessageResource.INVALID_PASSWORD_NULL_OR_WHITE_SPACE);
                return false;
            }

            if (password.Length < 8)
            {
                context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ErrorMessageResource.INVALID_PASSWORD_LENGTH);
                return false;
            }

            if (UpperCaseLetter().IsMatch(password) == false)
            {
                context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ErrorMessageResource.INVALID_PASSWORD_MISSING_UPPER_CASE_LETTER);
                return false;
            }

            if (LowerCaseLetter().IsMatch(password) == false)
            {
                context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ErrorMessageResource.INVALID_PASSWORD_MISSING_LOWER_CASE_LETTER);
                return false;
            }

            if (Numbers().IsMatch(password) == false)
            {
                context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ErrorMessageResource.INVALID_PASSWORD_MISSING_NUMBERS);
                return false;
            }

            if (SpecialCharactere().IsMatch(password) == false)
            {
                context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ErrorMessageResource.INVALID_PASSWORD_MISSING_SPECIAL_CHARACTERE);
                return false;
            }

            return true;
        }

        #region Regex
        [GeneratedRegex(@"[A-Z]+")]
        private static partial Regex UpperCaseLetter();

        [GeneratedRegex(@"[a-z]+")]
        private static partial Regex LowerCaseLetter();
        [GeneratedRegex(@"[0-9]+")]
        private static partial Regex Numbers();

        [GeneratedRegex(@"[\!\?\*\.\@\#\$\%\&]+")]
        private static partial Regex SpecialCharactere();

        #endregion
    }
}
