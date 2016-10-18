using System;
using System.Web;
using System.Web.Mail;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.IO;

public class EventLogSettings
{
  string m_sEventSource;

  public EventLogSettings()
  {
    m_sEventSource = ConfigurationManager.AppSettings["errorNotifier_EventLogSource"];
    if (m_sEventSource == null)
      m_sEventSource = "ASP.NET App Error";
  }

  public string EventSource
  {
    get { return m_sEventSource;  }
    set { m_sEventSource = value; }
  }

}
