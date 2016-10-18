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
using HyperCatalog.Shared;
using Infragistics.WebUI.UltraWebGrid;
using HyperCatalog.Business;
#endregion



/// <summary>
/// Display notification lsit of selected user
///		--> Return to the user list
///		--> Select notifications
///		--> Save modification
/// </summary>
public partial class user_notifications : HCPage
{
  #region Declarations
  private int userId = -1;
  protected string[] delays = new string[] { "1 week", "2 weeks", "3 weeks", "4 weeks" };
  protected HyperCatalog.Business.CollectionView cv;
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
    #region Capabilities
    if (SessionState.User.IsReadOnly)
    {
      uwToolbar.Items.FromKeyButton("Save").Enabled = false;
    }
    if (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_USERS))
    {
      UITools.HideToolBarButton(uwToolbar, "Save");
      UITools.HideToolBarSeparator(uwToolbar, "SaveSep");
    }
    #endregion

    try
    {      
      lbError.CssClass = "hc_error";
      lbError.Visible = false;
      if (!Page.IsPostBack)
      {
        BindLists();
      }
    }
    catch
    {
      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "clientScript", "<script>back();</script>");
    }
  }

  #region Data load & bind
  private void BindLists()
  {
    SessionState.EditedUser.ClearNotifications();
    cv = new HyperCatalog.Business.CollectionView(SessionState.EditedUser.Notifications);
    dg.DataSource = SessionState.EditedUser.Role.Notifications;
    dg.DataBind();
    Utils.InitGridSort(ref dg, false);

    if (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_USERS) || SessionState.User.IsReadOnly)
    {
      TemplatedColumn colHeader = (TemplatedColumn)dg.Columns.FromKey("Select");
      HeaderItem cellItemHeader = colHeader.HeaderItem;
      CheckBox cbHeader = (CheckBox)cellItemHeader.FindControl("g_ca");
      cbHeader.Enabled = false;

      foreach (UltraGridRow r in dg.Rows)
      {
        TemplatedColumn col = (TemplatedColumn)r.Cells.FromKey("Select").Column;
        CellItem cellItem = (CellItem)col.CellItems[r.Index];
        CheckBox cb = (CheckBox)cellItem.FindControl("g_sd");
        cb.Enabled = false;
      }
    }
  }
  protected void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
  {
    try
    {
      cv.ApplyFilter("Id", Convert.ToInt32(e.Row.DataKey), HyperCatalog.Business.CollectionView.FilterOperand.Equals);

      TemplatedColumn col = (TemplatedColumn)e.Row.Cells.FromKey("Delay").Column;
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

      bool allowNotif = (bool)e.Row.Cells.FromKey("AllowNotificationInAdvance").Value;
      bool allowInAdv = (allowNotif && SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_USERS));
      if (allowNotif)
      {
        ddDelay.SelectedIndex = obj != null ? Convert.ToInt32(obj.Delay) : 0;
      }
      else
      {
        ddDelay.Items.Clear();
        ddDelay.Items.Add(new ListItem("Instant", "-1"));
      }
      ddDelay.Enabled = allowInAdv;
    }
    catch (Exception exc)
    {
      e.Row.Cells.FromKey("Delay").Text = exc.ToString();
    }
    finally
    {
      cv.RemoveFilter();
    }
  }
  #endregion

  private void Save()
  {
    if (dg.Rows.Count > 0)
    {
      bool success = true;
      success = SessionState.EditedUser.Notifications.RemoveAll();
      TemplatedColumn col = (TemplatedColumn)dg.Rows[0].Cells.FromKey("Select").Column;
      foreach (CellItem cell in col.CellItems)
      {
        TemplatedColumn colDelay = (TemplatedColumn)cell.Cell.Row.Cells.FromKey("Delay").Column;
        CellItem cellItemDelay = (CellItem)colDelay.CellItems[cell.Cell.Row.Index];
        CheckBox cb = (CheckBox)cell.FindControl("g_sd");
        DropDownList ddDelay = (DropDownList)cellItemDelay.FindControl("ddDelay");
        if (cb.Checked)
          success = success && SessionState.EditedUser.Notifications.Add(Convert.ToInt32(cell.Cell.Row.DataKey), ddDelay.Enabled ? Convert.ToInt32(ddDelay.SelectedValue) : 0);
      }

      SessionState.EditedUser.ClearNotifications();
      BindLists();
      if (success)
      {
        if (SessionState.EditedUser.Id == SessionState.User.Id)
        {
          SessionState.User = SessionState.EditedUser;
        }
        lbError.Text = "Data saved!";
        lbError.CssClass = "hc_success";
        lbError.Visible = true;
      }
      else
      {
        lbError.Text = "Error, update failed" + HyperCatalog.Business.UserNotification.LastError;
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

