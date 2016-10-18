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
using HyperCatalog.Business;
using HyperCatalog.Shared;
using Infragistics.WebUI.UltraWebGrid;
using HyperCatalog.WebServices.TranslationWS;
using HyperCatalog.WebServices.EventLoggerWS;
#endregion

namespace hc_termbase.UI.Globalize
{
	/// <summary>
	/// Description résumée de TRUpload.
	/// </summary>
  public partial class TRUpload : HCPage
  {
    string credential;

    protected void Page_Load(object sender, System.EventArgs e)
    {
      // connect to webservice
      lbMessage.Visible = false;
      txtTRMessage.Visible = false;
      wBtDelete.Enabled = wBtProcess.Enabled = true;
      System.Net.CookieContainer cookies = new System.Net.CookieContainer();
      QDEUtils.wsTranslationObj.CookieContainer = cookies;
      credential = QDEUtils.wsTranslationObj.SignOn(SessionState.User.Pseudo, SessionState.User.ClearPassword);
      if (!Page.IsPostBack)
      {
        UpdateDataView();
        LoadCurrentTRsToDelete();
      }
    }

    #region Code généré par le Concepteur Web Form
    override protected void OnInit(EventArgs e)
    {
      //
      // CODEGEN : Cet appel est requis par le Concepteur Web Form ASP.NET.
      //
      InitializeComponent();
      base.OnInit(e);
    }

    /// <summary>
    /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
    /// le contenu de cette méthode avec l'éditeur de code.
    /// </summary>
    private void InitializeComponent()
    {

    }
    #endregion

    public static string ToHex(byte[] bytes)
    {
      string hex = "";
      for (int i = 0; i < bytes.Length; i++)
      {
        hex += bytes[i].ToString("x2");
      }
      return hex;
    }

    private void UpdateDataView()
    {
      dg.Visible = wBtDelete.Visible = wBtProcess.Visible = true;
      DataSet ds = new DataSet();
      string xmlDirContent = QDEUtils.wsTranslationObj.GetFilesToProcess(credential);
      if (xmlDirContent != String.Empty)
      {
        System.Xml.XmlDataDocument datadoc = new System.Xml.XmlDataDocument();
        datadoc.DataSet.ReadXml(new System.IO.StringReader(xmlDirContent));
        if (datadoc.DataSet.Tables.Count == 2)
        {
          dg.DataSource = datadoc.DataSet.Tables[1];
          dg.DataBind();
          Utils.InitGridSort(ref dg, false);
        }
        else
        {
          dg.Visible = false;
        }
      }
      else
      {
        dg.Visible = false;
        wBtDelete.Visible = wBtProcess.Visible = false;
      }
      QDEUtils.wsTranslationObj.SignOff(credential);
      ds.Dispose();
    }

    private void LoadCurrentTRsToDelete()
    {
      using (TRList ds = TR.GetAll("CloseDate IS NULL"))
      {
        ddlCurrentTRs.DataMember = "Id";
        ddlCurrentTRs.DataValueField = "Id";
        ddlCurrentTRs.DataSource = ds;
        ddlCurrentTRs.DataBind();
      }
    }


