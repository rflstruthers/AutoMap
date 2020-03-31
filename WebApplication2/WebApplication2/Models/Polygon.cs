using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Polygon
    {
        public int PolygonID { get; set; }
        public string PolygonName { get; set; }
        public List<Point> Points { get; set; }      // first point and last point are connected
    }
}