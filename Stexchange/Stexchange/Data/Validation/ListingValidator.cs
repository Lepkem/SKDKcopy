using FluentValidation;
using Stexchange.Data.Helpers;
using Stexchange.Data.Models;

namespace Stexchange.Data.Validation
{
    public class ListingValidator : AbstractValidator<Listing>
    {
        public ListingValidator()
        {
            RuleFor(x => x.Title.Trim()).NotEmpty().WithErrorCode(StandardMessages.RequiredField("title"));
            RuleFor(x => x.Title.Trim().Length).LessThanOrEqualTo(80).WithErrorCode(StandardMessages.AmountOfCharacters());
            RuleFor(x => x.Title.Trim()).Matches(@"[\w\s]+");

            RuleFor(x => x.Quantity).NotNull().WithErrorCode(StandardMessages.RequiredField("hoeveelheid"));
            RuleFor(x => x.Quantity).GreaterThan(0);

            RuleFor(x => x.NameLatin.Trim().Length).LessThanOrEqualTo(Listing.MaxLtNameLength).WithErrorCode(StandardMessages.AmountOfCharacters());
            RuleFor(x => x.NameLatin).Matches(@"[\w\s]+");

            RuleFor(x => x.Description.Trim()).NotEmpty().WithErrorCode(StandardMessages.RequiredField("beschrijving")); 
            RuleFor(x => x.Description.Trim().Length).LessThanOrEqualTo(Listing.MinDescriptionSize).WithErrorCode(StandardMessages.AmountOfCharacters());
            RuleFor(x => x.Description.Trim().Length).GreaterThanOrEqualTo(Listing.MaxDescriptionSize).WithErrorCode(StandardMessages.AmountOfCharacters());

            RuleFor(x => x.NameNl.Trim()).NotEmpty().WithErrorCode(StandardMessages.RequiredField("de Nederlandse naam")); 
            RuleFor(x => x.NameNl.Trim().Length).LessThanOrEqualTo(Listing.MaxNlNameLength).WithErrorCode(StandardMessages.AmountOfCharacters());
            RuleFor(x => x.NameNl.Trim()).Matches(@"[\w\s]+");


        }
        
    }
}
