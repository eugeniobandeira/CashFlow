using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Expenses;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Test.Expenses.Register
{
    public class RegisterExpenseUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            var loggedUser = UserBuilder.Build();
            var req = InsertExpenseRequestBuilder.Build();
            var useCase = CreateUseCase(loggedUser);

            //Act
            var result = await useCase.Execute(req);

            //Assert
            result.Should().NotBeNull();
            result.Title.Should().Be(req.Title);
            result.Description.Should().Be(req.Description);
            result.Amount.Should().Be(req.Amount);
        }

        [Fact]
        public async Task Empty_Title_Error()
        {
            //Arrange
            var loggedUser = UserBuilder.Build();
            var req = InsertExpenseRequestBuilder.Build();
            req.Title = string.Empty;
            var useCase = CreateUseCase(loggedUser);

            //Act
            var act = async () => await useCase.Execute(req);

            //Assert
            var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
            result.Where(ex => ex.GetErrors().Count() == 1 && ex.GetErrors()
                .Contains(ErrorMessageResource.TITLE_REQUIRED));
        }

        #region UseCase Builder
        private static RegisterExpenseUseCase CreateUseCase(UserEntity userEntity)
        {
            var repository = ExpensesWriteOnlyRepositoryBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var mapper = MapperBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(userEntity);

            return new RegisterExpenseUseCase(repository, unitOfWork, mapper, loggedUser);
        }
        #endregion
    }
}
