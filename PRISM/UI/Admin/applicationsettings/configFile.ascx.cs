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
  /// Description résumée de ConfigFile.
  /// </summary>
  public partial class ConfigFileControl : System.Web.UI.UserControl
  {
    protected Business.CapabilitiesEnum updateCapability = Business.CapabilitiesEnum.MANAGE_USERS;

    #region System.Configuration objects
    private System.Configuration.Configuration _configuration;
    protected System.Web.Configuration.SystemWebSectionGroup _systemWebSectionGroup;

    public System.Configuration.Configuration configuration
    {
      get
      {
        return _configuration;
      }
      set
      {
        _configuration = value;
        if (_configuration != null)
          _systemWebSectionGroup = (System.Web.Configuration.SystemWebSectionGroup)configuration.GetSectionGroup("system.web");
      }
    }
    protected System.Web.Configuration.TraceSection traceSection
    {
      get
      {
        return _systemWebSectionGroup != null ? _systemWebSectionGroup.Trace : null;
      }
    }
    protected System.Web.Configuration.SessionStateSection sessionSection
    {
      get
      {
        return _systemWebSectionGroup != null ? _systemWebSectionGroup.SessionState : null;
      }
    }
    protected System.Web.Configuration.CustomErrorsSection customErrorsSection
    {
      get
      {
        return _systemWebSectionGroup != null ? _systemWebSectionGroup.CustomErrors : null;
      }
    }
    protected System.Web.Configuration.AuthenticationSection authenticationSection
    {
      get
      {
        return _systemWebSectionGroup != null ? _systemWebSectionGroup.Authentication : null;
      }
    }
    protected System.Web.Configuration.AuthorizationSection authorizationSection
    {
      get
      {
        return _systemWebSectionGroup != null ? _systemWebSectionGroup.Authorization : null;
      }
    }
    protected System.Web.Configuration.CompilationSection compilationSection
    {
      get
      {
        return _systemWebSectionGroup != null ? _systemWebSectionGroup.Compilation : null;
      }
    }
    protected System.Web.Configuration.XhtmlConformanceSection xhtmlConformanceSection
    {
      get
      {
        return _systemWebSectionGroup != null ? _systemWebSectionGroup.XhtmlConformance : null;
      }
    }
    protected System.Web.Configuration.HttpRuntimeSection httpRunTimeSection
    {
      get
      {
        return _systemWebSectionGroup != null ? _systemWebSectionGroup.HttpRuntime : null;
      }
    }
    protected System.Web.Configuration.GlobalizationSection globalizationSection
    {
      get
      {
        return _systemWebSectionGroup != null ? _systemWebSectionGroup.Globalization : null;
      }
    }
    protected System.Web.Configuration.PagesSection pagesSection
    {
      get
      {
        return _systemWebSectionGroup != null ? _systemWebSectionGroup.Pages : null;
      }
    }
    protected System.Web.Configuration.HttpHandlersSection httpHandlersSection
    {
      get
      {
        return _systemWebSectionGroup != null ? _systemWebSectionGroup.HttpHandlers : null;
      }
    }
    protected System.Web.Configuration.HttpModulesSection httpModulesSection
    {
      get
      {
        return _systemWebSectionGroup != null ? _systemWebSectionGroup.HttpModules : null;
      }
    }
    protected System.Web.Configuration.HttpCookiesSection httpCookiesSection
    {
      get
      {
        return _systemWebSectionGroup != null ? _systemWebSectionGroup.HttpCookies : null;
      }
    }
    #endregion

    protected override void OnDataBinding(EventArgs e)
    {
      base.OnDataBinding(e);
      System.Web.Configuration.HttpHandlerAction t;

      if (traceSection != null)
        ddlTraceMode.SelectedValue = traceSection.TraceMode.ToString();

      if (sessionSection != null)
      {
        ddlSessionMode.SelectedValue = sessionSection.Mode.ToString();
        ddlSessionCookieLess.SelectedValue = sessionSection.Cookieless.ToString();
      }

      if (customErrorsSection != null)
        ddlCustomErrorsMode.SelectedValue = customErrorsSection.Mode.ToString();

      if (authenticationSection != null)
      {
        ddlAuthenticationMode.SelectedValue = authenticationSection.Mode.ToString();
        ddlAuthenticationFormsCookieLess.SelectedValue = authenticationSection.Forms.Cookieless.ToString();
        ddlAuthenticationFormsProtection.SelectedValue = authenticationSection.Forms.Protection.ToString();
      }

      if (globalizationSection != null)
      {
        try
        {
          ddlGlobalizationFileEncoding.SelectedValue = globalizationSection.FileEncoding.CodePage.ToString();
          ddlGlobalizationRequestEncoding.SelectedValue = globalizationSection.RequestEncoding.CodePage.ToString();
          ddlGlobalizationResponseEncoding.SelectedValue = globalizationSection.ResponseEncoding.CodePage.ToString();
        }
        catch { }
      }

      if (pagesSection != null)
      {
        ddlPagesCompilationMode.SelectedValue = pagesSection.CompilationMode.ToString();
      }
    }

    protected void rptLocations_ItemCreated(object sender, RepeaterItemEventArgs e)
    {
      //Panel subLocation = (Panel)e.Item.FindControl("subLocation");
      //if (subLocation != null)
      //{
      //  ConfigFileControl subConfigFile = new ConfigFileControl();
      //  subConfigFile.configuration = ((System.Configuration.ConfigurationLocation)e.Item.DataItem).OpenConfiguration();
      //  subLocation.Controls.Add(subConfigFile);
      //}
    }
    protected void chkProperty_CheckedChanged(object sender, EventArgs e)
    {
      CheckBox chkProperty = (CheckBox)sender;
      if (chkProperty != null)
      {
        RepeaterItem propItem = (RepeaterItem)chkProperty.NamingContainer;
        if (propItem != null)
        {
          RepeaterItem sectionItem = (RepeaterItem)propItem.NamingContainer.NamingContainer;
          try
          {
            chkProperty.Checked = Convert.ToBoolean(((System.Reflection.PropertyInfo)propItem.DataItem).GetValue(sectionItem.DataItem, null));
          }
          catch { }
        }
      }
    }
    protected void ddl_DataBound(object sender, EventArgs e)
    {
      DropDownList ddl = (DropDownList)sender;
      if (ddl != null)
      {
        RepeaterItem propItem = (RepeaterItem)ddl.NamingContainer;
        if (propItem != null)
        {
          RepeaterItem sectionItem = (RepeaterItem)propItem.NamingContainer.NamingContainer;
          try
          {
            ddl.ClearSelection();
            ListItem ddlItem = ddl.Items.FindByValue(((System.Reflection.PropertyInfo)propItem.DataItem).GetValue(sectionItem.DataItem, null).ToString());
            if (ddlItem != null)
              ddlItem.Selected = true;
          }
          catch {
          }
        }
      }

    }
    protected string displayXml(string xml)
    {
      return "<pre>" + (xml != null ? Server.HtmlEncode(xml.Replace("\t", "")) : "&nbsp;") + "</pre>";
    }
  }
}
