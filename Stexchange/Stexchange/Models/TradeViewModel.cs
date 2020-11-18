using Stexchange.Controllers;
using Stexchange.Data;
using Stexchange.Data.Builders;
using Stexchange.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stexchange.Models
{
    public class TradeViewModel
    {
        private Database db;
        private List<Listing> listingCache;
        private Dictionary<int, User> userCache;

        public TradeViewModel(Database db)
        {
            this.db = db;
        }

        public void renewListingCache(ref List<Listing> cache)
        {
            cache = (from listing in db.Listings
                     select new ListingBuilder(listing)
                        .SetProperty("Pictures",
                            (from img in db.Images
                             where img.ListingId == listing.Id
                             select img).ToList())
                        .SetProperty("Categories",
                            (from filter in db.FilterListings
                             where filter.ListingId == listing.Id
                             select filter.Value).ToList())
                        .SetProperty("Owner", userCache[listing.Id])
                        .Complete()
                        ).ToList();
        }

        public void renewUserCache(ref List<User> cache)
        {
            cache = (from user in db.Users
                     join listing in db.Listings on user.Id equals listing.UserId
                     select user).ToList();
        }

        public List<Listing> RetrieveListings(long token)
        {
            if(ServerController.GetSessionData(token, out Tuple<int, string> sessionData)) {
                listingCache.ForEach(listing => listing.SetDistance(sessionData.Item2));
            } else
            {
                listingCache.ForEach(listing => listing.Distance = -1);
            }
            return listingCache;
        }
    }
}
