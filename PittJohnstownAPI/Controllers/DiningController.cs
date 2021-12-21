using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PittJohnstownAPI.Items.Dining;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PittJohnstownAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DiningController : ControllerBase
    {
        [HttpGet("{period}/{diningStation}")]
        public async Task<List<Item>> GetByDiningStation(string diningStation, string period)
        {
            var jsonObject = await GetDataJsonByPeriod(period);


            return jsonObject?
                .Menu?
                .Periods?
                .Categories?
                .Find(item => item.Name != null && item.Name.Equals(diningStation, StringComparison.OrdinalIgnoreCase))
                ?.Items ?? new List<Item>();
                


        }

        // GET api/<DiningController>/5
        [HttpGet("{period}")]
        public async Task<List<Category>> GetByPeriod(string period)
        {
            var jsonObject = await GetDataJsonByPeriod(period);

            if (jsonObject == null) return new List<Category>();


            return jsonObject?
                .Menu?
                .Periods?
                .Categories ?? new List<Category>();
        }


        private static async Task<Root?> GetDataJsonByPeriod(string period)
        {
            var handler = WebHandler.GetInstance();
            var periodId = await GetPeriod(period);

            if (string.IsNullOrEmpty(periodId))
            {
                return null;
            }

            var baseUrl = $"https://api.dineoncampus.com/v1/location/5f3c3313a38afc0ed9478518/periods/{periodId}?platform=0&date={DateTime.Now:yyyy-MM-dd}";
            var content = await WebHandler.GetWebsiteContent(baseUrl);

            return JsonConvert.DeserializeObject<Root>(content);
        }


        private static async Task<string?> GetPeriod(string period)
        {
            var baseUrl = $"https://api.dineoncampus.com/v1/location/5f3c3313a38afc0ed9478518/periods?platform=0&date={DateTime.Now:yyyy-MM-dd}";

            var content = await WebHandler.GetWebsiteContent(baseUrl);

            var myDeserializedClass = JsonConvert.DeserializeObject<Root>(content);
            if (myDeserializedClass == null) return string.Empty;


            foreach (var var in myDeserializedClass.Periods.Select(k => k?.Name?.Equals(period, StringComparison.OrdinalIgnoreCase).ToString()))
            {
                if (var != null && var.Equals(period, StringComparison.OrdinalIgnoreCase)) return var;
            }

            return string.Empty;
        }
    }
}
