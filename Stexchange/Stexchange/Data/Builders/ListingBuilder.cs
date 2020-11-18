using Stexchange.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Stexchange.Data.Builders
{
    public class ListingBuilder
    {
        private Listing construct;

        public ListingBuilder()
        {
            Reset();
        }

        public void Reset()
        {
            construct = new Listing();
        }

        public Listing Complete() => construct;

        public ListingBuilder SetProperty(PropertyInfo propertyInfo, object value)
        {
            if(value is object && value.GetType().Equals(propertyInfo.PropertyType))
            {
                propertyInfo.SetValue(this, value);
            }
            return this;
        }

        public ListingBuilder SetProperty(string propertyName, object value)
        {
            if(propertyName is object)
            {
                var propertyInfo = typeof(Listing).GetProperty(propertyName);
                if (propertyInfo is object)
                {
                    return SetProperty(propertyInfo, value);
                }
            }
            return this;
        }
    }
}
