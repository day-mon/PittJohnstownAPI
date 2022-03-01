using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<IEnumerable<LaundryModel>>> Get(string dormitory)
        {
            var upper = dormitory.ToUpper().Trim();


            if (!_laundryApiCalls.ContainsKey(upper))
            {
                return NotFound($"{dormitory} was not found at the University of Pittsburgh @ Johnstown");
            }

            var value = _laundryApiCalls[upper];


            var content = await WebHandler.GetWebsiteContent($"{BaseUrl}{value}");

            if (string.IsNullOrEmpty(content))
            {
                return FailedDependency("Exception was caught while attempting to connect to internal api");
            }

            var myDeserializedClass = JsonConvert.DeserializeObject<Root>(content);


            if (myDeserializedClass == null)
            {
                return FailedDependency(
                    "Error occured while trying to deserialize a internal api response.");
            }

            return new ActionResult<IEnumerable<LaundryModel>>(myDeserializedClass.LaundryObjects
                .Where(item =>
                {
                    var aT = item.Type.ToLower();
                    return aT.Equals("washfl") || aT.Equals("dry");
                })
                .Select(item => new LaundryModel(item, dormitory)));
        }

        private ObjectResult FailedDependency(string content)
        {
            return StatusCode(StatusCodes.Status424FailedDependency, content);
        }
    }
}