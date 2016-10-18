using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Collections;

/// <summary>
/// Summary description for RequestInterceptor
/// Date: 9/26/2011
/// Purpose: XSS attack keyword filtering
/// </summary>
public class RequestInterceptor : IHttpModule
{
    public RequestInterceptor()
    {
        
    }
    public void Init(HttpApplication context)
    {
        context.PreRequestHandlerExecute += new EventHandler(context_PreRequestHandlerExecute);
    }

    void context_PreRequestHandlerExecute(object sender, EventArgs e)
    {
        try
        {
            HttpContext.Current.Request.ValidateInput();  //commented due to breaking the functionality while inputing HTML tags in the textboxes
            foreach (string strQuery in HttpContext.Current.Request.QueryString)
            {
                if (!IsValid(HttpContext.Current.Request.QueryString[strQuery].ToString(), true))
                    throw new Exception("XSS error!");
            }

            //foreach (string strQuery in HttpContext.Current.Request.Form)
            //{
            //    if (!IsValid(HttpContext.Current.Request.Form[strQuery].ToString(), false))
            //        throw new Exception("XSS error!");
            //}
        }
        catch (Exception ex)  
        {            
            HttpContext.Current.Response.Redirect("~/404.aspx");          
        }
    }
    private bool IsValid(string strValidate, bool isQuery)
    {
        ArrayList arrkeywords = (ArrayList)HttpContext.Current.Application["xsskeywords"];
        foreach (string xss in arrkeywords)
        {
            Regex regex = new Regex(xss);
            if (regex.IsMatch(strValidate.ToLower())) return false; 
            else continue;
        }
        if (isQuery)
        {
            ArrayList arrqueryext = (ArrayList)HttpContext.Current.Application["xssquerystringsextended"];
            foreach (string xss in arrqueryext)
            {
                Regex regex = new Regex(xss);
                if (regex.IsMatch(strValidate.ToLower())) return false;
                else continue;
            }
        }
        return true;
    }
       public void Dispose(){}
    
}
