using System;
using System.Web.Security;
using System.Security.Principal;

using System.Web;

namespace HyperCatalog.Shared.Security
{
  public class User : GenericPrincipal
  {
    private static string cookieName;

    public User(Identity id):base(id,new string[0])
    {
    }

    public static void GetAuthenticationCookie()
    {
      string instanceName = HttpContext.Current.Request.ApplicationPath.Split('/')[1].ToUpper();
      cookieName = FormsAuthentication.FormsCookieName + "_" + instanceName;
      HttpCookie authCookie = HttpContext.Current.Request.Cookies[cookieName];
      if(null == authCookie) // There is no authentication cookie.
      {
        return;
      } 
      FormsAuthenticationTicket authTicket = null;
      try
      {
        authTicket = FormsAuthentication.Decrypt(authCookie.Value);
        if (null == authTicket) // Cookie failed to decrypt.
        {
          return; 
        }
      }
      catch (Exception exc)
      {
        // Log exception details (omitted for simplicity)
        return;
      }

      HyperCatalog.Shared.Security.Identity id = new HyperCatalog.Shared.Security.Identity(authTicket);
      // This principal will flow throughout the request.
      HyperCatalog.Shared.Security.User principal = new HyperCatalog.Shared.Security.User(id);
      // Attach the new principal object to the current HttpContext object
      HttpContext.Current.User = principal;
    }

    public void SetAuthenticationCookie()
    {
      // Create the authentication ticket
      FormsAuthenticationTicket authTicket = new 
        FormsAuthenticationTicket(1,   // version
        HyperCatalog.Shared.SessionState.User.Pseudo,                // user name
        DateTime.Now,                  // creation
        DateTime.Now.AddMinutes(60),   // Expiration
        false,                         // Persistent
        HyperCatalog.Shared.SessionState.User.Id.ToString());// User data

      // Now encrypt the ticket
      string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

      // Create a cookie and add the encrypted ticket to the cookie as data.
      HttpContext.Current.Response.Cookies.Add(new HttpCookie(cookieName,encryptedTicket));
      HttpContext.Current.User = this;
    }

    public static void RemoveAuthenticationCookie()
    {
      string instanceName = HttpContext.Current.Request.ApplicationPath.Split('/')[1].ToUpper();
      cookieName = FormsAuthentication.FormsCookieName + "_" + instanceName;
      HttpContext.Current.Response.Write(cookieName);
      HttpContext.Current.Response.Cookies.Remove(cookieName);
    }

  }

  public class Identity : GenericIdentity
  {
    public string Pseudo;
    public int UserId = -1;

    public Identity(string pseudo):base(pseudo)
    {
      Pseudo = pseudo;
    }

    public Identity(string pseudo,int userId):this(pseudo)
    {
      UserId = userId;
    }

    public Identity(FormsAuthenticationTicket authTicket):this(authTicket.Name)
    {
      if (authTicket!=null)
        try
        {
          UserId = Convert.ToInt32(authTicket.UserData);
        }
        catch{}
    }


    public override bool IsAuthenticated
    {
      get
      {
        return UserId>=0;
      }
    }

  }

}