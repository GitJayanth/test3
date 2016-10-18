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
using HyperComponents.Data.dbAccess;
using HyperCatalog.Business;
using System.Data.SqlClient;

namespace HyperCatalog.UI.Inputforms
{	/// <summary>
  /// Description résumée de userprofile_products.
  /// </summary>
  public partial class inputforms_usage : HCPage
  {
		#region Declarations
    
    private string InputFormId = string.Empty;
		#endregion

    #region Constantes
    private const string SP_INPUTFORM_OVERVIEW = "dbo._InputForm_Overview";
    #endregion

    #region Code généré par le Concepteur Web Form
    override protected void OnInit(EventArgs e)
    {
      InitializeComponent();
      txtFilter.AutoPostBack = false;
      txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);"); Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "<script defer=true>var txtFilterField;txtFilterField = document.getElementById('" + txtFilter.ClientID + "');</script>");       
      base.OnInit(e);
    }
		
    /// <summary>
    /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
    /// le contenu de cette méthode avec l'éditeur de code.
    /// </summary>
    private void InitializeComponent()
    {    
			this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);
			this.dg.InitializeRow += new Infragistics.WebUI.UltraWebGrid.InitializeRowEventHandler(this.dg_InitializeRow);

		}
    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
    {
      if (Request["i"] != null)
      {
        // Retrieve Id of input form
        InputFormId = Request["i"].ToString();

      if (Request["filter"] != null)
      {
        txtFilter.Text = Request["filter"].ToString();
      }
        if (!Page.IsPostBack)
        {
          UpdateDataView();
        }
      }
    }
    
    void UpdateDataView()
    {
			// Open connection
      using (Database dbObj = Utils.GetMainDB())
      {

        // SQL query
        using (DataSet ds = dbObj.RunSPReturnDataSet(SP_INPUTFORM_OVERVIEW,
          new SqlParameter("@InputFormId", InputFormId)))
        {
          dbObj.CloseConnection();

          if (ds != null)
          {
            if (dbObj.LastError.Length == 0)
            {
              dg.DataSource = ds.Tables[0];
              Utils.InitGridSort(ref dg, false);
              dg.DataBind();
              dg.Columns.Remove(dg.Columns.FromKey("__Spacer"));

              if (dg.Rows.Count > 0)
              {
                dg.Visible = true;
                pnlApplLevel.Visible = true;
                lbNoresults.Visible = false;
              }
              else
              {
                if (txtFilter.Text.Length > 0)
                  lbNoresults.Text = "No record match your search (" + txtFilter.Text + ")";

                dg.Visible = false;
                pnlApplLevel.Visible = false;
                lbNoresults.Visible = true;
              }
            }
            else
            {
              lbError.CssClass = "hc_error";
              lbError.Text = dbObj.LastError;
              lbError.Visible = true;
            }
            ds.Dispose();
          }
        }
      }
    }

    private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;
      string itemName = r.Cells.FromKey("ItemName").Text;
      r.Cells.FromKey("ItemName").Text = "[" + r.Cells.FromKey("LevelId").Text + "] " + itemName;

      bool isDeleted = false;
      if (txtFilter.Text.Length > 0)
			{
        string filter = txtFilter.Text.Trim().ToLower();

        if (itemName.Length == 0 || itemName.ToLower().IndexOf(filter) < 0)
        {
          isDeleted = true;
          r.Delete();
        }
        else
          UITools.HiglightGridRowFilter(ref r, txtFilter.Text);
      }

      if (!isDeleted)
      { 
        // Display PLs
        if (r.Cells.FromKey("PLs") != null && r.Cells.FromKey("PLs").Text != null && r.Cells.FromKey("PLs").Text.Length > 0)
        {
          string[] PLs = r.Cells.FromKey("PLs").Text.Split(',');
          string sPLs = string.Empty;

          if (r.Cells.FromKey("DefinedPLs") != null && r.Cells.FromKey("DefinedPLs").Text != null && r.Cells.FromKey("DefinedPLs").Text.Length > 0)
          {
            string definedPLs = r.Cells.FromKey("DefinedPLs").Text;
            for (int i = 0; i < PLs.Length; i++)
            {
              if (sPLs.Length > 0) sPLs += ", ";

              if (definedPLs.IndexOf(PLs[i]) >= 0)
                sPLs += "<font color=red>" + PLs[i] + "</font>";
              else
                sPLs += PLs[i];
            }
          }
          else
          {
            for (int i = 0; i < PLs.Length; i++)
            {
              if (sPLs.Length > 0) sPLs += ", ";
              sPLs += PLs[i];
            }
          }
          r.Cells.FromKey("PLs").Text = sPLs;
        }
      }
    }

    private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      if (btn == "export")
      {
        Utils.ExportToExcel(dg, InputFormId.ToString()+ "-Items", InputFormId.ToString()+ "-Items");
      }
    }
  }
}