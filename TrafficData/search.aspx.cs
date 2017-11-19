using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TrafficData
{
    public partial class search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string a = Request.Params["location"];
            string minYear = Request.Params["minYear"];
            string maxYear = Request.Params["maxYear"];
            bool usable = ErrorCheck(a, minYear, maxYear);

            if (usable)
            {
                JObject jsonObject = new JObject();
                SqlConnection myConnection = new SqlConnection("Data Source=MICHAELPC;Initial Catalog=qldCrashes;Integrated Security=True");
                try
                {
                    myConnection.Open();
                    SqlCommand myCommand = SetupQuery(a, minYear, maxYear, myConnection);
                    ExectueQuery(jsonObject, myCommand);
                    myConnection.Close();
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.ToString());
                }

                SendResponse(jsonObject);
            }

        }

        private static bool ErrorCheck(string postCode, string minYear, string maxYear)
        {
            int ignored = 0;
            bool usable = int.TryParse(postCode, out ignored);
            usable = int.TryParse(minYear, out ignored);
            usable = int.TryParse(maxYear, out ignored);
            return usable;
        }

        private void SendResponse(JObject jsonObject)
        {
            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";
            Response.Write(jsonObject.ToString());
            Response.End();
        }

        private static void ExectueQuery(JObject jsonObject, SqlCommand myCommand)
        {
            SqlDataReader myReader = myCommand.ExecuteReader();
            List<CrashData> data = new List<CrashData>();
            while (myReader.Read())
            {
                data.Add(new CrashData
                {
                    Lat = (double)myReader["Crash_Latitude_GDA94"],
                    Lng = (double)myReader["Crash_Longitude_GDA94"]
                });
            }
            jsonObject.Add("data", JToken.FromObject(data));
        }

        private static SqlCommand SetupQuery(string a, string minYear, string maxYear, SqlConnection myConnection)
        {
            SqlParameter myParam = new SqlParameter("@Param1", SqlDbType.VarChar);
            myParam.Value = a;
            SqlParameter MinYear = new SqlParameter("@MinYear", SqlDbType.Int);
            MinYear.Value = int.Parse(minYear);
            SqlParameter MaxYear = new SqlParameter("@MaxYear", SqlDbType.Int);
            MaxYear.Value = int.Parse(maxYear);

            SqlCommand myCommand = new SqlCommand("select * from locations WHERE Loc_Post_Code = @Param1 AND Crash_Year >= @MinYear AND Crash_Year <= @MaxYear; ", myConnection);//AND Crash_Street LIKE '%' + @Param2 + '%'
            myCommand.Parameters.Add(myParam);
            myCommand.Parameters.Add(MinYear);
            myCommand.Parameters.Add(MaxYear);
            return myCommand;
        }
    }

    [Serializable]
    public class CrashData
    {
        private double lat;
        private double lng;

        public double Lat { get { return lat; } set { lat = value; } }
        public double Lng { get { return lng; } set { lng = value; } }


        public CrashData()
        {
        }
    }
}