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
using HyperComponents.Data.dbAccess;
using HyperCatalog.Business;
using HyperCatalog.Shared;
#endregion

/// <summary>
/// Display culture properties
///		--> Save new or modified culture
///		--> Delete culture
///		--> Return to the list of culture
/// </summary>
public partial class localization_properties : HCPage
{
	#region Declarations
	private HyperCatalog.Business.Culture cul;
  private string cultureCode;
  private string masterCulture=string.Empty;
	#endregion
    
	protected void Page_Load(object sender, System.EventArgs e) 
	{
    #region Check Capabilities
		if (SessionState.User.IsReadOnly)
		{
			uwToolbar.Items.FromKeyButton("Save").Enabled = false;
			uwToolbar.Items.FromKeyButton("Delete").Enabled = false;
		}

    if (!SessionState.User.HasCapability(CapabilitiesEnum.EXTEND_CONTENT_MODEL))
    {
      UITools.HideToolBarButton(uwToolbar, "Save");
      UITools.HideToolBarSeparator(uwToolbar, "SaveSep");
      UITools.HideToolBarButton(uwToolbar, "Delete");
      UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
    }

    #endregion 
    if (Request["c"] != null)
    {
      cultureCode = Request["c"].ToString();
      masterCulture = HyperCatalog.Shared.SessionState.MasterCulture.Code;
      using (cul = HyperCatalog.Business.Culture.GetByKey(cultureCode))
      {
        if (!Page.IsPostBack)
        {
          UpdateDataEdit();
        }
      }
    }
	} 
	private void UpdateDataEdit()
  {
    #region RegionCountries list
    // retrieve all regions (level1) and countries
    using (RegionList _regions = HyperCatalog.Business.Region.GetAllRegions())
    {
      using (CountryList _countries = Country.GetAllCountries())
      {
        ddlRegionsCountries.Items.Clear();
        foreach (HyperCatalog.Business.Region r in _regions)
        {
          if ((r.Code2 != null))
          {
            if ((r.ParentCode == null) && (masterCulture == string.Empty))
            {
              ddlRegionsCountries.Items.Add(new ListItem("[" + r.Code2 + "] " + r.Name + " (M)", r.Code2));
            }
            if (r.ParentCode != null)
            {
              ddlRegionsCountries.Items.Add(new ListItem("[" + r.Code2 + "] " + r.Name + " (R)", r.Code2));
            }
          }
        }
        foreach (HyperCatalog.Business.Country co in _countries)
        {
          ddlRegionsCountries.Items.Add(new ListItem("[" + co.Code + "] " + co.Name + " (C)", co.Code));
        }
      }
    }
    #endregion
    #region Load Culture Type list
    dlCultureTypeId.DataSource = Enum.GetNames(typeof(CultureType));
		dlCultureTypeId.DataBind();
    #endregion

		if (cul != null)
		{
      txtCultureCode.Enabled = false;
      #region Retrieve information about the current culture
			txtCultureName.Text = cul.Name.ToString();
      txtCultureId.Text = cul.Id.ToString();
      // Case Master culture
      if (cul.Type == CultureType.Master)
      {
        ddlRegionsCountries.Items.Clear();
        ddlRegionsCountries.Items.Add(new ListItem("[" + cul.CountryCode + "] " + cul.Name, cul.CountryCode));
        LoadLanguageList(true);
        dlCultureTypeId.SelectedValue = Enum.GetName(typeof(CultureType), CultureType.Master);
      }
      else
      {
        ddlRegionsCountries.SelectedValue = cul.CountryCode;
        if (ddlRegionsCountries.SelectedItem.Text.IndexOf("(R)") > 0)
        {
          LoadLanguageList(true);
        }
        else
        {
          LoadLanguageList(false);
        }
      }
      ddlLanguages.SelectedValue = cul.LanguageCode;

      ddlRegionsCountries.Enabled = false;
      ddlLanguages.Enabled = false;
      #region Populate values in FallBackCode Dropdown list
      dlFallBackCode.Visible = true;
			dlCultureTypeId.SelectedValue = cul.Type.ToString();
			switch (cul.Type)
			{
        case CultureType.Master:
          dlFallBackCode.Visible = false;
          break;
        case CultureType.Regionale:
          using (CultureList rList = HyperCatalog.Business.Culture.GetAll("CultureTypeId < 1"))
          {
          dlFallBackCode.DataSource = rList;
          dlFallBackCode.DataBind();
          }
          break;
        default: // Locale
          using (CultureList lList = HyperCatalog.Business.Culture.GetAll("CultureTypeId = 1"))
          {
            dlFallBackCode.DataSource = lList;
            dlFallBackCode.DataBind();
            dlFallBackCode.SelectedValue = cul.FallbackCode.ToString().Trim();
          }
          break;
      }
      #endregion
      #endregion
		}
		else
		{
      #region New culture
      UITools.HideToolBarButton(uwToolbar, "Delete");
      UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
      ddlRegionsCountries.Enabled = true;
      ddlLanguages.Enabled = true;
      dlFallBackCode.Items.Clear();
      dlFallBackCode.Visible = true;
      ddlRegionsCountries.SelectedIndex = 0;
      if (ddlRegionsCountries.SelectedItem.Text.IndexOf("(R)") > 0)
      {
        LoadLanguageList(true);
      }
      else
      {
        LoadLanguageList(false);
      }
      ddlLanguages.SelectedIndex = 0;
      switch (((CultureType)Enum.Parse(typeof(CultureType), dlCultureTypeId.SelectedValue)))
      {
        case CultureType.Master:
          dlFallBackCode.Visible = false;
          break;
        case CultureType.Regionale:
          using (CultureList rList = HyperCatalog.Business.Culture.GetAll("CultureTypeId < 1"))
          {
            dlFallBackCode.DataSource = rList;
            dlFallBackCode.DataBind();
            dlFallBackCode.SelectedValue = masterCulture;
          }
          break;
        default: // Locale
          using (CultureList rList = HyperCatalog.Business.Culture.GetAll("CultureTypeId < 2"))
          {
            dlFallBackCode.DataSource = rList;
            dlFallBackCode.DataBind();
            dlFallBackCode.SelectedValue = masterCulture;
          }
          break;
      }
			//dlFallBackCode.DataSource = HyperCatalog.Business.Culture.GetAll("CultureTypeId = " + HyperCatalog.Business.Culture.GetByType((CultureType)Enum.Parse(typeof(CultureType), dlCultureTypeId.SelectedValue)));
			//dlFallBackCode.DataBind();
			//dlCultureTypeId.SelectedIndex = 0;
      #endregion
		}
    // display culturecode, following ddlRegionCountries + ddlLanguages
    txtCultureCode.Text = ddlRegionsCountries.SelectedValue.ToLower() + '-' + ddlLanguages.SelectedValue.ToLower();
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
	}
	private void Save()
	{
    if (cultureCode == "-1")
    {
      HyperCatalog.Business.Culture c = HyperCatalog.Business.Culture.GetByKey(txtCultureCode.Text.Trim());
        if (c == null)
        {
          // Create new culture
          cul = new HyperCatalog.Business.Culture(txtCultureCode.Text.Trim(), txtCultureName.Text.Trim(), dlFallBackCode.SelectedValue.Trim(),
            (CultureType)Enum.Parse(typeof(CultureType), dlCultureTypeId.SelectedValue),
            0, 0, ddlLanguages.SelectedValue, ddlRegionsCountries.SelectedValue);
        }
        else
        {
          lbError.CssClass = "hc_error";
          lbError.Text = "This culture code [" + txtCultureCode.Text.Trim() + "] already exist !";
          lbError.Visible = true;
          return;
        }
    }
    else
    {
      // update culture
      cul = HyperCatalog.Business.Culture.GetByKey(cultureCode);
      cul.Code = txtCultureCode.Text;
      cul.Name = txtCultureName.Text;
      cul.FallbackCode = dlFallBackCode.SelectedValue.Trim();
    }
    if (cul != null)
    {
      if (cul.Save())
      {
        // create/update
        cul.Dispose();
        lbError.Text = "Data saved!";
        lbError.CssClass = "hc_success";
        lbError.Visible = true;
      }
      else
      {
        lbError.CssClass = "hc_error";
        lbError.Text = HyperCatalog.Business.Culture.LastError;
        lbError.Visible = true;
      }
    }
    else
    {
      lbError.CssClass = "hc_error";
      lbError.Text = "Error: Culture not found";
      lbError.Visible = true;
    }
  }
	private void Delete()
	{
    using (HyperCatalog.Business.Culture cul = HyperCatalog.Business.Culture.GetByKey(txtCultureCode.Text))
    {

      if (cul != null)
      {
        if (cul.Delete(HyperCatalog.Shared.SessionState.User.Id))
        {
          Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "clientScript", "<script>back();</script>");
        }
        else
        {
          lbError.CssClass = "hc_error";
          lbError.Text = HyperCatalog.Business.Culture.LastError;
          lbError.Visible = true;
        }
      }
      else
      {
        lbError.CssClass = "hc_error";
        lbError.Text = "Error: Culture not found";
        lbError.Visible = true;
      }
    }
	}

  private void RefreshFallBackCode()
  {
    dlFallBackCode.Items.Clear();
    dlFallBackCode.Visible = true;
    switch (((CultureType)Enum.Parse(typeof(CultureType), dlCultureTypeId.SelectedValue)))
    {
      case CultureType.Master:
        dlFallBackCode.Visible = false;
        break;
      case CultureType.Regionale:
        using (CultureList rList = HyperCatalog.Business.Culture.GetAll("CultureTypeId = 0"))
        {
          dlFallBackCode.DataSource = rList;
          dlFallBackCode.DataBind();
          dlFallBackCode.SelectedValue = masterCulture;
        }
        break;
      default: // Locale
        using (CultureList lList = HyperCatalog.Business.Culture.GetAll("CultureTypeId = 1"))
        {
          dlFallBackCode.DataSource = lList;
          dlFallBackCode.DataBind();
          dlFallBackCode.SelectedIndex = 0;
        }
        break;
    }
  }

  protected void ddlRegionsCountries_SelectedIndexChanged(object sender, EventArgs e)
  {
    txtCultureCode.Text = ddlRegionsCountries.SelectedValue.ToLower() + '-' + ddlLanguages.SelectedValue.ToLower();
    LoadLanguageList(false);
    if (ddlRegionsCountries.SelectedItem.Text.IndexOf("(R)") > 0)
    {
      LoadLanguageList(true);
    }

  }
  protected void ddlLanguages_SelectedIndexChanged(object sender, EventArgs e)
  {
    txtCultureCode.Text = ddlRegionsCountries.SelectedValue.ToLower() + '-' + ddlLanguages.SelectedValue.ToLower();
  }

  private void LoadLanguageList(bool forRegion)
  {
    ddlLanguages.Items.Clear();
    string filter = string.Empty;
    dlCultureTypeId.SelectedValue = Enum.GetName(typeof(CultureType), CultureType.Locale);
    // if region, display only englisg languages
    if (forRegion)
    {
      filter = " LanguageCode = 'EN'";
      dlCultureTypeId.SelectedValue = Enum.GetName(typeof(CultureType), CultureType.Regionale);
    }
    RefreshFallBackCode();
    using (LanguageList _languages = Language.GetAll(filter))
    {
      foreach (Language l in _languages)
      {
        ddlLanguages.Items.Add(new ListItem("[" + l.Code + "] " + l.Name, l.Code));
      }
    }
  }
}

