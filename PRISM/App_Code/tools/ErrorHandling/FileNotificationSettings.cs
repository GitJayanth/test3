using System;
using System.Web;
using System.Web.Mail;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.IO;
using HyperCatalog.Shared;

public class FileNotificationSettings
{
  string m_sFileName;

  public FileNotificationSettings()
  {
    m_sFileName = SessionState.CacheParams["errorNotifier_Filename"].Value.ToString();
    if (m_sFileName == null)
      m_sFileName = "error.txt";
  }

  public string Filename
  {
    get { return m_sFileName;  }
    set { m_sFileName = value; }
  }
}
