using CashFlow.Domain.Interface.Security.Cryptography;
using CashFlow.Domain.Interface.Security.Tokens;
using CashFlow.Domain.Interface.User;
using CashFlow.Domain.Requests.Login;
using CashFlow.Domain.Responses.Users;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Login
{
    public class DoLoginUseCase : IDoLoginUseCase
    {
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IPasswordEncripter _passwordEncripter;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public DoLoginUseCase(
            IUserReadOnlyRepository userReadOnlyRepository,
            IPasswordEncripter passwordEncripter,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userReadOnlyRepository = userReadOnlyRepository;
            _passwordEncripter = passwordEncripter;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<RegisteredUserResponse> Execute(LoginRequest req)
        {
            var user = await _userReadOnlyRepository.GetUserByEmail(req.Email) 
                ?? throw new InvalidLoginException();

            var passwordMatch = _passwordEncripter.Verify(req.Password, user.Password);

            if (passwordMatch is false)
                throw new InvalidLoginException();

            return new RegisteredUserResponse
            {
                Name = user.Name,
                Token = _jwtTokenGenerator.Generate(user)
            };
        }
    }
}
