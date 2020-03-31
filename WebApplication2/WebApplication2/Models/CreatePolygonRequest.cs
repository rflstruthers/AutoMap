using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class CreatePolygonRequest
    {
        public AuthorizationRequest Authorization { get; set; }
        public int CompanyID { get; set; }
        public List<Polygon> Polygons { get; set; }
    }
}