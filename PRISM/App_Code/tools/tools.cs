#region uses
using System;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;
using Infragistics.WebUI.UltraWebGrid;
using Infragistics.WebUI.UltraWebGrid.ExcelExport;
using Infragistics.Documents.Excel;
using HyperComponents.Security.Cryptography;
using HyperCatalog.Shared;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using HyperCatalog.UI.Tools;
using System.Collections;
#endregion

public class Utils
{
  public static bool SendMail(string email, string fullName, string subject, string body, bool urgent)
  {
      try
      {
          string appName = SessionState.CacheParams["AppName"].Value.ToString();
          string supportInfo = SessionState.CacheParams["SupportInfo"].Value.ToString();
          string smtpServer = SessionState.CacheParams["TREmailSmtpServer"].Value.ToString();
          string mailConfidentiality = SessionState.CacheParams["MailConfidentiality"].Value.ToString();
          HyperComponents.Net.SMTP.EmailUtil m = new HyperComponents.Net.SMTP.EmailUtil();

          m.FromEmail = SessionState.CacheParams["UserEmailFrom"].Value.ToString(); ;
          m.SmtpServer = smtpServer;
          m.Urgent = urgent;
          m.Subject = "[" + appName + "] - " + subject;
          m.strWelcome = "Dear " + fullName + ",\n<br><br>\n";
          m.strBody = body;
          m.strSignature = "<br><br>Your " + appName + " system administrator<br>\n";
          m.strSignature += supportInfo.Replace(@"\n", @"<br/>");
          m.strSignature += "<hr/>\n";
          m.strSignature += "<FONT face='arial, helvetica, sans-serif' color=#5a7173 size=1>" + mailConfidentiality.Replace("\n", "<br>") + "</font>";

          bool isSent = m.SendEmail(fullName, email);
          return isSent;
      }
      catch (Exception ex)
      {
          return true;

      }
  }
  public static HyperComponents.Data.dbAccess.Database GetMainDB()
  {
    return new HyperComponents.Data.dbAccess.Database(SessionState.CacheComponents["Crystal_DB"].ConnectionString);
  }

  const string GRID_SPACER_KEY = "__Spacer";
  public static bool InitGridSort(ref UltraWebGrid dg)
  {
    return InitGridSort(ref dg, true);
  }

  public static bool InitGridSort(ref UltraWebGrid dg, bool allowPaging)
  {
    dg.DisplayLayout.AllowColSizingDefault = AllowSizing.Fixed;
    dg.DisplayLayout.AllowColumnMovingDefault = AllowColumnMoving.None;
    dg.DisplayLayout.EnableInternalRowsManagement = true;
    dg.DisplayLayout.AllowSortingDefault = Infragistics.WebUI.UltraWebGrid.AllowSorting.Yes;
    dg.DisplayLayout.HeaderClickActionDefault = HeaderClickAction.SortSingle;
    dg.DisplayLayout.CellClickActionDefault = CellClickAction.RowSelect;
    dg.DisplayLayout.SelectTypeRowDefault=SelectType.Single;
    #region paging
    if (allowPaging)
    {
      dg.PreRender += new System.EventHandler(HCGrid_PreRender);
      dg.DisplayLayout.Pager.AllowPaging = true;
      dg.DisplayLayout.Pager.PageSize = Convert.ToInt32(SessionState.CacheParams["GridPagerSize"].Value);
      dg.DisplayLayout.Pager.PagerAppearance = PagerAppearance.Both;
      dg.DisplayLayout.Pager.StyleMode = PagerStyleMode.ComboBox;
      dg.DisplayLayout.Pager.Pattern = "[default] of [pagecount] [prev] [next]";
    }
    #endregion
    #region Misc Enhancements
    foreach (UltraGridColumn c in dg.Columns)
    {
      c.CellStyle.Wrap = true;
    }
    dg.DisplayLayout.RowHeightDefault = Unit.Empty;
    #endregion
    #region add end spacer column
    if (dg.Columns.FromKey(GRID_SPACER_KEY) == null)
    {
      UltraGridColumn spacer = new UltraGridColumn();
      spacer.HeaderText = string.Empty;
      spacer.Key = GRID_SPACER_KEY;
      spacer.NullText = string.Empty;
      spacer.Width = Unit.Percentage(99);
      spacer.AllowResize = AllowSizing.Fixed;
      spacer.DefaultValue = " ";
      spacer.Header.ClickAction = HeaderClickAction.Select;
      spacer.AllowRowFiltering = false;
      spacer.AllowGroupBy  = AllowGroupBy.No;      
      foreach (UltraGridBand band in dg.Bands)
      {
        band.Columns.Add(spacer);
        band.Columns.FromKey(GRID_SPACER_KEY).Move(band.Columns.Count-1);        
        foreach (UltraGridRow dr in dg.Rows)
        {
          dr.Cells.FromKey(GRID_SPACER_KEY).Text = string.Empty;
        }        
      }
    }
    #endregion
    #region add CTRL+C event handler to copy the whole line
    string handler = dg.DisplayLayout.ClientSideEvents.KeyDownHandler;
    if (handler == null || handler == String.Empty)
    {
      // Add copy/paste functionality
      dg.DisplayLayout.ClientSideEvents.KeyDownHandler = "g_kd";
    }
    #endregion
    return true;
  }
  public static bool EnableIntelligentSort(ref UltraWebGrid dg, int sortColIndex)
  {
    // Call this method and then rely on index to process sort!
    if (dg.Page !=null)
    {
      if (dg.Columns.FromKey("s_a") != null)
      {
        dg.Columns.Remove(dg.Columns.FromKey("s_a"));
      }
      UltraGridColumn sortCol = new UltraGridColumn("s_a", "", ColumnType.NotSet, null);
      dg.Columns.Insert(sortColIndex, sortCol);
      dg.Columns.FromKey("s_a").Width = Unit.Pixel(55);
      string srcScript = "s(\"" + dg.ClientID +"\",";
      foreach (UltraGridRow r in dg.Rows)
      {
        r.Cells[sortColIndex].Text = "<img src='/hc_v4/img/ed_dot.gif' onclick='" + srcScript + "\"dd\");'><img src='/hc_v4/img/ed_down.gif' onclick='" + srcScript + "\"d\");'><img src='/hc_v4/img/ed_up.gif' onclick='" + srcScript + "\"t\");'><img src='/hc_v4/img/ed_upt.gif' onclick='" + srcScript + "\"tt\");'>";
      }
      return true;
    }
    return false;
  }


