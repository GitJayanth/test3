using System;
using System.Web;
using System.Net.Mail;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.IO;
using HyperCatalog.Shared;
using HyperCatalog.WebServices.EventLoggerWS;

public class ErrorHandler
{
  public bool UseEventLog = false;
  public bool UseFile = false;
  public bool UseEmail = false;
  public bool UseCustomPage = false;
  public bool UseEventLogger = false;
		
  EmailNotificationSettings m_emailSettings = new EmailNotificationSettings();
  FileNotificationSettings  m_fileSettings  = new FileNotificationSettings();
  EventLogSettings          m_logSettings   = new EventLogSettings();

  private string _strWhatHappened = SessionState.CacheParams["errorNotifier_WhatHappened"].Value.ToString();
  private string _strHowUserAffected = SessionState.CacheParams["errorNotifier_HowUserAffected"].Value.ToString();
  private string _strWhatUserCanDo = SessionState.CacheParams["errorNotifier_WhatUserCanDo"].Value.ToString();

  public ErrorHandler()
  {
    UseEventLog = SessionState.CacheParams["errorNotifier_UseEventLog"].Value.ToString() == "1";
    UseFile = SessionState.CacheParams["errorNotifier_UseFile"].Value.ToString() == "1";
    UseEmail = SessionState.CacheParams["errorNotifier_UseEmail"].Value.ToString() == "1";
    UseCustomPage = SessionState.CacheParams["errorNotifier_UseCustomPage"].Value.ToString() == "1";
    UseEventLogger = SessionState.CacheParams["errorNotifier_UseEventLogger"].Value.ToString() == "1";
  }
		
  public EmailNotificationSettings EmailSettings
  {
    get { return m_emailSettings;  }
    set { m_emailSettings = value; }
  }

  public FileNotificationSettings FileSettings
  {
    get { return m_fileSettings;  }
    set { m_fileSettings = value; }
  }

  public EventLogSettings LogSettings
  {
    get { return m_logSettings;  }
    set { m_logSettings = value; }
  }

  public void HandleException()
  {
    Exception e = HttpContext.Current.Server.GetLastError();

    if (e == null)
      return;

    e = e.GetBaseException();
			
    if (e != null)
      HandleException(e);
  }

  public void HandleException(Exception e)
  {
    string sExceptionDescription = FormatExceptionDescription(e);
    string sCustomStyles = GetCustomStyle();
    if (UseFile)
    {
      WriteToFile(GetCustomHeader() + sExceptionDescription + GetCustomFooter());
    }
    if (UseEmail)
    {
      SendEmail(GetCustomHeader() + sExceptionDescription + GetCustomFooter());
    }			
    if (UseEventLog)
    {
      LogEvent(e.Message + "\n" +e.StackTrace);
    }
    if (UseEventLogger)
    {
      LogToDB(e.Message, e.StackTrace);
    }
    if (UseCustomPage)
    {
      DisplayCustomPage(sExceptionDescription);
    }
  }

  protected virtual string FormatExceptionDescription(Exception e)
  {
    string ptb1 = "class='ptb1'";
    string ptb3 = "class='ptb3'";
    string main = "class='main'";
    StringBuilder sb = new StringBuilder();
    HttpContext context = HttpContext.Current;
    sb.Append("<table " + main + ">");
    if (SessionState.User != null)
    {
      sb.Append("<tr><td " + ptb1 +">User Id</td><td " + ptb3 +">" + SessionState.User.Id.ToString() + "</td></tr>");
      sb.Append("<tr><td " + ptb1 +">User Name</td><td " + ptb3 +">" + SessionState.User.FullName + "</td></tr>");
    }
    sb.Append("<tr><td " + ptb1 +">Time of Error</td><td " + ptb3 +">" + DateTime.Now.ToString("g") + Environment.NewLine + "</td></tr>");
    sb.Append("<tr><td " + ptb1 +">URL</td><td " + ptb3 +">" + context.Request.Url + Environment.NewLine + "</td></tr>");
    sb.Append("<tr><td " + ptb1 +">Form</td><td " + ptb3 +">" + context.Request.Form.ToString() + Environment.NewLine + "</td></tr>");
    sb.Append("<tr><td " + ptb1 +">QueryString</td><td " + ptb3 +">" + context.Request.QueryString.ToString() + Environment.NewLine + "</td></tr>");			
    sb.Append("<tr><td " + ptb1 +">Server Name</td><td " + ptb3 +">" + context.Request.ServerVariables["SERVER_NAME"] + Environment.NewLine + "</td></tr>");
    sb.Append("<tr><td " + ptb1 +">User Agent</td><td " + ptb3 +">" + context.Request.UserAgent + Environment.NewLine + "</td></tr>");
    sb.Append("<tr><td " + ptb1 +">User IP</td><td " + ptb3 +">" + context.Request.UserHostAddress + Environment.NewLine + "</td></tr>");
    sb.Append("<tr><td " + ptb1 +">User Host Name</td><td " + ptb3 +">" + context.Request.UserHostName + Environment.NewLine + "</td></tr>");
    sb.Append("<tr><td " + ptb1 +">User is Authenticated</td><td " + ptb3 +">" + context.User.Identity.IsAuthenticated.ToString() + Environment.NewLine + "</td></tr>");
			

    while (e != null)
    {				
      sb.Append("<tr><td " + ptb1 +">Message</td><td " + ptb3 +"><b><font color='red'>" + e.Message + Environment.NewLine + "</font><b></td></tr>");
      sb.Append("<tr><td " + ptb1 +">Source</td><td " + ptb3 +">" + e.Source+ Environment.NewLine + "</td></tr>");
      sb.Append("<tr><td " + ptb1 +">TargetSite</td><td " + ptb3 +">" + e.TargetSite + Environment.NewLine + "</td></tr>");
      sb.Append("<tr><td " + ptb1 +">StackTrace</td><td " + ptb3 +">" + e.StackTrace + Environment.NewLine + "</td></tr>");
      sb.Append("</table>");
      e = e.InnerException;
    }
			
    sb.Append("\n\n");
    return sb.ToString();
  }

