using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace agro_proba
{
    public partial class ReportStickers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Browser.Cookies)
            {
                HttpCookie cookie_stickers = Request.Cookies["Agrochim31_Report_Stickers"];
                ObjectDataSource1.SelectParameters["id_organization"].DefaultValue = cookie_stickers["id_organization"].ToString();
                ObjectDataSource1.SelectParameters["count_probes"].DefaultValue = cookie_stickers["count_probes"].ToString();
                ObjectDataSource1.DataBind();
            }
        }
    }
}