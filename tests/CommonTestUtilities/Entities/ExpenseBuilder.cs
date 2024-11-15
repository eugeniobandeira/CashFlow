using CashFlow.Domain.Entities;
using Bogus;
using CashFlow.Domain.Enums;

namespace CommonTestUtilities.Entities
{
    public class ExpenseBuilder
    {
        public static List<ExpenseEntity> Collection(UserEntity user, uint count = 2)
        {
            var list = new List<ExpenseEntity>();

            if (count ==  0)
                count = 1;

            var expenseId = 1;

            for (int i = 0; i < count; i++)
            {
                var expense = Build(user);
                expense.Id = expenseId++;

                list.Add(expense);
            }

            return list;
        }

        public static ExpenseEntity Build(UserEntity user)
        {
            return new Faker<ExpenseEntity>()
                .RuleFor(user => user.Id, _ => 1)
                .RuleFor(user => user.Title, faker => faker.Commerce.ProductName())
                .RuleFor(user => user.Description, faker => faker.Commerce.ProductDescription())
                .RuleFor(user => user.Date, faker => faker.Date.Past())
                .RuleFor(user => user.Amount, faker => faker.Random.Decimal(min: 1, max: 1000))
                .RuleFor(user => user.PaymentType, faker => faker.PickRandom<PaymentTypeEnum>())
                .RuleFor(user => user.UserId, _ => user.Id);

        }
    }
}
