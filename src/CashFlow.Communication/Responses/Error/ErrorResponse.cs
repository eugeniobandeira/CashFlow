namespace CashFlow.Communication.Responses.Error
{
    public class ErrorResponse
    {
        public List<string> ErrorMessage { get; set; }
        public ErrorResponse(string errorMessage)
        {
            ErrorMessage = [];
        }

        public ErrorResponse(List<string> errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
