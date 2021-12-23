using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using PittJohnstownAPI.Items.Course;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PittJohnstownAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        // GET api/<CourseController>/5
        [HttpGet("{periodId}/{courseId}")]
        public async Task<CourseModel> GetCourseByCourseId(int periodId, int courseId)
        {
            var handler = WebHandler.GetInstance();
            var redirects = await WebHandler.CheckRedirects("https://psmobile.pitt.edu/app/catalog/classSearch/");

            //https://psmobile.pitt.edu/app/catalog/classSearch

            if (redirects)
            {
                return new CourseModel();
            }

            var period = IsValidTerm(periodId);

            if (!period)
            {
                return new CourseModel();
            }

            var url = $"https://psmobile.pitt.edu/app/catalog/classsection/UPITT/{periodId}/{courseId}";
            var content = await WebHandler.GetWebsiteContent(url);
            return GetCourseFromHtml(content);
        }


        private static CourseModel GetCourseFromHtml(string content)
        {
            var course = new CourseModel();
            var html = new HtmlDocument();
            html.LoadHtml(content);
            var all = GetElementsByClassName(html, "section-content clearfix");
            var identifier = GetElementsByClassName(html, "page-title  with-back-btn", false)
                .FirstOrDefault()?
                .InnerText ?? "";

            course.Identifier = identifier;

            Console.WriteLine(identifier);

            var elementsLeft = GetElementsByClassName(html, "pull-left");
            var elementsRight = GetElementsByClassName(html, "pull-right");


            var elementsRightSize = elementsLeft.Count;

            // TODO: Meeting Dates

            for (int left = 0, right = 0; right < elementsRightSize; left++, right++)
            {
                // Text on left side of class page (i.e Description, Class Times, etc);
                var textLeft = elementsLeft[left].InnerText.Trim();
                // Text on right side of class page (i.e actual data)
                var textRight = elementsRight[right].InnerText.Trim();

                switch (textLeft)
                {
                    case "Session":
                        course.Session = textRight;
                        break;
                    case "Class Number":
                        course.ClassNumber = int.Parse(textRight);
                        break;
                    case "Career":
                        course.Career = textRight;
                        break;
                    case "Dates":
                        var dates = textRight.Split('-');
                        var start = dates[0].Trim().Split("/");
                        var end = dates[1].Trim().Split("/");

                        var startDate = new DateTime(int.Parse(start[2]), int.Parse(start[0]), int.Parse(start[1]));
                        var endDate = new DateTime(int.Parse(end[2]), int.Parse(end[0]), int.Parse(end[1]));

                        course.StartDate = startDate;
                        course.EndDate = endDate;
                        break;
                    case "Units":
                        course.Units = int.Parse(textRight[..1]);
                        break;
                    case "Grading":
                        course.Grading = textRight;
                        break;
                    case "Description":
                        course.Description = textRight;
                        break;
                    case "Class Attributes":
                        course.ClassAttributes = textRight;
                        break;
                    case "Drop Consent":
                        course.DropConsent = textRight;
                        break;
                    case "Enrollment Requirements":
                        course.EnrollmentRequirements = textRight;
                        break;
                    case "Instructor(s)":
                        course.Instructors = textRight.Split(',').ToList();
                        break;
                    case "Meets":
                        course.MeetingDays = ParseDayOfWeeks(textRight);
                        // returns start date if value is null 
                        course.StartDate = ParseClassTime(textRight, course) ?? course.StartDate;
                        course.EndDate = ParseClassTime(textRight, course, 2) ?? course.EndDate;

                        break;
                    case "Meeting Dates":
                        break;
                    case "Location":
                        course.Location = textRight;
                        break;
                    case "Room":
                        course.Room = textRight;
                        break;
                    case "Campus":
                        course.Campus = textRight;
                        break;
                    case "Components":
                        course.Components = textRight;
                        break;
                    case "Status":
                        course.Status = textRight;
                        break;
                    case "Seats Taken":
                        course.SeatsTaken = int.Parse(textRight);
                        break;
                    case "Seats Open":
                        course.SeatsOpen = int.Parse(textRight);
                        break;
                    case "Class Capacity":
                        course.ClassCapacity = int.Parse(textRight);
                        break;
                    case "Unrestricted Seats":
                        course.UnrestrictedSeats = int.Parse(textRight);
                        break;
                    case "Restricted Seats":
                        course.RestrictedSeats = int.Parse(textRight);
                        break;
                    case "Wait List Total":
                        course.WaitListTotal = int.Parse(textRight);
                        break;
                    case "Wait List Capacity":
                        course.WaitListCapacity = int.Parse(textRight);
                        break;
                }
            }


            return course;
        }


        private static List<HtmlNode> GetElementsByClassName(HtmlDocument doc, string className,
            bool ignoreAllButDivs = true)
        {
            var regex = new Regex("\\b" + Regex.Escape(className) + "\\b", RegexOptions.Compiled);

            return doc.DocumentNode
                .Descendants()
                .Where(n => n.NodeType == HtmlNodeType.Element)
                .Where(e => ignoreAllButDivs
                    ? e.Name == "div" && regex.IsMatch(e.GetAttributeValue("class", ""))
                    : regex.IsMatch(e.GetAttributeValue("class", "")))
                .ToList();
        }


        private static List<string> ParseDayOfWeeks(string? days)
        {
            if (string.IsNullOrWhiteSpace(days)) return new List<string>();
            if (days.Equals("TBA")) return new List<string> {"TBA"};

            var dayOfWeeks = new Dictionary<string, DayOfWeek>
            {
                ["Mo"] = DayOfWeek.Monday,
                ["Tu"] = DayOfWeek.Tuesday,
                ["We"] = DayOfWeek.Wednesday,
                ["Th"] = DayOfWeek.Thursday,
                ["Thr"] = DayOfWeek.Thursday,
                ["Fr"] = DayOfWeek.Friday,
                ["Fri"] = DayOfWeek.Friday
            };

            //split on upper case
            var splitStr = Regex.Split(days, "\\s+");
            var daysStr = Regex.Split(splitStr[0], @"(?<!^)(?=[A-Z])");
            return daysStr.Length == 0
                ? new List<string>()
                : (from day in daysStr where dayOfWeeks.ContainsKey(day) select dayOfWeeks[day].ToString()).ToList();
        }

        private static DateTime? ParseClassTime(string? days, CourseModel course, int type = 1)
        {
            if (days is null or "TBA") return null;
            var splitStr = Regex.Split(days, "\\s+");


            var start = splitStr[1];
            var end = splitStr[3];

            switch (type)
            {
                case 1 when start.Contains("AM"):
                {
                    var time = start.Split("AM");
                    var clock = time[0].Split(":");

                    var houseParsed = int.TryParse(clock[0], out var hour);
                    var minuteParsed = int.TryParse(clock[1], out var minute);

                    if (!houseParsed)
                    {
                        return null;
                    }

                    var c = course.StartDate;

                    if (minuteParsed) return new DateTime(c.Year, c.Month, c.Day, hour, minute, 0);
                    return null;
                }
                case 1 when start.Contains("PM"):
                {
                    var time = start.Split("PM");
                    var clock = time[0].Split(":");

                    var houseParsed = int.TryParse(clock[0], out var hour);


                    if (!houseParsed)
                    {
                        return null;
                    }

                    var min = 0;

                    if (!clock[1].Equals("00", StringComparison.OrdinalIgnoreCase))
                    {
                        var minuteParsed = int.TryParse(clock[1], out var minute);

                        min = minute;
                        if (!minuteParsed)
                        {
                            return null;
                        }
                    }

                    if (hour != 12)
                    {
                        hour += 12;
                    }

                    var c = course.StartDate;

                    return new DateTime(c.Year, c.Month, c.Day, hour, min, 0);
                }
                case 2 when end.Contains("AM"):
                {
                    var time = end.Split("AM");
                    var clock = time[0].Split(":");

                    var houseParsed = int.TryParse(clock[0], out var hour);
                    var minuteParsed = int.TryParse(clock[1], out var minute);

                    if (!houseParsed)
                    {
                        return null;
                    }

                    var c = course.EndDate;


                    if (minuteParsed) return new DateTime(c.Year, c.Month, c.Day, hour, minute, 0);
                    return null;
                }
                case 2 when end.Contains("PM"):
                {
                    var time = end.Split("PM");
                    var clock = time[0].Split(":");
                    var houseParsed = int.TryParse(clock[0], out var hour);


                    if (!houseParsed)
                    {
                        return null;
                    }

                    var min = 0;

                    if (!clock[1].Equals("00", StringComparison.OrdinalIgnoreCase))
                    {
                        var minuteParsed = int.TryParse(clock[1], out var minute);

                        min = minute;
                        if (!minuteParsed)
                        {
                            return null;
                        }
                    }

                    if (hour != 12)
                    {
                        hour += 12;
                    }

                    var c = course.EndDate;

                    return new DateTime(c.Year, c.Month, c.Day, hour, min, 0);
                }
                default:
                    return null;
            }
        }


        private static bool IsValidTerm(int PeriodId)
        {
            return Regex.IsMatch(PeriodId.ToString(), "2\\d\\d[147]");
        }
    }
}