using System;
using System.Data;
using HyperCatalog.Shared;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HyperCatalog.Business;

/// <summary>
/// This class contains all parameters for road map
/// </summary>
public partial class RoadMap_CountryParameters : HCPage
{
  #region Declarations
  private string _StatusList = string.Empty;
  private string _WorkflowStatus = string.Empty;
  private string _Countries = string.Empty;
  private string _Classes = string.Empty;
  private string _FilterLiveDate = string.Empty;
  private DateTime? _LiveDate1 = null;
  private DateTime? _LiveDate2 = null;
  private string _FilterObsoleteDate = string.Empty;
  private DateTime? _ObsoleteDate1 = null;
  private DateTime? _ObsoleteDate2 = null;
  private bool _Scope = true;
  #endregion

  public string StatusList
  {
    get
    {
      _StatusList = string.Empty;
      foreach (ListItem li in lStatus.Items)
      {
        _StatusList += li.Selected ? li.Value + "," : "";
      }
      if (_StatusList.Length > 0)
        _StatusList = _StatusList.Substring(0, _StatusList.Length - 1);
      return _StatusList;
    }
  }
  public string WorkflowStatus
  {
    get
    {
      _WorkflowStatus = string.Empty;
      foreach (ListItem li in lWorkflowStatus.Items)
      {
        _WorkflowStatus += li.Selected ? li.Value + "," : "";
      }
      if (_WorkflowStatus.Length > 0)
        _WorkflowStatus = _WorkflowStatus.Substring(0, _WorkflowStatus.Length - 1);
      return _WorkflowStatus;
    }
  }
  public string Countries
  {
    get
    {
      if (ddlCountries.SelectedValue != null)
        _Countries = ddlCountries.SelectedValue;
      else
        _Countries = string.Empty;
      return _Countries;
    }
  }
  public string Classes
  {
    get
    {
      _Classes = string.Empty;
      foreach (ListItem li in lClasses.Items)
      {
        _Classes += li.Selected ? li.Value + "," : "";
      }
      if (_Classes.Length > 0)
        _Classes = _Classes.Substring(0, _Classes.Length - 1);
      return _Classes;
    }
  }
  public string FilterLiveDate
  {
    get
    {
      if (ddlLD.SelectedValue != null)
        _FilterLiveDate = ddlLD.SelectedValue;
      else
        _FilterLiveDate = string.Empty;
      return _FilterLiveDate;
    }
  }
  public string FilterObsoleteDate
  {
    get
    {
      if (ddlOD.SelectedValue != null)
        _FilterObsoleteDate = ddlOD.SelectedValue;
      else
        _FilterObsoleteDate = string.Empty;
      return _FilterObsoleteDate;
    }
  }
  public DateTime? LiveDate1
  {
    get
    {
      if (pnlLD.Visible && wdLD != null && wdLD.Text != null && wdLD.Text.Length > 0)
      {
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
        ci.DateTimeFormat.ShortDatePattern = SessionState.User.FormatDate;
        ci.DateTimeFormat.LongDatePattern = SessionState.User.FormatDate;

        _LiveDate1 = Convert.ToDateTime(wdLD.Value, ci);
      }
      return _LiveDate1;
    }
  }
  public DateTime? LiveDate2
  {
    get
    {
      if (pnlLDTo.Visible && wdLDTo != null && wdLDTo.Text != null && wdLDTo.Text.Length > 0)
      {
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
        ci.DateTimeFormat.ShortDatePattern = SessionState.User.FormatDate;
        ci.DateTimeFormat.LongDatePattern = SessionState.User.FormatDate;

        _LiveDate2 = Convert.ToDateTime(wdLDTo.Value, ci);
      }
      return _LiveDate2;
    }
  }
  public DateTime? ObsoleteDate1
  {
    get
    {
      if (pnlOD.Visible && wdOD != null && wdOD.Text != null && wdOD.Text.Length > 0)
      {
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
        ci.DateTimeFormat.ShortDatePattern = SessionState.User.FormatDate;
        ci.DateTimeFormat.LongDatePattern = SessionState.User.FormatDate;

        _ObsoleteDate1 = Convert.ToDateTime(wdOD.Value, ci);
      }
      return _ObsoleteDate1;
    }
  }
  public DateTime? ObsoleteDate2
  {
    get
    {
      if (pnlODTo.Visible && wdODTo != null && wdODTo.Text != null && wdODTo.Text.Length > 0)
      {
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
        ci.DateTimeFormat.ShortDatePattern = SessionState.User.FormatDate;
        ci.DateTimeFormat.LongDatePattern = SessionState.User.FormatDate;

        _ObsoleteDate2 = Convert.ToDateTime(wdODTo.Value, ci);
      }
      return _ObsoleteDate2;
    }
  }
  public bool Scope
  {
    get
    {
      _Scope = rdMyProduct.Checked;
      return _Scope;
    }
  }

