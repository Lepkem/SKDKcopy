using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Stexchange.Controllers
{
    public class TradeController : Controller
    {
        public IActionResult Trade()
        {
            return View();
        }

        public List<Object> Get()
        {
            throw new NotImplementedException();
        }

        public List<Object> Get(string token)
        {
            throw new NotImplementedException();
        }
    }
}
