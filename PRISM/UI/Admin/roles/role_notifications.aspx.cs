#region Uses
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
using Infragistics.WebUI.UltraWebGrid;
#endregion

/// <summary>
/// Display Notifications list having the selected role
///		--> Return to the role list
///		--> Export in Excel
///		--> Filter on all fields of the grid
/// </summary>
public partial class role_Notifications : HCPage
{
  #region Declarations
  protected System.Web.UI.WebControls.DropDownList DDL_NotificationsList;

  private int roleId = -1;
  #endregion

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

  }
  #endregion

  protected void Page_Load(object sender, System.EventArgs e)
  {
    try
    {
			if (Request["r"] != null)
				roleId = Convert.ToInt32(Request["r"]);

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
    using (Database dbObj = Utils.GetMainDB())
    {
      DataSet ds = dbObj.RunSPReturnDataSet("_Role_GetNotifications", "Notifications", new SqlParameter("@RoleId", roleId));
      dbObj.CloseConnection();
      if (dbObj.LastError == string.Empty)
      {
        dg.DataSource = ds.Tables["Notifications"];
        Utils.InitGridSort(ref dg);
        dg.DataBind();
        ds.Dispose();
      }
      else
      {
        lbError.CssClass = "hc_error";
        lbError.Text = dbObj.LastError;
        lbError.Visible = true;
      }
    }
  }


  private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
  {
    string btn = be.Button.Key.ToLower();
    #region Export
    if (btn == "export")
    {
      UpdateDataView();
      Utils.ExportToExcel(dg, "Role_Notifications", "Role-" + Role.GetByKey(roleId).Name);
    }
    #endregion
    #region Apply changes
    if (btn == "save")
    {
      Save();
    }
    #endregion
  }

  /// <summary>
  /// Add a new Notification to the role selected
  /// </summary>
  private void Add()
  {
    using (Database dbObj = Utils.GetMainDB())
    {
      int r = dbObj.RunSPReturnInteger("_RoleNotification_Add",
        new SqlParameter("@RoleId", roleId),
        new SqlParameter("@NotificationId", DDL_NotificationsList.SelectedValue));
      dbObj.CloseConnection();
      if ((dbObj.LastError == string.Empty) && (r > 0))
      {
        lbError.Text = "Data added";
        lbError.CssClass = "hc_success";
        lbError.Visible = true;
        UpdateDataView();
      }
      else
      {
        lbError.Text = "Error: role Notification relation can't be created";
        lbError.CssClass = "hc_error";
        lbError.Visible = true;
      }
    }
  }

  /// <summary>
  /// Save changes for each row
  /// </summary>
  private void Save()
  {
    using (Database dbObj = Utils.GetMainDB())
    {

      // *************************************************************************
      // Update role Notifications scope
      // *************************************************************************
      if (dg.Rows.Count > 0)
      {
        string updateSQL = string.Empty;
        string NotificationId;
        lbError.Visible = false;

        for (int i = 0; i < dg.Rows.Count; i++)
        {
          NotificationId = dg.Rows[i].Cells.FromKey("NotificationId").Text;
          if (Convert.ToBoolean(dg.Rows[i].Cells.FromKey("InScope").Value))
            updateSQL += string.Format("IF (NOT EXISTS(SELECT NotificationId FROM RoleNotifications WHERE NotificationId = {0}AND RoleId={1})) BEGIN INSERT INTO RoleNotifications(RoleId, NotificationId) VALUES({1} , {0}); END ", NotificationId, roleId);
          else
            updateSQL += string.Format("IF (EXISTS(SELECT NotificationId FROM RoleNotifications WHERE NotificationId = {0} AND RoleId={1})) BEGIN DELETE from RoleNotifications WHERE NotificationId = {0} AND RoleId= {1}; END ", NotificationId, roleId);
        }
        Debug.Trace("PRISM.UI", "updateSQL=<BR/>" + updateSQL, DebugSeverity.High);

        dbObj.RunSQLReturnRS(updateSQL);
        dbObj.CloseConnection();
        if (dbObj.LastError.Length > 0)
        {
          lbError.CssClass = "hc_error";
          lbError.Text = dbObj.LastError + "<br/>";
          lbError.Visible = true;
        }
        else
        {
          lbError.CssClass = "hc_success";
          lbError.Text = "Data Saved<br/>";
          lbError.Visible = true;
          UpdateDataView();
        }
      }
      else
      {
        UpdateDataView();
      }
      UITools.RefreshTab(Page, "Notifications", Utils.GetCount(dbObj, string.Format("SELECT COUNT(*) FROM RoleNotifications WHERE RoleId = {0}", roleId)));
    }
  }

}

