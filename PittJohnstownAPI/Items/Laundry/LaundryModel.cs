using Newtonsoft.Json;

namespace PittJohnstownAPI.Items.Laundry
{
    public class LaundryModel
    {
        public LaundryModel(LaundryObject Item, string Location)
        {
            this.type = Item.Type.ToUpper().StartsWith('D') ? "Dryer" : "Washer";
            this.IsWorking = Item.Percentage <= 5;
            this.ApplianceID = Item.ApplianceDesc;
            this.IsInUse = Item.StatusToggle > 0 && IsWorking;
            this.TimeRemaining = Item.TimeLeftLite;
            this.Location = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Location);
        }

        public string ApplianceID { get; set; }
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
