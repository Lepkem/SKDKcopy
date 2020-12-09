using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Stexchange.Controllers
{
    public class ServerController : Controller
    {
        public static class Cookies
        {
            public const string SessionData = "SessionData";
        }

        /// <summary>
        /// Dictionary that contains all currently active session tokens.
        /// </summary>
        private static readonly Dictionary<long, Tuple<int, string>> sessions = new Dictionary<long, Tuple<int, string>>();

        /// <summary>
        /// Checks if the session exists (and is active).
        /// </summary>
        /// <param name="token">Session token</param>
        /// <returns>True iff the session token exists in the session dictionary</returns>
        public static bool SessionExists(long token)
        {
            return sessions.Keys.Contains(token);
        }

        /// <summary>
        /// Retrieves the data associated to the session token if it exists
        /// </summary>
        /// <param name="token">session token</param>
        /// <param name="data">variable to store retrieved data in</param>
        /// <returns>True iff a key, value pair exists for the session token.</returns>
        public static bool GetSessionData(long token, out Tuple<int, string> data)
        {
            return sessions.TryGetValue(token, out data);
        }

        /// <summary>
        /// Retrieves the id of the user that is logged in from the Request cookie.
        /// </summary>
        /// <exception cref="Exception">If the user is not logged in.</exception>
        /// <returns>The id of the user.</returns>
        public int GetUserId()
        {
            Tuple<int, string> session;
            try
            {
                Request.Cookies.TryGetValue(Cookies.SessionData, out string cookieVal);
                long token = Convert.ToInt64(cookieVal ?? throw new ArgumentNullException("Cookie does not exist"));
                if(!GetSessionData(token, out session))
                {
                    throw new Exception("Session does not exist");
                }
            } catch (ArgumentNullException ane)
            {
                throw new Exception(ane.Message, ane);
            }
            return session.Item1;
        }
        
        /// <summary>
        /// Creates a key, value pair for the session in the session dictionary.
        /// </summary>
        /// <param name="user">The value of the session (user data)</param>
        public static long CreateSession(Tuple<int, string> user)
        {
            long token = generateToken(user);
            sessions.Add(token, user);
            return token;
        }

        /// <summary>
        /// Removes the key, value pair associated with the token.
        /// </summary>
        /// <param name="token">session token</param>
        /// <returns>True iff the key was found and removed.</returns>
        public static bool TerminateSession(long token)
        {
            return sessions.Remove(token);
        }

        /// <summary>
        /// Generates a 64 bit token for a user.
        /// </summary>
        /// <param name="data">user data</param>
        /// <returns>64 bit unique token</returns>
        private static long generateToken(Tuple<int, string> data)
        {
            return data.GetHashCode() * DateTime.Now.Ticks;
        }
    }
}
