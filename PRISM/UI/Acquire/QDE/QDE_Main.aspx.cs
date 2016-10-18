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
using HyperCatalog.Shared;
	/// <summary>
	/// Description résumée de qde.
	/// </summary>
	public partial class qde_main : HCPage
	{
    protected string sExpanded = "n";
    protected string sSearch = "0";
    protected string sNode = string.Empty;
    protected string masterOnly="1";

    protected void Page_Load(object sender, System.EventArgs e)
    {
      // Ensure that scope has not change during session
      QDEUtils.UpdateCultureCodeFromRequest();      
      frametv.Attributes["src"] = "QDE_TV.aspx";
      frameitemscope.Attributes["src"] = "QDE_ItemScope.aspx";
      using (HyperCatalog.Business.Item item = QDEUtils.GetItemIdFromRequest())
      {
        System.Int64 itemId = item.Id;
        if (HyperCatalog.Business.Item.IsEligible(itemId, SessionState.Culture.CountryCode)
          && item != null && !item.IsExcludedByPublisher(SessionState.Culture.CountryCode)
          && (!SessionState.User.HasCultureInScope(SessionState.Culture.Code) && SessionState.Culture.Type != HyperCatalog.Business.CultureType.Master))
        {
          // item is eligible in current country and it is not user scope
          SessionState.TVAllItems = true;
          SessionState.User.LastVisitedItemReadOnly = itemId;
        }
        else if (!HyperCatalog.Business.Item.IsEligible(itemId, SessionState.Culture.CountryCode)
                 || item == null
                 || (item != null && item.IsExcludedByPublisher(SessionState.Culture.CountryCode)))
        {
          // item is not eligible in current country
          if (SessionState.TVAllItems)
          {
              //  08/12/2009 QC 2692 - Modified by Sateesh -- The workflow Status 'R'/'C' only should be  visible in locales
            itemId = SessionState.User.LastVisitedItemReadOnly = SessionState.User.GetFirstReadCountryItem(SessionState.Culture.CountryCode, SessionState.Culture.Code);
          }
          else
          {
              //  08/12/2009 QC 2692 - Modified by Sateesh -- The workflow Status 'R'/'C' only should be  visible in locales
            itemId = SessionState.User.LastVisitedItemReadOnly = SessionState.User.LastVisitedItem = SessionState.User.GetFirstCountryItem(SessionState.Culture.CountryCode,SessionState.Culture.Code);
          }
        }
        else
        {
          // item is eligible in current country and it is in user scope
          SessionState.User.LastVisitedItemReadOnly = SessionState.User.LastVisitedItem = itemId;
        }
        if (SessionState.TVAllItems)
        {
          frametv.Attributes["src"] += "?all=1";
          frameitemscope.Attributes["src"] += "?all=1";
        }
        SessionState.User.QuickSave();
        if (Request["g"] != null) // Translate
        {
          frametoolbar.Attributes["src"] = "QDE_ToolBar.aspx?g=" + Request["g"].ToString();
        }
        framecontent.Attributes["src"] = "QDE_FormRoll.aspx?i=" + itemId.ToString() + "&l=" + SessionState.Culture.Code;
      }
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

    }
		#endregion
	}
