using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class UpdatePolygonRequest
    {
        public AuthorizationRequest Authorization { get; set; }
        public List<Polygon> Polygons { get; set; }
    }
}