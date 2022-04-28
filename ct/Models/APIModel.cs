using Newtonsoft.Json;
using ct.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using System.Text;
using System.IO;
using System.Web;

namespace ct.Models
{
    public class APIModel
    {
        public static string BaseUri = "http://erp.ctuniversity.in/";
        //public static string BaseUri ="http://localhost:55252/";

        public static string RmsUri = BaseUri + "administration/AddRMSQueries";
        public static string MarkAttendanceUri = BaseUri + "academic/AddStudentAttendance";
        public static string HrUri = BaseUri + "HR/";
        public static string AcademicUri = BaseUri + "Academic/";
        public static string UserId { get; set; }
        public APIModel(string uid)
        {
            UserId = uid;
        }

        QueryResult GetQueryResult(string url)
        {
            //Compulsary stuff for every requirement
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);

            httpRequest.Headers["Authorization"] = "QWRtaW46QWRtaW4=";

            var userid = Convert.ToBase64String(Encoding.UTF8.GetBytes(UserId));

            httpRequest.Headers["UserID"] = userid;

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                //Serialization after result should be of the type which is returned by the erp api function
                var queryResult = JsonConvert.DeserializeObject<QueryResult>(result);

                return queryResult;
            }
        }

        QueryResult PostData(string url, object deserialiseddata)
        {
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Headers["Authorization"] = "QWRtaW46QWRtaW4=";

            var userid = Convert.ToBase64String(Encoding.UTF8.GetBytes(UserId));

            httpRequest.Headers["UserID"] = userid;
            httpRequest.ContentType = "application/json";

            var data = JsonConvert.SerializeObject(deserialiseddata);

            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                //Serialization after result should be of the type which is returned by the erp api function
                var queryResult = JsonConvert.DeserializeObject<QueryResult>(result);
                return queryResult;
            }
        }

        //GET LIST OF STUDENTS ON BASIS OF GIVEN PARAMETERS
        public QueryResult GetStudentListNew(string CourseCode, string Section, string StudentGroup)
        {
            try
            {
                //Links to be refered from erp and change acording to requirement
                var url = BaseUri + "Academic/GetStudentinfonew?CourseCode=" + CourseCode + "&Section=" + Section + "&StudentGroup=" + StudentGroup;
                var queryResult = GetQueryResult(url);

                return queryResult;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //GET LIST OF COURSES TAUGHT BY THAT FACULTY
        public QueryResult GetCourseListNew()
        {
            try
            {
                //Links to be refered from erp and change acording to requirement
                var url = BaseUri + "Academic/GetCourseListNew";

                var queryResult = GetQueryResult(url);

                queryResult.data = JsonConvert.DeserializeObject<Models.Academics.TeachersCourseModel[]>(queryResult.data.ToString());
                return queryResult;
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        //POST ATTENDANCE 
        //*****************************************************************************************************
        public QueryResult PostAttendance(List<Models.Academics.AddAttendance> AddAttendanceList, string CourseCode, string Currdate,
                                         string Time, string AttendanceType, string StudentGroup, string Section)
        {
            var url = MarkAttendanceUri + "?CourseCode=" + CourseCode + "&Currdate=" + Currdate + "&Section=" + Section +
                      "&Time=" + Time + "&AttendanceType=" + AttendanceType + "&StudentGroup=" + StudentGroup;

            var queryresult = PostData(url, AddAttendanceList);

            return queryresult;
        }

        public object Post(Models.Academics.PostAttendanceModel pam)
        {
           
            try
            {
                var result = new APIModel(pam.UID).PostAttendance(pam.AddAttendanceList, pam.CourseCode, pam.Currdate,
                                                              pam.Time, pam.AttendanceType, pam.StudentGroup, pam.Section);

                if (result.status == true)
                {
                    return result.status;
                }
                else
                {
                   
                    return HttpContext.Current.Request;
                   // return Request.CreateResponse(HttpStatusCode.BadRequest, result.message);
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        //***************************************************************************************************

        //leave session
        public QueryResult GetLeavesSession()
        {
            try
            {
                //Links to be refered from erp and change acording to requirement
                var url = HrUri + "GetLeaveSession";
                var queryResult = GetQueryResult(url);

                return queryResult;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //To post DPR
        public QueryResult AddDPR(Administration.EmployeeDPR ed)
        {
            try
            {
                Administration.EmployeeDPR[] dpr = new Administration.EmployeeDPR[1];
                dpr[0] = ed;

                string url = BaseUri + "administration/AddEmployeeDPR";
                var queryResult = PostData(url, dpr);
                return queryResult;
            }
            catch (Exception e) { return null; }
        }



    }
}


