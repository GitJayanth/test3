namespace HyperCatalog.UI.DAM.UserControls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	/// Description résumée de CustomPager.
	/// </summary>
	public partial class CustomPager : System.Web.UI.UserControl
	{

    public DataGrid Grid;
    protected int PageCount 
    {
      get
      {
        return Grid.PageCount;
      }
    }

    protected int CurrentPageIndex 
    {
      get
      {
        return Grid.CurrentPageIndex;
      }
    }


    protected int PageGroupCount = 5;
    protected int PageGroupIndex
    {
      get
      {
        return (int)Math.Ceiling((double)(Grid.CurrentPageIndex/PageGroupCount));
      }
    }

    private int[] _groupPages = null;
    protected int[] GroupPages
    {
      get
      {
        if (_groupPages==null)
        {
          int pageCount = Math.Min(PageGroupCount,Grid.PageCount - PageGroupIndex * PageGroupCount);
          if (pageCount>=0)
          {
            int offset = PageGroupIndex * PageGroupCount;
            if (PageGroupIndex>0)
            {
              offset -= PageGroupCount - pageCount;
              pageCount = PageGroupCount;
            }
            _groupPages = new int[pageCount];
            for (int i=0;i<pageCount;i++)
              _groupPages[i] = offset + i;
          }
          else
            _groupPages = new int[0];
        }
        return _groupPages;
      }
    }

    private int[] _allPages = null;
    protected int[] AllPages
    {
      get
      {
        if (_allPages==null)
        {
          int pageCount = Grid.PageCount;
          _allPages = new int[pageCount];
          for (int i=0;i<pageCount;i++)
            _allPages[i] = i+1;
        }
        return _allPages;
      }
    }

    public event EventHandler PageChanged;

    protected override void OnPreRender(EventArgs e)
    {
      Visible = pageList.Visible = true;
      DataBind();
      Grid.Visible = (Grid.Items.Count>0);
      Visible = (PageCount>1);
      pageList.Visible = (pageList.Items.Count > PageGroupCount);
      pageList.SelectedValue = (CurrentPageIndex+1).ToString();

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
		///		Méthode requise pour la prise en charge du concepteur - ne modifiez pas
		///		le contenu de cette méthode avec l'éditeur de code.
		/// </summary>
		private void InitializeComponent()
		{
      pageList.SelectedIndexChanged += new EventHandler(pageList_SelectedIndexChanged);
    }
		#endregion

    protected void pagerClick(object sender, CommandEventArgs e)
    {
      switch (e.CommandName)
      {
        case "GotoPage":
          ChangePage(Convert.ToInt32(e.CommandArgument));
          break;
        case "GotoGroup":
          ChangePage(Convert.ToInt32(e.CommandArgument) * PageGroupCount);
          break;
      }
    }

    public void ChangePage(int pageIndex)
    {
      Grid.CurrentPageIndex = pageIndex;
      Grid.SelectedIndex = -1;
      Grid.EditItemIndex = -1;
      throwPageChanged();
    }

    protected void throwPageChanged()
    {
      if (PageChanged!=null)
        PageChanged(this,EventArgs.Empty);
    }

    private void pageList_SelectedIndexChanged(object sender, EventArgs e)
    {
      ChangePage(Convert.ToInt32(pageList.SelectedValue)-1);
    }


  }
}
