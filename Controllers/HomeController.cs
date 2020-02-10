using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using News.Models;
using News.DAL;
namespace News.Controllers
{
    public class HomeController : Controller
    {
            
        public IActionResult Index()
        {
            var websiteid = 3;
            ArticleContext articleContext = HttpContext.RequestServices.GetService(typeof(ArticleContext)) as ArticleContext;

            if (!articleContext.CheckConnection())
            {
                return Content("Could not establish connection to the database.");
            }
            
            ViewData["articles"] = articleContext.GetAllArticles(websiteid).OrderByDescending(a => a.Date).ToList();

            GenreContext genreContext = HttpContext.RequestServices.GetService(typeof(GenreContext)) as GenreContext;
            ViewData["genres"] = genreContext.ListGenres(websiteid);

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //Check if not null
        //Check if sesion id exists in database
        //get the user id related to the session id
        

    }
}
