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
            string postCode = Request.Params["location"];
            string minYear = Request.Params["minYear"];
            string maxYear = Request.Params["maxYear"];
            bool usable = ErrorCheck(postCode, minYear, maxYear);
            if (usable) PostcodeYearQuery(postCode, minYear, maxYear);
        }

        /// <summary>
        /// Checks the supplied variables to make sure they are safe to use for the queries
        /// </summary>
        /// <param name="postCode">the postcode to query</param>
        /// <param name="minYear">the earliest year to query</param>
        /// <param name="maxYear">the newest year to query</param>
        /// <returns></returns>
        private static bool ErrorCheck(string postCode, string minYear, string maxYear)
        {
            int ignored = 0;
            bool usable = int.TryParse(postCode, out ignored);
            usable = int.TryParse(minYear, out ignored);
            usable = int.TryParse(maxYear, out ignored);
            return usable;
        }

        /// <summary>
        /// Queries the database for all locations that are within a postcode and between the specified years
        /// </summary>
        /// <param name="postCode">the postcode to query</param>
        /// <param name="minYear">the earliest year to query</param>
        /// <param name="maxYear">the newest year to query</param>
        private void PostcodeYearQuery(string postCode, string minYear, string maxYear)
        {
            JObject jsonObject = null;
            //a database connection that contains traffic data
            SqlConnection myConnection = new SqlConnection("Data Source=MICHAELPC;Initial Catalog=qldCrashes;Integrated Security=True");
            //Searches for the query in the database and places the results into a JSON object consisting of a CrashData list 
            try
            {
                myConnection.Open();
                SqlCommand myCommand = SetupLatitudeQuery(postCode, minYear, maxYear, myConnection);
                jsonObject = ExectueLatitudeQuery(myCommand);
                myConnection.Close();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.ToString());
            }
            //display results
            SendJSON(jsonObject);
        }

        /// <summary>
        /// Sends JSON to the front-end
        /// </summary>
        /// <param name="jsonObject">the JSON to send to the front-end</param>
        private void SendJSON(JObject jsonObject)
        {
            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";
            Response.Write(jsonObject.ToString());
            Response.End();
        }

        /// <summary>
        /// Executes a query that retrieves Crash_Latitude_GDA94 and Crash_Longitude_GDA94 from a database 
        /// </summary>
        /// <param name="queryCommand">the query that is to be run</param>
        /// <returns>a jObject that represents the response, which consists of a CrashData List</returns>
        private static JObject ExectueLatitudeQuery(SqlCommand queryCommand)
        {
            JObject response = new JObject();
            SqlDataReader myReader = queryCommand.ExecuteReader();
            List<CrashData> data = new List<CrashData>();
            //goes through each query result and adds them to a list of latitudes and longitudes
            while (myReader.Read())
            {
                data.Add(new CrashData
                {
                    Lat = (double)myReader["Crash_Latitude_GDA94"],
                    Lng = (double)myReader["Crash_Longitude_GDA94"]
                });
            }
            //converts the list to JSON data
            response.Add("data", JToken.FromObject(data));
            return response;
        }

        /// <summary>
        /// Creates a SQL query to get the relevant locations with all of the provided commands securely entered 
        /// </summary>
        /// <param name="postCode">the postcode to search within</param>
        /// <param name="minYear">the oldest year for records</param>
        /// <param name="maxYear">the newest year for records</param>
        /// <param name="myConnection">the database to query</param>
        /// <returns>the constructed query</returns>
        private static SqlCommand SetupLatitudeQuery(string postCode, string minYear, string maxYear, SqlConnection myConnection)
        {
            //construct parameters
            SqlParameter myParam = new SqlParameter("@Param1", SqlDbType.VarChar);
            myParam.Value = postCode;
            SqlParameter MinYear = new SqlParameter("@MinYear", SqlDbType.Int);
            MinYear.Value = int.Parse(minYear);
            SqlParameter MaxYear = new SqlParameter("@MaxYear", SqlDbType.Int);
            MaxYear.Value = int.Parse(maxYear);

            //create query
            SqlCommand myCommand = new SqlCommand("select * from locations WHERE Loc_Post_Code = @Param1 AND Crash_Year >= @MinYear AND Crash_Year <= @MaxYear; ", myConnection);//AND Crash_Street LIKE '%' + @Param2 + '%'
            //add the parameters to the query
            myCommand.Parameters.Add(myParam);
            myCommand.Parameters.Add(MinYear);
            myCommand.Parameters.Add(MaxYear);
            return myCommand;
        }
    }

    /// <summary>
    /// The format that each response is sent to the front-end in
    /// </summary>
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