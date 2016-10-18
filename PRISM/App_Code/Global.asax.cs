using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;

namespace HyperCatalog 
{
	/// <summary>
	/// Description résumée de Global.
	/// </summary>
	public class Global : System.Web.HttpApplication
	{
		/// <summary>
		/// Variable nécessaire au concepteur.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public Global()
		{
			InitializeComponent();
		}	
		
		protected void Application_Start(Object sender, EventArgs e)
		{
          System.Web.HttpContext.Current.Trace.Warn("Application Starts begins");
          HyperCatalog.Business.Settings.Values = new HyperCatalog.Business.Settings.WebConfigSettingValues();
          System.Web.HttpContext.Current.Trace.Warn("Application Starts Ends");
        } 
  
		protected void Session_Start(Object sender, EventArgs e)
		{
            System.Web.HttpContext.Current.Trace.Warn("Session Starts");      
        }

    protected void Application_AuthorizeRequest(object sender, EventArgs e)
    {
      if (Request.Path.ToLower().IndexOf("webresource.axd") > -1)
      {
        Response.Cache.SetCacheability(HttpCacheability.Public);
        string eTag = "axd-cache";
        Response.Cache.SetETag(eTag);
        if (Request.Headers.Get("If-None-Match") == eTag)
        {
          Response.StatusCode = 304;
          Response.End();
        }
      }
    }
		protected void Application_BeginRequest(Object sender, EventArgs e)
		{
          string instanceName = ((System.Web.HttpApplication)sender).Request.ApplicationPath.Split('/')[1].ToUpper();
          System.Web.HttpContext.Current.Trace.Warn("Application_BeginRequest Starts and instance name=" + instanceName);
          if (System.Configuration.ConfigurationManager.AppSettings[instanceName+"_ConnectionString"]!=null)
            HyperCatalog.Business.Settings.ConnectionString = System.Configuration.ConfigurationManager.AppSettings[instanceName+"_ConnectionString"];
		}

		protected void Application_EndRequest(Object sender, EventArgs e)
		{
    }

		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{
          //EZilla fix for 68988, only ensure persistent connection if user is not trying to logout
          if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Request["__EVENTTARGET"] != null && ! (System.Web.HttpContext.Current.Request["__EVENTTARGET"].ToString().IndexOf("logoutLk_btn_ho") > 0))
          {
            HyperCatalog.Shared.Security.User.GetAuthenticationCookie();
          }
		}

		protected void Application_Error(Object sender, EventArgs e)
		{
            ErrorHandler handler = new ErrorHandler();
            handler.HandleException();
           
            Server.ClearError();
		}

		protected void Session_End(Object sender, EventArgs e)
		{

		}

		protected void Application_End(Object sender, EventArgs e)
		{

		}
    protected void Application_PreRequestHandlerExecute(Object sender, EventArgs e)
    {
    }
			
		#region Code généré par le Concepteur Web Form
		/// <summary>
		/// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
		/// le contenu de cette méthode avec l'éditeur de code.
		/// </summary>
		private void InitializeComponent()
		{    
			this.components = new System.ComponentModel.Container();
		}
		#endregion
	}
}

