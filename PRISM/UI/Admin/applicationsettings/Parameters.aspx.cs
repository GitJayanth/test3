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

namespace HyperCatalog.UI.Admin.Architecture
{
	/// <summary>
	/// Description résumée de Parameters.
	/// </summary>
	public partial class Parameters : HCPage
	{
    #region UI

    protected Business.CapabilitiesEnum updateCapability = Business.CapabilitiesEnum.MANAGE_USERS;
    #endregion

    #region Business objects
    protected HyperCatalog.Business.ApplicationParameterCollection applicationParameters = HyperCatalog.Business.ApplicationSettings.Parameters;
    #endregion

    protected override void OnLoad(EventArgs e)
    {
      UITools.CheckConnection(Page);
      msgLbl.Visible = false;

      if (!IsPostBack)
      {
        parameters.DataSource = applicationParameters.Keys;
        parameters.DataBind();
      }

      base.OnLoad (e);
    }

    protected override void OnPreRender(EventArgs e)
    {
      mainToolBar.Items.FromKeyButton("Save").Enabled = !HyperCatalog.Shared.SessionState.User.IsReadOnly && HyperCatalog.Shared.SessionState.User.HasCapability(updateCapability);

      base.OnPreRender (e);
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
      mainToolBar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(mainToolBar_ButtonClicked);
		}
    #endregion

    #region Events
    private void mainToolBar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      switch (be.Button.Key)
      {
        case "Save":
          if (!HyperCatalog.Shared.SessionState.User.IsReadOnly && HyperCatalog.Shared.SessionState.User.HasCapability(updateCapability))
          {
            for (int i=0; i<parameters.Items.Count; i++)
            {
              Label lbParameterName = (Label)parameters.Items[i].FindControl("lbParameterName");
              TextBox txtParameterValue = (TextBox)parameters.Items[i].FindControl("txtParameterValue");
              if (lbParameterName!=null && txtParameterValue!=null)
                applicationParameters[lbParameterName.Text].Value = txtParameterValue.Text.Equals(string.Empty)?null:txtParameterValue.Text;
            }
            if (applicationParameters.SaveWithTrace(HyperCatalog.Shared.SessionState.User.Id))
              Tools.UITools.SetMessage(msgLbl,"Parameters updated.",Tools.UITools.MessageLevel.Information);
            else
              Tools.UITools.SetMessage(msgLbl,"Parameters could not be saved.",Tools.UITools.MessageLevel.Error);
          }
          else
            Tools.UITools.SetMessage(msgLbl,"You are not allowed to modify parameters.",Tools.UITools.MessageLevel.Warning);

          break;
      }
    }
    #endregion
  }
}
