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
using HyperCatalog.Business;
using HyperCatalog.Shared;
using HyperComponents.Data.dbAccess;
using System.Data.SqlClient;
using Infragistics.WebUI.UltraWebNavigator;
using System.Text;
using System.IO;

namespace HyperCatalog.UI.Acquire.QDE
{
  /// <summary>
  /// QDE_CloneItem class clones item
  /// </summary>
  public partial class QDE_ExportItem : HCPage
  {
    #region Declarations
    public HyperCatalog.Business.Item itemObj;
    // public RadioButton RadioButton1=new RadioButton();
    // public RadioButton RadioButton2=new RadioButton();

    int skuLevelId = 7;
    Culture cul;
    bool displaySoftRoll = false;
    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
    {
      #region Retrieve Item information
      try
      {
        itemObj = QDEUtils.GetItemIdFromRequest();
        skuLevelId = HyperCatalog.Shared.SessionState.SkuLevel.Id;
        cul = QDEUtils.UpdateCultureCodeFromRequest();

        if (Request["r"] != null)
        {
          displaySoftRoll = Convert.ToBoolean(Request["r"]);
        }
      }
      catch
      {
        throw new ArgumentException("Item Id was not provided");
      }
      #endregion

      #region Capability
      if ((SessionState.User.IsReadOnly) && (SessionState.User.HasCapability(CapabilitiesEnum.EXPORT_ITEMS)) &&
        (itemObj.LevelId < Convert.ToInt32(SessionState.CacheParams["Item_ExportMaxLevel"].Value)))
      {
        UITools.DenyAccess(DenyMode.Popup);
      }
      #endregion

      try
      {
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Initialize", "<script>cbFilterID='" + cbFilter.ClientID + "'</script>");

        if (!Page.IsPostBack)
        {
          UpdateDataView();
        }
      }
      catch (Exception ex)
      {
        UITools.JsCloseWin(Utils.jsReplace(ex.ToString()));
      }
    }
    private void AddChild(Node n, HyperCatalog.Business.Item obj)
    {
      if ((displaySoftRoll && (obj.IsRoll || obj.GetRoll() == null))
        || (!displaySoftRoll && !obj.IsRoll))
      {
        Node child = new Node();
        child.DataKey = obj.Id;
        child.Text = string.Empty;
        if (obj.IsRoll)
          child.Text += "<img src='/hc_v4/img/ed_roll.gif'> ";
        child.Text += obj.FullName;
        child.ImageUrl = child.SelectedImageUrl = "/hc_v4/ig/s_" + obj.Status.ToString().Substring(0, 1) + ".gif";
        if (obj.LevelId == skuLevelId)
        {
          child.ImageUrl = "/hc_v4/img/type_" + obj.TypeId.ToString() + ".png";
          if (obj.Sku.ToUpper().EndsWith("T")) // Top Value
          {
            child.ImageUrl = "/hc_v4/img/type_1.png";
          }
          child.SelectedImageUrl = child.ImageUrl;
          switch (obj.Status.ToString())
          {
            case ("O"):
              child.Text = "<font color=gray>[O] </font>" + child.Text;
              break;
            case ("F"):
              child.Text = "<font color=green>[F] </font>" + child.Text;
              break;
          }
        }
        else
        {
          if (obj.LevelId > skuLevelId)
          {
            child.SelectedImageUrl = child.ImageUrl = "/hc_v4/img/option.png";
          }
        }
        n.Nodes.Add(child);

        foreach (HyperCatalog.Business.Item subItem in obj.Childs)
        {
          AddChild(child, subItem);
        }
      }
    }

