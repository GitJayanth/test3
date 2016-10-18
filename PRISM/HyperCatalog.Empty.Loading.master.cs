using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HyperCatalog.UI.Login;
using HyperCatalog.Shared;

public partial class EmptyLoadingMasterPage : System.Web.UI.MasterPage
{
    private void Page_Load(object sender, System.EventArgs e)
    {
      if (Session["JustLoggedIn"] != null)
      {
        Session.Remove("JustLoggedIn");
        Response.Redirect("~/");
      }

      string commonPath = Page.ResolveUrl("~/").ToLower();
      string fullPath = HttpContext.Current.Request.Path.ToLower(); 
      string pagePath = fullPath.Replace(commonPath, string.Empty);
      string script = "<script>var sContextualHelp='';";    
      script += "var sRootUrl='" + commonPath + "';";
      //using (HyperCatalog.Business.Menu m = HyperCatalog.Business.Menu.GetByKey(pagePath))
      //{
      //  if (m != null)
      //  {
      //    if (m.Help != string.Empty)
      //    {
      //      script += "sContextualHelp='" + m.Help.Replace("'", " ") + "';";
      //    }
      //  }
      //}
      script += "</script>";
      // User Info
      Page.ClientScript.RegisterStartupScript(Page.GetType(),"sContextualHelp", script);   
    }

    #region Web Form Designer generated code
    override protected void OnInit(EventArgs e) 
    {
      InitializeComponent();
      base.OnInit(e);
    }
		
    private void InitializeComponent() 
    {
      this.Load += new System.EventHandler(this.Page_Load);

    }
    #endregion
}

