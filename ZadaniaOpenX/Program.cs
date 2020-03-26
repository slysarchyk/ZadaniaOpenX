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
            CountPost();
            UserWritePost();
            CheckRepeatTitlePost();
            ClosestUser();
        }

        public static List<User> DeserealizeUser()
        {
            string userJson = File.ReadAllText(@"C:\Users\Admin\source\repos\ZadaniaOpenX\ZadaniaOpenX\Data\JSON\Users.json");
            List<User> restoredUser = JsonConvert.DeserializeObject<List<User>>(userJson);

            return restoredUser;
        }

        public static List<Post> DeserealizePost()
        {
            string postJson = File.ReadAllText(@"C:\Users\Admin\source\repos\ZadaniaOpenX\ZadaniaOpenX\Data\JSON\Posts.json");
            List<Post> restoredPost = JsonConvert.DeserializeObject<List<Post>>(postJson);

            return restoredPost;
        }

        public static void CountPost()
        {
            var restoredPost = DeserealizePost();
            Console.WriteLine($"{restoredPost.Count()} - posts have been written");
        }

        public static void UserWritePost()
        {
            var restoredUser = DeserealizeUser();
            var restoredPost = DeserealizePost();

            foreach (var item in restoredUser)
                Console.WriteLine($"{item.UserName} - write - {restoredPost.Count(x => x.UserId == item.Id)} posts");
            Console.ReadLine();
        }

        public static void CheckRepeatTitlePost()
        {
            var restoredPost = DeserealizePost();

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
        }

        static void ClosestUser()
        {
            var restoredUser = DeserealizeUser();

            foreach (var geoitem in restoredUser)
            {
                Console.WriteLine($"{geoitem.UserName}`s address is in coordinates: Lat - {geoitem.Address.Geo.Lat}, Lng - {geoitem.Address.Geo.Lng}");
                GeoCoordinate geoUser = new GeoCoordinate(geoitem.Address.Geo.Lat, geoitem.Address.Geo.Lng);

                var minDistance = restoredUser.Select(x => new { Coor = new GeoCoordinate(x.Address.Geo.Lat, x.Address.Geo.Lng), Username = x.UserName }).
                    Where(x => geoUser.GetDistanceTo(x.Coor) > 0).
                    Min(x => geoUser.GetDistanceTo(x.Coor) / 1000);

                Console.WriteLine($"User short distance {minDistance}");
            }
            Console.ReadLine();
        }
    }
}
