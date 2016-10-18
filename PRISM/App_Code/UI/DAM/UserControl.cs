using System;
using System.Web.UI.WebControls;

namespace HyperCatalog.UI.DAM
{
	/// <summary>
	/// Description résumée de UserControl.
	/// </summary>
	public class UserControl : System.Web.UI.UserControl
	{
    public enum MessageLevel : int
    {
      Information = 0,
      Warning = 1,
      Error = 2
    }

    private static string[] cssClassLevels = {"lbl_msgInformation","lbl_msgWarning","lbl_msgError"};
    public static void SetMessage(Label label, string text, MessageLevel level)
    {
      label.Text = text;
      label.CssClass = cssClassLevels[(int)level];
      label.Visible = true;
    }

  }
}
