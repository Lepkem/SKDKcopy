using Microsoft.AspNetCore.Mvc;
using Stexchange.Data;
using Stexchange.Data.Models;
using Stexchange.Models;
using System.Collections.Generic;
using System.Linq;



namespace Stexchange.Controllers
{
    public class DetailAdvertisementController : Controller
    {
        public DetailAdvertisementController(Database db)
        {
            Database = db;
        }
        private Database Database;
    
        // id in parameter. GET ROUTE VALUE to get id of advertisement
        public IActionResult DetailAdvertisement()
        {
            Listing advertisement = (from ad in Database.Listings
                                 where ad.Id == 6
                                 select ad).FirstOrDefault();

            List<FilterListing> advertisementFilters = (from filters in Database.FilterListings
                                        where filters.ListingId == advertisement.Id
                                        select filters).ToList();

            List<ImageData> advertisementImages = (from images in Database.Images
                                       where images.ListingId == advertisement.Id
                                       select images).ToList();

            var advertisementList = new DetailAdvertisementModel()
            {
                Id = advertisement.Id,
                Title = advertisement.Title,
                Description = advertisement.Description,
                Name_NL = advertisement.NameNl,
                Name_LT = advertisement.NameLatin,
                Quantity = advertisement.Quantity,
                User_id = advertisement.UserId,
                Created_at = advertisement.CreatedAt,
                Visible = advertisement.Visible,
                Renewed = advertisement.Renewed,
                Last_modified = advertisement.LastModified,
                Filterlist = advertisementFilters,
                Imagelist = advertisementImages
            };
            return View(advertisementList);
        }
    }
}
