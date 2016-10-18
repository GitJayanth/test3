#region Uses
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
using HyperCatalog.Business;
using HyperComponents.Data.dbAccess;
using HyperCatalog.Shared;
using System.Data.SqlClient;
#endregion
public partial class UI_Acquire_ArchivalReport : System.Web.UI.Page
{
    #region Code généré par le Concepteur Web Form
    override protected void OnInit(EventArgs e)
    {
        //
        // CODEGEN : Cet appel est requis par le Concepteur Web Form ASP.NET.
        //
        InitializeComponent();
        base.OnInit(e);
    }

    /// <summary>
    /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
    /// le contenu de cette méthode avec l'éditeur de code.
    /// </summary>
    private void InitializeComponent()
    {

    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            FrmDate.Value = DateTime.UtcNow.ToString("MM/dd/yyyy");
            ToDate.Value = DateTime.UtcNow.ToString("MM/dd/yyyy");
        }

    }


    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        Database dbObj = new Database(HyperCatalog.Business.ApplicationSettings.Components["Archive_DB"].ConnectionString, 700000);

        using (DataSet ds = dbObj.RunSPReturnDataSet("_Report_ItemArchive", new SqlParameter("@FrmDt", Convert.ToDateTime(FrmDate.Text).ToString("MM/dd/yyyy")), new SqlParameter("@EndDt", Convert.ToDateTime(ToDate.Text).ToString("MM/dd/yyyy"))))
        {
            //dbObj.TimeOut = 70000;
            dbObj.CloseConnection();
            if (dbObj.LastError != string.Empty)
            {
                lbMessage.Text = "No Archived Products Found -> " + dbObj.LastError;
                lbMessage.CssClass = "hc_error";
                lbMessage.Visible = true;
            }
            else
            {
                #region Results
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dg.DataSource = ds;
                    lbMessage.Visible = false;
                    //Utils.InitGridSort(ref dg);
                    dg.DataBind();
                    dg.Visible = true;
                }
                #endregion
                #region No result
                else
                {
                    lbMessage.Text = "No Archived Products found";
                    lbMessage.Visible = true;
                    dg.Visible = false;
                }
                #endregion
            }
        }

    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        Utils.ExportToExcel(dg, "Archival  Report", "Archival Report");
    }

}
