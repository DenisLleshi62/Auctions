using AuctionsProject.Models.Products;
using FluentValidation;

namespace AuctionsDenis.Validators;

public class CreateProductsValidator:AbstractValidator<CreateProducts>
{ 
    public CreateProductsValidator()
    {
        RuleFor(u => u.ProductName).NotEmpty();
        RuleFor(u => u.StartingBid).GreaterThan(0);
        RuleFor(u => u.FinishingDate).GreaterThan(DateTime.Now);
           
    }
    
}
