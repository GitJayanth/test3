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
using Infragistics.WebUI.UltraWebGrid;

namespace HyperCatalog.UI.Admin.Architecture
{
	/// <summary>
	/// Description résumée de Regions.
	/// </summary>
	public partial class Regions : HCPage
	{
    #region UI
    #region Grid
    #endregion
    #region Properties
    #endregion

    protected Business.CapabilitiesEnum updateCapability = Business.CapabilitiesEnum.MANAGE_USERS;
    #endregion

    #region Business objects
    protected HyperCatalog.Business.Region rootRegion = HyperCatalog.Business.Region.GetRootRegion();
    private HyperCatalog.Business.RegionList allRegions = HyperCatalog.Business.Region.GetAllRegions();
    private HyperCatalog.Business.Region _currentRegion;
    protected HyperCatalog.Business.Region currentRegion
    {
      get
      {
        if (_currentRegion == null && ViewState["currentRegionCode"]!=null)
          _currentRegion = HyperCatalog.Business.Region.GetByKey((string)ViewState["currentRegionCode"]);
        return _currentRegion;
      }
      set
      {
        if (value!=null)
        {
          ViewState["currentRegionCode"] = value.Code;
          _currentRegion = value;
        }
        else
        {
          ViewState.Remove("currentRegionCode");
          _currentRegion = null;
        }

        txtParentRegionCodeValue.Items.Clear();
        foreach (HyperCatalog.Business.Region region in allRegions)
        {
          if (value==null || value.FindRegion(region.Code)==null)
            txtParentRegionCodeValue.Items.Add(new ListItem(region.Name == null || region.Name.Equals(string.Empty) ? region.Code : region.Name, region.Code));
        }

        if (value!=null)
        {
          txtRegionNameValue.Text = value.Name;
          txtRegionCodeValue.Text = value.Code;
          txtRegionCode2Value.Text = value.Code2;
          if (!Page.IsPostBack)
          {
              cbPublishable.Checked = value.Publishable;
              cbFallbackToEnglish.Checked = value.FallBackToEnglish;
          }
          txtParentRegionCodeValue.ClearSelection();
          txtParentRegionCodeValue.SelectedValue = value.ParentCode;
          //parent region control disabled when editing root region
          txtParentRegionCodeValue.Enabled = (value.ParentCode!=null);

          txtParentRegionCodeValue.Items.Remove(txtParentRegionCodeValue.Items.FindByValue(value.Code));
        }
        else
        {
          txtRegionNameValue.Text = null;
          txtRegionCodeValue.Text = null;
          txtRegionCode2Value.Text = null;
          Business.Region root = Business.Region.GetRootRegion();
          if (root!=null)
            txtParentRegionCodeValue.SelectedValue = Business.Region.GetRootRegion().Code;
        }
        //when updating, code and code2 cannot be edited
        txtRegionCodeValue.Enabled = (value==null);
        txtRegionCode2Value.Enabled = (value == null);
      }
    }
    #endregion

    protected override void OnLoad(EventArgs e)
    {
      UITools.CheckConnection(Page);
      propertiesMsgLbl.Visible = mainMsgLbl.Visible = false;

      if (Request["r"] != null && Request["r"].Length > 0)
        currentRegion = rootRegion.FindRegion(Request["r"]);

      if (!IsPostBack)
      {
        if (Request["r"] == null || Request["r"].Length == 0)
          loadRegionGrid();
        else
          UpdateDataEdit(Request["r"]);
      }

      base.OnLoad (e);
    }

    protected override void OnPreRender(EventArgs e)
    {
      mainToolBar.Items.FromKeyButton("Add").Enabled =
        propertiesToolBar.Items.FromKeyButton("Delete").Enabled = 
        propertiesToolBar.Items.FromKeyButton("Save").Enabled = 
        !HyperCatalog.Shared.SessionState.User.IsReadOnly && HyperCatalog.Shared.SessionState.User.HasCapability(updateCapability);
      txtParentRegionCodeValue.Enabled = currentRegion==null && rootRegion!=null || currentRegion!=null && currentRegion.Parent!=null;

      base.OnPreRender (e);
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
      this.mainToolBar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.mainToolBar_ButtonClicked);
      this.propertiesToolBar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(propertiesToolBar_ButtonClicked);
    }
    #endregion

