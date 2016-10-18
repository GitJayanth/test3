using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
/// <summary>
/// Summary description for LogNet
/// </summary>
public class LogNet
{
    
	public LogNet()
	{
		
	}
    
        public static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    

    }
