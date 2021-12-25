namespace PittJohnstownAPI.Models.Course
{
    public class CourseModel
    {
        public string Identifier { get; set; }
        public string? Session { get; set; }
        public int ClassNumber { get; set; }
        public string? Career { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Units { get; set; }
        public string? DropConsent { get; set; }
        public string? Grading { get; set; }
        public string? Description { get; set; }
        public string? ClassAttributes { get; set; }
        public string? EnrollmentRequirements { get; set; }

        // Class Details
        public List<string> Instructors { get; set; }
        public List<string> MeetingDays { get; set; }
        public string? Campus { get; set; }
        public string? Location { get; set; }
        public string? Room { get; set; }
        public string? Components { get; set; }

        // Class Availability 
        public string? Status { get; set; }
        public int SeatsTaken { get; set; }
        public int SeatsOpen { get; set; }
        public int ClassCapacity { get; set; }
        public int UnrestrictedSeats { get; set; }
        public int RestrictedSeats { get; set; }
        public int WaitListTotal { get; set; }
        public int WaitListCapacity { get; set; }
    }
}
