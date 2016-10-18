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
using HyperCatalog.Business;
#endregion

  /// <summary>
  /// Display list of container using the selected data type
  ///		--> Return to the list of data type
  ///		--> Export in excel
  ///		--> Filter on all fields
  /// </summary>
public partial class datatype_containers : HCPage
{
	#region Declarations

	private string dataTypeCode = string.Empty;
	#endregion

	#region Code généré par le Concepteur Web Form
	override protected void OnInit(EventArgs e)
	{
		InitializeComponent();
		this.txtFilter.AutoPostBack = false;
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
    if (Request["d"] != null)
      dataTypeCode = Request["d"].ToString();
      if (Request["filter"] != null)
      {
        txtFilter.Text = Request["filter"].ToString();
      }

    if ((!Page.IsPostBack) && (dataTypeCode.Length > 0))
    {
      UpdateDataView();
    }
  }

  private void UpdateDataView()
  {
    string sSql = "DataTypeCode = '" + dataTypeCode + "'";
    string filter = txtFilter.Text;
    if (filter != string.Empty)
    {
      string cleanFilter = filter.Replace("'", "''").ToLower();
      cleanFilter = cleanFilter.Replace("[", "[[]");
      cleanFilter = cleanFilter.Replace("_", "[_]");
      cleanFilter = cleanFilter.Replace("%", "[%]");

      sSql += " AND (LOWER(Tag) like '%" + cleanFilter + "%'";
      sSql += " OR LOWER(Name) like '%" + cleanFilter + "%'";
      sSql += " OR LOWER(Definition) like '%" + cleanFilter + "%'";
      sSql += " OR LOWER(DataType) like '%" + cleanFilter + "%'";
      sSql += " OR LOWER(ContainerType) like '%" + cleanFilter + "%')";
    }
    using (HyperCatalog.Business.ContainerList HCContainers = HyperCatalog.Business.Container.GetAll(sSql, "Tag"))
    {
      if (HCContainers != null)
      {
        if (HCContainers.Count > 0)
        {
          dg.DataSource = HCContainers;
          Utils.InitGridSort(ref dg);
          dg.DataBind();

          lbNoresults.Visible = false;
          dg.Visible = true;
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

	private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
	{
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
			Utils.ExportToExcel(dg, dataTypeCode + "-Containers", dataTypeCode + "-Containers");
		}
	}
}
