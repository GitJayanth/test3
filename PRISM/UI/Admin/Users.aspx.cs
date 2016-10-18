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
using System.Data.SqlClient;
using HyperCatalog.Shared;
using HyperComponents.Data.dbAccess;
#endregion

//TODO: Sort Order on dates and IsActive flag is freezed for the moment
namespace HyperCatalog.UI.Admin
{
  /// <summary>
  /// Display user list
  ///		--> Add new user
  ///		--> Modify user
  ///		--> Export in Excel
  ///		--> Filter on all fields of the grid
  ///		--> Filter on locked users
  /// </summary>
  public partial class Users : HCPage
  {
		#region Declarations
    int _LastLoginPos = 0;
		#endregion

    #region Code généré par le Concepteur Web Form
    override protected void OnInit(EventArgs e)
    {
      InitializeComponent();
      txtFilter.AutoPostBack = false;
      txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);"); Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "<script defer=true>var txtFilterField;txtFilterField = document.getElementById('" + txtFilter.ClientID + "');</script>");       
      _LastLoginPos = dg.Columns.FromKey("LastLogin").Index;

			//this.cbFilterLock.CheckedChanged += new System.EventHandler(this.cbFilterLock_CheckedChanged);
			base.OnInit(e);
    }
		
    private void InitializeComponent()
    {
		}
    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
    {
      if (Request["filter"] != null)
      {
        txtFilter.Text = Request["filter"].ToString();
      }
      if (Request["u"] != null)
      {
        UpdateDataEdit(Request["u"].ToString());
        return;
      }
      if (!IsPostBack)
      {
        UpdateDataView();

      }
    }
    protected override void OnPreRender(EventArgs e)
    {
        if (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_USERS))
        {
            uwToolbar.Items.FromKeyButton("Add").Visible = false;
            UITools.HideToolBarSeparator(uwToolbar.Items.FromKeySeparator("Sep0"));
            hyperlink1.Visible = false;
            UITools.HideToolBarSeparator(uwToolbar.Items.FromKeySeparator("Sep0"));

        }
        else
        {
            hyperlink1.NavigateUrl = SessionState.CacheComponents["DeligatedAdmin"].URI;

        }
      base.OnPreRender(e);
    }

    private void UpdateDataView()
    {
      //cbFilterLock.Checked = SessionState.tmPageIndexExpression == "1";
      string sSql = string.Empty;
      sSql = " SELECT * " +
       " FROM (SELECT obj.UserId AS Id, FirstName + ' ' + LastName as FullName, O.OrgName , R.RoleName , LastLoginDate, LogCount, IsActive" +
       "      FROM Roles R WITH (NOLOCK), Users obj WITH (NOLOCK), Organizations O WITH (NOLOCK)" +
       "      WHERE R.RoleId = obj.RoleId AND obj.OrgId = O.OrgId" +
       "      ) Q";

			string filter = txtFilter.Text;
			if (filter!=string.Empty)
      {
				string cleanFilter = filter.Replace("'", "''").ToLower();
				cleanFilter = cleanFilter.Replace("[", "[[]");
				cleanFilter = cleanFilter.Replace("_", "[_]");
				cleanFilter = cleanFilter.Replace("%", "[%]");

        sSql += " WHERE (LOWER(FullName) like '%" + cleanFilter + "%'";
        sSql += " OR LOWER(RoleName) like '%" + cleanFilter + "%'";
        sSql += " OR LOWER(OrgName) like '%" + cleanFilter + "%')";
      }
      //if (cbFilterLock.Checked)
      //{
      //  //if (filter != string.Empty)
      //  //  sSql += " AND ";
      //  //sSql += " AttemptsLeft = 0";
      //}
      sSql += " ORDER BY IsActive DESC, FullName ASC";
      using (Database dbObj = Utils.GetMainDB())
      {
        using (DataSet dsUsers =dbObj.RunSQLReturnDataSet(sSql, ""))
        {
          Trace.Warn("Users", "Users retrieval OK");
          lbTitle.Text = "User list";
          if (dsUsers!=null)
          {
            dg.Columns.FromKey("LastLogin").Format = SessionState.User.FormatDate + ' ' + SessionState.User.FormatTime;
            if (dsUsers.Tables[0].Rows.Count > 0) 
				    {
              dg.DataSource = dsUsers.Tables[0].DefaultView;
              Utils.InitGridSort(ref dg);
              Trace.Warn("Users", "     ->begin databind");
              dg.DataBind();
              Trace.Warn("Users", "     ->end databind");
              dg.Visible = true;
					    lbNoresults.Visible = false;
				    }
				    else
				    {
					    if (txtFilter.Text.Length > 0)
						    lbNoresults.Text = "No record match your search ("+txtFilter.Text+")";
					    lbNoresults.Visible = true;
					    dg.Visible = false;
				    }
            webTab.Visible = false;
            panelGrid.Visible= true;
          }
        }
      }
            
      /*Trace.Warn("Users", "Retrieve User with filter [" + sSql + "]");
      UserList users = HyperCatalog.Business.User.GetAll(sSql);
      Trace.Warn("Users", "Users retrieval OK");

      lbTitle.Text = "User list";
      if (users!=null)
      {
				if (users.Count > 0) 
				{
					users.Sort("IsActive DESC, FullName ASC");
          dg.DataSource = users;
          Trace.Warn("Users", "Starting Init Grid Sort");
          Utils.InitGridSort(ref dg);
          Trace.Warn("Users", "Starting Databind");
          dg.DataBind();
          
          Trace.Warn("Users", "Databind done");
          dg.Columns.FromKey("LastLogin").Format = SessionState.User.FormatDate + ' ' + SessionState.User.FormatTime;

					dg.Visible = true;
					lbNoresults.Visible = false;
				}
				else
				{
					if (txtFilter.Text.Length > 0)
						lbNoresults.Text = "No record match your search ("+txtFilter.Text+")";

					lbNoresults.Visible = true;
					dg.Visible = false;
				}
        webTab.Visible = false;
        panelGrid.Visible= true;
      }    */  
    }

    void UpdateDataEdit(string selUserId)
    {
      panelGrid.Visible = false;

      webTab.EnableViewState = false;
      webTab.Tabs.GetTab(0).ContentPane.TargetUrl = "./user/user_properties.aspx?u=" + selUserId;
      HyperCatalog.Business.User curUser = HyperCatalog.Business.User.GetByKey(Convert.ToInt32(selUserId));
      if (curUser == null)
      {
        webTab.Tabs[1].Visible = webTab.Tabs[2].Visible = webTab.Tabs[3].Visible = false;
        lbTitle.Text = "User: New";
      }
      else
      {
        lbTitle.Text = "User: " + curUser.FirstName + " " + curUser.LastName;
        webTab.Tabs.GetTab(1).ContentPane.TargetUrl = "./user/user_notifications.aspx?u=" + selUserId;
        webTab.Tabs.GetTab(2).ContentPane.TargetUrl = "./user/user_localizations.aspx?u=" + selUserId;
        webTab.Tabs.GetTab(3).ContentPane.TargetUrl = "./user/user_PLs.aspx?u=" + selUserId;
				if (curUser.Cultures != null)
          webTab.Tabs.GetTab(2).Text = "Catalogs(" + curUser.Cultures.Count.ToString() + ")";
      }
      webTab.Visible = true;
      webTab.SelectedTabIndex = 0;

    }

    protected void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      Trace.Warn("Users", "     ->dg_InitializeRow");
      Infragistics.WebUI.UltraWebGrid.UltraGridCell c;
      c = e.Row.Cells[_LastLoginPos];
      if (c.Value != null && c.Value!=DBNull.Value)
      {
        c.Value = HyperCatalog.Shared.SessionState.User.FormatUtcDate(Convert.ToDateTime(c.Value));
      }
			if (txtFilter.Text != string.Empty)
			{
				Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;
				UITools.HiglightGridRowFilter(ref r, txtFilter.Text);
			}
    }

    protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      if (btn == "export")
      {
        Utils.ExportToExcel(dg, "Users", "Users");
      }
      if (btn == "add")
      {
        if (SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_USERS))
          UpdateDataEdit("-1");
      }
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

      //  private void cbFilterLock_CheckedChanged(object sender, System.EventArgs e)
      //  {
      //SessionState.tmPageIndexExpression = cbFilterLock.Checked ? "1" : "0";
      //if (cbFilterLock.Checked)
      //      {
      //  using (UserList users = HyperCatalog.Business.User.GetAll(" AttemptsLeft=0"))
      //  {
      //    lbTitle.Text = "User list";
      //    if (users != null)
      //    {
      //      if (users.Count > 0)
      //        users.Sort("IsActive DESC");

      //      dg.DataSource = users;

      //      Utils.InitGridSort(ref dg);
      //      dg.DataBind();

      //      dg.Columns.FromKey("LastLogin").Format = SessionState.User.FormatDate + ' ' + SessionState.User.FormatTime;

      //      webTab.Visible = false;
      //      panelGrid.Visible = true;
      //    }
      //  }
      //      }
      //      else
      //      {
      //          UpdateDataView();
      //      }
      //  }
  }
}