using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace News.Models
{
    public class User
    {
        [Display(Name = "id")]
        public int UserId { get; set; }
        [Display(Name = "email")]
        public string Email { get; set; }
        [Display(Name = "username")]
        public string Username { get; set; }
        //userid will have a foreign key reference to favorites table and history table so we can display this information on their profile
    }
}
