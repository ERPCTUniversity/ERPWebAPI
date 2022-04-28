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

namespace ct.Controllers
{
    public class DashboardController : ApiController
    {
        public IHttpActionResult getAll(string UID)
        {
            SqlCommand sqlcmd;
            string con = System.Configuration.ConfigurationManager.ConnectionStrings["CTUMSCONApp"].ConnectionString;
            SqlConnection conn = new SqlConnection(con);
            conn.Open();
            string anncount, leavecount, taskcount, noticount, msgcount;
            var ann = "select count(*) from AnnouncementMaster where EntryDate >= cast(CONVERT(date, getdate())as varchar(20))+' 00:00:00' and EntryDate <=  cast(CONVERT(date, getdate())as varchar(20))+' 23:59:59' and OrganizationId= 1";
            sqlcmd = new SqlCommand(ann);
            sqlcmd.Connection = conn;
            sqlcmd.CommandType = CommandType.Text;
            anncount = sqlcmd.ExecuteScalar().ToString();

            var leave = "select count(*) FROM EmployeeLeaveApplication LRM LEFT JOIN EmployeeMaster EM on EM.UniqueNo COLLATE SQL_Latin1_General_CP1_CI_AS = LRM.UniqueNo LEFT JOIN DepartmentMaster DM on EM.DepartmentId = DM.Id left join LeaveCategory LC on LRM.LeaveCategory = LC.Id Left join LeaveSanctioningAuthorityMaster LSAM on EM.UniqueNo = LSAM.UniqueNo LEFT join EmployeeMaster EMM ON EMM.UniqueNo ='" + UID + "' LEFT JOIN DepartmentMaster DMM ON EMM.DepartmentId = DMM.Id where((LSAM.RecommendingAuthority = ' + UID + ' and LRM.IsApproved = 0)  or(LSAM.SanctioningAuthority = '" + UID + "' and LRM.IsApproved = 1 and IsSanctioned = 0)) AND status = 'Pending'";
            sqlcmd = new SqlCommand(leave);
            sqlcmd.Connection = conn;
            sqlcmd.CommandType = CommandType.Text;
            leavecount = sqlcmd.ExecuteScalar().ToString();

            var task = "select count(*) from TaskDetail TD where TD.SendTo = '" + UID + "' and TD.Status = 'Pending'";
            sqlcmd = new SqlCommand(task);
            sqlcmd.Connection = conn;
            sqlcmd.CommandType = CommandType.Text;
            taskcount = sqlcmd.ExecuteScalar().ToString();

            var not = "SELECT count(*) FROM dbo.NotificationsMaster nm WHERE nm.entrydate>= cast(CONVERT(date, getdate())as varchar(20))+' 00:00:00' and nm.EntryDate <=  cast(CONVERT(date, getdate())as varchar(20))+' 23:59:59' and OrganizationId= 1";
            sqlcmd = new SqlCommand(not);
            sqlcmd.Connection = conn;
            sqlcmd.CommandType = CommandType.Text;
            noticount = sqlcmd.ExecuteScalar().ToString();

            var msg = "SELECT count(*) FROM dbo.MessageMaster mm WHERE mm.SendTo='" + UID + "' AND mm.IsRead=0";
            sqlcmd = new SqlCommand(msg);
            sqlcmd.Connection = conn;
            sqlcmd.CommandType = CommandType.Text;
            msgcount = sqlcmd.ExecuteScalar().ToString();

            return Ok(new { Announcements = anncount, Leaves = leavecount, PendingTasks = taskcount, Notifications = noticount, Messages = msgcount });

        }

