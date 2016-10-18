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
using HyperCatalog.Business;
using HyperCatalog.Shared;
using Infragistics.WebUI.UltraWebGrid;
#endregion

namespace HyperCatalog.UI.Admin.Architecture
{
  /// <summary>
  /// Instant run parameters for the delivery process
  /// </summary>
  public partial class Delivery : HCPage
  {
    #region Declarations
    protected Infragistics.WebUI.UltraWebGrid.UltraWebGrid countriesGrid;
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
      this.mainToolBar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.mainToolBar_ButtonClicked);
      this.propertiesToolBar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.propertiesToolBar_ButtonClicked);

    }
    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
    {
      #region Check Capabilities
      if (SessionState.User.IsReadOnly)
      {
        mainToolBar.Items.FromKeyButton("Add").Enabled = false;
        mainToolBar.Items.FromKeyButton("Delete").Enabled = false;
        mainToolBar.Items.FromKeyButton("Save").Enabled = false;
        propertiesToolBar.Items.FromKeyButton("Save").Enabled = false;
      }
      #endregion
      if (!Page.IsPostBack)
      {
        ShowPackages();
        panelGrid.Visible = true;
        panelProperties.Visible = false;
      }
    }

    /// <summary>
    /// Display all packages available
    /// </summary>
    private void ShowPackages()
    {
      lbTitle.Text = "Packages list";
      using (PackageList p = Package.GetAll())
      {
        p.Sort("Sort");
        #region No result
        if (p.Count == 0)
        {
          mainMsgLbl.Text = "No package found";
          mainMsgLbl.Visible = true;
          UITools.HideToolBarButton(mainToolBar, "Delete");
          UITools.HideToolBarSeparator(mainToolBar, "SepDelete");
          packagesGrid.Visible = false;
        }
        #endregion
        #region Results
        else
        {
          packagesGrid.DataSource = p;
          mainMsgLbl.Visible = false;
          packagesGrid.Bands[0].ColHeadersVisible = Infragistics.WebUI.UltraWebGrid.ShowMarginInfo.Yes;
          packagesGrid.DataBind();
          packagesGrid.Visible = true;
          EnableIntelligentSortForPackages(ref packagesGrid, Convert.ToInt32(txtSortColPos.Value));
          packagesGrid.DisplayLayout.AllowSortingDefault = AllowSorting.No;
          UITools.ShowToolBarButton(mainToolBar, "Delete");
          UITools.ShowToolBarSeparator(mainToolBar, "SepDelete");
        }
        #endregion
      }
    }

    /// <summary>
    /// Display the package selected
    /// </summary>
    private void ShowPackage(int package)
    {
      #region Load Region/country package listbox
      HyperCatalog.Business.CountryList cList = HyperCatalog.Business.Country.GetAllCountries();
      ddlRegionPackage.Items.Add(new ListItem("GENERIC EMEA", "GENERIC EMEA"));
      ddlRegionPackage.Items.Add(new ListItem("APJ WIDE", "APJ WIDE"));
      foreach (Country c in cList)
      {
        ddlRegionPackage.Items.Add(new ListItem(c.Name, c.Code));
      }
      #endregion
      lbTitle.Text = "Package properties";
      propertiesMsgLbl.Visible = false;
      Package p = Package.GetByKey(package);
      txtPackageId.Text = package.ToString();
      if (p != null)
      {
        cbActive.Checked = p.IsActive;
        txtName.Text = p.Name;
        txtFileName.Text = p.FileName;
        cbTimeStamp.Checked = p.IncludeTimeStamp;
        txtFTPURL.Text = p.FTPURL;
        txtFTPLogin.Text = p.FTPLogin;
        txtFTPPassword.Text = p.FTPPassword;
        txtUploadPath.Text = p.UploadPath;
        txtFTPPort.Text = p.FTPPort;
        ddlFTPMode.SelectedValue = p.FTPMode;
        txtEmail.Text = p.NotificationEmail;
        panelCreator.Visible = true;
        hlCreator.Text = p.User.FullName;
        lbOrganization.Text = p.User.Organization.Name;
        ddlRegionPackage.SelectedValue = (p.RegionPackage != string.Empty) ? p.RegionPackage : p.Countries[0].CountryCode;
        panelFilter.Visible = false;
        if (p.RegionPackage == string.Empty)
        {
          panelFilter.Visible = true;
          cboxFilter.Items[0].Selected = p.Countries[0].FilterPreReleased;
          cboxFilter.Items[1].Selected = p.Countries[0].FilterLive;
          cboxFilter.Items[2].Selected = p.Countries[0].FilterObsolete;
          cboxFilter.Items[3].Selected = p.Countries[0].ApplyFallbackEnglish;
        }
      }
      else
      {
        panelFilter.Visible = false;
        cbActive.Checked = false;
        txtPackageId.Text = package.ToString();
        txtName.Text = String.Empty;
        txtFileName.Text = String.Empty;
        cbTimeStamp.Checked = false;
        ddlRegionPackage.SelectedIndex = 0;
        txtFTPURL.Text = string.Empty;
        txtFTPLogin.Text = string.Empty;
        txtFTPPassword.Text = string.Empty;
        txtUploadPath.Text = string.Empty;
        txtFTPPort.Text = string.Empty;
        ddlFTPMode.SelectedIndex = 0;
        txtEmail.Text = string.Empty;
        panelCreator.Visible = false;
      }
    }

    /// <summary>
    /// Display the selected package properties
    /// </summary>
    /// <param name="selWord">word</param>
    void UpdateDataEdit(string selPackage)
    {
      panelGrid.Visible = false;
      panelProperties.Visible = true;
      ShowPackage(Convert.ToInt32(selPackage));
    }

    /// <summary>
    /// Link to the package selected
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void UpdateGridItem(object sender, System.EventArgs e)
    {
      Infragistics.WebUI.UltraWebGrid.CellItem cellItem = ((Infragistics.WebUI.UltraWebGrid.CellItem)((System.Web.UI.WebControls.LinkButton)sender).Parent);
      UpdateDataEdit(cellItem.Cell.Row.Cells.FromKey("PackageId").Text);
    }

    /// <summary>
    /// functions of the main Toolbar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="be"></param>
    private void mainToolBar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      #region Add new package
      if (btn == "add")
      {
        UpdateDataEdit("-1");
      }
      #endregion
      #region DAM
      if (btn == "dam")
      {
        Response.Redirect("./DeliveryOutputs.aspx");
      }
      #endregion
      #region Delete packages
      if (btn == "delete")
      {
        DeleteSelectedItems();
      }
      #endregion
      #region Sort packages
      if (btn == "save")
      {
        SortPackages();
      }
      #endregion
    }

    /// <summary>
    /// update multiple lines on the grid
    /// </summary>
    private void SortPackages()
    {
      mainMsgLbl.Visible = false;

      if (packagesGrid != null)
      {
        System.Text.StringBuilder sortList = new System.Text.StringBuilder(String.Empty);
        foreach (UltraGridRow r in packagesGrid.Rows)
        {
          if (r.Cells.FromKey("Select").Style.CssClass != "ptbgroup")
          {
            int pId = Convert.ToInt32(r.Cells.FromKey("PackageId").Value);
            if (sortList.Length > 0)
              sortList.Append("|");
            sortList.Append(pId + "," + r.Index);
          }
        }

        if (sortList.Length > 0)
        {
          bool success = HyperCatalog.Business.Package.SaveNewSort(sortList.ToString());
          ShowPackages();
          if (!success)
          {
            mainMsgLbl.CssClass = "hc_error";
            mainMsgLbl.Text = HyperCatalog.Business.Package.LastError;
            mainMsgLbl.Visible = true;
          }
          else
          {
            mainMsgLbl.CssClass = "hc_success";
            mainMsgLbl.Text = "New sort saved!";
            mainMsgLbl.Visible = true;
          }
        }
      }
    }

    /// <summary>
    /// functions of the properies packages Toolbar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="be"></param>
    private void propertiesToolBar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      #region Return List
      if (btn == "list")
      {
        ShowPackages();
        panelGrid.Visible = true;
        panelProperties.Visible = false;
      }
      #endregion
      #region Save package
      if (btn == "save")
      {
        BtnSave();
      }
      #endregion
    }

    /// <summary>
    /// Delete packages selected
    /// </summary>
    private void DeleteSelectedItems()
    {
      mainMsgLbl.Text = string.Empty;
      int nbDeletedRows = 0;
      foreach (Infragistics.WebUI.UltraWebGrid.UltraGridRow dr in packagesGrid.Rows)
      {
        TemplatedColumn col = (TemplatedColumn)dr.Cells.FromKey("Select").Column;
        CheckBox cb = (CheckBox)((CellItem)col.CellItems[dr.Index]).FindControl("g_sd");
        if (cb.Checked)
        {
          Package p = Package.GetByKey(Convert.ToInt32(dr.Cells.FromKey("PackageId").Value));
          if (!p.Delete())
          {
            mainMsgLbl.Text = "Error: package [" + dr.Cells.FromKey("PackageId").Value.ToString() + "] can't be deleted<br>";
            mainMsgLbl.CssClass = "hc_error";
            mainMsgLbl.Visible = true;
            break;
          }
          else
          {
            nbDeletedRows++;
          }
        }
      }
      if (nbDeletedRows > 0)
      {
        mainMsgLbl.Text = "Data deleted";
        mainMsgLbl.CssClass = "hc_success";
        mainMsgLbl.Visible = true;
        ShowPackages();
        packagesGrid.DisplayLayout.Pager.CurrentPageIndex = 1;
      }
    }

    /// <summary>
    /// Save the package
    /// </summary>
    private void BtnSave()
    {
      bool canContinue = true;
      if (ddlRegionPackage.SelectedValue != "GENERIC EMEA" && ddlRegionPackage.SelectedValue != "APJ WIDE")
      {
        canContinue = cboxFilter.Items[0].Selected || cboxFilter.Items[1].Selected || cboxFilter.Items[2].Selected;
      }
      if (canContinue)
      {
        Package p = Package.GetByKey(Convert.ToInt32(txtPackageId.Text.Trim()));
        if (p == null)
        {
          #region new package
          p = new Package(-1, txtName.Text, SessionState.User.Id, 0, cbActive.Checked, Utils.CleanFileName(txtFileName.Text), (ddlRegionPackage.SelectedValue == "NONE" ? string.Empty : ddlRegionPackage.SelectedValue.ToString()), cbTimeStamp.Checked, txtEmail.Text,
            txtFTPURL.Text, txtFTPLogin.Text, txtFTPPassword.Text, txtFTPPort.Text, ddlFTPMode.SelectedValue.ToString(), SessionState.User.FullName, txtUploadPath.Text);
          #endregion
        }
        else
        {
          #region Update package
          p.Name = txtName.Text;
          p.IsActive = cbActive.Checked;
          p.FileName = Utils.CleanFileName(txtFileName.Text);
          p.IncludeTimeStamp = cbTimeStamp.Checked;
          p.FTPURL = txtFTPURL.Text.Trim();
          p.FTPLogin = txtFTPLogin.Text.Trim();
          p.FTPPassword = txtFTPPassword.Text.Trim();
          p.UploadPath = txtUploadPath.Text.Trim();
          p.FTPPort = txtFTPPort.Text;
          p.FTPMode = ddlFTPMode.SelectedValue.ToString();
          p.NotificationEmail = txtEmail.Text;
          p.UserId = SessionState.User.Id;
          p.Countries.Clear();
          string err = string.Empty;
          if (ddlRegionPackage.SelectedValue != "GENERIC EMEA" && ddlRegionPackage.SelectedValue != "APJ WIDE")
          {
            p.RegionPackage = string.Empty;
            p.Countries.Add(new PackageCountry(p.Id, ddlRegionPackage.SelectedValue.ToString(),
              cboxFilter.Items[0].Selected,
              cboxFilter.Items[1].Selected,
              cboxFilter.Items[2].Selected,
              cboxFilter.Items[3].Selected));
          }
          else
          {
            p.Countries.Clear();
            p.RegionPackage = ddlRegionPackage.SelectedValue;
          }
          #endregion
        }
        #region Save result
        if (p != null)
        {
          if (p.Save())
          {
            propertiesMsgLbl.Text = "Data saved";
            propertiesMsgLbl.CssClass = "hc_success";
            propertiesMsgLbl.Visible = true;
          }
          else
          {
            propertiesMsgLbl.Text = Package.LastError;
            propertiesMsgLbl.CssClass = "hc_error";
            propertiesMsgLbl.Visible = true;
          }
        #endregion
        }
      }
      else
      {
        propertiesMsgLbl.Text = "Please select a least one type of product to deliver";
        propertiesMsgLbl.CssClass = "hc_error";
        propertiesMsgLbl.Visible = true;
      }
    }

    /// <summary>
    /// Delete the package
    /// </summary>
    private void BtnDelete()
    {
      Package p = Package.GetByKey(Convert.ToInt32(txtPackageId.Text));
      if (!p.Delete())
      {
        mainMsgLbl.Text = "Error: package [" + txtPackageId.Text + "] can't be deleted<br>";
        mainMsgLbl.CssClass = "hc_error";
        mainMsgLbl.Visible = true;
      }
      else
      {
        panelProperties.Visible = false;
        panelGrid.Visible = true;
        ShowPackages();
      }
    }
    protected void ddlRegionPackage_SelectedIndexChanged(object sender, EventArgs e)
    {
      if ((ddlRegionPackage.SelectedValue == "GENERIC EMEA") || (ddlRegionPackage.SelectedValue == "APJ WIDE"))
      {
        panelFilter.Visible = false;
      }
      else
      {
        panelFilter.Visible = true;
        cboxFilter.Items[0].Selected = false;
            cboxFilter.Items[1].Selected = false;
            cboxFilter.Items[2].Selected = false;
            cboxFilter.Items[3].Selected = false;
      }

    }

    public static bool EnableIntelligentSortForPackages(ref UltraWebGrid dg, int sortColIndex)
    {
      // Call this method and then rely on index to process sort!
      if (dg.Page != null)
      {
        dg.Page.ClientScript.RegisterClientScriptBlock(dg.Page.GetType(), "Grid_Utils", "<script type=\"text/javascript\" src=\"/hc_v4/js/hypercatalog_grid.js\"></script>");
        if (dg.Columns.FromKey("s_a") != null)
        {
          dg.Columns.Remove(dg.Columns.FromKey("s_a"));
        }
        UltraGridColumn sortCol = new UltraGridColumn("s_a", "", ColumnType.NotSet, null);
        dg.Columns.Insert(sortColIndex, sortCol);
        dg.Columns.FromKey("s_a").Width = Unit.Pixel(35);
        string srcScript = "s(\"" + dg.ClientID + "\",";
        foreach (UltraGridRow r in dg.Rows)
        {
          r.Cells[sortColIndex].Text = "<img title='move down' src='/hc_v4/img/ed_down.gif' onclick='" + srcScript + "\"d\");'>" +
                                       "<img title='move up' src='/hc_v4/img/ed_up.gif' onclick='" + srcScript + "\"t\");'>";
        }
        return true;
      }
      return false;
    }
}
}
