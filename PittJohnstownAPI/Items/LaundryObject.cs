using Newtonsoft.Json; 
namespace PittJohnstownAPI.Items{ 

    public class LaundryObject
    {
        [JsonProperty("x")]
        public double X { get; set; }

        [JsonProperty("y")]
        public double Y { get; set; }

        [JsonProperty("wall_1_x")]
        public int Wall1X { get; set; }

        [JsonProperty("wall_1_y")]
        public int Wall1Y { get; set; }

        [JsonProperty("wall_2_x")]
        public int Wall2X { get; set; }

        [JsonProperty("wall_2_y")]
        public int Wall2Y { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("appliance_type")]
        public string ApplianceType { get; set; }

        [JsonProperty("model_number")]
        public string ModelNumber { get; set; }

        [JsonProperty("orientation")]
        public string Orientation { get; set; }

        [JsonProperty("appliance_desc_key")]
        public string ApplianceDescKey { get; set; }

        [JsonProperty("appliance_desc")]
        public string ApplianceDesc { get; set; }

        [JsonProperty("combo")]
        public bool? Combo { get; set; }

        [JsonProperty("stacked")]
        public bool? Stacked { get; set; }

        [JsonProperty("opacity")]
        public int? Opacity { get; set; }

        [JsonProperty("status_toggle")]
        public int? StatusToggle { get; set; }

        [JsonProperty("average_run_time")]
        public int? AverageRunTime { get; set; }

        [JsonProperty("time_remaining")]
        public int? TimeRemaining { get; set; }

        [JsonProperty("time_left_lite")]
        public string TimeLeftLite { get; set; }

        [JsonProperty("percentage")]
        public double? Percentage { get; set; }

        [JsonProperty("status_toggle2")]
        public int? StatusToggle2 { get; set; }

        [JsonProperty("average_run_time2")]
        public int? AverageRunTime2 { get; set; }

        [JsonProperty("time_remaining2")]
        public int? TimeRemaining2 { get; set; }

        [JsonProperty("time_left_lite2")]
        public string TimeLeftLite2 { get; set; }

        [JsonProperty("percentage2")]
        public double? Percentage2 { get; set; }

        [JsonProperty("appliance_desc_key2")]
        public string ApplianceDescKey2 { get; set; }

        [JsonProperty("appliance_desc2")]
        public string ApplianceDesc2 { get; set; }
    }

}