using Stexchange.Controllers;
using Stexchange.Data;
using Stexchange.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stexchange.Models
{
    public class ChatViewModel
    {
        private Database db;

        public ChatViewModel(Database db)
        {
            this.db = db;
        }

        /// <summary>
        /// Returns the chats from the given user, ordered by most recent activity.
        /// </summary>
        /// <param name="token">The session token of the user</param>
        /// <returns></returns>
        public List<Chat> GetChats(long token)
        {
            if(!ServerController.GetSessionData(token, out Tuple<int, string> session)){
                throw new Exception("Session expired.");
            }
            List<Chat> chats = (from chat in db.Chats
                            where (chat.ResponderId == session.Item1 ||
                                (from listing in db.Listings
                                    where listing.UserId == session.Item1
                                    select listing.Id).Contains(chat.AdId))
                            select new EntityBuilder<Chat>(chat)
                            .SetProperty("Messages", (
                                from message in db.Messages
                                where message.ChatId == chat.Id
                                orderby message.Timestamp descending
                                select message
                            ).ToList())
                            .Complete()).ToList();
            chats = (from chat in chats
                     orderby chat.Messages[0].Timestamp descending
                     select chat).ToList();
            return chats;
        }
    }
}
