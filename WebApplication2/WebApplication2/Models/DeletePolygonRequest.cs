using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class DeletePolygonRequest
    {
        public AuthorizationRequest Authorization { get; set; }
        public List<int?> PolygonIDs { get; set; }
    }
}