using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PittJohnstownAPI.Models.Dining;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PittJohnstownAPI.Controllers
{
    [Route("api/[controller]/{period}")]
    [ApiController]
    public class DiningController : ControllerBase
    {
        [HttpGet("{diningStation}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(List<FoodModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<FoodModel>>> GetByDiningStation(string diningStation, string period)
        {
            if (!(period.Equals("breakfast", StringComparison.OrdinalIgnoreCase) ||
                  period.Equals("lunch", StringComparison.OrdinalIgnoreCase) ||
                  period.Equals("dinner", StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(period)))
            {
                return UnprocessableEntity(
                    $"{period} is a incorrect Period. Please try a correct one such as Breakfast, Lunch, or Dinner");
            }

            var (error, _, response) = await GetDataJsonByPeriod(period);

            if (response == null)
            {
                return EvaluateErrors(error);
            }


            return response
                .Menu
                .Periods?
                .Categories
                .Find(item => item.Name != null && item.Name.Equals(diningStation, StringComparison.OrdinalIgnoreCase))
                ?.Items ?? new List<FoodModel>();
        }

        // GET api/<DiningController>/5
        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]

        [ProducesResponseType(typeof(List<FoodModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Category>>> GetByPeriod(string period)
        {
            var (error, _, root) = await GetDataJsonByPeriod(period);

            
            
            if (root == null) return EvaluateErrors(error);


            return root?
                .Menu
                .Periods?
                .Categories ?? new List<Category>();
        }


        private static async Task<FunctionResponse> GetDataJsonByPeriod(string period)
        {
            var periodFunctionResponse = await GetPeriod(period);

            if (periodFunctionResponse.Period == string.Empty)
            {
                return periodFunctionResponse;
            }


            var baseUrl =
                $"https://api.dineoncampus.com/v1/location/5f3c3313a38afc0ed9478518/periods/{periodFunctionResponse}?platform=0&date={DateTime.Now:yyyy-MM-dd}";
            var content = await WebHandler.GetWebsiteContent(baseUrl);

            return string.IsNullOrEmpty(content)
                ? new FunctionResponse("Something went wrong when attempting to read response.", string.Empty, null)
                : new FunctionResponse(string.Empty, string.Empty, JsonConvert.DeserializeObject<DiningModel>(content));
        }


        private static async Task<FunctionResponse> GetPeriod(string period)
        {
            var baseUrl =
                $"https://api.dineoncampus.com/v1/location/5f3c3313a38afc0ed9478518/periods?platform=0&date={DateTime.Now:yyyy-MM-dd}";

            var content = await WebHandler.GetWebsiteContent(baseUrl);
            
            if (content.Equals( @"{""status"":""error"",""msg"":""No menu""}", StringComparison.OrdinalIgnoreCase))
            {
                return new FunctionResponse("No menu available for today's date!", string.Empty, null);
            }
            
            if (content == string.Empty)
            {
                return new FunctionResponse("Something went wrong when attempting to read response.", string.Empty,
                    null);
            }

            var myDeserializedClass = JsonConvert.DeserializeObject<DiningModel>(content);
            if (myDeserializedClass == null)
                return new FunctionResponse("Could not deserialize response.", string.Empty, null);


            foreach (var var in myDeserializedClass.Periods.Select(k =>
                         k.Name?.Equals(period, StringComparison.OrdinalIgnoreCase).ToString()))
            {
                if (var != null && var.Equals(period, StringComparison.OrdinalIgnoreCase))
                    return new FunctionResponse(string.Empty, var, null);
            }

            return new FunctionResponse("Could not find period.", string.Empty, null);
        }



        private ActionResult EvaluateErrors(string error)
        {
            return error switch
            {
                "Something went wrong when attempting to read response." => Unauthorized($"{error} Could not connect to external url"),
                "Could not deserialize response" => UnprocessableEntity(error),
                "Could not find period." => UnprocessableEntity(error),
                "No menu available for today's date!" => NotFound(error),
                _ => BadRequest("Unknown error occured")
            };
        }
        private record FunctionResponse(string Error, string Period, DiningModel? Root);
    }
}