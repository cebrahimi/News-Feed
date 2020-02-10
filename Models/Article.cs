using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
//using System.Web.Mvc;

namespace News.Models
{
    public class Article
    {
        [Display(Name = "id")]
        public int ID { get; set; }

        [Display(Name = "title")]
        public string Title { get; set; }

        [Display(Name = "webUrl")]
        public string WebsiteUrl { get; set; }

        [Display(Name = "publisherUrl")]
        public string PublisherUrl { get; set; }

        [Display(Name = "date")]
        public string Date { get; set; }

        [Display(Name = "websiteName")]
        public string WebsiteName { get; set; }

        [Display(Name = "genre")]
        public List<String> Genres { get; set; }

        [Display(Name = "description")]
        public string Description { get; set; }


    }
}