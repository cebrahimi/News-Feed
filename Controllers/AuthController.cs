using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using News.DAL;
using News.Models;

namespace News.Controllers
{
    public class AuthController : Controller
    {
        [HttpPost]
        public IActionResult HandleLogin(LoginRegisterForm model)
        {
            LoginContext context =
                HttpContext.RequestServices.GetService(typeof(LoginContext)) as LoginContext;
            
            int userId = context.CheckCredentials(model.LoginUserEmail, model.LoginPassword);

            if (userId == 0)
            {
                return Content("User not found or password is incorrect");
            }
            SetCookie(userId);
            return Content("");
        }

        [HttpPost]
        public IActionResult HandleRegister(LoginRegisterForm model)
        {
            
            LoginContext context = HttpContext.RequestServices.GetService(typeof(LoginContext)) as LoginContext;
        
            if (model.RegisterPassword != model.RegisterPasswordCheck)
            {
                return Content("Passwords don't match!");
            }
            
            if (context.CheckEmail(model.RegisterEmail))
            {
                return Content("Email already in use");
            }

            string hashed = Hashing(model.RegisterPassword);
            string result = context.AddUser(model.RegisterEmail, hashed, model.RegisterUsername,
                model.RegisterPasswordCheck);
            
            int userId = context.CheckCredentials(model.RegisterEmail, model.RegisterPassword);
            SetCookie(userId);

            return Content($"{result}");
        }

        [HttpPost]
        public IActionResult ChangePassword(string password, int userId)
        {
            LoginContext context = HttpContext.RequestServices.GetService(typeof(LoginContext)) as LoginContext;
            string hashed = Hashing(password);
            
            return Ok(context.changePassword(hashed, userId));
        }

        private string Hashing(string password)
        {
            string hashed;
            MD5 md5Hash = MD5.Create();
            hashed = GetMd5Hash(md5Hash, password);
            return hashed;
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        private void SetCookie(int userId)
        {
            var sessionId = Guid.NewGuid().ToString();
            CookiesContext context = HttpContext.RequestServices.GetService(typeof(CookiesContext)) as CookiesContext;

            CookieOptions cookieOptions = new CookieOptions
            {
                Expires = new DateTimeOffset(DateTime.Now.AddDays(1))
            };
            HttpContext.Response.Cookies.Append("date_of_creation", DateTime.Now.ToString(), cookieOptions); //set cookie
            HttpContext.Response.Cookies.Append("session_id", sessionId, cookieOptions); //set cookie
            
            context.AddSession(userId, sessionId);
        }

    }
}