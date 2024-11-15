using Bogus;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Helper;
using CommonTestUtilities.Cryptography;

namespace CommonTestUtilities.Entities
{
    public class UserBuilder
    {
        public static UserEntity Build(string role = RolesHelper.ADMIN)
        {
            var passwordEncripter = new PasswordEncrypterBuilder().Build();

            var user = new Faker<UserEntity>()
                .RuleFor(usr => usr.Id, _ => 1)
                .RuleFor(usr => usr.Name, faker => faker.Person.FirstName)
                .RuleFor(usr => usr.Email, (faker, user) => faker.Internet.Email(user.Name))
                .RuleFor(usr => usr.Password, (_, user) => passwordEncripter.Encrypt(user.Password))
                .RuleFor(usr => usr.UserId, _ => Guid.NewGuid())
                .RuleFor(usr => usr.Role, _ => role);

            return user;
        }
    }
}
