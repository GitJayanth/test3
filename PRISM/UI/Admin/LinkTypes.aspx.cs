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
using Infragistics.WebUI.WebDataInput;
using Infragistics.WebUI.UltraWebGrid;
#endregion

namespace HyperCatalog.UI.Admin
{
	/// <summary>
	/// Display list of link type
	///		--> Export in Excel
	///		--> Filter on item type name
	///		--> Add a new link type
	///		--> Modify link type
	///	Add a new link type or modify link type
	///		--> Save link type
	///		--> Delete link type
	///		--> Return to the list of link types
	/// </summary>
	public partial class LinkTypes : HCPage
	{
		#region Declarations
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
		/// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
		/// le contenu de cette méthode avec l'éditeur de code.
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
      if (HyperCatalog.Shared.SessionState.User.IsReadOnly)
      {
        uwToolbar.Items.FromKeyButton("Add").Enabled = false;
        uwToolbar.Items.FromKeyButton("Apply").Enabled = false;
      }

      if (!HyperCatalog.Shared.SessionState.User.HasCapability(CapabilitiesEnum.EXTEND_CONTENT_MODEL))
      {
        UITools.HideToolBarSeparator(uwToolbar, "ApplySep");
        UITools.HideToolBarButton(uwToolbar, "Apply");
        UITools.HideToolBarSeparator(uwToolbar, "AddSep");
        UITools.HideToolBarButton(uwToolbar, "Add");
      }
      #endregion

      if (!Page.IsPostBack)
      {
        UpdateDataView();
      }
      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "InitVar", "<script>var webtab='" + webTab.ClientID + "'</script>");
    }

		private void UpdateDataView()
		{
      if (Request["filter"] != null)
      {
        txtFilter.Text = Request["filter"].ToString();
      }
			lbErrorGrid.Visible = false;
			string sSql = string.Empty;
      webTab.Visible = false;
			// Filter
			string filter = txtFilter.Text;
			if (filter!=string.Empty)
			{
				string cleanFilter = filter.Replace("'", "''").ToLower();
				cleanFilter = cleanFilter.Replace("[", "[[]");
				cleanFilter = cleanFilter.Replace("_", "[_]");
				cleanFilter = cleanFilter.Replace("%", "[%]");

				sSql += " LOWER(LinkTypeName) like '%" + cleanFilter +"%' ";
				sSql += " OR LOWER(Description) like '%" + cleanFilter +"%' ";
			}

      using (LinkTypeList linkTypes = LinkType.GetAll(sSql, "Sort"))
      {
        if (linkTypes != null)
        {
          if (linkTypes.Count > 0)
          {
            dg.DataSource = linkTypes;
            Utils.InitGridSort(ref dg, false);
            dg.DataBind();

            if (filter == string.Empty)
              Utils.EnableIntelligentSort(ref dg, Convert.ToInt32(txtSortColPos.Value));

            dg.DisplayLayout.AllowSortingDefault = AllowSorting.No;
            dg.Columns.Remove(dg.Columns.FromKey("__Spacer"));

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
          lbTitle.Text = "Link types list";
        }
      }
		}

		void UpdateDataEdit(string selLinkTypeId)
		{
			panelGrid.Visible = false;

			if ((selLinkTypeId != null) && (selLinkTypeId.Length > 0))
			{
				webTab.EnableViewState = false;
				webTab.Tabs.FromKeyTab("Properties").ContentPane.TargetUrl = "./linktypes/linktype_properties.aspx?t="+selLinkTypeId;

				LinkType linkType = LinkType.GetByKey(Convert.ToInt32(selLinkTypeId));
				if (linkType == null)  // create new link type
				{
					lbTitle.Text = "LinkType: New";
					webTab.Tabs.FromKeyTab("Items").Visible = false; // hide Items tab
					webTab.Tabs.FromKeyTab("ItemTypes").Visible = false; // hide ItemTypes tab
				}
				else // update link type
 				{
					lbTitle.Text = "LinkType: "+linkType.Name;
					webTab.Tabs.FromKeyTab("Items").ContentPane.TargetUrl = "./linktypes/linktype_items.aspx?t="+selLinkTypeId;
					webTab.Tabs.FromKeyTab("Items").Visible = true;
					webTab.Tabs.FromKeyTab("Items").Text = "Items ("+linkType.GetItemCount(false).ToString()+")"; // LinkFrom is false (count of companions)
					
					lbTitle.Text = "LinkType: "+linkType.Name;
					webTab.Tabs.FromKeyTab("ItemTypes").ContentPane.TargetUrl = "./linktypes/linktype_itemtypes.aspx?t="+selLinkTypeId;
					webTab.Tabs.FromKeyTab("ItemTypes").Visible = true;
					webTab.Tabs.FromKeyTab("ItemTypes").Text = "Item types ("+linkType.GetItemTypeCount().ToString()+")";
				}
				webTab.Visible = true;      
			}
		}

		private void ApplyNewSort()
		{
			lbErrorGrid.Visible=false;

			if (dg != null)
			{
				bool isSaved = false;				

				foreach (UltraGridRow r in dg.Rows)
				{
					int linkTypeId = Convert.ToInt32(r.Cells.FromKey("Id").Value);
					HyperCatalog.Business.LinkType lt = HyperCatalog.Business.LinkType.GetByKey(linkTypeId);
					lt.Sort = r.Index;

					if (!lt.Save())
					{
						lbErrorGrid.CssClass = "hc_error";
						lbErrorGrid.Text = HyperCatalog.Business.LinkType.LastError;
						lbErrorGrid.Visible=true;
						
						isSaved = false;
						break;
					}
					else
						isSaved = true;
				}

				if (isSaved)
				{
					lbErrorGrid.CssClass = "hc_success";
					lbErrorGrid.Text = "New sort saved!";
					lbErrorGrid.Visible=true;
				}
			}
		}

		private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
		{
			// Display filter
			if (txtFilter.Text != string.Empty)
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

		private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
		{
			string btn = be.Button.Key.ToLower();
			if (btn == "export")
			{
                
                Utils.ExportToExcelFromGrid(dg, "Link Types", "Link Types", Page, null, "Link Types");
			}
			else if (btn == "add")
			{
				UpdateDataEdit("-1");
			}
			else if (btn == "apply")
			{
				ApplyNewSort();
			}
		}
	}
}
