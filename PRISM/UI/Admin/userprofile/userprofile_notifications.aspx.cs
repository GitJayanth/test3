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
using HyperCatalog.Shared;
using Infragistics.WebUI.UltraWebGrid;

	/// <summary>
	/// Description résumée de userprogile_notification.
	/// </summary>
	public partial class userprofile_notifications : HCPage
	{
    protected string[] delays = new string[] { "1 week", "2 weeks", "3 weeks", "4 weeks" } ;
    protected HyperCatalog.Business.CollectionView cv;
  
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
      //
      lbError.CssClass = "hc_error";
      lbError.Visible = false;
      if (!Page.IsPostBack)
      {
        BindLists();
      }
    }

    #region Data load & bind
    private void BindLists()
    {
      SessionState.User.ClearNotifications();
      cv = new HyperCatalog.Business.CollectionView(SessionState.User.Notifications);
      SessionState.User.Role.ClearNotifications();
      dg.DataSource = SessionState.User.Role.Notifications;
      Utils.InitGridSort(ref dg, false);
      dg.DataBind();
      dg.DisplayLayout.AllowSortingDefault = AllowSorting.No;
    }
    protected void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      cv.ApplyFilter("Id", Convert.ToInt32(e.Row.DataKey), HyperCatalog.Business.CollectionView.FilterOperand.Equals);

      TemplatedColumn col = (TemplatedColumn)e.Row.Cells.FromKey("nDelay").Column;
      CellItem cellItem = (CellItem)col.CellItems[e.Row.Index];
      DropDownList ddDelay = (DropDownList)cellItem.FindControl("ddDelay");
      for (int i = 0; i < delays.Length; i++)
      {
        ddDelay.Items.Add(new ListItem(delays[i], i.ToString()));
      }

      HyperCatalog.Business.UserNotification obj = cv.Count == 1 ? (HyperCatalog.Business.UserNotification)cv[0] : null;
      col = (TemplatedColumn)e.Row.Cells.FromKey("Select").Column;
      cellItem = (CellItem)col.CellItems[e.Row.Index];
      CheckBox cb = (CheckBox)cellItem.FindControl("g_sd");
      cb.Checked = obj != null;
      bool allowInAdv = (bool)e.Row.Cells.FromKey("AllowNotificationInAdvance").Value;
      if (allowInAdv)
      {
        //the user has checked this notification
        ddDelay.SelectedIndex = obj != null? Convert.ToInt32(obj.Delay):0;
      }
      else
      {
        ddDelay.Items.Clear();
        ddDelay.Items.Add(new ListItem("Instant", "-1"));
      }
      //allow offset editing only if AllowNotificationInAdvance
      ddDelay.Enabled = allowInAdv;
      cv.RemoveFilter();
    }
    #endregion

    private void Save()
    {      
      if (SessionState.User == null)
      {
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"clientScript", "<script>back();</script>");
      }

      if (dg.Rows.Count > 0)
      {
        bool success = true;
        success = SessionState.User.Notifications.RemoveAll();
        TemplatedColumn col = (TemplatedColumn)dg.Rows[0].Cells.FromKey("Select").Column;

        foreach (CellItem cell in col.CellItems)
        {
          TemplatedColumn colDelay = (TemplatedColumn)cell.Cell.Row.Cells.FromKey("nDelay").Column;
          CellItem cellItemDelay = (CellItem)colDelay.CellItems[cell.Cell.Row.Index];
          CheckBox cb = (CheckBox)cell.FindControl("g_sd");
          DropDownList ddDelay = (DropDownList)cellItemDelay.FindControl("ddDelay");
          if (cb.Checked)
            success = success && SessionState.User.Notifications.Add(Convert.ToInt32(cell.Cell.Row.DataKey), ddDelay.Enabled?Convert.ToInt32(ddDelay.SelectedValue): 0);
        }

        SessionState.User.ClearNotifications();
        BindLists();
        if (success)
        {
          lbError.Text = "Data saved!";
          lbError.CssClass = "hc_success";
          lbError.Visible = true;
        }
        else
        {
          lbError.Text = "Error, update failed" + HyperCatalog.Business.UserNotification.LastError ;
          lbError.CssClass = "hc_error";
          lbError.Visible = true;
        }
      }
    }
    private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      if (btn == "save")
      {
        Save();
      }
    }
  }
