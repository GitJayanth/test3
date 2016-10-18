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
using HyperComponents.Data.dbAccess;
using Infragistics.WebUI.UltraWebNavigator;
#endregion

#region History
// Add content field (CHARENSOL Mickael 13/10/2005)	  
// Add Label field (REY Pervenche 18/10/2005)	  
// Add capabilities (CHARENSOL Mickael 24/10/2005)
// Update "save" and "delete" buttons are "read only" (CHARENSOL Mickael 24/10/2005)
// Update default value for container and tooltip for inheritance method (CHARENSOL Mickael 08/11/05)
// Add WWXPath field (CHARENSOL Mickael 21/11/2005)
// Add MMD,MECD,CDM field (Paul 13/04/2007)
#endregion

/// <summary>
/// Display container properties
///		--> Save new or update container
///		--> Delete container (if it is not used in input form)
///		--> Return to the list of container
/// </summary>
public partial class container_Properties : HCPage
{
  #region Declarations
  private HyperCatalog.Business.Container container;
  private int containerId;
  #endregion

  protected void Page_Load(object sender, System.EventArgs e)
  {
    if (SessionState.User.IsReadOnly)
    {
      uwToolbar.Items.FromKeyButton("Save").Enabled = false;
      uwToolbar.Items.FromKeyButton("Delete").Enabled = false;
    }

	if (Request["c"] != null)
        containerId = Convert.ToInt32(Request["c"]);

      if (SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_ITEM_PDB_REPUBLISH) && containerId != -999)

      {
          uwToolbar.Items.FromKeyButton("RePublish").Visible = true;
      }

      else
      {
          uwToolbar.Items.FromKeyButton("RePublish").Visible = false;
          UITools.HideToolBarSeparator(uwToolbar, "RePublishSep");
      }


