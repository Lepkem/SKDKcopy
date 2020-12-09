using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stexchange.Data;
using Stexchange.Data.Models;
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

        /// <summary>
        /// Route to retrieve the chat inbox of the user.
        /// User must be logged in.
        /// If the pre-condition is not met,
        /// the client will be redirected to the Home view.
        /// </summary>
        /// <returns>The Chat view for the user.</returns>
        public IActionResult Chat()
        {
            long token;
            try
            {
                Request.Cookies.TryGetValue("SessionToken", out string cookieVal);
                token = Convert.ToInt64(cookieVal ?? throw new ArgumentNullException("Session does not exist."));
            } catch (ArgumentNullException)
            {
                return View("Home");
            }
            if (!ServerController.GetSessionData(token, out Tuple<int, string> session))
            {
                return View("Home");
            }
            List<Chat> chats = (from chat in _db.Chats
                                where (chat.ResponderId == session.Item1 ||
                                    (from listing in _db.Listings
                                     where listing.UserId == session.Item1
                                     select listing.Id).Contains(chat.AdId))
                                select new EntityBuilder<Chat>(chat)
                                .SetProperty("Messages", (
                                    from message in _db.Messages
                                    where message.ChatId == chat.Id
                                    orderby message.Timestamp descending
                                    select message
                                ).ToList())
                                .Complete()).ToList();
            chats = (from chat in chats
                     orderby chat.Messages[0].Timestamp descending
                     select chat).ToList();
            return View(model: new ChatViewModel(chats));
        }
    }
}
