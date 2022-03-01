using Newtonsoft.Json;

namespace PittJohnstownAPI.Models.Laundry
{
    public class LaundryModel
    {
        public LaundryModel(LaundryObject item, string location)
        {
            type = item.Type.ToUpper().StartsWith('D') ? "Dryer" : "Washer";
            IsWorking = item.Percentage <= 5;
            ApplianceID = item.ApplianceDesc;
            IsInUse = item.StatusToggle > 0 && IsWorking;
            TimeRemaining = item.TimeLeftLite;
            this.Location = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(location);
        }

        public string? ApplianceID { get; set; }
        public string type { get; set; }
        public bool IsWorking { get; set; }
        public string TimeRemaining { get; set; }
        public bool IsInUse { get; set; }
        public string Location { get; set; }


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
