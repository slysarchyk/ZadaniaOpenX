using GeoCoordinatePortable;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZadaniaOpenX.Data.Models;

namespace ZadaniaOpenX
{
    class Zadania
    {
        //1 pobieram dane z plików "JSON" 2 method
        public List<User> DeserealizeUser()
        {
            string userJson = File.ReadAllText(@"C:\Users\Admin\source\repos\ZadaniaOpenX\ZadaniaOpenX\Data\JSON\Users.json");
            List<User> restoredUser = JsonConvert.DeserializeObject<List<User>>(userJson);

            return restoredUser;
        }

        public List<Post> DeserealizePost()
        {
            string postJson = File.ReadAllText(@"C:\Users\Admin\source\repos\ZadaniaOpenX\ZadaniaOpenX\Data\JSON\Posts.json");
            List<Post> restoredPost = JsonConvert.DeserializeObject<List<Post>>(postJson);

            return restoredPost;
        }

        //2 licze posty, oraz zwracam usera i odpowiednią ilość postów (2 - methods)
        public void CountPost()
        {
            var restoredPost = DeserealizePost();
            Console.WriteLine($"{restoredPost.Count()} - posts have been written");
            Console.WriteLine();
        }

        public void UserWritePost()
        {
            var restoredUser = DeserealizeUser();
            var restoredPost = DeserealizePost();

            foreach (var item in restoredUser)
                Console.WriteLine($"{item.UserName} - write - {restoredPost.Count(x => x.UserId == item.Id)} posts");
            Console.ReadLine();
        }

        //3 sprawdzam czy tytuły postów są unikalne i zwracam listę tytułów które nie są
        public void CheckRepeatTitlePost()
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

        //4 dla każdego użytkownika szukam innego użytkownika, który mieszka najbliżej niego
        public void ClosestUser()
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
