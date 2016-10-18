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
using HyperCatalog.Business;
using HyperCatalog.Shared;
#endregion

#region History
// Update "save" and "delete" buttons are "read only" (CHARENSOL Mickael 24/10/05)
#endregion

/// <summary>
/// Display container group properties
///		--> Return to the list of container group
///		--> Save new or modified container group
///		--> Delete container group
/// </summary>
public partial class containergroup_properties : HCPage
{
	#region Declarations
	protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
  protected System.Web.UI.WebControls.Label Label4;
  protected System.Web.UI.WebControls.DropDownList DropDownList1;
  protected System.Web.UI.WebControls.ImageButton ImageButton1;

	private int groupId = -1;
  int maxDepth = Convert.ToInt32(SessionState.CacheParams["ContainerGroup_MaxNestedLevel"].Value);

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
    this.btExpand.Click += new System.Web.UI.ImageClickEventHandler(this.btExpand_Click);

  }
	#endregion


  protected void Page_Load(object sender, System.EventArgs e)
  {
    if (HyperCatalog.Shared.SessionState.User.IsReadOnly)
    {
      uwToolbar.Items.FromKeyButton("Save").Enabled = false;
      uwToolbar.Items.FromKeyButton("Delete").Enabled = false;
    }

    if (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_DICTIONARY))
    {
      UITools.HideToolBarButton(uwToolbar, "Save");
      UITools.HideToolBarSeparator(uwToolbar, "SaveSep");
      UITools.HideToolBarButton(uwToolbar, "Delete");
      UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
    }

    panelParent.Visible = true;
    if (Request["d"] != null)
    {
      groupId = Convert.ToInt32(Request["d"]);
      panelId.Visible = groupId != -1;
    }
    if ((Request.Form["__EventTarget"] == "Path") && (Request.Form["__EventArgument"] != string.Empty))
    {
      BuildParent(Convert.ToInt32(Request.Form["__EventArgument"]));
    }

    if (!Page.IsPostBack)
    {
      UpdateDataView();
    }
  }
		
	private void UpdateDataView()
	{
		if (groupId == -1)
		{
			// add new container group
      panelId.Visible = false;
			txtGroupId.Enabled = true;   
      BuildParent(-1);

			UITools.HideToolBarButton(uwToolbar, "Delete");
			UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
		}
		else
		{
			// update container group
			ContainerGroup group = ContainerGroup.GetByKey(groupId);

			if (group != null)
			{
				txtGroupId.Text = group.Id.ToString();
        txtGroupId.Enabled = false;
        txtGroupCode.Text = group.Code;
        txtGroupName.Text = group.Name;
        //txtGroupName.Enabled = group.AbsoluteContainersCount == 0;
        BuildParent(group.ParentId);
			}
			else
			{
				lbError.CssClass = "hc_error";
				lbError.Text = "Error: Container group is null";
				lbError.Visible = true;
			}
		}
	}

	private void Save()
	{
		ContainerGroup group = null;
    ContainerGroup groupParent = ContainerGroup.GetByKey(Convert.ToInt32(ddlParentList.SelectedValue));
    string path = string.Empty;
    if (groupParent != null)
    {
      path = groupParent.Path + groupParent.Name + "/";
    }
    if (path.IndexOf(txtGroupName.Text + "/") > 0)
    {
      lbError.CssClass = "hc_error";
      lbError.Text = "Error: Container group has already a parent with the same name";
      lbError.Visible = true;
    }
    else
    {
      if (txtGroupId.Enabled)
      {
        group = new ContainerGroup(-1, Convert.ToInt32(ddlParentList.SelectedValue), txtGroupCode.Text, txtGroupName.Text, string.Empty, 0, 0, 0);
      }
      else
      {
        group = ContainerGroup.GetByKey(Convert.ToInt32(txtGroupId.Text));
        if (group != null)
        {
          group.Code = txtGroupCode.Text;
          group.Name = txtGroupName.Text;
        }
        group.ParentId = Convert.ToInt32(ddlParentList.SelectedValue);
      }

      if (group != null)
      {
        if (group.Save(txtGroupId.Enabled))
        {
          lbError.Text = "Data saved!";
          lbError.CssClass = "hc_success";
          lbError.Visible = true;
          SessionState.ClearAppContainerGroups();

          // Create new container group
          //Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"clientScript", "<script>back();</script>"); 
        }
        else
        {
          lbError.CssClass = "hc_error";
          lbError.Text = ContainerGroup.LastError;
          lbError.Visible = true;
        }
      }
      else
      {
        lbError.CssClass = "hc_error";
        lbError.Text = "Error: Container group is null";
        lbError.Visible = true;
      }
    }
 	}
    
	private void Delete()
	{
		ContainerGroup group = ContainerGroup.GetByKey(Convert.ToInt32(txtGroupId.Text));
		if (group != null)
		{
			if (group.Delete(HyperCatalog.Shared.SessionState.User.Id))
			{
        SessionState.ClearAppContainerGroups();
				Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"clientScript", "<script>back();</script>");
			}
			else
			{
				lbError.CssClass = "hc_error";
				lbError.Text = ContainerGroup.LastError;
				lbError.Visible = true;
			}
		}
		else
		{
			lbError.CssClass = "hc_error";
			lbError.Text = "Error: Container group is null";
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
		if (btn == "delete")
		{
			Delete();
		}
	}

  protected void ddlParentList_SelectedIndexChanged(object sender, System.EventArgs e)
  {
    Trace.Warn("ddlParentList_SelectedIndexChanged,ddlParentList.SelectedValue=" +ddlParentList.SelectedValue.ToString());
    if (ddlParentList.SelectedValue != "-1")
    {
      int nbParents = Convert.ToInt32(ViewState["nbParents"]);
      Trace.Warn("nbParents=" + ViewState["nbParents"].ToString());
      Trace.Warn("maxDepth=" + maxDepth.ToString());
      if (nbParents < maxDepth)
      {
        Trace.Warn("nbParents < maxDepth");
        // Retrieve the parent group
        int parentId = Convert.ToInt32(ddlParentList.SelectedValue);
        ContainerGroup cgParent = ContainerGroup.GetByKey(parentId);
        int nbChilds = cgParent.Childs.Count;
        foreach (ContainerGroup cg in cgParent.Childs)
        {
          if (cg.Id == groupId)
          {
            nbChilds--;
            break;
          }
        }
        btExpand.Visible = nbChilds > 0;
        Trace.Warn("nbChilds=" + nbChilds.ToString());
      }
      else
      {
        btExpand.Visible = false;
      }    
      Trace.Warn("btExpand.Visible=" + btExpand.Visible.ToString());
    }      
  }

  private void btExpand_Click(object sender, System.Web.UI.ImageClickEventArgs e)
  {
    int parentId = Convert.ToInt32(ddlParentList.SelectedValue);
    Trace.Warn("btExpand_Click and parentId = " + parentId.ToString());
    if (parentId >=0)
    {
      BuildParent(ContainerGroup.GetByKey(parentId).Childs[0].Id);
    }
    else
    {
      using (ContainerGroupList cgAll = ContainerGroup.GetAll("ContainerGroupParentId = -1"))
      {
        BuildParent(cgAll[0].Id);
      }
    }
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="parentId"></param>
  /// <param name="id"></param>
  /// Can move or change an container group even if it has children: Decision business review 09/05/06
  private void BuildParent(int parentId)
  {
    Trace.Warn("BuildParent(" + parentId.ToString() +")");
    Trace.Warn("  GroupId = " + groupId.ToString());
    ViewState["nbParents"] = 0;
    lParentName.Text = string.Empty;
    bool bCanMove = true;
    ContainerGroup cg = ContainerGroup.GetByKey(groupId);
    // Retrieve container based on parentId parameter
    ContainerGroup cgComboDisplayLevel = ContainerGroup.GetByKey(parentId);
    ContainerGroupList cgChildren = null;     
    ddlParentList.Items.Clear();
    if (cgComboDisplayLevel == null)
    {
      if (cg != null)
      {
        Trace.Warn("  cgComboDisplayLevel is null [#maxDepth =" + maxDepth.ToString() + ", cg.ChildDepth=" + cg.ChildDepth.ToString() + "]");
      }
      else
      {
        Trace.Warn("  cgComboDisplayLevel is null [#maxDepth =" + maxDepth.ToString() + ", cg is null]");
      }
      ddlParentList.Items.Clear();
      ddlParentList.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Root", "-1"));
      if (cg != null)
      {
        btExpand.Visible = maxDepth > cg.ChildDepth;
      }
      else
      {
        btExpand.Visible = maxDepth > 0;
      }
    }
    else
    {
      if (cg != null)
      {
        Trace.Warn("[cg.ChildDepth=" + cg.ChildDepth.ToString() + "]");
      }
      if (cgComboDisplayLevel.Parent == null)
      {
        using (ContainerGroupList cgAll = ContainerGroup.GetAll("ContainerGroupParentId = -1 and ContainersCount=0 AND ContainerGroupId <> " + groupId.ToString()))
        {
          cgChildren = cgAll;
        }
      }
      else
      {
        cgChildren = cgComboDisplayLevel.Parent.Childs;
      }
      // If container group has children
      if (cgChildren != null && cgChildren.Count > 0)
      {
        ddlParentList.DataSource = cgChildren;
        ddlParentList.DataBind();
        // Remove the current item from the drop down list
        ddlParentList.Items.Remove(ddlParentList.Items.FindByValue(txtGroupId.Text));
        ddlParentList.Visible = ddlParentList.Items.Count > 0;
        if (cg == null) // User is creating a new container group
        {
          ddlParentList.SelectedIndex = 0;
        }
        else
        {
          // Select the current parent
          if (ddlParentList.Items.FindByValue(cg.ParentId.ToString()) != null)
          {
            ddlParentList.SelectedValue = cg.ParentId.ToString();
          }
        }
        int nbChilds = cgChildren[0].Childs.Count;
        foreach (ContainerGroup cgc in cgChildren[0].Childs)
        {
          if (cgc.Id == groupId)
          {
            nbChilds--;
            break;
          }
        }
        btExpand.Visible = nbChilds > 0;
      }
      #region maxDepth
      // If the maxDepth < 0 impossible to add child to a container group
      if (maxDepth <= 0)
      {
        panelParent.Visible = false;
      }
      else
      {
        if (cgComboDisplayLevel != null)
        {
          string[] parents = cgComboDisplayLevel.Path.Split('/');
          int nbParents = parents.Length - 1;
          ViewState["nbParents"] = nbParents;
          if (btExpand.Visible)
          {
            btExpand.Visible = nbParents < maxDepth;
          }
        }
      }
      #endregion
      #region Build Label path
      ContainerGroup cgParent = cgComboDisplayLevel.Parent;
      while (cgParent != null)
      {
        lParentName.Text = bCanMove ? "<A HREF=javascript:__doPostBack('Path','" + cgParent.Id.ToString() + "')>" + cgParent.Name + "</A>/" + lParentName.Text : cgParent.Name + "/" + lParentName.Text;
        if (cgParent.ParentId != -1) { cgParent = cgParent.Parent; }
        else { cgParent = null; }
      }
      lParentName.Text = bCanMove ? "<A HREF=javascript:__doPostBack('Path','-1')>Root</A>/" + lParentName.Text : "Root/" + lParentName.Text;
      if (parentId != -1) { lParentName.Visible = true; }
      else { lParentName.Visible = false; }
      #endregion
    }
  }
}
