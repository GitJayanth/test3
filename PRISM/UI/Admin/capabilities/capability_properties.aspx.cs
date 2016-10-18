#region uses
using System;
using System.Xml;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HyperCatalog.Shared;
using HyperCatalog.Business;
using System.Data.SqlClient;
#endregion

#region History
	// Buttons "save" and "delete" read only (CHARENSOL 24/10/2005)
#endregion

/// <summary>
/// Display capability properties
///		--> Return to the list
///		--> Save new or modified capability
/// </summary>
public partial class capability_Properties : HCPage
{
	#region Declarations
	
	private int capabilityId = -1;
	Capability capability = null;
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
	/// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
	/// le contenu de cette méthode avec l'éditeur de code.
	/// </summary>
	private void InitializeComponent()
	{    
		this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);

	}
	#endregion

	protected void Page_Load(object sender, System.EventArgs e)
	{
		if (SessionState.User.IsReadOnly)
		{
			uwToolbar.Items.FromKeyButton("Save").Enabled = false;
		}

		try
		{
			if (Request["r"] != null)
				capabilityId = Convert.ToInt32(Request["r"]);

			if (!Page.IsPostBack)
			{
				UpdateDataView();
			}

		}
		catch
		{
			Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"clientScript", "<script>back();</script>");  
		}
	}
	    
  protected void UpdateDataView()
  {
    if (capabilityId >= 0)
    {
      capability = Capability.GetByKey(capabilityId);
      if (capability != null)
      {
        //txtCapabilityId.Text = capabilityId.ToString();
        txtCapabilityName.Text = capability.Name;
        txtDescription.Text = capability.Description;

        Page.DataBind();
      }
      else
      {
        lbError.CssClass = "hc_error";
        lbError.Text = "Error: Capability is null";
        lbError.Visible = true;
      }
    }
    else
    {
      Page.DataBind();
    }
    UpdateTreeView();
  }

  private void UpdateTreeView()
  {
    using (CollectionView vMenuList = new CollectionView(HyperCatalog.Business.Menu.GetAll()))
    {
      for (int i = 0; i < vMenuList.Count; i++)
      {
        HyperCatalog.Business.Menu m = (HyperCatalog.Business.Menu)vMenuList[i];
        if (m.ParentId <= 0 && m.Active)
        {
          AddNode(null, m);
        }
      }
    }
  }
    private void AddNode(Infragistics.WebUI.UltraWebNavigator.Node entryNode, HyperCatalog.Business.Menu m)
  {
    if (m.Active)
    {
      bool isChecked = false;
      foreach (HyperCatalog.Business.Menu cMenu in capability.Menus)
      {
        if (cMenu.Id == m.Id)
        {
          isChecked = true;
          break;
        }
      }
      Infragistics.WebUI.UltraWebNavigator.Node n = new Infragistics.WebUI.UltraWebNavigator.Node();
      n.DataKey = m.Id;
      n.Text = m.Text;
      n.Checked = isChecked;
      n.TargetUrl = "javascript://";
      if (entryNode == null)
      {
        entryNode = uwMenu.Nodes.Add(n);
      }
      else
      {
        if (m.Icon != string.Empty)
        {
          n.ImageUrl = "/hc_v4/img/" + m.Icon;

        }
        entryNode = entryNode.Nodes.Add(n);
      }
      foreach (HyperCatalog.Business.Menu subMenu in m.Childs)
      {
        if (m.Active)
        {
          AddNode(entryNode, subMenu);
        }
      }
    }
  }

  private void Save()
  {
    // Update capability menus
    capability = Capability.GetByKey(capabilityId);
    capability.Menus.Clear();
    XmlDocument t_SourceTreeData = new XmlDocument();
    uwMenu.WriteXmlDoc(t_SourceTreeData, true, false);
    XmlNodeList t_TreeNodeList = t_SourceTreeData.SelectNodes("//Node[Checked]");
     
    XmlNode node = null;
    for (int i=0;i<t_TreeNodeList.Count;i++)
    {
      node = t_TreeNodeList[i].SelectSingleNode("./DataKey");
      if (node!=null)
      {
          capability.Menus.Add(new HyperCatalog.Business.Menu(Convert.ToInt32(node.InnerText)));
      }
    }
    if (capability.Save())
    {
      lbError.Text = "Data saved !";
      lbError.CssClass = "hc_success";
      lbError.Visible = true;
    }
    else
    {
      lbError.Text = "Save failed";
      lbError.CssClass = "hc_Error";
      lbError.Visible = true;
    }    
  }
	private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
	{
		string btn = be.Button.Key.ToLower();
		if (btn == "save")
		{
			Save();
		}
	}
}