    private void UpdateDataView()
    {
      // Check if main item has a roll in its descendants
      if (itemObj.HasRollDescendant() || itemObj.GetRoll() != null)
      {
        cbFilter.Visible = true;
         //UITools.ShowToolBarLabel(uwToolbar, "lbSoftRoll");
        uwToolbar.Items.FromKeyButton("lbSoftRoll").Visible = true;
        UITools.ShowToolBarSeparator(uwToolbar, "CloseSep");
      }
      else
      {
        cbFilter.Visible = false;
        //UITools.HideToolBarLabel(uwToolbar, "lbSoftRoll");
        uwToolbar.Items.FromKeyButton("lbSoftRoll").Visible = false;
        UITools.HideToolBarSeparator(uwToolbar, "CloseSep");
      }

      cbFilter.Checked = displaySoftRoll;

      //lbTitle.Text = "Export product - " + itemObj.FullName;
      //lbError.Visible = false;
      webTree.ClearAll();
      Node mainNode = webTree.Nodes.Add("Select the Items to export");
      mainNode.CheckBox = CheckBoxes.False;
      if (displaySoftRoll && itemObj.GetRoll() != null)
        itemObj = itemObj.GetRoll();
      AddChild(mainNode, itemObj);
      webTree.ExpandAll();
    }

    protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      if (btn.Equals("export"))
      {
        Export();
      }
    }
    private void Export()
    {
      string itemList = string.Empty;
      for (int i = 0; i < webTree.CheckedNodes.Count; i++)
      {
        if (i > 0) itemList += ",";
        itemList += ((Node)webTree.CheckedNodes[i]).DataKey;
      }
      RadioButton rdexcel = (RadioButton)uwToolbar.FindControl("RadioButton2");
      RadioButton rdxml = (RadioButton)uwToolbar.FindControl("RadioButton1");
      //RadioButton rdexcel = (RadioButton)RadioButton2;
      //RadioButton rdxml = (RadioButton)RadioButton1;
      if (itemList.Length > 0)
      {
        if (rdxml.Checked)
        {
          using (HyperCatalog.WebServices.WSCPIImport.WSCPIImport exportObj = HyperCatalog.WebServices.WSInterface.WSCPIImport)
          {
            //exportObj.BeginExportCPI("toto", 1, "", null, null);
            HyperCatalog.WebServices.WSCPIImport.xmlxsdClass r = exportObj.ExportCPI(itemList, SessionState.User.Id, cul.Code);
            Response.Expires = 0;
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Clear();
            if (r != null)
            {
              Response.ContentType = "application/octet-stream";
              Response.AddHeader("Content-Disposition", "attachment;filename=" + System.IO.Path.GetFileName(r.strFileName + ".zip"));
              //Response.AddHeader("Content-Length", r.zipinbytes.Length.ToString());
              Response.Flush();
              Response.BinaryWrite(r.zipinbytes);
              Response.Flush();
              Response.End();
              r = null;
            }
            else
            {
              Response.Write("An error Occured with ItemList = " + itemList);
              Response.Flush();
            }
            Response.End();
          }
        }
        else if (rdexcel.Checked)
        {
          //Response.End();

          using (HyperCatalog.WebServices.WSCASExport1.WSCASExport obj = HyperCatalog.WebServices.WSInterface.WSCASExport)
          //using (CAS.WSCPIImport obj = new CAS.WSCPIImport())
          {
            obj.Credentials = System.Net.CredentialCache.DefaultCredentials;
            //string r = string.Empty;
            //string sFilename = string.Empty;
            //string sArchFolder = string.Empty;
            //r = obj.ExportCPI(itemList, SessionState.User.Id, cul.Code);
            ////exportObj.BeginExportCPI("toto", 1, "", null, null);
            ////HyperCatalog.WebServices.WSCPIImport.xmlxsdClass r = exportObj.ExportCPI(itemList, SessionState.User.Id, cul.Code);                      
            //r = r.Replace("CAS Export Completed Successfully ", "");
            ////r = @"\\cceap0014\prism_dev\PRISM_DEV_FPE_Processing\Gemstone\Offline\XLS_Inbound\OfflineCPI_46_20070404_105041.xls";

            //if (r != "Failed")
            //{
            //  string[] SPLITPATH = r.Split(':');
            //  byte[] fileBytes = null;
            //  Stream fileStream = File.OpenRead(SPLITPATH[0]);
            //  if (fileStream.CanRead)
            //  {
            //    long fileSize = fileStream.Length;
            //    fileBytes = new byte[fileSize];
            //    fileStream.Read(fileBytes, 0, Convert.ToInt32(fileSize));
            //  }
            //  fileStream.Close();
            //  sFilename = SPLITPATH[0].Substring(SPLITPATH[0].LastIndexOf("\\"), SPLITPATH[0].Length - (SPLITPATH[0].LastIndexOf("\\"))).Replace("\\", "");
            //  sArchFolder = SPLITPATH[1] + "\\";
            //  System.IO.File.Move(SPLITPATH[0], sArchFolder + sFilename);

            //  Response.ClearContent();
            //  Response.ClearHeaders();
            //  Response.Clear();
            //  Response.Expires = 0;
            //  Response.Buffer = true;
            //  Response.ContentType = "application/octet-stream";  // Handle this 
            //  Response.AddHeader("Content-Disposition", "attachment; filename=" + sFilename);
            //  Response.AppendHeader("Content-Length", fileBytes.Length.ToString());
            //  Response.Flush();
            //  Response.BinaryWrite(fileBytes);
            //  Response.Flush();
            //  Response.End();
            //  Response.Clear();
            //}
            //else
            //{
            //  Response.Write("An error Occured with ItemList = " + itemList);
            //}
            //Response.End();

            //HyperCatalog.WebServices.WSCPIImport.xmlxsdClass r = exportObj.ExportCPI(itemList, SessionState.User.Id, cul.Code);
            HyperCatalog.WebServices.WSCASExport1.xmlxsdClass r = obj.ExportCPI(itemList, SessionState.User.Id, cul.Code);
            //CAS.xmlxsdClass r = obj.ExportCPI(itemList, SessionState.User.Id, cul.Code);
            Response.Expires = 0;
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Clear();
            if (r != null)
            {
              Response.ContentType = "application/octet-stream";
              Response.AddHeader("Content-Disposition", "attachment;filename=" + System.IO.Path.GetFileName(r.strFileName));
              //Response.AddHeader("Content-Length", r.zipinbytes.Length.ToString());
              //Response.AppendHeader("Content-Length", fileBytes.Length.ToString());
              Response.Flush();
              Response.BinaryWrite(r.zipinbytes);
              Response.Flush();
              Response.End();
              r = null;
            }
            else
            {
              Response.Write("An error Occured with ItemList = " + itemList);
            }
            Response.End();
          }
          /*
              if (r != "Failed")
              {
                         
                  //Response.Write(r.ToString());

                  byte[] fileBytes = null;
                  Stream fileStream = File.OpenRead(r);
                  if (fileStream.CanRead)
                  {
                    long fileSize = fileStream.Length;
                    fileBytes = new byte[fileSize];
                    fileStream.Read(fileBytes, 0, Convert.ToInt32(fileSize));
                  }
                  fileStream.Close();

                  sFilename = r.Substring(r.LastIndexOf("\\"), r.Length - (r.LastIndexOf("\\"))).Replace("\\", "");


                  Response.ClearContent();
                  Response.ClearHeaders();
                  Response.Clear();
                  Response.Expires = 0;
                  Response.Buffer = true;
                  Response.ContentType = "application/octet-stream";  // Handle this 
                  Response.AddHeader("Content-Disposition", "attachment; filename=" + sFilename);
                  Response.AppendHeader("Content-Length", fileBytes.Length.ToString());
                  Response.Flush();
                  Response.BinaryWrite(fileBytes);
                  Response.Flush();
                  Response.End();
                  Response.Clear(); */

        }
        else
        {
          lbError.CssClass = "hc_error";
          lbError.Text = "Select the Excel/XML Export Option";
          lbError.Visible = true;

        }
      }
      else
      {
        lbError.CssClass = "hc_error";
        lbError.Text = "Select the Items to export";
        lbError.Visible = true;
      }
    }
  }
}