  override protected void OnInit(EventArgs e)
  {
    base.OnInit(e);
    vList.BackButtonClick += new RoadMap_CountryViewList.BackButtonClickHandler(BackButtonClick);
    vGraphic.BackButtonClick += new RoadMap_CountryViewGraphic.BackButtonClickHandler(BackButtonClick);
    vList.ViewGraphicButtonClick += new RoadMap_CountryViewList.ViewGraphicButtonClickHandler(vList_ViewGraphicButtonClick);
    vList.ViewListButtonClick += new RoadMap_CountryViewList.ViewListButtonClickHandler(vList_ViewListButtonClick);
    vGraphic.ViewListButtonClick += new RoadMap_CountryViewGraphic.ViewListButtonClickHandler(vGraphic_ViewListButtonClick);
    vGraphic.ViewGraphicButtonClick += new RoadMap_CountryViewGraphic.ViewGraphicButtonClickHandler(vGraphic_ViewGraphicButtonClick);
  }

  protected void Page_Load(object sender, EventArgs e)
  {
    if (HyperCatalog.Shared.SessionState.User.HasCapability(CapabilitiesEnum.VIEW_ROADMAP))
    {
      System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
      ci.DateTimeFormat.ShortDatePattern = SessionState.User.FormatDate;
      ci.DateTimeFormat.LongDatePattern = SessionState.User.FormatDate;
      wdLD.CalendarLayout.Culture = ci;
      wdLDTo.CalendarLayout.Culture = ci;
      wdOD.CalendarLayout.Culture = ci;
      wdODTo.CalendarLayout.Culture = ci;

      // Init var
      Page.ClientScript.RegisterStartupScript(Page.GetType(), "Init", "<script>var lStatus='" + lStatus.ClientID + "'; var cbStatus='" + cbStatusAll.ClientID + "';var lWorkflowStatus='" + lWorkflowStatus.ClientID + "'; var cbWorkflowStatus='" + cbWorkflowStatusAll.ClientID + "';  var lClasses='" + lClasses.ClientID + "'; var cbClasses='" + cbClassesAll.ClientID + "';</script>");
      if (!Page.IsPostBack)
      {
        UpdateDataView();
      }
      lbClasses.Text = SessionState.ItemLevels[1].Name;
    }
  }

  #region Event methods
  protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
  {
    string btn = be.Button.Key.ToLower();

    if (btn.Equals("submit"))
    {
      Submit();
    }
  }
  protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
  {
    string filter = string.Empty;
    if (ddlLD != null)
    {
      filter = ddlLD.SelectedValue;
      pnlLD.Visible = false;
      pnlLDTo.Visible = false;
      if (!filter.Equals("any"))
      {
        pnlLD.Visible = true;
        if (filter.Equals("from"))
        {
          pnlLDTo.Visible = true;
        }
      }
    }
    if (ddlOD != null)
    {
      filter = ddlOD.SelectedValue;
      pnlOD.Visible = false;
      pnlODTo.Visible = false;
      if (!filter.Equals("any"))
      {
        pnlOD.Visible = true;
        if (filter.Equals("from"))
        {
          pnlODTo.Visible = true;
        }
      }
    }
  }
  void BackButtonClick(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent e)
  {
    pnlList.Visible = pnlGraphic.Visible = false;
    pnlParameters.Visible = true;
  }
  void vList_ViewGraphicButtonClick(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent e)
  {
    rdList.Checked = false;
    rdGraphic.Checked = true;

    RetrieveParameter();
    vGraphic.Refresh();

    pnlGraphic.Visible = true;
    pnlParameters.Visible = pnlList.Visible = false;
  }
  void vList_ViewListButtonClick(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent e)
  {
    RetrieveParameter();
    vList.Refresh();

    pnlList.Visible = true;
    pnlParameters.Visible = pnlGraphic.Visible = false;
  }
  void vGraphic_ViewListButtonClick(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent e)
  {
    rdGraphic.Checked = false;
    rdList.Checked = true;

    RetrieveParameter();
    vList.Refresh();

    pnlList.Visible = true;
    pnlParameters.Visible = pnlGraphic.Visible = false;
  }
  void vGraphic_ViewGraphicButtonClick(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent e)
  {
    RetrieveParameter();
    vGraphic.Refresh();

    pnlGraphic.Visible = true;
    pnlParameters.Visible = pnlList.Visible = false;
  }
  #endregion

