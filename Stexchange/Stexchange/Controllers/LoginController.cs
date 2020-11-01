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
using System.Threading.Tasks;

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

        [HttpGet("[controller]/[action]/{id}")]
        public async Task<IActionResult> Verification(string id)
        {
			Guid guid;
			if (!Guid.TryParse(id, out guid))
			{
				// Send BadRequest if the id was malformed
				return BadRequest();
			}

            var verification = (from code in Database.UserVerifications
                                where code.Guid == guid
                                select code).FirstOrDefault();

            //check if verificationlink has already been activated
            if (verification.IsVerified == true)
            {
                TempData["Message"] = "E-mailverificatie Voltooid";
                return View("Verify");
            }

            else
            {
                if (!(verification is null))
                {
                    verification.IsVerified = true;
                    await Database.SaveChangesAsync();

                    TempData["Message"] = "E-mailverificatie Voltooid";
                }
                return View("Verify");
            }
        }

        [HttpGet("[controller]/[action]/{vEmail}")]
        public async Task<IActionResult> SendNewCode(string vEmail)
        {
            var user = (from u in Database.Users
                        where !u.Verification.IsVerified
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
        public async Task<IActionResult> Register(string email)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!new EmailAddressAttribute().IsValid(email))
                    {
                        TempData["Message"] = "E-mail is ongeldig";
                        return View("Login");
                    }
                    
                    //Generates verification code
                    Random rnd = new Random();

                    // Create a new UserVerification object with a new unique Guid and verification code
                    var verification = new UserVerification()
                    {
                        Guid = Guid.NewGuid()
                    };

                    var new_User = new User()
                    {
                        Email = email,
                        Verification = verification
                    };

                    if (Database.Users.Any(u => u.Email == email))
                    {
                        // found users with the given email
                        TempData["Message"] = "E-mail is al gebruikt";
                        return View("Login");
                    }
                 
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

        private void SendEmail(string address, string body)
        {
			EmailService.QueueMessage(address, body);
		}

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}