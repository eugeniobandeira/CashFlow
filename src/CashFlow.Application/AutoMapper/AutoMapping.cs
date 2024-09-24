using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Communication.Responses.Register;
using CashFlow.Domain.Entities;

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
            CreateMap<InsertExpenseRequest, ExpenseEntity>();
        }

        private void EntityToResponse()
        {
            CreateMap<ExpenseEntity, RegisteredExpenseResponse>();
            CreateMap<ExpenseEntity, ShortExpenseResponse>();
        }
    }
}
