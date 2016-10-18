#region uses
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HyperCatalog.Business;
using System.Data.SqlClient;
using HyperComponents.Data.dbAccess;
using HyperCatalog.Shared;
#endregion

namespace HyperCatalog.UI.Acquire
{
	/// <summary>
	/// Manage the Dictionary Sytem Words by Locale
	/// --> Add a new dictionary
	/// --> Display the list of all dictionaries available
	/// </summary>
  public partial class SystemWords : HCPage
  {
    #region Declarations
    protected System.Web.UI.WebControls.DropDownList DDL_LocalesList;
    #endregion

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
      this.dg.InitializeRow += new Infragistics.WebUI.UltraWebGrid.InitializeRowEventHandler(this.dg_InitializeRow);

    }
    #endregion
 
    protected void Page_Load(object sender, System.EventArgs e)
    {
      #region Check Capabilities
      if ((SessionState.User.IsReadOnly) || (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_SPELL_CHECKER)))
      {
        uwToolbar.Items.FromKeyButton("Add").Enabled = false;
      }
      #endregion
      if (!Page.IsPostBack)
      {
        ShowDictionaryList();
      }
      else
      {
        #region Action after changes in term page 
        if (Request["action"]!=null && Request["action"].ToString().ToLower()=="reload")
        {
          ShowDictionaryList();
        }
        #endregion
      }
    }

    private void ShowDictionaryList()
    {
      using (Database dbObj = Utils.GetMainDB())
      {
        using (DataSet ds = dbObj.RunSQLReturnDataSet("SELECT LanguageCode, COUNT(*) as Count FROM DictionarySystemWords GROUP BY LanguageCode", "Dictionaries"))
        {
          dg.DataSource = ds.Tables["Dictionaries"];
          lbNoresults.Visible = false;
          dg.Bands[0].ColHeadersVisible = Infragistics.WebUI.UltraWebGrid.ShowMarginInfo.Yes;
          if (ds.Tables["Dictionaries"].Rows.Count == 0)
          {
            lbNoresults.Text = "None dictionary available";
            lbNoresults.Visible = true;
            dg.Bands[0].ColHeadersVisible = Infragistics.WebUI.UltraWebGrid.ShowMarginInfo.No;
          }
          Utils.InitGridSort(ref dg);
          dg.DataBind();
        }
      }
    }
    
    private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      #region Display full name of CultureCode
      using (CultureList list = HyperCatalog.Business.Culture.GetAll("CultureTypeId=" + HyperCatalog.Business.Culture.GetCultureTypeFromEnum(CultureType.Locale)))
      {
        for (int i = 0; i < list.Count; i++)
        {
          if (list[i].Code == e.Row.Cells.FromKey("Locale").Text)
          {
            e.Row.Cells.FromKey("Locale").Text = "[" + e.Row.Cells.FromKey("Locale").Text + "] " + list[i].Name;
          }
        }
      }
      #endregion
    }

  }
}
