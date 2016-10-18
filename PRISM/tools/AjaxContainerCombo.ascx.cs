namespace HyperCatalog.Web.Components
{
	using System;
	using System.Data;
	using System.Drawing;
  using System.Web;
  using System.Web.UI;
  using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	/// Description résumée de AjaxCombo.
	/// </summary>
	public partial class AjaxContainerCombo : System.Web.UI.UserControl
	{

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

    private DateTime timestamp;
    public int maxRecords
    {
      set{ViewState["MaxRecords"] = value;}
      get{ return ViewState["MaxRecords"]==null?10:(int)ViewState["MaxRecords"];}
    }
    public string Tag
    {
      get {return WebCombo1.DisplayValue;}
      set {WebCombo1.DisplayValue = value;}
    }
    public HyperCatalog.Business.Container Container
    {
      get {return HyperCatalog.Business.Container.GetByTag(Tag);}
    }
    protected void Page_Load(object sender, System.EventArgs e)
    {
      //We'll need this js file.  WebCombo doesn't normally do 'adds', so it doesn't import this automatically.
      Infragistics.WebUI.Shared.Util.ClientScript.ResolveAndRegisterClientScriptInclude("/ig_common/20053/scripts/ig_webgrid_an.js","","/ig_common/20053/scripts/ig_webgrid_an.js","ig_webgrid_an.js",this.WebCombo1,"ig_webgrid_an");
      Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"HyperCatalogAjaxCombo", "	<script type='text/javascript' src='/hc_v4/js/ajaxcombo.js' ></script>");
      Infragistics.WebUI.Shared.Util.ClientScript.ResolveAndRegisterClientScriptInclude("AjaxCombo.js","","AjaxCombo.js","AjaxCombo.js",this.WebCombo1,"HyperCatalogAjaxCombo");
      ((Infragistics.WebUI.UltraWebGrid.UltraWebGrid)this.WebCombo1.Controls[0]).DisplayLayout.ClientSideEvents.BeforeRowActivateHandler="BeforeRowActivated";
      WebCombo1.DropDownLayout.DropdownHeight = Unit.Pixel(Convert.ToInt32(WebCombo1.DropDownLayout.RowHeightDefault.Value) * (maxRecords + 1) + 25); 
      if (WebCombo1.DropDownLayout.DropdownHeight.Value > 220)
      {
        WebCombo1.DropDownLayout.DropdownHeight = Unit.Pixel(220);
      }
      if (Tag != null)
      {
        WebCombo1.DisplayValue = Tag;
      }
    }
    
    public void Process()
    {
      //Check if we have a serch, or if this is a normal page_load
      string search=this.Request.QueryString["searchString"];
      string id=this.Request.QueryString["id"];
      if (id != null)
      {
        if (id.StartsWith(this.UniqueID)) // Mayeb another control is calling us
        {
          //Is this a TypeAhead request, or do we want to return the details for a specific order?
          bool returnRows=this.Request.QueryString["rows"]=="true";
          if(search!=null)
          {
            this.Response.Clear();
            string data=GetMatches(search,returnRows);
            if(returnRows)this.Response.ContentType="text/xml";
            this.Response.Write(data);
            this.Response.Flush();
            this.Response.Close();
          }
        }
      }
    }

    private string GetMatches(string search, bool returnRows)
    {
      System.Text.StringBuilder builder=new System.Text.StringBuilder();
      if(search!=null)
      {
        search=search.Trim();
        //Let's validate the text, be sure that someone didn't enter a non-numeric character.
        this.Response.ContentType="text/xml";               
        //Open our connection, get ready to query
        timestamp=DateTime.Now;
        using (HyperCatalog.Business.ContainerList containers = HyperCatalog.Business.Container.GetAll("Tag like '" + search + "%'"))
        {
          //We want to return all details for the specific OrderID
          //Set up our Select command
          builder.Append("<data><rows>");
          int nbRows = 0;
          foreach (HyperCatalog.Business.Container c in containers)
          {
            //Use a string builder for speed.  Create xml for response stream
            builder.Append(String.Format("<row tag=\"{0}\" name=\"{1}\" />", new object[] { HttpUtility.HtmlEncode(c.Tag), "[" + HttpUtility.HtmlEncode(c.Tag) + "] - " + HttpUtility.HtmlEncode(c.Name) }));
            nbRows++;
            if (nbRows == maxRecords)
            {
              break;
            }
          }
          builder.Append("</rows>");
          builder.AppendFormat("<stats queryTime=\"{0}\" results=\"{1}\" ></stats>", ((double)new TimeSpan(DateTime.Now.Ticks - timestamp.Ticks).TotalMilliseconds) / 1000, containers.Count);
          builder.Append("</data>");
        }
      }
      return builder.ToString();

    }
	}
}
