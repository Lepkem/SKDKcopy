using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stexchange.Data;
using Stexchange.Models;

namespace Stexchange.Controllers
{
    public class DetailAdvertisementController : Controller
    {
        public DetailAdvertisementController(Database db, ILogger<TradeViewModel> logger)
        {
            Database = db;
            Logger = logger;
        }
        private Database Database;
        private ILogger<TradeViewModel> Logger;

        // id in parameter. GET ROUTE VALUE to get id of advertisement
        public IActionResult DetailAdvertisement()
        {
            // test id
            TempData["id"] = 3;
            TempData.Keep("id");
            return View(model: new TradeViewModel(Database, Logger));
        }
    }
}