  public static void CleanNodeText(ref Infragistics.WebUI.UltraWebNavigator.Node n)
  {
    Infragistics.WebUI.UltraWebNavigator.Node parent;
    int k, maxLen;
    string commonText;
    parent = n.Parent;
    while (parent != null)
    {
      string parentText = parent.TargetFrame;
      commonText = string.Empty;
      k = 0;          
      maxLen = parentText.Length;
      if (n.Text.Length<maxLen)
      {
        maxLen = n.Text.Length;
      }
      while ((k<maxLen) && (n.Text.Substring(k, 1).ToLower()==parentText.Substring(k,1).ToLower()))
      {
        commonText = n.Text.Substring(0,k+1);
        k ++;
      }
      while (commonText.Length > 0 && !commonText.EndsWith(" "))
      {
        commonText = commonText.Substring(0, commonText.Length - 1);
      }
      if (commonText != string.Empty)
      {
        k = commonText.Length;
        if (k<maxLen && Utils.IsInteger(n.Text.Substring(k, 1)))
        {
          while (commonText.Substring(k-1,1) != " ")
          {
            commonText = commonText.Substring(0, k-1);
            k = commonText.Length;
          }
        }
        k = commonText.Length-1;
        if (Utils.IsInteger(commonText.Substring(k,1)))
        {
          while (commonText.Substring(k,1) != " ")
          {
            commonText = commonText.Substring(0, k);
            k = commonText.Length-1;
          }
        }
        if ( commonText!=string.Empty && n.Text.Replace(commonText, "").Replace(parentText, "")!=string.Empty)
        {
          n.Text = n.Text.Replace(commonText, "").Replace(parentText, "");
        }
        if (n.Text.EndsWith("]"))
        {
          string s = n.Text;
          k=n.Text.Length;
          while (s!=string.Empty && !s.EndsWith("["))
          {
            k--;
            s = s.Substring(0, s.Length-1);
          }
          if (s!=string.Empty)
          {
            n.Text = n.Text.Substring(k-1,n.Text.Length-k+1) + " " + s.Substring(0, s.Length-1);
            n.Text = n.Text.Trim();
          }
        }
      }
      parent = parent.Parent;
    }   
  }

  public static string ColorStatus(string s)
  {
    string r = s;
    if (s.ToLower() == "o")
    {
      r = "<font color=gray>O</font>";
    }
    if (s.ToLower() == "f")
    {
      r = "<font color=green>F</font>";
    }
    if (s.ToLower() == "e")
    {
      r = "<font color=red>E</font>";
    }
    return r;
  }

  public static string CleanNodeText2(string nodeName, DataRowCollection parents)
  {
    int k, maxLen;
    string commonText;
    for (int i=parents.Count-1;i>=0;i--)
    {
      string parentText = parents[i]["ItemName"].ToString();
      commonText = string.Empty;
      k = 0;          
      maxLen = parentText.Length;
      if (nodeName.Length<maxLen)
      {
        maxLen = nodeName.Length;
      }
      while ((k<maxLen) && (nodeName.Substring(k, 1).ToLower() == parentText.Substring(k,1).ToLower()))
      {
        commonText = nodeName.Substring(0,k+1);
        k ++;
      }
      while (commonText.Length > 0 && !commonText.EndsWith(" "))
      {
        commonText = commonText.Substring(0, commonText.Length - 1);
      }
      if (commonText != string.Empty)
      {
        k = commonText.Length;
        if (k<maxLen && Utils.IsInteger(nodeName.Substring(k, 1)))
        {
          while (commonText.Substring(k-1,1) != " ")
          {
            commonText = commonText.Substring(0, k-1);
            k = commonText.Length;
          }
        }
        k = commonText.Length-1;
        if (Utils.IsInteger(commonText.Substring(k,1)))
        {
          while (commonText.Substring(k,1) != " ")
          {
            commonText = commonText.Substring(0, k);
            k = commonText.Length-1;
          }
        }
        if ( commonText!=string.Empty && nodeName.Replace(commonText, "").Replace(parentText, "")!=string.Empty)
        {
          nodeName = nodeName.Replace(commonText, "").Replace(parentText, "");
        }
        if (nodeName.EndsWith("]"))
        {
          string s = nodeName;
          k=nodeName.Length;
          while (s!=string.Empty && !s.EndsWith("["))
          {
            k--;
            s = s.Substring(0, s.Length-1);
          }
          if (s!=string.Empty)
          {
            nodeName = nodeName.Substring(k-1,nodeName.Length-k+1) + " " + s.Substring(0, s.Length-1);
            nodeName = nodeName.Trim();
          }
        }
      }
    }
    return nodeName;
  }
  public static string NullToEmpty(string val, string defaultResult)
  {
    if (val.Equals(DBNull.Value))
    {
      return defaultResult;
    }
    else
    {
      return val;
    }
  }


