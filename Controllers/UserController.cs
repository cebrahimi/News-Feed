using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using News.DAL;
using News.Models;

namespace News.Controllers
{
    public class UserController : Controller
    {
        [HttpPost]
        public IActionResult AddFavourite(int userId, int articleId)
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;
            return Ok(context.AddFavourite(userId, articleId));
        }
        [HttpPost]
        public IActionResult RemoveFavourite(int userId, int articleId)
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;
            return Ok(context.RemoveFavourite(userId, articleId));
        }
        [HttpPost]
        public IActionResult GetFavourites(int userId)
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;
            return Ok(context.GetFavourites(userId));
        }
        [HttpPost]
        public IActionResult GetUserId(string email)
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;
            return Ok(context.GetUserId(email));
        }
        [HttpPost]
        public IActionResult GetUser(int userId)
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;
            return Ok(context.GetUser(userId));
        }
    }
}