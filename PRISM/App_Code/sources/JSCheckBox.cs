using System;

using System.Web.UI.WebControls;

namespace HyperCatalog.UI.WebControls
{
	/// <summary>
	/// Description résumée de JSCheckBox.
	/// </summary>
	public class JSCheckBox : CheckBox
	{
    public string JSOnClick;

    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {
      if (JSOnClick!=null)
      {
        if (this.Attributes["onclick"]==null)
          this.Attributes.Add("onclick",JSOnClick);
        else
          this.Attributes["onclick"] += JSOnClick;
      }
      base.Render (writer);
    }

	}
}
