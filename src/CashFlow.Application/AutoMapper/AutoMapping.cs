using AutoMapper;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Requests.Expenses;
using CashFlow.Domain.Requests.Users;
using CashFlow.Domain.Responses.Expenses;
using CashFlow.Domain.Responses.Register;
using CashFlow.Domain.Responses.Users;


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
            CreateMap<UserRequest, UserEntity>()
                .ForMember(dest => dest.Password, config => config.Ignore());

            CreateMap<ExpenseRequest, ExpenseEntity>()
                .ForMember(dest => dest.Tags, config => config.MapFrom(src => src.Tags.Distinct()));

            CreateMap<TagEnum, TagEntity>()
                .ForMember(dest => dest.Value, config => config.MapFrom(src => src));
        }

        private void EntityToResponse()
        {
            CreateMap<ExpenseEntity, RegisteredExpenseResponse>();
            CreateMap<ExpenseEntity, ShortExpenseResponse>();
            CreateMap<UserEntity, UserProfileResponse>();
        }
    }
}
