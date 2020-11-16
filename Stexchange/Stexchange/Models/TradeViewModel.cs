using Stexchange.Data;
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
        public List<Listing> ListingCache;

        public TradeViewModel(Database db)
        {
            this.db = db;
        }

        public void renewListingCache(ref List<Object> cache)
        {
            throw new NotImplementedException();
            /*
            * Query:
            * select A.*, I.image, F.with_pot, F.water, F.light, F.ph, F.neutrients, F.indigenous, F.type, F.give_away
            * from Advertisement as A
            * join Image as I on A.id=I.ad_id
            * join Filter as F on A.id=F.id
            * where A.invisible=false
            * order by A.created_at desc;
            */
        }

        public List<Object> RetrieveListings(long token)
        {
            throw new NotImplementedException();
        }
    }
}
