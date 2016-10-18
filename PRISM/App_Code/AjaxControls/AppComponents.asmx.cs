//using AJAXControls;
using System;
using System.ComponentModel;
using System.IO;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Web.Caching;
using System.Collections.Specialized;
using System.Web.Services;
using System.Web.Services.Protocols;
using HyperCatalog.Business;
using HyperComponents.Data.dbAccess;

namespace HyperCatalog.UI.Main.AjaxControls
{
  /// <summary>
  /// Description résumée de LookupContainer.
  /// </summary>
  [WebService(Namespace = "http://www.hypercatalog.fr/webservices/",
     Description = "")]

  public class AppComponents : WebService//, IPopupService
  {
    public AppComponents()
    {
      //CODEGEN : Cet appel est requis par le Concepteur des services Web ASP.NET
      InitializeComponent();
    }

    #region Code généré par le Concepteur de composants
		
    //Requis par le Concepteur des services Web 
    private IContainer components = null;
				
    /// <summary>
    /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
    /// le contenu de cette méthode avec l'éditeur de code.
    /// </summary>
    private void InitializeComponent()
    {
    }

    /// <summary>
    /// Nettoyage des ressources utilisées.
    /// </summary>
    protected override void Dispose( bool disposing )
    {
      if(disposing && components != null)
      {
        components.Dispose();
      }
      base.Dispose(disposing);		
    }
		
    #endregion

    /// <summary>
    /// </summary>
		[WebMethod(Description="Return Child nodes in XML Format")]      
		public string Check(int appComponentId)
		{
      string result = "";
      Business.ApplicationComponent component = Business.ApplicationSettings.Components[appComponentId];
      if (component != null)
      {
        switch (component.Type)
        {
          case HyperCatalog.Business.ApplicationComponentTypes.Batch:
            //if (component.Action == null)
            //  result += "<LI style=\"color:orange;\">Action is not set</LI>";
            //else
              result += "<LI style=\"color:green;\">OK</LI>";
            break;
          case HyperCatalog.Business.ApplicationComponentTypes.Database:
            if (component.HardwareComponentId <= 0 && component.URI == null)
              result += "<LI style=\"color:orange;\">Hardware is not set</LI>";
            else
            {

              HyperComponents.Data.dbAccess.Database dbObj = new HyperComponents.Data.dbAccess.Database(component.ConnectionString);
              DataSet ds = dbObj.RunSQLReturnDataSet("SELECT TOP 1 VersionNumber FROM DBVersion ORDER BY VersionDate DESC");
              if (dbObj.LastError != "")
                result += "<LI style=\"color:red;\">" + dbObj.LastError + "</LI>";
              else if (ds==null || ds.Tables.Count!=1 || ds.Tables[0].Rows.Count != 1)
                result += "<LI style=\"color:orange;\">OK but datatable version was not found</LI>";
              else
                result += "<LI style=\"color:green;\">OK (version " + ds.Tables[0].Rows[0].ItemArray[0] + ")</LI>";
            }
            break;
          case HyperCatalog.Business.ApplicationComponentTypes.Hardware:
            if (component.URI == null)
              result += "<LI style=\"color:red;\">IP is not set</LI>";
            else
              result += "<LI style=\"color:green;\">OK</LI>";
            break;
          case HyperCatalog.Business.ApplicationComponentTypes.WebService:
            if (component.URI == null)
              result += "<LI style=\"color:red;\">URL is not set</LI>";
            else
            {
              try
              {
                XmlDocument wsXml = new XmlDocument();
                wsXml.Load(System.Net.WebRequest.Create(component.URI + "?WSDL").GetResponse().GetResponseStream());
                int methodCount = (int)(HyperComponents.Xml.Xml.SelectNodes(wsXml, "//wsdl:operation", "wsdl").Count / 3);
                if (methodCount > 0)
                  result += "<LI style=\"color:green;\">OK: <a target=\"_blank\" href=\"" + component.URI + "\">" + component.Name + "</a> reached (" + methodCount + " method" + (methodCount>1?"s":"") + " found)</BR>";
                else
                  result += "<LI style=\"color:orange;\"><a target=\"_blank\" href=\"" + component.URI + "\">" + component.Name + "</a> reached but no method found inside</BR>";
              }
              catch (Exception exc)
              {
                result += "<LI style=\"color:red;\"><a target=\"_blank\" href=\"" + component.URI + "\">" + component.Name + "</a> not reachable</LI>";
              }
            }
            break;
          case HyperCatalog.Business.ApplicationComponentTypes.WebUI:
            if (component.URI == null)
              result += "<LI style=\"color:red;\">URL is not set</LI>";
            else
            {
              try
              {
                XmlDocument wsXml = new XmlDocument();
                Stream resStream = System.Net.WebRequest.Create(component.URI).GetResponse().GetResponseStream();
                try
                {
                  wsXml.Load(resStream);
                  result += "<LI style=\"color:green;\">OK: <a target=\"_blank\" href=\"" + component.URI + "\">" + component.Name + "</a> reached</BR>";
                }
                catch
                {
                  if (resStream.ReadByte()>0)
                    result += "<LI style=\"color:green;\">OK: <a target=\"_blank\" href=\"" + component.URI + "\">" + component.Name + "</a> reached</BR>";
                  else
                    result += "<LI style=\"color:red;\"><a target=\"_blank\" href=\"" + component.URI + "\">" + component.Name + "</a> not reachable</BR>";
                }
              }
              catch (Exception exc)
              {
                result += "<LI style=\"color:red;\"><a target=\"_blank\" href=\"" + component.URI + "\">" + component.Name + "</a> not reachable</LI>";
              }
            }
          break;
        }
      } 
      return result;
    }
  }
}
