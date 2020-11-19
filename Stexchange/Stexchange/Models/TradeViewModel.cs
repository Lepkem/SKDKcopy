using Microsoft.Extensions.Logging;
using Stexchange.Controllers;
using Stexchange.Data;
using Stexchange.Data.Builders;
using Stexchange.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Stexchange.Models
{
    public sealed class TradeViewModel : IDisposable
    {
        private Database db;
        private ILogger log;
        private Thread cacheWorker;

        private List<Listing> listingCache;
        private Dictionary<int, User> userCache;

        public TradeViewModel(Database db, ILogger<TradeViewModel> logger)
        {
            this.db = db;
            log = logger;
            cacheWorker = new Thread(() =>
            {
                var task = Run();
                task.Wait();
            });
            cacheWorker.Start();
        }

        /// <summary>
        /// Updates the state of Listings in the database that is stored in private field of this object.
        /// </summary>
        /// <param name="cache">Reference to the private field.</param>
        private void renewListingCache(ref List<Listing> cache)
        {
            var start = DateTime.Now;
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
            var elapsed = DateTime.Now - start;
            log.LogTrace($"Finished renewing Listing cache.\nTime elapsed: {elapsed}");
        }

        /// <summary>
        /// Updates the state of Users in the database that is stored in private field of this object.
        /// </summary>
        /// <param name="cache">Reference to the private field.</param>
        private void renewUserCache(ref Dictionary<int, User> cache)
        {
            var start = DateTime.Now;
            cache = (from user in db.Users
                    join listing in db.Listings on user.Id equals listing.UserId
                    select user).ToDictionary((User user) => user.Id);
            var elapsed = DateTime.Now - start;
            log.LogTrace($"Finished renewing Listing cache.\nTime elapsed: {elapsed}");
        }

        /// <summary>
        /// Retrieves all listings from the cache.
        /// </summary>
        /// <param name="token">Users session token. Used to calculate distance.
        /// If null is passed, the distance will be a default value.</param>
        /// <returns></returns>
        public List<Listing> RetrieveListings(long? token)
        {
            if(token is object && ServerController.GetSessionData((long) token, out Tuple<int, string> sessionData)) {
                listingCache.ForEach(listing => listing.SetDistance(sessionData.Item2));
            } else
            {
                listingCache.ForEach(listing => listing.Distance = -1);
            }
            return listingCache;
        }

        /// <summary>
        /// Task that renews the cache every minute.
        /// </summary>
        /// <returns>Task</returns>
        private async Task Run()
        {
            do
            {
                await Task.Run(() =>
                {
                    renewListingCache(ref listingCache);
                    renewUserCache(ref userCache);
                });
                await Task.Delay(60000);
            } while (true);
        }
    }
}
