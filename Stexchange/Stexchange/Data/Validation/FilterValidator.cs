using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using Stexchange.Data.Models;

namespace Stexchange.Data.Validation
{
    public class PhFilterValidator : AbstractValidator<Filter>
    {
        private List<string> valueList = new List<string>(){$"null", "kalkrijk", "zuur", "neutraal"};
        public PhFilterValidator()
        {
            RuleFor(x => valueList.Contains(x.Value));
        }
    }

    public class WaterFilterValidator : AbstractValidator<Filter>
    {
        private List<string> valueList = new List<string>() { $"null", "wisselnat", "vochtig", "matig_vochtig", "droog"  };
        public WaterFilterValidator()
        {
            RuleFor(x => valueList.Contains(x.Value));
        }
    }

    public class NutrientsFilterValidator : AbstractValidator<Filter>
    {
        private List<string> valueList = new List<string>() { $"null", "veel", "gemiddeld", "weinig" };
        public NutrientsFilterValidator()
        {
            RuleFor(x => valueList.Contains(x.Value));
        }
    }

    public class LightFilterValidator : AbstractValidator<Filter>
    {
        private List<string> valueList = new List<string>() { $"zonlicht", "halfschaduw", "schaduw" };
        public LightFilterValidator()
        {
            RuleFor(x => valueList.Contains(x.Value));
        }
    }

    public class IndigenousFilterValidator : AbstractValidator<Filter>
    {
        private List<string> valueList = new List<string>() { $"null", "inheems", "niet_inheems" };
        public IndigenousFilterValidator()
        {
            RuleFor(x => valueList.Contains(x.Value));
        }
    }

    public class WithPotFilterValidator : AbstractValidator<Filter>
    {
        private List<string> valueList = new List<string>() { $"ja", "nee" };
        public WithPotFilterValidator()
        {
            RuleFor(x => valueList.Contains(x.Value));
        }
    }

    public class GiveAwayFilterValidator : AbstractValidator<Filter>
    {
        private List<string> valueList = new List<string>() { $"null", "ja", "nee" };
        public GiveAwayFilterValidator()
        {
            RuleFor(x => valueList.Contains(x.Value));
        }
    }

    public class PlantTypeFilterValidator : AbstractValidator<Filter>
    {
        private List<string> valueList = new List<string>() { $"plant", "zaad", "stek" };
        public PlantTypeFilterValidator()
        {
            RuleFor(x => valueList.Contains(x.Value));
        }
    }
}
