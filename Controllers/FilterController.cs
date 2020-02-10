using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using News.DAL;
using News.Models;
using Org.BouncyCastle.Crypto.Digests;

namespace News.Controllers
{
    public class FilterController : Controller
    {
        public IActionResult GetFilteredArticles(int websiteId)
        {
            HttpContext.Session.SetInt32("websiteId", websiteId);

            ArticleContext context = HttpContext.RequestServices.GetService(typeof(ArticleContext)) as ArticleContext;
            var articles = context.GetAllArticles(websiteId);

            articles = articles.OrderByDescending(a => a.Date).ToList();

            ViewData["articles"] = articles;

            return PartialView("_Articles");
        }
        
        public IActionResult GetFilteredGenres(int websiteId)
        {
            HttpContext.Session.SetInt32("websiteId", websiteId);

            GenreContext genreContext = HttpContext.RequestServices.GetService(typeof(GenreContext)) as GenreContext;

            ViewData["genres"] = genreContext.ListGenres(websiteId);

            return PartialView("_Genres");
        }
        [HttpPost]
        public IActionResult GetArticles(List<int> articleIds)
        {
            ArticleContext context = HttpContext.RequestServices.GetService(typeof(ArticleContext)) as ArticleContext;

            var articles = context.GetArticles(articleIds);

            return Ok(articles);
        }
    }
}