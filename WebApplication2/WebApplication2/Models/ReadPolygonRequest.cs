using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class ReadPolygonRequest
    {
        public AuthorizationRequest Authorization { get; set; }
        public List<int> CompanyIDs { get; set; }
    }
}