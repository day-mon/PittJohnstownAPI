using Newtonsoft.Json; 
using System.Collections.Generic; 
namespace PittJohnstownAPI.Items.Dining{ 

    public class Item
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("mrn")]
        public string Mrn { get; set; }

        [JsonProperty("rev")]
        public string Rev { get; set; }

        [JsonProperty("mrn_full")]
        public string MrnFull { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        [JsonProperty("webtrition_id")]
        public int WebtritionId { get; set; }

        [JsonProperty("sort_order")]
        public int SortOrder { get; set; }

        [JsonProperty("portion")]
        public string Portion { get; set; }

        [JsonProperty("qty")]
        public object Qty { get; set; }

        [JsonProperty("ingredients")]
        public string Ingredients { get; set; }

        [JsonProperty("nutrients")]
        public List<Nutrient> Nutrients { get; } = new List<Nutrient>();

        [JsonProperty("filters")]
        public List<Filter> Filters { get; } = new List<Filter>();

        [JsonProperty("custom_allergens")]
        public List<object> CustomAllergens { get; } = new List<object>();

        [JsonProperty("calories")]
        public int Calories { get; set; }
    }

}