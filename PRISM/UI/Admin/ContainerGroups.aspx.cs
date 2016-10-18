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
	/// Display the list of the container group
	///		--> Add a new container group
	///		--> Modify container group
	///		--> Export in Excel
	///		--> Filter on Name field
	/// </summary>
	public partial class ContainerGroups : HCPage
	{
		#region Declarations
		//protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator2;
		//protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator3;
		//protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator4;
		//protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator5;
		//protected System.Web.UI.WebControls.RequiredFieldValidator Requiredfieldvalidator2;
		protected System.Web.UI.WebControls.TextBox txtDescription;
		protected System.Web.UI.WebControls.Label Name;
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
      if (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_DICTIONARY))
      {
        UITools.HideToolBarButton(uwToolbar, "Add");
        UITools.HideToolBarSeparator(uwToolbar, "AddSep");
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
      webTab.Visible = false;
			string filter = txtFilter.Text;
			if (filter!=string.Empty)
			{
				string cleanFilter = filter.Replace("'", "''").ToLower();
				cleanFilter = cleanFilter.Replace("[", "[[]");
				cleanFilter = cleanFilter.Replace("_", "[_]");
				cleanFilter = cleanFilter.Replace("%", "[%]");

				sSql += " LOWER(ContainerGroup) like '%" + cleanFilter +"%'";
			}

      using (ContainerGroupList groups = ContainerGroup.GetAll(sSql))
      {
        if (groups != null)
        {
          if (groups.Count > 0)
          {
            dg.DataSource = groups;
            Utils.InitGridSort(ref dg, false);
            dg.DataBind();

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

          panelGrid.Visible = true;
          lbTitle.Text = UITools.GetTranslation("Container groups list");
        }
      }
		}


		protected void UpdateDataEdit(string selGroupId)
		{
			if ((selGroupId == null) || (selGroupId.Length == 0))
			{ 
				webTab.Tabs.GetTab(0).ContentPane.TargetUrl = "./ContainerGroups/containergroup_properties.aspx?d=-1";
				lbTitle.Text = "Container group: New";
				webTab.Tabs.GetTab(1).Visible = false;
			}
			else
			{
				string selGroupName = string.Empty;
				ContainerGroup group = ContainerGroup.GetByKey(Convert.ToInt32(selGroupId));
				if (group != null)
				{
					selGroupName = group.Name;
					webTab.Tabs.GetTab(0).ContentPane.TargetUrl = "./ContainerGroups/containergroup_properties.aspx?d=" + selGroupId;
          if (group.Childs.Count == 0)
          {
            webTab.Tabs.GetTab(1).ContentPane.TargetUrl = "./ContainerGroups/containergroup_containers.aspx?d=" + selGroupId;
            if ((group.Containers != null) && (group.Containers.Count > 0))
              webTab.Tabs.GetTab(1).Text = "Containers (" + group.Containers.Count.ToString() +")";
            else
              webTab.Tabs.GetTab(1).Text = "Containers (0)";
            webTab.Tabs.GetTab(1).Visible = true;      
          }
          else {webTab.Tabs.GetTab(1).Visible = false;}



					lbTitle.Text = "Container group: " + selGroupName;
				}
			}

			panelGrid.Visible = false;
			webTab.Visible = true;
		}

		private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
		{
      string path = e.Row.Cells.FromKey("AbsolutePath").Text;
      string sLevel = string.Empty;
      string[] parents = path.Split('/');
      for (int i=2;i<parents.Length;i++)
      {
        sLevel += "&nbsp;&nbsp;&nbsp;&nbsp;";
      }
      string code = e.Row.Cells.FromKey("Code").Text;
      if (code != string.Empty) { code = "[" + code + "] "; }
      e.Row.Cells.FromKey("Name").Text = sLevel + code + e.Row.Cells.FromKey("Name").Text;
      
      Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;
      UITools.HiglightGridRowFilter(ref r, txtFilter.Text);

		}

		private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
		{
			string btn = be.Button.Key.ToLower();
			if (btn == "export")
			{
				Utils.ExportToExcel(dg, "ContainerGroups", "ContainerGroups");
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