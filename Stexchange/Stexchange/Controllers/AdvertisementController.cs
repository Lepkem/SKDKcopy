using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stexchange.Controllers
{
    public class AdvertisementController : Controller
    {
        public IActionResult PostAdvertisement()
        {
            return View();
        }
    }
}
