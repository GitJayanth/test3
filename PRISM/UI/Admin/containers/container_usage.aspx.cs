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
using HyperComponents.Data.dbAccess;
using System.Data.SqlClient;
using HyperCatalog.Business;
#endregion


/// <summary>
/// Display list of items using the selected container
///		--> Return to the list of containers
///		--> Export in Excel
///		--> Filter on Item field
/// ///     --> Bug 66022: Added the loadCultureList() function to add new functionality to see the usage based on cultures.
/// </summary>
public partial class container_usage : HCPage
{
	#region Declarations
	
	private int containerId=-1;
	#endregion
    
	#region Code généré par le Concepteur Web Form
	override protected void OnInit(EventArgs e)
	{
		InitializeComponent();
		txtFilter.AutoPostBack = false;
        txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);"); Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "<script defer=true>var culSelect ; culSelect = document.getElementById('" + CultureList.ClientID + "') ;var txtFilterField;txtFilterField = document.getElementById('" + txtFilter.ClientID + "');</script>"); 
        base.OnInit(e);
	}
		
	/// <summary>
	/// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
	/// le contenu de cette méthode avec l'éditeur de code.
	/// </summary>
	private void InitializeComponent()
	{
        CultureList = (DropDownList)uwToolbar.Items.FromKeyCustom("CultureFilter").FindControl("CultureList");
        if (CultureList != null)
            CultureList.SelectedIndexChanged += new EventHandler(CultureList_SelectedIndexChanged);
       this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);
		this.dg.InitializeRow += new Infragistics.WebUI.UltraWebGrid.InitializeRowEventHandler(this.dg_InitializeRow);

	}
	#endregion

    private void loadCultureLIst(string selectedCulture)
    {
        CultureList.Items.Clear();
        //add the "no-selection" culture at the top
        HyperCatalog.Business.CultureList CList = HyperCatalog.Shared.SessionState.User.Cultures;
        CultureList.DataSource = CList;
        CultureList.DataBind();
        if (selectedCulture != null)
            CultureList.Items.FindByValue(selectedCulture).Selected = true;
    }


  protected void Page_Load(object sender, System.EventArgs e)
  {
    try
    {
        string selectedCulture = null;
      if (Request["c"] != null)
        containerId = Convert.ToInt32(Request["c"]);

      if (Request["filter"] != null)
      {
        txtFilter.Text = Request["filter"].ToString();
      }
      if (Request["CultureList"] != null)
      {
          selectedCulture = Request["CultureList"].Trim();
      }
           
      if (!Page.IsPostBack)
      {
        loadCultureLIst(selectedCulture);
        UpdateDataView();
      }
    }
    catch
    {
      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "clientScript", "<script>back();</script>");
    }
  }
    
	void UpdateDataView()
	{
    using (HyperCatalog.Business.Container container = HyperCatalog.Business.Container.GetByKey(containerId))
    {
      //HyperCatalog.Business.ChunkList chunks = container.Chunks(HyperCatalog.Shared.SessionState.Culture.Code);
        using (HyperCatalog.Business.ChunkList chunks = container.Chunks(CultureList.SelectedValue.ToString()))
        {

        if (chunks != null)
        {
          if (chunks.Count > 0)
          {
            dg.DataSource = chunks;
            Utils.InitGridSort(ref dg);
            dg.DataBind();

            UITools.RefreshTab(this.Page, "Usage", dg.Rows.Count);

            dg.Visible = true;
            lbNoresults.Visible = false;
          }
          else
          {
            dg.Visible = false;
            lbNoresults.Visible = true;
          }
        }
      }
    }
	}
	private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
	{
		bool keep = true;
        string chunkValue = e.Row.Cells.FromKey("Value").Text;
		if ((e.Row.Cells.FromKey("ItemSku").Value != null) && (e.Row.Cells.FromKey("ItemSku").Text.Length > 0))
			e.Row.Cells.FromKey("ItemName").Text = e.Row.Cells.FromKey("ItemName").Text + " [" + e.Row.Cells.FromKey("ItemSku").Text +"]";

		if (txtFilter.Text.Length > 0)
		{
			string filter = txtFilter.Text.ToLower();
			string itemName = e.Row.Cells.FromKey("ItemName").Text.ToLower();

			if ((itemName.IndexOf(filter) < 0) && (chunkValue.IndexOf(filter) < 0))
			{
				keep = false;
				e.Row.Delete();
			}
			else
			{
				Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;
				UITools.HiglightGridRowFilter(ref r, txtFilter.Text);
			}
		}

		if (keep)
		{		
			e.Row.Cells.FromKey("ChunkStatus").Text = "<img src='/hc_v4/img/S" + e.Row.Cells.FromKey("ChunkStatus").Text.Substring(0,1) + ".gif'>";
      if (chunkValue == HyperCatalog.Business.Chunk.BlankValue)
      {
        e.Row.Cells.FromKey("Value").Text = HyperCatalog.Business.Chunk.BlankText;
        e.Row.Cells.FromKey("Value").Style.CustomRules = string.Empty;
      }
      else
      {
        e.Row.Cells.FromKey("Value").Text = UITools.HtmlEncode(chunkValue).Replace(Environment.NewLine, "<br/>");
      }
      if (IsRegion(CultureList.SelectedValue.ToString()))
          e.Row.Cells.FromKey("ItemName").Text = "<a href='../../../redirect.aspx?p=UI/Acquire/qde.aspx&c=" + CultureList.SelectedValue.ToString() + "&i=" + e.Row.Cells.FromKey("ItemId").Text + "' target='_BLANK'\">" + e.Row.Cells.FromKey("ItemName").Text + "</a>";
      else
          e.Row.Cells.FromKey("ItemName").Text = "<a href='../../../redirect.aspx?p=UI/Globalize/QDETranslate.aspx&c=" + CultureList.SelectedValue.ToString() + "&i=" + e.Row.Cells.FromKey("ItemId").Text + "' target='_BLANK'\">" + e.Row.Cells.FromKey("ItemName").Text + "</a>";

		}
	}
    private bool IsRegion(string value)
    {
        foreach (Culture cul in HyperCatalog.Shared.SessionState.User.Cultures)
        {
            if (cul.Code.Equals(value))
            {
                return cul.Type == CultureType.Master || cul.Type == CultureType.Regionale;
            }
        }
        return false;
    }


	private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
	{
		string btn = be.Button.Key.ToLower();
		if (btn == "export")
		{
            dg.Columns.FromKey("ChunkStatus").Hidden = true;
            dg.Columns.FromKey("ST").Hidden = false;
            Utils.ExportToExcelFromGrid(dg, containerId + "-Containers", containerId + "-Containers", Page, null, containerId + "-Containers");
            dg.Columns.FromKey("ChunkStatus").Hidden = false;
            dg.Columns.FromKey("ST").Hidden = true;
			//1089 Kanthi Utils.ExportToExcel(dg, containerId + "-Containers", containerId + "-Containers");         
		}
    }

    private void CultureList_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateDataView();
	}
}