    if (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_DICTIONARY))
    {
      UITools.HideToolBarButton(uwToolbar, "Save");
      UITools.HideToolBarSeparator(uwToolbar, "SaveSep");
      UITools.HideToolBarButton(uwToolbar, "Delete");
      UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
    }

    try
    {
      
      using (container = HyperCatalog.Business.Container.GetByKey(containerId))
      {

        if (Request["u"] != null && Request["u"].ToString() == "1")
        {
          lbError.CssClass = "hc_success";
          lbError.Text = "Data saved!";
          lbError.Visible = true;
        }

        ShowHintInfo();

        if (!Page.IsPostBack)
        {
          UpdateDataEdit();
        }
      }
    }
    catch (Exception ex)
    {
      //				Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"clientScript", "<script>UpdateTabs(-1);</script>"); 
      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "clientScript", "<script>alert('" + UITools.CleanJSString(ex.ToString()) + "')</script>");
    }
  }

  protected void UpdateDataEdit()
  {
    #region Load ContainerGroups drop down
    ddlContainerGroups.Items.Clear();
    if (container == null)
    {
      ddlContainerGroups.Items.Add(new ListItem("--> Please choose a group <--", "-1"));
    }
    for (int i = 0; i < SessionState.AppContainersGroups.Count; i++)
    {
      if (SessionState.AppContainersGroups[i].SubGroupCount == 0)
      {
        ddlContainerGroups.Items.Add(new ListItem(SessionState.AppContainersGroups[i].Path + SessionState.AppContainersGroups[i].Name, SessionState.AppContainersGroups[i].Id.ToString()));
      }
    }
    #endregion

    dlDataType.DataSource = SessionState.AppDataTypes;
    dlDataType.DataBind();
    dlContainerType.DataSource = SessionState.AppContainerTypes;
    dlContainerType.DataBind();
    dlLookupField.DataSource = SessionState.AppLookupGroups;
    dlLookupField.DataBind();
    dlInheritanceMethod.DataSource = SessionState.AppInheritanceMethods;
    dlInheritanceMethod.DataBind();
    using (Database dbObj = Utils.GetMainDB())
    {
      ddlSegments.DataSource = SessionState.AppContainerSegments;
      ddlSegments.DataTextField = "SegmentName";
      ddlSegments.DataValueField = "SegmentId";
      ddlSegments.DataBind();

      dlLookupField.Items.Insert(0, new ListItem("None", ""));
      if (container != null)
      {
        ddlContainerGroups.SelectedValue = container.GroupId.ToString();
        txtContainerId.Text = container.Id.ToString();
        txtTag.Text = container.Tag;
        txtContainerName.Text = container.Name;
        txtDefinition.Text = container.Definition;
        txtEntryRule.Text = container.EntryRule;
        txtMaxLength.Value = container.MaxLength;
        txtSample.Text = container.Sample;
        cbLabel.Value = container.Label;
        tbWWXPath.Text = container.WWXPath;
        if (container.Creator != null)
        {
          hlCreator.NavigateUrl = "mailto:" + UITools.GetDisplayEmail(container.Creator.Email);
          hlCreator.Text = "Created by " + container.Creator.FullName + " on " + SessionState.User.FormatUtcDate(container.CreateDate.Value, true, SessionState.User.FormatDate + ' ' + SessionState.User.FormatTime) + "<br/><br/>";
          if (container.Modifier != null && container.ModifyDate.HasValue)
          {
            hlModifier.NavigateUrl = "mailto:" + UITools.GetDisplayEmail(container.Modifier.Email);
            hlModifier.Text = "Modified by " + container.Modifier.FullName + " on " + SessionState.User.FormatUtcDate(container.ModifyDate.Value, true, SessionState.User.FormatDate + ' ' + SessionState.User.FormatTime) + "<br/><br/>";
          }
        }
        cbPublishable.Checked = container.Publishable;
        cbRegionalizable.Checked = container.Regionalizable;
        cbLocalizable.Checked = container.Localizable;
        cbTranslatable.Checked = container.Translatable;
        cbReadOnly.Checked = container.ReadOnly;
        cbKeepIfObsolete.Checked = container.KeepIfObsolete;
        cbMECD.Checked = container.Mecd;
        cbMMD.Checked = container.Mmd;
        cbCDM.Checked = container.Cdm;
        ddlSegments.SelectedValue = container.SegmentId.ToString();
        txtSort.Value = container.Sort;
        txtInputMask.Text = container.InputMask;
        if (container.Lookup != null)
        {
          dlLookupField.SelectedValue = container.LookupId.ToString();
        }
        using (InputFormContainerList IFCList = InputFormContainer.GetAll("ContainerId = " + containerId.ToString()))
        {
          if (IFCList != null)
          {
            foreach (InputFormContainer ifc in IFCList)
            {
              if (ifc.Type != InputFormContainerType.Normal)
              {
                dlLookupField.SelectedIndex = 0;
                dlLookupField.Enabled = false;
                break;
              }
            }
          }
        }

        dlDataType.SelectedValue = container.DataTypeCode.ToString();
        dlContainerType.SelectedValue = container.ContainerTypeCode.ToString();
        dlInheritanceMethod.SelectedValue = container.InheritanceMethodId.ToString();

        panelId.Visible = true;
        ViewState["action"] = "update";

        if (SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_DICTIONARY))
        {
          UITools.ShowToolBarButton(uwToolbar, "Delete");
          UITools.ShowToolBarSeparator(uwToolbar, "DeleteSep");
        }

        // Update field Enable/Disable
        if (cbReadOnly.Checked)
          cbTranslatable.Enabled = cbRegionalizable.Enabled = cbLocalizable.Enabled = false;

        // all content
        if (dbObj != null)
        {
          int r = dbObj.RunSPReturnInteger("dbo._Container_HasNotContent",
            new SqlParameter("@ContainerId", container.Id),
            new SqlParameter("@CultureTypeId", (int)CultureType.Master));
          if (r < 0)
          {
            UITools.HideToolBarButton(uwToolbar, "Delete");
            UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");

            txtTag.Enabled = false;
            dlContainerType.Enabled = false;
            dlInheritanceMethod.Enabled = false;
            dlDataType.Enabled = false;
            txtInputMask.Enabled = false;

            if (txtMaxLength.ValueDouble > 0)
              txtMaxLength.MinValue = txtMaxLength.ValueDouble;
            else
              txtMaxLength.Enabled = false;
          }

          // regionalized content
          r = dbObj.RunSPReturnInteger("dbo._Container_HasNotContent",
            new SqlParameter("@ContainerId", container.Id),
            new SqlParameter("@CultureTypeId", (int)CultureType.Regionale));
          if (r < 0 && cbRegionalizable.Checked)
          {
            cbRegionalizable.Enabled = false;
          }

          // localized content (or translated content)
          r = dbObj.RunSPReturnInteger("dbo._Container_HasNotContent",
            new SqlParameter("@ContainerId", container.Id),
            new SqlParameter("@CultureTypeId", (int)CultureType.Locale));
          if (r < 0)
          {
            if (cbTranslatable.Checked)
              cbTranslatable.Enabled = false;
            if (cbLocalizable.Checked)
              cbLocalizable.Enabled = false;
          }
        }
      }
      else
      {
        ViewState["action"] = "create";
        ddlContainerGroups.SelectedIndex = 0;
        dlDataType.SelectedValue = "T";
        txtMaxLength.Value = 0;
        hlCreator.Text = "";
        dlLookupField.Items.Insert(0, new ListItem("", ""));
        cbLocalizable.Checked = false;
        cbRegionalizable.Checked = false;
        cbPublishable.Checked = false;
        cbTranslatable.Checked = true;
        txtSort.Text = "0";
        dlContainerType.SelectedValue = "*";
        dlInheritanceMethod.SelectedValue = "1";
        tbWWXPath.Text = string.Empty;
        cbKeepIfObsolete.Checked = false;
        ddlSegments.SelectedIndex = 0;
        cbReadOnly.Checked = false;
        cbMECD.Checked = false;
        cbMMD.Checked = false;
        cbCDM.Checked = false;

        UITools.HideToolBarButton(uwToolbar, "Delete");
        UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
      }
    }
    //ChiantiUpdate();
  }

  private void Save()
  {
    txtLabel.Text = cbLabel.Value;
    if ((container != null) || (container == null && ddlContainerGroups.SelectedIndex != 0))
    {
      Page.Validate();
      if (Page.IsValid)
      {
        // Retrieve the new text in the templated columns
        // Prepare the command text
        lbError.Text = "";
        int lookupValueId = -1;
        bool isContainerTypeChanged = false;
        if (dlLookupField.SelectedIndex > 0)
        {
          lookupValueId = Convert.ToInt32(dlLookupField.SelectedValue);
        }

        if (ViewState["action"].ToString() == "update")
        {
          container.Tag = txtTag.Text;
          container.Name = txtContainerName.Text;
          container.Definition = txtDefinition.Text;
          container.EntryRule = txtEntryRule.Text;
          container.DataTypeCode = Convert.ToChar(dlDataType.SelectedValue);
          container.MaxLength = Convert.ToInt32(txtMaxLength.Value);
          container.ValidationMask = string.Empty;
          container.Sample = txtSample.Text;
          container.Mecd = cbMECD.Checked;
          container.Mmd  = cbMMD.Checked;
          container.Cdm = cbCDM.Checked;
          container.GroupId = Convert.ToInt32(ddlContainerGroups.SelectedValue);
          container.InputMask = txtInputMask.Text;
          container.LookupId = lookupValueId;
          container.DataTypeCode = Convert.ToChar(dlDataType.SelectedValue);

          // Note: When the container type is changed from or to features, then we will reload the complete page
          if (container.ContainerTypeCode != Convert.ToChar(dlContainerType.SelectedValue) && (dlContainerType.SelectedValue == "F" || container.ContainerTypeCode.ToString() == "F"))
          {
            isContainerTypeChanged = true;
          }
          container.ContainerTypeCode = Convert.ToChar(dlContainerType.SelectedValue);
          container.Translatable = cbTranslatable.Checked;
          container.ModifierId = SessionState.User.Id;
          container.InheritanceMethodId = Convert.ToInt32(dlInheritanceMethod.SelectedValue);
          container.Publishable = cbPublishable.Checked;
          container.Regionalizable = cbRegionalizable.Checked;
          container.Localizable = cbLocalizable.Checked;
          container.ReadOnly = cbReadOnly.Checked;
          container.Sort = Convert.ToInt32(txtSort.Value);
          container.Label = cbLabel.Value;
          container.LabelId = -1; // The label is Always recreate
          container.WWXPath = tbWWXPath.Text;
          container.KeepIfObsolete = cbKeepIfObsolete.Checked;
          container.SegmentId = Convert.ToInt32(ddlSegments.SelectedValue);
        }
        else
        {
          container = new HyperCatalog.Business.Container(-1,
            txtTag.Text,
            txtContainerName.Text,
            txtDefinition.Text,
            txtEntryRule.Text,
            string.Empty,
            txtSample.Text,
            txtInputMask.Text,
            Convert.ToChar(dlDataType.SelectedValue),
            cbTranslatable.Checked,
            Convert.ToChar(dlContainerType.SelectedValue),
            Convert.ToInt32(txtMaxLength.Value),
            lookupValueId,
            Convert.ToInt32(txtSort.Value),
            SessionState.User.Id,
            -1,
            Convert.ToInt32(ddlContainerGroups.SelectedValue),
            DateTime.UtcNow,
            null,
            cbPublishable.Checked, false,
            Convert.ToInt32(dlInheritanceMethod.SelectedValue),
            cbRegionalizable.Checked,
            cbLocalizable.Checked,
            cbReadOnly.Checked,
            cbLabel.Value.ToString(),
            -1,
            tbWWXPath.Text,
            cbKeepIfObsolete.Checked,
            Convert.ToInt32(ddlSegments.SelectedValue),
            dlDataType.Text,
            dlContainerType.Text,
            cbMECD.Checked,
            cbMMD.Checked,
            cbCDM.Checked
          );
        }
        if (container.Save())
        {
          SessionState.ClearAppContainers();
          SessionState.ClearAppContainerGroups();
          if (txtMaxLength.ValueDouble > 0)
            txtMaxLength.MinValue = txtMaxLength.ValueDouble;

          if (ViewState["action"].ToString() == "create")
          {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "clientScript1", "<script>UpdateTabs(" + container.Id + ");</script>");
          }
          else if (isContainerTypeChanged)
          {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "clientScript2", "<script>UpdateTabsAfterUpdate(" + container.Id + ");</script>");
          }
          else
          {
            container = HyperCatalog.Business.Container.GetByKey(containerId);
            lbError.CssClass = "hc_success";
            lbError.Text = "Data saved!";
            lbError.Visible = true;

            if (container.Modifier != null && container.ModifyDate.HasValue)
            {
              hlModifier.NavigateUrl = "mailto:" + UITools.GetDisplayEmail(container.Modifier.Email);
              hlModifier.Text = "Modified by " + container.Modifier.FullName + " on " + SessionState.User.FormatUtcDate(container.ModifyDate.Value, true, SessionState.User.FormatDate + ' ' + SessionState.User.FormatTime) + "<br/><br/>";
              hlModifier.Visible = true;
            }
          }
        }
        else
        {
          lbError.CssClass = "hc_error";
          lbError.Text = HyperCatalog.Business.Container.LastError;
          lbError.Visible = true;
        }
      }
      else
      {
        Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "alertmissing", "<script>alert('Label is mandatory');</script>");
      }
    }
    else
    {
      Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "alertmissing", "<script>alert('Please choose a container group');</script>");
    }
    chianti();
  }

  private void Delete()
  {
    lbError.Visible = false;
    using (HyperCatalog.Business.Container container = HyperCatalog.Business.Container.GetByKey(containerId))
    {
      if (container.Delete(HyperCatalog.Shared.SessionState.User.Id))
      {
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "clientScript", "<script>UpdateTabs(-1)</script>");
      }
      else
      {
        lbError.CssClass = "hc_error";
        lbError.Text = HyperCatalog.Business.Container.LastError;
        lbError.Visible = true;
      }
    }
  }
    private void chianti()
    {
        using (Database dbObj = Utils.GetMainDB())
        {
            if (cbMMD.Checked == true)
            {
                using (DataSet ds = dbObj.RunSPReturnDataSet("_Container_AddUpdForChianti", new SqlParameter("@ContainerId",container.Id), new SqlParameter("@Mmd", 1)))
                {
                    dbObj.CloseConnection();
                }               
          }
          else
          {
              using (DataSet ds = dbObj.RunSPReturnDataSet("_Container_AddUpdForChianti", new SqlParameter("@ContainerId", container.Id), new SqlParameter("@Mmd", 0)))
              {
                  dbObj.CloseConnection();

              }
          }
        }
      }

    private void ChiantiUpdate()
    {
        using (Database dbObj = Utils.GetMainDB())
        {
            if (cbMMD.Checked == true)
            {
                using (DataSet ds = dbObj.RunSPReturnDataSet("_Container_AddUpdForChianti1", new SqlParameter("@Tag", container.Tag), new SqlParameter("@Mmd", 1)))
                {
                    dbObj.CloseConnection();
                }
            }
            else
            {
                using (DataSet ds = dbObj.RunSPReturnDataSet("_Container_AddUpdForChianti1", new SqlParameter("@Tag", container.Tag), new SqlParameter("@Mmd", 0)))
                {
                    dbObj.CloseConnection();

                }
            }
        }
    }


  protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
  {
    string btn = be.Button.Key.ToLower();
    if (btn == "save")
    {
      Save();
    }
    if (btn == "delete")
    {
      Delete();
    }

    if (btn == "republish")
    {
        PDBRefresh();
        lbError.Text = " Container '" + container.Name.ToString() + "' Republished successfully !";
        lbError.Visible = true;
    }
  }
    protected void PDBRefresh()
    {

        const string CST_PARAM_CONTAINERID = "@ContainerId";
        const string CST_PARAM_USERID = "@UserId";
        const string CST_SP_PDBRefresh_NAME = "_Containers _PDBRefresh";
        try
        {
            using (Database db = Utils.GetMainDB())
            {
                SqlParameter[] parameters ={ new SqlParameter(CST_PARAM_USERID, SessionState.User.Id), new SqlParameter(CST_PARAM_CONTAINERID, container.Id.ToString()) };
                int result = db.RunSPReturnInteger(CST_SP_PDBRefresh_NAME, parameters);
            }
        }
        catch (Exception ex)
        {
            const int CRYSTAL_UI_COMPONENT_ID = 2;
            Guid errorGuid = Guid.NewGuid();
            HyperCatalog.EventLogger.EventLogger.AddEventLog(CRYSTAL_UI_COMPONENT_ID, errorGuid, 0, HyperCatalog.EventLogger.Severity.INFORMATION, "Crystal UI PDB Refresh Error", "", "Crystal UI PDB Refresh Error");
        }
  }

  private void ShowHintInfo()
  {
    //<ul> tags omitted to avoid wasting space
    string s = "<table width=\"250px\"><tr><td>";
    foreach (InheritanceMethod m in SessionState.AppInheritanceMethods)
    {
      s += "<li><b>" + m.Name + ":</b> " + m.Description.Replace("'", "\'").Replace("\r", "").Replace("\n", "") + "</li>";
    }
    s += "</td></tr></table>";
    Page.ClientScript.RegisterStartupScript(this.GetType(), "strInheritanceMethodInfo", "<script>strInheritanceMethodInfo='" + s + "';</script>");
  }

  protected void cbReadOnly_CheckedChanged(object sender, System.EventArgs e)
  {
    if (cbReadOnly.Checked)
    {
      cbTranslatable.Enabled = cbRegionalizable.Enabled = cbLocalizable.Enabled = false;
      cbTranslatable.Checked = cbRegionalizable.Checked = cbLocalizable.Checked = false;
    }
    else
    {
      cbTranslatable.Enabled = cbRegionalizable.Enabled = cbLocalizable.Enabled = true;
    }
  }
  protected void dlDataType_SelectedIndexChanged(object sender, EventArgs e)
  {
    if (dlDataType.SelectedValue == "P")// Photo
    {
      dlLookupField.Enabled = false;
      txtInputMask.Enabled = false;
      txtMaxLength.Enabled = false;
      lbLookupField.Visible = lbInputMask.Visible = lbMaxLength.Visible = true;
    }
    else
    {
      lbLookupField.Visible = lbInputMask.Visible = lbMaxLength.Visible = false;
      txtInputMask.Enabled = dlLookupField.Enabled = true;
      using (InputFormContainerList IFCList = InputFormContainer.GetAll("ContainerId = " + containerId.ToString()))
      {
        if (IFCList != null)
        {
          foreach (InputFormContainer ifc in IFCList)
          {
            if (ifc.Type != InputFormContainerType.Normal)
            {
              dlLookupField.SelectedIndex = 0;
              dlLookupField.Enabled = false;
              break;
            }
          }
        }
      }

      if (container != null)
      {
        // all content
        using (Database db = Utils.GetMainDB())
        {
          if (db != null)
          {
            txtMaxLength.Enabled = true;
            int r = db.RunSPReturnInteger("dbo._Container_HasNotContent",
              new SqlParameter("@ContainerId", container.Id),
              new SqlParameter("@CultureTypeId", (int)CultureType.Master));
            if (r < 0)
            {
              UITools.HideToolBarButton(uwToolbar, "Delete");
              UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");

              txtTag.Enabled = false;
              dlContainerType.Enabled = false;
              dlInheritanceMethod.Enabled = false;
              dlDataType.Enabled = false;
              if (txtMaxLength.ValueDouble > 0)
                txtMaxLength.MinValue = txtMaxLength.ValueDouble;
              else
                txtMaxLength.Enabled = false;
            }
            else
            {
              txtMaxLength.Enabled = true;
            }
          }
        }
      }
    }

  }
}
