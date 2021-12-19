﻿using System.Diagnostics;
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
        public async Task<Course> GetCourseByCourseId(int periodId, int courseId)
        {

            var handler = WebHandler.GetInstance();
            var redirects = await handler.CheckRedirects("https://psmobile.pitt.edu/app/catalog/classSearch/");

            //https://psmobile.pitt.edu/app/catalog/classSearch

            if (redirects)
            {
                Debug.WriteLine("test");
                return new Course();
            }

            var period = IsValidTerm(periodId);

            if (!period)
            {
                Debug.WriteLine("test2");
                return new Course();
            }

            var url = $"https://psmobile.pitt.edu/app/catalog/classsection/UPITT/{periodId}/{courseId}";
            var content = await WebHandler.GetWebsiteContent(url);
            return GetCourseFromHtml(content);;
        }

        private bool ValidateCourseId(int CourseId)
        {
            return true;
        }

        private static Course GetCourseFromHtml(string content)
        {
            var course = new Course();
            var html = new HtmlDocument();
            html.LoadHtml(content);

            var all = GetElementsByClassName(html, "section-content clearfix");
            var identifier = GetElementsByClassName(html, "page-title  with-back-btn")
                .FirstOrDefault()?
                .InnerText ?? "";

            var elementsLeft = GetElementsByClassName(html, "pull-left");
            var elementsRight = GetElementsByClassName(html ,"pull-right");
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
                    case "Instructor(s)":
                        course.Instructors = textRight.Split(',').ToList();
                        break;
                    case "Meets":
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
                        course.Componenets = textRight;
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
                        course.ClassCapacaity = int.Parse(textRight);
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
            
            // Will finish tomorrow

            return course;
        }


        private static List<HtmlNode> GetElementsByClassName(HtmlDocument doc, String className)
        {
            var regex = new Regex("\\b" + Regex.Escape(className) + "\\b", RegexOptions.Compiled);

            return doc.DocumentNode
                .Descendants()
                .Where(n => n.NodeType == HtmlNodeType.Element)
                .Where(e => e.Name == "div" && regex.IsMatch(e.GetAttributeValue("class", "")))
                .ToList();
        }


        private static bool IsValidTerm(int PeriodId)
        {
            var periodStr = PeriodId.ToString();

            if (periodStr.Length != 4) return false;
            if (periodStr[0] != '2') return false;

            var SeasonNum = int.Parse(periodStr[3].ToString());
            // pretty you can just do var SeasonNum = PeriodStr[3]

            return !(SeasonNum != 4 && SeasonNum != 1 && SeasonNum != 7);
        }
    }
}
