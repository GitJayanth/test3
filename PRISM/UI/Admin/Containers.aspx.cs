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
using Infragistics.WebUI.UltraWebGrid;
using HyperCatalog.Shared;
using HyperComponents.Data.dbAccess;
using HyperCatalog.Business;
using HyperCatalog.UI.Tools;
#endregion

#region History
	// Change drop down list for container group (CHARENSOL Mickael 09/11/05)
#endregion

namespace HyperCatalog.UI.Admin
{
	/// <summary>
	/// Display list of containers
	///		--> Add a new container
	///		--> Modify container
	///		--> Export in Excel
	///		--> Filter on container group
	///		--> Filter on Tag, Name and Defintion fields
	/// </summary>
	public partial class Containers : HCPage
	{
		#region Declarations
		protected System.Web.UI.WebControls.TextBox TextBox1;
		protected System.Web.UI.WebControls.Label lbCreator;
		protected System.Web.UI.WebControls.Label lbCreateDate;
		protected System.Web.UI.WebControls.HyperLink HyperLink1;
		protected System.Web.UI.WebControls.ImageButton imgItem;
		protected System.Web.UI.WebControls.DropDownList DropDownList1;
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
		///		Méthode requise pour la prise en charge du concepteur - ne modifiez pas
		///		le contenu de cette méthode avec l'éditeur de code.
		/// </summary>
		private void InitializeComponent()
		{
			this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);
			this.dg.InitializeRow += new Infragistics.WebUI.UltraWebGrid.InitializeRowEventHandler(this.dg_InitializeRow);
			this.dg.InitializeLayout += new Infragistics.WebUI.UltraWebGrid.InitializeLayoutEventHandler(this.dg_InitializeLayout);

		}
		#endregion


    protected void Page_Load(object sender, System.EventArgs e)
    {
      #region Capabilities
      if (SessionState.User.IsReadOnly)
      {
        uwToolbar.Items.FromKeyButton("Add").Enabled = false;
      }
      if (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_DICTIONARY))
      {
        UITools.HideToolBarSeparator(uwToolbar, "AddSep");
        UITools.HideToolBarButton(uwToolbar, "Add");
      }
      #endregion
      if (Request["filter"] != null)
      {
        txtFilter.Text = Request["filter"].ToString();
      }

