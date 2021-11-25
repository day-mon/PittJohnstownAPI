using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PittJohnstownAPI.Items.Dining;
using System.Diagnostics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PittJohnstownAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DiningController : ControllerBase
    {
       
        [HttpGet("{period}/{DinnigStation}")]
        async public Task<List<Item>> GetByDiningStation(string DinnigStation, string period)
        {
            var jsonObject = await GetDataJsonByPeriod(period);

            if (jsonObject == null) return new List<Item>();


            return jsonObject.Menu
                .Periods
                .Categories
                .Find(item => item.Name.ToLower() == DinnigStation.ToLower())
                .Items;

        }
        // GET api/<DiningController>/5w
        [HttpGet("{period}")]
        async public Task<List<Category>> GetByPeriod(string period)
        {
            var jsonObject = await GetDataJsonByPeriod(period);

            if (jsonObject == null) return new List<Category>();


            return jsonObject.Menu
                .Periods
                .Categories;
        }


        async private Task<Root?> GetDataJsonByPeriod(string Period)
        {
            var handler = WebHandler.GetInstance();
            var PeriodId = await GetPeriod(Period);

            if (String.IsNullOrEmpty(PeriodId))
            {
                return null;
            }

            var BASE_URL = $"https://api.dineoncampus.com/v1/location/5f3c3313a38afc0ed9478518/periods/{PeriodId}?platform=0&date={DateTime.Now.ToString("yyyy-MM-dd")}";
            var content = await handler.GetWebsiteContent(BASE_URL);

            return JsonConvert.DeserializeObject<Root>(content);
        }


        async private Task<string> GetPeriod(string period)
        {

            var handler = WebHandler.GetInstance();
            string BASE_URL = $"https://api.dineoncampus.com/v1/location/5f3c3313a38afc0ed9478518/periods?platform=0&date={DateTime.Now.ToString("yyyy-MM-dd")}";

            var content = await handler.GetWebsiteContent(BASE_URL);

            var myDeserializedClass = JsonConvert.DeserializeObject<Root>(content);
            if (myDeserializedClass == null) return "";


            foreach (var k in myDeserializedClass.Periods)
            {
                if (k.Name.ToLower() == period.ToLower()) return k.Id;
            }

            return "";
         
        }
 
    }
}
