using System;
using System.Text.Json.Serialization;

namespace WebAPI.Controllers
{
    [Serializable]
    public class Payload
    {
        [JsonConstructor]
        public Payload() { }
        public string name { get; set; }
        public DateTime date { get; set; }
        public int requests_sent { get; set; }
    }
}
