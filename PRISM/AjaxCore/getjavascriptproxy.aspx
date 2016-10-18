<%@ Page Language="C#" Debug="true" %>

<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Net" %>
<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="System.Xml.Xsl" %>

<!-- 
/// 19.07.2005 white space removed
/// 20.07.2005 more datatypes and XML Documents
/// 04.09.2005 XslCompiledTransform
 -->
 
<script runat="server">
  private string FetchWsdl(string url) {
    string p = Page.ResolveUrl(url);
    url = "http";
    if (Request.IsSecureConnection)
    {
      url += "s";
    } 
    url += "://" + HyperCatalog.Business.ApplicationSettings.Components["Web server"].URI + p;
    Uri uri = new Uri(Request.Url, url + "?WSDL");
    uri = new Uri(Request.Url, "https://c4w23733.itcs.hpe.com/UIExtensions/Extensions.asmx?WSDL");
      
    /*if ((url != null) && (url.StartsWith("~/")))
      url = Request.ApplicationPath + url.Substring(1);
    if (url.EndsWith(".asmx", StringComparison.InvariantCultureIgnoreCase))
      url = url + "?WSDL";
    Uri uri = new Uri(Request.Url, url);*/
    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
    req.Credentials = CredentialCache.DefaultCredentials;
    // req.Proxy = WebRequest.DefaultWebProxy; // running on the same server !
    req.Timeout = 6 * 1000; // 6 seconds
    try
    {
      WebResponse res = req.GetResponse();
#if DOTNET11
    XmlDocument data = new XmlDocument();
    data.Load(res.GetResponseStream());

    XslTransform xsl = new XslTransform();
    xsl.Load(Server.MapPath("~/ajaxcore/wsdl.xslt"));

    System.IO.StringWriter sOut = new System.IO.StringWriter();
    xsl.Transform(data, null, sOut, null);
#else
      XmlReader data = XmlReader.Create(res.GetResponseStream());

      XslCompiledTransform xsl = new XslCompiledTransform();
      xsl.Load(Server.MapPath("~/ajaxcore/wsdl.xslt"));

      System.IO.StringWriter sOut = new System.IO.StringWriter();
      xsl.Transform(data, null, sOut);
#endif
      return (sOut.ToString());
    }
    catch (Exception e) {
      Response.Write(e.ToString());
      return string.Empty; 
    }

  } // FetchWsdl
</script>

<%
  string asText = Request.QueryString["html"];

  Response.Clear();
  if (asText != null) {
    Response.ContentType = "text/html";
    Response.Write("<pre>");
  } else {
    Response.ContentType = "text/text";
  } // if

  string fileName = Request.QueryString["service"];
  if (fileName == null)
    fileName = "CalcService";

  // get good filenames only (local folder)
  if ((fileName.IndexOf('$') >= 0) || (Regex.IsMatch(fileName, @"\b(COM\d|LPT\d|CON|PRN|AUX|NUL)\b", RegexOptions.IgnoreCase)))
    throw new ApplicationException("Error in filename.");

  if (! Server.MapPath(fileName).StartsWith(Request.PhysicalApplicationPath, StringComparison.InvariantCultureIgnoreCase))
    throw new ApplicationException("Can show local files only.");

  string ret = FetchWsdl(fileName);
  ret = Regex.Replace(ret, @"\n *", "\n");
  ret = Regex.Replace(ret, @"\r\n *""", "\"");
  ret = Regex.Replace(ret, @"\r\n, *""", ",\"");
  ret = Regex.Replace(ret, @"\r\n\]", "]");
  ret = Regex.Replace(ret, @"\r\n; *", ";");
  Response.Write(ret);
%>