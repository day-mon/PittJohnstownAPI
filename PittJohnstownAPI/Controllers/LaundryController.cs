﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PittJohnstownAPI.Models.Laundry;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PittJohnstownAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class LaundryController : ControllerBase
    {
        private const string BaseUrl = "https://www.laundryview.com/api/currentRoomData?school_desc_key=4590&location=";

        private readonly Dictionary<string, string> _laundryApiCalls = new()
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


        // GET api/<LaundryController>/5
        [HttpGet("{dormitory}")]
        public async Task<List<LaundryModel>> Get(string dormitory)
        {
            var upper = dormitory.ToUpper().Trim();
            var list = new List<LaundryModel>();


            if (!_laundryApiCalls.ContainsKey(upper))
            {
                return list;
            }

            var value = _laundryApiCalls[upper];

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "C# console program");

            var content = await client.GetStringAsync($"{BaseUrl}{value}");
            var myDeserializedClass = JsonConvert.DeserializeObject<Root>(content);



            if (myDeserializedClass == null)
            {
                return list;
            }
            
            return myDeserializedClass.LaundryObjects
                                 .Where(item => item.Type.ToUpper().Contains("Dry") || item.Type.Contains("washFL"))
                                 .Select(item => new LaundryModel(item, dormitory))
                                 .ToList();
        }
    }
}
