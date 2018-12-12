using GeoDemo.Models;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Xml.Linq;

namespace GeoDemo.KMLLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            XNamespace ns = "http://www.opengis.net/kml/2.2";
            var doc = XDocument.Load("electoral.kml");

            var placemarks = doc.Root
                           .Element(ns + "Document")
                           .Elements(ns + "Placemark");

            var elecs = new List<ElectoralZone>();
            foreach (var p in placemarks)
            {
                var zone = new ElectoralZone
                {
                    Name = p.Element(ns + "name").Value
                };

                foreach (var elec in p.Descendants(ns + "Polygon"))
                {
                    zone.Polygons.Add(new GeoPolygon
                    {
                        Points = elec.Descendants(ns + "coordinates").First().Value.Split(' ')
                            .Where(pt => !string.IsNullOrEmpty(pt))
                            .Select(pt => new GeoPoint
                            {
                                Latitude = double.Parse(pt.Split(',')[1]),
                                Longitude = double.Parse(pt.Split(',')[0])
                            }).ToList(),
                        Name = zone.Name
                    });


                }
                elecs.Add(zone);
            }

            using (var conn = new SqlConnection("Server=(local)\\SQLEXPRESS;Database=geo;User Id=sa;Password=test;"))
            {
                conn.Open();
                foreach (var z in elecs)
                {
                    var command = new SqlCommand("INSERT INTO Zones (name, geog) VALUES (@n, @g)")
                    {
                        Connection = conn
                    };

                    var geoParam = new SqlParameter("@g", GetGeographyFromText(z.ToWellFormedString()))
                    {
                        Direction = ParameterDirection.Input,
                        SourceColumn = "geog",
                        SqlDbType = SqlDbType.Udt,
                        UdtTypeName = "Geography"
                    };

                    var nameParam = new SqlParameter("@n", z.Name)
                    {
                        Direction = ParameterDirection.Input,
                        SourceColumn = "name",
                        SqlDbType = SqlDbType.NVarChar
                    };

                    command.Parameters.Add(nameParam);
                    command.Parameters.Add(geoParam);

                    command.ExecuteNonQuery();
                }

                conn.Close();
            }


            Console.WriteLine("Done");
            Console.ReadLine();
        }

        public static SqlGeography GetGeographyFromText(string pText)
        {
            try
            {
                return SqlGeography.STMPolyFromText(new SqlChars(new SqlString(pText)), 4326);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
