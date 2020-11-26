using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Stexchange.Data.Models;

namespace Stexchange.Data.Validation
{
    public class ListingValidator : AbstractValidator<Listing>
    {
        public ListingValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Title.Length <=80);

            RuleFor(x => x.Quantity).NotNull();
            RuleFor(x => x.Quantity).GreaterThan(0);

            RuleFor(x => x.NameLatin.Length < 50);

            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Description.Length <= 500);

            RuleFor(x => x.NameNl).NotEmpty();
            RuleFor(x => x.NameNl.Length <= 50);
            
            RuleFor(x => x.Quantity).GreaterThan(0);


        }
        
    }
}
