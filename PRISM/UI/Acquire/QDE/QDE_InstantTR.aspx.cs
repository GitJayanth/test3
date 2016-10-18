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
using HyperCatalog.WebServices.TranslationWS;

namespace HyperCatalog.UI.Acquire.QDE
{
  /// <summary>
  /// Popup used for roll creation or roll deletion
  /// </summary>
  public partial class QDE_InstantTR : HCPage
  {
    #region Declarations
    protected Infragistics.WebUI.WebSchedule.WebDateChooser wdcReplacementDate;
    protected System.Web.UI.WebControls.Label lbReplacementDate;
    protected System.Web.UI.WebControls.RequiredFieldValidator Requiredfieldvalidator;
    protected System.Web.UI.WebControls.Panel pnlCreation;
    protected System.Web.UI.WebControls.Panel pnlDeletion;

    private Item item = null;
    
        //PCF Requirement -- Regional project Management (Deepak.S)
        private string cultureCode;

    protected System.Web.UI.WebControls.Label lbAreYouSure;
    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
    {
      if (SessionState.User.IsReadOnly)
      {
        uwToolbar.Items.FromKeyButton("Apply").Enabled = false;
      }

      try
      {
        Int64 itemId = Convert.ToInt64(Request["i"]);
        
        cultureCode = Convert.ToString(Request["c"]); //PCF Requirement -- Regional project Management (Deepak.S)
        
        if (SessionState.User.HasItemInScope(itemId)) 
        {
          item = HyperCatalog.Business.Item.GetByKey(itemId);
          if (item != null)
          {
            lbError.Visible = false;
            if (!Page.IsPostBack)
            {
              lbTitle.Text = item.FullName + " - Translate";
            }
          }
          else
            UITools.DenyAccess(DenyMode.Popup);
        }
        else
          UITools.DenyAccess(DenyMode.Popup);
      }
      catch
      {
        UITools.JsCloseWin();
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
      this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);

    }
    #endregion

    private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      if (btn.Equals("apply"))
      {
        CreateTR();
      }
    }

    private void CreateTR()
    {
      WSTranslation ws = HyperCatalog.WebServices.WSInterface.Translation;
      CreateInstantTRDelegate dlgt =
              new CreateInstantTRDelegate(ws.CreateInstantTR);
      string credential = ws.SignOn(SessionState.User.Pseudo, SessionState.User.ClearPassword);
      if (credential != string.Empty)
      {
        // call BeginInvoke to initiate the asynchronous operation
        //PCF Requirement -- Regional project Management (Deepak.S)
        IAsyncResult ar =
          dlgt.BeginInvoke(credential, Guid.NewGuid(), item.Id, cultureCode, true, txtComment.Text, string.Empty, null, null);

        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "TRDone", "<script>OnTRCreatedUpdateParent(" + item.Id + ");</script>");
        ws.SignOff(credential);
      }
      else
      {
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "TRDone", "<script>alert('Sorry, the system was not able to connect to the web service. Please contact support');window.close();</script>");
      }
    }
    // define the delegate
    //PCF Requirement -- Regional project Management (Deepak.S)
    delegate bool CreateInstantTRDelegate(string credential, Guid eventLogId, long itemId, string cultureCode, bool notifyByEmail, string reason, string comment);
  }
}
