 using System;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Security.Permissions;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Data;
using HyperComponents.Data.dbAccess;
using HyperCatalog.Business;
  /// <summary>
  /// Description résumée de Class1.
  /// </summary>
  public class HOT
  {
    private static string FResourceRootURI = string.Empty;
    private static XmlNode addAttribute(ref XmlNode node, string attributeName, string nodeValue)
    {
      XmlNode newAttribute, cAttribute;
      cAttribute = node.Attributes.GetNamedItem(attributeName);
      if (cAttribute == null)
      {
        newAttribute = node.OwnerDocument.CreateAttribute(attributeName);
        newAttribute.InnerText = nodeValue;
        node.Attributes.SetNamedItem(newAttribute);
        return newAttribute;
      }
      else
      {
        cAttribute.Value = nodeValue;
        return cAttribute;
      }
    }

    /// <summary>
    /// This method had a single chunk to the current product
    /// </summary>
    /// <param name="rs">Current DataReader that contains the data from the DB</param>
    /// <param name="parentNode">"product/chunks" node that will contain the new chunk</param>
    /// <history>
    /// 	[Philippe] 	21/08/2003	Created
    /// </history>
    ///-----------------------------------------------------------------------------
    private static void addChunk(IDataReader rs, XmlNode parentNode)
    {
      XmlNode chunkNode;
      //string color;
      chunkNode = parentNode.OwnerDocument.CreateElement("chunk");
      addAttribute(ref chunkNode, "id", rs["ContainerID"].ToString());
      addAttribute(ref chunkNode, "xmlname", rs["Tag"].ToString());
      addAttribute(ref chunkNode, "status", rs["ChunkStatus"].ToString());


      // we do not proceed when culturecode does not contain "-" since it may refer to DAM URI.
      if (rs["CultureCode"].ToString().IndexOf("-") != -1)
      {
        string languageCode = rs["CultureCode"].ToString().Split('-')[1];
        if (languageCode != string.Empty)
        {
          addAttribute(ref chunkNode, "language", languageCode);
        }
      }
        chunkNode.InnerText = rs["ChunkValue"].ToString();
      parentNode.AppendChild(chunkNode);
    }  

    private static void addLinkChunk(IDataReader rs, XmlNode parentNode, string tag, string fieldName)
    {
      XmlNode chunkNode;
      //string color;
      if ((rs[fieldName] != DBNull.Value) && (rs[fieldName].ToString().Trim() != String.Empty))
      {
        chunkNode = parentNode.OwnerDocument.CreateElement("chunk");
        addAttribute(ref chunkNode, "xmlname", tag);
        // A fallback (for links only)
        try
        {
          addAttribute(ref chunkNode, "status", rs[("Status" + fieldName)].ToString());
        }
        catch { }
        // A fallback (for links only)
        try
        {
          addAttribute(ref chunkNode, "language", rs[("LanguageCode" + fieldName)].ToString());
        }
        catch { }

        chunkNode.InnerText = rs[fieldName].ToString();
        parentNode.AppendChild(chunkNode);
      }
    }

    //private static void addChunk(IDataReader rs,  XmlNode parentNode, string tag, string fieldName)
    //{
    //  XmlNode chunkNode;
    //  //string color;
    //  chunkNode = parentNode.OwnerDocument.CreateElement("chunk");
    //  if (rs["ContainerID"] != DBNull.Value)
    //  addAttribute(ref chunkNode, "id", rs["ContainerID"].ToString());
    //  if (tag != "Tag")
    //  {
    //    addAttribute(ref chunkNode, "xmlname", tag);
    //  }
    //  else
    //  {
    //    addAttribute(ref chunkNode, "xmlname", rs["Tag"].ToString());
    //  }
    //  if (rs["ChunkStatus"] != DBNull.Value)
    //  {
    //    addAttribute(ref chunkNode, "status", rs["ChunkStatus"].ToString());
    //  }
    //  else
    //  {
    //    // A fallback (for links only)
    //    addAttribute(ref chunkNode, "status", rs[("Status"+fieldName)].ToString());
      	
    //  }
      
    //  if (rs["CultureCode"] != DBNull.Value)
    //  {
    //      // we do not proceed when culturecode does not contain "-" since it may refer to DAM URI.
    //    if (rs["CultureCode"].ToString().IndexOf("-") != -1)
    //    {
    //      string languageCode = rs["CultureCode"].ToString().Split('-')[1];
    //      if (languageCode != string.Empty)
    //      {
    //          addAttribute(ref chunkNode, "language", languageCode);
    //      }
    //    }  
    //  }
    //  else
    //  {
    //    // A fallback (for links only)
    //    if (rs["LanguageCode" + fieldName] != DBNull.Value)
    //    addAttribute(ref chunkNode, "language", rs[("LanguageCode"+fieldName)].ToString());      	
    //  }  
    //  if (rs["CultureCode"].ToString() == "1")
    //  {
    //    // If the Container contains a resource than add the DAM URI
    //    chunkNode.InnerText = FResourceRootURI + rs[fieldName].ToString();
    //  }
    //  else
    //  {
    //    chunkNode.InnerText = rs[fieldName].ToString();
    //  }
    //  parentNode.AppendChild(chunkNode);
    //}

    /// <summary>
    /// This method had all the chunks to the current product
    /// </summary>
    /// <param name="rs">Current DataReader that contains the data from the DB</param>
    /// <param name="parentNode">"product" node that will contain the new chunks</param>
    /// <history>
    /// 	[Philippe] 	21/08/2003	Created
    /// </history>
    ///-----------------------------------------------------------------------------
    private static bool addChunks(ref IDataReader rs, XmlNode parentNode, int prodID)
    {
      XmlNode chunksNode;
      bool notEOF = true;
      chunksNode = parentNode.OwnerDocument.CreateElement("chunks");
      while (notEOF)
      {
        if (Convert.ToInt32(rs["ItemId"]) == prodID)
        {
          addChunk(rs, chunksNode);
          notEOF = rs.Read();
        }
        else
        {
          break;
        }
      }
      parentNode.AppendChild(chunksNode);
      return notEOF;
    }

    /// <summary>
    /// This method had all the void product items to the current parentNode
    /// This parentNode can be either "content" or "product"
    /// </summary>
    /// <param name="rs">Current DataReader that contains the data from the DB</param>
    /// <param name="parentNode">"product" or "content" node that will contain the current product</param>
    /// <history>
    /// 	[Philippe] 	21/08/2003	Created
    /// </history>
    ///-----------------------------------------------------------------------------
    private static bool AddProduct(IDataReader rs, ref XmlDocument xmlDoc, XmlNode parentNode)
    {
      XmlNode productNode;
      if (parentNode == null)
      {
        xmlDoc.AppendChild(xmlDoc.CreateElement("product"));
        productNode = xmlDoc.DocumentElement;
      }
      else
      {
        productNode = parentNode.OwnerDocument.CreateElement("product");
      }
      int levelId = Convert.ToInt32(rs["LevelId"]);
      int pid = Convert.ToInt32(rs["itemId"]);
      bool notEOF = addChunks(ref rs, productNode, pid);
      addAttribute(ref productNode, "productid", pid.ToString());
      addAttribute(ref productNode, "levelid", levelId.ToString());
      if (parentNode != null) parentNode.AppendChild(productNode);
      while (notEOF)
      {
        if (Convert.ToInt32(rs["LevelId"]) > levelId)
        {
          notEOF = AddProduct(rs, ref xmlDoc, productNode);
        }
        else
        {
          if (Convert.ToInt32(rs["LevelId"]) == levelId)
          {
            notEOF = AddProduct(rs, ref xmlDoc, parentNode);
          }
          else
              return notEOF;
        }
      }
      return rs.Read();
    }

    private static void AddLinks(IDataReader rs, ref XmlDocument xmlDoc)
    {
      int previouspid = -1;
      int previouscompatid = -1;
      XmlNode productNode = null;
      XmlNode compatNode = null;
      XmlNode compatProductNode = null;
      XmlNode compatChunksNode = null;
      while (rs.Read())
      {
        int pid = Convert.ToInt32(rs["ProductID"]);
        int compatid = Convert.ToInt32(rs["CompanionID"]);
        // new product
        if (previouspid != pid)
        {
          productNode = xmlDoc.SelectSingleNode("//product[@productid='" + pid.ToString() + "']");
          previouscompatid = -1;
        }
        if (productNode != null)
        {
          //
          compatNode = productNode.SelectSingleNode("./compatibilities/byproducts");
          if (compatNode == null)
          {
            XmlNode newNode = xmlDoc.CreateElement("compatibilities");
            productNode.AppendChild(newNode);
            compatNode = xmlDoc.CreateElement("byproducts");
            newNode.AppendChild(compatNode);
          }
          // new compatible product
          if (compatid != previouscompatid)
          {
            compatProductNode = xmlDoc.CreateElement("product");
            compatNode.AppendChild(compatProductNode);
            addAttribute(ref compatProductNode, "productid", compatid.ToString());
            if (rs["LinkTypeId"] != DBNull.Value)
              addAttribute(ref compatProductNode, "linktype", rs["LinkTypeId"].ToString());
            if (rs["CompanionCategoryID"] != DBNull.Value)
              addAttribute(ref compatProductNode, "groupid", rs["CompanionCategoryID"].ToString());
            if (rs["CompanionCategory"] != DBNull.Value)
              addAttribute(ref compatProductNode, "groupname", rs["CompanionCategory"].ToString());
            if (rs["SmallSerieID"] != DBNull.Value)
              addAttribute(ref compatProductNode, "smallserieid", rs["SmallSerieID"].ToString());
            if (rs["SmallSerieName"] != DBNull.Value)
              addAttribute(ref compatProductNode, "smallseriename", rs["SmallSerieName"].ToString());
            if (rs["BigSerieID"] != DBNull.Value)
              addAttribute(ref compatProductNode, "bigserieid", rs["BigSerieID"].ToString());
            if (rs["BigSerieName"] != DBNull.Value)
              addAttribute(ref compatProductNode, "bigseriename", rs["BigSerieName"].ToString());
            if (rs["ProdType"] != DBNull.Value)
              addAttribute(ref compatProductNode, "classname", rs["ProdType"].ToString());
            if (rs["ProdTypeId"] != DBNull.Value)
              addAttribute(ref compatProductNode, "classid", rs["ProdTypeId"].ToString());
            if (rs["status"] != DBNull.Value)
              addAttribute(ref compatProductNode, "status", rs["status"].ToString());
            if (rs["ShowInOutput"] != DBNull.Value)
              addAttribute(ref compatProductNode, "showinoutput", rs["ShowInOutput"].ToString());
            compatChunksNode = xmlDoc.CreateElement("chunks");
            compatProductNode.AppendChild(compatChunksNode);
          }
          addLinkChunk(rs, compatChunksNode, "prodnum", "CompanionNumber");
          addLinkChunk(rs, compatChunksNode, "prodname", "CompanionName");
          addLinkChunk(rs, compatChunksNode, "ltdproddes_medium", "CompanionDes");
          addLinkChunk(rs, compatChunksNode, "photods1", "CompanionPhoto");
          addLinkChunk(rs, compatChunksNode, "ltdproddesnotebasic", "CompanionFootnote");
          addLinkChunk(rs, compatChunksNode, "ltdproddes_long", "CompanionLongDes");
          addLinkChunk(rs, compatChunksNode, "ltdproddesnotecomp", "CompanionLongFootnote");
          addLinkChunk(rs, compatChunksNode, "infopointercountry", "CompanionInfopointercountry");
          addLinkChunk(rs, compatChunksNode, "pageyielda4bw", "CompanionAdditionalChunk1");
          addLinkChunk(rs, compatChunksNode, "pageyielda4clr", "CompanionAdditionalChunk2");
          addLinkChunk(rs, compatChunksNode, "ksp_01_headline_medium", "KSPChunkvalue1");
          addLinkChunk(rs, compatChunksNode, "ksp_02_headline_medium", "KSPChunkvalue2");
          addLinkChunk(rs, compatChunksNode, "ksp_03_headline_medium", "KSPChunkvalue3");
          addLinkChunk(rs, compatChunksNode, "proddes_overview_medium", "CompanionDes");

        }
        previouspid = pid;
        previouscompatid = compatid;
      }
    }


    /// <summary>
    /// Prepare content (from DB) function
    /// </summary>
    /// <param name="ProductId">Current Product ID</param>
    /// <param name="cultureCode">cultureCode code (5 chars) for which the content must be extracted</param>
    /// <param name="IncludeLinks">Indicates if (the links must be retrieve from the DB</param>
    /// <param name="IncludeLinksContent">Indicates if (the content relative to linked products
    /// must also be extracted. It make sense only if ("IncludeLinks" param is set to true</param>
    /// <param name="FinalChunksOnly">Retrieve only chunks with status="F" (final)</param>
    /// <param name="KeepMaster">if (the cultureCode is not "ww-en" (master), indicates if (the function must keep the original English content</param>
    /// <returns>PrepareContent XML unicode string</returns>
    /// <remarks>
    /// <para>
    /// All chunks are contained in XMLProducts big tree.
    /// Objective is to reduce amount of content in order to accelerate transformation steps
    /// </para>
    ///</remarks>
    /// <example>
    /// <para>
    /// prepareContentObject = new HyperCatalog.HOT.PrepareContentFromDB("yourConnectionstring")
    /// </para>
    /// <para>
    /// richTextBox1 = prepareContentObject.PrepareContent(91, "ww-en", true, true, False, False)
    /// </para>
    /// </example>
    /// <history>
    /// 	[Philippe] 	21/08/2003	Created
    /// </history>
    ///-----------------------------------------------------------------------------    
    public static string PrepareContent(string dbConnstring, string itemId, string cultureCode, bool includeLinks, bool includeLinksContent, bool FinalChunksOnly, bool KeepMaster)
    {
      //Database objDB = new Database(dbConnstring);
      //FIX FOR 71368
      Database objDB = new Database(dbConnstring, Convert.ToInt32(HyperCatalog.Business.ApplicationSettings.Parameters["Publication_TimeOut"].Value.ToString()));

      return DoPrepareContent(objDB, itemId, cultureCode, includeLinks, includeLinksContent, FinalChunksOnly, KeepMaster);
    }
    public static string PrepareContent(string itemId, string cultureCode, bool includeLinks, bool includeLinksContent, bool FinalChunksOnly, bool KeepMaster)
    {
      //Database objDB = new Database(HyperCatalog.Business.ApplicationSettings.Components["Crystal_DB"].ConnectionString);
      //FIX FOR 71368
      Database objDB = new Database(HyperCatalog.Business.ApplicationSettings.Components["Crystal_DB"].ConnectionString, Convert.ToInt32(HyperCatalog.Business.ApplicationSettings.Parameters["Publication_TimeOut"].Value.ToString()));
      return DoPrepareContent(objDB, itemId, cultureCode, includeLinks, includeLinksContent, FinalChunksOnly, KeepMaster);
    }
    private static string DoPrepareContent(Database objDB, string itemId, string cultureCode, bool includeLinks, bool includeLinksContent, bool FinalChunksOnly, bool KeepMaster)
    {
      XmlDocument xmlDoc;

      // Retreive the URI to the DAM webservice
      FResourceRootURI = ApplicationSettings.Components["DAM_Provider"].URI;

      try
      {
        xmlDoc = new XmlDocument();
        xmlDoc.AppendChild(xmlDoc.CreateProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\""));
        using (IDataReader rsContent = objDB.RunSPReturnRS("HOT_PrepareContent",
          new SqlParameter("@ItemId", itemId),
          new SqlParameter("@Mode", "C"),
          new SqlParameter("@CultureCode", cultureCode),
          new SqlParameter("@FinalChunksOnly", FinalChunksOnly),
          new SqlParameter("@KeepMaster", KeepMaster)))
        {

          if (rsContent.Read())
          {
            AddProduct(rsContent, ref xmlDoc, null);
          }
          rsContent.Close();

          {
            using (IDataReader rsLinks = objDB.RunSPReturnRS("HOT_PrepareContent",
                        new SqlParameter("@ItemId", itemId),
                        new SqlParameter("@Mode", "L"),
                        new SqlParameter("@CultureCode", cultureCode),
                        new SqlParameter("@FinalChunksOnly", FinalChunksOnly),
                        new SqlParameter("@KeepMaster", KeepMaster),
                        new SqlParameter("@RetrieveLinksContent", includeLinksContent)))
            {
                AddLinks(rsLinks, ref xmlDoc);
                rsLinks.Close();
            }
          }
          return xmlDoc.InnerXml;
        }
      }
      catch (Exception ex)
      {
        return "<product><chunks><chunk tag=\"ProdName\">AN ERROR OCCURED: \n" + ex.ToString() + " " + objDB.LastError + "</chunk></chunks></product>";
      }
    }
   
    public byte[] GetProductContent(string productId, string cultureCode)
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(PrepareContent(productId, cultureCode, false, false, true, true));
            return Streaming.StreamToByte(Streaming.GetDocumentStream(xmlDoc));
        }
        catch (Exception exc)
        {
            return Streaming.EmptyByte;
        }
    }
    public string GetProductContent2(string productId, string cultureCode)
    {
        try
        {
            return PrepareContent(productId, cultureCode, false, false, true, true);
        }
        catch (Exception exc)
        {
            return exc.ToString();
        }
    }
  }
