using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;
using NLog.Fluent;
using PittJohnstownAPI.Models.Dining;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PittJohnstownAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiningController : ControllerBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        [HttpGet("station/{period}/{diningStation}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status424FailedDependency)]
        [ProducesResponseType(typeof(string), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(List<FoodModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<FoodModel>>> GetByDiningStation(string diningStation, string period)
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

            var item = response
                .Menu
                .Periods?
                .Categories
                .Find(item => item.Name != null && item.Name.Equals(diningStation, StringComparison.OrdinalIgnoreCase))
                ?.Items;

            if (item == null)
            {
                return NotFound($"No stations found with the name {diningStation}.");
            }

            return item;
        }

        // GET api/<DiningController>/5
        [HttpGet("period/{period}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status424FailedDependency)]
        [ProducesResponseType(typeof(string), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(List<FoodModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Category>>> GetByPeriod(string period)
        {
            var (error, _, root) = await GetDataJsonByPeriod(period);


            if (root != null)
                return root
                    .Menu
                    .Periods?
                    .Categories ?? new List<Category>();
            
            Logger.Error("Error occured while attempting to get dining options by period");
            return EvaluateErrors(error);


        }


        [HttpGet("status/{status}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status424FailedDependency)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(List<Location>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DiningLocationInfoModel>>> GetDiningLocationByStatus(string status)
        {
            if (!(status.Equals("open", StringComparison.OrdinalIgnoreCase) ||
                  status.Equals("closed", StringComparison.OrdinalIgnoreCase)))
            {
                Logger.Error($"Status must be open or closed. {status} is not a valid status");
                return UnprocessableEntity($"Status must be open or closed. {status} is not a valid status");
            }

            var content = await WebHandler.GetWebsiteContent(
                "https://api.dineoncampus.com/v1/locations/status?site_id=5eb2ff424198d433da74c3bd&platform=0");
            var infoModel = JsonConvert.DeserializeObject<DiningInfoModel>(content);

            if (infoModel == null)
            {
                Logger.Error("Error occured when deserializing an external api response.");
                return EvaluateErrors(
                    "Error occured when deserializing an external api response.");
            }

            var models = infoModel.Locations
                .Where(loc => loc.Status.Label.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();


            if (models.Count != 0)
                return new ActionResult<IEnumerable<DiningLocationInfoModel>>(models.Select(loc =>
                    new DiningLocationInfoModel(loc.Name, loc.Open, loc.Status.Message)));
                    
            Logger.Warn($"Returning a 404, Could not find any dining locations with status: {status}");
            return NotFound($"Could not find any dining locations with the status: {status}!");

        }

        [HttpGet("period/{period}/{date}")]
        public async Task<ActionResult<List<Category>>> GetDinningOptionOnDate(string period, string date)
        {
            if (!DateTime.TryParse(date, out var result))
                return UnprocessableEntity($"{date} is not parseable. Format yyyy-MM-dd");
            var (error, _, root) = await GetDataJsonByPeriod(period, result);

            if (root == null)
            {
                return EvaluateErrors(error);
            }

            return root
                .Menu
                .Periods?
                .Categories ?? new List<Category>();
        }

        private static async Task<FunctionResponse> GetDataJsonByPeriod(string period, DateTime? time = null)
        {
            time ??= DateTime.Now;
            var periodFunctionResponse = await GetPeriod(period);

            if (periodFunctionResponse.Period == string.Empty)
            {
                return periodFunctionResponse;
            }

            var baseUrl =
                $"https://api.dineoncampus.com/v1/location/5f3c3313a38afc0ed9478518/periods/{periodFunctionResponse}?platform=0&date={time:yyyy-MM-dd}";
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

            if (content.Equals(@"{""status"":""error"",""msg"":""No menu""}", StringComparison.OrdinalIgnoreCase))
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
                "Something went wrong when attempting to read response." => Unauthorized(
                    $"{error} Could not connect to external url"),
                "Could not deserialize response" => UnprocessableEntity(error),
                "Could not find period." => UnprocessableEntity(error),
                "No menu available for today's date!" => NotFound(error),
                _ => FailedDependency("Internal API returned a invalid response.")
            };
        }

        private ObjectResult FailedDependency(string content)
        {
            return StatusCode(StatusCodes.Status424FailedDependency, content);
        }

        private record FunctionResponse(string Error, string Period, DiningModel? Root);
    }
}