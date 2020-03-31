using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class ReadCompaniesResponse
    {
        public string Status { get; set; }
        public List<Company> Companies { get; set; }
    }
}