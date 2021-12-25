using Newtonsoft.Json;

namespace PittJohnstownAPI.Models.Dining
{

    public class Menu
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("date")]
        public string? Date { get; set; }

        [JsonProperty("name")]
        public object? Name { get; set; }

        [JsonProperty("from_date")]
        public object? FromDate { get; set; }

        [JsonProperty("to_date")]
        public object? ToDate { get; set; }

        [JsonProperty("periods")]
        public Periods? Periods { get; set; }
    }

}