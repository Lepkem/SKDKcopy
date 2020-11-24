using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Stexchange.Data;
using Stexchange.Data.Models;
using Stexchange.Data.Builders;
using static System.String;
using System.Collections.Generic;

namespace Stexchange.Controllers
{
    public class AdvertisementController : Controller
    {
        public AdvertisementController(Database db, IConfiguration config)
        {
            Database _databse = db;
            IConfiguration _config = config;


        }

        private Database _database { get; }
        private IConfiguration _config { get; }

        

        [HttpPost]
        public async Task<IActionResult> PostAdvertisement(List<ImageData> img, string title, string description, 
            string name_nl, int quantity, string plant_type, string give_away, string with_pot, string name_lt="", 
            string light="", string water="", string ph="", string indigenous="")
        {
            //todo: def val planttype and withpot
            try
            {
                if (ModelState.IsValid)
                {
                    ListingBuilder lb = new ListingBuilder();
                    List<FilterListing> fl = new List<FilterListing>();
                    List<Filter> filters = new List<Filter>();
                   

                    //TODO: build a listing using the listing builder class 

                    //these are the required properties of the listing being built
                    if (img.First() != null && !IsNullOrEmpty(title) 
                                            && !IsNullOrEmpty(description) 
                                            && !IsNullOrEmpty(name_nl) 
                                            && !IsNullOrEmpty(plant_type)
                                            && quantity > 0
                                            && !IsNullOrEmpty(give_away))
                    {
                        lb.SetProperty("Pictures", img)
                            .SetProperty("Title", title)
                            .SetProperty("Description", description)
                            .SetProperty("NameNl", name_nl)
                            .SetProperty("Quantity", quantity);
                        filters.Add(new Filter(give_away));
                        filters.Add(new Filter(plant_type));
                        filters.Add(new Filter(with_pot));
                    }

                    //now the non-required properties
                    if (!IsNullOrEmpty(name_lt)) lb.SetProperty("NameLt", name_lt);
                    if (!IsNullOrEmpty(light)) filters.Add(new Filter(light));
                    if (!IsNullOrEmpty(water)) filters.Add(new Filter(water));
                    if (!IsNullOrEmpty(ph)) filters.Add(new Filter(ph));
                    if (!IsNullOrEmpty(indigenous)) filters.Add(new Filter(indigenous));

                    foreach (Filter fil in filters)
                    {
                        fl.Add(new FilterListing(/*here i should input the listing and filter vals*/));
                    }
                     
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = "Error: " + e.ToString();
            }
            //DONE: put in all name values empty or not
            //TODO: validate all name values from the form per value if they are empty or wrong
            //TODO: use fluentvalidator for this

            await Database.AddAsync();
            await Database.SaveChangesAsync();

            //TODO: iterate over the category list 
            //TODO: input the new tables into the database 
            // input the advert
            // input the filter
            // input the image(s)



            return View(); // todo: insert something here
        }
    }
}
