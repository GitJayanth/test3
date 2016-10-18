#region uses
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HyperCatalog.Shared;
using HyperCatalog.Business;
#endregion

namespace HyperCatalog.UI.Acquire.QDE
{
  public partial class QDEFrame : HCPage 
  {

    protected void Page_Load(object sender, System.EventArgs e)
    {
      UITools.ShowSearchBar(this);
      if (SessionState.Culture != null && SessionState.Culture.Type == CultureType.Locale)
      {
        SessionState.Culture = null;
        UITools.FindUserFirstCulture(false);
      }

      SessionState.User.LastVisitedPage = Request.Path;
      
      // TODO: If user has not the manage catego role or is readonly, don't show the MyItems Tab
      if (Item.GetByKey(SessionState.User.LastVisitedItem) == null)
      {
        try
        {
          SessionState.User.LastVisitedItem = SessionState.User.Items[0].Id;
        }
        catch
        {
          SessionState.User.LastVisitedItem = -1;
        }
      }
      if (Item.GetByKey(SessionState.User.LastVisitedItemReadOnly) == null)
      {
        try
        {
          SessionState.User.LastVisitedItemReadOnly = SessionState.User.Items[0].Id;
        }
        catch
        {
          SessionState.User.LastVisitedItemReadOnly = -1;
        }
      }
      SessionState.User.QuickSave();
    }

    
    #region Web Form Designer generated code
    override protected void OnInit(EventArgs e)
    {
      //
      // CODEGEN: This call is required by the ASP.NET Web Form Designer.
      //
      InitializeComponent();
      base.OnInit(e);
    }
		
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {    

    }
    #endregion
  }
}