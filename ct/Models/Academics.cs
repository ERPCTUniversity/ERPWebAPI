using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ct.Models
{
    public class Academics
    {
        //ATTENDANCE MODELS
        public class TeachersCourseModel
        {
            public string CourseCode { get; set; }
            public string CourseTitle { get; set; }
            public string L { get; set; }
            public string T { get; set; }
            public string P { get; set; }
            public string Section { get; set; }
            public string StudentGroup { get; set; }
            public string ProgramName { get; set; }
        }
        public class AddAttendance
        {
            public string TermId { get; set; }
            public string SectionAuthorizationId { get; set; }
            public string RegistrationNumber { get; set; }
            public string CourseCode { get; set; }
            public string Date { get; set; }
            public string Day { get; set; }
            public string TimeSlot { get; set; }
            public string Attendance { get; set; }
            public string EntryBy { get; set; }
            public string AttendanceType { get; set; }
            //public string StudentGroup { get; set; }
        }

        public class PostAttendanceModel
        {
            public List<Models.Academics.AddAttendance> AddAttendanceList { get; set; }
            public string CourseCode { get; set; }
            public string Currdate { get; set; }
            public string Time { get; set; }
            public string AttendanceType { get; set; }
            public string StudentGroup { get; set; }
            public string Section { get; set; }
            public string UID { get; set; }
        }
    }
}