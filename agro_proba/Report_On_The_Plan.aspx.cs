using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace agro_proba
{
    public partial class Report_On_The_Plan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Browser.Cookies)
            {
                HttpCookie cookie_on_the_plan = Request.Cookies["Agrochim31_Report_On_The_Plan"];
                ObjectDataSource1.SelectParameters["id_plan"].DefaultValue = cookie_on_the_plan["id_plan"].ToString();
                ObjectDataSource1.DataBind();
            }
        }
    }
}