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

namespace HyperCatalog
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        private bool _ShowFrame = true;
        public bool ShowFrame
        {
            get { return _ShowFrame; }
            set { _ShowFrame = value; }
        }

        #region SessionRefresh
        private void AddKeepAlive()
        {
            int int_MilliSecondsTimeOut = (this.Session.Timeout * 60000) - 30000;
            string str_Script = @"
<script type='text/javascript'>
//Number of Reconnects
var count=0;
//Maximum reconnects setting
var max = 5;
function Reconnect(){

count++;
if (count < max)
{
//window.status = 'Link to Server Refreshed ' + count.toString()+' time(s)' ;

var img = new Image(1,1);

img.src = sRootUrl + '/Reconnect.aspx';

}
}

window.setInterval('Reconnect()'," + int_MilliSecondsTimeOut.ToString() + @"); //Set to length required

</script>

";

            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Reconnect", str_Script);

        }
        #endregion
        protected string appName = "PRISM";
        private bool CheckIfUserHasAccessToPage()
        {
            string cur = HttpContext.Current.Request.Path.ToLower();
            if (cur.IndexOf("default.aspx") > 0 || cur.IndexOf("help.aspx") > 0 || cur.IndexOf("404.aspx") > 0 ||
                cur.IndexOf("forbidden.aspx") > 0 || cur.IndexOf("redirect.aspx") > 0 ||
                cur.IndexOf("redirect.aspx") > 0 || cur.IndexOf("deliveryoutputs.aspx") > 0
              || cur.IndexOf("search.aspx") > 0 || cur.IndexOf("/reports/") > 0) //Updated for eZ# 70039
            {
                return true;
            }
            DataRowCollection drc = SessionState.UIMenuItems.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                DataRow dr = drc[i];
                if (dr["URL"] != DBNull.Value && dr["URL"].ToString() != string.Empty)
                {
                    string s = dr["URL"].ToString().ToLower();
                    if (s.StartsWith("~"))
                    {
                        s = Page.ResolveUrl(s).ToLower();
                    }
                    if (s == cur || s.IndexOf(cur) > 0)
                    {
                        return true;
                    }
                }
            }
            UITools.DenyAccess(DenyMode.Standard);
            return false;
        }
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (SessionState.User.IsDefaultPassword && !Request.Path.EndsWith("UI/Admin/UserProfile.aspx"))
            {
                Response.Redirect("~/UI/Admin/UserProfile.aspx?newPwd=1");
                return;
            }
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);

            if (Session["JustLoggedIn"] != null)
                Session.Remove("JustLoggedIn");
            if (SessionState.Culture == null)
            {
                UITools.FindUserFirstCulture(false);
            }
            if (SessionState.Culture == null)
            {
                UITools.FindUserFirstCulture(true);
            }
            //foreach (HyperCatalog.Business.Culture cul in SessionState.User.Cultures)
            //{
            //  Response.Write(cul.Code + "<br/>");
            //}
            //Response.End();
            if (SessionState.Culture == null)
            {
                UITools.DenyAccess(DenyMode.Standard);
            }
            // EZilla 70966, Commented the code that makes the page keep alive.
            //AddKeepAlive();
            appName = HyperCatalog.Shared.SessionState.AppName;
            string commonPath = Page.ResolveUrl("~/").ToLower();
            string fullPath = HttpContext.Current.Request.Path.ToLower();
            string pagePath = fullPath.Replace(commonPath, string.Empty);
            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "PanelSearch", "<script>panelSearchId = '" + panelSearch.ClientID + "';txtSearchId = '" + txtSearch.ClientID + "';ccc = '" + SessionState.Culture.Code + "';</script>");
            if (!Page.IsPostBack)
            {
                txtSearch.Attributes.Add("onFocus", "if(this.value==' Search')this.value='';");
                txtSearch.Attributes.Add("onBlur", "if(this.value=='')this.value=' Search [' + ccc + ']';");
                txtSearch.Attributes.Add("onkeypress", "if (window.event && window.event.keyCode == 13){document.getElementById('SearchText').value = document.getElementById('" + txtSearch.ClientID + "').value;DoSkuSearch();return false;}");
                txtSearch.Attributes.Add("onClick", "select();");

                imgSearch.Attributes.Add("onClick", "document.getElementById('SearchText').value = document.getElementById('" + txtSearch.ClientID + "').value;DoSkuSearch();return false;");
                if (!fullPath.EndsWith("default.aspx"))
                {
                    SessionState.User.LastVisitedPage = HttpContext.Current.Request.Path + HttpContext.Current.Request.QueryString;
                    SessionState.User.QuickSave();
                }
                txtSearch.Text = "Search [" + SessionState.Culture.Code + "]";
                // Contextual help
                lbUser.Text = SessionState.User.FullName;
                lbOrganization.Text = SessionState.User.OrgName;
                lbRole.Text = SessionState.User.RoleName;
                lbTimeZone.Text = SessionState.User.GetUTCString(true).ToString();
            }
            string script = "<script>\nvar sContextualHelp='';";
            script += "\n\tvar sRootUrl='" + commonPath.Replace("'", "\'") + "';";
            string urlQuery = Server.UrlDecode(Request.Url.PathAndQuery).ToLower().Replace("'", "\'");
            script += "\n\tvar wsRootUrl='" + Server.UrlDecode(Page.Request.Url.ToString()).ToLower().Replace(urlQuery, string.Empty).Replace("'", "\'") + Page.Form.ResolveUrl("~/").Replace("'", "\'") + "';";
            script += "\n\tvar wsExtensionsUrl='https://c4w23733.itcs.hpe.com/UIExtensions/Extensions.asmx';";

            script += "\n</script>";
            // User Info
            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "sContextualHelp", script);

            // HPP Integration 

            //if (SessionState.AppName.IndexOf("Crystal") > -1)
            //{
            //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "HPCrystal", "<script type='text/javascript' src='" + "/hc_v4/js/Crystalcfhook/cfhook.js" + "'><" + "/script>\n");

            //}
            //else
            //{
            //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "HPGemstone", "<script type='text/javascript' src='" + "/hc_v4/js/Gemstonecfhook/cfhook.js" + "'><" + "/script>\n");
            //}
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            CheckIfUserHasAccessToPage();
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.logoutLk_btn_ho.Click += new System.EventHandler(this.logoutLk_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        protected void logoutLk_Click(object sender, System.EventArgs e)
        {
            string sKey = SessionState.User.Pseudo + SessionState.User.FullName;
            if (Cache[sKey] != null)
            {
                Cache.Remove(sKey);
            }
            SessionState.User.Dispose();
            if (SessionState.QDEChunk != null)
            {
                SessionState.QDEChunk.Dispose();
            }
            if (SessionState.CurrentItem != null)
            {
                SessionState.CurrentItem.Dispose();
            }
            if (SessionState.Culture != null)
            {
                SessionState.Culture.Dispose();
            }
            HyperCatalog.Shared.Security.User.RemoveAuthenticationCookie();
            Session.RemoveAll();
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            string hppurl = SessionState.CacheComponents["HPPLogout"].URI;
            Response.Redirect(hppurl);
        }
        protected void linkbutton1_Click(object sender, EventArgs e)
        {
            string hppurl = SessionState.CacheComponents["HPPEditProfile"].URI;
            Response.Redirect(hppurl);

        }
    }

}