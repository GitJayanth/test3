#region uses
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Infragistics.WebUI.UltraWebGrid;
using HyperCatalog.Business;
using HyperCatalog.Shared;
#endregion

public partial class UI_Admin_InputForms_InputForms_ContainersBatchAdd : HCPage
{
  private int inputFormId;
  private InputForm inputForm;
  private CollectionView ifcl
  {
    get{
      if (Cache["ifcl"] == null)
      {
        Cache.Add("ifcl", new CollectionView(InputForm.GetByKey(inputFormId).Containers), null, DateTime.Now.AddMinutes(2), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
      }
      return
        (CollectionView)Cache["ifcl"];
    }
    set
    {
      Cache["ifcl"] = value;
    }
  }

#region Code généré par le Concepteur Web Form
  override protected void OnInit(EventArgs e)
  {
    txtFilter.AutoPostBack = false;
    txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);"); Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "<script defer=true>var txtFilterField;txtFilterField = document.getElementById('" + txtFilter.ClientID + "');</script>");       
    base.OnInit(e);
  }
  #endregion

  protected void Page_Load(object sender, EventArgs e)
  {
    #region Capabilities
    if (SessionState.User.IsReadOnly)
    {
      uwToolbar.Items.FromKeyButton("Add").Enabled = false;
    }

    if (!SessionState.User.HasCapability(HyperCatalog.Business.CapabilitiesEnum.MANAGE_CARTOGRAPHY))
    {
      UITools.HideToolBarSeparator(uwToolbar, "Add");
      UITools.HideToolBarButton(uwToolbar, "CloseSep");
    }
    #endregion
    try
    {
      lbError.Visible = false;
      inputFormId = Convert.ToInt32(Request["f"]);
      if (Request["filter"] != null)
      {
        txtFilter.Text = Request["filter"].ToString();
      }
      else
      {
        SessionState.tmPageIndexExpression = "0";
      }
      if (!Page.IsPostBack)
      {
        UpdateGroups();
        UpdateDataView();
      }
      dg.DisplayLayout.CellClickActionDefault = CellClickAction.Edit;
    }
    catch (Exception ex)
    {
      UITools.JsCloseWin(ex.ToString().Replace(Environment.NewLine, " - ").Replace("'", "\""));
    }
  }

  private void UpdateGroups()
  {
    using (HyperCatalog.Business.ContainerGroupList hcContainers = HyperCatalog.Business.ContainerGroup.GetAll("ContainersCount > 0"))
    {
      hcContainers.Sort("Path ASC, Name ASC");
      ddlGroups.DataTextField = "Name";
      ddlGroups.DataValueField = "Id";
      if (hcContainers != null)
      {
        ddlGroups.Items.Add(new ListItem("-->Choose a group<--", ""));
        ddlGroups.Items.Add(new ListItem("[All Containers]", "-1"));
        for (int i = 0; i < hcContainers.Count - 1; i++)
        {
          ddlGroups.Items.Add(new ListItem(hcContainers[i].Path + hcContainers[i].Name, hcContainers[i].Id.ToString()));
        }
        if (ddlGroups.Items.Count > 0)
        {
          if (Request["filter"] != null)
          {
            ddlGroups.SelectedIndex = Convert.ToInt32(SessionState.tmPageIndexExpression);
          }
          else
          {
            ddlGroups.SelectedIndex = 0;
          }
        }
      }
    }
  }
  private void UpdateDataView()
  {
    if (ddlGroups.SelectedIndex > 0)
    {
      panelExplain.Visible = false;
      dg.Visible = true;
      using (inputForm = InputForm.GetByKey(inputFormId))
      {
        //ContainerList cl = Container.GetAll("GroupId = " + ddlGroups.SelectedValue);
        using (ContainerList cl = inputForm.GetCandidates(Convert.ToInt32(ddlGroups.SelectedValue)))
        {
          cl.Sort("Tag");
          dg.DataSource = cl;
          dg.DataBind();
          Utils.InitGridSort(ref dg, false);
          dg.DisplayLayout.Pager.AllowPaging = false;
          dg.DisplayLayout.AllowSortingDefault = Infragistics.WebUI.UltraWebGrid.AllowSorting.No;
          lbNbContainers.Text = " (#" + cl.Count.ToString() + " containers with [" + inputForm.InputFormTypeCode + "] type)";
        }
        if (ifcl != null)
        {
          ifcl.Dispose();
        }
      }
    }
    else {
      dg.Visible = false;
      panelExplain.Visible = true;      
    }
  }

