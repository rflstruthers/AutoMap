﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class UpdatePolygonResponse
    {
        public string Status { get; set; }
        public List<IDNames> PolygonIDNames { get; set; }
    }
}