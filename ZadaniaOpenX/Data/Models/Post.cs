using System;
using System.Text.Json.Serialization;

namespace ZadaniaOpenX.Data.Models
{
    public class Post
    {
        [JsonPropertyName("useId")]
        public int UserId { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("body")]
        public string Body { get; set; }
    }
}