  protected void ddlGroups_SelectedIndexChanged(object sender, EventArgs e)
  {
    SessionState.tmPageIndexExpression = ddlGroups.SelectedIndex.ToString();
    UpdateDataView();
  }
  protected void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
  {
    //ifcl.ApplyFilter("ContainerId", Convert.ToInt32(e.Row.Cells.FromKey("Id").Text), CollectionView.FilterOperand.Equals);
    e.Row.Cells.FromKey("ContainerName").Text = "[" + e.Row.Cells.FromKey("Tag").Text + "] - " + e.Row.Cells.FromKey("ContainerName").Text;
    TemplatedColumn col = (TemplatedColumn)e.Row.Cells.FromKey("ifcType").Column;
    CellItem cellItem = (CellItem)col.CellItems[e.Row.Index];
    DropDownList ddType = (DropDownList)cellItem.FindControl("ddType");

    if (e.Row.Cells.FromKey("LookupId") != null && e.Row.Cells.FromKey("LookupId").Text == "-1")
    {
      foreach (string t in Enum.GetNames(typeof(InputFormContainerType)))
      {
        ddType.Items.Add(new ListItem(t, t));
      }
    }
    else
    {
      string defaultValue = Enum.GetName(typeof(InputFormContainerType), InputFormContainerType.Normal);
      ddType.Items.Add(new ListItem(defaultValue, defaultValue));
      ddType.Enabled = false;
    }
    if (txtFilter.Text.Trim() != string.Empty)
    {
      Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;
      UITools.HiglightGridRowFilter(ref r, txtFilter.Text, true);
    }
    //if (ifcl.Count != 0)
    //{
    //  InputFormContainer obj = ((InputFormContainer)ifcl[0]);
    //  col = (TemplatedColumn)e.Row.Cells.FromKey("Select").Column;
    //  cellItem = (CellItem)col.CellItems[e.Row.Index];
    //  CheckBox cb = (CheckBox)cellItem.FindControl("g_sd");
    //  cb.Checked = true;
    //  cb.Enabled = false;
    //  col = (TemplatedColumn)e.Row.Cells.FromKey("Mandatory").Column;
    //  cellItem = (CellItem)col.CellItems[e.Row.Index];
    //  cb = (CheckBox)cellItem.FindControl("g_m");
    //  cb.Checked = obj.Mandatory;
    //  cb.Enabled = false;
    //  ddType.SelectedValue = obj.Type.ToString();
    //  e.Row.Cells.FromKey("Comment").Text = obj.Comment.ToString();
    //  e.Row.Cells.FromKey("Comment").AllowEditing = AllowEditing.No;
    //  ddType.Enabled = false;
    //}
    //else
    //{
      ddType.SelectedValue = InputFormContainerType.Normal.ToString();
    //}
    //ifcl.RemoveFilter();
  }

  private void Save()
  {
    lbError.Visible = false;
    Trace.Warn("Save");
    if (dg != null && dg.Rows != null && dg.Rows.Count > 0)
    {
      Trace.Warn("Entering loop");
      TemplatedColumn col = (TemplatedColumn)dg.Rows[0].Cells.FromKey("Select").Column;
      TemplatedColumn colMand = (TemplatedColumn)dg.Rows[0].Cells.FromKey("Mandatory").Column;
      TemplatedColumn colType = (TemplatedColumn)dg.Rows[0].Cells.FromKey("ifcType").Column;

      if (col != null)
      {
        for (int i = 0; i < col.CellItems.Count; i++)
        {
          try
          {
            CellItem cellItem = (CellItem)col.CellItems[i];
            Infragistics.WebUI.UltraWebGrid.UltraGridRow r = cellItem.Cell.Row;
            CheckBox cb = (CheckBox)cellItem.FindControl("g_sd");
            cellItem = (CellItem)colMand.CellItems[i];
            CheckBox chkMandatory = (CheckBox)cellItem.FindControl("g_m");
            cellItem = (CellItem)colType.CellItems[i];
            DropDownList ddType = (DropDownList)cellItem.FindControl("ddType");

            if (cb != null)
            {
              Trace.Warn("Testing for " + dg.Rows[i].Cells.FromKey("Tag").Text + "=>" + cb.Checked.ToString());
              if (cb.Checked && cb.Enabled)
              {
                using (InputFormContainer ifc = new InputFormContainer(-1, Convert.ToInt32(dg.Rows[i].Cells.FromKey("Id").Text),
            inputFormId, ddlGroups.Text,
            0,
            dg.Rows[i].Cells.FromKey("Tag").Text,
            dg.Rows[i].Cells.FromKey("ContainerName").Text,
            (bool)dg.Rows[i].Cells.FromKey("isReg").Value,
            dg.Rows[i].Cells.FromKey("Comment").Value != null ? dg.Rows[i].Cells.FromKey("Comment").Value.ToString() : string.Empty,
            chkMandatory.Checked,
            (InputFormContainerType)Enum.Parse(typeof(InputFormContainerType), ddType.SelectedValue, false),
            SessionState.User.Id,
            DateTime.UtcNow, -1, null))
                {
                  Trace.Warn("  -> Saving for " + dg.Rows[i].Cells.FromKey("Tag").Text);
                  if (!ifc.Save())
                  {
                    // Error
                    lbError.CssClass = "hc_error";
                    lbError.Text += ifc.ContainerName + " -> " + InputFormContainer.LastError + "<br/>" + Environment.NewLine;
                    lbError.Visible = true;
                    Trace.Warn("  -> Error " + ifc.ContainerName + " -> " + InputFormContainer.LastError);
                  }
                  Trace.Warn("  -> Success ");
                }
              }
            }
          }
          catch (Exception ex)
          {
            Trace.Warn("UNEXPECTED Error " + ex.ToString());
          }
        }
        //ifcl.Dispose();
        //Cache.Remove("ifcl");
        UpdateDataView();
        if (!lbError.Visible)
        {
          lbError.CssClass = "hc_success";
          lbError.Text = "Data saved!";
          lbError.Visible = true;
        }
      }
    }
  }
  
  protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
  {
    string btn = be.Button.Key.ToLower();
    Trace.Warn("BUTTON CLICKED = " + btn.ToString());
    if (btn == "add")
    {
      Save();
      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "reload", "<script>ReloadParent()</script>");
    }

  }
}
