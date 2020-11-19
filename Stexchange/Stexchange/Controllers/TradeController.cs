using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stexchange.Data.Models;

namespace Stexchange.Controllers
{
    public class TradeController : Controller
    {
        public IActionResult Trade()
        {
            return View();
        }
    }
}
