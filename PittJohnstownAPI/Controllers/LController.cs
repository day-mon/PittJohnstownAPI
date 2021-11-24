using Microsoft.AspNetCore.Mvc;
using PittJohnstownAPI.Items;
using System.Net;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PittJohnstownAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class LController : ControllerBase
    {

        private static readonly string BASE_URL = "https://www.laundryview.com/api/currentRoomData?school_desc_key=4590&location=";
        private readonly Dictionary<string, string> LAUNDRY_API_CALLS = new Dictionary<string, string>
        {
            ["HICKORY"] = "5813396",
            ["BRIAR"] = "581339005",
            ["BUCKHORN"] = "5813393",
            ["LLC"] = "58133912",
            ["OAK"] = "5813391",
            ["HAWTHORN"] = "5813397",
            ["HEATHER"] = "5813398",
            ["HEMLOCK"] = "5813392",
            ["MAPLE"] = "5813399",
            ["WILLOW"] = "58133912",
            ["LARKSPUR"] = "58133911",
            ["LAUREL"] = "5813394",
            ["CPAS"] = "581339013",

        };



        // GET: api/<LController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<LController>/5
        [HttpGet("{Dormatory}")]
        async public Task<List<LaundryItem>> Get(string Dormatory)
        {
            var upper = Dormatory.ToUpper().Trim();
            var list = new List<LaundryItem>();


            if (!LAUNDRY_API_CALLS.ContainsKey(upper))
            {
                return list;
            }

            var value = LAUNDRY_API_CALLS[upper];

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "C# console program");

            var content = await client.GetStringAsync($"{BASE_URL}{value}");
            var myDeserializedClass = JsonConvert.DeserializeObject<Root>(content);


            foreach (var item in myDeserializedClass.LaundryObjects)
            {

                list.Add(new LaundryItem(item, Dormatory));
                
            }

            return  list;
        }





        // POST api/<LController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<LController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
