using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using System.Data.SqlClient;
using Infragistics.WebUI.UltraWebGrid;
using Infragistics.WebUI.UltraWebToolbar;

using HyperCatalog.Business.DAM;
using HyperCatalog.Shared;
using HyperComponents.Data.dbAccess;

namespace HyperCatalog.UI.DAM.Controls
{
  /// <summary>
  /// Description résumée de LibraryList.
  /// </summary>
  public partial class LibraryList : UserControl
  {
    #region Business objects
    public int WorkspaceId
    {
      get
      {
        object _workspaceId = ViewState["workspaceId"];
        if (_workspaceId!=null && _workspaceId is int)
          return (int)_workspaceId;
        else
          try
          {
            _workspaceId = Convert.ToInt32(Request["w"]);
            WorkspaceId = (int)_workspaceId;
            return (int)_workspaceId;
          }
          catch (FormatException)
          {}
          catch (OverflowException)
          {}
        return -1;
      }
      set
      {
        ViewState["workspaceId"] = value;
      }
    }

    protected string filter
    {
      get
      {
        TextBox txtFilter = (TextBox)mTb.FindControl("txtFilter");
        return (txtFilter!=null?txtFilter.Text:string.Empty);
      }
      set
      {
        TextBox txtFilter = (TextBox)mTb.FindControl("txtFilter");
        if(txtFilter!=null)
          txtFilter.Text = value;
      }
    }

    private LibraryCollection _dataSource;
    protected LibraryCollection dataSource
    {
      get
      {
        if (_dataSource==null)
          _dataSource = (LibraryCollection)Library.GetAll(WorkspaceId).SetFilter(filter);
        return _dataSource;
      }
    }

    private Library _selectedLibrary;
    protected Library selectedLibrary
    {
      get
      {
        if (_selectedLibrary==null)
        {
          string libraryName = (string)ViewState["selectedLibraryName"];
          if (libraryName!=null)
            _selectedLibrary = dataSource[libraryName];
        }
        return _selectedLibrary;
      }
      set
      {
        ViewState["selectedLibraryName"] = value!=null?value.Name:null;
        _selectedLibrary = value;

        txtResourceTypeValue.Enabled = value==null;
        txtNameValue.Text = value != null ? value.Name : "";
        txtDescriptionValue.Value = value!=null?value.Description:"";
        txtTypeValue.ClearSelection();
        txtVersionValue.Checked = true;
        maxVersionsUnlimited.Checked = true;
        txtStorageValue.ClearSelection();
        if (value!=null)
        {
          txtTypeValue.SelectedValue = value.IsInternal?"1":"0";
          txtVersionValue.Checked = value.VersioningEnabled;
          maxVersionsUnlimited.Checked = value.MaxVersionCount<=0;
          maxVersionsValue.ValueInt = value.MaxVersionCount;
          if (value.IsInternal)
            txtStorageValue.SelectedValue = value.BinaryStorage.ToString();

          Session["DAM_currentLibraryId"] = value.Id;
        }
        txtPathValue.Text = value!=null?value.RelativePath:"";
      }
    }

    protected ResourceType[] resourceTypes = ResourceType.GetAll();
    #endregion

    protected override void OnLoad(EventArgs e)
    {
      UITools.CheckConnection(Page);
      if (!SessionState.User.HasCapability(Business.CapabilitiesEnum.MANAGE_RESOURCES))
      {
        Response.End();
        return;
      }

      propertiesMsgLbl.Visible = false;

      foreach (ResourceType resourceType in resourceTypes)
        txtResourceTypeValue3.Groups[0].Items.Add(new ListItem(resourceType.Name,resourceType.Id.ToString()));
 
      if (!IsPostBack)
      {
        txtResourceTypeValue.DataSource = resourceTypes;
        txtResourceTypeValue.DataBind();
        resourceTypeBoxes.DataSource = resourceTypes;
        resourceTypeBoxes.DataBind();
        resourceTypesAttributes.DataSource = resourceTypes;
        resourceTypesAttributes.DataBind();

        BindGrid();

        int _selectedLibraryId = -1;
        if (Request["l"]!=null)
          _selectedLibraryId = Convert.ToInt32(Request["l"]);
        if (_selectedLibraryId<0 && Session["DAM_currentLibraryId"] is int)
          _selectedLibraryId = (int)Session["DAM_currentLibraryId"];
        if (_selectedLibraryId>=0)
        {
          for (int itemIndex=0;itemIndex<libraryGrid.DataKeys.Count;itemIndex++)
            if (((int)libraryGrid.DataKeys[itemIndex]) == _selectedLibraryId)
            {
              chooseLibrary(itemIndex);
              break;
            }
        }
      }

      base.OnLoad (e);
    }

