using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using News.DAL;
using News.Models;

namespace News.Controllers
{
    public class PasswordResetController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private string GetUsername(string email)
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;
            var userId = context.GetUserId(email);
            return context.GetUser(userId).Username;
        }

        private string GetResetKey(string email)
        {
            var uuid = Guid.NewGuid();
            ResetContext resetContext = HttpContext.RequestServices.GetService(typeof(ResetContext)) as ResetContext;
            UserContext userContext = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;
            var userId = userContext.GetUserId(email);

            if (userId == -1)
            {
                return "";
            }
            resetContext.SetKey(userId, uuid.ToString());

            return uuid.ToString();
        }
        
        [HttpGet]
        public IActionResult CheckLink(string key)
        {
            ResetContext resetContext = HttpContext.RequestServices.GetService(typeof(ResetContext)) as ResetContext;
            int userId = resetContext.CheckKey(key);

            if (userId == -1)
            {
                return Content("Key does not match or is expired");
            }
            else
            {
                resetContext.removeKey(userId);
                ViewData["userId"] = userId;
                return View();
            }

        }
        [HttpPost]
        public bool SendEmail(string email)
        {
            var subject = "Forgot Password";
            var username = GetUsername(email);
            string body = "<p> Hello, " + username + "</p>";
            body += "<p>We've received your request for a password reset with your Outright News account.</p></br>";

            var resetKey = GetResetKey(email);
            if (resetKey == "")
            {
                return false;
            }
            
            string url = "https://localhost:44361/PasswordReset/CheckLink?key=" + resetKey;
            body += "<a href='" + url + "'>Click this link to reset your password</a></br>";
            body += "<p>Thanks, </p></br><p>The Outright News team</p>";
            
            var fromAddress = new MailAddress("outrightnews.csuf@gmail.com", "Outright News");
            var toAddress = new MailAddress(email, $"To {username}");
            const string fromPassword = "Outright123";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
            
            return true;
        }

    }
}
