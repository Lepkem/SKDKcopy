using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stexchange.Data;
using Stexchange.Models;

namespace Stexchange.Controllers
{
    public class ChatController : Controller
    {
        private Database _db;

        public ChatController(Database db)
        {
            _db = db;
        }

        public IActionResult Chat()
        {
            return View(model: new ChatViewModel(_db));
        }
    }
}
