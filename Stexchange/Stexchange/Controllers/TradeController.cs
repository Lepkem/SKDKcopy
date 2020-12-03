using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stexchange.Data;
using Stexchange.Data.Models;
using Stexchange.Models;

namespace Stexchange.Controllers
{
    public class TradeController : Controller
    {
        private IDbContextFactory<Database> dbFactory;
        private ILogger<TradeViewModel> logger;
        public TradeController(IDbContextFactory<Database> factory, ILogger<TradeViewModel> logger)
        {
            dbFactory = factory;
            this.logger = logger;
        }
        public IActionResult Trade()
        {
            return View(model: new TradeViewModel(dbFactory.CreateDbContext()));
        }
    }
}
