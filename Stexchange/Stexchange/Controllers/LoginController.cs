using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Stexchange.Data;
using Stexchange.Data.Models;
using Stexchange.Models;
using Stexchange.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Net;
using System.Collections.Generic;

namespace Stexchange.Controllers
{
    public class LoginController : Controller
    {
        public LoginController(Database db, IConfiguration config, EmailService emailService)
        {
            Database = db;
            EmailService = emailService;
            Config = config.GetSection("MailSettings");
        }

        private Database Database { get; }
        private EmailService EmailService { get; }
        private IConfiguration Config { get; }

        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Compares verificationlink to user's verificationcode.
        /// </summary>
        /// <param name="id">Verificationcode of user</param>
        /// <returns></returns>
        [HttpGet("[controller]/[action]/{id}")]
        public async Task<IActionResult> Verification(string id)
        {
			Guid guid;
			if (!Guid.TryParse(id, out guid))
			{
				// Send BadRequest if the id was malformed
				return BadRequest();
			}

            var guids = (from code in Database.UserVerifications
                         select code.Guid).ToArray();

            // Checks if guid exists
            if (!guids.Contains(guid))
            {
                return View("InvalidVerificationLink");
            }

            var verification = (from code in Database.UserVerifications
                                where code.Guid == guid
                                select code).FirstOrDefault();

            var user = (from u in Database.Users
                        where u.Verification.Guid == guid
                        select u).FirstOrDefault();

            // Checks if verificationlink has already been activated
            if (user.IsVerified == true)
            {
                return View("InvalidVerificationLink");
            }
            else
            {
                if (!(verification is null))
                {
                    user.IsVerified = true;
                    await Database.SaveChangesAsync();
                }
                return View("Verified");
            }
        }

        /// <summary>
        /// Send new verificationlink to user if he isn't verified.
        /// </summary>
        /// <param name="vEmail">Emailaddress of user</param>
        [HttpGet("[controller]/[action]/{vEmail}")]
        public async Task<IActionResult> SendNewCode(string vEmail)
        {
            var user = (from u in Database.Users
                        where !u.IsVerified
                            && u.Email == vEmail
                        select u).FirstOrDefault();
            
            if (!(user is null))
            {
                // Let entity framework find the UserVerification object for this user
                await Database.Entry(user).Reference(u => u.Verification).LoadAsync();

                user.Verification.Guid = Guid.NewGuid();
                
                string body = $@"STEXCHANGE
Verifieer je e-mailadres door op de onderstaande link te klikken
https://{ControllerContext.HttpContext.Request.Host}/login/Verification/{user.Verification.Guid}";

                // Send the verification email
                SendEmail(vEmail, body);

                await Database.SaveChangesAsync();
                return NoContent();
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email, string vEmail, string password, string confirm_password, string username, string postalcode)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!new EmailAddressAttribute().IsValid(email))
                    {
                        TempData["Message"] = "InvalidEmail";
                        return View("Login");
                    }

                    if (email != vEmail)
                    {
                        TempData["Message"] = "IncorrectEmails";
                        return View("Login");
                    }
                    
                    // Checks if postal code is valid
                    if (!new Regex(@"\d{4}[A-Z]{2}", RegexOptions.IgnoreCase).IsMatch(postalcode))
                    {
                        TempData["Message"] = "InvalidPostalCode";
                        return View("Login");
                    }

                    if (password != confirm_password)
                    {
                        TempData["Message"] = "IncorrectPasswords";
                        return View("Login");
                    }

                    // Checks if email already exists in database
                    if (Database.Users.Any(u => u.Email == email))
                    {
                        TempData["Message"] = "EmailExists";
                        return View("Login");
                    }

                    // Checks if username already exists in database
                    if (Database.Users.Any(u => u.Username == username))
                    {
                        TempData["Message"] = "UsernameTaken";
                        return View("Login");
                    }

                    // Create a new UserVerification object with a new unique Guid and verification code
                    var verification = new UserVerification()
                    {
                        Guid = Guid.NewGuid()
                    };

                    var new_User = new User()
                    {
                        Email = email,
                        Username = username,
                        Postal_Code = postalcode.ToUpper(),
                        Password = CreatePasswordHash(password, username),
                        Created_At = DateTime.Now,
                        Verification = verification
                    };
                 
                    await Database.AddAsync(new_User);
                    await Database.SaveChangesAsync();

                    string body = $@"STEXCHANGE
Verifieer je e-mailadres door op de onderstaande link te klikken
https://{ControllerContext.HttpContext.Request.Host}/login/Verification/{verification.Guid}";

                    // Send the verification email
                    SendEmail(email, body);

                    //Pass data from controller to view
                    TempData["Message"] = $"we hebben een verificatielink verstuurd naar: {new_User.Email}";
                    TempData["Email"] = new_User.Email;
                    return View("Verify");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error: " + ex.ToString();
            }
            return View("Login");
        }

        /// <summary>
        /// Adds message to queue
        /// </summary>
        /// <param name="address">The mail address of the user</param>
        /// <param name="body">The mail message</param>
        private void SendEmail(string address, string body) => EmailService.QueueMessage(address, body);

        /// <summary>
        /// Given a password and salt, returns a salted SHA512 hash.
        /// </summary>
        /// <param name="password">The password</param>
        /// <param name="salt">The salt to use (username)</param>
        /// <returns>The new password hash</returns>
        private byte[] CreatePasswordHash(string password, string salt)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException($"'{nameof(password)}' cannot be null or empty");
            if (string.IsNullOrEmpty(salt))
                throw new ArgumentException($"'{nameof(salt)}' cannot be null or empty");        
            
            using var sha512Hash = SHA512.Create();
            return sha512Hash.ComputeHash(Encoding.UTF8.GetBytes($"{salt}#:#{password}"));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}