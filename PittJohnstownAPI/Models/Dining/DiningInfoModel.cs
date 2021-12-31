using Newtonsoft.Json; 
using System.Collections.Generic; 
namespace PittJohnstownAPI.Models.Dining{ 

    public class DiningInfoModel
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("request_time")]
        public double RequestTime { get; set; }

        [JsonProperty("records")]
        public int Records { get; set; }

        [JsonProperty("locations")]
        public List<Location> Locations { get; set; }
    }

}