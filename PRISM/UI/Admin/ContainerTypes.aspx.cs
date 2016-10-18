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
using HyperCatalog.Business;
using HyperCatalog.Shared;
#endregion

namespace HyperCatalog.UI.Admin
{
  /// <summary>
  /// Display list of container types
  ///		--> Add new container type
  ///		--> Export in Excel
  ///		--> Filter on all fields of the grid
  ///		--> Modify container type
  /// </summary>
  public partial class ContainerTypes : HCPage
  {
		#region Declarations
    protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator2;
    protected System.Web.UI.WebControls.Label Label3;
    protected System.Web.UI.WebControls.Label Label4;
    protected System.Web.UI.WebControls.Label Label5;
    protected System.Web.UI.WebControls.Label Label6;
		#endregion

    #region Code généré par le Concepteur Web Form
    override protected void OnInit(EventArgs e)
    {
      //
      // CODEGEN : Cet appel est requis par le Concepteur Web Form ASP.NET.
      //
      InitializeComponent();
      txtFilter.AutoPostBack = false;
      txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);"); Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "<script defer=true>var txtFilterField;txtFilterField = document.getElementById('" + txtFilter.ClientID + "');</script>");       
      base.OnInit(e);
    }
		
    /// <summary>
    ///		Méthode requise pour la prise en charge du concepteur - ne modifiez pas
    ///		le contenu de cette méthode avec l'éditeur de code.
    /// </summary>
    private void InitializeComponent()
    {
			this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);
			this.dg.InitializeRow += new Infragistics.WebUI.UltraWebGrid.InitializeRowEventHandler(this.dg_InitializeRow);

		}
    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
    {
      #region Capabilities
      if (SessionState.User.IsReadOnly)
      {
        uwToolbar.Items.FromKeyButton("Add").Enabled = false;
      }
      if (!SessionState.User.HasCapability(CapabilitiesEnum.EXTEND_CONTENT_MODEL))
      {
        UITools.HideToolBarSeparator(uwToolbar, "AddSep");
        UITools.HideToolBarButton(uwToolbar, "Add");
      }
      #endregion
      if (Request["filter"] != null)
      {
        txtFilter.Text = Request["filter"].ToString();
      }

      if (!Page.IsPostBack)
      {
        UpdateDataView();
      }
    }

    private void UpdateDataView()
    {
      string sSql = string.Empty;

			string filter = txtFilter.Text;
      if (filter!=string.Empty)
      {
				string cleanFilter = filter.Replace("'", "''").ToLower();
				cleanFilter = cleanFilter.Replace("[", "[[]");
				cleanFilter = cleanFilter.Replace("_", "[_]");
				cleanFilter = cleanFilter.Replace("%", "[%]");

        sSql = " LOWER(ContainerType) like '%" + cleanFilter +"%' ";
      }

      using (ContainerTypeList containerTypes = ContainerType.GetAll(sSql))
      {
        if (containerTypes != null)
        {
          if (containerTypes.Count > 0)
          {
            dg.DataSource = containerTypes;
            Utils.InitGridSort(ref dg);
            dg.DataBind();

            lbNoresults.Visible = false;
            dg.Visible = true;
          }
          else
          {
            if (txtFilter.Text.Length > 0)
              lbNoresults.Text = "No record match your search (" + txtFilter.Text + ")";

            lbNoresults.Visible = true;
            dg.Visible = false;
          }

          panelGrid.Visible = true;
          lbTitle.Text = "Container types list";
        }
      }
		}


    protected void UpdateDataEdit(string selContainerTypeCode)
    {
			if ((selContainerTypeCode == null) || (selContainerTypeCode.Length == 0))
				selContainerTypeCode = " ";

      ContainerType containerType = ContainerType.GetByKey(Convert.ToChar(selContainerTypeCode));
      if (containerType == null)
      { 
        webTab.Tabs.GetTab(0).ContentPane.TargetUrl = "./ContainerTypes/containertype_properties.aspx?d=";
        lbTitle.Text = "Container type: New";
        webTab.Tabs.GetTab(1).Visible = false;
      
      }
      else
      {
        webTab.Tabs.GetTab(0).ContentPane.TargetUrl = "./ContainerTypes/containertype_properties.aspx?d=" + selContainerTypeCode;
        webTab.Tabs.GetTab(1).ContentPane.TargetUrl = "./ContainerTypes/containertype_containers.aspx?d=" + selContainerTypeCode;
				string sqlFilter = " ContainerTypeCode = '"+containerType.Code+"'";
        using (ContainerList c = HyperCatalog.Business.Container.GetAll(sqlFilter))
        {
          webTab.Tabs.GetTab(1).Text = "Containers (" + c.Count + ")";
        }
				webTab.Tabs.GetTab(1).Visible = true;      
        lbTitle.Text = "Container type: " + containerType.Name;
      }
      panelGrid.Visible = false;
      webTab.Visible = true;
    }

    private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;
      UITools.HiglightGridRowFilter(ref r, txtFilter.Text);
    }

    private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      if (btn == "export")
      {
        Utils.ExportToExcel(dg, "ContainerTypes", "ContainerTypes");
      }
      if (btn == "add")
      {
        UpdateDataEdit(string.Empty);
      }
    }

    // "Name" Link Button event handler
    protected void UpdateGridItem(object sender, System.EventArgs e)
    {
			if (sender != null)
			{
				Infragistics.WebUI.UltraWebGrid.CellItem cellItem = ((Infragistics.WebUI.UltraWebGrid.CellItem)((System.Web.UI.WebControls.LinkButton)sender).Parent);      
				string sId = cellItem.Cell.Row.Cells.FromKey("Id").Text;
			
				sId = Utils.CReplace(sId, "<font color=red><b>", "", 1);
				sId = Utils.CReplace(sId, "</b></font>", "", 1);
			
				UpdateDataEdit(sId);
			}
    }
  }
}
