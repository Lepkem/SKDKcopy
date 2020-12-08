using Microsoft.AspNetCore.Mvc;
using Stexchange.Data;
using Stexchange.Data.Models;
using Stexchange.Models;
using System;
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

        public IActionResult DetailAdvertisement()
        {
            List<string> filteroptions = new List<string>{ "light_", "water_", "plant_type_", "nutrients_", "ph_", "indigenous_", "with_pot_", "give_away_"};
            
            int testid = 2;

            Listing advertisement = (from ad in Database.Listings
                                 where ad.Id == testid
                                 select ad).FirstOrDefault();

            User advertisementOwner = (from u in Database.Users
                                         where u.Id == advertisement.UserId
                                         select u).FirstOrDefault();

            List<string> advertisementFilters = (from filters in Database.FilterListings
                                        where filters.ListingId == advertisement.Id
                                        select filters.Value).ToList();

            List<byte[]> advertisementImages = (from images in Database.Images
                                       where images.ListingId == advertisement.Id
                                       select images.Image).ToList();

            var imageList = ByteArrayToImage(advertisementImages);

            Dictionary<string, string> Filters = new Dictionary<string, string>();

            // Loops through advertisementfilters and compares each filter to each filteroptions
            for (int i = 0; i < filteroptions.Count; i++)
            {
                for(int j = 0; j < filteroptions.Count; j++)
                {
                    if (filteroptions[j].Substring(0, 2) == advertisementFilters[i].Substring(0, 2))
                    {
                        // Replaces filteroption name and underscores in filter value with empty strings or white spaces
                        var filterValue = advertisementFilters[i].Replace(filteroptions[j], "").Replace("_", " ");
                        // Replaces all underscores in filteroption names with empty strings
                        var filterKey = filteroptions[j].Replace("_", "");
                        Filters.Add(filterKey, filterValue);
                    }
                }
            }

            var advertisementList = new DetailAdvertisementModel()
            {
                Id = advertisement.Id,
                Title = advertisement.Title,
                Description = advertisement.Description,
                Name_NL = advertisement.NameNl,
                Name_LT = advertisement.NameLatin,
                Quantity = advertisement.Quantity,
                User_id = advertisement.UserId,
                Username = advertisementOwner.Username,
                Created_at = advertisement.CreatedAt,
                Visible = advertisement.Visible,
                Renewed = advertisement.Renewed,
                Last_modified = advertisement.LastModified,
                Filterlist = Filters,
                Imagelist = imageList
            };
            return View(advertisementList);
        }
        public List<string> ByteArrayToImage(List<byte[]> images)
        {
            List<string> imageslist = new List<string>();
            foreach (byte[] image in images)
            {
                // Convert byte arry to base64string   
                string base64string = Convert.ToBase64String(image);
                string imgDataURL = string.Format("data:image/png;base64,{0}", base64string);

                imageslist.Add(imgDataURL);
            }
            return imageslist;
        }
    }
}
