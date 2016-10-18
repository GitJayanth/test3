#region Uses
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HyperCatalog.Business;
using HyperCatalog.Shared;
using HyperComponents.Data.dbAccess;
using System.Data.SqlClient;
using Infragistics.WebUI.UltraWebGrid;
#endregion

public partial class UI_Globalize_TR_TR_Properties : HCPage
{
  #region Declarations
  private string t = null;
  #endregion

  protected void Page_Load(object sender, EventArgs e)
  {
    #region Capabilities
    if (SessionState.User.IsReadOnly || (!SessionState.User.HasCapability(CapabilitiesEnum.DELETE_TR_MTR_CTR)))
    {
      uwToolbar.Items.FromKeyButton("Delete").Enabled = uwToolbar.Items.FromKeyButton("DeleteLanguages").Enabled = false;
    }
    #endregion

    try
    {
      if (Request["tr"] != null)
      {
        t = Request["tr"].ToString();
      }
      if (!Page.IsPostBack)
      {
        UpdateDataView();
      }
    }
    catch (Exception fe)
    {
      string error = fe.ToString();
      UITools.DenyAccess(DenyMode.Popup);
    }

    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "InitCommentField", "<script>commentField='" + txtDeleteComment.ClientID + "'</script>");
  }

  private void UpdateDataView()
  {
    if (t != null)
    {
      using (TR tr = TR.GetByKey(Convert.ToInt64(t)))
      {
        if (tr != null)
        {
          if (tr.CloseDate != null)
          {
            uwToolbar.Items.FromKeyButton("Delete").Enabled = false;
            uwToolbar.Items.FromKeyButton("DeleteLanguages").Enabled = false;
          }
          txtId.Text = tr.Id.ToString();
          txtName.Text = tr.Item.Name;
          //txtRegionCode.Text = "eu-en";
          txtRegionCode.Text = tr.RegionCode;
          txtCreateDate.Text = tr.CreationDate.ToString();
          txtComment.Text = tr.Comment;
          txtType.Text = tr.Type.ToString();
          cbInstant.Checked = tr.InstantTR;
          txtInstantComment.Text = tr.InstantTRComment;
          using (Database dbObj = Utils.GetMainDB())
          {
            using (DataSet ds = dbObj.RunSPReturnDataSet("_TR_GetLanguagesInfo", new SqlParameter("@TRId", tr.Id)))
            {
              if (dbObj.LastError == string.Empty)
              {
                dgLanguages.DataSource = ds.Tables[0];
                dgLanguages.DataBind();
                dgLanguages.Visible = true;
                if (ds.Tables[0].Rows.Count == 1)
                {
                  dgLanguages.Columns.FromKey("Select").Hidden = true;
                  uwToolbar.Items.FromKeyButton("DeleteLanguages").Enabled = false;
                }
                dgLanguages.Columns.FromKey("EndOfTranslation").Format = SessionState.User.FormatDate;
                hlCreator.Text = tr.Creator.FullName;
                lbOrganizationCreator.Text = tr.Creator.Organization.Name;
                lError.Visible = false;
              }
              else
              {
                lError.Visible = true;
                lError.Text = dbObj.LastError;
              }
            }
          }
        }
        else
        {
          lError.Text = "[error] No TR found !";
          lError.Visible = true;
        }
      }
    }
    else
    {
      lError.Text = "[error] No TRId found !";
      lError.Visible = true;
    }
  
  }
  protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
  {
    #region delete TR
    if (be.Button.Key.ToLower() == "delete")
    {
        //Modified the function - Adding parameter to this function for QC - 6162 by Nisha
        DeleteTR(false);
    }
    #endregion
    //Added the new funtion to delete the TR with Chunks for QC - 6162 by Nisha
    #region delete TR with Chunks
    if (be.Button.Key.ToLower() == "deletechunks")
    {
        DeleteTR(true);
    }
    #endregion
    #region Delete TR Languages
    if (be.Button.Key.ToLower() == "deletelanguages")
    {
      DeleteTRLanguages();
    }
    #endregion
  }

  private void DeleteTRLanguages()
  {
    lError.Text = string.Empty;
    int nbDeletedRows = 0;
    string languagesDeleted = string.Empty;
    if (t != null)
    {
      using (TR tr = TR.GetByKey(Convert.ToInt64(t)))
      {
        #region Retrieve TR Users
        UserList usersList = HyperCatalog.Business.User.GetTRNotificationUsers(tr.Id, SessionState.User.Id);
        #endregion
        foreach (Infragistics.WebUI.UltraWebGrid.UltraGridRow dr in dgLanguages.Rows)
        {
          TemplatedColumn col = (TemplatedColumn)dr.Cells.FromKey("Select").Column;
          CheckBox cb = (CheckBox)((CellItem)col.CellItems[dr.Index]).FindControl("g_sd");
          if (cb.Checked)
          {
            languagesDeleted += dr.Cells.FromKey("Language").Value + ",";
            if (!tr.DeleteLanguage(dr.Cells.FromKey("LanguageCode").Value.ToString(), SessionState.User.Id))
            {
              lError.Text = "Error: TR [" + tr.Id.ToString() + "] Language [" + dr.Cells.FromKey("LanguageCode").Value.ToString() + "] can't be deleted<br>" + TR.LastError;
              lError.CssClass = "hc_error";
              lError.Visible = true;
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
            m.strBody = (nbDeletedRows == 1 ? "The <b>" + languagesDeleted.Substring(0, languagesDeleted.Length - 1) + "</b> language for the tr [" + tr.Id + "] has been deleted by " + SessionState.User.FullName + ".\n<br>" + txtDeleteComment.Text + "\n<br>" : "The <b>" + languagesDeleted.Substring(0, languagesDeleted.Length - 1) + "</b> languages for the tr [" + tr.Id + "] have been deleted by " + SessionState.User.FullName + ".\n<br>" + txtDeleteComment.Text + "\n<br>");
            m.SmtpServer = smtpServer;
            m.Subject = "[" + appName + "] - Languages removed for the TR [" + tr.Id + "]";
            m.strWelcome = "Dear user,\n<br><br>\n";
            m.strSignature = "<br>Your " + appName + " system administrator<br>\n";
            m.strSignature += supportInfo.Replace(@"\n", @"<br/>");
            m.strSignature += "<hr/>\n";
            m.strSignature += "<FONT face='arial, helvetica, sans-serif' color=#5a7173 size=1>" + mailConfidentiality.Replace("\n", "<br>") + "</font>";
            foreach (User u in usersList)
            {
                try
                {
                    m.SendEmail(u.FullName, u.Email);
                }
                catch (Exception e)
                {

                }
            
            }
          }
          #endregion
          lError.Text = "Data deleted";
          lError.CssClass = "hc_success";
          lError.Visible = true;
          UpdateDataView();
        }
      }
    }
    else
    {
      lError.Text = "[error] No TRId found !";
      lError.Visible = true;
    }
  }

  //Modified the function - Adding parameter to this function for QC - 6162 by Nisha
  private void DeleteTR(bool delChunks)
  {
    if (t != null)
    {
      using (TR tr = TR.GetByKey(Convert.ToInt64(t)))
      {
        #region Retrieve TR Users
        UserList usersList = HyperCatalog.Business.User.GetTRNotificationUsers(tr.Id, SessionState.User.Id);
        #endregion
        if (tr.confirmDelete(SessionState.User.Id, delChunks))
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
            m.strBody = "The tr [" + tr.Id + "] has been deleted by " + SessionState.User.FullName + ".\n<br>" + txtDeleteComment.Text + "\n<br>";
            m.SmtpServer = smtpServer;
            m.Subject = "[" + appName + "] - TR [" + tr.Id + "] removed";
            m.strWelcome = "Dear user,\n<br><br>\n";
            m.strSignature = "<br>Your " + appName + " system administrator<br>\n";
            m.strSignature += supportInfo.Replace(@"\n", @"<br/>");
            m.strSignature += "<hr/>\n";
            m.strSignature += "<FONT face='arial, helvetica, sans-serif' color=#5a7173 size=1>" + mailConfidentiality.Replace("\n", "<br>") + "</font>";
            foreach (User u in usersList)
            {
                try
                {
                    m.SendEmail(u.FullName, u.Email);
                }
                catch (Exception e)
                {

                }
            
            }
          }
          #endregion
          Page.ClientScript.RegisterStartupScript(this.GetType(), "reloadParent", "<script>ReloadParent();</script>");
        }
        else
        {
          lError.Text = "[error] Impossible to delete TR";
          lError.Visible = true;
        }
      }
    }
    else
    {
      lError.Text = "[error] No TRId found !";
      lError.Visible = true;
    }
  }

}
