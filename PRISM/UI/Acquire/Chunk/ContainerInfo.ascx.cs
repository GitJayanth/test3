namespace HyperCatalog.UI.Acquire.Chunk
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	/// Description r�sum�e de ContainerInfo.
	/// </summary>
	public partial class ContainerInfo : System.Web.UI.UserControl
	{

		private int _ContainerId = -1;
    protected HyperCatalog.Business.Container container;

    
    public int ContainerId
    {
      set
      {
        _ContainerId = value;
        if (HyperCatalog.Shared.SessionState.QDEContainer != null && HyperCatalog.Shared.SessionState.QDEContainer.Id == _ContainerId)
        {
          container = HyperCatalog.Shared.SessionState.QDEContainer;
        }
        else
        {
          container = HyperCatalog.Business.Container.GetByKey(_ContainerId);
        }
        Page.DataBind();
      }
      get { return _ContainerId;}
    }
    
    protected void Page_Load(object sender, System.EventArgs e)
		{
		}

		#region Code g�n�r� par le Concepteur Web Form
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN�: Cet appel est requis par le Concepteur Web Form ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		M�thode requise pour la prise en charge du concepteur - ne modifiez pas
		///		le contenu de cette m�thode avec l'�diteur de code.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