    protected override void OnPreRender(EventArgs e)
    {
      if (!IsPostBack && libraryGrid.SelectedIndex<0)
        chooseLibrary(libraryGrid.Items.Count>0?0:-1);

      if (selectedLibrary == null || selectedLibrary.ResourceCount > 0)
      {
        UITools.HideToolBarButton(pTb, "Delete");
        UITools.HideToolBarSeparator(pTb, "DeleteSep");
      }
      else
      {
        UITools.ShowToolBarButton(pTb, "Delete");
        UITools.ShowToolBarSeparator(pTb, "DeleteSep");
      }
  
      txtTypeValue.Enabled = selectedLibrary==null;
      txtPathValue.Enabled = txtStorageValue.Enabled = selectedLibrary==null || selectedLibrary.ResourceCount==0;

      pTb.Items.FromKeyButton("Save").Enabled = mTb.Items.FromKeyButton("Add").Enabled = !SessionState.User.IsReadOnly;

      base.OnPreRender (e);
    }

    protected override void Render(HtmlTextWriter writer)
    {
      base.Render(writer);

      //to be rendered after the infragistics initialisation
      Page.ClientScript.RegisterStartupScript(Page.GetType(), "dhtml", "typeChanged(document.getElementById(\"" + txtTypeValue.ClientID + "\"));", true);
    }


    #region Code généré par le Concepteur Web Form
    override protected void OnInit(EventArgs e)
    {
      //
      // CODEGEN : Cet appel est requis par le Concepteur Web Form ASP.NET.
      //
      InitializeComponent();
      ID = "ResourceList";
      base.OnInit(e);
    }
		
