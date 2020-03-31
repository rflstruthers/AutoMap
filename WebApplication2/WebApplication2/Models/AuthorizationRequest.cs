using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class AuthorizationRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string RequestingAppName { get; set; }
        public string RequestingAppVersion { get; set; }
    }
}