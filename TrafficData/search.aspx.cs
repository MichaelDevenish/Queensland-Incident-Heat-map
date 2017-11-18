﻿using Newtonsoft.Json;
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
        //this is where the majority of processing is done
        protected void Page_Load(object sender, EventArgs e)
        {
            string a = Request.Params["location"];
            int ignored = 0;
            bool usable = int.TryParse(a, out ignored);


            if (usable)
            {
                JObject jsonObject = new JObject();
                SqlConnection myConnection = new SqlConnection("Data Source=MICHAELPC;Initial Catalog=qldCrashes;Integrated Security=True");
                try
                {
                    myConnection.Open();
                    SqlDataReader myReader = null;
                    SqlParameter myParam = new SqlParameter("@Param1", SqlDbType.VarChar);
                    myParam.Value = a;

                    SqlCommand myCommand = new SqlCommand("select * from locations WHERE Loc_Post_Code = @Param1 ", myConnection);//AND Crash_Street LIKE '%' + @Param2 + '%'
                    myCommand.Parameters.Add(myParam);

                    myReader = myCommand.ExecuteReader();
                    List<CrashData> data = new List<CrashData>();
                    while (myReader.Read())
                    {
                        data.Add(new CrashData
                        {
                            Lat = (double)myReader["Crash_Latitude_GDA94"],
                            Lng = (double)myReader["Crash_Longitude_GDA94"]
                        });
                        //format json response here
                    }
                    jsonObject.Add("data", JToken.FromObject(data));
                    myConnection.Close();
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.ToString());
                }
                Response.Clear();
                Response.ContentType = "application/json; charset=utf-8";
                Response.Write(jsonObject.ToString());
                Response.End();
            }

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