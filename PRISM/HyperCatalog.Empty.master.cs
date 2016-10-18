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

public partial class EmptyMasterPage : System.Web.UI.MasterPage
{
    private void Page_Load(object sender, System.EventArgs e)
    {
        if (Session["JustLoggedIn"] != null)
        {
            Session.Remove("JustLoggedIn");
            Response.Redirect("~/");
        }
        Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);

        string commonPath = Page.ResolveUrl("~/").ToLower();
        string fullPath = Request.Path.ToLower();
        string pagePath = fullPath.Replace(commonPath, string.Empty);
        string script = "<script>\nvar sContextualHelp='';";
        script += "\n\tvar sRootUrl='" + commonPath.Replace("'", "\'") + "';";
        string urlQuery = Server.UrlDecode(Request.Url.PathAndQuery).ToLower().Replace("'", "\'");
        script += "\n\tvar wsRootUrl='" + Server.UrlDecode(Page.Request.Url.ToString()).ToLower().Replace(urlQuery, string.Empty).Replace("'", "\'") + Page.Form.ResolveUrl("~/").Replace("'", "\'") + "';";
        script += "\n\tvar wsExtensionsUrl='https://c4w23733.itcs.hpe.com/UIExtensions/Extensions.asmx';";
        script += "\n\tvar strPID='" + HyperCatalog.Shared.SessionState.CacheParams["HeaderLongNameLive"].Value.ToString() + "';";
        script += "\n\tvar strPOD='" + HyperCatalog.Shared.SessionState.CacheParams["HeaderLongNameObsolete"].Value.ToString() + "';";
        script += "\n\tvar strBlind='" + HyperCatalog.Shared.SessionState.CacheParams["HeaderLongNameBlind"].Value.ToString() + "';";
        script += "\n\tvar strAnnouncement='" + HyperCatalog.Shared.SessionState.CacheParams["HeaderLongNameAnnouncement"].Value.ToString() + "';";
        script += "\n\tvar strRemoval='" + HyperCatalog.Shared.SessionState.CacheParams["HeaderLongNameRemoval"].Value.ToString() + "';";
        script += "</script>";
        // User Info
        lbStartupJs.Text = script;
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

