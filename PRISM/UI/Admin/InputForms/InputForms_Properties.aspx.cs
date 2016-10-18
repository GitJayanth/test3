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
using System.Data.SqlClient;
using HyperCatalog.Shared;
using HyperCatalog.Business;
#endregion

#region History
// - Add input form type (CHARENSOL Mickael 28/10/2005)
// - Add Clone input form function (REY Pervenche 23/06/2006)
#endregion

namespace HyperCatalog.UI.Inputforms
{
  /// <summary>
  /// Display properties of input form
  ///		--> Return to the list of input form
  ///		--> Save new or modified input form
  ///		--> Delete input form (if it is used)
  /// </summary>
  public partial class InputForms_Properties : HCPage
  {
    #region Declarations
    private HyperCatalog.Business.InputForm ifObj = null;
    private System.Int32 ifId;
    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
    {
      #region Capabilities
      if (SessionState.User.IsReadOnly)
      {
        uwToolbar.Items.FromKeyButton("Save").Enabled = false;
        uwToolbar.Items.FromKeyButton("Delete").Enabled = false;
        uwToolbar.Items.FromKeyButton("Clone").Enabled = false;
      }

      if (!SessionState.User.HasCapability(HyperCatalog.Business.CapabilitiesEnum.MANAGE_CARTOGRAPHY))
      {
        UITools.HideToolBarButton(uwToolbar, "Save");
        UITools.HideToolBarSeparator(uwToolbar, "SaveSep");
        UITools.HideToolBarButton(uwToolbar, "Delete");
        UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
        UITools.HideToolBarButton(uwToolbar, "Clone");
        UITools.HideToolBarSeparator(uwToolbar, "CloneSep");
      }
      #endregion

      ifId = Convert.ToInt32(Request["i"]);
      if (!Page.IsPostBack)
      {
        ifObj = HyperCatalog.Business.InputForm.GetByKey(ifId);
        UpdateDataEdit();
      }
    }

    protected void UpdateDataEdit()
    {
      using (HyperCatalog.Business.InputFormTypeList inputFormTypes = HyperCatalog.Business.InputFormType.GetAll())
      {
        ddlInputFormType.DataSource = inputFormTypes;
        ddlInputFormType.DataBind();

        if (ifId == -1)
        {
          // *************************************************************************
          // Provide an empty screen to create a new Input form
          // *************************************************************************
          panelId.Visible = false;
          UITools.HideToolBarButton(uwToolbar, "Delete");
          UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
          UITools.HideToolBarButton(uwToolbar, "Clone");
          UITools.HideToolBarSeparator(uwToolbar, "CloneSep");
        }
        else
        {
          if (ifObj != null)
          {
            ddlInputFormType.SelectedValue = ifObj.InputFormTypeCode.ToString();
            txtInputFormId.Text = ifObj.Id.ToString();
            txtInputFormName.Text = ifObj.Name;
            txtInputFormShortName.Text = ifObj.ShortName;
            txtDescription.Text = ifObj.Description;
            cbIsActive.Checked = ifObj.IsActive;
            string creatorEmail = ifObj.Creator.Email;
            string createDate = ifObj.CreateDate.ToString();
            string creatorName = ifObj.Creator.FullName;
            hlCreator.NavigateUrl = "mailto:" + UITools.GetDisplayEmail(creatorEmail);
            hlCreator.Text = "Created by " + creatorName + " on " + createDate + "<br/><br/>";
            panelId.Visible = true;
          }
        }
      }
    }

    // Save the new or updated Input form
    private void Save()
    {
      ifObj = HyperCatalog.Business.InputForm.GetByKey(ifId);
      if (ifObj == null)
        ifObj = new HyperCatalog.Business.InputForm(-1, string.Empty, string.Empty, string.Empty, SessionState.User.Id,
          null, SessionState.User.Id,
          null, ' ', false);
      ifObj.Name = txtInputFormName.Text;
      ifObj.ShortName = txtInputFormShortName.Text;
      ifObj.Description = txtDescription.Text;
      ifObj.InputFormTypeCode = Convert.ToChar(ddlInputFormType.SelectedValue);
      // If the current item is not null and the inputform active status is changed, force current item to reload input forms
      if (ifObj.IsActive != cbIsActive.Checked && SessionState.CurrentItem != null)
      {
        SessionState.CurrentItem.InputForms.Clear();
        SessionState.CurrentItem.InputForms = null;
      }
      ifObj.IsActive = cbIsActive.Checked;
      if (!ifObj.Save())
      {
        SessionState.ClearAppInputForms();
        lbError.CssClass = "hc_error";
        lbError.Text = HyperCatalog.Business.InputForm.LastError;
        lbError.Visible = true;
      }
      else
      {
        if (!panelId.Visible)
        {
          Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "clientScript", "<script>Redirect(" + ifObj.Id + ");</script>");
        }
        else
        {
          lbError.Text = "Data saved!";
          lbError.CssClass = "hc_success";
          lbError.Visible = true;
        }
      }
    }
    // Delete current Input form
    private void Delete()
    {
      if (panelId.Visible)
      {
        ifObj = HyperCatalog.Business.InputForm.GetByKey(ifId);
        if (ifObj != null)
        {
          if (!ifObj.Delete(HyperCatalog.Shared.SessionState.User.Id))
          {
            SessionState.ClearAppInputForms();
            lbError.CssClass = "hc_error";
            lbError.Text = HyperCatalog.Business.InputForm.LastError;
            lbError.Visible = true;
          }
          else
          {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "clientScript", "<script>back();</script>");
          }
        }
      }
    }
    // Clone current Input form
    private void Clone()
    {
      ifObj = HyperCatalog.Business.InputForm.GetByKey(ifId);
      if (ifObj != null)
      {
        HyperCatalog.Business.InputForm newIFObj = ifObj.Clone();
        if (newIFObj == null)
        {
          lbError.CssClass = "hc_error";
          lbError.Text = HyperCatalog.Business.InputForm.LastError;
          lbError.Visible = true;
        }
        else
        {
          Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ReloadParent", "<SCRIPT>Redirect(" + newIFObj.Id + ")</SCRIPT>");
        }
      }
    }

    protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      Trace.Warn("uwToolbar_ButtonClicked with btn = " + btn);
      if (btn == "save")
      {
        Save();
      }
      if (btn == "delete")
      {
        Delete();
      }
      if (btn == "clone")
      {
        Clone();
      }

    }
  }
}
