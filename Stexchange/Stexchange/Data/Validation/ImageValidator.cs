using FluentValidation;
using Stexchange.Data.Models;

namespace Stexchange.Data.Validation
{
    public class ImageValidator : AbstractValidator<ImageData>
    {
        public ImageValidator()
        {
            RuleFor(x => x.Image).NotNull();
            
        }
    }
}
