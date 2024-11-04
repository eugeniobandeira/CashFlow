using CashFlow.Domain.Interface.Security.Cryptography;
using BC = BCrypt.Net.BCrypt;

namespace CashFlow.Infrastructure.Security.Cryptography
{
    internal class PasswordEncripter : IPasswordEncripter
    {
        public string Encrypt(string password)
        {
            string passwordHash = BC.HashPassword(password);

            return passwordHash;
        }

        public bool Verify(string password, string passwordHash) 
            => BC.Verify(password, passwordHash);
    }
}
