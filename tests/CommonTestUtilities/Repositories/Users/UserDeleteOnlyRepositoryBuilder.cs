using CashFlow.Domain.Entities;
using CashFlow.Domain.Interface.User;
using Moq;

namespace CommonTestUtilities.Repositories.Users
{
    public class UserDeleteOnlyRepositoryBuilder
    {
        private readonly Mock<IUserDeleteOnlyRepository> _repository;

        public UserDeleteOnlyRepositoryBuilder()
        {
            _repository = new Mock<IUserDeleteOnlyRepository>();
        }
        public UserDeleteOnlyRepositoryBuilder DoDeleteAsync(UserEntity user)
        {
            _repository
                .Setup(repo => repo.DeleteAsync(user))
                .Returns(Task.CompletedTask);

            return this;
        }

        public IUserDeleteOnlyRepository Build()
            => _repository.Object;
    }
}
