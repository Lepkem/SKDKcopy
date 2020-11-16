using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stexchange.Models
{
    public class TradeViewModel
    {
        private static List<Object> listingCache;

        private static void renewListingCache(ref List<Object> cache)
        {
            throw new NotImplementedException();
            /*
            var queryresult = 
            cache = queryresult
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
