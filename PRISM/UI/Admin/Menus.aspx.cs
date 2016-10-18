#region uses
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HyperComponents.Data.dbAccess;
using System.Data.SqlClient;
using System.Text;
using HyperCatalog.Business;
#endregion

namespace HyperCatalog.UI.Admin
{
	/// <summary>
	/// Display list of menu
	/// </summary>
	public partial class Menus : HCPage
	{
		#region Declarations
		#endregion

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

    protected void Page_Load(object sender, System.EventArgs e)
    {
      if (!Page.IsPostBack)
      {
        UpdateDataView();
      }
    }

    private void UpdateDataView()
    {
      using (Database dbObj = Utils.GetMainDB())
      {
        using (IDataReader rs = dbObj.RunSPReturnRS("_Menus_GetMap"))
        {
          StringBuilder s = new StringBuilder();
          if (dbObj.LastError == string.Empty)
          {
            bool notEof = rs.Read();
            if (notEof)
            {
              s.Append("<table border=1 CELLSPACING=0 CELLPADDING=0 style='border-collapse:collapse;'>");
              s.Append("<TH></TH><TH></TH><TH>Description</TH>");
              while (notEof)
              {
                s.Append(DrawItem(Convert.ToInt32(rs["Level"]), rs["Text"].ToString(), rs["Description"].ToString(), rs["Icon"].ToString()));
                notEof = rs.Read();
              }
              s.Append("</TABLE>");
            }
            lbMenus.Text = s.ToString();
            rs.Close();
            dbObj.CloseConnection();
          }
        }
      }
    }

    private string DrawItem(int level, string text, string description, string iconName)
    {
      string row = "\n\t<TR>";
      if (level==1)
      {
        row += "\n\t\t<TD width=135>\n\t\t\t<table border=0 class=level1Item><tr><td>" + text + "</td></tr></table>\n\t\t</td>\n\t\t<td width=135>&nbsp;</td>";
      }
      if (level==2)
      {
        row += "\n\t\t<TD width=135>\n\t\t<table border=0 class=level2Item><tr><td>";
        if (iconName!=string.Empty)
        {
          row += "<img style='float:left;margin-left:2;' src='/hc_v4/img/" + iconName + "'>";
        }
        row += "<div style='margin-top:2;margin-left:25px'>"+ text +"</div></td></tr></table></td><td width=135>&nbsp;</td>";
      }
      if (level==3)
      {
        row += "\n\t\t<TD width=135>&nbsp;</td>\n\t\t<TD width=135>\n\t\t\t<table border=0 class=level2Item><tr><td>";
        if (iconName!=string.Empty)
        {
          row += "<img style='float:left;margin-left:2;' src='/hc_v4/img/" + iconName + "'/>";
        }
        row += "<div style='margin-top:2;margin-left:25px'>"+ text +"</div></td></tr></table>\n\t\t</td>";
      }
      row += "\n\t\t<td width=* style='font-size:8pt'>" + description + "</td>\n\t</tr>";
      return row;
    }

  }
}
