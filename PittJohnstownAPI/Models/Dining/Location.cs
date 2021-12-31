using Newtonsoft.Json; 
namespace PittJohnstownAPI.Models.Dining{ 

    public class Location
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("open")]
        public bool Open { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("occupancy")]
        public string Occupancy { get; set; }

        [JsonProperty("address")]
        public Address Address { get; set; }
    }

}