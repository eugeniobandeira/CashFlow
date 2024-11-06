using AutoMapper;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Interface;
using CashFlow.Domain.Interface.Security.Cryptography;
using CashFlow.Domain.Interface.Security.Tokens;
using CashFlow.Domain.Interface.User;
using CashFlow.Domain.Requests.Users;
using CashFlow.Domain.Responses.Users;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.Register
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IMapper _mapper;
        private readonly IPasswordEncripter _passwordEncripter;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenGenerator _tokenGenerator;

        public RegisterUserUseCase(
            IMapper mapper, 
            IPasswordEncripter passwordEncripter,
            IUserReadOnlyRepository userReadOnlyRepository,
            IUnitOfWork unitOfWork,
            IUserWriteOnlyRepository userWriteOnlyRepository,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _mapper = mapper;
            _passwordEncripter = passwordEncripter;
            _userReadOnlyRepository = userReadOnlyRepository;
            _userWriteOnlyRepository = userWriteOnlyRepository;
            _unitOfWork = unitOfWork;
            _tokenGenerator = jwtTokenGenerator;
        }

        public async Task<RegisteredUserResponse> Execute(UserRequest req)
        {
            await Validate(req);

            var user = _mapper.Map<UserEntity>(req);
            user.Password = _passwordEncripter.Encrypt(req.Password);
            user.UserId = Guid.NewGuid();

            await _userWriteOnlyRepository.Add(user);

            await _unitOfWork.Commit();

            return new RegisteredUserResponse
            {
                Name = user.Name,
                Token = _tokenGenerator.Generate(user)
            };
        }

        private async Task Validate(UserRequest req)
        {
            var result = new UserValidator().Validate(req);

            var isEmailValid = await _userReadOnlyRepository.ExistUserWithEmail(req.Email);
            if (isEmailValid)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, ErrorMessageResource.EMAIL_ALREADY_REGISTERED));
            }

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
