using System;
using System.Web;
using System.Web.Mail;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.IO;
using HyperCatalog.Shared;

public class EmailNotificationSettings
{
  string m_sTo;
  string m_sFrom;
  string m_sSubject;
  string m_sFormat;
  string m_sSmtpServer = null;

  public EmailNotificationSettings()
  {
    m_sTo = SessionState.CacheParams["errorNotifier_EmailTo"].Value.ToString();
    if (m_sTo == null)
      m_sTo = "error@mydomain.com";

    m_sFormat = SessionState.CacheParams["errorNotifier_EmailFormat"].Value.ToString();
    if (m_sFormat == null)
      m_sFormat = "Text";
    m_sFrom = SessionState.CacheParams["errorNotifier_EmailFrom"].Value.ToString();
    if (m_sFrom == null)
      m_sFrom = "errornotifier@mydomain.com";
    
    m_sSubject = SessionState.CacheParams["errorNotifier_EmailSubject"].Value.ToString();
    if (m_sSubject == null)
      m_sSubject = "Error in ASP.NET app";

    m_sSmtpServer = SessionState.CacheParams["errorNotifier_EmailSmtpServer"].Value.ToString();
  }

  public string To
  {
    get { return m_sTo;  }
    set { m_sTo = value; }
  }

  public string From
  {
    get { return m_sFrom;  }
    set { m_sFrom = value; }
  }

  public string Subject
  {
    get { return m_sSubject;  }
    set { m_sSubject = value; }
  }
  public string Format
  {
    get { return m_sFormat;  }
    set { m_sFormat = value; }
  }
  

  public string SmtpServer
  {
    get { return m_sSmtpServer;  }
    set { m_sSmtpServer = value; }
  }

}
