namespace HyperCatalog.UI.AjaxControls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
  using System.Web.UI;
  using System.Web.UI.WebControls;
  using System.Web.UI.HtmlControls;

	/// <summary>
	/// Description résumée de LookUp.
	/// </summary>
	public partial class LookUp : System.Web.UI.UserControl
	{

    // [ValidationProperty("Value")]

    public string name = null;
    public string pageproperty = String.Empty;

    public string lookupservice = null;
    public bool nosubmit = false;
    public string Value = null;
    public string Width = "200";


    protected override void OnPreRender(EventArgs e) 
    {
      base.OnPreRender(e);

      if (this.name == null) this.name = this.ID;

      // register the JavaScripts includes without need for a Form.
      if (!Page.ClientScript.IsClientScriptBlockRegistered("CommonBehaviour")) 
      {
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"CommonBehaviour", "<script type='text/javascript' src='"
          + Page.ResolveUrl("~/ajaxcontrols/jcl.js")
          + "'><" + "/script>\n");
      } // if

      if (!Page.ClientScript.IsClientScriptBlockRegistered("MyBehaviour")) 
      {
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"MyBehaviour", "<script type='text/javascript' src='"
          + Page.ResolveUrl("~/ajaxcontrols/LookUp.js")
          + "'><" + "/script>\n");
      } // if
    } // OnPreRender


    // ----- implement IPostBackDataHandler Members to enable embedding into ASP.NET Forms -----

    public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection) 
    {
      String postedValue = postCollection[postDataKey];

      if ((Value == null) || (Value != postedValue)) 
      {
        Value = postedValue;
        return true;
      }
      return false;
    } // LoadPostData


    public void RaisePostDataChangedEvent() 
    {
      // no PostDataChangedEvent will be raised.
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
