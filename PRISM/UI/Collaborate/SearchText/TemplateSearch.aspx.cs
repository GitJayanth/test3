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
/******AS PART OF PRSIM UI TO PDB CHANGES BY REKHA THOMAS*************/
using HyperCatalog.Business;
using HyperCatalog.Shared;
/******END AS PART OF PRSIM UI TO PDB CHANGES BY REKHA THOMAS*************/
namespace HyperCatalog.UI
{
	/// <summary>
	/// Description résumée de SearchResult.
	/// </summary>
	public partial class SearchResult : HCPage
	{
    #region UI
    #region Grid
    #endregion
    #endregion
    /*****************AS PART OF PRISM UI TO PDB CHANGES BY REKHA THOMAS*************************/

        private Business.SearchQuery Query
        {
            get
            {
                return (Business.SearchQuery)ViewState["Query"];
            }
            set
            {
                ViewState["Query"] = value;
                if (value != null)
                {
                    //SQLItem = value.SQLItem;
                    //SQLContainer = value.SQLContainer;
                    //SQLCulture = value.SQLCulture;
                    //SQLContent = value.SQLContent;
                }
            }
        }
    /*****************END AS PART OF PRISM UI TO PDB CHANGES BY REKHA THOMAS*************************/
    protected override void OnLoad(EventArgs e)
    {
      UITools.CheckConnection(Page);

      mainToolBar.Items.FromKeyButton("Options").Selected = false;
      if (!IsPostBack)
        BindGrid();

      base.OnLoad (e);
      
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
		}
    #endregion

    #region Data load & bind
    public void BindGrid()
    {
      templatesGrid.DataSource = Shared.SessionState.User.SearchQueries;
      templatesGrid.DataBind();
    }

    #endregion

    #region Events
    private void mainToolBar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
    }

    //protected void GetPreparedExport(object sender, EventArgs e)
    //{
    //  DataSet ds = new DataSet();
    //  ds.Tables.Add("test");
    //  System.Text.StringBuilder sb = Utils.ExportDataTableToCSV(ds.Tables[0]);

    //  string stream = sb.ToString();

    //  Response.Clear();
    //  Response.ContentType = "text/plain";
    //  Response.AddHeader("Content-disposition", "attachment; filename="+((Infragistics.WebUI.UltraWebGrid.CellItem)((Button)sender).NamingContainer).Cell.Row.DataKey+".csv");
    //  Response.Write(stream);
    //  Response.End();
    //}

    #region Grid events
    protected void UpdateGridItem(object sender, System.EventArgs e)
    {
      Infragistics.WebUI.UltraWebGrid.CellItem cellItem = ((Infragistics.WebUI.UltraWebGrid.CellItem)((System.Web.UI.WebControls.LinkButton)sender).Parent);
      /************Start 3.5 Release for exporting Template Search*******************/
      DataTable objDTBusinessFilters = new DataTable();
      objDTBusinessFilters.Columns.Add(new DataColumn("FilterName"));
      objDTBusinessFilters.Columns.Add(new DataColumn("FilterValue"));

      DataRow objDR = null;
      objDR =objDTBusinessFilters.NewRow();
      objDR[0] = "Name";
      objDR[1] = cellItem.Cell.Row.Cells.FromKey("Name").Text + "";
      objDTBusinessFilters.Rows.Add(objDR);

      objDR = objDTBusinessFilters.NewRow();
      objDR[0] = "Visibility";
      objDR[1] = ((bool)cellItem.Cell.Row.Cells.FromKey("Visibility").Value) ? "Public" : "Private";
      objDTBusinessFilters.Rows.Add(objDR);

      objDR = objDTBusinessFilters.NewRow();
      objDR[0] = "Description";
      objDR[1] = cellItem.Cell.Row.Cells.FromKey("Description").Text + "";
      objDTBusinessFilters.Rows.Add(objDR);
      objDTBusinessFilters.AcceptChanges();
        /*
      lstBusinessFilters.Add(new ListItem("Name",cellItem.Cell.Row.Cells.FromKey("Name").Text+""));
      lstBusinessFilters.Add(new ListItem("Visibility", ((bool)cellItem.Cell.Row.Cells.FromKey("Visibility").Value) ? "Public" : "Private"));
      lstBusinessFilters.Add(new ListItem("Description",cellItem.Cell.Row.Cells.FromKey("Description").Text+""));*/
      Session["selectedBusinessFilters"] = objDTBusinessFilters;
      Session["selectedCustomFilters"]   = null;
      /************End 3.5 Release for exporting Template Search*******************/
      UpdateDataEdit((string)cellItem.Cell.Row.Cells.FromKey("Name").Value);
    }
    private void UpdateDataEdit(string queryName)
    {
      string columns = "";
      foreach (ListItem item in fieldFilter.Items)
        if (item.Selected)
          columns += "," + item.Value;
      if (columns.Length > 0)
        columns = columns.Substring(1, columns.Length - 1);

      Business.SearchQuery query = Shared.SessionState.User.SearchQueries[queryName];
      Session["SearchQuery"] = query;
      Session["SQLItem"] = query.SQLItem;
      Session["SQLContainer"] = query.SQLContainer;
      Session["SQLCulture"] = query.SQLCulture;
      Session["SQLContent"] = query.SQLContent;
      Session["Mandatory"] = query.Mandatory;
      // The following Code Determines what needs to be 
      // the value of cbd
      string cbdValue = string.Empty;
      if (HyperCatalog.Shared.SessionState.CacheComponents[query.DBComponentId].Name == "Crystal_DB")
          cbdValue = "1";
      else
          cbdValue = "0";
      /************Start 3.5 Release for exporting Template Search*******************/
      Session["SearchType"] = "Template Search";
      //Page.ClientScript.RegisterStartupScript(this.GetType(), "popup", "<script>var w = window.open('./SearchResult.aspx?cdb=" + cbdValue + "&cols=" + columns + "', 'dosearch', 'resizable=yes, scrollbar=yes',500, 800, 'yes', 1);</script>");
      Page.ClientScript.RegisterStartupScript(this.GetType(), "popup", "<script>var w = window.open('./SearchResult.aspx?cdb=" + cbdValue + "&cols=" + columns + "','_blank', 'menubar=no, directories=no, location=no, width=800, height=400, resizable=yes, scrollbars=1');w.focus()</script>");
    }

    /*********CHANGES AS PART OF PRISM UI TO PDB CHANGES BY REKHA THOMAS*********/
    protected void DeleteButton_Click(object sender, EventArgs e) // Delete Advanced search reports directly from the saved reports list
    {
        Infragistics.WebUI.UltraWebGrid.CellItem cellItem = ((Infragistics.WebUI.UltraWebGrid.CellItem)((System.Web.UI.WebControls.Button)sender).Parent);
        string queryName = cellItem.Cell.Row.Cells.FromKey("Name").Text + "";
        if (!string.IsNullOrEmpty(queryName))
        {
            Query  = (Business.SearchQuery)Shared.SessionState.User.SearchQueries[queryName];
            if (Query != null)
            {
              Query.Delete();
              BindGrid();
            }
        }
    }
    /*********END CHANGES AS PART OF PRISM UI TO PDB CHANGES BY REKHA THOMAS*********/
    #endregion
    #endregion
  }
}