    #region Data load & bind
    private void loadRegionGrid()
    {
      regionsGrid.Rows.Clear();
      if (rootRegion!=null)
        loadRegionRow(rootRegion,0);
    }
    private void loadRegionRow(HyperCatalog.Business.Region region, int offset)
    {
      UltraGridRow newRow = new UltraGridRow(new object[] { region.Code, region.Code + (region.Code2 != null && region.Code2 != string.Empty ? " (" + region.Code2 + ")" : ""), region.Name == null || region.Name.Equals(string.Empty) ? region.Code : region.Name, region.TotalCountryCount });
      regionsGrid.Rows.Add(newRow);
      newRow.DataKey = region.Code;
      newRow.Cells.FromKey("DisplayCode").Style.Padding.Left = Unit.Parse((offset * 5).ToString() + "px");

      //txtParentRegionCodeValue.Items.Add(new ListItem(region.Name == null || region.Name.Equals(string.Empty)?region.Code:region.Name,region.Code));

      foreach (HyperCatalog.Business.Region subRegion in region.SubRegions)
        loadRegionRow(subRegion,offset + 1);
    }
    private void regionsGrid_DemandLoad(object sender, RowEventArgs e)
    {
      string code = (string)e.Row.DataKey;
      if (code!=null)
      {
        UltraGridBand band = regionsGrid.Bands[1];
        if(band.Columns.Count==0)
        {
          //          foreach (UltraGridColumn col in regionsGrid.Bands[0].Columns)
          //            band.Columns.Add(new UltraGridColumn().CopyFrom(col));

          band.Columns.Add(new UltraGridColumn());
          band.Columns.Add(new UltraGridColumn());
          band.Columns[0].HeaderText="Code";
          band.Columns[1].HeaderText="Name";
          band.Columns[1].Width = Unit.Parse("300px");
        }

        HyperCatalog.Business.CountryList countries = rootRegion.SubRegions[code].Countries;
        foreach (string countryCode in countries)
        {
          HyperCatalog.Business.Country country = countries[countryCode];
          UltraGridRow newRow = new UltraGridRow(new object[]{country.Code,country.Name});
          newRow.DataKey = countryCode;
          e.Row.Rows.Add(newRow);
        }
      }
    }
    #endregion

