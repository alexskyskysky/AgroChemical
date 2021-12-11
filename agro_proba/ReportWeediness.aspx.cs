using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace agro_proba
{
    public partial class ReportWeediness : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Browser.Cookies)
            {
                HttpCookie cookie_report = Request.Cookies["Agrochim31_Report"];
                ObjectDataSource1.SelectParameters["id_organization"].DefaultValue = cookie_report["id"].ToString();
                ObjectDataSource1.SelectParameters["year"].DefaultValue = cookie_report["year"].ToString();
                ObjectDataSource1.DataBind();
            }
        }
    }
}