using Bogus;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Cryptography;

namespace CommonTestUtilities.Entities
{
    public class UserBuilder
    {
        public static UserEntity Build()
        {
            var passwordEncripter = PasswordEncripterBuilder.Build();

            var user = new Faker<UserEntity>()
                .RuleFor(usr => usr.Id, _ => 1)
                .RuleFor(usr => usr.Name, faker => faker.Person.FirstName)
                .RuleFor(usr => usr.Email, (faker, user) => faker.Internet.Email(user.Name))
                .RuleFor(usr => usr.Password, (_, user) => passwordEncripter.Encrypt(user.Password))
                .RuleFor(usr => usr.UserId, _ => Guid.NewGuid());

            return user;
        }
    }
}
