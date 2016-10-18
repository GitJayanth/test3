#region uses
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
using System.Data.SqlClient;
using HyperCatalog.Business;
#endregion

namespace HyperCatalog.UI.Admin
{
	/// <summary>
	/// Display list of organization
	/// </summary>
	public partial class Organizations : HCPage
	{
		#region Declarations

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
    ///		Méthode requise pour la prise en charge du concepteur - ne modifiez pas
    ///		le contenu de cette méthode avec l'éditeur de code.
    /// </summary>
    private void InitializeComponent()
    {
			this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);
			this.dg.InitializeRow += new Infragistics.WebUI.UltraWebGrid.InitializeRowEventHandler(this.dg_InitializeRow);

		}
    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
    {
      if (Request["filter"] != null)
      {
        txtFilter.Text = Request["filter"].ToString();
      }
      if (!Page.IsPostBack)
      {
        UpdateDataView();
      }
    }
    private void UpdateDataView()
    {
      string sSql = "SELECT * FROM Organizations";
      webTab.Visible = false;
			string filter = txtFilter.Text;
      if (filter!=string.Empty)
      {
				string cleanFilter = filter.Replace("'", "''").ToLower();
				cleanFilter = cleanFilter.Replace("[", "[[]");
				cleanFilter = cleanFilter.Replace("_", "[_]");
				cleanFilter = cleanFilter.Replace("%", "[%]");

        sSql += " WHERE ";
        sSql += " LOWER(OrgCode) like '%" + cleanFilter +"%' ";
        sSql += " OR LOWER(OrgName) like '%" + cleanFilter +"%' ";
        sSql += " OR LOWER(OrgType) like '%" + cleanFilter +"%' ";
        sSql += " OR LOWER(OrgDescription) like '%" + cleanFilter +"%' ";
      }

      using (Database dbObj = Utils.GetMainDB())
      {
        using (DataSet ds = dbObj.RunSQLReturnDataSet(sSql, "Organizations"))
        {
          dbObj.CloseConnection();
          if (dbObj.LastError.Length == 0)
          {
            if (ds != null)
            {
              if (ds.Tables["Organizations"] != null && ds.Tables["Organizations"].Rows.Count > 0)
              {
                dg.DataSource = ds.Tables["Organizations"];
                Utils.InitGridSort(ref dg);
                dg.DataBind();

                lbNoresults.Visible = false;
                dg.Visible = true;
              }
              else
              {
                if (txtFilter.Text.Length > 0)
                  lbNoresults.Text = "No record match your search (" + txtFilter.Text + ")";

                lbNoresults.Visible = true;
                dg.Visible = false;
              }

              lbTitle.Text = UITools.GetTranslation("Organization list");
              ds.Dispose();
            }
            else
            {
              lbError.CssClass = "hc_error";
              lbError.Text = "Error: a system error occurred";
              lbError.Visible = true;
            }
          }
          else
          {
            lbError.CssClass = "hc_error";
            lbError.Text = dbObj.LastError;
            lbError.Visible = true;
          }
        }
      }
    }

    void UpdateDataEdit(string selOrgId)
    {
      panelGrid.Visible = false;
      webTab.EnableViewState = false;
      webTab.Tabs.GetTab(0).ContentPane.TargetUrl = "./organizations/organization_properties.aspx?o=" + selOrgId;

      if (selOrgId == "-1")
      {
        webTab.Tabs.GetTab(1).ContentPane.TargetUrl = "";
        webTab.Tabs[1].Visible = false;
        lbTitle.Text = "Organization: New";

        webTab.Visible = true;
      }
      else
      {
        using (Organization org = Organization.GetByKey(Convert.ToInt32(selOrgId)))
        {
          if (org != null)
          {
            lbTitle.Text = "Organization: " + org.Name;
            webTab.Tabs.GetTab(1).ContentPane.TargetUrl = "./organizations/organization_members.aspx?o=" + selOrgId;
            using (Database dbObj = Utils.GetMainDB())
            {
              using (IDataReader rs = dbObj.RunSQLReturnRS("SELECT COUNT(*) FROM Users WHERE OrgId = " + selOrgId))
              {
                if (dbObj.LastError.Length == 0)
                {
                  rs.Read();
                  webTab.Tabs.GetTab(1).Text = webTab.Tabs.GetTab(1).Text + "(" + rs[0].ToString() + ")";
                  webTab.Visible = true;
                  rs.Close();
                }
                else
                {
                  lbError.CssClass = "hc_error";
                  lbError.Text = dbObj.LastError;
                  lbError.Visible = true;
                }
                dbObj.CloseConnection();
              }
            }
          }
          else
          {
            lbError.CssClass = "hc_error";
            lbError.Text = "Error: a system error occurred";
            lbError.Visible = true;
          }
        }
      }
    }

    private void BtnAdd_Click(object sender, System.EventArgs e)
    {
      UpdateDataEdit("-1");
    }

    private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      if (btn == "export")
      {
        Utils.ExportToExcel(dg, "Organizations", "Organizations");
      }
      if (btn == "add")
      {
        UpdateDataEdit("-1");
      }
    }

    private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;
      UITools.HiglightGridRowFilter(ref r, txtFilter.Text);
    }

    // "Name" Link Button event handler
    protected void UpdateGridItem(object sender, System.EventArgs e)
    {
			if (sender != null)
			{
				Infragistics.WebUI.UltraWebGrid.CellItem cellItem = ((Infragistics.WebUI.UltraWebGrid.CellItem)((System.Web.UI.WebControls.LinkButton)sender).Parent);      
				string sId = cellItem.Cell.Row.Cells.FromKey("Id").Text;
			
				sId = Utils.CReplace(sId, "<font color=red><b>", "", 1);
				sId = Utils.CReplace(sId, "</b></font>", "", 1);

				UpdateDataEdit(sId);
			}
    }
  }
}
