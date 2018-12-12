using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoDemo.Models
{
    public class ElectoralZone
    {
        public string Name { get; set; }
        public List<GeoPolygon> Polygons { get; set; } = new List<GeoPolygon>();

        public string ToWellFormedString()
        {
            return "MULTIPOLYGON(" + string.Join(",", Polygons.Select(p => "((" + string.Join(",", p.Points.Select(pt => "" + pt.Longitude + " " + pt.Latitude)) +"))")) + ")";
        }
    }
}
