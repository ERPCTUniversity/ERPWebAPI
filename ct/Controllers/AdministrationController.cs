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
    public class AdministrationController : ApiController
    {
        //Get STAFF ON LEAVE TODAY
        public QueryResult GetStaffOnLeave()
        {
            QueryResult result = new QueryResult();
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

                sqlcmd.CommandText = "[dbo].[pGetStaffOnLeaveApp]";

                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;

                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    var temp = dt.AsEnumerable();
                    var Response = dt.AsEnumerable().Select(x => new
                    {
                        AdjustmentTo = x["AdjustmentTo"].ToString(),
                        Designation = x["Department"].ToString(),
                        EmployeeName = x["EmployeeName"].ToString(),
                        UniqueNo = x["UniqueNo"].ToString(),
                        MobileNo = x["MobileNo"].ToString(),
                        EmailId = x["OfficialEmailId"].ToString()
                    }).ToList();
                    result.data = Response;
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
                if (con != null)
                {
                    con.Close();
                }
            }
            return result;
          
        }

        //*************************************************************************************
        //GATEPASS
        [System.Web.Http.HttpGet]
        // checking a User is Hosteller or Not
        public object CheckingSanctioningAuthority(string uniqueno)
        {
            //================procedures==============
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

                sqlcmd.CommandText = "[dbo].[pCheckSanctioningAuthority]";
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;
                sqlcmd.Parameters.AddWithValue("@UniqueNumber", uniqueno);
                sqlcmd.Parameters.AddWithValue("@Flag", '1');
                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {
                        CheckSanctioning = x["CheckSanctioning"].ToString(),
                    }).FirstOrDefault();

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
            //================procedures==============
        }


        [System.Web.Http.HttpGet]

        // getting all the gate pass lists of respective user

        public object GetPendingLeaves(string uniqueno, string flag)
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
                sqlcmd.CommandText = "[dbo].[pGetStudentPendingLeaves]";
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;

                sqlcmd.Parameters.AddWithValue("@UniqueNumber", uniqueno);
                sqlcmd.Parameters.AddWithValue("@flag", flag);
                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {
                        Id = x["Id"].ToString(),
                        RegistrationNumber = x["RegistrationNumber"].ToString(),
                        FromDate = x["FromDate"].ToString(),
                        ToDate = x["ToDate"].ToString(),
                        OutTime = x["OutTime"].ToString(),
                        InTime = x["InTime"].ToString(),
                        VisitTo = x["VisitTo"].ToString(),
                        Purpose = x["Purpose"].ToString(),
                        StudentName = x["StudentName"].ToString(),
                        FatherName = x["FatherName"].ToString(),
                        ParentsContact = x["ParentsContact"].ToString(),
                        Attachment = x["Attachment"].ToString(),
                        Status = x["Status"].ToString(),
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

        //Hostel User Detail

        [System.Web.Http.HttpGet]
        public object GetHosteUserDetail(string RegNo)
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

                sqlcmd.CommandText = "[dbo].[pGetStudentDetails]";
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;
                sqlcmd.Parameters.AddWithValue("@RegistrationNumber", RegNo);

                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {
                        RegistrationNo = x["RegistrationNo"].ToString(),
                        StudentName = x["StudentName"].ToString(),
                        UniversitySchool = x["UniversitySchool"].ToString(),
                        ProgramName = x["ProgramName"].ToString(),
                        Semester = x["Semester"].ToString(),
                        Batch = x["Batch"].ToString(),
                        Email = x["Email"].ToString(),
                        Mobile = x["Mobile"].ToString(),
                        AdmissionType = x["AdmissionType"].ToString(),
                        FatherName = x["FatherName"].ToString(),
                        MotherName = x["MotherName"].ToString(),
                        Gender = x["Gender"].ToString(),
                        DOB = x["DOB"].ToString(),
                        FatherPhoneNumber = x["FatherPhoneNumber"].ToString(),
                        AdhaarCardNumber = x["AdhaarCardNumber"].ToString(),
                        StudentCategory = x["StudentCategory"].ToString(),
                        Address = x["Address"].ToString(),
                        District = x["District"].ToString(),
                        State = x["State"].ToString(),
                        Pincode = x["Pincode"].ToString(),
                        RoomNo = x["RoomNo"].ToString(),
                        Block = x["Block"].ToString()
                    }).FirstOrDefault();

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


        [HttpGet]
        public static QueryResult GetStudentDetailforGP(string uid)
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

                sqlcmd.CommandText = "[dbo].[pGetStudentDetailsForGatepass]";
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;
                sqlcmd.Parameters.AddWithValue("@RegistrationNumber", uid);

                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {
                        RegistrationNo = x["RegistrationNo"].ToString(),
                        StudentName = x["StudentName"].ToString(),
                        UniversitySchool = x["UniversitySchool"].ToString(),
                        Hostel = x["Hostel"].ToString(),

                    }).FirstOrDefault();

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

        [HttpPost]
        public static QueryResult AddStudentDetailsForGatePass(Administration.StudentGatePassModel dto)

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
                sqlcmd.CommandText = "[dbo].[AddStudentDetailforGatePass]";
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;
                sqlcmd.Parameters.AddWithValue("@RegistrationNo", dto.RegistrationNo);
                sqlcmd.Parameters.AddWithValue("@StudentName", dto.StudentName);
                sqlcmd.Parameters.AddWithValue("@UniversitySchool", dto.UniversitySchool);
                sqlcmd.Parameters.AddWithValue("@Hostel", dto.Hostel);
                sqlcmd.Parameters.AddWithValue("@Date", dto.Date);
                sqlcmd.Parameters.AddWithValue("@FromTime", dto.FromTime);
                sqlcmd.Parameters.AddWithValue("@ToTime", dto.ToTime);
                sqlcmd.Parameters.AddWithValue("@ReasonForLeave", dto.ReasonForLeave);
                sqlcmd.Parameters.AddWithValue("@EntryBy", dto.RegistrationNo);


                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {
                        message = x["message"].ToString(),
                        status = x["status"].ToString(),
                    }).FirstOrDefault();
                    qr.status = Convert.ToBoolean(Response.status);
                    qr.message = Response.message;
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
        [HttpGet]
        public static QueryResult GetGatePassInformation(string uid)
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
                sqlcmd.CommandText = "[dbo].[pGetStudentGatePassDetail]";
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;
                sqlcmd.Parameters.AddWithValue("@RegistrationNo", uid);
                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                sqldataadapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {

                        RegistrationNo = x["RegistrationNo"].ToString(),
                        StudentName = x["StudentName"].ToString(),
                        UniversitySchool = x["UniversitySchool"].ToString(),
                        Hostel = x["Hostel"].ToString(),
                        Date = x["Date"].ToString(),
                        FromTime = x["FromTime"].ToString(),
                        ToTime = x["ToTime"].ToString(),
                        ReasonForLeave = x["ReasonForLeave"].ToString(),
                        IsApproved = x["IsApproved"].ToString()

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


        //*************************************************************************************

        //Get ADMISSION EMPLOYEES
        public QueryResult GetAdmissionEmployees()
        {
            QueryResult result = new QueryResult();
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

                sqlcmd.CommandText = "[dbo].[GetAdmissionEmployeesApp]";

                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;

                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    var temp = dt.AsEnumerable();
                    var Response = dt.AsEnumerable().Select(x => new
                    {
                        Id = x["Id"].ToString(),
                        EmployeeId = x["EmployeeId"].ToString(),
                        EmployeeName = x["EmployeeName"].ToString(),
                        Department = x["Department"].ToString(),
                        Designation = x["Designation"].ToString()

                    }).ToList();
                    result.data = Response;
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
                if (con != null)
                {
                    con.Close();
                }
            }
            return result;

        }

        // ANNOUNCEMENT CATEGORIES
        public QueryResult GetFiltersAnnouncements(string UIDs)
        {
            QueryResult result = new QueryResult();
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

                sqlcmd.CommandText = "[dbo].[GetAnnouncementCatApp]";

                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;
                sqlcmd.Parameters.AddWithValue("@UIDs", UIDs);

                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    var temp = dt.AsEnumerable();
                    var Response = dt.AsEnumerable().Select(x => new
                    {
                        Category = x["Category"].ToString()

                    }).ToList();
                    result.data = Response;
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
                if (con != null)
                {
                    con.Close();
                }
            }
            return result;

        }

        // CLUBS REGISTERED
        public QueryResult GetClubsRegistered(string RegistrationNo)
        {
            QueryResult result = new QueryResult();
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

                sqlcmd.CommandText = "[dbo].[pGetClubsForStudents]";

                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;
                sqlcmd.Parameters.AddWithValue("@RegistrationNo", RegistrationNo);

                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    var temp = dt.AsEnumerable();
                    var Response = dt.AsEnumerable().Select(x => new
                    {
                        RegistrationNumber = x["RegistrationNumber"].ToString(),
                        StudentName = x["StudentName"].ToString(),
                        Division = x["RegistrationNumber"].ToString(),
                        Clubs = x["Clubs"].ToString(),
                        DealingPerson = x["DealingPerson"].ToString()
                    }).ToList();
                    result.data = Response;
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
                if (con != null)
                {
                    con.Close();
                }
            }
            return result;

        }

        //POST DPR 
        [HttpPost]
        public HttpResponseMessage PostDPR([FromBody] Administration.EmployeeDPR ed)
        {
            try
            {
                //using (CTUmsEntitiesApp entities = new CTUmsEntitiesApp())
                //{
                //    entities.EmployeeDPRs.Add(ed);
                //    entities.SaveChanges();
                //}
                var res = new APIModel(ed.UniqueNo).AddDPR(ed);
                if (res.status)
                {
                    return Request.CreateResponse(HttpStatusCode.Created);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, res.message);
                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.ToString());
            }
        }

        //CHECK DPR
        public static QueryResult CheckBackDateDPR(string UID, string date)
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

                sqlcmd.CommandText = "[dbo].[pCheckDPR]";
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;
                sqlcmd.Parameters.AddWithValue("@UniqueNo", UID);
                sqlcmd.Parameters.AddWithValue("@date", date);


                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {
                        DPR = x["DPRDate"].ToString(),
                        Task = x["Task"].ToString(),
                        TaskDetails = x["TaskDetail"].ToString(),
                        TaskQuantity = x["TaskQuantity"].ToString(),
                       // Status = x["Status"].ToString(),
                        ManHour = x["ManhoursConsumed"].ToString()

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

        //****************************************************************************************
        //RMS

        public static QueryResult GetRMSCategoryList(string UID)
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

                sqlcmd.CommandText = "[dbo].[pGetRMSCategoryList]";
                sqlcmd.Parameters.AddWithValue("@UniqueNo", UID);
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;
                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {
                        Id = x["Id"].ToString(),
                        Category = x["Category"].ToString()

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

        //GET SUBCATEGORY AND DEALING PERSON BOTH
        public static QueryResult GetSubCategory(string Category, string UID)
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

                sqlcmd.CommandText = "[dbo].[pGetRMSSubCategoryList]";
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;
                sqlcmd.Parameters.AddWithValue("@Category", Category);
                sqlcmd.Parameters.AddWithValue("@uniqueno", UID);
                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {

                        SubcategoryId = x["id"].ToString(),
                        Subcategory = x["Subcategory"].ToString(),
                        DealingPerson = x["DealingPerson"].ToString(),
                        Name = x["Name"].ToString(),


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

        public static QueryResult GetBlockWiseFloorList(string Block)
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

                sqlcmd.CommandText = "[dbo].[pGetFloorsList]";
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;
                sqlcmd.Parameters.AddWithValue("@Block", Block);
                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {
                        FloorNo = x["FloorNo"].ToString()

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

        public static QueryResult GetRoomNo(string Block, string FloorNo)
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

                sqlcmd.CommandText = "[dbo].[pGetRoomNo]";
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;
                sqlcmd.Parameters.AddWithValue("@Block", Block);
                sqlcmd.Parameters.AddWithValue("@FloorNo", FloorNo);
                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {
                        RoomNo = x["RoomNo"].ToString()

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

        //POST
        public static QueryResult AddRMSQueries(Administration.RMSQueries dto)
        {

            QueryResult qr = new QueryResult();

            string ConnectionString = WebConfigurationManager.ConnectionStrings["CTUMSCON"].ConnectionString;
            SqlConnection con = null;
            SqlDataReader sqlrdr = null;
            try
            {
                con = new SqlConnection(ConnectionString);

                con.Open();

                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = con;
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.CommandText = "[dbo].[pAddRMSQueries]";
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;
                sqlcmd.Parameters.AddWithValue("@MessageType", dto.MessageType);
                sqlcmd.Parameters.AddWithValue("@Category", dto.Category);
                sqlcmd.Parameters.AddWithValue("@Subcategory", dto.Subcategory);
                sqlcmd.Parameters.AddWithValue("@Block", dto.Block);
                sqlcmd.Parameters.AddWithValue("@EntryBy", dto.UserId);
                sqlcmd.Parameters.AddWithValue("@RoomNo", dto.RoomNo);
                sqlcmd.Parameters.AddWithValue("@ContactNo", dto.ContactNo);
                sqlcmd.Parameters.AddWithValue("@DealingPerson", dto.DealingPerson);
                sqlcmd.Parameters.AddWithValue("@Description", dto.Description);
                sqlcmd.Parameters.AddWithValue("@FloorNo", dto.FloorNo);


                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {

                        message = x["message"].ToString(),
                        status = x["status"].ToString(),


                    }).FirstOrDefault();


                    qr.status = Convert.ToBoolean(Response.status);
                    qr.message = Response.message;
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

        //****************************************************************************************

        //****************************************************************************************
        //LEAVES

        //Get leave cat details Balance
        public static QueryResult GetCatLeavesBalance(string UIDcat)
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

                sqlcmd.CommandText = "[dbo].[pgetleavebalancefordashboard]";
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;
                sqlcmd.Parameters.AddWithValue("@UniqueNo", UIDcat);
                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {
                        Category = x["LeaveCategory"].ToString(),
                        Leaves = x["LeaveBalance"].ToString()

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

        //get total leaves taken
        public object gettotal(string UIDtotal)

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

                sqlcmd.CommandText = "[dbo].[pGetLeaveCountForDashboard]";
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;
                sqlcmd.Parameters.AddWithValue("@UniqueNo", UIDtotal);
                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {

                        LeavePeriod = x["LeavePeriod"].ToString(),
                        Count = x["Count"].ToString()
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

        //GET LIST OF ALL LEAVES TAKEN BY EMPLOYEE
        [Obsolete]
        public object getAllLeaves(string UID)

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

                sqlcmd.CommandText = "[dbo].[pGetEmployeeLeaveList]";
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;
                sqlcmd.Parameters.AddWithValue("@UniqueNumber", UID);
                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {
                        Category =x["LeaveCategory"],
                        Span = x["DaySpan"],
                        FromDate = x["FromDate"],
                        ToDate = x["ToDate"],
                        Status = x["Status"]

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

        //Get categories
        public static QueryResult GetLeaveCategory()
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

                var query = "select * from LeaveCategory";
                sqlcmd = new SqlCommand(query);
                sqlcmd.Connection = con;
                sqlcmd.CommandType = CommandType.Text;
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;

                sqldataadapter.SelectCommand.CommandType = CommandType.Text;
                sqldataadapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {

                        Id = x["Id"],
                        LeaveCategory = x["LeaveCategory"],
                        IsActive = x["IsActive"],
                        ParentCategory = x["ParentCategory"]

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

        //Get Employees for adjustment
        public static QueryResult GetAdjustmentEmployees()
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

                var query = "SELECT * FROM EmployeeMaster where EmployeeName is not null";
                sqlcmd = new SqlCommand(query);
                sqlcmd.Connection = con;
                sqlcmd.CommandType = CommandType.Text;
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;

                sqldataadapter.SelectCommand.CommandType = CommandType.Text;
                sqldataadapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {
                        Id = x["Id"],
                        OrganizationMasterId = Convert.ToInt32(x["OrganizationMasterId"]),
                        Title = x["Title"].ToString(),
                        UniqueNo = x["UniqueNo"].ToString(),
                        EmployeeName = x["EmployeeName"].ToString(),
                        MobileNo = x["MobileNo"].ToString(),
                        CareOff = x["CareOff"].ToString(),
                        FatherName = x["FatherName"].ToString(),
                        MotherName = x["MotherName"].ToString(),
                        EmployeeImage = x["EmployeeImage"].ToString(),
                        EmailId = x["EmailId"].ToString(),
                        Gender = x["Gender"].ToString(),
                        DateOfBirth = x["DateOfBirth"],
                        DateOfJoining = x["DateOfJoining"],
                        DateOfAnniversary = x["DateOfAnniversary"],
                        AddressId = x["AddressId"].ToString(),
                        DepartmentId = x["DepartmentId"].ToString(),
                        DesignationId = x["DesignationId"].ToString(),
                        MaritalStatus = x["MaritalStatus"].ToString(),
                        BloodGroup = x["BloodGroup"].ToString(),
                        OfficialEmailId = x["OfficialEmailId"].ToString(),
                        BankName = x["BankName"].ToString(),
                        BankIFC = x["BankIFC"].ToString(),
                        AccountNumber = x["AccountNumber"].ToString(),
                        PanNumber = x["PanNumber"].ToString(),
                        VoterID = x["VoterID"].ToString(),
                        AdharCardNo = x["AdharCardNo"].ToString(),
                        PassportNumber = x["PassportNumber"].ToString(),
                        PassportExpiryDate = x["PassportExpiryDate"],
                        PF = x["PF"].ToString(),
                        ESI = x["ESI"].ToString(),
                        EmergencyContactPerson = x["EmergencyContactPerson"].ToString(),
                        EmergencyContactNo = x["EmergencyContactNo"].ToString(),
                        EmergencyEmail = x["EmergencyEmail"].ToString(),
                        HighestQualification = x["HighestQualification"].ToString(),
                        HostelFacility = x["HostelFacility"],
                        HostelId = x["HostelId"],
                        AvailBusFacility = x["AvailBusFacility"],
                        BusNo = x["BusNo"].ToString(),
                        SecurityRequired = x["SecurityRequired"],
                        SecurityAmount = x["SecurityAmount"],
                        SecurityDeducted = x["SecurityDeducted"],
                        BalanceSecurity = x["BalanceSecurity"],
                        TeachingExperience = x["TeachingExperience"],
                        ResearchExperience = x["ResearchExperience"],
                        IndustryExperience = x["IndustryExperience"],
                        TotalExperience = x["TotalExperience"],
                        ComputerSkills = x["ComputerSkills"].ToString(),
                        EntryDate = x["EntryDate"],
                        EntryBy = x["EntryBy"].ToString(),
                        ModifyDate = x["ModifyDate"],
                        ModifyBy = x["ModifyBy"].ToString(),
                        WeekOff = x["WeekOff"].ToString(),
                        IsUpdated = x["IsUpdated"],
                        IsActive = x["IsActive"],
                        ThirdSaturday = x["ThirdSaturday"].ToString(),
                        StaffType = x["StaffType"].ToString(),
                        RelivingDate = x["RelivingDate"],
                        Graduation = x["Graduation"].ToString(),
                        PostGraduation = x["PostGraduation"].ToString(),
                        Doctorate = x["Doctorate"].ToString(),
                        SpecialisationInGraduation = x["SpecialisationInGraduation"].ToString(),
                        SpecialisationInPostGraduation = x["SpecialisationInPostGraduation"].ToString(),
                        SpecialisationInDoctorate = x["SpecialisationInDoctorate"].ToString(),
                        FutureCourse = x["FutureCourse"].ToString(),
                        PracticalSkill = x["PracticalSkill"].ToString(),
                        EmployeeLocation = x["EmployeeLocation"].ToString(),
                        CommitteeRoleId = x["CommitteeRoleId"].ToString(),
                        SpecializationArea = x["SpecializationArea"].ToString(),
                        Board10th = x["Board10th"].ToString(),
                        Percentage10th = x["Percentage10th"].ToString(),
                        Board12th = x["Board12th"].ToString(),
                        Percentage12th = x["Percentage12th"].ToString(),
                        DiplomaName = x["DiplomaName"].ToString(),
                        DiplomaYear = x["DiplomaYear"].ToString(),
                        DiplomaInstitution = x["DiplomaInstitution"].ToString(),
                        DiplomaPercentage = x["DiplomaPercentage"].ToString(),
                        YearGrad = x["YearGrad"].ToString(),
                        DiplomaSpecialisation = x["DiplomaSpecialisation"].ToString(),
                        GradInstitution = x["GradInstitution"].ToString(),
                        PyearGrad = x["PyearGrad"].ToString(),
                        PGradInstitution = x["PGradInstitution"].ToString(),
                        PGradPercentage = x["PGradPercentage"].ToString(),
                        DyearGrad = x["DyearGrad"].ToString(),
                        DGradInstitution = x["DGradInstitution"].ToString(),
                        AcademicDesignation = x["AcademicDesignation"].ToString(),
                        AdministrativeDesignation = x["AdministrativeDesignation"].ToString(),
                        IsDeclaration = x["IsDeclaration"],
                        IsLocked = x["IsLocked"],
                        CurrentDomainId = x["CurrentDomainId"],
                        PresentDomainId = x["PresentDomainId"]
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

        //Get Leave Session 

        public object GetLeaveSession(string uniqueno)
        {
            var result = new APIModel(uniqueno).GetLeavesSession();

            if (result.status == true)
            {
                return result.data;
            }
            else
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, result.message);

            }
        }

        //Post Leave
        [HttpPost]
        public object PostLeave([FromBody] Administration.Leave leave)
        {
            //var period = entities.LeavePeriodMasters.Where(i => i.IsActive == true).Select(i => i.Id).FirstOrDefault();
            QueryResult qr = new QueryResult();

            string ConnectionString = WebConfigurationManager.ConnectionStrings["CTUMSCONApp"].ConnectionString;
            SqlConnection con = null;
            SqlDataReader sqlrdr = null;

            var path = "";
            var attachment = "";
            try
            {
                if (leave.ImageBytes != null)
                {
                    var file_temp = leave.ImageBytes;
                    var filebytes = Convert.FromBase64String(file_temp);
                    var fileName = leave.FileName;


                    if (filebytes != null)
                    {

                        //string dir = "~/Attachments/Leave";
                        //String[] Filenamelist = fileName.Split('.');
                        //Filenamelist[0] = Filenamelist[0] + "_" + leave.UniqueNo;
                        //string FinalFileName = Filenamelist[0] + "." + Filenamelist[1];

                        //path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(dir), FinalFileName);
                        //File.WriteAllBytes(path, filebytes);
                        //attachment = FinalFileName;

                    }
                    else
                    {
                        throw new Exception("Only .xls,.xlsx ,.pdf, .txt , .csv , .png, .jpeg formats are allowed");
                    }

                }
                con = new SqlConnection(ConnectionString);
                con.Open();

                var PDL = "select * from LeavePeriodMaster where IsActive = 1";
                SqlCommand sqlcmd11 = new SqlCommand(PDL);
                sqlcmd11.Connection = con;
                sqlcmd11.CommandType = CommandType.Text;
                DataTable dt11 = new DataTable();
                SqlDataAdapter sqldataadapter11 = new SqlDataAdapter();
                sqldataadapter11.SelectCommand = sqlcmd11;

                sqldataadapter11.SelectCommand.CommandType = CommandType.Text;
                sqldataadapter11.Fill(dt11);

                if (dt11.Rows.Count > 0)
                {
                    var period = dt11.AsEnumerable().Select(x => new
                    {
                        Id = x["Id"]

                    }).FirstOrDefault().Id;

                    SqlCommand sqlcmd = new SqlCommand();
                    sqlcmd.Connection = con;
                    sqlcmd.CommandType = CommandType.StoredProcedure;

                    sqlcmd.CommandText = "[dbo].[pEmployeeLeaveApplication]";
                    DataTable dt = new DataTable();
                    SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                    sqldataadapter.SelectCommand = sqlcmd;
                    sqlcmd.Parameters.AddWithValue("@UniqueNo", leave.UniqueNo);
                    sqlcmd.Parameters.AddWithValue("@LeavePeriod", period);
                    sqlcmd.Parameters.AddWithValue("@LeaveCategory", leave.LeaveCategory);
                    sqlcmd.Parameters.AddWithValue("@DaySpan", leave.DaySpan);
                    sqlcmd.Parameters.AddWithValue("@EntryBy", leave.UniqueNo);
                    sqlcmd.Parameters.AddWithValue("@LeaveBalance", leave.LeaveBalance);
                    sqlcmd.Parameters.AddWithValue("@LeaveBalanceLeft", leave.LeaveBalanceLeft);
                    sqlcmd.Parameters.AddWithValue("@AdjustmentTo", leave.AdjustmentTo);

                    if (leave.HalfLeaveDate == null && leave.ShortLeaveDate == null)
                        sqlcmd.Parameters.AddWithValue("@StartDate", leave.StartDate);
                    if ((leave.LastDate == null))
                    {
                        sqlcmd.Parameters.AddWithValue("@LastDate", DBNull.Value);
                    }
                    else
                    {
                        sqlcmd.Parameters.AddWithValue("@LastDate", leave.LastDate);
                    }

                    //sqlcmd.Parameters.AddWithValue("@LastDate", leave.LastDate);
                    sqlcmd.Parameters.AddWithValue("@HalfDaySpan", leave.HalfDaySpan);
                    if (leave.ShortLeaveDate == null && leave.StartDate == null)
                        sqlcmd.Parameters.AddWithValue("@StartDate", leave.HalfLeaveDate);
                    if (leave.HalfLeaveDate == null && leave.StartDate == null)
                        sqlcmd.Parameters.AddWithValue("@StartDate", leave.ShortLeaveDate);
                    sqlcmd.Parameters.AddWithValue("@ShortLeaveTimeSlot", leave.ShortLeaveTimeSlot);
                    sqlcmd.Parameters.AddWithValue("@LeaveReason", leave.LeaveReason);
                    sqlcmd.Parameters.AddWithValue("@LeaveAddress", leave.LeaveAddress);
                    sqlcmd.Parameters.AddWithValue("@NoOfDays", leave.NoOfDays);
                    sqlcmd.Parameters.AddWithValue("@IsPDL", leave.IsPDL);
                    sqlcmd.Parameters.AddWithValue("@Attachment", attachment);
                    sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    sqldataadapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        var Response = dt.AsEnumerable().Select(x => new
                        {

                            message = x["message"].ToString(),
                            status = x["status"].ToString(),


                        }).FirstOrDefault();


                        qr.status = Convert.ToBoolean(Response.status);
                        qr.message = Response.message;
                    }
                    else
                    {
                        throw new Exception("No records found!");
                    }

                    //Update PDL data
                    con.Open();
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandText = "[dbo].[PDLApp]";
                    DataTable dt1 = new DataTable();
                    SqlDataAdapter sqldataadapter1 = new SqlDataAdapter();
                    sqldataadapter1.SelectCommand = sqlcmd;
                    sqlcmd.Parameters.AddWithValue("@id", leave.PdlId);
                    sqldataadapter1.SelectCommand.CommandType = CommandType.StoredProcedure;
                    sqldataadapter1.Fill(dt1);

                    sqldataadapter1.SelectCommand.CommandType = CommandType.StoredProcedure;
                    sqldataadapter1.Fill(dt1);
                    if (dt1.Rows.Count > 0)
                    {
                        var Response = dt1.AsEnumerable().Select(x => new
                        {
                            message = x["message"].ToString(),
                            status = x["status"].ToString(),
                        }).FirstOrDefault();
                        qr.status = Convert.ToBoolean(Response.status);
                        qr.message = Response.message;
                    }
                    else
                    {
                        throw new Exception("No records found!");
                    }

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
            if (qr.status == true)
            {
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, qr.message);
            }

        }


        //****************************************************************************************
        //LIVE EVENT DATA
        public static QueryResult GetLiveEvent()
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

                var query = "select * from LiveEvent";
                sqlcmd = new SqlCommand(query);
                sqlcmd.Connection = con;
                sqlcmd.CommandType = CommandType.Text;
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;

                sqldataadapter.SelectCommand.CommandType = CommandType.Text;
                sqldataadapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {

                        Id = x["Id"],
                        Link = x["Link"],
                        IsActive = x["IsActive"]
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

        // PHONE DIRECTORY
        public static QueryResult GetPhoneDirectory()
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

                var query = "select * from PhoneDirectory";
                sqlcmd = new SqlCommand(query);
                sqlcmd.Connection = con;
                sqlcmd.CommandType = CommandType.Text;
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;

                sqldataadapter.SelectCommand.CommandType = CommandType.Text;
                sqldataadapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {

                        ID = x["ID"],
                        Num = x["Num"],
                        Designation = x["Designation"]
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
        //PDL
        public static QueryResult getPDL(string UID)
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

                var query = "select * from PostDateLeaveMaster where UniqueNo = '"+UID+"'";
                sqlcmd = new SqlCommand(query);
                sqlcmd.Connection = con;
                sqlcmd.CommandType = CommandType.Text;
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;

                sqldataadapter.SelectCommand.CommandType = CommandType.Text;
                sqldataadapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {

                        id = x["id"],
                        UniqueNo = x["UniqueNo"],
                        EntryDate = x["EntryDate"],
                        IsActive = x["IsActive"],
                        TermId = x["TermId"],
                        ApplyDate = x["ApplyDate"]
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

        //GET REMAINING PDL
        public object GetRemainingPDLCount(string UID)
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
                sqlcmd.CommandText = "[dbo].[getPDLCountApp]";
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
                        PDLCount = x["PDLCount"].ToString()
                    }).FirstOrDefault();
                    qr.status = true;
                    qr.data = Response;
                }

                return qr;
            }


            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

        }
        //*************************************************************************************************

    }
}
