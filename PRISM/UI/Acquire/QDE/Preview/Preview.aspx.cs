#region uses
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HyperCatalog.Shared;
using HyperCatalog.Business;
using System.Xml;
using System.IO;
using HyperCatalog.WebServices.PublicationManagerWS;
using HyperComponents.Data.dbAccess;


using System.Collections.Generic;
using System.Text;
using System.Security.Permissions;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using HyperComponents.Data.dbAccess;
using HyperCatalog.WebServices;
using System.Web;
using System.Configuration;

#endregion

	/// <summary>
	/// Description résumée de Preview.
	/// </summary>
public partial class Preview : HCPage
{
  private HyperCatalog.Business.Item item;
  private HyperCatalog.Business.Item itemSoftroll;
  System.Collections.Specialized.StringCollection template;
  private ParamSet[] colorParams
  {
    get
    {
      return (ParamSet[])Session["colorParams"];
    }
    set
    {
      Session["colorParams"] = value;
    }
  }
  //Modified the code to add preview functionality for Softroll by Radha S - Start
  protected void Page_Load(object sender, System.EventArgs e)
  {
    if (!Page.IsPostBack)
    {
      try
      {
          if (Request["preview"].ToString() == "original")
          {
              item = QDEUtils.GetItemIdFromRequest();
              long itemId = item.Id;
              template = item.Templates;
              funLoadContent();
          }
          if (Request["preview"].ToString() == "softroll")
          {
              item = QDEUtils.GetItemIdFromRequest();
              Database dbObj = new Database(HyperCatalog.Business.ApplicationSettings.Components["Crystal_DB"].ConnectionString);
              DataSet ds = new DataSet();
              ds = dbObj.RunSQLReturnDataSet("select NodeOID,ItemId,IsRoll from Items where NodeOID in(select NodeOID  from Items where ItemId =" + item.Id + ")");
              if (ds.Tables[0].Rows.Count > 1)
              {
                  DataRow[] dr1 = ds.Tables[0].Select("IsRoll = true");
                  for (int drVal = 0; drVal < dr1.Length; drVal++)
                  {
                      long itemId = Convert.ToInt64(dr1[drVal]["ItemId"].ToString());
                      item = QDEUtils.GetItemIdFromRequest(itemId);
                  }
                  itemSoftroll = QDEUtils.GetItemIdFromRequest();
                  if (item.Templates.Count >= 0)
                  {
                      template = itemSoftroll.Templates;
                  }
                  funLoadContent();
              }
              else
              {
                  ClientScript.RegisterClientScriptBlock(GetType(), "Preview", "<script> alert('This Item is not a Softroll Item, Preview Not Available'); window.close();</script>", false);
                  tblPreview.Visible = false;
                  pnlMsg.Visible = false;

              }
          }
          //Modified the code to add preview functionality for Softroll by Radha S - End
      }
      catch
      {
        UITools.DenyAccess(DenyMode.Popup);
      }
    }
  }

  // Added the below function by Radha S
  protected void funLoadContent()
  {

      if (SessionState.Culture.CountryCode != string.Empty)
      {
          uwToolbar.Items.FromKeyLabel("Culture").Image = "/hc_v4/img/flags/" + SessionState.Culture.CountryCode + ".gif";
      }
      uwToolbar.Items.FromKeyLabel("Culture").Text = SessionState.Culture.Name;

      if (item.FullName.Length > 50)
      {
          uwToolbar.Items.FromKeyLabel("Item").Text = item.FullName.Substring(0, 50) + "...";
      }
      else
      {
          uwToolbar.Items.FromKeyLabel("Item").Text = item.FullName;
      }
      UpdateDataView();
  }