    #region Events
    #region Toolbar events
    private void mainToolBar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      switch (be.Button.Key)
      {
        case "Add":
          currentRegion = null;
          propertiesToolBar.Items.FromKeyButton("Delete").Visible = false;
          propertiesToolBar.Items.FromKeySeparator("SepDelete").Width = 0;
          txtRegionNameValue.Enabled = true;
          setPropertiesVisible(true);
          break;
        case "Export":
          Utils.ExportToExcel(regionsGrid, "Regions", "Regions");
          break;
      }
    }
    private void propertiesToolBar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      switch (be.Button.Key)
      {
        case "List":
          Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "UpdateCurrentRegion", "UpdateCurrentRegion('')", true);

          setPropertiesVisible(false);
          break;
        case "Save":
          if (!HyperCatalog.Shared.SessionState.User.IsReadOnly && HyperCatalog.Shared.SessionState.User.HasCapability(updateCapability))
          {
            if (currentRegion!=null)
            {
              // Update region
                if (currentRegion.Update(txtRegionCodeValue.Text, txtRegionCode2Value.Text, txtRegionNameValue.Text, txtParentRegionCodeValue.SelectedValue, cbPublishable.Checked, cbFallbackToEnglish.Checked))
              {
                Tools.UITools.SetMessage(propertiesMsgLbl,"Region \"" + currentRegion.Name + "\" updated.",Tools.UITools.MessageLevel.Information);
                loadRegionGrid();
              }
              else
                Tools.UITools.SetMessage(propertiesMsgLbl,"Region \"" + currentRegion.Name + "\" could not be updated.",Tools.UITools.MessageLevel.Error);
            }
            else if (txtRegionCodeValue.Text!=string.Empty)
            {
              // Insert new region
              if (rootRegion==null)
              {
                currentRegion = Business.Region.Create(txtRegionCodeValue.Text,txtRegionCode2Value.Text,txtRegionNameValue.Text,null, cbPublishable.Checked , cbFallbackToEnglish.Checked);
                if (currentRegion != null)
                {
                  Tools.UITools.SetMessage(mainMsgLbl,"Region \"" + txtRegionCodeValue.Text + "\" insertion succeeded.",Tools.UITools.MessageLevel.Information);
                  loadRegionGrid();
                  setPropertiesVisible(false);
                }
                else
                  Tools.UITools.SetMessage(propertiesMsgLbl, HyperCatalog.Business.Region.LastError, Tools.UITools.MessageLevel.Error);
              }
              else if (rootRegion.FindRegion(txtRegionCodeValue.Text)==null)
              {
                currentRegion = rootRegion.FindRegion(txtParentRegionCodeValue.SelectedValue).SubRegions.Add(txtRegionCodeValue.Text,txtRegionCode2Value.Text,txtRegionNameValue.Text, cbPublishable.Checked, cbFallbackToEnglish.Checked);
                if (currentRegion!=null)
                {
                  Tools.UITools.SetMessage(mainMsgLbl,"Region \"" + txtRegionCodeValue.Text + "\" insertion succeeded.",Tools.UITools.MessageLevel.Information);
                  loadRegionGrid();
                  setPropertiesVisible(false);
                }
                else
                  Tools.UITools.SetMessage(propertiesMsgLbl, HyperCatalog.Business.Region.LastError, Tools.UITools.MessageLevel.Error);
              }
              else
                Tools.UITools.SetMessage(propertiesMsgLbl,"This region code is already used for another region.",Tools.UITools.MessageLevel.Warning);
            }
            else
              Tools.UITools.SetMessage(propertiesMsgLbl,"You must provide a region code.",Tools.UITools.MessageLevel.Warning);
          }
          else
            Tools.UITools.SetMessage(propertiesMsgLbl,"You are not allowed to create or modify regions.",Tools.UITools.MessageLevel.Warning);
          break;
        case "Delete":
          if (currentRegion != null)
          {
            if (currentRegion.Delete(txtRegionCodeValue.Text))
            {
              Tools.UITools.SetMessage(mainMsgLbl, "Region \"" + txtRegionCodeValue.Text + "\" is deleted.", Tools.UITools.MessageLevel.Information);
              currentRegion = null;
              rootRegion = HyperCatalog.Business.Region.GetRootRegion();
              loadRegionGrid();
              setPropertiesVisible(false);
            }
            else
            {
              Tools.UITools.SetMessage(mainMsgLbl, HyperCatalog.Business.Region.LastError, Tools.UITools.MessageLevel.Error);
              setPropertiesVisible(false);
            }
          }
          break;
      }
    }
    #endregion

    #region Grid events
    protected void UpdateGridItem(object sender, System.EventArgs e)
    {
      Infragistics.WebUI.UltraWebGrid.CellItem cellItem = ((Infragistics.WebUI.UltraWebGrid.CellItem)((System.Web.UI.WebControls.LinkButton)sender).Parent);
      string regionCode = cellItem.Cell.Row.Cells.FromKey("RegionCode").Text;
      UpdateDataEdit(regionCode);

      Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "UpdateCurrentRegion", "UpdateCurrentRegion('" + regionCode + "')", true);
    }
    protected void GotoCountries(object sender, System.EventArgs e)
    {
      Infragistics.WebUI.UltraWebGrid.CellItem cellItem = ((Infragistics.WebUI.UltraWebGrid.CellItem)((System.Web.UI.WebControls.LinkButton)sender).Parent);
    }
    private void UpdateDataEdit(string selContainerId)
    {
      currentRegion = rootRegion.FindRegion(selContainerId);
      propertiesToolBar.Items.FromKeyButton("Delete").Visible = true;
      propertiesToolBar.Items.FromKeySeparator("SepDelete").Width = 8;
      setPropertiesVisible(true);
    }
    #endregion
    #endregion

    #region Private methods
    private void setPropertiesVisible(bool visible)
    {
      panelGrid.Visible = !visible;
      panelProperties.Visible = visible;
    }
    #endregion
  }
}
