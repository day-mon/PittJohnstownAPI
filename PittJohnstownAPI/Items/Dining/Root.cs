using Newtonsoft.Json; 
using System.Collections.Generic; 
namespace PittJohnstownAPI.Items.Dining
{ 
    public class Root
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
        public List<Periods> Periods { get; } = new List<Periods>();

        [JsonProperty("closed")]
        public bool Closed { get; set; }
    }

}