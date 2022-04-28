using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using ct.Models;


namespace ct.Controllers.Academics
{
    public class AcademicsController : ApiController
    {
        //ATTENDANCE
        //*************************************************************************************************

        //GET ATTENDANCE FOR STUDENT BASED ON REG NO
        public object GetStudentAttendanced(string UID)
        {
            QueryResult qr = new QueryResult();

            string ConnectionString = WebConfigurationManager.ConnectionStrings["CTUMSCONApp"].ConnectionString;
            SqlConnection con = null;
            SqlDataReader sqlrdr = null;
            try
            {
                con = new SqlConnection(ConnectionString);

                con.Open();

                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = con;
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "[dbo].[pStudentGetCourseDetails]";
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;
                sqlcmd.Parameters.AddWithValue("@UID", UID);
                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {
                        CourseCode = x["CourseCode"].ToString(),
                        TermId = x["TermId"].ToString(),
                        TeacherUID = x["TeacherUID"].ToString(),
                        TotalAttendence = x["TotalAttendence"].ToString(),
                        LectureAttended = x["LectureAttended"].ToString(),
                        Percentage = x["Percentage"].ToString(),
                        Total = x["Agg"].ToString(),
                      //  CourseTitle = x["CourseTitle"].ToString()

                    }).ToList();