      if (!IsPostBack)
      {
        if (Request["c"] != null)
        {
          string containerId = Request["c"].ToString();
          UpdateDataEdit(containerId);
        }
        else
        {
          UpdateGroups();
          dg.DataBind();
        }
      }

    }

    private void UpdateGroups()
    {
      webTab.Visible = false;
      ddlContainerGroup.Items.Add((new ListItem("[All Containers]", "-1")));
      for (int i = 0; i < SessionState.AppContainersGroups.Count; i++)
      {
        if (SessionState.AppContainersGroups[i].ContainersCount > 0)
        {
          ddlContainerGroup.Items.Add(new ListItem(SessionState.AppContainersGroups[i].Path + SessionState.AppContainersGroups[i].Name, SessionState.AppContainersGroups[i].Id.ToString()));
        }
      }
      ddlContainerGroup.SelectedIndex = ddlContainerGroup.Items.Count > 1 ? 1 : 0;
      if (SessionState.GroupId.Length > 0)
      {
        ddlContainerGroup.SelectedValue = SessionState.GroupId;
      }
    }
    
		void UpdateDataEdit(string selContainerId)
		{
			panelGrid.Visible = false;
			panelTab.Visible = true;

      string updateParameter = string.Empty;
      if (Request["u"] != null)
        updateParameter = "&u=1";

			webTab.EnableViewState = false;
      webTab.Tabs.GetTab(0).ContentPane.TargetUrl = "./containers/container_properties.aspx?c=" + selContainerId + updateParameter;
			if (selContainerId== "-999")
			{
				// input forms
				webTab.Tabs.GetTab(1).ContentPane.TargetUrl = "";
				webTab.Tabs[1].Visible = false;
				// usage
				webTab.Tabs.GetTab(2).ContentPane.TargetUrl = "";
				webTab.Tabs[2].Visible = false;
				// container dependencies
				webTab.Tabs.GetTab(3).ContentPane.TargetUrl = "";
				webTab.Tabs[3].Visible = false;
				// possible values
				webTab.Tabs.GetTab(4).ContentPane.TargetUrl = "";
				webTab.Tabs[4].Visible = false;

				lbTitle.Text = "Container: New";
			}
			else
			{
				HyperCatalog.Business.Container HCContainer = HyperCatalog.Business.Container.GetByKey(Convert.ToInt32(selContainerId));

        if (HCContainer != null)
        {
          lbTitle.Text = "Container: " + HCContainer.Name + " [" + HCContainer.Tag + "]";

          // Display tab (Input forms, usage and container dependencies)
          using (HyperCatalog.Business.InputFormContainerList contList = HyperCatalog.Business.InputFormContainer.GetAll(" ContainerId=" + selContainerId))
          {
            webTab.Tabs.GetTab(1).ContentPane.TargetUrl = "./containers/container_inputforms.aspx?c=" + selContainerId;
            webTab.Tabs.GetTab(1).Text = webTab.Tabs.GetTab(1).Text + "(" + contList.Count.ToString() + ")";
          }
          string sSql = string.Empty;
          sSql += "_Container_GetPossibleValuesCountAndDistinctCount " + HCContainer.Id.ToString();
          int nbUsed = 0;
          int nbPossibleValues = 0;
          using (Database db = Utils.GetMainDB())
          {
            using (IDataReader rs = db.RunSQLReturnRS(sSql))
            {
              rs.Read();
              nbUsed = (int)rs[0];
              nbPossibleValues = (int)rs[1];
              rs.Close();
            }
          }
          webTab.Tabs.GetTab(2).ContentPane.TargetUrl = "./containers/container_usage.aspx?c=" + selContainerId;
          webTab.Tabs.GetTab(2).Text = webTab.Tabs.GetTab(2).Text + "(" + nbUsed.ToString() + ")";
          if (HCContainer.ContainerTypeCode == 'F')
          {
            using (HyperCatalog.Business.ContainerDependencyList contDepList = HyperCatalog.Business.ContainerDependency.GetAll(" FeatureContainerId=" + selContainerId))
            {
              webTab.Tabs.GetTab(3).ContentPane.TargetUrl = "./containers/container_dependencies.aspx?c=" + selContainerId;
              webTab.Tabs.GetTab(3).Text = webTab.Tabs.GetTab(3).Text + "(" + contDepList.Count.ToString() + ")";
            }
          }
          else
          {
            webTab.Tabs.GetTab(3).ContentPane.TargetUrl = "";
            webTab.Tabs[3].Visible = false;
          }

          webTab.Tabs.GetTab(4).ContentPane.TargetUrl = "./containers/container_possiblevalues.aspx?c=" + selContainerId;
          webTab.Tabs.GetTab(4).Text = webTab.Tabs.GetTab(4).Text + "(" + nbPossibleValues.ToString() + ")";
        }
			}
			webTab.Visible = true;      
		}


		private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
		{
			string btn = be.Button.Key.ToLower();
			if (btn =="add")
			{
				UpdateDataEdit("-999");
			}
			else if (btn == "export")
			{
        Export.ExportContainers(this);
			}
      else if (btn == "btnchangegroup")
			{
        txtFilter.Text = string.Empty;
        SessionState.GroupId = ddlContainerGroup.SelectedValue;
        dg.DisplayLayout.Pager.CurrentPageIndex = 1;
        dg.DataBind();
      }      
		}


		private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
		{
			if (txtFilter.Text.Length > 0)
			{
				Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;
				UITools.HiglightGridRowFilter(ref r, txtFilter.Text);
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


    protected void dg_DataBinding(object sender, System.EventArgs e)
    {
      panelGrid.Visible = true;
      panelTab.Visible = false;

      string groupId = ddlContainerGroup.SelectedValue.Trim();
      lbTitle.Text = "Container list";

      string sSql = String.Empty;
      if (groupId.Trim() != string.Empty)
      {
        if (groupId != "-1")
        {
          sSql += "GroupId =" + groupId;
        }
        lbTitle.Text = ddlContainerGroup.Items[ddlContainerGroup.SelectedIndex].Text;
      }

      string filter = txtFilter.Text;
      if (filter != string.Empty)
      {
        if (sSql != string.Empty)
        {
          sSql += " AND ";
        }
        string cleanFilter = filter.Replace("'", "''").ToLower();
        cleanFilter = cleanFilter.Replace("[", "[[]");
        cleanFilter = cleanFilter.Replace("_", "[_]");
        cleanFilter = cleanFilter.Replace("%", "[%]");

        sSql += " (LOWER(Tag) like '%" + cleanFilter + "%'";
        sSql += " OR LOWER(Name) like '%" + cleanFilter + "%'";
        sSql += " OR LOWER(Definition) like '%" + cleanFilter + "%')";
      }
      using (HyperCatalog.Business.ContainerList HCContainers = HyperCatalog.Business.Container.GetAll(sSql, "Tag"))
      {
        if (HCContainers != null)
        {
          if (HCContainers.Count > 0)
          {
            dg.DataSource = HCContainers;

            dg.Visible = true;
            lbNoresults.Visible = false;
          }
          else
          {
            if (txtFilter.Text.Length > 0)
              lbNoresults.Text = "No record match your search (" + txtFilter.Text + ")";

            dg.Visible = false;
            lbNoresults.Visible = true;
          }
          lbTitle.Text = lbTitle.Text + " (" + HCContainers.Count.ToString() + ")";
        }
      }
    }

		private void dg_InitializeLayout(object sender, Infragistics.WebUI.UltraWebGrid.LayoutEventArgs e)
		{
			Utils.InitGridSort(ref dg);
		}

	}
}