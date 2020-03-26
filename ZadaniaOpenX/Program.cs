using GeoCoordinatePortable;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZadaniaOpenX.Data.Models;

namespace ZadaniaOpenX
{
    class Program
    {
        static void Main(string[] args)
        {
            //1 Зчитати з файлу і десереалізувати дані
            string userJson = File.ReadAllText(@"C:\Users\Admin\source\repos\ZadaniaOpenX\ZadaniaOpenX\Data\JSON\Users.json");
            string postJson = File.ReadAllText(@"C:\Users\Admin\source\repos\ZadaniaOpenX\ZadaniaOpenX\Data\JSON\Posts.json");

            List<User> restoredUser = JsonConvert.DeserializeObject<List<User>>(userJson);
            List<Post> restoredPost = JsonConvert.DeserializeObject<List<Post>>(postJson);

            //2 Вивести стільки постів написав кожен юзер
            foreach (var item in restoredUser)
                Console.WriteLine($"{item.UserName} - write - {restoredPost.Count(x => x.UserId == item.Id)} posts");
            Console.ReadLine();

            //3 Перевірити чи повторяються заголовки, і вивести які повторюються
            List<Post> reapetItem = restoredPost.GroupBy(x => x.Title).
                Where(x => x.Count() > 1).
                Select(x => x.First()).
                ToList();

            if (reapetItem.Count > 0)
            {
                foreach (var item in reapetItem)
                    Console.WriteLine($"Repeated Titles - {item.Title}");
            }
            else
                Console.WriteLine("No repeating Titles");
            Console.ReadLine();

            //4 Знвйти для кожного юзера іншого. який найближче живе
            foreach (var geoitem in restoredUser)
            {
                Console.WriteLine($"{geoitem.UserName}`s address is in coordinates: Lat - {geoitem.Address.Geo.Lat}, Lng - {geoitem.Address.Geo.Lng}");

                GeoCoordinate geoUser = new GeoCoordinate(geoitem.Address.Geo.Lat, geoitem.Address.Geo.Lng);

                //ця конструкція тільки для перевірки наочної в кінцевому видвлю
                foreach (var i in restoredUser)
                {
                    GeoCoordinate geoUser2 = new GeoCoordinate(i.Address.Geo.Lat, i.Address.Geo.Lng);
                    var distance = geoUser2.GetDistanceTo(geoUser);

                    List<double> res = new List<double>();
                    res.Add(distance/1000);

                    Console.WriteLine(distance);
                }
                //-------------------------------------------------------------------------

                var minDistance = restoredUser.Select(x => new { Coor = new GeoCoordinate(x.Address.Geo.Lat, x.Address.Geo.Lng), Username = x.UserName }).
                    Where(x => geoUser.GetDistanceTo(x.Coor) > 0).
                    Min(x => geoUser.GetDistanceTo(x.Coor) / 1000);
                
                Console.WriteLine($"User short distance {minDistance}");
                Console.WriteLine();
            }
            Console.ReadLine();
        }
    }
}
