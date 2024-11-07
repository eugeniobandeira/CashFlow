using CashFlow.Domain.Interface.Security.Cryptography;
using Moq;

namespace CommonTestUtilities.Cryptography
{
    public class PasswordEncrypterBuilder
    {
        private readonly Mock<IPasswordEncripter> _mock;

        public PasswordEncrypterBuilder()
        {
            _mock = new Mock<IPasswordEncripter>();

            _mock.Setup(pass =>
            pass.Encrypt(It.IsAny<string>()))
                .Returns("@a*2tl5#7");
        }

        public IPasswordEncripter Build()
            => _mock.Object;

        public PasswordEncrypterBuilder Verify(string? password)
        {
            if (string.IsNullOrWhiteSpace(password) is false)
                _mock.Setup(pass => pass.Verify(password, It.IsAny<string>())).Returns(true);

            return this; 
        }
 
    }
}
