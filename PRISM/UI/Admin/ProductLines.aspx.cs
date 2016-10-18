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
using System.Text;
using HyperCatalog.Business;
#endregion

namespace HyperCatalog.UI.Admin
{
	/// <summary>
	/// Display list of menu
	/// </summary>
	public partial class UIProductLines : HCPage
	{
		#region Declarations
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
    ///		Méthode requise pour la prise en charge du concepteur - ne modifiez pas
    ///		le contenu de cette méthode avec l'éditeur de code.
    /// </summary>
    private void InitializeComponent()
    {

    }
    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
    {
      if (!Page.IsPostBack)
      {
        UpdateDataView(false);
      }
      else
      {
        if (Request.Form["__EVENTTARGET"].ToString() == "PLCode")
        {
          UpdateDataEdit(Request.Form["__EVENTARGUMENT"].ToString());
        }
      }
    }

    private string BuildTable(bool showAll)
    {
      using (Database dbObj = Utils.GetMainDB())
      {
        string filter = showAll ? "" : " WHERE IsActive = 1";
        using (IDataReader rs = dbObj.RunSQLReturnRS("SELECT * FROM BPL " + filter + " ORDER by OrgName, GroupName, GBUName, PLCode"))
        {
          string curOrg = string.Empty, curGroup = string.Empty, curGBU = string.Empty, activeStart, activeEnd;
          bool isActive;
          webTab.Visible = false;
          StringBuilder s = new StringBuilder();
          s.Append("<table border=1 CELLSPACING=0 CELLPADDING=0 style='border-collapse:collapse;width:100%'>");
          if (dbObj.LastError == string.Empty)
          {
            while (rs.Read())
            {
              isActive = Convert.ToBoolean(rs["IsActive"]);
              activeStart = isActive ? "" : "<i>";
              activeEnd = isActive ? "" : "</i>";

              if (rs["OrgCode"].ToString() != curOrg)
              {
                curOrg = rs["OrgCode"].ToString();
                s.Append("\n\t<tr valign='top'><td class='org'>" + activeStart + "[" + curOrg + "] - " + rs["OrgName"].ToString() + activeEnd + "</td></tr>");
              }
              if (rs["GroupCode"].ToString() != curGroup)
              {
                curGroup = rs["GroupCode"].ToString();
                s.Append("\n\t<tr valign='top'><td class='group'>" + activeStart + "[" + curGroup + "] - " + rs["GroupName"].ToString() + activeEnd + "</td></tr>");
              }
              if (rs["GBUCode"].ToString() != curGBU)
              {
                curGBU = rs["GBUCode"].ToString();
                s.Append("\n\t<tr valign='top'><td class='gbu'>" + activeStart + "[" + curGBU + "] - " + rs["GBUName"].ToString() + activeEnd + "</td></tr>");
              }
              s.Append("\n\t<tr valign='top'><td class='pl'>" + activeStart + "<a href=\"javascript:__doPostBack('PLCode','" + rs["PLCode"] + "')\">[" + rs["PLCode"] + "] - " + rs["PLName"].ToString() + "</a>" + activeEnd + "</td></tr>");
            }
            s.Append("</table>");
            rs.Close();
            return s.ToString();
          }
          else
          {
            return dbObj.LastError;
          }
        }
      }
    }
    private void UpdateDataView(bool showAll)
    {
        lbPLs.Text = BuildTable(showAll);
        panelGrid.Visible = true;
        webTab.Visible = false;
    }

    private void UpdateDataEdit(string selPLCode)
    {
      panelGrid.Visible = false;
      webTab.Tabs.GetTab(0).ContentPane.TargetUrl = "./pls/pl_properties.aspx?p=" + selPLCode;
      PL p = PL.GetByKey(selPLCode);
      lbTitle.Text = "PL: [" + p.Code + "] " + p.Name;
      webTab.Tabs[1].Visible = true;
      webTab.Tabs.GetTab(1).ContentPane.TargetUrl = "./pls/pl_users.aspx?p=" + selPLCode;
      webTab.Visible = true;
      webTab.Tabs[2].Visible = false;
    }

    protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      switch (btn)
      {
        case ("export"):
          using (Database dbObj = Utils.GetMainDB())
          {
            DataSet ds = dbObj.RunSQLReturnDataSet("SELECT * FROM BPL ORder by OrgName, GroupName, GBUName, PLCode", "PRISM - Product Lines Extract");
            dbObj.CloseConnection();
            Utils.ExportDataSetToExcel(this, ds, "PRISM - Product Lines Extract.xls");
            return;
          }
        case ("active"):
          UpdateDataView(be.Button.Selected);
          return;
      case ("add"):
          panelGrid.Visible = false;
          webTab.Tabs.GetTab(2).ContentPane.TargetUrl = "./pls/PL_Add.aspx";
          webTab.Tabs[2].Visible = true;
          webTab.Visible = true;
          webTab.Tabs[0].Visible = false;
          webTab.Tabs[1].Visible = false;
          webTab.SelectedTabIndex = 2;
          return;

      }
    }
}
}
