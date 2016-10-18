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
using System.Text;
using Infragistics.WebUI.WebDataInput;
using Infragistics.WebUI.UltraWebGrid;
using HyperCatalog.Shared;
#endregion

namespace HyperCatalog.UI.Login
{
	/// <summary>
	/// This class allow to modify Home Page
	/// </summary>
	public partial class HomePageProperties : HCPage
	{
		#region Constants
		// field names
		private const string CST_FIELD_WEBPARTID="WebPartId";
		private const string CST_FIELD_SORT="Sort";
		private const string CST_FIELD_PANEID="PaneId";
		private const string CST_FIELD_PANENAME="PaneName";
		private const string CST_FIELD_CONTROL="Control";
		private const string CST_FIELD_CAPTION="Caption";
		private const string CST_FIELD_ISPRESENT="IsPresent";
		// table name
		private const string CST_TABLE_WEBPARTS="WebParts";
		#endregion
	
		#region Private methods
		/// <summary>
		/// Create DataSet object containing all modules and their properties. And update grid data
		/// </summary>
		private void BindData()
		{
			// Create dataset
			DataSet dsResult=new DataSet();

			// Create table
			DataTable dt=dsResult.Tables.Add(CST_TABLE_WEBPARTS);
			
			// Create columns
			dt.Columns.Add(CST_FIELD_WEBPARTID, Type.GetType("System.Int16"));
			dt.Columns.Add(CST_FIELD_ISPRESENT, Type.GetType("System.Boolean"));
			dt.Columns.Add(CST_FIELD_CAPTION, Type.GetType("System.String"));

			// Create rows
			DataSet dsModuleDefs=Configuration.LoadModuleDefs();
			if ((dsModuleDefs!=null) && (dsModuleDefs.Tables.Count>0))
			{
				for (int i=0; i<dsModuleDefs.Tables[0].Rows.Count; i++)
				{
					DataRow currentDR=dsModuleDefs.Tables[0].Rows[i];

					// Get properties
					int moduleDefId=(Int16)currentDR[CST_FIELD_WEBPARTID];
					DataRow drProperties=Configuration.GetModuleProperties(moduleDefId);

					// Add row in dataset
					object[] parameters={moduleDefId, (drProperties!=null), currentDR[CST_FIELD_CAPTION].ToString()};
					dt.Rows.Add(parameters);
				}
				dsModuleDefs.Dispose();

				// Bind data
				uwgModules.DataSource=dsResult;
				uwgModules.DataBind();
			}
			dsResult.Dispose();
		}


		/// <summary>
		/// Save properties in database
		/// </summary>
		/// <param name="dsProp">Dataset object containing all module properties for current user</param>
		private void SaveModules(DataSet dsResult)
		{
			if ((dsResult!=null) && (dsResult.Tables.Count>0))
			{
				// Load data for current user
				DataSet dsOld=Configuration.LoadUserModules();

				if ((dsOld!=null) && (dsOld.Tables.Count>0))
				{
					// Copy dataset object without data
					DataSet dsNew=dsOld.Clone();

					for (int i=0; i<dsResult.Tables[0].Rows.Count; i++)
					{
						DataRow currentDR=dsResult.Tables[0].Rows[i]; // current row
						DataRow drResult=Configuration.GetModulesDefinition((Int16)currentDR[CST_FIELD_WEBPARTID]); // get module definition
						
						object[] parameters={	(Int16)SessionState.User.Id, 
												(Int16)currentDR[CST_FIELD_WEBPARTID], 
												(Int16)currentDR[CST_FIELD_SORT], 
												(Int16)currentDR[CST_FIELD_PANEID], 
												drResult[CST_FIELD_CONTROL].ToString()};
						dsNew.Tables[0].Rows.Add(parameters);
					}
					// Save in session and database
					Configuration.SaveUserModules(dsNew);

					// free memory
					if (dsNew!=null)
						dsNew.Dispose();
				}
				// free memory
				if (dsOld!=null)
					dsOld.Dispose();
			}
			// free memory
			if (dsResult!=null)
				dsResult.Dispose();
		}
		#endregion

		#region Event methods
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// check connection
			UITools.CheckConnection(Page);
			if (!Page.IsPostBack)
				BindData();
		}

