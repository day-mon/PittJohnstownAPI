using Newtonsoft.Json; 
namespace PittJohnstownAPI.Models.Dining{ 

    public class Status
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }
    }

}