using CashFlow.Domain.Entities;
using CashFlow.Domain.Interface.User;
using Moq;

namespace CommonTestUtilities.Repositories.Users
{
    public class UserUpdateOnlyRepositoryBuilder
    {
        public static IUserUpdateOnlyRepository Build(UserEntity user)
        {
            var mock = new Mock<IUserUpdateOnlyRepository>();

            mock.Setup(repository => repository.GetByIdAsync(user.Id)).ReturnsAsync(user);

            return mock.Object;
        }
    }
}
