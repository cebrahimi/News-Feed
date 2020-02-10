using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
//using System.Web.Mvc;

namespace News.Models
{
    public class Genre
    {
        [Display(Name = "genreid")]
        public int genreID { get; set; }

        [Display(Name = "genre")]
        public string genre { get; set; }
    }
}