  protected void UpdateDataView()
  {
    try
    {
      dlTemplates.Items.Clear();
      if (template != null || template.Count >= 0)
      {
          foreach (string t in template)
          {
              dlTemplates.Items.Add(new ListItem(t, t));
          }
      }
      CultureList culs = SessionState.User.Cultures;
      CollectionView vCuls = new CollectionView(culs);
      if (SessionState.Culture.Code != HyperCatalog.Shared.SessionState.MasterCulture.Code)
      {
        vCuls.ApplyFilter("FallbackCode", SessionState.Culture.Code, CollectionView.FilterOperand.StartsWith);
        dlLocalizations.Items.Add(new ListItem(SessionState.Culture.Name, SessionState.Culture.Code));
      }
      foreach (Culture cul in vCuls)
      {
        string prefix = "";
        for (int i = 0; i < (int)cul.Type; i++)
          prefix += "_";
        dlLocalizations.Items.Add(new ListItem(prefix + cul.Name,cul.Code));
      }
      if (SessionState.Culture != null)
      {
        dlLocalizations.SelectedValue = SessionState.Culture.Code;
      }
      else
      {
        dlLocalizations.SelectedValue = HyperCatalog.Shared.SessionState.MasterCulture.Code;
      }

      Publication pubWS = HyperCatalog.WebServices.WSInterface.Publication;
      colorParams = pubWS.GetParamSet("HP Colors");
      dlColors.Items.Add(new ListItem("Default","default"));
      dlColors.Items.Add(new ListItem("Auto","auto"));

foreach (ParamSet param in colorParams)
        dlColors.Items.Add(new ListItem(param.Name,param.Id.ToString()));

        try
        {
            //FIX FOR 70901
            int paramVal = 0;
            //paramVal = pubWS.GetHotItemParam(item.Id.ToString(), dlTemplates.Text.ToString());
            paramVal = GetHotItemParam(item.Id.ToString(), dlTemplates.Text.ToString());
            //FIX FOR 70901

            if (paramVal > 0)
            {
                if (paramVal < dlColors.Items.Count)
                    dlColors.SelectedValue = dlColors.Items[paramVal + 1].Value;
            }
        }
        catch (Exception ex)
        { }

    }
    catch (Exception e)
    {
      Response.Write(e.Message);
    }
}

 #region Commented
//New method for 70901
    public int GetHotItemParam(string ItemId, string TemplateName)
    {
        int ParamVal = 0;
        Database dbObj = new Database(HyperCatalog.Business.ApplicationSettings.Components["PublicationManager_DB"].ConnectionString);
        DataSet ds = dbObj.RunSQLReturnDataSet(String.Format("SELECT ParamSetValueId FROM HOT_ItemParams WHERE ItemId = '{0}' and TemplateName = '{1}'", ItemId, TemplateName));
        if (ds != null)
            if (ds.Tables.Count > 0)
                if (ds.Tables[0].Rows.Count > 0)
                    ParamVal = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
        return ParamVal;
    }
//New method for 70901
#endregion


  #region stream
  public static void Reset(ref Stream stream)
  {
    if (stream != null)
    {
      stream.Flush();
      stream.Position = 0;
    }
  }
  public XmlDocument OpenXmlDocument(Stream stream)
  {
    XmlDocument xmlDoc = new XmlDocument();
    Exception exc = null;
    try
    {
      Reset(ref stream);
      if (stream.Length>0)
        xmlDoc.Load(stream);
    }
    catch (Exception x)
    {
      exc = x;
    }
    stream.Close();
    if (exc==null)
      return xmlDoc;
    else
      throw exc;
  }
  public Stream ByteToStream(byte[] buffer)
  {
    MemoryStream stream = new MemoryStream();
    stream.Write(buffer,0,buffer.Length);
    return stream;
  }
  public XmlDocument OpenXmlDocument(byte[] buffer)
  {
    Stream stream = ByteToStream(buffer);
    return OpenXmlDocument(stream);
  }
  #endregion

