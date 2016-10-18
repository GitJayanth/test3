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
using Infragistics.WebUI.UltraWebGrid;
using HyperCatalog.Shared;
using System.Data.SqlClient;
#endregion

namespace hc_termbase.UI
{
	/// <summary>
	/// Display Termbase settings : TermType Language
	/// --> Button "Add" to create a new TermType Language relation by select the termtype and the Language into combo 
  /// --> Button "delete selected" to delete the TermType Language relation selected in the grid
  /// --> Button "List" to return to the Termbase page
	/// </summary>
  public partial class TermBaseSettings : HCPage
  {
    #region Declarations
    
    
    protected System.Web.UI.WebControls.Label lbTitle;
    #endregion

    #region Code généré par le Concepteur Web Form
    override protected void OnInit(EventArgs e)
    {
      //
      // CODEGEN : Cet appel est requis par le Concepteur Web Form ASP.NET.
      //
      InitializeComponent();
      this.DDL_TermTypeList.SelectedIndexChanged += new System.EventHandler(this.DDL_TermTypeList_SelectedIndexChanged);
      this.DDL_RegionList.SelectedIndexChanged += new System.EventHandler(this.DDL_RegionList_SelectedIndexChanged);
      base.OnInit(e);
    }
		
    /// <summary>
    /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
    /// le contenu de cette méthode avec l'éditeur de code.
    /// </summary>
    private void InitializeComponent()
    {    
      //this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);
      this.dg.InitializeRow += new Infragistics.WebUI.UltraWebGrid.InitializeRowEventHandler(this.dg_InitializeRow);

    }
    #endregion 
  
    // TODO: Create in API an Object TermTypeLanguages
    protected void Page_Load(object sender, System.EventArgs e)
    {
      lbMessage.Text = string.Empty;
      #region Check Capabilities
        
      //if ((SessionState.User.IsReadOnly) || (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_TERM_BASE)))
      //{
        //uwToolbar.Items.FromKeyButton("Add").Enabled = false;
        //uwToolbar.Items.FromKeyButton("Delete").Enabled = false;
      //}
      #endregion

      //DDL_RegionList.AutoPostBack = false;
      if (!Page.IsPostBack)
      {
        try
        {
          #region Load TermType list 
          using (TermTypeList TermTypes = TermType.GetAll())
          {
            DDL_TermTypeList.DataSource = TermTypes;
            DDL_TermTypeList.DataBind();
          }
          #endregion
          LoadRegionList();
          ShowTermTypeLanguage();
        }
        catch
        {
          UITools.DenyAccess(DenyMode.Standard);
        }
      }
    }

    /// <summary>
    /// Show the Languages by TermTpe
    /// </summary>
    private void ShowTermTypeLanguage()
    {
      using (Database dbObj = Utils.GetMainDB())
      {
          using (DataSet ds = dbObj.RunSPReturnDataSet("_TermType_GetLanguages", new SqlParameter("@TermTypeCode", DDL_TermTypeList.SelectedValue), new SqlParameter("@RegionCode", DDL_RegionList.SelectedValue)))
        {
          dg.DataSource = ds;
          Utils.InitGridSort(ref dg);
          dg.DataBind();
        }
      }
    }

    /// <summary>
    /// Refresh list if filter by type
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DDL_TermTypeList_SelectedIndexChanged(object sender, System.EventArgs e)
    {
      LoadRegionList();
      ShowTermTypeLanguage();
    }
      private void DDL_RegionList_SelectedIndexChanged(object sender, System.EventArgs e)
      {
          //LoadRegionList();
          ShowTermTypeLanguage();
      }

    private void LoadRegionList()
    {
      #region Load Languages List

        using (CultureList cultureList = HyperCatalog.Business.Culture.GetByType(CultureType.Regionale))
      {
        using (Database dbObj = Utils.GetMainDB())
        {
            DDL_RegionList.DataSource = cultureList;
          DDL_RegionList.DataBind();
            /*
          if (DDL_RegionList.Items.Count == 0)
          {
            DDL_RegionList.Enabled = false;
            uwToolbar.Items.FromKeyButton("Add").Enabled = false;
          }
          else
          {
            DDL_RegionList.Enabled = true;
            uwToolbar.Items.FromKeyButton("Add").Enabled = true;
          }*/
        }
      }
      #endregion
    }

    /// <summary>
    /// Action for toolbar buttons
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="be"></param>
   /* private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      #region Add
      if (btn == "add")
      {
        Add();
        LoadRegionList();
      }
      #endregion
      #region delete selected settings
      if (btn == "delete")
      {
        Delete();
        LoadRegionList();
      }
      #endregion
    }
      */
    private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      // Display Language
      e.Row.Cells.FromKey("LanguageName").Text = "[" + e.Row.Cells.FromKey("LanguageCode").Text + "] " + e.Row.Cells.FromKey("LanguageName").Text;
    }

    /// <summary>
    /// Add a new termtype Language row
    /// </summary>
      /*
    private void Add()
    {
      using (Database dbObj = Utils.GetMainDB())
      {
        int r = dbObj.RunSPReturnInteger("_TermTypeLanguage_Add",
          new SqlParameter("@TermTypeCode", DDL_TermTypeList.SelectedValue),
          new SqlParameter("@LanguageCode", DDL_RegionList.SelectedValue));
        if ((dbObj.LastError == string.Empty) && (r > 0))
        {
          ShowTermTypeLanguage();
          lbMessage.Text = "Data added";
          lbMessage.CssClass = "hc_success";
          lbMessage.Visible = true;
        }
        else
        {
          lbMessage.Text = "Error: TermType Language relation can't be created";
          lbMessage.CssClass = "hc_error";
          lbMessage.Visible = true;
        }
      }
    }*/

    /// <summary>
    /// Delete the termtype Language row selected
    /// </summary>
    /*private void Delete()
    {
      using (Database dbObj = Utils.GetMainDB())
      {
        for (int i = 0; i < dg.Rows.Count; i++)
        {
          TemplatedColumn col = (TemplatedColumn)dg.Rows[i].Cells.FromKey("Select").Column;
          CheckBox cb = (CheckBox)((CellItem)col.CellItems[i]).FindControl("g_sd");
          if (cb.Checked)
          {
            dbObj.RunSQL("DELETE FROM TermTypeLanguages WHERE TermTypeCode='" + dg.Rows[i].Cells.FromKey("TermTypeCode").ToString() + "' AND LanguageCode='" + dg.Rows[i].Cells.FromKey("LanguageCode").ToString() + "'");
            if ((dbObj.LastError != string.Empty))
            {
              lbMessage.Text = "Error: TermType Language relation can't be created";
              lbMessage.CssClass = "hc_error";
              lbMessage.Visible = true;
              break;
            }
          }
        }
        ShowTermTypeLanguage();
        lbMessage.Text = "Data deleted";
        lbMessage.CssClass = "hc_success";
        lbMessage.Visible = true;
      }
    }*/
  }
}
