using Newtonsoft.Json;
using PittJohnstownAPI.Items.Laundry;

namespace PittJohnstownAPI.Models.Laundry
{
    public class Root
    {
        [JsonProperty("dimensions")]
        public Dimensions Dimensions { get; set; }

        [JsonProperty("roomOrientation")]
        public string RoomOrientation { get; set; }

        [JsonProperty("machineScale")]
        public int MachineScale { get; set; }

        [JsonProperty("upsideDownStacks")]
        public int UpsideDownStacks { get; set; }

        [JsonProperty("objects")]
        public List<LaundryObject> LaundryObjects { get; set; }

        [JsonProperty("schoolStyles")]
        public SchoolStyles SchoolStyles { get; set; }
    }

}