using CashFlow.Domain.Entities;
using CashFlow.Domain.Interface.Security.Tokens;
using Moq;

namespace CommonTestUtilities.Token
{
    public class JwtTokenGeneratorBuilder
    {

        public static IJwtTokenGenerator Build()
        {
            var mock = new Mock<IJwtTokenGenerator>();

            mock.Setup(tokenGenerator => 
            tokenGenerator
            .Generate(It.IsAny<UserEntity>()))
                .Returns("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c");

            return mock.Object;
        }
    }
}