  #region BuildPDFDoc
  private void BuildDocument(string itemIdStr, string cultureCode, string templateName, string preview)
  {
    try
    {
        //Modified the code to add preview functionality for Softroll by Radha S - Start
        if (preview == "original")
        {
            item = QDEUtils.GetItemIdFromRequest();
        }
        if (preview == "softroll")
        {
            item = QDEUtils.GetItemIdFromRequest();
            Database dbObj = new Database(HyperCatalog.Business.ApplicationSettings.Components["Crystal_DB"].ConnectionString);
            DataSet ds = new DataSet();
            ds = dbObj.RunSQLReturnDataSet("select NodeOID,ItemId,IsRoll from Items where NodeOID in(select NodeOID  from Items where ItemId =" + item.Id + ")");
            if (ds.Tables[0].Rows.Count > 1)
            {
                DataRow[] dr1 = ds.Tables[0].Select("IsRoll = true");
                for (int drVal = 0; drVal < dr1.Length; drVal++)
                {
                    long itemId = Convert.ToInt64(dr1[drVal]["ItemId"].ToString());
                    item = QDEUtils.GetItemIdFromRequest(itemId);
                }
                itemIdStr = Convert.ToString(item.Id);
            }
        }
        //Modified the code to add preview functionality for Softroll by Radha S - End
      HOT objCont = new HOT();

       //XmlDocument xmlProductItem = OpenXmlDocument(WSContentProvider.GetProductContent(itemIdStr, cultureCode));
      
//      XmlDocument xmlProductItem = OpenXmlDocument(GetProductContent(itemIdStr, cultureCode));
      XmlDocument xmlProductItem = OpenXmlDocument(objCont.GetProductContent(itemIdStr, cultureCode));
        
      if (xmlProductItem!=null)
      {
        try
        {
          foreach (XmlNode chunkNode in xmlProductItem.SelectNodes("//chunk"))
            chunkNode.Attributes["language"].Value = chunkNode.Attributes["language"].Value.Substring(3, 2);
        }
        catch { }
        #region collaterals
		if (xmlProductItem.SelectSingleNode("//chunk[@xmlname='IsCollateral']") != null)    //CR 4506
		{
        XmlNode proidlistNode = xmlProductItem.SelectSingleNode("//chunk[@xmlname='w_prodidlist']");
        if (proidlistNode != null)
        {
          XmlNode productNode = xmlProductItem.SelectSingleNode("product");
          foreach (string prodid in proidlistNode.InnerText.Split(';'))
            if (prodid != String.Empty)
            {
              //XmlDocument xmlLocalDoc = OpenXmlDocument(WSContentProvider.GetProductContent(prodid, cultureCode));
              //XmlDocument xmlLocalDoc = OpenXmlDocument(GetProductContent(prodid, cultureCode));
                XmlDocument xmlLocalDoc = OpenXmlDocument(objCont.GetProductContent(prodid, cultureCode));
                XmlNode newNode = xmlLocalDoc.SelectSingleNode("product");
              productNode.AppendChild(xmlProductItem.ImportNode(newNode, true));
            }
        }
		}
        #endregion

        #region Handle publication number in preview mode
        // Remove publication number if mode is preview
        if (dlResolutions.SelectedValue.ToLower() == "preview")
        {
          foreach (XmlNode node in xmlProductItem.SelectNodes(".//chunk[@xmlname='dspubn']"))
          {
            node.InnerText = "PREVIEW";
          }
          foreach (XmlNode node in xmlProductItem.SelectNodes(".//chunk[@xmlname='icdisplaypubn']"))
          {
            node.InnerText = "PREVIEW";
          }
          foreach (XmlNode node in xmlProductItem.SelectNodes(".//chunk[@xmlname='icprodpubn']"))
          {
            node.InnerText = "PREVIEW";
          }
          foreach (XmlNode node in xmlProductItem.SelectNodes(".//chunk[@xmlname='icpricepubn']"))
          {
            node.InnerText = "PREVIEW";
          }
        }
        #endregion

        long itemId = Convert.ToInt64(itemIdStr);
        string sessionId = string.Empty;//docFactory.SignOn(
        string feedBack = string.Empty;
      
        byte[] templateArray = HyperComponents.IO.Streaming.EmptyByte;
				templateArray = WSDam.ResourceBinaryGetByPath((string)SessionState.CacheParams["PubManager_DAMTemplateLibrary"].Value + "/" + templateName + ".hot");
        string parameters = string.Empty;

        switch (dlColors.SelectedValue)
        {
          case "auto":
            int index =Convert.ToInt32(itemId % colorParams.Length);
            dlColors.SelectedValue = dlColors.Items[index+1].Value;
            parameters = getXmlParams(itemId,templateName);
            break;
          case "default":
            break;
          default:
            parameters = getXmlParams(itemId,templateName);
            break;
        }
        if (templateArray != null && templateArray.Length > 0)
        {
            DateTime before = DateTime.Now;
            //          Response.Write("<B>An error occurred during creation of document for this product.</B><BR/>"
            //            + xmlProductItem.OuterXml.Length + "<BR/><BR/>"
            //            + templateArray.Length + "<BR/><BR/>"
            //            + cultureCode + "<BR/><BR/>"
            //            + parameters + "<BR/><BR/>"
            //            + dlResolutions.SelectedValue + "<BR/><BR/>"
            //            );
            //docFactory.Url = "http://83.145.97.142/HyperCatalog/HyperCatalog.WebServices/DocumentFactory/DocumentFactory.asmx";
            //WSDocFactory.Url = "http://localhost/DocFactory/DocumentFactory.asmx";
            //WSDocFactory.Credentials = System.Net.CredentialCache.DefaultCredentials;
            
            string languageCode = HyperCatalog.Business.Culture.GetByKey(cultureCode).LanguageCode;
            byte[] outputArray = WSDocFactory.CreateDocumentStreamFromBinary(sessionId, xmlProductItem.OuterXml, templateArray, "", languageCode, parameters, dlResolutions.SelectedValue, "", ref feedBack);
            if (outputArray != null && outputArray.Length > 0)
            {
                DateTime after = DateTime.Now;

                //          HyperCatalog.UI.Main.PublicationWS.Publication pubWS = new HyperCatalog.UI.Main.PublicationWS.Publication();
                //          pubWS.LogTemplateGeneration(SessionState.User.Id,itemId,templateName,dlResolutions.SelectedValue,cultureCode,((TimeSpan)after.Subtract(before)).Seconds);
                //          pubWS.Dispose();

                //Session["pdf"] = outputArray;
                Response.Expires = 0;
                Response.Buffer = true;
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("content-disposition", "attachment;filename=preview.pdf");
                Response.AddHeader("content-length", outputArray.Length.ToString());
                Response.BinaryWrite(outputArray);
                /*    /*FileStream fs = new FileStream(@"c:\projets\test.pdf", FileMode.CreateNew);
                        fs.Write(outputArray, 0, outputArray.Length);
                        fs.Close();*/
            }
            else
            {
                Response.Write("<B>" + item.FullName + "</B> - Content not available");
                Response.Flush();
                Response.End();
            }
        }
        else
        {
            Response.Write("<B>" + item.FullName + "</B> - Unable to retrieve Template from DAM");
            Response.Flush();
            Response.End();
        }

      }
    }
    catch (Exception e)
    {
        //Removed the system exceptiong which was displaying in the UI by Radha S
        Response.Write("<B>" + item.FullName + " - Unable to retrieve Content</B><BR/>");
        Response.Write("<B>An error occurred during creation of document for this product.</B><BR/>");
        Response.Flush();
        Response.End();
    }
  }
  private string getXmlParams(long itemId, string templateName)
  {
    string parameters = string.Empty;
    ParamSet chosenColor = null;
    foreach (ParamSet color in colorParams)
      if (color.Id.ToString() == dlColors.SelectedValue)
      {
        chosenColor = color;
        parameters = chosenColor.XmlParams;
        break;
      }

    if (parameters!=string.Empty)
    {
      Publication pubWS = HyperCatalog.WebServices.WSInterface.Publication;
      pubWS.SetItemParamSetChoice(itemId,templateName,"HP Colors",chosenColor.Id);
      pubWS.Dispose();
    }

    return parameters;
  }
  #endregion

