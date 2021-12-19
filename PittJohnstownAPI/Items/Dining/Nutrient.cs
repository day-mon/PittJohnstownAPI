using Newtonsoft.Json;

namespace PittJohnstownAPI.Items.Dining
{

    public class Nutrient
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("uom")]
        public string Uom { get; set; }

        [JsonProperty("value_numeric")]
        public string ValueNumeric { get; set; }
    }

}