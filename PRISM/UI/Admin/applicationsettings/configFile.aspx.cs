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
  public partial class ConfigFile : HCPage
  {
    protected Business.CapabilitiesEnum updateCapability = Business.CapabilitiesEnum.MANAGE_USERS;

    private System.Configuration.Configuration _configuration;
    protected System.Configuration.Configuration configuration
    {
      get
      {
        if (_configuration == null && ddlApps.SelectedIndex >= 0)
        {
          Business.ApplicationComponent component = HyperCatalog.Shared.SessionState.CacheComponents[Convert.ToInt32(ddlApps.SelectedValue)];
          if (component != null)
          {
            string path = component.URI;
            if (path != null && path != string.Empty)
            {
              if (path.StartsWith("http://") || path.StartsWith("https://"))
              {
                path = path.Replace("http://", "").Replace("https://", "");
                path = path.Remove(0, path.IndexOf('/'));
              }
              try
              {
                _configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(path.Substring(0, path.LastIndexOf('/')));
              }
              catch
              {
                Response.Write(String.Format("Component \"{0}\" is not located on this server.", component.Name));
              }
            }
          }
        }
        return _configuration;
      }
      set
      {
        _configuration = null;
      }
    }

    protected override void OnLoad(EventArgs e)
    {
      UITools.CheckConnection(Page);
      propertiesMsgLbl.Visible = false;

      #region if (!IsPostBack)
      if (!IsPostBack)
      {
        LoadPage();
        DataBind();
      }
      #endregion

      base.OnLoad(e);
    }
    protected override void OnPreRender(EventArgs e)
    {
      mainToolBar.Items.FromKeyButton("Save").Enabled = !HyperCatalog.Shared.SessionState.User.IsReadOnly && HyperCatalog.Shared.SessionState.User.HasCapability(updateCapability);

      base.OnPreRender(e);
    }

    #region Data load & bind
    private void LoadPage()
    {
      DropDownList ddlApps = (DropDownList)mainToolBar.FindControl("ddlApps");
      if (ddlApps != null)
        using (Business.ApplicationComponentCollection ds = Business.ApplicationComponent.GetAll(Business.ApplicationComponentTypes.WebUI | HyperCatalog.Business.ApplicationComponentTypes.WebService))
        {
          ddlApps.DataSource = ds;
        }
      ddlApps.DataBind();
      ddlApps.SelectedValue = HyperCatalog.Shared.SessionState.CacheComponents["Crystal_UI"].Id.ToString();

      //LoadGrid();
    }
    protected void ddlApps_SelectedIndexChanged(object sender, EventArgs e)
    {
      configuration = null;
      //LoadGrid();
      DataBind();
    }
    protected void rptLocations_ItemCreated(object sender, RepeaterItemEventArgs e)
    {
      //Repeater rptLocationKeys = (Repeater)e.Item.FindControl("rptLocationKeys");
      //if (rptLocationKeys != null)
      //{
      //  System.Configuration.ConfigurationLocation configLocation = (System.Configuration.ConfigurationLocation)e.Item.DataItem;
      //  rptLocationKeys.DataSource = configLocation.OpenConfiguration().AppSettings.Settings.AllKeys;
      //}
    }
    #endregion

    public void mainToolBar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      switch (be.Button.Key)
      {
        case "Export":
          //Utils.ExportToExcel(componentsGrid, "ApplicationComponents", "ApplicationComponents");
          break;
        case "Save":
          //foreach (RepeaterItem item in rptKeys.Items)
          //{
          //  Label lbKeyName = (Label)item.FindControl("lbKeyName");
          //  TextBox txtKeyValue = (TextBox)item.FindControl("txtKeyValue");
          //  if (lbKeyName != null && txtKeyValue != null)
          //  {
          //    if (configuration.AppSettings.Settings[lbKeyName.Text] != null)
          //      configuration.AppSettings.Settings[lbKeyName.Text].Value = txtKeyValue.Text;
          //    else
          //      Response.Write("null");
          //  }
          //  else
          //  {
          //    string test = "";
          //    foreach (Control control in item.Controls)
          //      test += " " + (control is WebControl ? ((WebControl)control).ID : control.ToString());
          //    Response.Write("null " + test + "<BR/>");
          //  }
          //  //System.Configuration.ConfigurationManager.AppSettings.Set(lblKeyName.Text, txtKeyName.Text);
          //}
          //configuration.Save();
          break;
        case "Delete":
          break;
      }
    }

  }
}
