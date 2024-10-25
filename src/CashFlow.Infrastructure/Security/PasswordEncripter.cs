using CashFlow.Domain.Interface.Security;
using BC = BCrypt.Net.BCrypt;

namespace CashFlow.Infrastructure.Security
{
    internal class PasswordEncripter: IPasswordEncripter
    {
        public string Encrypt(string password)
        {
            string passwordHash = BC.HashPassword(password);
            return passwordHash;
        }
    }
}