  #region Private methods
  private void UpdateDataView()
  {
    lbMissingLD1.Visible = lbMissingLD2.Visible = lbMissingOD1.Visible = lbMissingOD2.Visible = false;

    #region "Initialize status list"
    lStatus.Items.Add(new ListItem(ItemStatus.Unknown.ToString(), "U"));
    lStatus.Items.Add(new ListItem(ItemStatus.Live.ToString(), "L"));
    lStatus.Items.Add(new ListItem(ItemStatus.Obsolete.ToString(), "O"));
    lStatus.Items.Add(new ListItem(ItemStatus.Future.ToString(), "F"));
    lStatus.Items.Add(new ListItem(ItemStatus.Excluded.ToString(), "E"));
    lStatus.DataBind();
    lStatus.Rows = 5;
    #endregion
    #region "Initialize workflow status list"
    lWorkflowStatus.Items.Add(new ListItem(ItemWorkflowStatus.Unknown.ToString(), "U"));
    lWorkflowStatus.Items.Add(new ListItem(ItemWorkflowStatus.RegionValidated.ToString(), "R"));
    lWorkflowStatus.Items.Add(new ListItem(ItemWorkflowStatus.CountryValidated.ToString(), "C"));
    lWorkflowStatus.Items.Add(new ListItem(ItemWorkflowStatus.Excluded.ToString(), "E"));
    lWorkflowStatus.DataBind();
    lWorkflowStatus.Rows = 4;
    #endregion
    #region "Initialize filter list for Live date and Obsolete date"
    string[] filterList = { "any", "from", "<", "<=", "=", ">=", ">" };
    if (filterList != null)
    {
      ddlLD.DataSource = filterList;
      ddlLD.DataBind();
      ddlOD.DataSource = filterList;
      ddlOD.DataBind();

      ddlLD.SelectedIndex = 0;
      pnlLD.Visible = false;
      pnlLDTo.Visible = false;
      ddlOD.SelectedIndex = 0;
      pnlOD.Visible = false;
      pnlODTo.Visible = false;
    }
    #endregion
    #region "Initialize countries (in user's scope)"
    CountryList countries = HyperCatalog.Shared.SessionState.User.Countries;
    if (countries != null && countries.Count > 0)
    {
      ddlCountries.DataSource = countries;
      ddlCountries.DataTextField = "Name";
      ddlCountries.DataValueField = "Code";
      ddlCountries.DataBind();

      ddlCountries.SelectedIndex = 0;
    }
    #endregion
    #region "Initialize classes"
    string filter = "LevelId = 1"; // Retrieve all class
    ItemList items = Item.GetAll(filter);
    if (items != null)
    {
      items.Sort("Name");
      lClasses.DataSource = items;
      lClasses.DataTextField = "Name";
      lClasses.DataValueField = "Id";
      lClasses.DataBind();
    }
    #endregion
    #region "Initialize product scope"
    rdMyProduct.Checked = true;
    #endregion
    #region "Initialize view"
    rdList.Checked = true;
    #endregion

    pnlParameters.Visible = true;
    pnlList.Visible = pnlGraphic.Visible = false;
  }
  private void Submit()
  {
    bool invalidLifeDate = false;
    bool invalidObsoDate = false;

    #region "Live date"
    if (!FilterLiveDate.Equals("any"))
    {
      lbMissingLD1.Visible = false;
      lbMissingLD2.Visible = false;
      if (!LiveDate1.HasValue)
        lbMissingLD1.Visible = true;

      if (FilterLiveDate.Equals("from"))
      {
        if (LiveDate2.HasValue && !lbMissingLD1.Visible)
          invalidLifeDate = (((TimeSpan)LiveDate1.Value.Subtract(LiveDate2.Value)).Days > 0);
        else
          lbMissingLD2.Visible = true;
      }
    }
    #endregion
    #region "Obsolete date"
    if (!FilterObsoleteDate.Equals("any"))
    {
      lbMissingOD1.Visible = false;
      lbMissingOD2.Visible = false;
      if (!ObsoleteDate1.HasValue)
        lbMissingOD1.Visible = true;

      if (FilterObsoleteDate.Equals("from"))
      {
        if (ObsoleteDate2.HasValue && !lbMissingOD1.Visible)
          invalidObsoDate = (((TimeSpan)ObsoleteDate1.Value.Subtract(ObsoleteDate2.Value)).Days > 0);
        else
          lbMissingOD2.Visible = true;
      }
    }
    #endregion

    #region "View"
    bool isGraphic = rdGraphic.Checked;
    #endregion

    if (!lbMissingLD1.Visible && !lbMissingLD2.Visible && !lbMissingOD1.Visible && !lbMissingOD2.Visible && !invalidObsoDate && !invalidLifeDate)
    {
      RetrieveParameter();

      if (isGraphic)
      {
        vGraphic.Refresh();
        pnlGraphic.Visible = true;
        pnlParameters.Visible = pnlList.Visible = false;
      }
      else
      {
        ViewState["Filter"] = null;
        ViewState["CountryStyle"] = null;
        vList.Refresh();
        pnlList.Visible = true;
        pnlParameters.Visible = pnlGraphic.Visible = false;
      }
    }
    else
    {
      string msg = string.Empty;
      string rl = "\\n";
      if (lbMissingLD1.Visible || lbMissingLD2.Visible)
      {
        if (msg.Length > 0)
          msg += rl;
        msg += "- Live date is missing";
      }
      if (lbMissingOD1.Visible || lbMissingOD2.Visible)
      {
        if (msg.Length > 0)
          msg += rl;
        msg += "- Obsolete date is missing";
      }
      if (invalidLifeDate || invalidObsoDate)
      {
        if (invalidLifeDate)
        {
          if (msg.Length > 0)
            msg += rl;
          msg += "- Life date for TO parameter less than the life date for FROM parameter.";
          lbMissingLD1.Visible = lbMissingLD2.Visible = true;
        }
        if (invalidObsoDate)
        {
          if (msg.Length > 0)
            msg += rl;
          msg += "- Obsolete date for TO parameter less than the obsolete date for FROM parameter.";
          lbMissingOD1.Visible = lbMissingOD2.Visible = true;
        }

      }

      if (msg.Length > 0)
        Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "Error", "<script>alert('" + msg + "');</script>");
    }
  }
  private void RetrieveParameter()
  {
    #region "View"
    bool isGraphic = rdGraphic.Checked;
    #endregion

    if (isGraphic)
    {
      vGraphic.StatusList = this.StatusList;
      vGraphic.WorkflowStatus = this.WorkflowStatus;
      vGraphic.Countries = this.Countries;
      vGraphic.Classes = this.Classes;
      vGraphic.FilterLiveDate = this.FilterLiveDate;
      vGraphic.FilterObsoleteDate = this.FilterObsoleteDate;
      vGraphic.LiveDate1 = this.LiveDate1;
      vGraphic.LiveDate2 = this.LiveDate2;
      vGraphic.ObsoleteDate1 = this.ObsoleteDate1;
      vGraphic.ObsoleteDate2 = this.ObsoleteDate2;
      vGraphic.Scope = this.Scope;
    }
    else
    {
      vList.StatusList = this.StatusList;
      vList.WorkflowStatus = this.WorkflowStatus;
      vList.Countries = this.Countries;
      vList.Classes = this.Classes;
      vList.FilterLiveDate = this.FilterLiveDate;
      vList.FilterObsoleteDate = this.FilterObsoleteDate;
      vList.LiveDate1 = this.LiveDate1;
      vList.LiveDate2 = this.LiveDate2;
      vList.ObsoleteDate1 = this.ObsoleteDate1;
      vList.ObsoleteDate2 = this.ObsoleteDate2;
      vList.Scope = this.Scope;
    }
  }
  #endregion
}
