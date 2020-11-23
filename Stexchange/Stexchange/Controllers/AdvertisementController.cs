using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Stexchange.Data;
using Stexchange.Data.Models;

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
        public IActionResult PostAdvertisement(ImageData img, string title, string description, 
            string name_nl, int quantity, string plant_type, string name_lt="", string light="", 
            string water="", string ph="", string indigenous="", string with_pot="", string give_away="")
        {
            //TODO: put in all name values empty or not
            //TODO: validate all name values from the form per value if they are empty or wrong
            //TODO: build a listing using the listing builder class 
            //TODO: use the category list 


            return View(); // todo: insert something here
        }
    }
}
