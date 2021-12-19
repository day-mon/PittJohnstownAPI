using Newtonsoft.Json;

namespace PittJohnstownAPI.Items.Dining
{

    public class Filter
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("icon")]
        public bool Icon { get; set; }

        [JsonProperty("remote_file_name")]
        public object RemoteFileName { get; set; }

        [JsonProperty("custom_icons")]
        public List<object> CustomIcons { get; } = new List<object>();
    }

}