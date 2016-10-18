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

/// <summary>
/// Display member list of selected organization
///		--> Return to the organization list
///		--> Export in Excel
///		--> Filter on Name and Role fields
/// </summary>
public partial class organization_members : HCPage
{
	#region Declarations

	private int orgId = -1;
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

  HyperCatalog.Business.Organization org;
	protected void Page_Load(object sender, System.EventArgs e)
	{
		try
		{
      if (Request["filter"] != null)
      {
        txtFilter.Text = Request["filter"].ToString();
      }
  		orgId = Convert.ToInt32(Request["o"]);
      org = HyperCatalog.Business.Organization.GetByKey(orgId);
			if (!Page.IsPostBack)
			{
				UpdateDataView();
			}
		}
		catch
		{
			Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"clientScript", "<script>back();</script>");    
		}
	}
    
	void UpdateDataView()
	{
    if (org != null)
    {
      HyperCatalog.Business.UserList users = org.Users;

      if (users != null)
      {
        dg.DataSource = users;
        Utils.InitGridSort(ref dg, true);
        dg.DataBind();

        if (dg.Rows.Count > 0)
        {
          dg.Visible = true;
          lbNoresults.Visible = false;
        }
        else
        {
          if (txtFilter.Text.Length > 0)
            lbNoresults.Text = "No record match your search (" + txtFilter.Text + ")";
          dg.Visible = false;
          lbNoresults.Visible = true;
        }
      }
    }
  }

	private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
	{
		string btn = be.Button.Key.ToLower();
		if (btn == "export")
		{
      Utils.ExportToExcel(dg, Utils.CleanFileName(org.Name + "-Users"), Utils.CleanFileName(org.Name + "-Users"));
		}
	}

	private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
	{
    if (txtFilter.Text.Length > 0)
    {
      Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;

      string filter = txtFilter.Text.Trim().ToLower();
      string userName = r.Cells.FromKey("UserName").Value.ToString().ToLower();
      string roleName = r.Cells.FromKey("RoleName").Value.ToString().ToLower();

      if ((userName.Length == 0 || userName.IndexOf(filter) < 0) && (roleName.Length == 0 || roleName.IndexOf(filter) < 0))
        dg.Rows.Remove(r);
      else
        UITools.HiglightGridRowFilter(ref r, txtFilter.Text);
    }
  }
}
