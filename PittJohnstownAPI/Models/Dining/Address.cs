using Newtonsoft.Json; 
using System.Collections.Generic; 
namespace PittJohnstownAPI.Models.Dining{ 

    public class Address
    {
        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("metadata")]
        public object Metadata { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("zip_code")]
        public string ZipCode { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lon")]
        public double Lon { get; set; }

        [JsonProperty("phone")]
        public object Phone { get; set; }

        [JsonProperty("dst")]
        public bool Dst { get; set; }

        [JsonProperty("gmt")]
        public int Gmt { get; set; }

        [JsonProperty("phone_formatted")]
        public object PhoneFormatted { get; set; }

        [JsonProperty("ext_formatted")]
        public object ExtFormatted { get; set; }

        [JsonProperty("gmt_offset")]
        public int GmtOffset { get; set; }

        [JsonProperty("coordinates")]
        public List<double> Coordinates { get; set; }

        [JsonProperty("manual_coords")]
        public List<double> ManualCoords { get; set; }

        [JsonProperty("abbreviation")]
        public object Abbreviation { get; set; }

        [JsonProperty("zone_name")]
        public object ZoneName { get; set; }
    }

}