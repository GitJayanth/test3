using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HyperComponents.Data.dbAccess;
using HyperCatalog.Shared;

	/// <summary>
	/// Description résumée de qde_search.
	/// </summary>
	public partial class qde_search : HCPage
	{
    protected string searchField;
  
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
      this.dg.PageIndexChanged += new Infragistics.WebUI.UltraWebGrid.PageIndexChangedEventHandler(this.webGrid_PageIndexChanged);
      this.dg.SelectedRowsChange += new Infragistics.WebUI.UltraWebGrid.SelectedRowsChangeEventHandler(this.webGrid_SelectedRowsChange);

    }
    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
		{      
      
      if (Request["f"]!=null)
      {
        searchField = Server.HtmlDecode(Request["f"]);
      }
      if (!Page.IsPostBack)
      {
        if (Request["f"]!=null)
        {
          UpdateDataView();
        }
        else
        {
          panelResult.Visible = false;
          panelSearch.Visible = true;
        }
      }
      sField.Attributes.Add("onKeyDown", "if ((event.which && event.which == 13) || (event.keyCode && event.keyCode == 13)) {DoSearch();}");
		}

    private void UpdateDataView()
    {
      panelResult.Visible = true;
      panelSearch.Visible = false;      
      lbSearchDetail.Text = "Search results for " + searchField;
      using (Database dbObj = Utils.GetMainDB())
      {
        using (DataSet ds = dbObj.RunSPReturnDataSet("Search_Item", "Items",
          new SqlParameter("@UserId", SessionState.User.Id.ToString()),
          new SqlParameter("@SearchField", searchField)))
        {
          if (dbObj.LastError == string.Empty)
          {
            dg.DataSource = ds.Tables["Items"].DefaultView;
            Utils.InitGridSort(ref dg);
            dg.DataBind();
            lbSearchResults.Text = ds.Tables["Items"].Rows.Count.ToString() + " result(s) found";
            ds.Dispose();
          }
          else
          {
            dg.Visible = false;
            lbSearchResults.Text = "An error occurred while retrieving data: " + dbObj.LastError;
          }
        }
      }

    }

    private void webGrid_PageIndexChanged(object sender, Infragistics.WebUI.UltraWebGrid.PageEventArgs e)
    {
      dg.DisplayLayout.Pager.CurrentPageIndex = e.NewPageIndex;
      UpdateDataView();
    }

    private void webGrid_SelectedRowsChange(object sender, Infragistics.WebUI.UltraWebGrid.SelectedRowsEventArgs e)
    {
      System.Int64 itemId = Convert.ToInt64(e.SelectedRows[0].Cells[0].ToString());
      SessionState.TVAllItems = !SessionState.User.HasItemInScope(itemId);
      if (SessionState.TVAllItems)
      {
        SessionState.User.LastVisitedItemReadOnly = itemId;
      }
      else{
        SessionState.User.LastVisitedItem = itemId;
      }
      SessionState.User.QuickSave();
      if (SessionState.Culture.Code == HyperCatalog.Shared.SessionState.MasterCulture.Code)
      {
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"load", "<script>top.location='/UI/Acquire/QDE.aspx';top.location.reload();</script>");    
      }
      else
      {
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"load", "<script>top.location='/UI/Globalize/QDETranslate.aspx';top.location.reload();</script>");    
      }
    }

    /*private DataSet FindProducts(string s)
    {
      return dbObj.RunSPReturnDataSet("Search_Item", "Items", 
            new SqlParameter("@UserId", SessionState.User.Id.ToString()),
            new SqlParameter("@SearchField", s));
    }*/

	}
