namespace CashFlow.Domain.Interface.Security.Tokens
{
    public interface ITokenProvider
    {
        string TokenOnRequest();
    }
}
