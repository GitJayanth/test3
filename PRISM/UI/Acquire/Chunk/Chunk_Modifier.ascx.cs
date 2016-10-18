#region uses
using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HyperCatalog.Shared;
#endregion

namespace HyperCatalog.UI.Acquire.Chunk
{

	/// <summary>
	/// Description résumée de Chunk_Modifier.
	/// </summary>
  public partial class Chunk_Modifier : System.Web.UI.UserControl
  {
    private HyperCatalog.Business.Chunk _Chunk=null;
    
    public HyperCatalog.Business.Chunk Chunk
    {
      get{ return _Chunk;}
      set{ _Chunk = value;ShowUser();}     
    }

		protected void Page_Load(object sender, System.EventArgs e)
		{      
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
		///		Méthode requise pour la prise en charge du concepteur - ne modifiez pas
		///		le contenu de cette méthode avec l'éditeur de code.
		/// </summary>
		private void InitializeComponent()
		{

    }
		#endregion

    private void ShowUser()
    {
      if (_Chunk!=null)
      {
        hlCreator.Text =  UITools.GetDisplayName(_Chunk.User.FullName);
        HyperCatalog.Business.Item item = HyperCatalog.Business.Item.GetByKey(_Chunk.ItemId);
        hlCreator.NavigateUrl = "mailto:" + UITools.GetDisplayEmail(_Chunk.User.Email) + "?subject=" + Server.UrlEncode(((string)("Chunk \"" + _Chunk.ContainerName + "\" [" + _Chunk.ContainerId + "] for product {" + item.Name + " " + item.Sku +"}")).Replace(" ","_")).Replace("_","%20");
        lbOrganization.Text = _Chunk.User.Organization.Name;
        lbOrganization.Visible = UITools.GetDisplayEmail(_Chunk.User.Email) == _Chunk.User.Email;
        if (SessionState.User != null)
        {
          lbCreatedOn.Text = SessionState.User.FormatUtcDate(_Chunk.ModifyDate.Value, true, SessionState.User.FormatDate + ' ' + SessionState.User.FormatTime);
        }
        else
        {
          lbCreatedOn.Text = _Chunk.ModifyDate.Value.ToLongDateString();
        }
      }
    }
	}
}
