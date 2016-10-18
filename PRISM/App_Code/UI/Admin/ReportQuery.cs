using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HyperCatalog.Business;
using HyperCatalog.Shared;


/// <summary>
/// Summary description for ReportQuery
/// </summary>
namespace Reporting
{
    public class ReportQuery
    {
        public ReportQuery()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        private static string SQLQuery; // Has the value for the ItenmName to process for import
        public string PSQLQuery
        {
            get { return SQLQuery; }
            set { SQLQuery = value; }
        }
    }
}
