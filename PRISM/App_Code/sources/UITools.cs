using System;
using System.Web.UI.WebControls;

namespace HyperCatalog.UI.Admin.Architecture.Tools
{
	/// <summary>
	/// Description résumée de UITools.
	/// </summary>
	public class UITools
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
