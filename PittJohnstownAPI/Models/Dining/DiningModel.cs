using Newtonsoft.Json;

namespace PittJohnstownAPI.Models.Dining
{
    public class DiningModel
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("request_time")]
        public double RequestTime { get; set; }

        [JsonProperty("records")]
        public int Records { get; set; }

        [JsonProperty("allergen_filter")]
        public bool AllergenFilter { get; set; }

        [JsonProperty("menu")]
        public Menu Menu { get; set; }

        [JsonProperty("periods")]
        public List<Periods> Periods { get; } = new();

        [JsonProperty("closed")]
        public bool Closed { get; set; }
    }

}