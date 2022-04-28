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
    public class TransactionController : ApiController
    {
        //Get Transactions
        public QueryResult GetTransactions(string UID)
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
                sqlcmd.CommandText = "[dbo].[GetTransactionsApp]";
                DataTable dt = new DataTable();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter();
                sqldataadapter.SelectCommand = sqlcmd;
                sqlcmd.Parameters.AddWithValue("@UserID", UID);
                sqldataadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    var Response = dt.AsEnumerable().Select(x => new
                    {
                        TransactionID = x["TransactionID"].ToString(),
                        UserID = x["UserID"].ToString(),
                        CreditBalance = x["CreditBalance"].ToString(),
                        DeptOrSchool = x["DeptOrSchool"].ToString(),
                        EntryBy = x["EntryBy"].ToString(),
                        EntryOn = x["EntryOn"].ToString(),
                        Message = x["Message"].ToString(),
                        Title = x["Title"].ToString(),

                    }).ToList();

                    qr.status = true;
                    qr.data = Response;
                }
                else
                {
                    qr.status = false;
                    qr.message = "No Data Available";
                }

                return qr;

            }

            catch (Exception ex)
            {
                throw new Exception("No records found!");
            }

        }

        //Get Leaderboard
        public QueryResult GetLeaderBoard(string UIDs)
        {
            QueryResult result = new QueryResult();
            //------------------
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

                sqlcmd.CommandText = "[dbo].[pGetTransactions]";

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
                        Balance = x["Transactions"].ToString(),
                        UserId = x["UserId"].ToString(),
                        UserName = x["Name"].ToString()
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
            //------------------         

            //============SchoolName wise=======

            SqlConnection con1 = null;
            SqlDataReader sqlrdr1 = null;
            try
            {
                con1 = new SqlConnection(ConnectionString);

                con1.Open();

                SqlCommand sqlcmd1 = new SqlCommand();
                sqlcmd1.Connection = con1;
                sqlcmd1.CommandType = CommandType.StoredProcedure;

                sqlcmd1.CommandText = "[dbo].[pGetTransactionsSchoolWise]";

                DataTable dt1 = new DataTable();
                SqlDataAdapter sqldataadapter1 = new SqlDataAdapter();
                sqldataadapter1.SelectCommand = sqlcmd1;
                sqlcmd1.Parameters.AddWithValue("@UID", UIDs);
                sqldataadapter1.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldataadapter1.Fill(dt1);

                if (dt1.Rows.Count > 0)
                {
                    var Response = dt1.AsEnumerable().Select(x => new
                    {
                        Balance = x["Transactions"].ToString(),
                        UserId = x["UserId"].ToString(),
                        UserName = x["Name"].ToString()
                    }).ToList();
                    result.data1 = Response;
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
            //============SchoolName wise=======
            return result;
        }
    }
}