    protected void btnUpload_Click(object sender, System.EventArgs e)
    {
      bodyPre.InnerText = "";
      if (txtTRUpload.HasFile)
      {
        // Call the Translation Web Service to upload the file on the shared server
        System.IO.BinaryReader br = new System.IO.BinaryReader(txtTRUpload.FileContent);
        byte[] bufArray = br.ReadBytes(Convert.ToInt32(br.BaseStream.Length));
        br.Close();
        txtTRUpload.FileContent.Close();
        bool wsUpload = false;
        Int64[] correctTRids = new Int64[0];
        Int64[] deletedTRids = new Int64[0];
        try
        {
          wsUpload = QDEUtils.wsTranslationObj.UploadZipRequest(credential, txtTRUpload.FileName, bufArray, out correctTRids, out deletedTRids);
        }
        catch
        {
          wsUpload = false;
        }
        if (!wsUpload)
        {
          bodyPre.InnerHtml += "Cannot Upload: " + QDEUtils.wsTranslationObj.GetLastError();
        }
        else
        {
          bodyPre.InnerHtml += "<fieldset><legend>File uploaded</legend>";
          bodyPre.InnerHtml += "  <b>Name:</b> " + txtTRUpload.FileName + "<br>";
          bodyPre.InnerHtml += "  <b>Size:</b> " + bufArray.Length + "<br>";
          bodyPre.InnerHtml += "  <b>Type:</b> " + txtTRUpload.PostedFile.ContentType + "<br>";
          if (correctTRids.Length > 0)
          {
            string temp = "";
            foreach (Int64 id in correctTRids)
              temp += ", " + id;
            bodyPre.InnerHtml += "  <b>Accepted:</b> " + (temp.Length > 0 ? temp.Substring(2) : "none") + "<br>";
          }
          if (deletedTRids.Length > 0)
          {
            string temp = "";
            foreach (Int64 id in deletedTRids)
              temp += ", " + id;
            bodyPre.InnerHtml += "  <b>Rejected:</b> " + (temp.Length > 0 ? temp.Substring(2) : "none") + " (deleted TRs)" + "<br>";
          }
          if (correctTRids.Length == 0 && deletedTRids.Length == 0)
            bodyPre.InnerHtml += "  <b>No TR found:</b> bad or empty zip file" + "<br>";
          bodyPre.InnerHtml += "</fieldset>";
          UpdateDataView();
          regTRFormat.Visible = false;
        }
      }
    }

