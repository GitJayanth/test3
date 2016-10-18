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
#endregion

	/// <summary>
	/// Display the expression properties (master)
	/// --> Button "Save" to create or update the expression
	/// --> Button "List" to return to the Termbase page 
	/// </summary>
public partial class expression_properties : HCPage
{
  #region Declarations
  protected System.Web.UI.WebControls.Panel panelUser;
  protected System.Web.UI.WebControls.Label Label2;
  protected System.Web.UI.WebControls.Label Label3;
  protected System.Web.UI.WebControls.Label Label11;
  protected System.Web.UI.WebControls.Label Label1;
  protected System.Web.UI.WebControls.RequiredFieldValidator Requiredfieldvalidator2;
  protected HyperCatalog.Business.TermType termType;
  protected System.Web.UI.WebControls.DropDownList DDL_TermTypeList;
  protected System.Web.UI.WebControls.Label lTermType;
  private long expressionId;
  private TMExpression t;
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
    if ((SessionState.User.IsReadOnly) || (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_TRANSLATION_MEMORY)))
    {
      uwToolbar.Items.FromKeyButton("Save").Enabled = false;
      uwToolbar.Items.FromKeyButton("Delete").Enabled = false;

      txtTMValue.Enabled = txtComment.Enabled = !SessionState.User.IsReadOnly;
    }
    #endregion
    if (Request["e"] != null)
    {
      expressionId = Convert.ToInt64(Request["e"]);    
      if (!Page.IsPostBack)
      {
        try
        {
          using (t = TMExpression.GetByKey(expressionId))
          {
            ShowExpression();
          }
        }
        catch
        {
          UITools.DenyAccess(DenyMode.Standard);
        }
      }
    }
  }

  /// <summary>
  /// Show TM Expression
  /// </summary>
  private void ShowExpression()
  {
    if (t != null)
    {
      #region Retrieve information about the current container
      txtTMValue.Text = t.Value.ToString();
      txtComment.Text = t.Comment.ToString();

      hlCreator.Text = UITools.GetDisplayName(t.User.FullName);
      hlCreator.NavigateUrl = "mailto:" + UITools.GetDisplayEmail(t.User.Email) + Server.HtmlEncode("?subject=TMExpression[#" + t.Id.ToString() +"]"); ;
      lbOrganization.Text = t.User.OrgName;
      lbOrganization.Visible = hlCreator.Text == t.User.FullName;
      lbCreatedOn.Text = SessionState.User.FormatUtcDate(t.ModifyDate.Value, true, SessionState.User.FormatDate + ' ' + SessionState.User.FormatTime);

      #endregion
    }
    else
    {
      #region Add new TM Expression
      hlCreator.Text =  SessionState.User.FullName;
      hlCreator.NavigateUrl = "mailto:" + UITools.GetDisplayEmail(SessionState.User.Email) + Server.HtmlEncode("?subject=TMExpression");;
      lbOrganization.Text = SessionState.User.OrgName;
      lbCreatedOn.Text = SessionState.User.FormatUtcDate(DateTime.Now, true, SessionState.User.FormatDate + ' ' + SessionState.User.FormatTime);
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
    if ( btn == "save")
    {
      BtnSave();
    }
    #endregion
    #region Delete
    if ( btn == "delete")
    {
      BtnDelete();
    }
    #endregion
  }

  /// <summary>
  ///  Save the TM expression
  /// </summary>
  private void BtnSave()
  {
    if (t == null)
    {
      #region New TM Expression
      t = new TMExpression(expressionId, txtTMValue.Text.Trim(), txtComment.Text.Trim(), 
        0, 0, SessionState.User.Id, DateTime.UtcNow, User.Identity.Name);
      #endregion 
    }
    else
    {
      #region Update TM Expression
      t.UserId = SessionState.User.Id;
      t.Value = txtTMValue.Text.Trim();
      t.Comment = txtComment.Text.Trim();
      #endregion
    }
    int r = t.Save();
    #region Save result
    if (r > 0)
    {
      lbMessage.Text = "Data saved";
      lbMessage.CssClass = "hc_success";
      lbMessage.Visible = true;
    }
    else
    {
      lbMessage.Text = TMExpression.LastError;
      lbMessage.CssClass = "hc_error";
      lbMessage.Visible = true;
    }
    #endregion
  }

  /// <summary>
  /// Delete the TM expression
  /// </summary>
  private void BtnDelete()
  {
    using (t = TMExpression.GetByKey(expressionId))
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
          lbMessage.Text = TMExpression.LastError;
          lbMessage.CssClass = "hc_error";
          lbMessage.Visible = true;
        }
        #endregion
      }
      else
      {
        lbMessage.Text = "Error: TM Expression can't be deleted - Expression not found";
        lbMessage.CssClass = "hc_error";
        lbMessage.Visible = true;
      }
    }
  }


}     

