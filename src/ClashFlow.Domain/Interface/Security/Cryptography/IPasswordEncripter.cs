namespace CashFlow.Domain.Interface.Security.Cryptography
{
    public interface IPasswordEncripter
    {
        string Encrypt(string password);
    }
}
