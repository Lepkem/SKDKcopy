using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Stexchange.Controllers
{
    public class ServerController : Controller
    {
        /// <summary>
        /// Dictionary that contains all currently active session tokens.
        /// </summary>
        private static readonly Dictionary<long, Tuple<int, char[]>> sessions = new Dictionary<long, Tuple<int, char[]>>();

        /// <summary>
        /// Checks if the session exists (and is active).
        /// </summary>
        /// <param name="token">Session token</param>
        /// <returns>True iff the session token exists in the session dictionary</returns>
        public bool SessionExists(long token)
        {
            return sessions.Keys.Contains(token);
        }

        /// <summary>
        /// Retrieves the data associated to the session token if it exists
        /// </summary>
        /// <param name="token">session token</param>
        /// <param name="data">variable to store retrieved data in</param>
        /// <returns>True iff a key, value pair exists for the session token.</returns>
        public bool GetSessionData(long token, out Tuple<int, char[]> data)
        {
            return sessions.TryGetValue(token, out data);
        }
        
        /// <summary>
        /// Creates a key, value pair for the session in the session dictionary.
        /// </summary>
        /// <param name="user">The value of the session (user data)</param>
        public long CreateSession(Tuple<int, char[]> user)
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
        public bool TerminateSession(long token)
        {
            return sessions.Remove(token);
        }

        /// <summary>
        /// Generates a 64 bit token for a user.
        /// </summary>
        /// <param name="data">user data</param>
        /// <returns>64 bit unique token</returns>
        private long generateToken(Tuple<int, char[]> data)
        {
            return data.GetHashCode() * DateTime.Now.Ticks;
        }
    }
}
