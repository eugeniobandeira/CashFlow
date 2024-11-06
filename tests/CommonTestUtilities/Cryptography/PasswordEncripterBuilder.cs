using CashFlow.Domain.Interface.Security.Cryptography;
using Moq;

namespace CommonTestUtilities.Cryptography
{
    public class PasswordEncripterBuilder
    {
        public static IPasswordEncripter Build()
        {
            var mock = new Mock<IPasswordEncripter>();

            mock.Setup(pass =>
            pass.Encrypt(It.IsAny<string>()))
                .Returns("@a*2tl5#7");

            return mock.Object;
        }
    }
}
