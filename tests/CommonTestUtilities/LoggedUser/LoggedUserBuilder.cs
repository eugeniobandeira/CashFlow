using CashFlow.Domain.Entities;
using CashFlow.Domain.Interface.Service.LoggedUser;
using Moq;

namespace CommonTestUtilities.LoggedUser
{
    public class LoggedUserBuilder
    {
        public static ILoggedUser Build(UserEntity userEntity)
        {
            var mock = new Mock<ILoggedUser>();

            mock.Setup(loggedUser => loggedUser.GetAsync()).ReturnsAsync(userEntity);

            return mock.Object;
        }
    }
}
