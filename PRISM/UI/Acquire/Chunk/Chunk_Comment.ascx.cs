#region uses
using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
#endregion

namespace HyperCatalog.UI.Acquire.Chunk
{
  /// <summary>
  /// Description résumée de Chunk_Comment.
  /// </summary>
  public partial class Chunk_Comment : System.Web.UI.UserControl
  {
    private HyperCatalog.Business.Chunk _Chunk=null;
    
    public HyperCatalog.Business.Chunk Chunk
    {
      get{ return _Chunk;}
      set{ _Chunk = value;ShowComment();}
    }

    public string Comment
    {
      get{ return txtComment.Text;}
    }

    protected void Page_Load(object sender, System.EventArgs e)
    {
    }
    private void ShowComment()
    {
      if (_Chunk != null)
      {
        if (!IsPostBack)
                    txtComment.Text = _Chunk.Comment;
                else
                    _Chunk.Comment = txtComment.Text;
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
    ///		Méthode requise pour la prise en charge du concepteur - ne modifiez pas
    ///		le contenu de cette méthode avec l'éditeur de code.
    /// </summary>
    private void InitializeComponent()
    {

    }
    #endregion
  }
}