  protected void BtnGenerate_Click(object sender, System.EventArgs e)
  {
    Response.Clear();
    BuildDocument(Request["i"].ToString(), dlLocalizations.SelectedValue, dlTemplates.SelectedValue, Request["preview"].ToString());
    Response.End();
}

  public static void GetItemParams(string ItemId, string TemplateName, string Thr)
    {
        int ParamVal = 0;
        Database dbObj = new Database(HyperCatalog.Business.ApplicationSettings.Components["PublicationManager_DB"].ConnectionString);
        DataSet ds = dbObj.RunSQLReturnDataSet(String.Format("SELECT ParamSetValueId FROM HOT_ItemParams WHERE ItemId = '{0}' and TemplateName = '{1}'", ItemId, TemplateName));
        if (ds != null)
            ParamVal = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
        //Console.WriteLine("Inside from " + Thr);
        Publication pubWS = HyperCatalog.WebServices.WSInterface.Publication;

        HyperCatalog.WebServices.PublicationManagerWS.ParamSet[] colorParams = pubWS.GetParamSet("HP Colors");
        HyperCatalog.WebServices.PublicationManagerWS.ParamSet chosenColor = null;
        foreach (HyperCatalog.WebServices.PublicationManagerWS.ParamSet color in colorParams)
            if (color.Id == ParamVal)
            {
                chosenColor = color;
                break;
            }
    }
}
