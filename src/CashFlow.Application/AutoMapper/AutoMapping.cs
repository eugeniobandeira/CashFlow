using AutoMapper;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Requests.Expenses;
using CashFlow.Domain.Requests.Users;
using CashFlow.Domain.Responses.Expenses;
using CashFlow.Domain.Responses.Register;


namespace CashFlow.Application.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            RequestToEntity();
            EntityToResponse();
        }

        private void RequestToEntity()
        {
            CreateMap<ExpenseRequest, ExpenseEntity>();
            CreateMap<UserRequest, UserEntity>()
                .ForMember(dest => dest.Password, config => config.Ignore());
        }

        private void EntityToResponse()
        {
            CreateMap<ExpenseEntity, RegisteredExpenseResponse>();
            CreateMap<ExpenseEntity, ShortExpenseResponse>();
        }
    }
}
