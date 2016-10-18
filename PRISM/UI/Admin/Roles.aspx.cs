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
using HyperCatalog.Business;
using HyperCatalog.Shared;
using System.Data.SqlClient;

#endregion

#region History
	  // Add capabilities (CHARENSOL Mickael 24/10/2005)
#endregion 

namespace HyperCatalog.UI.Admin
{
	/// <summary>
	/// Display role list
	///		--> Add new role
	///		--> Modify role
	///		--> Export in Excel
	///		--> Filter on all fields of the grid
	/// </summary>
	public partial class Roles : HCPage
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
      UITools.HideToolBarButton(uwToolbar, "Add");
      UITools.HideToolBarSeparator(uwToolbar, "AddSep");
    }
    
    /// <summary>
    /// Display all roles
    /// </summary>
    private void UpdateDataView()
    {
      webTab.Visible = false;
      panelGrid.Visible = dg.Visible = true;      
      string sSql = string.Empty;

			string filter = txtFilter.Text;
      if (filter!=string.Empty)
      {
				string cleanFilter = filter.Replace("'", "''").ToLower();
				cleanFilter = cleanFilter.Replace("[", "[[]");
				cleanFilter = cleanFilter.Replace("_", "[_]");
				cleanFilter = cleanFilter.Replace("%", "[%]");

        sSql += " LOWER(Name) like '%" + cleanFilter +"%' ";
        sSql += " OR LOWER(Description) like '%" + cleanFilter +"%' ";
      }
      using (RoleList roles = Role.GetAll(sSql))
      {
        if (roles != null)
        {
          if (roles.Count > 0)
          {
            dg.DataSource = roles;
            Utils.InitGridSort(ref dg);
            dg.DataBind();

            dg.Visible = true;
            lbNoresults.Visible = false;
          }
          else
          {
            if (txtFilter.Text.Length > 0)
              lbNoresults.Text = "No record match your search (" + txtFilter.Text + ")";

            lbNoresults.Visible = true;
            dg.Visible = false;
          }
        }
			}
    }

    protected void UpdateDataEdit(string roleId)
    {
      panelGrid.Visible = false;
      webTab.Tabs.GetTab(0).ContentPane.TargetUrl = "./roles/role_properties.aspx?r=" + roleId;
      Role role = Role.GetByKey(Convert.ToInt32(roleId));
      if (role == null)
      { 
        webTab.Tabs.GetTab(1).ContentPane.TargetUrl = "";
        webTab.Tabs[1].Visible = webTab.Tabs[2].Visible = false;
        lbTitle.Text = "Role: New";
      }
      else
      {        
        lbTitle.Text = "Role: " + role.Name;
        webTab.Tabs[1].Visible = webTab.Tabs[2].Visible = true;
        webTab.Tabs.GetTab(1).ContentPane.TargetUrl = "./roles/role_users.aspx?r=" + roleId;
        webTab.Tabs.GetTab(1).Text = "Users " + "(" + role.Users.Count.ToString() + ")";
        webTab.Tabs.GetTab(2).ContentPane.TargetUrl = "./roles/role_capabilities.aspx?r=" + roleId;
        webTab.Tabs.GetTab(2).Text = "Capabilities " + "(" + role.Capabilities.Count.ToString() + ")";
        webTab.Tabs.GetTab(3).ContentPane.TargetUrl = "./roles/role_notifications.aspx?r=" + roleId;
        webTab.Tabs.GetTab(3).Text = "Notifications " + "(" + role.Notifications.Count.ToString() + ")";
      }
      webTab.Visible = true;
    }


    private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      if (btn == "export")
			{
        Utils.ExportToExcel(dg, "Roles", "Roles");
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