                    qr.status = true;
                    qr.data = Response;
                }
                else
                {
                    throw new Exception("No records found!");
                }

            }
            catch (Exception ex)
            {
                qr.data = null;
                qr.status = false;
                qr.message = ex.Message;
            }
            finally
            {
                if (sqlrdr != null)
                {
                    sqlrdr.Close();
                }
                if (con != null)
                {
                    con.Close();
                }
            }
            return qr;
        }

        //SEE LIST OF COURSES FOR A TEACHER
        public object GetCourseList(string UID, string TermID)
        {
             QueryResult qr = new  QueryResult();

            string ConnectionString = WebConfigurationManager.ConnectionStrings["CTUMSCONApp"].ConnectionString;
            SqlConnection con = null;
            SqlDataReader sqlrdr = null;
            try
            {
                con = new SqlConnection(ConnectionString);

                con.Open();

                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = con;
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "[dbo].[CourseListViewAttendanceApp]";
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;
                sqlcmd.Parameters.AddWithValue("@UID", UID);
                sqlcmd.Parameters.AddWithValue("@TermID", TermID);

                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {
                        CourseCode = x["CourseCode"].ToString(),
                        CourseTitle = x["CourseTitle"].ToString(),
                        Section = x["Section"].ToString(),
                        StudentGroup = x["StudentGroup"].ToString()
                    }).ToList();

                    qr.status = true;
                    qr.data = Response;
                }
                else
                {
                    throw new Exception("No records found!");
                }

            }
            catch (Exception ex)
            {
                qr.data = null;
                qr.status = false;
                qr.message = ex.Message;
            }
            finally
            {
                if (sqlrdr != null)
                {
                    sqlrdr.Close();
                }
                if (con != null)
                {
                    con.Close();
                }
            }
            return qr;
        }

        //GET STUDENT'S ATTENDANCE BASED ON REG NO AND COURSE CODE
        public object GetStudentAttendance(string RegistrationNumber, string CourseCode)
        {
            try
            {
                string ConnectionString = WebConfigurationManager.ConnectionStrings["CTUMSCONApp"].ConnectionString;
                SqlConnection con = null;
                SqlDataReader sqlrdr = null;
                try
                {
                    con = new SqlConnection(ConnectionString);

                    con.Open();

                    SqlCommand sqlcmd = new SqlCommand();
                    sqlcmd.Connection = con;
                    sqlcmd.CommandType = CommandType.StoredProcedure;

                    sqlcmd.CommandText = "[dbo].[pGetStudentCourseAttendance]";
                    DataTable dt = new DataTable();
                    SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                    sqldataadapter.SelectCommand = sqlcmd;
                    sqlcmd.Parameters.AddWithValue("@RegistrationNumber", RegistrationNumber);
                    sqlcmd.Parameters.AddWithValue("@CourseCode", CourseCode);
                    sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    sqldataadapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        var Response = dt.AsEnumerable().Select(x => new {
                            Date = x["Date"].ToString(),
                            Attendance = x["Attendance"].ToString()
                        }).ToList();

                        return Response;
                    }
                    else
                    {
                        throw new Exception("No records found!");
                    }

                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
                finally
                {
                    if (sqlrdr != null)
                    {
                        sqlrdr.Close();
                    }
                    if (con != null)
                    {
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //GET ALL MONTHS WHEN ATTENDANCE IS MARKED
        public object GetAttendanceMonths(string UID, string CourseCode, string Section, string Group)
        {
             QueryResult qr = new  QueryResult();

            string ConnectionString = WebConfigurationManager.ConnectionStrings["CTUMSCONApp"].ConnectionString;
            SqlConnection con = null;
            SqlDataReader sqlrdr = null;
            try
            {
                con = new SqlConnection(ConnectionString);

                con.Open();

                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = con;
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "[dbo].[AttendanceMonths]";
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;
                sqlcmd.Parameters.AddWithValue("@UID", UID);
                sqlcmd.Parameters.AddWithValue("@CourseCode", CourseCode);
                sqlcmd.Parameters.AddWithValue("@Section", Section);
                sqlcmd.Parameters.AddWithValue("@Group", Group);
                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {
                        Month = x["Month"].ToString()
                    }).ToList();

                    qr.status = true;
                    qr.data = Response;
                }
                else
                {
                    throw new Exception("No records found!");
                }

            }
            catch (Exception ex)
            {
                qr.data = null;
                qr.status = false;
                qr.message = ex.Message;
            }
            finally
            {
                if (sqlrdr != null)
                {
                    sqlrdr.Close();
                }
                if (con != null)
                {
                    con.Close();
                }
            }
            return qr;
        }

        //GET TOTAL CLASS ATTENDANCE (ALL LECTURES - HOW MANY PRESENT AND HOW MANY ABSENT)
        public object GetTotalLectures(string UID, string MonthYear, string CourseCode)
        {
             QueryResult qr = new  QueryResult();

            string ConnectionString = WebConfigurationManager.ConnectionStrings["CTUMSCONApp"].ConnectionString;
            SqlConnection con = null;
            SqlDataReader sqlrdr = null;
            try
            {
                con = new SqlConnection(ConnectionString);

                con.Open();

                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = con;
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "[dbo].[ClassAttendanceApp]";
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;
                sqlcmd.Parameters.AddWithValue("@UID", UID);
                sqlcmd.Parameters.AddWithValue("@CourseCode", CourseCode);
                sqlcmd.Parameters.AddWithValue("@MonthYear", MonthYear);
                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {
                        CourseCode = x["CourseCode"].ToString(),
                        date = x["date"].ToString(),
                        TimeSlot = x["TimeSlot"].ToString(),
                        Present = x["Present"].ToString(),
                        Absent = x["Absent"].ToString(),
                        Day = x["Day"].ToString(),
                        MonthDate = x["MonthDate"].ToString()

                    }).ToList();

                    qr.status = true;
                    qr.data = Response;
                }
                else
                {
                    throw new Exception("No records found!");
                }

            }
            catch (Exception ex)
            {
                qr.data = null;
                qr.status = false;
                qr.message = ex.Message;
            }
            finally
            {
                if (sqlrdr != null)
                {
                    sqlrdr.Close();
                }
                if (con != null)
                {
                    con.Close();
                }
            }
            return qr;
        }

        //GET LIST OF STUDENTS ON BASIS OF GIVEN PARAMETERS
        public object GetStudentListNew(string CourseCode, string Section, string StudentGroup, string UID)
        {
            var result = new APIModel(UID).GetStudentListNew(CourseCode, Section, StudentGroup);

            if (result.status == true)
            {
                return result.data;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, result.message);
            }
        }

        //GET LIST OF ALL COURSES OF THAT FACULTY
        public object GetTeachersCourse(string UID)
        {
            SqlCommand sqlcmd;
            string con = System.Configuration.ConfigurationManager.ConnectionStrings["CTUMSCONApp"].ConnectionString;
            SqlConnection conn = new SqlConnection(con);
            conn.Open();
            try
            {
                var result = new APIModel(UID).GetCourseListNew();

                if (result.status == true)
                {
                    var courselist = ((Models.Academics.TeachersCourseModel[])result.data);

                    for (int i = 0; i < courselist.Count(); i++)
                    {
                        var course = courselist[i];
                        var programcode1 = "select top (1) PCode from StudentSemesterRegistration where CurrentSection= '" + course.Section + "'";
                        sqlcmd = new SqlCommand(programcode1);
                        sqlcmd.Connection = conn;
                        sqlcmd.CommandType = CommandType.Text;
                        var programcode = sqlcmd.ExecuteScalar().ToString();
                        var cl = "select top(1) ProgramName from ProgramMaster where ProgramCode='"+ programcode +"'";
                        sqlcmd = new SqlCommand(cl);
                        sqlcmd.Connection = conn;
                        sqlcmd.CommandType = CommandType.Text;
                        courselist[i].ProgramName = sqlcmd.ExecuteScalar().ToString();
                    }

                    return courselist;
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result.message);
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
           
        }

        //POST ATTENDANCE
        [HttpPost]
        public object AddStudentAttendance(List<Models.Academics.AddAttendance> AddAttendanceList, string CourseCode, string Currdate, string Time, string AttendanceType, string StudentGroup, string Section, string UID)
        {
            var result = new APIModel(UID).PostAttendance(AddAttendanceList, CourseCode, Currdate,
                                                          Time, AttendanceType, StudentGroup, Section);

            if (result.status == true)
            {
                return result;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, result.message);
            }

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
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result.message);
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        //BIOMETRIC ATTENDANCE
        public object Get(string UniqueNo)
        {
            Consul.QueryResult qr = new Consul.QueryResult();

            string ConnectionString = WebConfigurationManager.ConnectionStrings["CTUMSCONApp"].ConnectionString;
            SqlConnection con = null;
            SqlDataReader sqlrdr = null;
            try
            {
                con = new SqlConnection(ConnectionString);

                con.Open();

                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = con;
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.CommandText = "[dbo].[pGetEmployeeAttendanceList]";
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;
                sqlcmd.Parameters.AddWithValue("@UniqueNo", UniqueNo);
                sqlcmd.Parameters.AddWithValue("@IsDisplayforDashboard", true);
                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new {
                        UniqueNumber = x["UniqueNumber"].ToString(),
                        Status = x["Status"].ToString(),
                        Remarks = x["Remarks"].ToString(),
                        Date = x["Date"].ToString(),
                        InTime = x["InTime"].ToString(),
                        OutTime = x["OutTime"].ToString(),

                    }).ToList();

                    return Response;
                }
                else
                {
                    throw new Exception("No records found!");
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (sqlrdr != null)
                {
                    sqlrdr.Close();
                }
                if (con != null)
                {
                    con.Close();
                }
            }
            return qr;
        }


        //**************************************************************************************************


        //LIBRARY
        //************************************************************************************************
        //SEE BOOKS ISSUED TO A USER (give book = true, just to solve ambiguity)
        public object GetBooksData(string UID, string book)
        {
            try
            {
                string ConnectionString = WebConfigurationManager.ConnectionStrings["CTUMSCONApp"].ConnectionString;
                SqlConnection con = null;
                SqlDataReader sqlrdr = null;
                try
                {
                    con = new SqlConnection(ConnectionString);

                    con.Open();

                    SqlCommand sqlcmd = new SqlCommand();
                    sqlcmd.Connection = con;
                    sqlcmd.CommandType = CommandType.StoredProcedure;

                    sqlcmd.CommandText = "[dbo].[pGetBooksData]";
                    DataTable dt = new DataTable();
                    SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                    sqldataadapter.SelectCommand = sqlcmd;
                    sqlcmd.Parameters.AddWithValue("@UID", UID);
                    sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    sqldataadapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        var Response = dt.AsEnumerable().Select(x => new {
                            Library = x["Library"].ToString(),
                            Book = x["Book"].ToString(),
                            Author = x["Author"].ToString(),
                            Returned = x["Returned"].ToString()
                        }).ToList();

                        return Response;
                    }
                    else
                    {
                        throw new Exception("No records found!");
                    }

                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
                finally
                {
                    if (sqlrdr != null)
                    {
                        sqlrdr.Close();
                    }
                    if (con != null)
                    {
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //*************************************************************************************************

        //*************************************************************************************************
        //TIMETABLE
        public object getTimeTable(string UID)
        {
            QueryResult qr = new QueryResult();

            string ConnectionString = WebConfigurationManager.ConnectionStrings["CTUMSCONApp"].ConnectionString;
            SqlConnection con = null;
            SqlDataReader sqlrdr = null;
            try
            {
                con = new SqlConnection(ConnectionString);

                con.Open();

                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = con;
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.CommandText = "[dbo].[pGetTimeTableApp]";
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;
                sqlcmd.Parameters.AddWithValue("@UniqueNo", UID);
                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {
                        CourseCode = x["CourseCode"].ToString(),
                        Time = x["Timeslot"].ToString(),
                        Course = x["coursetitle"].ToString(),
                        Room = x["Room"].ToString(),
                        Day = x["day"].ToString(),
                        TeacherUID = x["TeacherUId"].ToString(),
                        sectionAuthorizationId = x["sectionauthorizationid"].ToString(),
                        EmployeeName = x["EmployeeName"].ToString(),
                        Section = x["Section"].ToString(),
                        StudentGroup = x["studentgroup"].ToString(),
                        L = x["L"].ToString(),
                        T = x["T"].ToString(),
                        P = x["P"].ToString(),
                        Image = "null"
                    }).ToList();
                    qr.status = true;
                    qr.data = Response;
                }
                else
                {
                    throw new Exception("No records found!");
                }

            }
            catch (Exception ex)
            {
                qr.data = null;
                qr.status = false;
                qr.message = ex.Message;
            }
            finally
            {
                if (sqlrdr != null)
                {
                    sqlrdr.Close();
                }
                if (con != null)
                {
                    con.Close();
                }
            }
            return qr.data;
        }
        //*************************************************************************************************

        //*************************************************************************************************
        //TERMID
        public object get()
        {
            QueryResult qr = new QueryResult();

            string ConnectionString = WebConfigurationManager.ConnectionStrings["CTUMSCONApp"].ConnectionString;
            SqlConnection con = null;
            SqlDataReader sqlrdr = null;
            try
            {
                con = new SqlConnection(ConnectionString);

                con.Open();

                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = con;
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.CommandText = "[dbo].[pGetAllTermIDApp]";
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;
                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {

                        TermId = x["TermId"].ToString()

                    }).ToList();

                    qr.status = true;
                    qr.data = Response;
                }
                else
                {
                    throw new Exception("No records found!");
                }

            }
            catch (Exception ex)
            {
                qr.data = null;
                qr.status = false;
                qr.message = ex.Message;
            }
            finally
            {
                if (sqlrdr != null)
                {
                    sqlrdr.Close();
                }
                if (con != null)
                {
                    con.Close();
                }
            }
            return qr;
        }
        //*************************************************************************************************
    }
}
