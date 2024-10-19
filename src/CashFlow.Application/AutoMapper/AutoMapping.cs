using AutoMapper;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Requests;
using CashFlow.Domain.Responses;
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
        }

        private void EntityToResponse()
        {
            CreateMap<ExpenseEntity, RegisteredExpenseResponse>();
            CreateMap<ExpenseEntity, ShortExpenseResponse>();
        }
    }
}
