using FluentValidation;
using Stexchange.Data.Models;

namespace Stexchange.Data.Validation
{
    public class ListingValidator : AbstractValidator<Listing>
    {
        public ListingValidator()
        {
            RuleFor(x => x.Title.Trim()).NotEmpty();
            RuleFor(x => x.Title.Trim().Length <=80);
            RuleFor(x => x.Title.Trim()).Matches(@"[\w\s]+");

            RuleFor(x => x.Quantity).NotNull();
            RuleFor(x => x.Quantity).GreaterThan(0);

            RuleFor(x => x.NameLatin.Length < 50);
            RuleFor(x => x.NameLatin).Matches(@"[\w\s]+");

            RuleFor(x => x.Description.Trim()).NotEmpty();
            RuleFor(x => x.Description.Trim().Length <= 500);
            RuleFor(x => x.Description.Trim().Length >= 50);

            RuleFor(x => x.NameNl.Trim()).NotEmpty();
            RuleFor(x => x.NameNl.Trim().Length <= 50);
            RuleFor(x => x.NameNl.Trim()).Matches(@"[\w\s]+");

            RuleFor(x => x.Quantity).GreaterThan(0);


        }
        
    }
}
