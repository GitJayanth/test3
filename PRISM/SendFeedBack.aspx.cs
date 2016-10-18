#region "Uses"
using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using HyperCatalog.Business;
using HyperComponents.Data.dbAccess;
using HyperCatalog.Shared;
#endregion

	/// <summary>
	/// Summary description for DashBoard_Edit.
	/// </summary>
public partial class SendFeedBack : HCPage
{
  #region declaration
  #endregion
  
  protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
  
  protected void Page_Load(object sender, System.EventArgs e)
  {
    if (!Page.IsPostBack)
    {
      ClassificationList.Items.Add("Low");
      ClassificationList.Items.Add("Medium");
      ClassificationList.Items.Add("Serious");
      ClassificationList.Items.Add("Critical");
      Label1.Text = SessionState.User.FullName;
      Label2.Text = SessionState.User.FormatUtcDate(DateTime.Now, true, SessionState.User.FormatDate + ' ' + SessionState.User.FormatTime);
      string pageName = SessionState.User.LastVisitedPage.ToString();
      pageName = pageName.Substring(pageName.LastIndexOf('/')+1);
      txtCurrentPage.Text = pageName;
      }
  }
    
  #region Web Form Designer generated code
  override protected void OnInit(EventArgs e)
  {
    // CODEGEN: This call is required by the ASP.NET Web Form Designer.
    //
    InitializeComponent();
    base.OnInit(e);
  }
		
  /// <summary>
  /// Required method for Designer support - do not modify
  /// the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent()
  {    
    this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);

  }
  #endregion

  private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
  {
    if (be.Button.Key == "Submit")
    {
      if (SaveFeedback(HyperCatalog.Shared.SessionState.User.Id,txtComment.Text,"page.aspx",ClassificationList.SelectedIndex)==1)
        lblSubmitResult.Text = "Your feedback has been registered, you can now close this window.";
      else
        lblSubmitResult.Text = "This feedback could not be registered, please try later.";
      UITools.HideToolBarButton(uwToolbar, "Submit");
    }
  }
  private int SaveFeedback( int CreatorId, string Feedback ,string Page, int Classification)
  {
    using (Database dbObj = new Database(SessionState.CacheComponents["Crystal_UI"].DatabaseComponent.ConnectionString))
    {
      string sSql = "INSERT INTO Feedbacks(CreateDate, CreatorId, Comment, Page, Classification) VALUES(getutcdate(), " + CreatorId + ", '" + Feedback + "', '" + SessionState.User.LastVisitedPage.ToString() + "', " + Classification + ")";
      dbObj.RunSQL(sSql);
      dbObj.CloseConnection();
      if (dbObj.LastError != string.Empty)
        return -1;
      else
        return 1;
    }
  }

 

}