  void WriteToFile(string sText)
  {
    string sPath = HttpContext.Current.Server.MapPath(FileSettings.Filename);
    try
    {
      FileStream fs = new FileStream(sPath, FileMode.Append, FileAccess.Write);
      StreamWriter writer = new StreamWriter(fs);
      writer.Write(sText);
      writer.Close();
      fs.Close();
    }
    catch (Exception ex)
    {
      SendEmail("Error processing Exceptions!: " + ex.Message);
    }
  }

  public void SendEmail(string sBody)
  {
      MailMessage msg = new MailMessage(EmailSettings.From, EmailSettings.To);

    msg.IsBodyHtml = EmailSettings.Format.ToLower()=="html";
    msg.Subject    = EmailSettings.Subject;
    msg.Body       = sBody;

    try
    {
        System.Net.Mail.SmtpClient smtpClient = new SmtpClient();
        if (EmailSettings.SmtpServer != null)
            smtpClient.Host = EmailSettings.SmtpServer;
      smtpClient.Send(msg);
    }
    catch (Exception )
    {
    }
  }

  void LogEvent(string sText)
  {	
    try
    {
      if (!EventLog.SourceExists(LogSettings.EventSource))
      {
        EventLog.CreateEventSource(LogSettings.EventSource, "Application");
      }

      EventLog log = new EventLog();
      log.Source = LogSettings.EventSource;
      log.WriteEntry(sText, EventLogEntryType.Error);
    }
    catch (Exception ex)
    {
      SendEmail("Error processing Exceptions!: " + ex.Message);
    }

  }
  void LogToDB(string sMessage, string sStackTrace)
  {
    try
    {
      HyperCatalog.WebServices.EventLoggerWS.WSEventLogger _EventLog = HyperCatalog.WebServices.WSInterface.EventLogger;
      const int CRYSTAL_UI_COMPONENT_ID = 2;
      Guid errorGuid =Guid.NewGuid();
      _EventLog.AddEventLog(CRYSTAL_UI_COMPONENT_ID, errorGuid, 0, HyperCatalog.WebServices.EventLoggerWS.Severity.BEGIN, "Crystal UI Error", "", string.Empty, SessionState.User!=null?SessionState.User.Id:0,0,0,string.Empty);
      _EventLog.AddEventLog(CRYSTAL_UI_COMPONENT_ID, errorGuid, 0, HyperCatalog.WebServices.EventLoggerWS.Severity.ERROR, "Crystal UI Error", sMessage, string.Empty, SessionState.User != null ? SessionState.User.Id : 0, 0, 0, sStackTrace);
      _EventLog.AddEventLog(CRYSTAL_UI_COMPONENT_ID, errorGuid, 0, HyperCatalog.WebServices.EventLoggerWS.Severity.FAILURE, "Crystal UI Error", "", string.Empty, SessionState.User != null ? SessionState.User.Id : 0, 0, 0, string.Empty);
      _EventLog.Dispose();
    }
    catch (Exception ex)
    {
      SendEmail("Error processing Exceptions!: " + ex.Message);
    }
  }
    