		protected void uwgModules_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
		{
			// Build dataset containing all pane names
			DataSet ds=new DataSet();
			DataTable dt=ds.Tables.Add("PaneList");
			dt.Columns.Add("Value", Type.GetType("System.Int32"));
			dt.Columns.Add("Text", Type.GetType("System.String"));
			dt.Rows.Add(new object[] {0, "LeftPane"});
			dt.Rows.Add(new object[] {1, "ContentPane"});
			dt.Rows.Add(new object[] {2, "RightPane"});

			// Get properties for this row
			int moduleDefId=(Int16)e.Row.Cells.FromKey(CST_FIELD_WEBPARTID).Value;
			DataRow drProperties=Configuration.GetModuleProperties(moduleDefId);

			// Find pane list in current row
			TemplatedColumn currentTempColPane=(TemplatedColumn)e.Row.Cells.FromKey(CST_FIELD_PANENAME).Column;
			CellItem currentCellItemPane=(CellItem)currentTempColPane.CellItems[e.Row.Index];
			DropDownList currentDDL=(DropDownList)currentCellItemPane.FindControl("ddlPaneList");

			// Find module order in current row
			TemplatedColumn currentTempColOrder=(TemplatedColumn)e.Row.Cells.FromKey(CST_FIELD_SORT).Column;
			CellItem currentCellItemOrder=(CellItem)currentTempColOrder.CellItems[e.Row.Index];
			WebNumericEdit currentWNE=(WebNumericEdit)currentCellItemOrder.FindControl("wneModuleOrder");
			
			// Update data in pane list
			currentDDL.DataSource=ds;
			currentDDL.DataTextField="Text";
			currentDDL.DataValueField="Value";
			currentDDL.DataBind();
			int selectedPane=0;
			if (drProperties!=null)
				selectedPane=(Int16)drProperties[CST_FIELD_PANEID]; 
			currentDDL.SelectedIndex=selectedPane; // select pane name

			// Update data in edit for module order
			currentWNE.Value = 0;
			if (drProperties!=null)
				currentWNE.Value = drProperties[CST_FIELD_SORT].ToString() !=string.Empty? Convert.ToInt32(drProperties[CST_FIELD_SORT].ToString()): 0;

			// free memory
			ds.Dispose();
		}

		protected void uwtProperties_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
		{
			if (be.Button.Key.Equals("btnSave"))
			{
				DataSet dsResult=new DataSet();
				DataTable dt=dsResult.Tables.Add(CST_TABLE_WEBPARTS);
				dt.Columns.Add(CST_FIELD_WEBPARTID, Type.GetType("System.Int16"));
				dt.Columns.Add(CST_FIELD_PANEID, Type.GetType("System.Int16"));
				dt.Columns.Add(CST_FIELD_SORT, Type.GetType("System.Int16"));

				for (int i=0; i<uwgModules.Rows.Count; i++)
				{
					UltraGridRow currentRow=uwgModules.Rows[i];
					bool isPresent=(bool)currentRow.Cells.FromKey(CST_FIELD_ISPRESENT).Value;
					if (isPresent)
					{
						// get module identifier
						int moduleDefId=Convert.ToInt32(currentRow.Cells.FromKey(CST_FIELD_WEBPARTID).Value.ToString());
						// get pane name
						TemplatedColumn tempColPane=(TemplatedColumn)currentRow.Cells.FromKey(CST_FIELD_PANENAME).Column;
						DropDownList ddlPaneList=(DropDownList)((CellItem)tempColPane.CellItems[currentRow.Index]).FindControl("ddlPaneList");
						int paneId=Convert.ToInt32(ddlPaneList.SelectedValue);
						// get module order
						TemplatedColumn tempColOrder=(TemplatedColumn)currentRow.Cells.FromKey(CST_FIELD_SORT).Column;
						WebNumericEdit wneModuleOrder=(WebNumericEdit)((CellItem)tempColOrder.CellItems[currentRow.Index]).FindControl("wneModuleOrder");
						int moduleOrder=wneModuleOrder.ValueInt;

						// add new module
						dt.Rows.Add(new object[] {moduleDefId, paneId, moduleOrder});
					}
				}

				// save properties
				SaveModules(dsResult);

				// refresh document
				Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"close", "<script>opener.document.location.reload();window.close();</script>");
			}
			else if (be.Button.Key.Equals("btnCancel"))
			{
				Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"close", "<script>window.close();</script>");
			}
		}
		#endregion
	}
}
