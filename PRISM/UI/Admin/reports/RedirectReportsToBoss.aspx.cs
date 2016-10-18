using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HyperCatalog.Shared;
using System.Reflection;
using System.Data.SqlClient;
using HyperCatalog.Business;


public partial class UI_Admin_reports_RedirectReportsToBoss : System.Web.UI.Page
{
    private HyperComponents.Data.dbAccess.Database dbObj;
    private HyperComponents.Data.dbAccess.DatabaseQuery dbQuery;
    //private int timeout = MIN_TIMEOUT;
    protected void Page_Load(object sender, EventArgs e)
    {
        string reportId, reportName;
        //BossLink.NavigateUrl = string.Empty;

        //AccessLink.ForeColor = System.Drawing.Color.Blue;
        reportId = Request.QueryString["reportId"];
        reportName = Request.QueryString["reportName"];
        DataSet ds = new DataSet();
        dbObj = Utils.GetMainDB();
        dbObj.OpenConnection();
        dbQuery = dbObj.CreateSPQuery("Report_GetBossReports", new SqlParameter("@ReportName", reportName));
        ds = dbQuery.RunAndReturnDataSet();
        dbObj.CloseConnection();
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {

            string tablestring = "";
            string header = "";
            string _sCompany = "";
            //Commeneted by Vidya 21/03/2016
            //int hpiReport = 0;
            //End
            int hpeReport = 0;
            tablestring = tablestring + "<table>";
            _sCompany = ApplicationSettings.Parameters["BossReports"].Value.ToString();


            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["BossDescription"].ToString().Contains(_sCompany))
                {
                    string strUrl = (string)HyperCatalog.Shared.SessionState.CacheParams["Boss_Link_HPI"].Value;
                    strUrl = strUrl + "&iDocID=" + dr["BossId"].ToString() + "&sDoc=" + dr["BossDescription"].ToString() + "&sType=wid&sRefresh=Y";
                    header = "<tr><th style='font-size:80%';height='20%'; Font-Names:'Arial'; Font-Size:'Small'>HPI Reports</tr></th>";
                    //Commeneted by Vidya 21/03/2016
                    //if (hpiReport ==0)
                    //tablestring = tablestring + header + "<tr><td style='font-size:80%';height='20%'; Font-Names:'Arial'; Font-Size:'Small'><a href=" + strUrl + " target='_blank'>" + dr["BossDescription"].ToString() + "</a></td></tr>";
                    //else
                    //tablestring = tablestring + "<tr><td style='font-size:80%';height='20%'; Font-Names:'Arial'; Font-Size:'Small'><a href=" + strUrl + " target='_blank'>" + dr["BossDescription"].ToString() + "</a></td></tr>";
                    //hpiReport = hpiReport + 1; 
                }
                else
                {
                    string strUrl = (string)HyperCatalog.Shared.SessionState.CacheParams["BOSS_Link"].Value;
                    strUrl = strUrl + "&iDocID=" + dr["BossId"].ToString() + "&sDoc=" + dr["BossDescription"].ToString() + "&sType=wid&sRefresh=Y";
                    header = "<tr><th style='font-size:80%';height='20%'; Font-Names:'Arial'; Font-Size:'Small'>HPE Reports</tr></th>";
                    if (hpeReport == 0)
                        tablestring = tablestring + header + "<tr><td style='font-size:80%';height='20%'; Font-Names:'Arial'; Font-Size:'Small'><a href=" + strUrl + " target='_blank'>" + dr["BossDescription"].ToString() + "</a></td></tr>";
                    else
                        tablestring = tablestring + "<tr><td style='font-size:80%';height='20%'; Font-Names:'Arial'; Font-Size:'Small'><a href=" + strUrl + " target='_blank'>" + dr["BossDescription"].ToString() + "</a></td></tr>";
                    hpeReport = hpeReport + 1;

                }
            }
            tablestring = tablestring + "</table>";
            divTable.InnerHtml = tablestring;
        }
        //BossLink.NavigateUrl = (strihow ng)HyperCatalog.Shared.SessionState.CacheParams["BOSS_Link"].Value;
        //BossLink.NavigateUrl = "http://bosspro-ci.atlanta.hp.com/OpenDocument/opendoc/openDocument.jsp?sIDType=CUID";
        //BossLink.NavigateUrl =  BossLink.NavigateUrl + "&reportId=" + reportId + "&reportName=" + reportName + "&sType=wid&sRefresh=Y";
    }

}