    protected void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      if (!e.Row.Cells.FromKey("FileName").ToString().ToLower().EndsWith(".xml"))
      {
        e.Row.Delete();
      }
    }
    protected void BtnDeleteTR_Click(object sender, EventArgs e)
    {
      //if (txtTRId.Text == string.Empty)
      //{
      //  txtTRMessage.Text = "Please provide a TR id.";
      //  txtTRMessage.CssClass = "hc_error";
      //  txtTRMessage.Visible = true;
      //  return;
      //}
      System.Int64 TRId = -1;
      UserList usersList;
      try
      {
        TRId = Convert.ToInt64(ddlCurrentTRs.SelectedValue);
        #region Retrieve TR Users
        usersList = HyperCatalog.Business.User.GetTRNotificationUsers(TRId, SessionState.User.Id);
        #endregion
      }
      catch
      {
        txtTRMessage.Text = "The TR id you provided is not valid.";
        txtTRMessage.CssClass = "hc_error";
        txtTRMessage.Visible = true;
        return;
      }
      //HyperComponents.Data.dbAccess.Database dbObj = new HyperComponents.Data.dbAccess.Database(SessionState.CacheComponents["Crystal_DB"].ConnectionString);
      // QC 1232 CRYS : Issue when deleting TR (Increased the time-out parameter to 15 mins)
        HyperComponents.Data.dbAccess.Database dbObj = new HyperComponents.Data.dbAccess.Database(SessionState.CacheComponents["Crystal_DB"].ConnectionString, 900);
      dbObj.RunSPReturnInteger("_TR_Del",
         new System.Data.SqlClient.SqlParameter("@TRId", TRId.ToString()),
         new System.Data.SqlClient.SqlParameter("@UserId", SessionState.User.Id.ToString()),
         new System.Data.SqlClient.SqlParameter("@DeleteChunks", 1));
      if (dbObj.LastError != string.Empty)
      {
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "errTr", "<script>alert('An error occurred: " + dbObj.LastError + "');</script>");
      }
      else
      {
        #region Send mail
        if (usersList != null)
        {
          string appName = SessionState.CacheParams["AppName"].Value.ToString();
          string supportInfo = SessionState.CacheParams["SupportInfo"].Value.ToString();
          string smtpServer = SessionState.CacheParams["TREmailSmtpServer"].Value.ToString();
          string fromEmail = SessionState.CacheParams["TREmailFrom"].Value.ToString();
          string mailConfidentiality = SessionState.CacheParams["MailConfidentiality"].Value.ToString();
          string trPath = SessionState.CacheParams["TRPhysicalPath"].Value.ToString();
          HyperComponents.Net.SMTP.EmailUtil m = new HyperComponents.Net.SMTP.EmailUtil();
          m.FromEmail = fromEmail;
          m.strBody = "The tr [" + TRId.ToString() + "] has been deleted by " + SessionState.User.FullName + ".\n<br>" + txtDeleteComment.Text + "\n<br>";
          m.SmtpServer = smtpServer;
          m.Subject = "[" + appName + "] - TR [" + TRId.ToString() + "] removed";
          m.strWelcome = "Dear user,\n<br><br>\n";
          m.strSignature = "<br>Your " + appName + " system administrator<br>\n";
          m.strSignature += supportInfo.Replace(@"\n", @"<br/>");
          m.strSignature += "<hr/>\n";
          m.strSignature += "<FONT face='arial, helvetica, sans-serif' color=#5a7173 size=1>" + mailConfidentiality.Replace("\n", "<br>") + "</font>";
          foreach (User u in usersList)
          {
            m.SendEmail(u.FullName, u.Email);
          }
        }
        #endregion
        txtTRMessage.Text = "TR [" + TRId.ToString() +"] deleted successfully.";
        txtTRMessage.CssClass = "hc_success";
        txtTRMessage.Visible = true;
        LoadCurrentTRsToDelete();
      }

    }
    protected void wBtProcess_Click1(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
      HyperCatalog.WebServices.EventLoggerWS.WSEventLogger eventLog = HyperCatalog.WebServices.WSInterface.EventLogger;
      foreach (Infragistics.WebUI.UltraWebGrid.UltraGridRow dr in dg.Rows)
      {
        TemplatedColumn col = (TemplatedColumn)dr.Cells.FromKey("Select").Column;
        CheckBox cb = (CheckBox)((CellItem)col.CellItems[dr.Index]).FindControl("g_sd");
        if (cb.Checked)
        {
          int _AppComponentId = SessionState.CacheComponents["TranslationManager"].Id;
          Guid eventLogId = System.Guid.NewGuid();
          QDEUtils.wsTranslationObj.ProcessRequest(credential, eventLogId, dr.Cells.FromKey("FileName").ToString(), true);
          //QDEUtils.wsTranslationObj.BeginProcessRequest(credential, eventLogId, dr.Cells.FromKey("FileName").ToString(), true, null, null);
        }
      }
      UpdateDataView();
    }
    protected void wBtDelete_Click1(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
      lbMessage.Text = string.Empty;
      int nbDeletedRows = 0;
      foreach (Infragistics.WebUI.UltraWebGrid.UltraGridRow dr in dg.Rows)
      {
        TemplatedColumn col = (TemplatedColumn)dr.Cells.FromKey("Select").Column;
        CheckBox cb = (CheckBox)((CellItem)col.CellItems[dr.Index]).FindControl("g_sd");
        if (cb.Checked)
        {
          if (!QDEUtils.wsTranslationObj.DeleteFile(credential, dr.Cells.FromKey("FileName").ToString()))
          {
            lbMessage.Text = "Error: File [" + dr.Cells.FromKey("FileName").ToString() + "] can't be deleted<br>reason:" + QDEUtils.wsTranslationObj.GetLastError();
            lbMessage.CssClass = "hc_error";
            lbMessage.Visible = true;
            break;
          }
          else
          {
            nbDeletedRows++;
          }
        }
      }
      if (nbDeletedRows > 0)
      {
        lbMessage.Text = "File(s) deleted";
        lbMessage.CssClass = "hc_success";
        lbMessage.Visible = true;
        UpdateDataView();
      }

    }
}
}
