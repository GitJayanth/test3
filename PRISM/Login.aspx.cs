using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System;
using System.Net.Mail;
using System.Web.Security;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using HyperCatalog.Shared;
using HyperCatalog.Business;
using System.Configuration;
using System.Web.Caching;
using System.Collections.Generic;
using System.Collections.Specialized;
using log4net;
using HyperComponents.Data.dbAccess;



public partial class _Login : System.Web.UI.Page
{
    protected string appName;
    protected string Pseudo;
    protected int UserId;
    protected string Email;
    protected string LastName;
    protected string FirstName;
    protected DateTime ModifyDate;
    protected string Company;

    //public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    //log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    protected void Page_Load(object sender, EventArgs e)
    {

        // log4net.Config.XmlConfigurator.Configure();
        LogNet.log.Info("Login_debug: Page_Load : Start Time=" + DateTime.Now);
        string headerName = "CL_Header";
        string headerValue = "c_u";
        string decodedString = null;
        //  string headerValue = "=?UTF-8?B?cHJlZmVycmVkbGFuZ3VhZ2U9RU58aHBjbG5hbWU9cmF2aW5kZXItcmVkZHl8aHBjbGlkbnVtYmVyPTFiYjcxMDk2MzQyYzVjMTEyZDQwMmE2MDU0MGE5ZjA4fGhwcmVzaWRlbnRjb3VudHJ5Y29kZT1VU3xzbj1BbGxfUHVifGdpdmVubmFtZT1EY2N8ZW1haWw9QWxsUHViQGhwLmNvbXxjcmVhdGV0aW1lc3RhbXA9MjAwNy0wNy0yNSAwNzoyNTo1Nnxtb2RpZnl0aW1lc3RhbXA9MjAwNy0wNy0yNSAwNzoyNTo1NnxjbGFuZz1VUy1FTg==?=";
        //string headerValue = null;
        //string headerValue = "?UTF-8?B?cHJlZmVycmVkbGFuZ3VhZ2U9ZW58aHBjbG5hbWU9ZGNjX2FsbF9wdWJ8aHBjbGlkbnVtYmVyPTFiYjcxMDk2MzQyYzVjMTEyZDQwMmE2MDU0MGE5ZjA4fGhwcmVzaWRlbnRjb3VudHJ5Y29kZT1VU3xzbj1hbGxfcHVifGdpdmVubmFtZT1kY2N8ZW1haWw9QWxsUHViQGhwLmNvbXxjcmVhdGV0aW1lc3RhbXA9MjAwNy0wNy0yNSAwNzoyNTo1Nnxtb2RpZnl0aW1lc3RhbXA9MjAwOS0wMS0xOSAwNjozOTo0NnxjbGFuZz1VUy1lbg==?=";

        //Response.Write("<br>Header vars: ");
        NameValueCollection headerVariables = Request.Headers;
        String[] headerKeys = headerVariables.AllKeys;
        //Response.Write(((0 == headerKeys.Length) ? "(none)" : "" + headerKeys.Length) + "<br>");
        for (int i = 0; i < headerKeys.Length; ++i)
        {
            if (headerKeys[i].ToString() == headerName)
            {
                headerValue = headerVariables[headerKeys[i]];
                //Response.Write("Header Value is " + headerVariables[headerKeys[i]]);
            }

        }
        string actualdecodedString = "a";


        //Response.Cookies["CL_Cookie"].Value = "patrick";
        //Response.Cookies["CL_Cookie"].Expires = DateTime.Now.AddDays(1);


        if (headerValue == null)
        {

            if (Request.Cookies["CL_Cookie"] != null)
            {
                HttpCookie aCookie = Request.Cookies["CL_Cookie"];
                headerValue = aCookie.Value;
            }

        }
        if (headerValue != null)
        {
            actualdecodedString = headerValue;

        }
        int index1 = actualdecodedString.LastIndexOf("?=");
        string newdecodedString = null; ;
        int pos;

        try
        {

            pos = headerValue.IndexOf("?UTF-8?B?");



            if (pos == 0 || pos == 1)
            {
                char c = actualdecodedString[actualdecodedString.Length - 1];
                char c1 = actualdecodedString[actualdecodedString.Length - 2];


                if (c == '=' && c1 == '?')
                {
                    int l = actualdecodedString.IndexOf("?=");

                    newdecodedString = actualdecodedString.Substring(pos + 9, l - pos - 9);

                }
                else
                {
                    newdecodedString = actualdecodedString.Substring(pos + 9, actualdecodedString.Length - 9);

                }

            }
            else
            {
                newdecodedString = actualdecodedString;
            }


            try
            {
                decodedString = base64Decode(newdecodedString);
            }
            catch (Exception notBase64)
            {
                decodedString = newdecodedString;
            }

            //     Response.Write("<br>");
            //Response.Write("Decoded header is :" + decodedString);
            //    Response.Write("<br>");

            string strData = decodedString;
            char[] separator = new char[] { '|' };
            string[] strSplitArr = strData.Split(separator);
            //string userId = strSplitArr[0];
            //Response.Write(userId);

            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (string arrStr in strSplitArr)
            {
                //Response.Write(arrStr + "\n");
                switch (arrStr.Split('=')[0])
                {
                    case "hpclname":
                        dict.Add("Pseudo", arrStr.Split('=')[1]);
                        break;
                    case "email":
                        dict.Add("Email", arrStr.Split('=')[1]);
                        break;
                    case "modifytimestamp":
                        dict.Add("ModifyDate", arrStr.Split('=')[1]);
                        break;
                    case "sn":
                        dict.Add("LastName", arrStr.Split('=')[1]);
                        break;
                    case "givenname":
                        dict.Add("FirstName", arrStr.Split('=')[1]);
                        break;

                    default:
                        //Response.Write("");
                        break;
                }
            }


            /*Response.Write("Pseudo : "+dict["Pseudo"]);
            Response.Write("<br>");
            Response.Write("Email : "+ dict["Email"]);
            Response.Write("<br>");
            Response.Write("ModifyDate : " + dict["ModifyDate"]);
            Response.Write ("<br>");
            Response.Write("LastName : " + dict["LastName"]);
            Response.Write ("<br>");
            Response.Write("FirstName : " + dict["FirstName"]);
            Response.Write ("<br>");*/


            //Pseudo = "balaji_prism";
            //Email = "balaji.n.viswanath@hpe.com";
            //FirstName = "Balaji";
            //LastName = "Viswanath";
            //ModifyDate = Convert.ToDateTime(DateTime.Now);

            //Pseudo = "harishprabhubg";
            //Email = "harish-prabhu.bg@hpe.com";
            //FirstName = "Harish";
            //LastName = "Prabhu";
            //ModifyDate = Convert.ToDateTime(DateTime.Now);

            //Pseudo = "venkata-siva-jayant.chillara@hpe.com";
            //Email = "venkata-siva-jayant.chillara@hpe.com";
            //FirstName = "Venkata";
            //LastName = "Jayanth";

            //Company = "HPE";
          

            //Pseudo = "maheshl";
            //Email = "mahesh.lakkannavar@hpe.com";
            //FirstName = "Mahesh";
            //LastName = "L";
            //ModifyDate = Convert.ToDateTime(DateTime.Now);


            //  Response.Write("<br>");
            //        Response.Write("Psedo is :" + Pseudo);
            //Response.Write("<br>");
            //Pseudo = "ravinder-reddy";
            // Label1.Text = Pseudo;
            Pseudo = dict["Pseudo"];
            Email = dict["Email"];
            FirstName = dict["FirstName"];
            LastName = dict["LastName"];
            ModifyDate = Convert.ToDateTime(dict["ModifyDate"]);

            Company = GetOrganizationbyPsuedo(Pseudo);
            SessionState.CompanyName = Company;

            try
            {

                if (!SessionState.CheckVersion())
                {
                    lbError.Text = "Sorry, but it is impossible to enter the application. This release of the application cannot be ran with that version of database.";
                    lbError.Visible = true;
                }

                appName = SessionState.CacheParams["AppName"].Value.ToString();
                Page.DataBind();

                if (!Page.IsPostBack)
                {
                    //Pseudo = "dcc_all_pub";
                    using (HyperCatalog.Business.UserList users = HyperCatalog.Business.User.GetByKeyList(Pseudo))
                    {

                        // Response.Write("Users count is :" + users.Count);


                        if (users.Count > 1)
                        {
                            Roleselect.Visible = true;
                            validateBtn.Visible = true;
                            lblRole.Visible = true;

                            for (int i = 0; i < users.Count; i++)
                            {
                                Roleselect.Items.Add(new ListItem(users[i].RoleName, users[i].Id.ToString()));
                                //Response.Write("user id is " + users[i].Id.ToString());

                            }

                        }
                        else
                        {
                            if (users.Count == 1)
                            {

                                UserId = users[0].Id;
                                validateuser();
                                //Response.Write("user id is " + users[0].Id.ToString());
                            }


                        }

                    }
                }
            }


            catch (Exception us)
            {

                //lbError.Text = "You cannot have access to the " + appName + ". Your account may not have been affected to a group. Please contact an PRISM support administrator.";

                lbError.Text = us.Message;
                lbError.Visible = true;
                lblRole.Visible = false;

                // throw new Exception("Error in user retreival" + us.Message);
            }


            //validateuser();
        }

        catch (Exception ex)
        {
            string str = ex.ToString();
            // throw new Exception("Error in header" + ee.Message);
        }
        LogNet.log.Info("Login_debug: Page_Load : End Time=" + DateTime.Now);
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

    protected void validateuser()
    {
        LogNet.log.Debug("Login_debug: validateuser() : Start Time=" + DateTime.Now);
        if (SessionState.CheckVersion())
        {
            using (HyperCatalog.Business.User user = HyperCatalog.Business.User.GetByKey(UserId))
            {
                lbError.Text = "login incorrect.<br>Try again.";
                Response.Write("user id in  GetByKey(UserId)" + UserId);
                if (user != null)
                {

                    if (user.IsActive)
                    {
                        if (user.CulturesCount > 0)
                        {
                            if (user.ItemsCount == 0 && user.RoleId != 9)
                            {
                                lbError.Text = "Sorry, your [" + user.RoleName + "] account has no item assigned<br>Please contact the administrator.";
                            }
                            else
                            {
                                string sKey = user.Pseudo + user.FullName;
                                string sUser = null;//Upon Shelley's request, the message will not be displayed any more.
                                if (sUser == null || sUser == String.Empty)
                                {
                                    #region Prevent multiple connections (this does not work in Web farm)"
                                    TimeSpan SessTimeOut = new TimeSpan(0, 0, HttpContext.Current.Session.Timeout, 0, 0);
                                    HttpContext.Current.Cache.Insert(sKey, sKey, null, DateTime.MaxValue, SessTimeOut,
                                       System.Web.Caching.CacheItemPriority.NotRemovable, null);
                                    Session["uniqueLogin"] = sKey;
                                    #endregion

                                    bool bRefresh = user.UpdateItemScope();
                                    if (bRefresh)
                                    {
                                        SessionState.User = user;
                                        lbError.Visible = false;

                                        SessionState.User.LogCount++;
                                        SessionState.Culture = null;
                                        UITools.FindUserFirstCulture(false);
                                        if (SessionState.Culture == null)
                                        {
                                            UITools.FindUserFirstCulture(true);
                                        }
                                        SessionState.User.LastLogOnDate = DateTime.UtcNow;
                                        SessionState.User.Password = HyperCatalog.DataAccessLayer.SqlTools.EncryptString(SessionState.User.ClearPassword);
                                        //HPP User update details
                                        SessionState.User.Pseudo = Pseudo;
                                        SessionState.User.LastName = LastName;
                                        SessionState.User.FirstName = FirstName;
                                        SessionState.User.Email = Email;
                                        SessionState.User.HPPQuickSave(true);
                                        //HPP User update details

                                        SessionState.User.QuickSave(true);
                                        SessionState.TVAllItems = false;
                                        HyperCatalog.Shared.Security.User curUser = new HyperCatalog.Shared.Security.User(new HyperCatalog.Shared.Security.Identity(SessionState.User.Email, SessionState.User.Id));
                                        FormsAuthentication.Initialize();
                                        curUser.SetAuthenticationCookie();


                                        // Redirect the user to the originally requested page
                                        Session["JustLoggedIn"] = true;
                                        try
                                        {
                                            FormsAuthentication.RedirectFromLoginPage(Pseudo, false);
                                        }
                                        catch (Exception ex)
                                        {
                                            string strURL = (null != Request.QueryString["ReturnURL"] && Request.QueryString["ReturnURL"] != "") ? Request.QueryString["ReturnURL"] : "Default.aspx";
                                            FormsAuthentication.SetAuthCookie(Pseudo, false);
                                            Response.Redirect(strURL);
                                        }

                                    }
                                    else
                                    {
                                        lbError.Text = "Sorry, we cannot pre-compute your items in cache<br>Please contact the administrator.";
                                    }
                                }
                                else
                                {
                                    lbError.Text = "<br>Sorry, it seems that your are already logged with this account.<br>" +
                                                   "If it is not the case, it is because you've ended your session by closing your browser.<br><br/>" +
                                                   "To avoid this message, please always use the Logoff functionality<br/><br/>" +
                                                   "The next you will try to connect, this message will not appear";
                                    Cache.Remove(sKey);
                                }
                            }
                        }
                        else
                        {
                            lbError.Text = "Sorry, your [" + user.RoleName + "] account has no culture assigned<br>Please contact the administrator to update your catalog.";
                        }
                    }
                    else
                    {
                        lbError.Text = "Sorry, your [" + user.RoleName + "] account is not activated<br>Please contact the administrator.";
                    }
                }

            }

        }
        lbError.Visible = true;
        LogNet.log.Debug("Login_debug: validateuser() : End Time=" + DateTime.Now);
    }


    public string base64Decode(string data)
    {
        try
        {

            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();

            byte[] todecode_byte = Convert.FromBase64String(data);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);

            return result;
        }
        catch (Exception e)
        {
            throw new Exception("Error in base64Decode" + e.Message);
        }
    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void validateBtn_Click_new(object sender, EventArgs e)
    {


        UserId = Convert.ToInt32(Roleselect.SelectedValue);

        //Response.Write(UserId);
        validateuser();

    }

    private string GetOrganizationbyPsuedo(string pseudo)
    {
        string sqlQuery = @"select O.OrgCode from Users U inner join Organizations O on U.OrgId = O.OrgId where U.Pseudo = '" + pseudo + "'";
        string result = string.Empty;
        using (Database dbObj = Utils.GetMainDB())
        {
            using (IDataReader rs = dbObj.RunSQLReturnRS(sqlQuery))
            {
                if (rs.Read())
                {
                    result = Convert.ToString(rs[0]).Trim();
                }
                dbObj.CloseConnection();
            }
        }
        return result;
    }
}
