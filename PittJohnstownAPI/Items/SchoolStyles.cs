using Newtonsoft.Json; 
namespace PittJohnstownAPI.Items{ 

    public class SchoolStyles
    {
        [JsonProperty("school_theme")]
        public string SchoolTheme { get; set; }

        [JsonProperty("school_logo")]
        public string SchoolLogo { get; set; }

        [JsonProperty("room_wall")]
        public string RoomWall { get; set; }

        [JsonProperty("room_bg")]
        public string RoomBg { get; set; }

        [JsonProperty("header_status_legend_bg")]
        public string HeaderStatusLegendBg { get; set; }

        [JsonProperty("header_status_legend_color")]
        public string HeaderStatusLegendColor { get; set; }

        [JsonProperty("content_box_bg")]
        public string ContentBoxBg { get; set; }

        [JsonProperty("content_box_color")]
        public string ContentBoxColor { get; set; }
    }

}