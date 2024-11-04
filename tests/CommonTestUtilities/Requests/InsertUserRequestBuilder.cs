using Bogus;
using CashFlow.Domain.Requests.Users;

namespace CommonTestUtilities.Requests
{
    public class InsertUserRequestBuilder
    {
        public static UserRequest Build()
        {
            return new Faker<UserRequest>()
                .RuleFor(user => user.Name, faker => faker.Person.FirstName)
                .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name))
                .RuleFor(user => user.Password, faker => faker.Internet.Password(prefix: "!Aa1"));
        }
    }
}
