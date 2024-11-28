﻿using CashFlow.Domain.Requests.Users;
using FluentValidation;

namespace CashFlow.Application.UseCases.Users.ChangePassword
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordValidator()
        {
            RuleFor(pass => pass.NewPassword).SetValidator(new PasswordValidator<ChangePasswordRequest>());
        }
    }
}
