using FluentValidation;
using WebApi.Models.Users;

namespace AuctionsDenis.Validators;

public class RegisterRequestValidator:AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(u => u.Password).MinimumLength(8);
        RuleFor(u => u.UserName).NotNull().MinimumLength(3).MaximumLength(20);
        RuleFor(u => u.FirstName).NotNull().NotEmpty();
        RuleFor(u => u.LastName).NotNull().NotEmpty();

    }
}

