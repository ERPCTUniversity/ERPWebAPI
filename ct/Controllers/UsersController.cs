using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Configuration;
using System.Web.Http;
using ct.Models;

namespace ct.Controllers
{
    public class UsersController : ApiController
    {
        //GET USERS
        public object Get()
        {
            string userid = Thread.CurrentPrincipal.Identity.Name;
            ct.Models.QueryResult qr = new ct.Models.QueryResult();

            string ConnectionString = WebConfigurationManager.ConnectionStrings["CTUMSCONApp"].ConnectionString;
            SqlConnection con = null;
            SqlDataReader sqlrdr = null;
            con = new SqlConnection(ConnectionString);

            con.Open();
            SqlCommand sqlcmd;

            var emp1 = "select count(*) from EmployeeMaster where UniqueNo = '" + userid + "'";
            sqlcmd = new SqlCommand(emp1);
            sqlcmd.Connection = con;
            sqlcmd.CommandType = CommandType.Text;
            var count = Convert.ToInt32(sqlcmd.ExecuteScalar());

            var result = 0;
                if (count == 1)
                {

                var res1 = "select count(*) from EmployeeMaster where UniqueNo = '" + userid + "' and Category like 'Teaching'";
                sqlcmd = new SqlCommand(res1);
                sqlcmd.Connection = con;
                sqlcmd.CommandType = CommandType.Text;
                var result1 = Convert.ToInt32(sqlcmd.ExecuteScalar());
                
                    if (result1 == 1) //TEACHING STAFF
                    {
                        result = 1;
                    }
                    else if (result1 == 0) //NON-TEACHING STAFF
                    {
                        result = 2;
                    }
                }


            //To log out old sessions and store new

                string sessionID = Guid.NewGuid().ToString() + Guid.NewGuid().ToString();

                con = new SqlConnection(ConnectionString);
                SqlCommand sqlcmd1 = new SqlCommand();
                sqlcmd1.Connection = con;
                sqlcmd1.CommandType = CommandType.StoredProcedure;
                sqlcmd1.CommandText = "[dbo].[LogOutLogInApp]";
                DataTable dt1 = new DataTable();
                SqlDataAdapter sqldataadapter1 = new SqlDataAdapter();
                sqldataadapter1.SelectCommand = sqlcmd1;
                sqlcmd1.Parameters.AddWithValue("@UID", userid);
                sqlcmd1.Parameters.AddWithValue("@SessionID", sessionID);
                sqldataadapter1.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter1.Fill(dt1);
               
            UserAuthDetailModel userAuthDetailModel = new UserAuthDetailModel
            {
                Id = result,
                SessionID = sessionID
            };

            return userAuthDetailModel;
        }
    }
}
