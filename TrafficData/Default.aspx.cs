using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TrafficData
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<int> yearList = new List<int>();
            for (int i = 2001; i <= DateTime.Now.Year; i++)
            {
                yearList.Add(i);
            }
            BottomYear.DataSource = yearList;
            BottomYear.DataBind();
            topYear.DataSource = yearList;
            topYear.DataBind();
            BottomYear.SelectedValue = 2001.ToString();
            topYear.SelectedValue = DateTime.Now.Year.ToString();
        }

    }
}