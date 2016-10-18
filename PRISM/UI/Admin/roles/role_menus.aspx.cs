#region uses
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HyperComponents.Data.dbAccess;
using Infragistics.WebUI.UltraWebGrid;
using HyperCatalog.Business;
#endregion

/// <summary>
/// Display menu list attached with selected role 
///		--> Return to the role list
///		--> Select menu to attach to the selected role
/// </summary>
public partial class role_menus : HCPage
{
	#region Declarations
	protected System.Web.UI.WebControls.Button BtnCancel;

	private string roleId;
	#endregion

	#region Code généré par le Concepteur Web Form
	override protected void OnInit(EventArgs e)
	{
		InitializeComponent();
		base.OnInit(e);
	}
    
	/// <summary>
	/// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
	/// le contenu de cette méthode avec l'éditeur de code.
	/// </summary>
	private void InitializeComponent()
	{    
		this.webTree.NodeBound += new Infragistics.WebUI.UltraWebNavigator.NodeBoundEventHandler(this.webTree_NodeBound);
		this.webTree.NodeChecked += new Infragistics.WebUI.UltraWebNavigator.NodeCheckedEventHandler(this.webTree_NodeChecked);

	}
	#endregion

	protected void Page_Load(object sender, System.EventArgs e)
	{
		if (Request["r"]!=null)
			roleId = Request["r"].ToString();

		if (!Page.IsPostBack)
		{        
			InitTreeView();
			UpdateDataView();
			webTree.Nodes[0].Expand(false);
		}
	}

	private void UpdateDataView()
	{
    if (roleId != String.Empty)
    {
      using (Database dbObj = Utils.GetMainDB())
      {
        using (DataSet ds = dbObj.RunSQLReturnDataSet("SELECT M.MenuId, M.Text as MenuText FROM Menus M JOIN RoleMenus R ON R.MenuId = M.MenuId AND R.RoleId = " + roleId, "RoleMenus"))
        {
          if (dbObj.LastError.Length > 0)
          {
            lbError.CssClass = "hc_error";
            lbError.Text = dbObj.LastError;
            lbError.Visible = true;
          }
          else
          {
            if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
            {
              dg.DataSource = ds.Tables[0].DefaultView;
              Utils.InitGridSort(ref dg);
              dg.DataBind();

              dg.Visible = true;
            }
            else
            {
              lbError.CssClass = "hc_error";
              lbError.Text = "Nothing in role scope...";
              lbError.Visible = true;
              dg.Visible = false;
            }
            if (ds != null)
              ds.Dispose();
          }
        }
      }
    }
	}
  
	private void InitTreeView()
	{
    if (roleId != String.Empty)
    {
      using (Database dbObj = Utils.GetMainDB())
      {
        DataSet ds = dbObj.RunSPReturnDataSet("_Role_GetAllMenus", "Menus", new SqlParameter("@RoleId", roleId));
        if (dbObj.LastError.Length > 0)
        {
          lbError.CssClass = "hc_error";
          lbError.Text = dbObj.LastError;
          lbError.Visible = true;
        }
        else
        {
          try
          {
            ds.Relations.Add("MenuParents", ds.Tables[0].Columns["MenuId"], ds.Tables[0].Columns["ParentId"]);

            webTree.DataSource = ds;
            webTree.Levels[0].ColumnName = "MenuText";
            webTree.Levels[0].RelationName = "MenuParents";
            for (int i = 1; i < 4; i++)
            {
              webTree.Levels[i].RelationName = "MenuParents";
              webTree.Levels[i].ColumnName = "MenuText";
              webTree.Levels[i].LevelKeyField = "MenuId";
              webTree.Levels[i].CheckboxColumnName = "RoleScope";
            }
            webTree.DataMember = "Menus";
            webTree.DataBind();
            for (int i = 1; i < webTree.Nodes.Count; i++)
            {
              webTree.Nodes[i].Hidden = webTree.Nodes[i].Level == 0;
            }
            if (ds != null)
              ds.Dispose();
          }
          catch (Exception ex)
          {
            lbError.CssClass = "hc_error";
            lbError.Text = ex.Message;
            lbError.Visible = true;
          }
        }
      }
    }
	}

	private void webTree_NodeChecked(object sender, Infragistics.WebUI.UltraWebNavigator.WebTreeNodeCheckedEventArgs e)
	{
		if(e.Node.Checked == true) 
		{
			//e.Node.Expanded = true;
			RecurseNodes(e.Node, true);
			AddMenuInScope(e.Node.DataKey.ToString());
		}
		else
		{
			RecurseNodes(e.Node, false);
			RemoveMenuFromScope(e.Node.DataKey.ToString());
		}    
		UpdateDataView();
	}
	private void RecurseNodes(Infragistics.WebUI.UltraWebNavigator.Node n, bool check)
	{
		for(int i = 0; i < n.Nodes.Count; i++) 
		{
			if (check) 
			{
				if (n.Nodes[i].Checked)
				{
					RemoveMenuFromScope(n.Nodes[i].DataKey.ToString());
				}
				n.Nodes[i].CheckBox = Infragistics.WebUI.UltraWebNavigator.CheckBoxes.False;
			}
			else
			{
				n.Nodes[i].CheckBox = Infragistics.WebUI.UltraWebNavigator.CheckBoxes.True;
			}
			RecurseNodes(n.Nodes[i], check);
			n.Nodes[i].Checked = check;
		}
	}

	private void webTree_NodeBound(object sender, Infragistics.WebUI.UltraWebNavigator.WebTreeNodeEventArgs e)
	{
		if (e.Node.Parent!=null)
		{
			e.Node.Text = e.Node.Text;
			if (e.Node.Parent.Checked)
			{
				e.Node.Checked = true;
				e.Node.CheckBox = Infragistics.WebUI.UltraWebNavigator.CheckBoxes.False;
			}
			else
			{
				e.Node.CheckBox = Infragistics.WebUI.UltraWebNavigator.CheckBoxes.True;
			}
			if (e.Node.Checked)
			{
				Infragistics.WebUI.UltraWebNavigator.Node p = e.Node.Parent;
				while (p!=null)
				{
					if (p.Level > 0)
					{
						//p.Expand(false);
						p = p.Parent;
					}
					else
					{
						p=null;
					}
				}
			}
		}
	}
    
	private void  AddMenuInScope(string menuId)
	{
		lbError.Visible = false;
		lbError.Text = string.Empty;

    using (Database dbObj = Utils.GetMainDB())
    {
      dbObj.RunSQLReturnRS("IF NOT EXISTS (SELECT MenuId FROM RoleMenus WHERE RoleId = " + roleId + " AND MenuId=" + menuId + ") INSERT INTO RoleMenus(RoleId, MenuId) VALUES(" + roleId + ", " + menuId + ")");
      if (dbObj.LastError.Length > 0)
      {
        lbError.CssClass = "hc_error";
        lbError.Text += "Error updating scope: " + dbObj.LastError + "<br/>";
        lbError.Visible = true;
      }
    }
	}
  
	private void  RemoveMenuFromScope(string menuId)
	{
		lbError.Visible = false;
		lbError.Text = string.Empty;

    using (Database dbObj = Utils.GetMainDB())
    {
      dbObj.RunSQLReturnRS("DELETE FROM RoleMenus WHERE RoleId = " + roleId + " AND MenuId=" + menuId);
      if (dbObj.LastError != string.Empty)
      {
        lbError.CssClass = "hc_error";
        lbError.Text += "Error updating scope: " + dbObj.LastError + "<br/>";
        lbError.Visible = true;
      }
    }
	}
}
