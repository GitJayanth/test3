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
  /// Description résumée de Components.
  /// </summary>
  public partial class Components : HCPage
  {
    #region UI
    private string batchDescriptionArray
    {
      get
      {
        return (string)ViewState["batchDescriptionArray"];
      }
      set
      {
        ViewState["batchDescriptionArray"] = value;
      }
    }
    private string batchServerArray
    {
      get
      {
        return (string)ViewState["batchServerArray"];
      }
      set
      {
        ViewState["batchServerArray"] = value;
      }
    }
    private string batchActionArray
    {
      get
      {
        return (string)ViewState["batchActionArray"];
      }
      set
      {
        ViewState["batchActionArray"] = value;
      }
    }
    #endregion

    #region Business objects
    protected HyperCatalog.Business.ApplicationComponentCollection appComponents = HyperCatalog.Business.ApplicationSettings.Components;
    protected HyperCatalog.Business.ApplicationComponent currentComponent
    {
      get
      {
        return (HyperCatalog.Business.ApplicationComponent)ViewState["currentComponent"];
      }
      set
      {
        ViewState["currentComponent"] = value;

        txtComponentIdValue.Text = value != null ? value.Id.ToString() : "";
        idRow.Visible = value != null;
        txtComponentNameValue.Text = value != null ? value.Name : "";
        txtComponentDescriptionValue.Value = value != null ? value.Description : "";
        txtComponentVersionNumberValue.Text = value != null ? value.VersionNumber : "";

        txtComponentTypeValue.ClearSelection();
        if (value != null)
        {
          ListItem typeItem = txtComponentTypeValue.Items.FindByValue(value.TypeCode);
          if (typeItem != null)
            typeItem.Selected = true;
        }

        txtComponentURIValue.Text = value != null ? value.URI : "";
        txtComponentLoginValue.Text = value != null ? value.Login : "";
        string pass = null;
        if (value != null && value.Password != null)
          for (int i = 0; i < value.Password.Length; i++)
            pass += i;
        txtComponentPasswordValue.Value = pass;
        txtComponentOptionsValue.Text = value != null ? value.Options : "";

        txtComponentDBValue.ClearSelection();
        txtComponentHWValue.ClearSelection();
        if (value != null)
        {
          if (value.DatabaseComponentId > 0)
            txtComponentDBValue.SelectedValue = value.DatabaseComponentId.ToString();
          if (value.HardwareComponentId > 0)
            txtComponentHWValue.SelectedValue = value.HardwareComponentId.ToString();
        }

        if (value != null && value.Action != null)
          setActionValue(value.Action);
        else
        {
          ComponentActionTypeValue.ClearSelection();
          txtComponentActionValue.Text = "";
        }

        txtComponentGroupByValue.Text = value != null ? value.UIGroupBy : "";
      }
    }
    protected Business.CapabilitiesEnum updateCapability = Business.CapabilitiesEnum.MANAGE_USERS;
    #endregion

    protected override void OnLoad(EventArgs e)
    {
      UITools.CheckConnection(Page);
      propertiesMsgLbl.Visible = false;

      #region url parameters
      //      if (Request["ReturnUrl"]!=null)
      //      {
      //        propertiesToolBar.Items.FromKeyButton("List").Text = "Back";
      //        try
      //        {
      //          Page.ClientScript.RegisterStartupScript(this.GetType(),"1","<SCRIPT>var returnUrl = \"" + Request["ReturnUrl"] + "\";</SCRIPT>");
      //        }
      //        catch{}
      //      }
      //
      if (Request["c"] != null)
      {
        try
        {
          UpdateDataEdit(Convert.ToInt32(Request["c"]));
        }
        catch { }
      }
      #endregion

      #region if (!IsPostBack)
      if (!IsPostBack)
      {
        LoadGrid();
        DataBind();

        Business.ApplicationComponentCollection ds;
        using (ds = Business.ApplicationComponent.GetAll(Business.ApplicationComponentTypes.Database))
        {
          txtComponentDBValue.DataSource = ds;
        }
        txtComponentDBValue.DataTextField = "Name";
        txtComponentDBValue.DataValueField = "Id";
        txtComponentDBValue.DataBind();
        txtComponentDBValue.Items.Insert(0, new ListItem("---", ""));
        using (ds = Business.ApplicationComponent.GetAll(Business.ApplicationComponentTypes.Hardware))
        {
          txtComponentHWValue.DataSource = ds;
        }
        txtComponentHWValue.DataTextField = "Name";
        txtComponentHWValue.DataValueField = "Id";
        txtComponentHWValue.DataBind();
        txtComponentHWValue.Items.Insert(0, new ListItem("---", ""));

        if (batchDescriptionArray == null || batchServerArray == null || batchActionArray == null)
        {
          batchDescriptionArray = null;
          batchServerArray = null;
          batchActionArray = null;

          foreach (Business.ApplicationComponent batch in appComponents)
          {
            batchDescriptionArray += (batchDescriptionArray == null ? "" : ",") + "'" + (batch.Description != null ? batch.Description.Replace("'", "\\'") : "") + "'";
            batchServerArray += (batchServerArray == null ? "" : ",") + "'" + (batch.URI != null ? batch.URI.Replace("'", "\\'") : "") + "'";
            batchActionArray += (batchActionArray == null ? "" : ",") + "'" + (batch.Action != null ? batch.Action.Replace("'", "\\'") : "") + "'";
          }
        }
      }
      #endregion

      Page.ClientScript.RegisterArrayDeclaration("batchDescriptionArray", batchDescriptionArray);
      Page.ClientScript.RegisterArrayDeclaration("batchServerArray", batchServerArray);
      Page.ClientScript.RegisterArrayDeclaration("batchActionArray", batchActionArray);

      base.OnLoad(e);
    }
    protected override void OnPreRender(EventArgs e)
    {
      mainToolBar.Items.FromKeyButton("Add").Enabled = propertiesToolBar.Items.FromKeyButton("Save").Enabled = !HyperCatalog.Shared.SessionState.User.IsReadOnly && HyperCatalog.Shared.SessionState.User.HasCapability(updateCapability);

      Page.ClientScript.RegisterStartupScript(this.GetType(), "start1", "batchChanged(document.getElementById('" + txtComponentDBValue.ClientID + "'),document.getElementById('newDescriptionDB'));" + Environment.NewLine + "batchChanged(document.getElementById('" + txtComponentHWValue.ClientID + "'),document.getElementById('newDescriptionHW'));" + Environment.NewLine, true);
      Page.ClientScript.RegisterStartupScript(this.GetType(), "start2", "var txtComponentTypeValue = document.getElementById('" + txtComponentTypeValue.ClientID + "');" + Environment.NewLine + "var ComponentActionTypeValue = document.getElementById('" + ComponentActionTypeValue.ClientID + "');", true);

      base.OnPreRender(e);
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
      mainToolBar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(mainToolBar_ButtonClicked);
      propertiesToolBar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(propertiesToolBar_ButtonClicked);
      componentsGrid.InitializeRow += new Infragistics.WebUI.UltraWebGrid.InitializeRowEventHandler(componentsGrid_InitializeRow);
      componentsGrid.DemandLoad += new Infragistics.WebUI.UltraWebGrid.DemandLoadEventHandler(componentsGrid_DemandLoad);
    }

    #endregion

    #region Data load & bind
    private void LoadGrid()
    {
      HyperCatalog.Business.ApplicationSettings.Reset();
      componentsGrid.DataSource = appComponents;

      if (componentsGrid.Bands.Count == 1)
        AddBand(componentsGrid);
    }
    private void AddBand(Infragistics.WebUI.UltraWebGrid.UltraWebGrid grid)
    {
      UltraGridBand newBand = new UltraGridBand();
      grid.Bands.Add(newBand);
      newBand.ColHeadersVisible = ShowMarginInfo.No;
      newBand.DataKeyField = grid.Bands[0].DataKeyField;
      newBand.Columns.CopyFrom(grid.Bands[0].Columns);

      ((TemplatedColumn)newBand.Columns[1]).CellTemplate = ((TemplatedColumn)grid.Bands[0].Columns[1]).CellTemplate;
    }
    private void componentsGrid_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      Business.ApplicationComponent comp = (Business.ApplicationComponent)e.Data;
      e.Row.ShowExpand = comp.DatabaseComponentId >= 0 || comp.HardwareComponentId >= 0;
    }
    #endregion

    #region Events
    #region Toolbar events
    private void mainToolBar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      switch (be.Button.Key)
      {
        case "Add":
          UpdateDataEdit(-1);
          break;
        case "Export":
          Utils.ExportToExcel(componentsGrid, "ApplicationComponents", "ApplicationComponents");
          break;
        case "Consistency":
          panelGrid.Visible = false;
          panelProperties.Visible = false;

          break;
      }
    }
    private void propertiesToolBar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      switch (be.Button.Key)
      {
        case "List":
          panelGrid.Visible = true;
          panelProperties.Visible = false;
          currentComponent = null;
          break;
        case "Save":
          if (!HyperCatalog.Shared.SessionState.User.IsReadOnly && HyperCatalog.Shared.SessionState.User.HasCapability(updateCapability))
          {
            if (appComponents.Contains(txtComponentNameValue.Text) && (currentComponent == null || !currentComponent.Name.Equals(txtComponentNameValue.Text)))
            {
              Tools.UITools.SetMessage(propertiesMsgLbl, "Component \"" + currentComponent.Name + "\" already exists.", Tools.UITools.MessageLevel.Error);
            }
            if (txtComponentNameValue.Text == string.Empty)
            {
              Tools.UITools.SetMessage(propertiesMsgLbl, "Component name is mandatory.", Tools.UITools.MessageLevel.Error);
            }
            else if (currentComponent != null)
            {
              currentComponent.Name = txtComponentNameValue.Text;
              currentComponent.Description = txtComponentDescriptionValue.Value;
              currentComponent.TypeCode = txtComponentTypeValue.SelectedValue;
              currentComponent.URI = txtComponentURIValue.Text;
              currentComponent.Login = txtComponentLoginValue.Text;
              if (txtComponentPasswordValue.Value != "")
                currentComponent.Password = txtComponentPasswordValue.Value;
              currentComponent.Options = txtComponentOptionsValue.Text;
              currentComponent.Action = getActionValue();
              currentComponent.UIGroupBy = txtComponentGroupByValue.Text;
              currentComponent.VersionNumber = txtComponentVersionNumberValue.Text;

              if (currentComponent.SaveWithTrace(HyperCatalog.Shared.SessionState.User.Id))
              {
                Tools.UITools.SetMessage(propertiesMsgLbl, "Component \"" + currentComponent.Name + "\" updated.", Tools.UITools.MessageLevel.Information);
                LoadGrid();
                DataBind();
              }
              else
                Tools.UITools.SetMessage(propertiesMsgLbl, "Component \"" + currentComponent.Name + "\" could not be updated.", Tools.UITools.MessageLevel.Error);
            }
            else
            {
              currentComponent = appComponents.Add(txtComponentNameValue.Text, txtComponentDescriptionValue.Value, txtComponentTypeValue.SelectedValue, txtComponentURIValue.Text, txtComponentLoginValue.Text, txtComponentPasswordValue.Value, txtComponentOptionsValue.Text, getActionValue(), null, txtComponentGroupByValue.Text, txtComponentVersionNumberValue.Text);
              if (currentComponent != null && currentComponent.SaveWithTrace(HyperCatalog.Shared.SessionState.User.Id))
              {
                Tools.UITools.SetMessage(propertiesMsgLbl, "Component \"" + currentComponent.Name + "\" insertion succeeded.", Tools.UITools.MessageLevel.Information);
                panelGrid.Visible = true;
                panelProperties.Visible = false;
                currentComponent = null;
                LoadGrid();
                DataBind();
              }
              else
                Tools.UITools.SetMessage(propertiesMsgLbl, "Component insertion failed.", Tools.UITools.MessageLevel.Error);
            }
          }
          else
            Tools.UITools.SetMessage(propertiesMsgLbl, "You're not allowed to create or modify components.", Tools.UITools.MessageLevel.Warning);

          break;
        case "Delete":
          break;
      }
    }
    #endregion

    #region Grid events
    protected void UpdateGridItem(object sender, System.EventArgs e)
    {
      Infragistics.WebUI.UltraWebGrid.CellItem cellItem = ((Infragistics.WebUI.UltraWebGrid.CellItem)((System.Web.UI.WebControls.LinkButton)sender).Parent);
      UpdateDataEdit((int)cellItem.Cell.Row.Cells.FromKey("Id").Value);
    }
    private void UpdateDataEdit(int selContainerId)
    {
      currentComponent = selContainerId >= 0 ? appComponents[selContainerId] : null;

      panelGrid.Visible = false;
      panelProperties.Visible = true;
    }
    private void componentsGrid_DemandLoad(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      Business.ApplicationComponent comp = (Business.ApplicationComponent)appComponents[(int)e.Row.Cells.FromKey("Id").Value];
      if (comp != null)
      {
        e.Row.Expanded = true;
        UltraGridBand band = componentsGrid.Bands[e.Row.BandIndex + 1];

        if (comp.DatabaseComponentId >= 0)
        {
          Business.ApplicationComponent dbComp = comp.DatabaseComponent;
          UltraGridRow newRow = new UltraGridRow(new object[] { comp.DatabaseComponentId, dbComp.Name, dbComp.Type, dbComp.Description });
          e.Row.Rows.Add(newRow);

          newRow.ShowExpand = (dbComp.DatabaseComponentId >= 0 || dbComp.HardwareComponentId >= 0);
        }

        if (comp.HardwareComponentId >= 0)
        {
          Business.ApplicationComponent hwComp = comp.HardwareComponent;
          UltraGridRow newRow = new UltraGridRow(new object[] { comp.HardwareComponentId, hwComp.Name, hwComp.Type, hwComp.Description });
          e.Row.Rows.Add(newRow);

          newRow.ShowExpand = (hwComp.DatabaseComponentId >= 0 || hwComp.HardwareComponentId >= 0);
        }
      }

      if (componentsGrid.Bands[e.Row.BandIndex + 1] == null)
        AddBand(componentsGrid);
    }

    #endregion
    #endregion

    #region Private methods
    private string getActionValue()
    {
      switch (ComponentActionTypeValue.SelectedValue)
      {
        case "sql":
          return "sql:" + txtComponentActionValue.Text;
        case "sp":
          return "sp:" + txtComponentActionValue.Text;
        default:
          return txtComponentActionValue.Text;
      }
    }

    private void setActionValue(string actionValue)
    {
      if (actionValue.ToLower().StartsWith("sql:"))
      {
        ComponentActionTypeValue.SelectedValue = "sql";
        txtComponentActionValue.Text = actionValue.Substring(4, actionValue.Length - 4).Trim();
      }
      else if (actionValue.ToLower().StartsWith("sp:"))
      {
        ComponentActionTypeValue.SelectedValue = "sp";
        txtComponentActionValue.Text = actionValue.Substring(3, actionValue.Length - 3).Trim();
      }
      else
      {
        txtComponentActionValue.Text = actionValue;
      }
    }

    #endregion
  }
}
