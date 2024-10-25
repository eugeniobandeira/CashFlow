namespace CashFlow.Domain.Interface.Security
{
    public interface IPasswordEncripter
    {
        string Encrypt(string password);
    }
}
