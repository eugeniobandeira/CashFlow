using CashFlow.Domain.Entities;
using CashFlow.Domain.Interface.User;
using Moq;

namespace CommonTestUtilities.Repositories.User
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

        public UserReadOnlyRepositoryBuilder GetUserByEmail(UserEntity userEntity)
        {
            _repository.Setup(repo => repo.GetUserByEmail(userEntity.Email)).ReturnsAsync(userEntity);

            return this;
        }
    }
}
