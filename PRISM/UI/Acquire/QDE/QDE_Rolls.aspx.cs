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

namespace HyperCatalog.UI.Acquire.QDE
{
  /// <summary>
  /// Popup used for roll creation or roll deletion
  /// </summary>
  public partial class QDE_Rolls : HCPage
  {
    #region Declarations
    protected System.Web.UI.WebControls.RequiredFieldValidator Requiredfieldvalidator;
    protected System.Web.UI.WebControls.Panel pnlCreation;
    protected System.Web.UI.WebControls.Panel pnlDeletion;

    private Item item = null;
    protected System.Web.UI.WebControls.Label lbAreYouSure;
    private string action = string.Empty;
    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
    {
      if (SessionState.User.IsReadOnly)
      {
        uwToolbar.Items.FromKeyButton("Apply").Enabled = false;
      }

      try
      {
        Int64 itemId = -1;
        if (Request["a"] != null)
          action = Request["a"].ToString();
        QDEUtils.GetItemIdFromRequest();
        itemId = SessionState.CurrentItem.Id;
        QDEUtils.UpdateCultureCodeFromRequest();

        if (SessionState.User.HasItemInScope(itemId))
        {
          item = SessionState.CurrentItem;
          if (item != null)
          {
              lbError.Visible = false;
              if (HyperCatalog.Business.ApplicationParameter.IOHeirachyStatus() == 0)
              {

                  if (!Page.IsPostBack)
                  {
                      lbTitle.Text = item.FullName + " - Roll";
                      if (action.Equals("create"))
                      {
                          DisplayForCreation();
                      }
                      else // action="modify"
                      {
                          DisplayForModification();
                      }
                  }

                  System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                  ci.DateTimeFormat.ShortDatePattern = SessionState.User.FormatDate;
                  ci.DateTimeFormat.LongDatePattern = SessionState.User.FormatDate;
                  wdcReplacementDate.CalendarLayout.Culture = ci;
                  wdcReplacementDate.MinDate = DateTime.UtcNow.AddDays(1);
              }

              else
              {
                  UITools.JsCloseWin("The Product Hierarchy Refresh job is in progress.  User cannot perform this action.  Please try once the job completes!");

              }
          }
          else
              UITools.DenyAccess(DenyMode.Popup);
        }
        else
          UITools.DenyAccess(DenyMode.Popup);
      }
      catch
      {
        UITools.JsCloseWin();
      }
    }

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

    private void DisplayForCreation()
    {
      lbReplacementDate.Text = "Replacement date";
      lbComment.Text = "Comment";

      hlCreator.Visible = false;
      wdcReplacementDate.Value = DateTime.Now.AddDays(1);
    }

    private void DisplayForModification()
    {
      lbReplacementDate.Text = "Replacement date";
      lbComment.Text = "Comment";

      if ((item != null) && (item.ItemRoll != null))
      {
        Roll roll = item.ItemRoll;
        hlCreator.Text = "Created on " + roll.CreateDate.Value.ToString(SessionState.User.FormatDate) + " by <a href='mailto:" + UITools.GetDisplayEmail(roll.Creator.Email) + "?subject=Roll Item [#" + roll.Id.ToString() + "]' title='Send Mail'>" + roll.Creator.FullName + " [" + roll.Creator.Organization.Name + "]</a>";
        hlCreator.Visible = true;
        wdcReplacementDate.Value = roll.ReplacementDate.Value;
        txtComment.Text = roll.Comment;
      }
    }

    private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      if (btn.Equals("apply"))
      {
        SaveRoll();
      }
    }

    private void SaveRoll()
    {
      DateTime dt = Convert.ToDateTime(wdcReplacementDate.Value);
      if (dt.CompareTo(DateTime.Now) < 0)
      {
        // dt is inferior to today
        lbError.CssClass = "hc_error";
        lbError.Text = "Roll date must occur after today";
        lbError.Visible = true;
      }
      else
      {
        Roll roll = null;
        if (action.Equals("create"))
        {
          roll = new Roll(item.Id, dt, txtComment.Text);
        }
        else
        {
          roll = item.ItemRoll;
          roll.Comment = txtComment.Text;
          roll.ReplacementDate = dt;
        }

        if (roll != null)
        {
          if (roll.Save(SessionState.User.Id))
          {
            Trace.Warn("Creating new Item - Updating User Session");
            SessionState.CurrentItem = null;
            SessionState.User.LastVisitedItem = item.Id;

            Trace.Warn("Reloading Page");
            if (action.Equals("create"))
            {
              Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "SaveRoll", "<script>UpdateContentCreation(" + item.Id + ");</script>");
            }
            else
            {
              Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "SaveRoll", "<script>UpdateContentModification(" + item.Id + ");</script>");
            }
          }
          else
          {
            lbError.CssClass = "hc_error";
            lbError.Text = Item.LastError;
            lbError.Visible = true;
          }
        }
      }
    }
  }
}
