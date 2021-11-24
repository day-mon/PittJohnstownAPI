using Newtonsoft.Json;
using System.Diagnostics;

namespace PittJohnstownAPI.Items
{
    public class LaundryItem
    {
        
        public string ApplianceID = "0";
        public string type = "N/A";
        public bool IsWorking = false;
        public string TimeRemaining = "N/A";
        public bool IsInUse = false;
        public string Location = "N/A";


        public LaundryItem()
        {
            
        }

        public LaundryItem(LaundryObject Item, string Location)
        {
            var ItemType = Item.Type;
            Debug.WriteLine(ItemType);

            if (!(ItemType.ToUpper().Contains("DRY") || ItemType.Contains("washFL"))) return;

            this.type = Item.Type.ToUpper().StartsWith('D') ? "Dryer" : "Washer";
            this.IsWorking = Item.Percentage <= 5;
            this.ApplianceID = Item.ApplianceDesc;
            this.IsInUse = Item.StatusToggle > 0 && IsWorking;

            Debug.WriteLine($"{Item.TimeLeftLite} | {Item.TimeLeftLite} | {Item.TimeRemaining} | {Item.TimeRemaining2}");

            this.TimeRemaining = Item.TimeLeftLite;
            this.Location = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Location);

            Debug.WriteLine($"Called: {this}");
        }



        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
