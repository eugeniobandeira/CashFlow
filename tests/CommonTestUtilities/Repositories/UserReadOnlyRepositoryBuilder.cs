using CashFlow.Domain.Interface.User;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class UserReadOnlyRepositoryBuilder
    {
        private readonly Mock<IUserReadOnlyRepository> _repository;

        public UserReadOnlyRepositoryBuilder()
        {
            _repository = new Mock<IUserReadOnlyRepository>();
        }

        public void ExistUserWithEmail(string email)
        {
            _repository.Setup(userReadOnly => userReadOnly.ExistUserWithEmail(email)).ReturnsAsync(true);
        }

        public IUserReadOnlyRepository Build() => _repository.Object;
    }
}
