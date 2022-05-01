using AuctionsProject.Models.Products;
using FluentValidation;

namespace AuctionsDenis.Validators;

public class UpdateProductValidator:AbstractValidator<UpdateProduct>
{ 
    public UpdateProductValidator()
    {
        RuleFor(u => u.ProductName).NotEmpty();
        RuleFor(u => u.FinishingDate).GreaterThan(DateTime.Now);
           
    }
}