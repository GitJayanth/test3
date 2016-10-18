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

namespace HyperCatalog.UI.Admin
{
	/// <summary>
	/// Display list of data type
	///		--> Export in Excel
	///		--> Filter on all fields of the grid
	///		--> Modify data type
	/// </summary>
	public partial class DataTypes : HCPage
	{
		#region Declarations
    protected System.Web.UI.WebControls.TextBox txtDataTypeCode;
    protected System.Web.UI.WebControls.TextBox txtDataType;
    protected System.Web.UI.WebControls.TextBox txtComment;
    protected System.Web.UI.WebControls.TextBox txtExample;
    protected System.Web.UI.WebControls.TextBox txtDescription;
    protected System.Web.UI.WebControls.Label label1;
    protected System.Web.UI.WebControls.Label Label2;
    protected System.Web.UI.WebControls.Label Label3;
    protected System.Web.UI.WebControls.Label Label4;
    protected System.Web.UI.WebControls.Label Label5;
    protected System.Web.UI.WebControls.TextBox txtRegularExpression;
    protected System.Web.UI.WebControls.TextBox txtContainerTypeCode;
    protected System.Web.UI.WebControls.TextBox txtContainerType;
    protected System.Web.UI.WebControls.Label Label6;

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

		}
    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
    {
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
      string sSql = "SELECT * FROM DataTypes WHERE IsActive = 1 ";

			string filter = txtFilter.Text;
      if (filter!=string.Empty)
      {
				string cleanFilter = filter.Replace("'", "''").ToLower();
				cleanFilter = cleanFilter.Replace("[", "[[]");
				cleanFilter = cleanFilter.Replace("_", "[_]");
				cleanFilter = cleanFilter.Replace("%", "[%]");

        sSql += " AND (LOWER(DataType) Like '%" + cleanFilter +"%'";
        sSql += "      OR LOWER(Description) Like '%" + cleanFilter +"%'";
        sSql += "      OR LOWER(Example) Like '%" + cleanFilter +"%'";
        sSql += "      OR LOWER(Comment) Like '%" + cleanFilter +"%'";
        sSql += "      OR LOWER(RegExpression) Like '%" + cleanFilter +"%'";
        sSql += "      OR LOWER(InputType) Like '%" + cleanFilter +"%'";
        sSql += " )";
      }
      using (Database dbObj = Utils.GetMainDB())
      {
        using (DataSet ds = dbObj.RunSQLReturnDataSet(sSql, "DataTypes"))
        {
          dbObj.CloseConnection();

          if (dbObj.LastError.Length == 0)
          {
            if (ds != null)
            {
              if (ds.Tables["DataTypes"] != null && ds.Tables["DataTypes"].Rows.Count > 0)
              {
                dg.DataSource = ds.Tables["DataTypes"];
                Utils.InitGridSort(ref dg);
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

              ds.Dispose();

              webTab.Visible = false;
              panelGrid.Visible = true;
              lbTitle.Text = UITools.GetTranslation("Data types list");
            }
          }
        }
      }
    }

    protected void UpdateDataEdit(string selDataType)
    {
			if (selDataType==string.Empty)
			{ 
				webTab.Tabs.GetTab(0).ContentPane.TargetUrl = "./datatypes/datatype_properties.aspx?d=";
				lbTitle.Text = "Report: New";
				webTab.Tabs.GetTab(1).Visible = false;
						
				panelGrid.Visible = false;
				webTab.Visible = true;
			}
			else
			{
				DataType dataType = DataType.GetByKey(Convert.ToChar(selDataType));
				if (dataType != null)
				{
					webTab.Tabs.GetTab(0).ContentPane.TargetUrl = "./datatypes/datatype_properties.aspx?d=" + selDataType;
					webTab.Tabs.GetTab(1).ContentPane.TargetUrl = "./datatypes/datatype_containers.aspx?d=" + selDataType;
				
					string sqlFilter = " DataTypeCode = '"+selDataType+"'";
          using (ContainerList c = HyperCatalog.Business.Container.GetAll(sqlFilter))
          {
            webTab.Tabs.GetTab(1).Text = "Containers (" + c.Count + ")";
            webTab.Tabs.GetTab(1).Visible = true;
            lbTitle.Text = "DataType: " + dataType.Name;
          }
					panelGrid.Visible = false;
					webTab.Visible = true;
				}
			}
    }

    private void BtnAdd_Click(object sender, System.EventArgs e)
    {
      UpdateDataEdit(string.Empty);
    }

    private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;
      UITools.HiglightGridRowFilter(ref r, txtFilter.Text);
    }

    private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      if (btn == "export")
      {
        Utils.ExportToExcel(dg, "DataTypes", "DataTypes");
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
