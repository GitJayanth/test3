using System;
using Infragistics.WebUI.UltraWebNavigator;
using System.Data;
using System.Data.SqlClient;
using HyperCatalog.Shared;
using System.Web.UI.WebControls;

namespace HyperCatalog.UI.Login
{
  /// <summary>
  /// Description résumée de MenuBuilder.
  /// </summary>
  public class MenuBuilder
  {
    public MenuBuilder()
    {
    }

    public static void DrawMenu(ref UltraWebMenu webMenu)
    {
      webMenu.Width = Unit.Pixel(100);
      webMenu.Items.Clear();
      for (int i = 0; i < HyperCatalog.Shared.SessionState.UIMenuItems.Rows.Count; i++)
      {
        DataRow rs = HyperCatalog.Shared.SessionState.UIMenuItems.Rows[i];
        string parentId = rs["ParentId"].ToString();
        string menuId = rs["MenuId"].ToString();
        string text = rs["Text"].ToString().Trim();
        string hint = rs["Hint"].ToString().Trim();
        string url = NullToEmpty(rs["URL"].ToString(), "");
        string icon = NullToEmpty(rs["Icon"].ToString(), "");
        if (icon != string.Empty)
          icon = HCPage.LayoutURL + "/img/" + icon;
        string width = rs["Width"].ToString();
        AddItem(ref webMenu, parentId, menuId, text, hint, url, icon, width);
      }
    }

    private static Infragistics.WebUI.UltraWebNavigator.Item FindParent(ref UltraWebMenu webMenu, string parentMenuId)
    {
      for (int i = 0; i < webMenu.Items.Count; i++)
      {
        if (webMenu.Items[i].DataKey.ToString() == parentMenuId)
        {
          return webMenu.Items[i];
        }
        for (int j = 0; j < webMenu.Items[i].Items.Count; j++)
        {
          if (webMenu.Items[i].Items[j].DataKey.ToString() == parentMenuId)
          {
            return webMenu.Items[i].Items[j];
          }
        }
      }
      return null;
    }

    private static void AddItem(ref UltraWebMenu webMenu, string parentMenuId, string menuId, string menuName, string toolTip, string menuAction, string icon, string width)
    {
      Infragistics.WebUI.UltraWebNavigator.Item n, p = FindParent(ref webMenu, parentMenuId);
      n = new Infragistics.WebUI.UltraWebNavigator.Item();
      n.DataKey = menuId;
      n.Text = menuName;
      if (parentMenuId != "0")
      {
        n.ImageUrl = icon;
      }
      n.TargetUrl = menuAction;
      if (p == null)
      {
        webMenu.Items.Add(n);
        webMenu.Width = Unit.Pixel(Convert.ToInt32(webMenu.Width.Value) + Convert.ToInt32(width));
      }
      else
      {
        p.Items.Add(n);
      }
    }

    private static string NullToEmpty(string val, string defaultResult)
    {
      if (val.Equals(DBNull.Value))
      {
        return defaultResult;
      }
      else
      {
        return val;
      }
    }

  }
}