    /// <summary>
    /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
    /// le contenu de cette méthode avec l'éditeur de code.
    /// </summary>
    private void InitializeComponent()
    {
      this.mTb.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.mainToolbar_ButtonClicked);
      this.pTb.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.propertiesToolbar_ButtonClicked);
      this.resourceTypesAttributes.ItemCreated += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.resourceTypesAttributes_ItemCreated);

      this.librariesPagerTop.Grid = this.libraryGrid;
      this.librariesPagerTop.PageChanged += new EventHandler(librariesPager_PageChanged);
      this.librariesPagerBottom.Grid = this.libraryGrid;
      this.librariesPagerBottom.PageChanged += new EventHandler(librariesPager_PageChanged);
    }
    #endregion

    #region Data load & bind
    protected void BindGrid()
    {
      using (Database dbObj = new Database(SessionState.CacheComponents["DAM_DB"].ConnectionString))
      {
        string sqlQuery = " SELECT L.* " +
          " FROM VIEW_DAM_Libraries L " +
          " LEFT OUTER JOIN DAM_LibraryRepositories LW ON L.LibraryId = LW.LibraryId ";
        if (WorkspaceId >= 0)
          sqlQuery += " WHERE LW.RepositoryId = @RepositoryId{0}";
        sqlQuery += " ORDER BY L.LibraryName";
        sqlQuery = String.Format(sqlQuery, filter != null ? " AND (LibraryName LIKE '%' + @Filter+ '%' OR LibraryDescription LIKE '%' + @Filter+ '%')" : "");
        using (DataSet ds = dbObj.RunSQLReturnDataSet(sqlQuery, Database.NewSqlParameter("@RepositoryId", SqlDbType.Int, WorkspaceId), Database.NewSqlParameter("@Filter", SqlDbType.NVarChar, 500, filter)))
        {
          libraryGrid.DataSource = ds;
          libraryGrid.DataBind();
        }

        chooseLibrary(0);
      }
    }

    protected void BindLibrary()
    {
      txtResourceTypeValue.ClearSelection();
      txtResourceTypeValue3.ClearSelection();
      if (selectedLibrary!=null)
      {
        ResourceType[] libraryTypes = ResourceType.GetAllForLibrary(selectedLibrary.Id);
        foreach (ResourceType libraryType in libraryTypes)
          txtResourceTypeValue.Items.FindByValue(libraryType.Id.ToString()).Selected = true;
      }
      else
      {
        foreach (ListItem typeItem in txtResourceTypeValue.Items)
          typeItem.Selected = true;
      }
    }

    private void resourceTypesAttributes_ItemCreated(object sender, RepeaterItemEventArgs e)
    {
      Repeater variantAttributes = (Repeater)e.Item.FindControl("variantAttributes");
      if (e.Item.ItemIndex>=0 && e.Item.ItemIndex<resourceTypes.Length)
      {
        ResourceType dataItem = resourceTypes[e.Item.ItemIndex];
        if (variantAttributes!=null && dataItem!=null)
        {
          VariantAttributeSet attributes = dataItem.Attributes;
          variantAttributes.DataSource = attributes;
          variantAttributes.DataBind();

          foreach (RepeaterItem item in variantAttributes.Items)
          {
            DropDownList attrValueList = (DropDownList)item.FindControl("attrValueList");
            CheckBoxList attrValueBox = (CheckBoxList)item.FindControl("attrValueBox");

            VariantAttribute attrDef = attributes.GetAttributeDefinition(item.ItemIndex);
            if (attrDef!=null)
            {
              if (attrValueList!=null)
              {
                attrValueList.DataSource = attrDef.PossibleValues;
                attrValueList.DataBind();
                attrValueList.Items.Insert(0,new ListItem("none",""));
                attrValueList.SelectedValue = attrDef.DefaultValue;
              }
              if (attrValueBox!=null)
              {
                attrValueBox.DataSource = attrDef.PossibleValues;
                attrValueBox.DataBind();
                attrValueBox.ClearSelection();
                foreach (ListItem attrItem in attrValueBox.Items)
                  attrItem.Selected = true;
              }
            }
          }
        }
      }
    }

    #endregion

    #region Events
    #region Toolbars events
    private void mainToolbar_ButtonClicked(object sender, ButtonEvent be)
    {
      switch (be.Button.Key)
      {
        case "Add":
          chooseLibrary(-1);
          break;
        case "Reload":
          _dataSource = null;
          filter = string.Empty;
          BindGrid();
          break;
        default:
          _dataSource = null;
          BindGrid();
          break;
      }
    }

    private void propertiesToolbar_ButtonClicked(object sender, ButtonEvent be)
    {
      Library library = null;
      switch (be.Button.Key)
      {
        case "Delete":
          if (SessionState.User.IsReadOnly)
            return;

          library = selectedLibrary;
          if (library != null)
          {
            string name = library.Name;
            if (library.ResourceCount > 0)
              SetMessage(propertiesMsgLbl, String.Format("Library \"{0}\" could not be deleted because it is not empty.", name), MessageLevel.Error);
            else if (library.Delete())
            {
              SetMessage(propertiesMsgLbl, String.Format("Library \"{0}\" deleted.", name), MessageLevel.Information);
              selectedLibrary = null;
            }
            else
              SetMessage(propertiesMsgLbl, String.Format("Library \"{0}\" could not be deleted.", name), MessageLevel.Error);
          }
          else
            SetMessage(propertiesMsgLbl, "Deletion failed. Try again please.", MessageLevel.Error);

          _dataSource = null;
          BindGrid();

          break;
        case "Save":
          if (SessionState.User.IsReadOnly)
            return;

          library = selectedLibrary;
          bool isNew = (library==null);
          if (isNew)
            library = new Library(txtNameValue.Text,txtTypeValue.SelectedValue == "1");

          if (library!=null)
          {
            library.Name = txtNameValue.Text;
            library.RelativePath = txtPathValue.Text;
            library.Description = txtDescriptionValue.Value;
            if (library.IsInternal && library.ResourceCount==0)
              library.BinaryStorage = (FileVersion.StoreMode)Enum.Parse(typeof(FileVersion.StoreMode),txtStorageValue.SelectedValue);

            if (WorkspaceId>=0)
              library.WorkspaceId = WorkspaceId;

            bool success = library.Save();

            if (success && isNew)
            {
              ArrayList types = new ArrayList();
              foreach (ListItem typeItem in txtResourceTypeValue.Items)
                if (typeItem.Selected)
                  types.Add(Convert.ToInt32(typeItem.Value));
              success = library.SetResourceTypeRestriction((int[])types.ToArray(typeof(int)));
            }

            if (success)
            {
              if (isNew)
                SetMessage(propertiesMsgLbl, "Library \"" + txtNameValue.Text + "\" created.", MessageLevel.Information);
              else
                SetMessage(propertiesMsgLbl, "Library \"" + txtNameValue.Text + "\" updated.", MessageLevel.Information);
            }
            else
            {
              if (isNew)
                SetMessage(propertiesMsgLbl, String.Format("Library \"{0}\" has not been created.", txtNameValue.Text), MessageLevel.Error);
              else
                SetMessage(propertiesMsgLbl, String.Format("Library \"{0}\" has not been updated.", txtNameValue.Text), MessageLevel.Error);
            }
          }
          else
            SetMessage(propertiesMsgLbl, String.Format("Library \"{0}\" has not been created.", txtNameValue.Text), MessageLevel.Error);

          BindGrid();

          if (library!=null)
          {
            int _selectedLibraryId = library.Id;
            for (int itemIndex=0;itemIndex<libraryGrid.DataKeys.Count;itemIndex++)
              if (((int)libraryGrid.DataKeys[itemIndex]) == _selectedLibraryId)
              {
                chooseLibrary(itemIndex);
                break;
              }
          }
          break;
      }
    }

    #endregion

    #region Grid
    protected void ChooseLibrary(object sender, System.EventArgs e)
    {
      DataGridItem item = (DataGridItem) ((System.Web.UI.WebControls.LinkButton)sender).Parent.Parent;
      if (item!=null)
        chooseLibrary(item.ItemIndex);
    }

    private void chooseLibrary(int itemIndex)
    {
      selectedLibrary = itemIndex >= 0 && libraryGrid.DataKeys.Count > itemIndex ? dataSource[(int)libraryGrid.DataKeys[itemIndex]] : null;
      libraryGrid.SelectedIndex = itemIndex;
      propertiesLbl.Text = itemIndex>=0?"Properties":"New library";
      BindLibrary();
    }
    #endregion

    private void librariesPager_PageChanged(object sender, EventArgs e)
    {
      BindGrid();
    }
    #endregion
  }

}
