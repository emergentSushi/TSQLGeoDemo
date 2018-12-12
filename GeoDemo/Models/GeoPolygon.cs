﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoDemo.Models
{
    public class GeoPolygon
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<GeoPoint> Points { get; set; } = new List<GeoPoint>();
    }
}
