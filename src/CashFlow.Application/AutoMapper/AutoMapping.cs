using AutoMapper;
using CashFlow.Communication.Requests;
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
            => CreateMap<InsertExpenseRequest, ExpenseEntity>();

        private void EntityToResponse() 
            => CreateMap<ExpenseEntity, InsertExpenseRequest>();
    }
}
