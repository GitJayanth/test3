#region Copyright (c)  Hewlett-Packard. All Rights Reserved
/* ---------------------------------------------------------------------*
*        THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE.       *
* --------------------------------------------------------------------- *
 * History Section
 * Date             Name            Description                                         Ref
 * June 02 2009     S.Balakumar     ACQ - 11 (ACQ - 8.20) - Translating Choice Lists    #ACQ8.20
 * --------------------------------------------------------------------- *
*/
#endregion

#region uses
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using Infragistics.WebUI.UltraWebGrid;
using Infragistics.WebUI.WebDataInput;
using HyperCatalog.Shared;
using HyperCatalog.UI.Tools;
#endregion

/// <summary>
/// Display containers attached to the input form
///		- Return to the list of input forms
///		- Attach new container
///		- Remove container in input form
///		- Apply a new sort
///		- Export in Excel
///		- Filter on all fields of the grid
/// </summary>
public partial class InputForms_Containers : HCPage
{
  #region Declarations
	
  public HyperCatalog.Business.InputFormContainerList ifContainerList;
  protected System.Int32 ifId;
  private string _CleanFilter = String.Empty;
  private string currentGroup = string.Empty;
  private int groupCount = 0;
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

  /// <summary>
  /// currentGroup will store the path for the current container
  /// Since we are expecting to receive from the SP a list ordered by
  /// path, each time the path changes, a new title row will be added in the 
  /// grid
  /// </summary>
  protected void Page_Load(object sender, System.EventArgs e)
  {
    #region Capabilities
    if (SessionState.User.IsReadOnly)
    {
      uwToolbar.Items.FromKeyButton("Add").Enabled = false;
      uwToolbar.Items.FromKeyButton("BatchAdd").Enabled = false;
      uwToolbar.Items.FromKeyButton("Save").Enabled = false;
      uwToolbar.Items.FromKeyButton("Delete").Enabled = false;
    }

    if (!SessionState.User.HasCapability(HyperCatalog.Business.CapabilitiesEnum.MANAGE_CARTOGRAPHY))
    {
      UITools.HideToolBarSeparator(uwToolbar, "SaveSep");
      UITools.HideToolBarButton(uwToolbar, "Save");
      UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
      UITools.HideToolBarButton(uwToolbar, "Delete");
    }
    #endregion

    try
    {
      if (Request["i"] != null)
        ifId = Convert.ToInt32(Request["i"]);
      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ifid", "<script>var ifid = '" + ifId + "';</script>");
      if (Request["filter"] != null)
      {
        txtFilter.Text = Request["filter"].ToString();
      }
      if (!Page.IsPostBack)
      {
        UpdateDataView();
      }
      if (Request["action"] == "reload") { UpdateDataView(); }
      if (Request["__EVENTTARGET"] != null) // Check if user is trying to sort a group
      {
        if (Request["__EVENTTARGET"].ToString() == "groupup" || Request["__EVENTTARGET"].ToString() == "groupdown")
        {
          SortGroup(Request["__EVENTTARGET"].ToString() == "groupup", Convert.ToInt32(Request["__EVENTARGUMENT"]));
        }
        else
        {
          if (Request["__EVENTTARGET"].ToString() == "groupdel")
          {
            DeleteGroup(Convert.ToInt32(Request["__EVENTARGUMENT"]));
          }
        }
      }
    }
    catch
    {
      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "clientScript", "<script>back();</script>");
    }
  }

  /// <summary>
  /// Display the list of containers attached to the current input form
  /// </summary>
  private void UpdateDataView()
  {
    lbError.Visible = false;

    if (ifId != -1)
    {
      ifContainerList = HyperCatalog.Business.InputForm.GetByKey(ifId).Containers;
      dg.DataSource = ifContainerList;
      //Utils.InitGridSort(ref dg, false);
      dg.DataBind();

      if (txtFilter.Text.Length == 0)
      {
        EnableIntelligentSortForInputFormContainers(ref dg, Convert.ToInt32(txtSortColPos.Value));
      }

      dg.DisplayLayout.AllowSortingDefault = AllowSorting.No;

      // Refresh tab title 
      UITools.RefreshTab(this.Page, "Containers", ifContainerList.Count);

      if (dg.Rows.Count == 0)
      {
        if (txtFilter.Text.Length > 0)
          lbNoresults.Text = "No record match your search ("+txtFilter.Text+")";

        dg.Visible = false;
        lbNoresults.Visible = true;

        UITools.HideToolBarButton(uwToolbar, "Delete");
        UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");    
        UITools.HideToolBarButton(uwToolbar, "Save");
        UITools.HideToolBarSeparator(uwToolbar, "SaveSep");            
      }
      else
      {
        dg.Visible = true;
        lbNoresults.Visible = false;

        if (SessionState.User.HasCapability(HyperCatalog.Business.CapabilitiesEnum.MANAGE_CARTOGRAPHY))
				{
					UITools.ShowToolBarButton(uwToolbar, "Delete");
					UITools.ShowToolBarSeparator(uwToolbar, "DeleteSep"); 
					uwToolbar.Items.FromKeyButton("Delete").DefaultStyle.Width = Unit.Pixel(120);
				}

        if (txtFilter.Text.Length == 0)
        {
          if (SessionState.User.HasCapability(HyperCatalog.Business.CapabilitiesEnum.MANAGE_CARTOGRAPHY))
					{
						UITools.ShowToolBarButton(uwToolbar, "Save");
						UITools.ShowToolBarSeparator(uwToolbar, "SaveSep"); 
						uwToolbar.Items.FromKeyButton("Save").DefaultStyle.Width = Unit.Pixel(160); 
					}
        }
        else
        { 
          UITools.HideToolBarButton(uwToolbar, "Save");
          UITools.HideToolBarSeparator(uwToolbar, "SaveSep");  
        }
      }
    }
    InitializeGridGrouping();
  } 
   
  /// <summary>
  /// Delete multiple containers on the grid
  /// </summary>
  private void Delete()
  {
    lbError.Visible = false;
    bool success = true;
    if (dg != null && dg.Rows != null && dg.Rows.Count > 0)
    {
      TemplatedColumn col = (TemplatedColumn)dg.Rows[0].Cells.FromKey("Select").Column;
      if (col != null)
      {
        for (int i=0; i<col.CellItems.Count; i++)
        {
          CellItem cellItem = (CellItem)col.CellItems[i];
          Infragistics.WebUI.UltraWebGrid.UltraGridRow r = cellItem.Cell.Row;
          CheckBox cb = (CheckBox)cellItem.FindControl("g_sd");
          if (cb!=null)
          {
            if (cb.Checked)
            {
              HyperCatalog.Business.InputFormContainer ifCObj = HyperCatalog.Business.InputFormContainer.GetByKey(Convert.ToInt32(r.Cells.FromKey("InputFormContainerId").Value));
              success = ifCObj.Delete(HyperCatalog.Shared.SessionState.User.Id);
              if (!success)
              {
                UpdateDataView();
                lbError.CssClass = "hc_error";
                lbError.Text = HyperCatalog.Business.InputFormContainer.LastError;
                lbError.Visible = true;

                break;
              }
            }
          }
        }
      }
    }
    if (success) {UpdateDataView();}
  }
  
  /// <summary>
  /// update multiple lines on the grid
  /// </summary>
  private void Save()
  {
    lbError.Visible=false;

    if (dg != null)
    {
      System.Text.StringBuilder sortList = new System.Text.StringBuilder(String.Empty);
      foreach (UltraGridRow r in dg.Rows)
      {
        if (r.Cells.FromKey("Select").Style.CssClass != "ptbgroup")
        {
          int ifcId = Convert.ToInt32(r.Cells.FromKey("InputFormContainerId").Value);
          if (sortList.Length > 0)
            sortList.Append("|");
          sortList.Append(ifcId+","+r.Index);
        }
      }

      if (sortList.Length > 0)
      {
        bool success = HyperCatalog.Business.InputFormContainer.SaveNewSort(sortList.ToString());
        UpdateDataView();
        if (!success)
        {
          lbError.CssClass = "hc_error";
          lbError.Text = HyperCatalog.Business.InputFormContainer.LastError;
          lbError.Visible=true;
        }
        else
        {
          lbError.CssClass = "hc_success";
          lbError.Text = "New sort saved!";
          lbError.Visible = true;
        }
      }
    }
  }

  /// <summary>
  /// Toolbar action
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="be"></param>
  private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
  {
    string btn = be.Button.Key.ToLower();
    if ( btn == "save")
    {
      Save();
    }
    if (btn == "export")
    {
      Export.ExportIFContainers(ifId, this);
    }
    if (btn == "delete")
    {
      Delete();
    }
  }

  private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
  {
    // Display hyperlink for each container
    string sIFCId = e.Row.Cells.FromKey("InputFormContainerId").Text;
    e.Row.Cells.FromKey("ContainerName").Text = "<a href='javascript:SC("+sIFCId+")'>["+e.Row.Cells.FromKey("Tag").Text+"] - "+e.Row.Cells.FromKey("ContainerName").Text+"</a>";

    // Check if the container group has changed and if yes, add an extra row.
    string containerGroup = e.Row.Cells.FromKey("ContainerGroupPath").Value.ToString();
    if (currentGroup != containerGroup)
    {
      currentGroup = containerGroup;
      groupCount++;
    }
    // Display filter
    if (txtFilter.Text.Length > 0)
    {
      Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;

      string filter = txtFilter.Text.Trim().ToLower();
      string containerName = e.Row.Cells.FromKey("ContainerName").Text.ToLower();
      string comment = e.Row.Cells.FromKey("Comment").Text.ToLower();

      if ((containerName.Length == 0 || containerName.IndexOf(filter) < 0) && (comment.Length == 0 || comment.IndexOf(filter) < 0))
        r.Delete();
      else
        UITools.HiglightGridRowFilter2(ref r, txtFilter.Text, true, Convert.ToInt32(txtSortColPos.Value));
    } 
  }

  // "Name" Link Button event handler
  protected void UpdateGridItem(object sender, System.EventArgs e)
  {
    if (sender != null)
    {
      Infragistics.WebUI.UltraWebGrid.CellItem cellItem = ((Infragistics.WebUI.UltraWebGrid.CellItem)((System.Web.UI.WebControls.LinkButton)sender).Parent);      
      string sInputFormContainerId = cellItem.Cell.Row.Cells.FromKey("InputFormContainerId").Text;

      Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"Update", "<script>SC("+sInputFormContainerId+");</script>");
    }
  }


  private string GetSortJs()
  {
    // Call this method and then rely on index to process sort!
    if (dg.Page != null)
    {
      return "<img title=\"move down (immediated save!)\" src=\"/hc_v4/img/ed_down.gif\" onclick=\"cg_down()\">" +
             "<img title=\"move up (immediated save!)\" src=\"/hc_v4/img/ed_up.gif\" onclick=\"cg_up()\">"+
             "<img title=\"delete group (immediated save!)\" src=\"/hc_v4/img/ed_sup.gif\" onclick=\"cg_sup()\">";
    }
    return string.Empty;
  }

  public static bool EnableIntelligentSortForInputFormContainers(ref UltraWebGrid dg, int sortColIndex)
  {
    // Call this method and then rely on index to process sort!
    if (dg.Page !=null)
    {
      dg.Page.ClientScript.RegisterClientScriptBlock(dg.Page.GetType(),"Grid_Utils", "<script type=\"text/javascript\" src=\"/hc_v4/js/hypercatalog_grid.js\"></script>");
      if (dg.Columns.FromKey("s_a") != null)
      {
        dg.Columns.Remove(dg.Columns.FromKey("s_a"));
      }
      UltraGridColumn sortCol = new UltraGridColumn("s_a", "", ColumnType.NotSet, null);
      dg.Columns.Insert(sortColIndex, sortCol);
      dg.Columns.FromKey("s_a").Width = Unit.Pixel(35);
      string srcScript = "s(\"" + dg.ClientID +"\",";
      foreach (UltraGridRow r in dg.Rows)
      {
        r.Cells[sortColIndex].Text = "<img title='move down' src='/hc_v4/img/ed_down.gif' onclick='" + srcScript + "\"d\");'>"+
                                     "<img title='move up' src='/hc_v4/img/ed_up.gif' onclick='" + srcScript + "\"t\");'>";
      }
      return true;
    }
    return false;
  }

  //Modified the code to fix the QC-7030 - Display text for container group by Radha S
  private void InitializeGridGrouping()
  {
      int begCol = dg.Columns.FromKey("Select").Index;
      int i = 0;
      int x = dg.Rows.Count;
      TemplatedColumn col = (TemplatedColumn)dg.Columns.FromKey("Select");
      if (dg.Rows.Count > 0)
      {
          groupCount = 0;
          while (i < dg.Rows.Count)
          {
              string containerGroup = dg.Rows[i].Cells.FromKey("ContainerGroupPath").Value.ToString();
              if (i == 0 || currentGroup != containerGroup)
              {
                  currentGroup = containerGroup;
                  dg.Rows.Insert(i, new UltraGridRow());
                  CheckBox c = (CheckBox)((CellItem)col.CellItems[x]).FindControl("g_sd");
                  c.Visible = false;
                  Label l = (Label)((CellItem)col.CellItems[x]).FindControl("grp_lbl");
                  l.Visible = true;
                  l.Text = currentGroup + GetSortJs();
                  l.CssClass = "ptbgroup";
                  l.BorderStyle = BorderStyle.None;
                  UltraGridRow groupRow = dg.Rows[i];
                  UltraGridCell groupCellMax = groupRow.Cells[dg.Columns.Count - 1]; // initialize all cells for this row
                  foreach (UltraGridCell cell in groupRow.Cells)
                  {
                      cell.Style.CssClass = string.Empty;
                  }
                  UltraGridCell groupCell = groupRow.Cells.FromKey("Select");
                  groupCell.ColSpan = 7;//dg.Columns.Count - 1 - begCol;
                  groupCell.Text = containerGroup + GetSortJs();
                  groupCell.Title = containerGroup;
                  groupCell.Style.CssClass = "ptbgroup";
                  groupCell.Style.CustomRules = "";
                  i++;
                  x++;
              }
              i++;
          }
      }
  }
  private void DeleteGroup(int activeRowIndex)
  {
    string containerGroup = dg.Rows[activeRowIndex].Cells.FromKey("Select").Title;
    lbError.Visible = false;
    bool success = true;
    if (dg != null && dg.Rows != null && dg.Rows.Count > 0)
    {      
      for (int i = 0; i < dg.Rows.Count; i++)
      {
        CellsCollection cl = dg.Rows[i].Cells;
          if (cl.FromKey("Select").Style.CssClass != "ptbgroup" && cl.FromKey("ContainerGroupPath").Value.ToString()==containerGroup)
          {
              HyperCatalog.Business.InputFormContainer ifCObj = HyperCatalog.Business.InputFormContainer.GetByKey(Convert.ToInt32(cl.FromKey("InputFormContainerId").Value));               
              if (!ifCObj.Delete(HyperCatalog.Shared.SessionState.User.Id))
              {
                success = false;
                lbError.CssClass = "hc_error";
                lbError.Text += HyperCatalog.Business.InputFormContainer.LastError + "<br/>" + Environment.NewLine;
                lbError.Visible = true;
              }
          }
      }
    }
    UpdateDataView();
  }

  private void SortGroup(bool moveUp, int activeRowIndex)
  {
    int i = activeRowIndex;
    int targetRowIndex = -1;
    if (moveUp)
    {
      i--;
      while (i >= 0 && targetRowIndex <= 0)
      {
        if (dg.Rows[i].Cells.FromKey("Select").Style.CssClass == "ptbgroup")
        {
          targetRowIndex = i;
        }
        i--;
      }
      if (targetRowIndex >= 0)
      {
        // if a group has been found
        i = activeRowIndex;
        string groupName = dg.Rows[activeRowIndex].Cells.FromKey("Select").Text;
        int j=0;
        while (i < dg.Rows.Count)
        {
          UltraGridRow r = dg.Rows[i];
          dg.Rows.Remove(r);
          dg.Rows.Insert(targetRowIndex + j,r); 
          i++;
          j++;
          if (j==0)
          {// Group moved must be renamed
            dg.Rows[targetRowIndex].Cells.FromKey("Select").Text = groupName;
          }
          if (i < dg.Rows.Count)
          {
            if (dg.Rows[i].Cells.FromKey("Select").Style.CssClass == "ptbgroup")
            { // Stop bubbling up the rows
              i = dg.Rows.Count;
            }
          }
        }
        Save();
        UpdateDataView();
      }
    }
    else
    {
      i++;
      while (i < dg.Rows.Count && targetRowIndex <= 0)
      {
        if (dg.Rows[i].Cells.FromKey("Select").Style.CssClass == "ptbgroup")
        {
          targetRowIndex = i;
        }
        i++;
      }
      if (targetRowIndex >= 0)
      { // Move Up the group we have found
        SortGroup(true, targetRowIndex);
      }
    }
  }
}



