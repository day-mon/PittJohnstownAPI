using Newtonsoft.Json;

namespace PittJohnstownAPI.Models.Dining
{

    public class Periods
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("sort_order")]
        public int SortOrder { get; set; }

        [JsonProperty("categories")]
        public List<Category> Categories { get; } = new();
    }

}