  public static string CleanFileName(string s)
  {
    return s.Replace("#", "_").Replace(@"\", "_").Replace("/", "_").Replace(":", "_").Replace("*", "_").Replace(@"""", "_").Replace("<", "_").Replace(">", "_").Replace("|", "_");
  }
  public static string jsReplace(string s)
  {
    string k = s.Trim();
    k = k.Replace("\\", "\\\\");
    return k.Replace("\"", "\"\"");
  }

  public static int GetCount(HyperComponents.Data.dbAccess.Database dbObj, string sqlStr)
  {
    int r = -1;
    using (IDataReader rs = dbObj.RunSQLReturnRS(sqlStr))
    {
      if (rs.Read())
      {
        r = Convert.ToInt32(rs[0].ToString());
      }
      dbObj.CloseConnection();
    }
		return r;
  }

  public static int GetSelectedIndexOf(DataRowCollection wholeRows, string valueToMatch,string rowToCompare) 
  {
    int index = 0;
    foreach (DataRow row in wholeRows) 
    {
      if (valueToMatch == Convert.ToString(row[rowToCompare])) 
      {
        return index;
      }
      ++index;
    }
    return 0;
  }
  public static bool IsDecimal(string theValue)
  {
    try
    {
      Convert.ToDouble(theValue);
      return true;
    } 
    catch 
    {
      return false;
    }
  } //IsDecimal

  #region "PMT Ugly Tools"
  public static DateTime? PMTConvertToDate(string strDate)
  {
    DateTime? d;
    try
    {
      string day, month, year;
      if (strDate.IndexOf("-") > 0)
      {
        string[] strDateSplit = strDate.Split('-');
        day = strDateSplit[0].ToString();
        month = strDateSplit[1].ToString();
        year = strDateSplit[2].ToString();
      }
      else
      {
        year = strDate.Substring(0, 4);
        month = strDate.Substring(4, 2);
        day = strDate.Substring(6, 2);
      }
      switch (month.ToLower())
      {
        case "jan": month = "01"; break;
        case "feb": month = "02"; break;
        case "mar": month = "03"; break;
        case "apr": month = "04"; break;
        case "may": month = "05"; break;
        case "jun": month = "06"; break;
        case "jul": month = "07"; break;
        case "aug": month = "08"; break;
        case "sep": month = "09"; break;
        case "oct": month = "10"; break;
        case "nov": month = "11"; break;
        case "dec": month = "12"; break;
      }
      d = Convert.ToDateTime(day + "-" + month + "-" + year);
    }
    catch (Exception ex)
    {
      d = null;
    }
    return d;
  }
  #endregion

   
  public static bool IsNaturalNumber(String strNumber)
  {
    Regex objNotNaturalPattern=new Regex("[^0-9]");
    Regex objNaturalPattern=new Regex("0*[1-9][0-9]*");
    return  !objNotNaturalPattern.IsMatch(strNumber) &&
      objNaturalPattern.IsMatch(strNumber);
  }  

  // Function to test for Positive Integers with zero inclusive  

  public static bool IsWholeNumber(String strNumber)
  {
    Regex objNotWholePattern=new Regex("[^0-9]");
    return !objNotWholePattern.IsMatch(strNumber);
  }  

  // Function to Test for Integers both Positive & Negative  

  public static bool IsInteger(String strNumber)
  {
    Regex objNotIntPattern=new Regex("[^0-9-]");
    Regex objIntPattern=new Regex("^-[0-9]+$|^[0-9]+$");
    return  !objNotIntPattern.IsMatch(strNumber) &&  objIntPattern.IsMatch(strNumber);
  } 

  // Function to Test for Positive Number both Integer & Real 

  public static bool IsPositiveNumber(String strNumber)
  {
    Regex objNotPositivePattern=new Regex("[^0-9.]");
    Regex objPositivePattern=new Regex("^[.][0-9]+$|[0-9]*[.]*[0-9]+$");
    Regex objTwoDotPattern=new Regex("[0-9]*[.][0-9]*[.][0-9]*");
    return !objNotPositivePattern.IsMatch(strNumber) &&
      objPositivePattern.IsMatch(strNumber)  &&
      !objTwoDotPattern.IsMatch(strNumber);
  }  

  // Function to test whether the string is valid number or not
  public static bool IsNumber(String strNumber)
  {
    Regex objNotNumberPattern=new Regex("[^0-9.-]");
    Regex objTwoDotPattern=new Regex("[0-9]*[.][0-9]*[.][0-9]*");
    Regex objTwoMinusPattern=new Regex("[0-9]*[-][0-9]*[-][0-9]*");
    String strValidRealPattern="^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
    String strValidIntegerPattern="^([-]|[0-9])[0-9]*$";
    Regex objNumberPattern =new Regex("(" + strValidRealPattern +")|(" + strValidIntegerPattern + ")");
    return !objNotNumberPattern.IsMatch(strNumber) &&
      !objTwoDotPattern.IsMatch(strNumber) &&
      !objTwoMinusPattern.IsMatch(strNumber) &&
      objNumberPattern.IsMatch(strNumber);
  }  

  // Function To test for Alphabets. 

  public static bool IsAlpha(String strToCheck)
  {
    Regex objAlphaPattern=new Regex("[^a-zA-Z]");
    return !objAlphaPattern.IsMatch(strToCheck);
  }
  // Function to Check for AlphaNumeric.
  public static bool IsAlphaNumeric(String strToCheck)
  {
    Regex objAlphaNumericPattern=new Regex("[^a-zA-Z0-9]");
    return !objAlphaNumericPattern.IsMatch(strToCheck);    
  }
  public static bool IsSimpleString(String strToCheck)
  {
    Regex obj=new Regex("^\\s*[a-zA-Z,\\s]+\\s*$");
    return obj.IsMatch(strToCheck);    
  }

  // Encrypt string

  public static CryptoRijndael GetEncryptionObject()
  {
    /*System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
      byte[] Key = enc.GetBytes(Convert.ToString("--HYP.0BJ++!/#&~").PadRight(24, '|'));
      byte[] IV = enc.GetBytes("504HYC");
      CryptoSymmetric c = new CryptoSymmetric(CryptoSymmetric.CryptoAlgorithm.TripleDES, Key, IV);*/    
    CryptoRijndael c = new CryptoRijndael();
    return c;
  }

  private static string GetKey()
  {
      if (SessionState.User != null)
      {
        return Convert.ToString("--HYP.0BJ++!/#&~" + SessionState.User.Id.ToString().PadRight(24, '|'));
      }
      else
      {
        return Convert.ToString("--HYP.0BJ++!/#&~").PadRight(24, '|');
      }
  }
  public static string EncryptString(string source)
  {
    
    try
    {
      return CryptoRijndael.Encrypt(source, GetKey());
    }
    catch
    {
      return source;
    }
  }

  public static string DecryptString(string source)
  {
    try
    {
      return CryptoRijndael.Decrypt(source, GetKey());
    }
    catch
    {
      return source;
    }

  }

  /// <summary> 
  /// same  common params as the VBScript DateDiff:  http://msdn.microsoft.com/library/default.asp?url=/library/en-us/script56/html/vsfctdatediff.asp
  /// /*Sample Code: 
  /// * System.DateTime dt1 = new  System.DateTime(1974,12,16); 
  /// * System.DateTime dt2 = new System.DateTime(1973,12,16); 
  /// * Page.Response.Write(Convert.ToString(DateDiff("t", dt1, dt2)));
  ///</summary> 
  ///<param name="howtocompare"></param> 
  ///<param name="startDate"></param>
  ///<param name="endDate"></param>
  ///<returns></returns>
  public static double DateDiff(string howtocompare, System.DateTime startDate,
    System.DateTime endDate) 
  {
    double diff=0; 
    try 
    {
      System.TimeSpan TS =new System.TimeSpan(startDate.Ticks-endDate.Ticks); 
      #region converstion options 
      switch (howtocompare.ToLower()) 
      {
        case "m": diff = Convert.ToDouble(TS.TotalMinutes); break; 
        case "s": diff = Convert.ToDouble(TS.TotalSeconds); break; 
        case "t": diff = Convert.ToDouble(TS.Ticks); break; 
        case "mm": diff = Convert.ToDouble(TS.TotalMilliseconds); break; 
        case "yyyy": diff = Convert.ToDouble(TS.TotalDays/365); break; 
        case "q": diff = Convert.ToDouble((TS.TotalDays/365)/4); break; 
        default: //d 
          diff = Convert.ToDouble(TS.TotalDays); break; 
      } 
      #endregion 
    }                   
    catch 
    { 
      diff = -1; 
    }
                   
    return diff; 
  }


  public static Infragistics.WebUI.UltraWebNavigator.Node FindNodeInTree(Infragistics.WebUI.UltraWebNavigator.Nodes nodes, string key)
  {
    Infragistics.WebUI.UltraWebNavigator.Node resultNode = null;
    // Search is first level childs
    int i = 0;
    while (i < nodes.Count && resultNode==null)        
    {
      if (nodes[i].DataKey != null && nodes[i].DataKey.ToString() == key)
      {
        return nodes[i];
      }
      else
      {
        resultNode=FindNodeInTree(nodes[i].Nodes, key);
      }
      i++;
    }
    return resultNode;   
  }

  //--- Custom Replace Function CReplace
  //---	C# Loop Version
  //---	intMode = 0 = Case-Sensitive
  //---	intMode = 1 = Case-Insensitive
  public static string CReplace(	String strExpression,
    String strSearch,
    String strReplace,
    int intMode
    )
  {
    //-- Philippe Fix to handle Non Breaking Spaces
    char nbs = (char)160;
    if (strExpression.IndexOf(nbs)>0){
      strExpression = strExpression.Replace(nbs + " ", " ");
      strExpression = strExpression.Replace(" " + nbs, " ");
      strExpression = strExpression.Replace(nbs.ToString(), " ");      
    }
    //-- End of Fix
		string strReturn = strExpression;
		int intPosition;
		string strTemp;
		string strSearchTemp;
    if (strSearch!=null && strSearch.Length>0)
    {
      if(intMode == 1) 
      {
        strReturn = "";
        strSearchTemp = strSearch.ToUpper();
        strTemp = strExpression.ToUpper();
        intPosition = strTemp.IndexOf(strSearchTemp);
        while(intPosition >= 0)
        {
          strReturn = strReturn + strExpression.Substring(0,intPosition);
          string strReplaceTemp = strReplace.Replace(strSearch, strExpression.Substring(intPosition, strSearch.Length));
          strReturn += strReplaceTemp; 
          strExpression = strExpression.Substring(intPosition+strSearch.Length);
          strTemp = strTemp.Substring(intPosition+strSearch.Length);
          intPosition = strTemp.IndexOf(strSearchTemp);
        }
        strReturn = strReturn + strExpression;
      } 
      else 
      {
        //--- vbBinaryCompare
        strReturn = strExpression.Replace(strSearch,strReplace);
      }
    }
		return strReturn;
  }
 
  private static void dgExcelExport_CellExporting(object sender, Infragistics.WebUI.UltraWebGrid.ExcelExport.CellExportingEventArgs e)
  {
    if (e.Value!=null)
    {
      string v = e.Value.ToString();
      if (v.IndexOf("<font color=red>")>=0)
      {
        e.Value = v = v.Replace("<font color=red>", "").Replace("</font>", "").ToString();
        e.Value = v = v.Replace("<b>", "").Replace("</b>", "").ToString();
      }
      //Nisha Verma added code to remove HTML tags from Exported HTML file for Links project req(PR665368) on 8th feb 2013 start
      e.Value = v = v.Replace("<b>", "").Replace("</b>", "").ToString();
      //Nisha Verma added code to remove HTML tags from Exported HTML file for Links project req(PR665368) on 8th feb 2013 end 
      e.Value = v = v.Replace("</font>", "");
      e.Value = v = v.Replace("<font color=gray>", "");
      e.Value = v = v.Replace("<font color=green>", "");
      e.Value = v = v.Replace("<font color=darkblue>", "");
      e.Value = v = v.Replace("&nbsp;", " ");
      e.Value = v = v.Replace("<br/>", "\n\r");
      if (v.IndexOf("<img src='/hc_v4/img/")==0)
      {
        try
        {
          e.Value = v = v.Replace("<img src='/hc_v4/img/S", string.Empty).Replace(".gif'>", "").ToString();
          e.Value = v = v.Replace("<img src='/hc_v4/img/s", string.Empty).Replace(".gif'>", "").ToString();
          e.Value = v = v.Replace("<img src='/hc_v4/img/", string.Empty).Replace(".gif'>", "").ToString();
        }        
        catch (Exception ex)
        {
          e.Value = ex.Message;
        }
      }
      
      if (v.IndexOf("<a href=")==0)
      {
        try 
        {
          string s= v.Substring(0, v.Length-4);
          int i = s.LastIndexOf(">");
          e.Value = s.Substring(i+1, s.Length-i-1);
        }
        catch (Exception ex)
        {
          e.Value = ex.Message;
        }
      }            
    }
  }

  public static void HCGrid_PreRender(object sender, System.EventArgs e)
  {
    Infragistics.WebUI.UltraWebGrid.UltraWebGrid dg = (Infragistics.WebUI.UltraWebGrid.UltraWebGrid)sender;
    dg.DisplayLayout.Pager.AllowPaging = dg.Rows.Count > dg.DisplayLayout.Pager.PageSize;    
  }

  public static void ExportToExcel(UltraWebGrid dg, string downloadName, string documentName)
  {
      using (Infragistics.WebUI.UltraWebGrid.ExcelExport.UltraWebGridExcelExporter uwExcelReport = new UltraWebGridExcelExporter("report"))
      {
          uwExcelReport.CellExporting += new Infragistics.WebUI.UltraWebGrid.ExcelExport.CellExportingEventHandler(dgExcelExport_CellExporting);
          foreach (UltraGridColumn col in dg.Columns)
          {
              if (col.Key == GRID_SPACER_KEY || col.Key == "s_a" || col.ServerOnly) // Spacer and Sort Columns are excluded
              {
                  col.Hidden = true;
              }
          }
          dg.Page.Controls.Add(uwExcelReport);
          uwExcelReport.DownloadName = downloadName;
          uwExcelReport.WorksheetName = documentName;
          uwExcelReport.Export(dg);
          //dg.Page.Controls.Remove(uwExcelReport);
      }
  }

   public static void ExportToExcelFromGrid(UltraWebGrid dg, string downloadName, string documentName, Page page,ListItemCollection lstItemCol,string reportTitle)
   {
       
            foreach (UltraGridColumn col in dg.Columns)
            {
                if (col.Key == GRID_SPACER_KEY || col.Key == "s_a" || col.ServerOnly) // Spacer and Sort Columns are excluded
                {
                    col.Hidden = true;
                }
            }
       string fileName = downloadName+".xls";
       page.Response.Clear();
       page.Response.ClearContent();
       page.Response.ClearHeaders();
       page.Response.Charset = string.Empty;
       page.Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
       page.Response.ContentType = "application/ms-excel;";
       //Fix for CR 5109 - Prabhu R S
       page.Response.ContentEncoding = System.Text.Encoding.UTF8;

       int ColCount= 0;
       foreach (UltraGridColumn col in dg.Columns)
       {
           if (!col.Hidden)
           {
               ColCount++;
           }
       }
       //page.Response.Write(ColCount.ToString());
       /******** Writing the User info and Current date ***********/
       //Fix for QC 2374 (06 Triage CRYS: Unable to export "All Chunks" for Ink/Toner/Paper/Printer Supplies (IM2695252))
       //Prabhu R S - PCF1 - 31/Jul/09
       page.Response.Write("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'></head><body><table border = '1'>");
       page.Response.Write("<tr align='left' style='font-size: 14;font-family:Arial Unicode MS;border: none' valign='top'><td style='font-weight:bold;border: none' colspan=" + ColCount + ">" + reportTitle + "</td></tr>");
       page.Response.Write("<tr align='left' style='font-size: 14;font-family:Arial Unicode MS;border: none' valign='top'><td style='font-weight:bold;border: none'>Generated By: </td><td style='border: none' colspan=" + (ColCount - 1).ToString() + ">" + SessionState.User.FullName + "</td></tr>");
       page.Response.Write("<tr align='left' style='font-size: 14;font-family:Arial Unicode MS;border: none' valign='top'><td style='font-weight:bold;border: none'>Exported On: </td><td style='border: none' colspan=" + (ColCount - 1).ToString() + ">" + SessionState.User.FormatUtcDate(DateTime.UtcNow, true, "MM/dd/yyyy:HH:mm:ss") + "</td></tr>");
           
       /************** Writing the Extra info(i.e Selected PLs) *************/
       
            if (lstItemCol != null)
            {
                foreach (ListItem lItem in lstItemCol)
                {
                    page.Response.Write("<tr align='left' style='font-size: 14;font-family:Arial Unicode MS;border: none' valign='top'>");
                    page.Response.Write("<td style='font-weight:bold;border: none'>" + lItem.Text + "" + "</td>");
                    page.Response.Write("<td style='border: none' colspan=" + (ColCount - 1).ToString() + ">" + lItem.Value + "" + "</td>");
                    page.Response.Write("</tr>");
                }
                
            }
       /*********** Writing the Grid Headers **************************/
            page.Response.Write("<tr align='left' style='font-size:14;font-family:Arial Unicode MS;'>");
            foreach (UltraGridColumn col in dg.Columns)
            {
                if (!col.Hidden)
                {
                    page.Response.Write("<td style='font-weight:bold;'>" + col.Header.Caption + "</td>");
                }
            }
            page.Response.Write("</tr>");
       
       /*********** Writing the Grid Datas **************************/
            string strExport = string.Empty;
            string cellStyle = string.Empty;
            
            for (int row = 0; row < dg.Rows.Count; row++)
            {
                page.Response.Write("<tr align='left' style='font-size:14;font-family:Arial Unicode MS;'>");
                for (int col = 0; col < dg.Columns.Count; col++)
                {
                    if (!dg.Columns[col].Hidden)
                    {
                       if(dg.Rows[row].Cells[col].Style.Font.Bold)
                            cellStyle = "<td style='color:"+dg.Columns[col].CellStyle.ForeColor.Name+";font-weight:bold;'>";
                        else
                            cellStyle = "<td style='color:" + dg.Columns[col].CellStyle.ForeColor.Name + ";'>";
                        
                       
                        page.Response.Write(cellStyle);
                        strExport = (dg.Rows[row].Cells[col].Value+"").Trim();
                        if (strExport != string.Empty && strExport!="" && strExport != null)
                        {
                            if (strExport.Contains("<a href"))
                            {
                                strExport = strExport.Remove(0, strExport.IndexOf(">") + 1);
                                strExport.Replace("</a>", "");
                            }
                        }
                       
                        page.Response.Write(strExport);
                        page.Response.Write("</td>");
                    }
                }
                page.Response.Write("</tr>");
            }
            page.Response.Write("</table></body></html>");
    }
    /************Start For exporting Advanced Search*******************/
    public static void ExportToExcelFromGrid(UltraWebGrid dg, string downloadName, string documentName, Page page, DataTable objDTBussinessFilters,DataTable dtCustomFilters, string reportTitle,string searchType)
    {

        foreach (UltraGridColumn col in dg.Columns)
        {
            if (col.Key == GRID_SPACER_KEY || col.Key == "s_a" || col.ServerOnly) // Spacer and Sort Columns are excluded
            {
                col.Hidden = true;
            }
        }
        string fileName = downloadName + ".xls";
        page.Response.Clear();
        page.Response.ClearContent();
        page.Response.ClearHeaders();
        page.Response.Charset = string.Empty;
        page.Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
        page.Response.ContentType = "application/ms-excel;";
        //Fix for CR 5109 - Prabhu R S
        page.Response.ContentEncoding = System.Text.Encoding.UTF8;
        int ColCount = 0;
        foreach (UltraGridColumn col in dg.Columns)
        {
            if (!col.Hidden)
            {
                ColCount++;
            }
        }
        //page.Response.Write(ColCount.ToString());
        /******** Writing the User info and Current date ***********/
        //Fix for QC 2374 (06 Triage CRYS: Unable to export "All Chunks" for Ink/Toner/Paper/Printer Supplies (IM2695252))
        //Prabhu R S - PCF1 - 31/Jul/09
        page.Response.Write("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'></head><body><table border = '1'>");
        page.Response.Write("<tr align='left' style='font-size: 14;font-family:Arial Unicode MS;border: none' valign='top'><td style='font-weight:bold;border: none' colspan=" + ColCount + ">" + reportTitle + "</td></tr>");
        page.Response.Write("<tr align='left' style='font-size: 14;font-family:Arial Unicode MS;border: none' valign='top'><td style='font-weight:bold;border: none'>Generated By: </td><td style='border: none' colspan=" + (ColCount - 1).ToString() + ">" + SessionState.User.FullName + "</td></tr>");
        page.Response.Write("<tr align='left' style='font-size: 14;font-family:Arial Unicode MS;border: none' valign='top'><td style='font-weight:bold;border: none'>Exported On: </td><td style='border: none' colspan=" + (ColCount - 1).ToString() + ">" + SessionState.User.FormatUtcDate(DateTime.UtcNow, true, "MM/dd/yyyy:HH:mm:ss") + "</td></tr>");
        /************** Writing the Extra info *************/
        /****** Selected Custom Filters ****************/
        if (dtCustomFilters != null && searchType == "Custom Search")
        {
        object[] objDistinctParams =GetDistinctValues(dtCustomFilters.Select(), "ParameterName");
        DataRow[] drCol = null;
        
            if (dtCustomFilters.Rows.Count > 0)
            {
                page.Response.Write("<tr align='left' style='font-size: 14;font-family:Arial Unicode MS;border: none' valign='top'>");
                page.Response.Write("<td style='font-weight:bold;border: none;background-color:Silver' colspan=" + (ColCount).ToString() + ">Custom Filters</td>");
                page.Response.Write("</tr>");
            }
            for (int countParams = 0; countParams < objDistinctParams.Length; countParams++)
            {

                drCol = dtCustomFilters.Select("ParameterName='" + objDistinctParams[countParams].ToString() + "'");
                page.Response.Write("<tr align='left' style='font-size: 14;font-family:Arial Unicode MS;border: none' valign='top'>");
                page.Response.Write("<td style='font-weight:bold;border: none' colspan=" + (ColCount).ToString() + ">" + drCol[0]["ParameterName"] + "" + "</td>");
                page.Response.Write("</tr>");
                bool Operator = false;
                if (drCol[0]["Operator"] != null && drCol[0]["Operator"] != string.Empty)
                {
                    page.Response.Write("<tr align='left' style='font-size: 14;font-family:Arial Unicode MS;border: none' valign='top'>");
                    page.Response.Write("<td style='font-weight:bold;border: none' valign='top' rowspan=" + (drCol.Length).ToString() + ">" + drCol[0]["Operator"] + "" + "</td>");
                    //page.Response.Write("</tr>");
                    Operator = true;
                }
                foreach (DataRow objDR in drCol)
                {
                    if (!Operator)
                    {
                        page.Response.Write("<tr align='left' style='font-size: 14;font-family:Arial Unicode MS;border: none' valign='top'>");
                    }
                    if ((objDR["filterText"] + "") != "")
                    {
                        page.Response.Write("<td style='border: none'>" + objDR["filterText"] + "" + "</td>");
                    }
                    page.Response.Write("<td style='border: none' colspan=" + (ColCount - 2).ToString() + ">" + objDR["filterValue"] + "" + "</td>");
                    page.Response.Write("</tr>");
                    Operator = false;

                }

            }
        }
        /****** Selected Bussiness Filters ****************/
        if (objDTBussinessFilters != null)
        {
            page.Response.Write("<tr align='left' style='font-size: 14;font-family:Arial Unicode MS;border: none' valign='top'>");
            string subHeading = searchType == "Template Search" ? "Template Search" : "Business Filters";
            page.Response.Write("<td style='font-weight:bold;border: none;background-color:Silver' colspan=" + (ColCount).ToString() + ">"+subHeading+"</td>");
            page.Response.Write("</tr>");
            foreach (DataRow dr in objDTBussinessFilters.Rows)
            {
                page.Response.Write("<tr align='left' style='font-size: 14;font-family:Arial Unicode MS;border: none' valign='top'>");
                page.Response.Write("<td style='font-weight:bold;border: none'>" + dr[0] + "" + "</td>");
                page.Response.Write("<td style='border: none' colspan=" + (ColCount - 1).ToString() + ">" + dr[1] + "" + "</td>");
                page.Response.Write("</tr>");
            }

        }
       
        /*********** Writing the Grid Headers **************************/
        page.Response.Write("<tr align='left' style='font-size:14;font-family:Arial Unicode MS;'>");
        foreach (UltraGridColumn col in dg.Columns)
        {
            if (!col.Hidden)
            {
                page.Response.Write("<td style='font-weight:bold;'>" + col.Header.Caption + "</td>");
            }
        }
        page.Response.Write("</tr>");

        /*********** Writing the Grid Datas **************************/
        string strExport = string.Empty;
        string cellStyle = string.Empty;

        for (int row = 0; row < dg.Rows.Count; row++)
        {
            page.Response.Write("<tr align='left' style='font-size:14;font-family:Arial Unicode MS;'>");
            for (int col = 0; col < dg.Columns.Count; col++)
            {
                if (!dg.Columns[col].Hidden)
                {
                    if (dg.Rows[row].Cells[col].Style.Font.Bold)
                        cellStyle = "<td style='color:" + dg.Columns[col].CellStyle.ForeColor.Name + ";font-weight:bold;'>";
                    else
                        cellStyle = "<td style='color:" + dg.Columns[col].CellStyle.ForeColor.Name + ";'>";


                    page.Response.Write(cellStyle);
                    strExport = (dg.Rows[row].Cells[col].Value + "").Trim();
                    if (strExport != string.Empty && strExport != "" && strExport != null)
                    {
                        if (strExport.Contains("<a href"))
                        {
                            strExport = strExport.Remove(0, strExport.IndexOf(">") + 1);
                            strExport.Replace("</a>", "");
                        }
                    }

                    page.Response.Write(strExport);
                    page.Response.Write("</td>");
                }
            }
            page.Response.Write("</tr>");
        }
        page.Response.Write("</table></body></html>");
    }

    public static object[] GetDistinctValues(DataRow[] dRowCol, string colName)
    {
        Hashtable hTable = new Hashtable();
        foreach (DataRow drow in dRowCol)
            if (!hTable.ContainsKey(drow[colName]))
                hTable.Add(drow[colName], string.Empty);
        object[] objArray = new object[hTable.Keys.Count];
        hTable.Keys.CopyTo(objArray, 0);
        return objArray;
    }
    /************End For exporting Advanced Search*******************/
    public static System.Text.StringBuilder ExportDataTableToCSVForSpecificReport(DataTable dt, string sep, ListItemCollection lstItemCol, string reportTitle)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        bool isHeader = true;
        /******** Writing the User info and Current date ***********/
        sb.Append(reportTitle);
        sb.Append(Environment.NewLine);
        sb.Append("Generated By:" + sep + SessionState.User.FullName);
        sb.Append(Environment.NewLine);
        sb.Append("Exported On:" + sep + SessionState.User.FormatUtcDate(DateTime.UtcNow, true, "MM/dd/yyyy:HH:mm:ss"));
        sb.Append(Environment.NewLine);

        /************** Writing the Extra info(i.e Selected PLs) *************/
        
        if (lstItemCol != null)
        {
            foreach (ListItem lItem in lstItemCol)
            {
                sb.Append(lItem.Text + "" + sep + lItem.Value + "" + Environment.NewLine);
                
            }

        }
        foreach (DataRow row in dt.Rows)
        {
            bool isFirst = true;
            if (isHeader)
            {
                foreach (DataColumn col in dt.Columns)
                    if (col.ColumnMapping != MappingType.Hidden)
                    {
                        sb.Append((isFirst ? "" : sep) + col.Caption);
                        isFirst = false;
                    }
            }
            sb.Append(Environment.NewLine);
            isHeader = false;
            isFirst = true;
            foreach (DataColumn col in dt.Columns)
                if (col.ColumnMapping != MappingType.Hidden)
                {
                    string value = row[col].ToString();
                    if (value.IndexOf(sep) >= 0)
                        value = "\"" + sep + "\"";
                    sb.Append((isFirst ? "" : sep) + value);
                    isFirst = false;
                }
        }
        return sb;
    }

  public static System.Text.StringBuilder ExportDataTableToCSV(DataTable dt)
  {
    return ExportDataTableToCSV(dt, ";");
  }
  public static System.Text.StringBuilder ExportDataTableToCSV(DataTable dt, string sep)
  {
    System.Text.StringBuilder sb = new System.Text.StringBuilder();
    bool isHeader = true;
    foreach (DataRow row in dt.Rows)
    {
      bool isFirst = true;
      if (isHeader)
      {
        foreach (DataColumn col in dt.Columns)
          if (col.ColumnMapping != MappingType.Hidden)
          {
            sb.Append((isFirst ? "" : sep) + col.Caption);
            isFirst = false;
          }
      }
      sb.Append(Environment.NewLine);
      isHeader = false;
      isFirst = true;
      foreach (DataColumn col in dt.Columns)
        if (col.ColumnMapping != MappingType.Hidden)
        {
          string value = row[col].ToString();
          if (value.IndexOf(sep) >= 0)
            value = "\"" + sep + "\"";
          sb.Append((isFirst ? "" : sep) + value);
          isFirst = false;
        }
    }
    return sb;
  }

  public static void ExportDataSetToExcel(System.Web.UI.Page pg, DataSet ds, string DownloadName)
  {
    Infragistics.WebUI.UltraWebGrid.ExcelExport.UltraWebGridExcelExporter uwExcelReport;
    uwExcelReport = new Infragistics.WebUI.UltraWebGrid.ExcelExport.UltraWebGridExcelExporter("report");
    Infragistics.Documents.Excel.Workbook wbk = new Infragistics.Documents.Excel.Workbook();	// Create a new Work book

    // Setting Export Mode to Custom removes the prompt for download
    uwExcelReport.ExportMode = Infragistics.WebUI.UltraWebGrid.ExcelExport.ExportMode.Custom;
    pg.Controls.Add(uwExcelReport);

    foreach (DataTable dt in ds.Tables)
    {
      wbk.Worksheets.Add(dt.TableName);
      wbk.ActiveWorksheet = wbk.Worksheets[dt.TableName];
      using (Infragistics.WebUI.UltraWebGrid.UltraWebGrid dg = new Infragistics.WebUI.UltraWebGrid.UltraWebGrid("export"))
      {
        //if (dt == null && dt.Rows.Count > 0)
        //{
        if (dg != null && dt.Rows.Count>0)
        {
          /*pg.Response.Write("<br/>" + dt.TableName +", ");
          pg.Response.Write(dt.Columns.Count.ToString());
          for (int k = 0; k < dt.Columns.Count; k++)
          {
            pg.Response.Write(dt.Columns[k].ColumnName +", ");
          }*/
          dg.DataSource = dt;
          //try
          //{
            dg.DataBind();
          /*}
          catch (Exception ex)
          {
            pg.Response.Write(" --> " + ex.ToString());
          }*/
          pg.Controls.Add(dg);
          uwExcelReport.Export(dg, wbk);
          pg.Controls.Remove(dg);
        }
        //}
      }
    }
    ds.Dispose();

    wbk.ActiveWorksheet = wbk.Worksheets[0];
    MemoryStream stream = new MemoryStream();
    BIFF8Writer.WriteWorkbookToStream(wbk, stream);

    pg.Response.Clear();
    pg.Response.ClearContent();
    pg.Response.ClearHeaders();
    pg.Response.AddHeader("content-disposition", "attachment;filename=" + DownloadName);
    pg.Response.ContentType = "application/ms-excel";
    //Fix for CR 5109 - Prabhu R S
    pg.Response.ContentEncoding = System.Text.Encoding.UTF8;
    pg.Response.AppendHeader("Content-Length", stream.Length.ToString());

    pg.EnableViewState = false;
    pg.Response.OutputStream.Write(stream.ToArray(), 0, Convert.ToInt32(stream.Length));
    pg.Response.End();
    //BinaryWriteToResponse(DownloadName, pg.Response, stream);
  }
  public static void BinaryWriteToResponse(string fileName, HttpResponse resp, System.IO.MemoryStream stream)
  {
    // Philippe: rewrote the function in order to be https compliant
    resp.Clear();
    resp.ClearContent();
    resp.ClearHeaders();
    resp.AddHeader("Accept-Header", stream.Length.ToString());
    resp.ContentType = "application/octet-stream";
    //Fix for CR 5109 - Prabhu R S
    resp.ContentEncoding = System.Text.Encoding.UTF8;
    resp.AppendHeader("Content-Disposition","attachment;filename=\"" + fileName + "\"; " +
                      "size=" + stream .Length.ToString() + "; " +
                      "creation-date=" + DateTime.Now.ToString("R") + "; " +
                      "modification-date=" + DateTime.Now.ToString("R") + "; " +
                      "read-date=" + DateTime.Now.ToString("R"));
    resp.OutputStream.Write(
    stream.ToArray(), 0,
    Convert.ToInt32(stream.Length));
    resp.Flush();
    stream.Close();
    try { resp.End(); }
    catch { }

  }

  /// <summary> 
  /// Prevent controls from making multiple server PostBack requests if one is already happening. 
  /// </summary> 
  /// <param name="control">A WebControl that causes a server PostBack.</param> 
  /// <param name="eventType">Type of event, like "onchange" or "onclick".</param> 
  public static void PreventMultiplePostback(System.Web.UI.WebControls.WebControl control, string eventType) 
  { 
    if(!control.Page.ClientScript.IsClientScriptBlockRegistered("bPostbackHappeningNow")) 
    {
        control.Page.ClientScript.RegisterClientScriptBlock(control.Page.GetType(), "bPostbackHappeningNow", 
        "<script lang='JavaScript'>var bPostbackHappeningNow = false;</script>"); 
    } 
    System.Text.StringBuilder sb = new System.Text.StringBuilder(); 
    sb.Append("if (typeof(Page_ClientValidate) == 'function') { "); 
    sb.Append("if (Page_ClientValidate() == false) { return false; }} "); 
    sb.Append("if (bPostbackHappeningNow) return false;"); 
    sb.Append("bPostbackHappeningNow = true;"); 
    control.Attributes.Add(eventType, sb.ToString()); 
  }
  #region AcquistionDashBoardExcelExport
  /*<summary>
  Description: 70252 - GEMS: Acquisition Dashboard export file columns are not lined up correctly 
  The issue was due to the Default way in which the UltraWebGridExcel Exporter works
  The issue could've been fixed in the allready exisiting Utils.ExcelExporter but dint do that 
  b'coz in other Hierarchical Grid users may *still* would like to view the excel as it was allready present.
  The Fix is to remove the headers in all the Bands except Band[0] and also to allign all the Bands to the first Cell of the Excel.
  Header is removed under BeginExport event, allignment is done under RowExporting Event
  Dependencies: NONE
  Authored by: Ramachandran
  Date: 06/13/2007 
  </summary>*/
   public static void AcqExportToExcel(UltraWebGrid dg, string downloadName, string documentName)
    {
        using (Infragistics.WebUI.UltraWebGrid.ExcelExport.UltraWebGridExcelExporter uwExcelReport = new UltraWebGridExcelExporter("report"))
        {
            uwExcelReport.CellExporting += new Infragistics.WebUI.UltraWebGrid.ExcelExport.CellExportingEventHandler(dgExcelExport_CellExporting);
            //following 2 lines are event handlers added
            uwExcelReport.RowExporting += new RowExportingEventHandler(uwExcelReport_RowExporting);
            uwExcelReport.BeginExport += new BeginExportEventHandler(uwExcelReport_BeginExport);
            foreach (UltraGridColumn col in dg.Columns)
            {
                if (col.Key == GRID_SPACER_KEY || col.Key == "s_a" || col.ServerOnly) // Spacer and Sort Columns are excluded
                {
                    col.Hidden = true;
                }
            }
            dg.Page.Controls.Add(uwExcelReport);
            uwExcelReport.DownloadName = downloadName;
            uwExcelReport.WorksheetName = documentName;
            uwExcelReport.Export(dg);
            //dg.Page.Controls.Remove(uwExcelReport);
        }
    }

    static void uwExcelReport_RowExporting(object sender, RowExportingEventArgs e)
    {
        // All Rows should start from 1st Cell
        e.CurrentColumnIndex = 0;
    }

    static void uwExcelReport_BeginExport(object sender, BeginExportEventArgs e)
    {
        foreach (UltraGridBand dgBand in e.Layout.Bands)
        {
            // Remove all the headers in Bands other than the primary Band
            if (dgBand.Index > 0)
            {
                dgBand.ColHeadersVisible = ShowMarginInfo.No;
            }
        }
    }
  #endregion

   
}
namespace ExcelHelper
{

	public class ExcelExporter
	{
		/// <summary>
		/// This helper function will take an Excel Workbook 
		/// and write it to the response stream. In this fashion, 
		/// the end user will be prompted to download or open the
		/// resulting Excel file. 
		/// </summary>
		/// <param name="theWorkBook"></param>
		/// <param name="FileName"></param>
		/// <param name="resp"></param>
		public static void WriteToResponse(Workbook theWorkBook, string fileName, HttpResponse resp)
		{
            //Create the Stream class
			System.IO.MemoryStream theStream = new System.IO.MemoryStream();

            //Write the in memory Workbook object to the Stream
			BIFF8Writer.WriteWorkbookToStream(theWorkBook, theStream);
      Utils.BinaryWriteToResponse(fileName, resp, theStream); 
		}

		/// <summary>
		/// This will save the WorkBook to a disk file
		/// </summary>
		/// <param name="theWorkBook"></param>
		/// <param name="theFile"></param>
		public static void WriteToDisk(Workbook theWorkBook, string theFile)
		{
			BIFF8Writer.WriteWorkbookToFile(theWorkBook, theFile);
		}

		public static Workbook DataSetToExcel(DataSet ds)
		{
			Workbook b = new Workbook();
			Worksheet w;

			int iRow = 0;
			int iCell = 0;

			foreach (DataTable t in ds.Tables)
			{
				w = b.Worksheets.Add(t.TableName);

				iCell = 0;

				foreach (DataColumn col in t.Columns)
				{
					iRow = 0;

					w.Rows[iRow].Cells[iCell].Value = col.ColumnName;

					iRow += 1;

					foreach (DataRow r in t.Rows)
					{
						if (!(r[col.ColumnName] is byte[]))
						{
							w.Rows[iRow].Cells[iCell].Value = r[col.ColumnName];
						}
						else
						{
							w.Rows[iRow].Cells[iCell].Value = r[col.ColumnName].ToString();
						}

						iRow += 1;
					}

					iCell += 1;
				} //DataColumn
			} //DataTable

			return b;
		}

	}

}

namespace HyperCatalog.ExcelHelper
{

  public class ExcelExporter
  {
    /// <summary>
    /// This helper function will take an Excel Workbook 
    /// and write it to the response stream. In this fashion, 
    /// the end user will be prompted to download or open the
    /// resulting Excel file. 
    /// </summary>
    /// <param name="theWorkBook"></param>
    /// <param name="FileName"></param>
    /// <param name="resp"></param>
    public static void WriteToResponse(Workbook theWorkBook, string fileName, HttpResponse resp)
    {
      //Create the Stream class
      System.IO.MemoryStream theStream = new System.IO.MemoryStream();

      //Write the in memory Workbook object to the Stream
      BIFF8Writer.WriteWorkbookToStream(theWorkBook, theStream);
      Utils.BinaryWriteToResponse(fileName, resp, theStream); 
    }

    /// <summary>
    /// This will save the WorkBook to a disk file
    /// </summary>
    /// <param name="theWorkBook"></param>
    /// <param name="theFile"></param>
    public static void WriteToDisk(Workbook theWorkBook, string theFile)
    {
      BIFF8Writer.WriteWorkbookToFile(theWorkBook, theFile);
    }

    public static Workbook DataSetToExcel(DataSet ds)
    {
      Workbook b = new Workbook();
      Worksheet w;

      int iRow = 0;
      int iCell = 0;

      foreach (DataTable t in ds.Tables)
      {
          if (ds.Tables[0].TableName.Length > 31)
          {
              //Nisha Verma Added the code for IE error while exporting the long input form name containers
              string trunk = string.Empty;
              trunk = Convert.ToString(t.TableName.ToString().Replace(@"[", @"(").Replace(@"]", @")"));
              if (trunk.Length > 30)
              {
                  trunk = trunk.Substring(0, 30);
                  w = b.Worksheets.Add(trunk);
              }
              else
              {
                  w = b.Worksheets.Add(t.TableName.ToString().Replace(@"[", @"(").Replace(@"]", @")"));
              }

          }
          else
          {

              w = b.Worksheets.Add(t.TableName.ToString().Replace(@"[", @"(").Replace(@"]", @")"));
          }

        iCell = 0;
        iRow = 0;
        w.Rows[iRow].Cells[iCell].Value = t.TableName;
        WorksheetMergedCellsRegion region = w.MergedCellsRegions.Add(0, 0, 0, t.Columns.Count - 1);
        region.CellFormat.Font.Height = 300;
        w.Rows[iRow].Height = 400;
        foreach (DataColumn col in t.Columns)
        {
          iRow = 1;

          w.Rows[iRow].Cells[iCell].Value = col.ColumnName;

          iRow += 1;

          foreach (DataRow r in t.Rows)
          {
            if (!(r[col.ColumnName] is byte[]))
            {
              w.Rows[iRow].Cells[iCell].Value = r[col.ColumnName];
            }
            else
            {
              w.Rows[iRow].Cells[iCell].Value = r[col.ColumnName].ToString();
            }

            iRow += 1;
          }

          iCell += 1;
        } //DataColumn
      } //DataTable

      return b;
    }
  }
}
