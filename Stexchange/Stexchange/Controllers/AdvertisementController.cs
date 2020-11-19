using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Stexchange.Data;

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
        public IActionResult PostAdvertisement()
        {
            return View();
        }
    }
}