        //MESSAGES 
        public object getMessages(string UID)
        {
            ct.Models.QueryResult qr = new ct.Models.QueryResult();

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
                sqlcmd.CommandText = "[dbo].[pGetMessagesForDashBoard]";
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;
                sqlcmd.Parameters.AddWithValue("@UniqueNo", UID);
                sqlcmd.Parameters.AddWithValue("@IsDisplayForDashboard", 0); ;
                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {
                        Description = x["Description"].ToString(),
                        EntryBy = x["SendBy"].ToString(),
                        EntryDate = x["EntryDate"].ToString()
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

        //NOTIFICATIONS
        public object getNotifications(string UID)
        {
            ct.Models.QueryResult qr = new ct.Models.QueryResult();

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
                sqlcmd.CommandText = "[dbo].[pGetNotificationsList]";
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;
                sqlcmd.Parameters.AddWithValue("@UniqueNumber", UID);
                sqlcmd.Parameters.AddWithValue("@IsDisplayForDashboard", 1); ;
                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {
                        Description = x["Notification"].ToString(),
                        FromDate = x["FromDate"].ToString(),
                        ToDate = x["ToDate"].ToString(),
                        EntryBy = x["NotificationBy"].ToString(),
                        EntryDate = x["EntryDate"].ToString()
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

        //PROFILE
        public object getProfile(string UID) {
           QueryResult qr = new QueryResult();

            string ConnectionString = WebConfigurationManager.ConnectionStrings["CTUMSCONApp"].ConnectionString;
            SqlConnection con = null;
            SqlDataReader sqlrdr = null;
            try
            {
                con = new SqlConnection(ConnectionString);

                con.Open();
                SqlCommand sqlcmd;

                var emp1 = "select count(*) from EmployeeMaster where UniqueNo = '" + UID + "'";
                sqlcmd = new SqlCommand(emp1);
                sqlcmd.Connection = con;
                sqlcmd.CommandType = CommandType.Text;
                var count = Convert.ToInt32(sqlcmd.ExecuteScalar());

                var std1 = "select count(*) from StudentDetail where RegistrationNo = '" + UID + "'";
                sqlcmd = new SqlCommand(std1);
                sqlcmd.Connection = con;
                sqlcmd.CommandType = CommandType.Text;
                var count1 = Convert.ToInt32(sqlcmd.ExecuteScalar());
                if (count == 1)
                {
                    var employee = "select * from EmployeeMaster where UniqueNo = '" + UID + "'";
                    sqlcmd = new SqlCommand(employee);
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
                            Id =  x["Id"],
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
                            HostelFacility =  x["HostelFacility"],
                            HostelId =  x["HostelId"],
                            AvailBusFacility =  x["AvailBusFacility"],
                            BusNo = x["BusNo"].ToString(),
                            SecurityRequired =  x["SecurityRequired"],
                            SecurityAmount =  x["SecurityAmount"],
                            SecurityDeducted =  x["SecurityDeducted"],
                            BalanceSecurity =  x["BalanceSecurity"],
                            TeachingExperience =  x["TeachingExperience"],
                            ResearchExperience =  x["ResearchExperience"],
                            IndustryExperience =  x["IndustryExperience"],
                            TotalExperience =  x["TotalExperience"],
                            ComputerSkills = x["ComputerSkills"].ToString(),
                            EntryDate = x["EntryDate"],
                            EntryBy = x["EntryBy"].ToString(),
                            ModifyDate = x["ModifyDate"],
                            ModifyBy = x["ModifyBy"].ToString(),
                            WeekOff = x["WeekOff"].ToString(),
                            IsUpdated =  x["IsUpdated"],
                            IsActive =  x["IsActive"],
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
                            IsDeclaration =  x["IsDeclaration"],
                            IsLocked =  x["IsLocked"],
                            CurrentDomainId =  x["CurrentDomainId"],
                            PresentDomainId =  x["PresentDomainId"]
                        }).ToList();

                        qr.status = true;
                        qr.data = Response;

                    }
                    else
                    {
                        throw new Exception("No records found!");
                    }

                } else if (count1 == 1)

                {
                    var student = "select * from StudentDetail where RegistrationNo = '" + UID + "'";
                    sqlcmd = new SqlCommand(student);
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
                            id =  x["id"],
                            OrganizationMasterId =  x["OrganizationMasterId"],
                            AcadmicProgrameMasterId =  x["AcadmicProgrameMasterId"],
                            ParentSection = x["ParentSection"].ToString(),
                            Batch = x["Batch"].ToString(),
                            UniversitySchool = x["UniversitySchool"].ToString(),
                            AdmissionType = x["AdmissionType"].ToString(),
                            RegistrationNo = x["RegistrationNo"].ToString(),
                            ProgramName = x["ProgramName"].ToString(),
                            StudentName = x["StudentName"].ToString(),
                            FatherName = x["FatherName"].ToString(),
                            MotherName = x["MotherName"].ToString(),
                            Gender = x["Gender"].ToString(),
                            DOB = x["DOB"],
                            Mobile = x["Mobile"].ToString(),
                            FatherPhoneNumber = x["FatherPhoneNumber"].ToString(),
                            Email = x["Email"].ToString(),
                            AdhaarCardNumber = x["AdhaarCardNumber"].ToString(),
                            StudentCategory = x["StudentCategory"].ToString(),
                            Address = x["Address"].ToString(),
                            District = x["District"].ToString(),
                            State = x["State"].ToString(),
                            StateType = x["StateType"].ToString(),
                            Reported = x["Reported"].ToString(),
                            EntryBy = x["EntryBy"].ToString(),
                            ModifyBy = x["ModifyBy"].ToString(),
                            DeActivateBy = x["DeActivateBy"].ToString(),
                            CTASStudentAmbassador = x["CTASStudentAmbassador"].ToString(),
                            StudentImage = x["StudentImage"].ToString(),
                            PassportNumber = x["PassportNumber"].ToString(),
                            StudentStatus = x["StudentStatus"].ToString(),
                            StudentOfficialEmailAddress = x["StudentOfficialEmailAddress"].ToString(),
                            RegisteredBy = x["RegisteredBy"].ToString(),
                            Remarks = x["Remarks"].ToString(),
                            IsAlumni =  x["IsAlumni"],
                            IsTransportAvail =  x["IsTransportAvail"],
                            IsRegistered =  x["IsRegistered"],
                            RegisteredOn = x["RegisteredOn"],
                            DeactivationDate = x["DeactivationDate"],
                            PMSStatusDate = x["PMSStatusDate"],
                            IsPMSStudent =  x["IsPMSStudent"],
                            IsStudentAmbassador =  x["IsStudentAmbassador"],
                            LocationId = x["LocationId"],
                            DeActivateOn =x["DeActivateOn"],
                            IsActive =  x["IsActive"],
                            ModifyOn = x["ModifyOn"],
                            EntryOn = x["EntryOn"],
                            Pincode =  x["Pincode"],
                            Semester =  x["Semester"]

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

        //RESET COUNTS
        public object get(string UID, string attribute)
        {
            QueryResult qr = new QueryResult();

            string ConnectionString = WebConfigurationManager.ConnectionStrings["CTUMSCONApp"].ConnectionString;
            SqlConnection con = null;
            SqlDataReader sqlrdr = null;
            try
            {
                //string anncount, leavecount, taskcount, noticount, msgcount;
                if (attribute == "Messages")
                {
                    try
                    {
                        con = new SqlConnection(ConnectionString);

                        con.Open();

                        SqlCommand sqlcmd = new SqlCommand();
                        sqlcmd.Connection = con;
                        sqlcmd.CommandType = CommandType.StoredProcedure;

                        sqlcmd.CommandText = "[dbo].[pAddAMessageReadStatusApp]";
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
                                status = x["status"].ToString()

                            }).FirstOrDefault();


                            qr.status = Convert.ToBoolean(Response.status);

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


                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }


            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return qr;
            //return Request.CreateResponse(HttpStatusCode.Created);
        }

        //TASKS 
        public object getTasks(string UID)
        {
            ct.Models.QueryResult qr = new ct.Models.QueryResult();

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
                sqlcmd.CommandText = "[dbo].[pGetTasksAllApp]";
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
                        TaskNo = x["TaskNo"].ToString(),
                        Task = x["Task"].ToString(),
                        TaskType = x["TaskType"].ToString(),
                        Status = x["Status"].ToString(),
                        SendBy = x["SendBy"].ToString(),
                        FromDate = x["FromDate"].ToString()
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

        //ISACTIVESS
        public static QueryResult GetIsActive()
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

                var query = "select * from AppIsActive";
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
                       
                        ann = x["Announcement"],
                        noti = x["Notifications"],
                        mess = x["MyMessage"],
                        timetable = x["TimeTable"],
                        studatt = x["StudentAttendance"],
                        books = x["Books"],
                        dpr = x["DPR"],
                        hr = x["HR"],
                        tasks = x["Tasks"],
                        rms = x["RMS"],
                        markatt = x["MarkAttendence"],
                        userdetail = x["UserDetail"],
                        credits = x["Credits"],
                        profile = x["MyProfile"],
                        Isserver = x["IsServer"],
                        minappversion = x["MinVersion"]

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

    }
}


