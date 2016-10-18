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
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using HyperCatalog.Shared;
using HyperComponents.Data.dbAccess;
using HyperCatalog.Business;
using HyperCatalog.UI.Tools;


public partial class UI_Admin_MMDFullFillmentReport : System.Web.UI.Page
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
         this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);
         //this.dg.InitializeRow += new Infragistics.WebUI.UltraWebGrid.InitializeRowEventHandler(this.dg_InitializeRow);

     }
     #endregion
        
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
           PLCodeLoad();           
        }

    }

    private DataSet PLCodeLoad()
    {
        using (Database dbObj = Utils.GetMainDB())
        {
            using (DataSet ds = dbObj.RunSPReturnDataSet("_MMDGetAllPLCodes"))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlPlcode.DataSource = ds.Tables[0].DefaultView;
                    ddlPlcode.DataTextField = "PLCode";
                    ddlPlcode.DataValueField = "PLCode";
                    ddlPlcode.DataBind();
                    ddlPlcode.Visible = true;
                }
                return ds;
                dbObj.CloseConnection();
                dbObj.Dispose();
            }
        }
    }

    private void UpdateDataView()
    {
       using (Database dbObj = Utils.GetMainDB())
        {
            using (DataSet ds = dbObj.RunSPReturnDataSet("_Report_MMDFullFillmentByPlCode", new SqlParameter("@PLCode", ddlPlcode.SelectedValue)))
            {
                dbObj.CloseConnection();
                if (dbObj.LastError != string.Empty)
                {
                    lbMessage.Text = "No MMD Complaint Products Found -> " + dbObj.LastError;
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
                        lbMessage.Text = "No MMD Complaint Products found";
                        lbMessage.Visible = true;
                        dg.Visible = false;
                    }
                    #endregion
                }
            }
        }
    }
    
    protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
        string btn = be.Button.Key.ToString();
        #region Apply
        if (btn == "Generate")
        {
            UpdateDataView();
        }
        #endregion
        #region Export
        if (btn == "Export")
        {
            Utils.ExportToExcel(dg, "MMDFullFillment Report", "MMDFullFillment Report");
        }
        #endregion
    }

  }
