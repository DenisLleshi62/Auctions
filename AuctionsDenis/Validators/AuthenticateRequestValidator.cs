using FluentValidation;
using WebApi.Models.Users;

namespace AuctionsDenis.Validators;

public class AuthenticateRequestValidator:AbstractValidator<AuthenticateRequest>
{
    public AuthenticateRequestValidator()
    {
        RuleFor(u => u.Password).MinimumLength(8);
        RuleFor(u => u.UserName).NotNull().MinimumLength(3).MaximumLength(20);
           
    }
}



