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
using Infragistics.WebUI.UltraWebNavigator;
using HyperCatalog.Shared;
using System.Data.SqlClient;
using HyperCatalog.Business;

public partial class Translate_tvaspx : HCPage
{
	#region Protected vars
	#endregion

	#region Privates vars
	//private string itemId = string.Empty;
	#endregion

	protected void Page_Load(object sender, System.EventArgs e)
	{
    if (SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_TRANSLATION_SETTING))
    {		
			if (!Page.IsPostBack)
			{
				if (Request["i"]!=null)
					SelectedItem.Value = Request["i"].ToString();
				else
					SelectedItem.Value = "-1";
			}

			InitTreeView();
		}
		else
			UITools.DenyAccess(DenyMode.Frame);
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
	/// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
	/// le contenu de cette méthode avec l'éditeur de code.
	/// </summary>
	private void InitializeComponent()
	{    
		this.webTree.NodeBound += new Infragistics.WebUI.UltraWebNavigator.NodeBoundEventHandler(this.webTree_NodeBound);

	}
	#endregion

  private void InitTreeView()
  {
    // get in database
    SqlParameter[] parameters = {new SqlParameter("@UserId", SessionState.User.Id.ToString()),
										new SqlParameter("@RequiredOnly", 1),
										new SqlParameter("@SelectedItemId", -1),
										new SqlParameter("@CultureCode", SessionState.Culture.Code),
										new SqlParameter("@Type", 3)}; // Localisation type 
    using (Database dbObj = Utils.GetMainDB())
    {
      using (DataSet ds = dbObj.RunSPReturnDataSet("dbo.PM_RetrieveProjects", "", parameters))
      {

        // get last error if necessary
        if (dbObj.LastError != string.Empty)
        {
          Response.Write(dbObj.LastError);
          Response.End();
        }

        if ((ds != null) && (ds.Tables[0].Rows.Count > 0))
        {
          // Add relations
          try
          {
            ds.Relations.Add("ItemDatesParents", ds.Tables[0].Columns["itemId"], ds.Tables[1].Columns["ParentId"], false);
          }
          catch (System.Exception x)
          {
            Response.Write("step1->" + x.Message);
            Response.End();
          }
          try
          {
            ds.Relations.Add("ItemParents", ds.Tables[1].Columns["itemId"], ds.Tables[1].Columns["ParentId"], false);
          }
          catch (System.Exception x)
          {
            Response.Write("step2->" + x.Message);
            Response.End();
          }
          webTree.ClearAll();
          webTree.DataSource = ds.Tables[0].DefaultView;
          webTree.Levels[0].RelationName = "ItemDatesParents";
          webTree.Levels[0].ColumnName = "ItemNameAndCountMissingTrans";
          webTree.Levels[0].LevelKeyField = "ItemId";
          webTree.Levels[0].TargetFrameName = "ItemName";
          webTree.Levels[0].ImageColumnName = "ImgPriority";
          for (int i = 1; i < 10; i++)
          {
            webTree.Levels[i].RelationName = "ItemParents";
            webTree.Levels[i].ColumnName = "ItemNameAndCountMissingTrans";
            webTree.Levels[i].LevelKeyField = "ItemId";
            webTree.Levels[i].TargetFrameName = "ItemName";
          }
          webTree.DataMember = ds.Tables[0].TableName;
          webTree.DataBind();

          if (SelectedItem.Value != "-1")
          {
            bool isFound = false; // found node
            //Expand tree on selected item
            for (int j = 0; j < webTree.Nodes.Count; j++)
            {
              Node currentNode = webTree.Nodes[j];
              isFound = isFound || ExpandNode(currentNode);
            }
            if (!isFound)
            {
              SelectedItem.Value = "-1";
              webTree.Nodes[0].Expand(false);
            }
            // Select item
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "loadGrid", "<script>loadGrid(" + SelectedItem.Value + ");</script>");
          }
          else
            webTree.Nodes[0].Expand(false);
        }
      }
    }
  }

	private bool ExpandNode(Node node)
	{
		string itemId = node.DataKey.ToString();
		if (itemId.Equals(SelectedItem.Value))
		{
			node.Expand(false);
			webTree.SelectedNode = node;
			return true;
		}
		else
		{
			if(node.Nodes.Count>0) // has children
			{
				for(int i=0; i<node.Nodes.Count; i++)
				{
					if (ExpandNode(node.Nodes[i]))
					{
						node.Expand(false);
						return true;
					}
				}
			}
		}
		return false;
	}

	private void webTree_NodeBound(object sender, Infragistics.WebUI.UltraWebNavigator.WebTreeNodeEventArgs e)
	{
		//*************************************************************
		// Copy Item Id to node Tag so that Client script can access it
		//*************************************************************
		e.Node.Tag = e.Node.DataKey.ToString();

		//*************************************************************
		// Clean Nodes text (remove common text with parents)
		//*************************************************************
		Infragistics.WebUI.UltraWebNavigator.Node n = e.Node;
		Utils.CleanNodeText(ref n);
	}
}
