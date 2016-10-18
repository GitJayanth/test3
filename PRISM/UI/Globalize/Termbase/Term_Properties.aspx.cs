#region Copyright (c)  Hewlett-Packard. All Rights Reserved
/* ---------------------------------------------------------------------*
*        THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE.       *
* --------------------------------------------------------------------- *
 * History Section
 * Date             Name            Description                                         Ref
 * June 03 2009     S.Balakumar     ACQ - 11 (ACQ - 8.20) - Translating Choice Lists    #ACQ8.20
 * --------------------------------------------------------------------- *
*/
#endregion


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
using HyperComponents.Data.dbAccess;
#endregion

namespace HyperCatalog.UI.Termbase
{
  /// <summary>
  /// Display the term properties (master)
  /// --> Button "Save" to create or update the term
  /// --> Button "Delete" to delete the term
  /// --> Button "List" to return to the Termbase page 
  /// </summary>
  public partial class term_properties : HCPage
  {
    #region Declarations
    protected System.Web.UI.WebControls.Panel panelUser;
    protected System.Web.UI.WebControls.RequiredFieldValidator Requiredfieldvalidator2;
    protected HyperCatalog.Business.TermType termType;
    protected long termId;
    protected Term t;
    #endregion

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
      this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);

    }
    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
    {
      #region Check Capabilities
        
      if ((SessionState.User.IsReadOnly) || (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_TERM_BASE)))
      {
        uwToolbar.Items.FromKeyButton("Save").Enabled = false;
        uwToolbar.Items.FromKeyButton("Delete").Enabled = false;
      }
      #endregion
      if (Request["t"] != null)
      {
          try
          {
              termId = Convert.ToInt64(Request["t"]);
              using (t = Term.GetByKey(termId))
              {
                  if (!Page.IsPostBack)
                  {
                      ShowTerm();
                  }
              }
          }
          catch
          {
              UITools.DenyAccess(DenyMode.Standard);
          }
      }
      else
      {
          //ACQ8.20 Starts
          try
          {
              if ("1".Equals(Convert.ToString(ApplicationSettings.Parameters["TermsIsTranslatableDefault"].Value)))
                  IsTranslatable.Checked = true;
              else
                  IsTranslatable.Checked = false;
          }
          catch (Exception ex) { }
          //ACQ8.20 Ends
      }
    }

    /// <summary>
    /// Show Term
    /// </summary>
    private void ShowTerm()
    {
      #region Load TermType list
      using (TermTypeList TermTypes = TermType.GetAll())
      {
        DDL_TermTypeList.DataSource = TermTypes;
        DDL_TermTypeList.DataBind();
      }
      #endregion

      if (t != null)
      {
        #region Retrieve information about the current container
        txtTermValue.Text = t.Value.ToString();
        using (ContainerList clist = HyperCatalog.Business.Container.GetAll("LabelId=" + termId))
        {
          if (clist.Count != 0)
          {
            txtTermValue.Enabled = false;
          }
          else { txtTermValue.Enabled = true; }
          txtComment.Text = t.Comment.ToString();
          //ACQ8.20 Ends
          if (t.TermTypeCode.ToString() == "S")
              IsTranslatable.Enabled = false;
          IsTranslatable.Checked = t.IsTranslatable;
          //ACQ8.20 Ends
          lUsage.Text = "Used " + t.UsageCount.ToString() + " time(s)";
          DDL_TermTypeList.SelectedValue = t.TermTypeCode.ToString();
          DDL_TermTypeList.Enabled = false;

          hlCreator.Text = UITools.GetDisplayName(t.User.FullName);
          hlCreator.NavigateUrl = "mailto:" + UITools.GetDisplayEmail(t.User.Email) + Server.HtmlEncode("?subject=Term[#" + t.Id.ToString() + "]"); ;
          lbOrganization.Text = t.User.OrgName;
          lbOrganization.Visible = hlCreator.Text == t.User.FullName;
          lbCreatedOn.Text = SessionState.User.FormatUtcDate(t.ModifyDate.Value, true, SessionState.User.FormatDate + ' ' + SessionState.User.FormatTime);
          UITools.ShowToolBarButton(uwToolbar, "Delete");
          UITools.ShowToolBarSeparator(uwToolbar, "SepDelete");
        }
        using (Database dbObj = Utils.GetMainDB())
        {
          using (DataSet ds = dbObj.RunSQLReturnDataSet("dbo.[_Term_GetAssociatedInputForms] " + t.Id.ToString()))
          {
            dbObj.CloseConnection();
            if (ds.Tables[0].Rows.Count > 0)
            {
              dg.DataSource = ds;
              dg.DataBind();
              fUsage.Visible = true;
            }
            else { fUsage.Visible = false; }
          }
            
        }
        #endregion
      }
      else
      {
        #region New Term
        DDL_TermTypeList.Enabled = true;
        hlCreator.Text = SessionState.User.FullName;
        hlCreator.NavigateUrl = "mailto:" + UITools.GetDisplayEmail(SessionState.User.Email) + Server.HtmlEncode("?subject=TMExpression"); ;
        lbOrganization.Text = SessionState.User.OrgName;
        lbCreatedOn.Text = SessionState.User.FormatUtcDate(DateTime.Now, true, SessionState.User.FormatDate + ' ' + SessionState.User.FormatTime);

        fUsage.Visible = false;
        UITools.HideToolBarButton(uwToolbar, "Delete");
        UITools.HideToolBarSeparator(uwToolbar, "SepDelete");
        #endregion
      }
    }

    /// <summary>
    /// Action for toolbar buttons
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="be"></param>
    private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      #region Save
      if (btn == "save")
      {
        BtnSave();
      }
      #endregion
      #region Delete
      if (btn == "delete")
      {
        BtnDelete();
      }
      #endregion
    }

    /// <summary>
    /// Save the term
    /// </summary>
    private void BtnSave()
    {
      if (t == null)
      {
        #region new term
          t = new Term(termId, txtTermValue.Text.Trim(), txtComment.Text.Trim(),Convert.ToChar(DDL_TermTypeList.SelectedValue)=='S'?true:IsTranslatable.Checked,
          Convert.ToChar(DDL_TermTypeList.SelectedValue), DDL_TermTypeList.SelectedItem.Text, 0, 0, SessionState.User.Id, DateTime.UtcNow, User.Identity.Name);
          //ACQ8.20 Added IsTranslatable value
        #endregion
      }
      else
      {
        #region Update term
        //t.TermTypeCode = Convert.ToChar(DDL_TermTypeList.SelectedValue);
        t.UserId = SessionState.User.Id;
        t.IsTranslatable = IsTranslatable.Checked; //ACQ8.20
        t.Value = txtTermValue.Text.Trim();
        t.Comment = txtComment.Text.Trim();
        #endregion
      }
      int r = t.Save();
      #region Save result
      if (r > 0)
      {
        if (termId == -1)
        {
          Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "clientScript", "<script>Redirect(" + r + ");</script>");
        }
        else
        {
          lbMessage.Text = "Data saved";
          lbMessage.CssClass = "hc_success";
          lbMessage.Visible = true;
        }
      }
      else
      {
        lbMessage.Text = Term.LastError;
        lbMessage.CssClass = "hc_error";
        lbMessage.Visible = true;
      }
      #endregion
    }

    /// <summary>
    /// Delete Term
    /// </summary>
    private void BtnDelete()
    {
      if (t != null)
      {
        #region Delete result
        if (t.Delete(HyperCatalog.Shared.SessionState.User.Id))
        {
          Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "clientScript", "<script>back();</script>");
        }
        else
        {
          lbMessage.Text = Term.LastError;
          lbMessage.CssClass = "hc_error";
          lbMessage.Visible = true;
        }
        #endregion
      }
      else
      {
        lbMessage.Text = "Error: Term can't be deleted - term not found";
        lbMessage.CssClass = "hc_error";
        lbMessage.Visible = true;
      }
    }

  }
}