using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Configuration;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using ct.Models;

namespace ct.Controllers
{
    public class UserDetailsController : ApiController
    {
        //To get User's Name, Picture, and current Domain
        public object get(string UID)
        {
            QueryResult qr = new QueryResult();

            string ConnectionString = WebConfigurationManager.ConnectionStrings["CTUMSCONApp"].ConnectionString;
            SqlConnection con = null;
            SqlDataReader sqlrdr = null;
            try
            {
                con = new SqlConnection(ConnectionString);
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = con;

                con.Open();
                var emp = "select count(*) from EmployeeMaster where UniqueNo = '" + UID + "'";
                sqlcmd = new SqlCommand(emp);
                sqlcmd.Connection = con;
                sqlcmd.CommandType = CommandType.Text;
                var count = Convert.ToInt32(sqlcmd.ExecuteScalar());


                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "[dbo].[UserDetailsApp]";
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;
                sqlcmd.Parameters.AddWithValue("@UID", UID);
                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    if (count == 1)
                    {


                        var Response = dt.AsEnumerable().Select(x => new
                        {
                            UniqueNo = x["UniqueNo"].ToString(),
                            EmployeeName = x["EmployeeName"].ToString(),
                            EmployeeImage = x["EmployeeImage"].ToString(),
                            ParentDomain = x["ParentDomain"].ToString(),
                            CurrentDomain = x["CurrentDomain"].ToString(),
                            Designation = x["Designation"].ToString(),

                        }).ToList();

                        qr.status = true;
                        qr.data = Response;

                    }
                    else

                    {
                        var Response = dt.AsEnumerable().Select(x => new
                        {

                            RegistrationNo = x["RegistrationNo"].ToString(),
                            StudentName = x["StudentName"].ToString(),
                            StudentImage = x["StudentImage"].ToString(),
                            ProgramName = x["ProgramName"].ToString(),
                            Semester = x["Semester"].ToString()
                        }).ToList();

                        qr.status = true;
                        qr.data = Response;
                    }
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

        //add user log in
        public object AddUserLogData(string uid)
        {
            QueryResult qr = new QueryResult();

            string ConnectionString = WebConfigurationManager.ConnectionStrings["CTUMSCONApp"].ConnectionString;
            SqlConnection con = null;
            SqlDataReader sqlrdr = null;
            try
            {
                con = new SqlConnection(ConnectionString);
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = con;
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "[dbo].[AddUserLogApp]";
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;
                sqlcmd.Parameters.AddWithValue("@UID", uid);
                sqlcmd.Parameters.AddWithValue("@Controller", "0");
                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {
                        message = x["message"].ToString(),
                        status = x["status"].ToString()
                    }).FirstOrDefault();
                    qr.status = Convert.ToBoolean(Response.status);
                    qr.message = Response.message;
                }
                return qr;
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

        }

        //Get UserFeedback

        public object AddUserFeedBack(string uid, string data)
        {
            QueryResult qr = new QueryResult();

            string ConnectionString = WebConfigurationManager.ConnectionStrings["CTUMSCONApp"].ConnectionString;
            SqlConnection con = null;
            SqlDataReader sqlrdr = null;
            try
            {
                con = new SqlConnection(ConnectionString);
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = con;
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "[dbo].[AddUserFeedbackApp]";
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;
                sqlcmd.Parameters.AddWithValue("@UID", uid);
                sqlcmd.Parameters.AddWithValue("@Description", data);
                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {
                        message = x["message"].ToString(),
                        status = x["status"].ToString()
                    }).FirstOrDefault();
                    qr.status = Convert.ToBoolean(Response.status);
                    qr.message = Response.message;
                }

                return qr;
            }


            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        //Adding user log out session details
        public object GetEdit(string UID, string sessionID, string date)
        {
            QueryResult qr = new QueryResult();
            sessionID = Encoding.UTF8.GetString(Convert.FromBase64String(sessionID));
            string ConnectionString = WebConfigurationManager.ConnectionStrings["CTUMSCONApp"].ConnectionString;
            SqlConnection con = null;
            SqlDataReader sqlrdr = null;
            try
            {
                con = new SqlConnection(ConnectionString);
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = con;
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "[dbo].[AddUserLogOutApp]";
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;
                sqlcmd.Parameters.AddWithValue("@SessionID", sessionID);
                sqlcmd.Parameters.AddWithValue("@IsActive", false);
                sqlcmd.Parameters.AddWithValue("@Session", null);
                sqlcmd.Parameters.AddWithValue("@Token", null);
                sqlcmd.Parameters.AddWithValue("@LastLoggedOut", date);
                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {
                        message = x["message"].ToString(),
                        status = x["status"].ToString()
                    }).FirstOrDefault();
                    qr.status = Convert.ToBoolean(Response.status);
                    qr.message = Response.message;
                }

                return qr;
            }


            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

        }

        //Adding user logggin session details
        public object Get(string UID, string sessionID, string token, string appVersion = "1.0")
        {
            QueryResult qr = new QueryResult();

            string ConnectionString = WebConfigurationManager.ConnectionStrings["CTUMSCONApp"].ConnectionString;
            SqlConnection con = null;
            SqlDataReader sqlrdr = null;

            sessionID = Encoding.UTF8.GetString(Convert.FromBase64String(sessionID));
            token = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            try
            {
                con = new SqlConnection(ConnectionString);
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = con;
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "[dbo].[AddUserLogInApp]";
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;
                sqlcmd.Parameters.AddWithValue("@SessionID", sessionID);
                sqlcmd.Parameters.AddWithValue("@AppVersion", appVersion);
                sqlcmd.Parameters.AddWithValue("@Token", token);
                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {
                        message = x["message"].ToString(),
                        status = x["status"].ToString()
                    }).FirstOrDefault();
                    qr.status = Convert.ToBoolean(Response.status);
                    qr.message = Response.message;
                }

                return qr;

            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.ToString());
            }






        }
    }
}
