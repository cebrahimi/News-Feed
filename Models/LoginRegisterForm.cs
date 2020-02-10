using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace News.Models
{
    public class LoginRegisterForm
    {
        [DisplayName("")]
        public string LoginUserEmail { get; set; }
        [DisplayName("")]
        public string LoginPassword { get; set; }
        [DisplayName("")]
        public string RegisterEmail { get; set; }
        [DisplayName("")]
        public string RegisterUsername { get; set; }
        [DisplayName("")]
        public string RegisterPassword { get; set; }
        [DisplayName("")]
        public string RegisterPasswordCheck { get; set; }
    }
}
