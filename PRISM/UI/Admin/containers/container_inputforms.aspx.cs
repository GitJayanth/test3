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
/// Display list of input forms containing the selected container
///		--> Return to the list of containers
///		--> export in Excel
///		--> Filter on Name and Description fields
/// </summary>
public partial class container_inputforms : HCPage
{
	#region Declarations

	private string containerId=string.Empty;
	private bool isPopup = false;
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
    try
    {
      if (Request["c"] != null)
        containerId = Request["c"].ToString();
      if (Request["b"] != null)
        isPopup = Request["b"].ToString().Equals("1");

      if (Request["filter"] != null)
      {
        txtFilter.Text = Request["filter"].ToString();
      }
      if (!Page.IsPostBack)
      {
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
    string filter = txtFilter.Text;
    if (isPopup)
    {
      pnlTitle.Visible = true;
      UITools.HideToolBarSeparator(uwToolbar, "ListSep");
      UITools.HideToolBarButton(uwToolbar, "List");

      if (containerId.Length > 0)
      {
        using (HyperCatalog.Business.Container container = HyperCatalog.Business.Container.GetByKey(Convert.ToInt32(containerId)))
        {
          if (container != null)
            uwtoolbarTitle.Items.FromKeyLabel("Action").Text = "[" + container.Tag + "] - " + container.Name;
        }
      }
    }
    else
    {
      pnlTitle.Visible = false;
      UITools.ShowToolBarSeparator(uwToolbar, "ListSep");
      UITools.ShowToolBarButton(uwToolbar, "List");
    }

    string sqlFilter = string.Empty;
    if (filter != string.Empty)
    {
      string cleanFilter = filter.Replace("'", "''").ToLower();
      cleanFilter = cleanFilter.Replace("[", "[[]");
      cleanFilter = cleanFilter.Replace("_", "[_]");
      cleanFilter = cleanFilter.Replace("%", "[%]");

      sqlFilter += " (LOWER(Name) like '%" + cleanFilter + "%'";
      sqlFilter += " OR LOWER(Description) like '%" + cleanFilter + "%')";
    }

    using (Database dbObj = Utils.GetMainDB())
    {
      using (DataSet ds = dbObj.RunSPReturnDataSet("dbo._Container_GetInputForms", "InputForms",
        new SqlParameter("@ContainerId", containerId),
        new SqlParameter("@Filter", sqlFilter)))
      {
        dbObj.CloseConnection();

        if (dbObj.LastError.Length == 0)
        {
          if (ds != null)
          {
            if (ds.Tables["InputForms"] != null && ds.Tables["InputForms"].Rows.Count > 0)
            {
              dg.DataSource = ds.Tables["InputForms"];
              Utils.InitGridSort(ref dg);
              dg.DataBind();

              UITools.RefreshTab(this.Page, "InputForms", dg.Rows.Count);

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
          }
        }
      }
    }
  }
    
	private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
	{
		// update input type
		e.Row.Cells.FromKey("InputType").Text = InputFormContainer.GetTypeFromString(e.Row.Cells.FromKey("InputType").Text).ToString();

		// update filter
		if (txtFilter.Text.Length > 0)
		{
			Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;
			UITools.HiglightGridRowFilter(ref r, txtFilter.Text);
		}
	}

	private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
	{
		string btn = be.Button.Key.ToLower();
		if (btn == "export")
		{
      using (HyperCatalog.Business.Container container = HyperCatalog.Business.Container.GetByKey(Convert.ToInt32(containerId)))
      {
        if (container != null)
        {
          Utils.ExportToExcel(dg, "[" + container.Tag + "] " + container.Name + " - InputForms", "[" + container.Tag + "] " + container.Name + " - InputForms");
        }
      }
		}
	}
}
