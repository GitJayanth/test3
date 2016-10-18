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
#endregion

namespace HyperCatalog.UI.Admin
{
  /// <summary>
  /// Display list of item type
  ///		--> Export in Excel
  ///		--> Filter on item type name
  ///		--> Add a new item type
  ///		--> Modify item type (name or resource)
  ///	Add a new item type or modify item type
  ///		--> Save item type
  ///		--> Delete item type
  ///		--> Return to the list of item types
  /// </summary>
  public partial class ItemTypes : HCPage
  {
		#region Declarations
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
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
			this.uwToolBarEdit.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);

		}
		#endregion

    protected void Page_Load(object sender, System.EventArgs e)
		{ 
			// <Temp>
			UITools.HideToolBarSeparator(uwToolbar, "AddSep");
			UITools.HideToolBarButton(uwToolbar, "Add");
			UITools.HideToolBarSeparator(uwToolBarEdit, "DeleteSep");
			UITools.HideToolBarButton(uwToolBarEdit, "Delete");
			UITools.HideToolBarSeparator(uwToolBarEdit, "SaveSep");
			UITools.HideToolBarButton(uwToolBarEdit, "Save");
			// </Temp>

				if (HyperCatalog.Shared.SessionState.User.IsReadOnly)
				{
					uwToolBarEdit.Items.FromKeyButton("Save").Enabled = false;
					uwToolBarEdit.Items.FromKeyButton("Delete").Enabled = false;
				}

        if (!HyperCatalog.Shared.SessionState.User.HasCapability(CapabilitiesEnum.EXTEND_CONTENT_MODEL))
				{
					UITools.HideToolBarSeparator(uwToolbar, "AddSep");
					UITools.HideToolBarButton(uwToolbar, "Add");
					UITools.HideToolBarSeparator(uwToolBarEdit, "DeleteSep");
					UITools.HideToolBarButton(uwToolBarEdit, "Delete");
					UITools.HideToolBarSeparator(uwToolBarEdit, "SaveSep");
					UITools.HideToolBarButton(uwToolBarEdit, "Save");
				}

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

				sSql = " LOWER(ItemTypeName) like '%" + cleanFilter +"%' ";
			}
			using (ItemTypeList itemTypes = ItemType.GetAll(sSql))
      {
        if (itemTypes != null)
        {
          if (itemTypes.Count > 0)
          {
            dg.DataSource = itemTypes;
            Utils.InitGridSort(ref dg);
            dg.DataBind();

            dg.Visible = true;
            lbNoresults.Visible = false;
          }
          else
          {
            if (txtFilter.Text.Length > 0)
              lbNoresults.Text = "No record match your search (" + txtFilter.Text + ")";
            lbNoresults.Visible = true;
            dg.Visible = false;
          }
        }
			}

			panelGrid.Visible= true;
			lbTitle.Text = "Item types list";
		}

		private void UpdateDataEdit(string selItemTypeId)
		{
			ItemType itemType = null;
			if (selItemTypeId.Length>0)
				itemType = ItemType.GetByKey(Convert.ToInt32(selItemTypeId));

			if (itemType == null)
			{ 
				lbTitle.Text = "Item type: New";	
				wneTypeId.Value = "-1";

				lbItemTypeId.Visible = false;
				wneTypeId.Visible = false;

				UITools.HideToolBarButton(uwToolBarEdit, "Delete");
				UITools.HideToolBarSeparator(uwToolBarEdit, "DeleteSep");
			}
			else
			{
				lbTitle.Text = "Item type: " + itemType.Name;
				wneTypeId.Value = itemType.Id;
				txtTypeName.Text = itemType.Name;
				txtIcon.Text = itemType.Icon;
					
				lbItemTypeId.Visible = true;
				wneTypeId.Visible = true;
				wneTypeId.Enabled = false;
			}
			panelEdit.Visible = true;
			panelGrid.Visible = false;
		}

		private void Save()
		{
			lbError.Text = string.Empty;
			int iTypeId = wneTypeId.ValueInt;
			ItemType itemType = ItemType.GetByKey(iTypeId);

			if (iTypeId == -1)
			{
				// create
				if (itemType == null)
					itemType = new ItemType(iTypeId, txtTypeName.Text, txtIcon.Text);
				else
				{
					lbError.CssClass = "hc_error";
					lbError.Text = "Error: add/update failed - the properties can't be saved";
					lbError.Visible = true;
					return;
				}
			}
			else
			{
				// update
				if (itemType != null)
				{
					itemType.Name = txtTypeName.Text;
					itemType.Icon = txtIcon.Text;
				}
				else
				{
					lbError.CssClass = "hc_error";
					lbError.Text = "Error: add/update failed - the properties can't be saved";
					lbError.Visible = true;
					return;
				}
			}

			if (itemType.Save())
			{
				if (!wneTypeId.Visible)
				{
					// updated
					wneTypeId.Value = itemType.Id;
					wneTypeId.Text = itemType.Id.ToString();

					wneTypeId.Visible = true;
					lbItemTypeId.Visible = true;
					wneTypeId.Enabled = false;

          if (HyperCatalog.Shared.SessionState.User.HasCapability(CapabilitiesEnum.EXTEND_CONTENT_MODEL))
					{
						UITools.ShowToolBarButton(uwToolBarEdit, "Delete");
						UITools.ShowToolBarSeparator(uwToolBarEdit, "DeleteSep");
					}
				}
				// created
				lbError.Text = "Data saved!";
				lbError.CssClass = "hc_success";
				lbError.Visible = true;
			}
			else
			{
				lbError.CssClass = "hc_error";
				lbError.Text = "Error: add/update failed - the properties can't be saved";
				lbError.Visible = true;
			}
		}

		private void Delete()
		{
			ItemType itemType = ItemType.GetByKey(wneTypeId.ValueInt);

			if (itemType != null)
			{
				if (itemType.Delete(HyperCatalog.Shared.SessionState.User.Id))
				{
					lbError.Visible = false;
					lbError.Text = string.Empty;
					
					panelEdit.Visible = false;
					panelGrid.Visible = true;
					
					UpdateDataView();
				}
				else
				{
					lbError.CssClass = "hc_error";
					lbError.Text = "Error: this item type is still used";
					lbError.Visible = true;
				}   
			}
			else
			{
				lbError.CssClass = "hc_error";
				lbError.Text = "Error: a system error occurred";
				lbError.Visible = true;
			}
		}

		private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
		{
      Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;
      UITools.HiglightGridRowFilter(ref r, txtFilter.Text);
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
				Utils.ExportToExcel(dg, "ItemTypes", "ItemTypes");
			}
			else if (btn == "add")
			{
				UpdateDataEdit(string.Empty);
			}
			else if (btn == "save")
			{
				Save();
			}
			else if (btn == "delete")
			{
				Delete();
			}
		}
  }
}