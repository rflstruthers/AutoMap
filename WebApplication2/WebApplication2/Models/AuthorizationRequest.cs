using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class AuthorizationRequest
    {
        [Key]
        public string Login { get; set; }
        public string Password { get; set; }
        public string RequestingAppName { get; set; }
        public string RequestingAppVersion { get; set; }

    }
}