  private void WriteLine(string sText)
  {
    HttpContext.Current.Response.Write(sText);
    HttpContext.Current.Response.Write(Environment.NewLine);
  }

  private string GetCustomStyle()
  {
    StringBuilder sb = new StringBuilder(); 
    sb.Append("<STYLE>" + Environment.NewLine);
    sb.Append("body {font-family:'Verdana';font-weight:normal;font-size: .7em;color:black; background-color:white;}" + Environment.NewLine);
    sb.Append("b {font-family:'Verdana';font-weight:bold;color:black;margin-top: -5px}" + Environment.NewLine);
    sb.Append("H1 { font-family:'Verdana';font-weight:normal;font-size:16pt;color:red }" + Environment.NewLine);
    sb.Append("H2 { font-family:'Verdana';font-weight:normal;font-size:13pt;color:maroon }" + Environment.NewLine);
    sb.Append("pre {font-family:'Lucida Console';font-size: .9em}" + Environment.NewLine);
    sb.Append(".ptb1 {border-right: gainsboro 1px inset;border-bottom: gainsboro 1px inset;padding-right: 1pt;padding-left: 1px;font-size: 8pt;font-family: Verdana, Tahoma, Arial;background-color: silver;}" + Environment.NewLine);
    sb.Append(".ptb3 {padding-right: 5pt;border-top: silver 1px solid;padding-left: 1pt; font-size: 7.7pt;border-left: silver 1px solid;color: black;padding-top: 1pt;font-family: Verdana, Arial, Tahoma;background-color: white;text-align: left;}" + Environment.NewLine);
    sb.Append(".main {background-color: whitesmoke;border: 1px solid #808080;padding: 0px;margin: 0px;width: 500px;height: 100%;}" + Environment.NewLine);
    sb.Append("</STYLE>" + Environment.NewLine);
    return sb.ToString();
  }
  
  private string GetCustomHeader()
  {
    StringBuilder sb = new StringBuilder(); 
    sb.Append("<HTML>" + Environment.NewLine);
    sb.Append("<HEAD>" + Environment.NewLine);
    sb.Append("<TITLE>" + Environment.NewLine);
    sb.Append(SessionState.CacheParams["errorNotifier_PageTitle"].Value.ToString() + Environment.NewLine);
    sb.Append("</TITLE>" + Environment.NewLine);
    sb.Append(GetCustomStyle() + Environment.NewLine);
    sb.Append("</HEAD>" + Environment.NewLine);
    sb.Append("<BODY>" + Environment.NewLine);
    return sb.ToString();
  }
  private string GetCustomFooter()
  {
    StringBuilder sb = new StringBuilder(); 
    string strPageTitle = SessionState.CacheParams["errorNotifier_PageTitle"].Value.ToString();
    sb.Append("</BODY>" + Environment.NewLine);
    sb.Append("</HTML>" + Environment.NewLine);
    return sb.ToString();
  }
  private void DisplayCustomPage(string sText)
  {
    
    HttpContext.Current.Response.Clear();
    HttpContext.Current.Response.ClearContent();
    WriteLine(GetCustomHeader());
    WriteLine("<H1>");
    WriteLine(SessionState.CacheParams["errorNotifier_PageTitle"].Value.ToString());
    WriteLine("<hr width=100% size=1 color=silver></H1>");
    WriteLine("<H2>What Happened:</H2>");
    WriteLine("<BLOCKQUOTE>");
    WriteLine(_strWhatHappened);
    WriteLine("</BLOCKQUOTE>");
    WriteLine("<H2>How this will affect you:</H2>");
    WriteLine("<BLOCKQUOTE>");
    WriteLine(_strHowUserAffected);
    WriteLine("</BLOCKQUOTE>");
    WriteLine("<H2>What you can do about it:</H2>");
    WriteLine("<BLOCKQUOTE>");
    WriteLine(_strWhatUserCanDo);
    WriteLine("</BLOCKQUOTE>");
    WriteLine("<H2>More info:</H2>");
    WriteLine("<TABLE width=100% bgcolor='#ffffcc'>");
    WriteLine("<TR><TD>");
    WriteLine("<CODE><PRE>");
    WriteLine(sText);
    WriteLine("</PRE></CODE>");
    WriteLine("<TD><TR>");
    WriteLine(GetCustomFooter());
    HttpContext.Current.Response.Flush();
    HttpContext.Current.Server.ClearError();
    HttpContext.Current.Response.End();
  }

  public void ClearLoggedFileErrors()
  {
    try
    {
      File.Delete(HttpContext.Current.Server.MapPath(FileSettings.Filename));
    }
    catch (Exception )
    {
    }
  }
}
