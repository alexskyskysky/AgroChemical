using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace agro_proba
{
    public partial class ReportFertilizerProtocol : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Browser.Cookies)
            {
                HttpCookie cookie_report_protocol = Request.Cookies["Agrochim31_Report_Protocol"];
                ObjectDataSource1.SelectParameters["id_protocol"].DefaultValue = cookie_report_protocol["id"].ToString();
                ObjectDataSource1.DataBind();
                ObjectDataSource2.SelectParameters["id_protocol"].DefaultValue = cookie_report_protocol["id"].ToString();
                ObjectDataSource2.DataBind();
            }
        }
    }
}