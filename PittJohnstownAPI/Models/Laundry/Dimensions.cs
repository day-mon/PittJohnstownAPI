using Newtonsoft.Json;
namespace PittJohnstownAPI.Items.Laundry
{
    public class Dimensions
    {
        [JsonProperty("x")]
        public int X { get; set; }

        [JsonProperty("y")]
        public int Y { get; set; }
    }

}