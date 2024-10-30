using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Interface.Security.Tokens
{
    public interface IJwtTokenGenerator
    {
        string Generate(UserEntity userEntity);
    }
}
