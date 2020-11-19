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

        /// <summary>
        /// Initializes a ListingBuilder with a clean construct.
        /// </summary>
        public ListingBuilder()
        {
            Reset();
        }

        /// <summary>
        /// Initializes a ListingBuilder with an existing construct.
        /// </summary>
        /// <param name="existing"></param>
        public ListingBuilder(Listing existing)
        {
            construct = existing;
        }

        /// <summary>
        /// Resets this ListingBuilder to a clean construct.
        /// </summary>
        public void Reset()
        {
            construct = new Listing();
        }

        /// <summary>
        /// Returns the construct of this ListingBuilder.
        /// </summary>
        /// <returns></returns>
        public Listing Complete() => construct;

        /// <summary>
        /// Sets the specified property to the specified value.
        /// </summary>
        /// <param name="propertyInfo">PropertyInfo Object representing the property to set</param>
        /// <param name="value">The value for the property</param>
        /// <returns></returns>
        public ListingBuilder SetProperty(PropertyInfo propertyInfo, object value)
        {
            if(value.GetType().Equals(propertyInfo.PropertyType))
            {
                propertyInfo.SetValue(this, value);
            }
            return this;
        }

        /// <summary>
        /// Sets the specified property to the specified value.
        /// </summary>
        /// <param name="propertyName">The name of the property to set</param>
        /// <param name="value">The value for the property</param>
        /// <returns></returns>
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
