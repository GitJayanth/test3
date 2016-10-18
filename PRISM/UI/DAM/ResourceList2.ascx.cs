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

using System.IO;
using System.Data.SqlClient;
using Infragistics.WebUI.UltraWebGrid;
using Infragistics.WebUI.UltraWebToolbar;
using Infragistics.WebUI.WebSchedule;

using HyperCatalog.Business.DAM;
using HyperCatalog.Shared;
using HyperComponents.Data.dbAccess;

namespace HyperCatalog.UI.DAM.Controls
{
  /// <summary>
  /// Description résumée de ResourceList.
  /// </summary>
  public partial class ResourceList2 : UserControl
  {
    public enum Mode : int
    {
      Complete = 0,
      Minimal = 1
    }
    public Random rn = new Random();
    private string libraryListPattern = "{0} [{1}]";
    private TextBox _txtResourceFilter;
    protected string resourceFilter
    {
      get
      {
        if (_txtResourceFilter == null)
          _txtResourceFilter = (TextBox)mTb.FindControl("txtResourceFilter");
        return (_txtResourceFilter != null ? _txtResourceFilter.Text : string.Empty);
      }
      set
      {
        if (_txtResourceFilter == null)
          _txtResourceFilter = (TextBox)mTb.FindControl("txtResourceFilter");
        if (_txtResourceFilter != null)
          _txtResourceFilter.Text = value;
      }
    }
    protected string variantFilter
    {
      get
      {
        TextBox txtFilter = (TextBox)vTb.FindControl("txtVariantFilter");
        return (txtFilter != null ? txtFilter.Text : string.Empty);
      }
      set
      {
        TextBox txtFilter = (TextBox)vTb.FindControl("txtVariantFilter");
        if (txtFilter != null)
          txtFilter.Text = value;
      }
    }
    private bool addVariant
    {
      get
      {
        object _addVariant = ViewState["addVariant"];
        if (_addVariant != null && _addVariant is bool)
          return (bool)_addVariant;
        return false;
      }
      set
      {
        ViewState["addVariant"] = value;
      }
    }
    public Mode DisplayMode = Mode.Complete;
    private string _damHandlerURL;
    private string damHandlerURL
    {
      get
      {
        if (_damHandlerURL == null)
          _damHandlerURL = HyperCatalog.Shared.SessionState.CacheComponents["DAM_Provider"].URI;
        return _damHandlerURL;
      }
    }

    #region business
    public int WorkspaceId
    {
      get
      {
        object _workspaceId = ViewState["workspaceId"];
        if (_workspaceId != null && _workspaceId is int)
          return (int)_workspaceId;
        else
        {
          if (Request["w"] != null)
          {
            _workspaceId = Convert.ToInt32(Request["w"]);
            WorkspaceId = (int)_workspaceId;
            return (int)_workspaceId;
          }
          return -1;
        }
      }
      set
      {
        ViewState["workspaceId"] = value;
      }
    }

    private LibraryCollection _userLibraries;
    protected LibraryCollection UserLibraries
    {
      get
      {
        if (_userLibraries == null)
          _userLibraries = Library.GetAll(WorkspaceId);
        return _userLibraries;
      }
    }

    private string _currentCultureCode;
    public string CurrentCultureCode
    {
      get
      {
        if (_currentCultureCode == null)
          _currentCultureCode = (string)ViewState["currentCultureCode"];
        return _currentCultureCode;
      }
      set
      {
        ViewState["currentCultureCode"] = _currentCultureCode = value;
      }
    }

    private int libId = -1;
    private int _currentLibraryId
    {
      get
      {
        try
        {
          if (libId >= 0)
            return libId;
          if (libraryList.Items.Count > 0)
            return Convert.ToInt32(libraryList.SelectedValue);
        }
        catch { }
        return -1;
      }
      set
      {
        if (UserLibraries.ContainsKey(value))
        {
          libId = value;
          if (libraryList.Items.Count > 0)
          {
            libraryList.ClearSelection();
            libraryList.SelectedValue = value.ToString();
          }
          Session["DAM_currentLibraryId"] = value;
        }
        if (_currentLibrary != null && _currentLibrary.Id != value)
          _currentLibrary = null;
      }
    }

    private Library _currentLibrary;
    public Library CurrentLibrary
    {
      get
      {
        if (_currentLibrary == null && _currentLibraryId >= 0)
          _currentLibrary = Library.GetByKey(_currentLibraryId);
        return _currentLibrary;
      }
      set
      {
        _currentLibrary = value;
      }
    }

    private int _currentResourceId
    {
      get
      {
        object id = ViewState["_currentResourceId"];
        return id == null ? -1 : (int)id;
      }
      set
      {
        ViewState["_currentResourceId"] = value;
        if (_currentResource != null && _currentResource.Id != value)
          _currentResource = null;
      }
    }
    private int lastResourceId
    {
      get
      {
        return ViewState["lastResourceId"] == null ? -1 : (int)ViewState["lastResourceId"];
      }
      set
      {
        ViewState["lastResourceId"] = value;
      }
    }

    private HyperCatalog.Business.DAM.Resource _currentResource;
    public HyperCatalog.Business.DAM.Resource CurrentResource
    {
      get
      {
        if (_currentResource == null && _currentLibraryId >= 0 && _currentResourceId >= 0)
        {
          Trace.Warn("ResourceList2 : Getter : Business.DAM.Resource.GetByKey(_currentLibraryId, _currentResourceId)");
          _currentResource = Business.DAM.Resource.GetByKey(_currentLibraryId, _currentResourceId);
        }
        return _currentResource;
      }
      set
      {
        _currentResource = value;
      }
    }

    private ResourceType[] _resourceTypes;
    protected ResourceType[] resourceTypes
    {
      get
      {
        if (_resourceTypes == null)
        {
          _resourceTypes = _currentLibraryId >= 0 ? ResourceType.GetAllForLibrary(_currentLibraryId) : ResourceType.GetAll();
        }
        return _resourceTypes;
      }
    }
    #endregion

    protected override void OnLoad(EventArgs e)
    {
      if (this.Visible)
      {

        if (DisplayMode == Mode.Complete && !SessionState.User.HasCapability(Business.CapabilitiesEnum.MANAGE_RESOURCES))
          UITools.DenyAccess(DenyMode.Frame);

        #region init grids
        Utils.InitGridSort(ref rGd);
        //Utils.InitGridSort(ref variantsGrid);
        #endregion

        propertiesMsgLbl.Visible = false;

        mTb.Items.FromKeyButton("Add").Visible = completePanel.Visible = (DisplayMode == Mode.Complete);
        minimalCell.ColSpan = (DisplayMode == Mode.Complete ? 1 : 3);

        if (!IsPostBack)
        {
          HyperCatalog.Business.CultureList cultures = HyperCatalog.Business.Culture.GetAll();
          _currentCultureCode = HyperCatalog.Shared.SessionState.MasterCulture.Code;
          if (cultures != null)
            cultures.Sort("Name ASC");
          txtVariantCultureValue.DataSource = cultures;
          txtVariantCultureValue.DataBind();

          _currentResourceId = -1;
          _currentLibrary = null;
          _resourceTypes = null;
          if (Request["l"] != null)
          {
            try
            {
              _currentLibraryId = Convert.ToInt32(Request["l"]);
            }
            catch { }
          }
          else if (Request["resource"] != null)
          {
            try
            {
              string[] splitted = Request["resource"].Split('/');
              if (splitted.Length == 2)
              {
                _currentLibrary = Library.GetByKey(splitted[0]);
                if (_currentLibrary != null)
                  _currentResource = _currentLibrary.Resources[splitted[1].Split('.')[0]];
              }
            }
            catch { }
          }
          if (Request["culture"] != null)
          {
            CurrentCultureCode = Request["culture"];
          }
          if (_currentLibraryId < 0 && Session["DAM_currentLibraryId"] is int)
            _currentLibraryId = (int)Session["DAM_currentLibraryId"];

          BindDAM();
        }
      }
      base.OnLoad(e);

      //Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "ChgLib", "function ChgLib(libId){window.location='" + Request.Url.PathAndQuery.Substring(0, Request.Url.PathAndQuery.Length - Request.Url.Query.Length) + "?w=" + WorkspaceId.ToString() + "&l=' + libId;}", true);
      //libraryList.Attributes.Add("onchange", "ChgLib(this.value)");
    }
    protected override void OnPreRender(EventArgs e)
    {
      bool isReadOnly = Shared.SessionState.User.IsReadOnly;
      resourcePropertiesLbl.Text = CurrentResource == null ? "New resource" : addVariant ? "New variant" : "Resource information";
      variantAddProperties.Visible = CurrentResource == null || addVariant;
      resourceLbl.Visible = CurrentResource != null;
      txtResourceTypeValue.Enabled = CurrentResource == null;
      txtResourceNameValue.Enabled = CurrentResource == null || !addVariant;
      basicResourceInformation.Visible = CurrentResource != null && !addVariant;
        //Added for QC# 775.
      linkAssociation.Visible = CurrentResource != null? CurrentResource.TypeId == 2:false;
      txtResourceDescriptionValue.Disabled = CurrentResource != null && addVariant;
      rGd.DisplayLayout.Pager.AllowPaging = rGd.Rows.Count > 0;
      variantsGrid.Columns[1].Visible = variantsGrid.Items.Count > 1;
      variantsPanel.Visible = CurrentResource != null && !addVariant;

      Infragistics.WebUI.UltraWebToolbar.TBarButton CancelButton = pTb.Items.FromKeyButton("Cancel");
      if (CancelButton != null)
        CancelButton.Visible = CurrentResource == null || addVariant;
      Infragistics.WebUI.UltraWebToolbar.TBarButton DelButton = pTb.Items.FromKeyButton("Del");
      if (DelButton != null)
        DelButton.Visible = CurrentResource != null && !addVariant;
      Infragistics.WebUI.UltraWebToolbar.TBarButton CRTButton = pTb.Items.FromKeyButton("CRT");
      if (CRTButton != null)
        CRTButton.Visible = CurrentResource != null && CurrentResource.Type.Name == "Template";

      Infragistics.WebUI.UltraWebToolbar.TBarButton AddResourceButton = mTb.Items.FromKeyButton("Add");
      AddResourceButton.Visible = CurrentLibrary.IsInternal && DisplayMode == Mode.Complete;

      #region restrict interface to user capabilities
      Infragistics.WebUI.UltraWebToolbar.TBSeparator SepAddButton = pTb.Items.FromKeySeparator("SepAdd");
      Infragistics.WebUI.UltraWebToolbar.TBarButton SaveButton = pTb.Items.FromKeyButton("Save");
      Infragistics.WebUI.UltraWebToolbar.TBarButton AddVariantButton = vTb.Items.FromKeyButton("Add");

      bool isCapable = SessionState.User.HasCapability(Business.CapabilitiesEnum.MANAGE_RESOURCES);
      if (DelButton != null)
        DelButton.Visible = !isReadOnly && (DelButton.Visible && isCapable);
      if (AddResourceButton != null)
        AddResourceButton.Visible = !isReadOnly && (AddResourceButton.Visible && isCapable);
      if (SepAddButton != null)
        SepAddButton.ToolBar.Items.Remove(SepAddButton);
      if (SaveButton != null)
        SaveButton.Visible = !isReadOnly && isCapable;
      if (AddVariantButton != null)
        AddVariantButton.Visible = !isReadOnly && isCapable;
      #endregion

      #region edition cell style
      string css = ".editOptionalLabelCell\r\n" +
        "{\r\n" +
        "	border-right: 1pt double;\r\n" +
        "	border-top: 1pt double;\r\n" +
        "	font-size: 8pt;\r\n" +
        "	border-left: 1pt double;\r\n" +
        "	border-bottom: silver 1pt double;\r\n" +
        "	padding-left: 1px;\r\n" +
        "	padding-right: 10px;\r\n";
      css += addVariant ?
        "	color: gray;\r\n" +
        "	background-color: #E5E5E5;\r\n"
        :
        "	color: black;\r\n" +
        "	background-color: lightgrey;\r\n";
      css += "}\r\n";
      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "css", "<STYLE>" + css + "</STYLE>");
      #endregion

      base.OnPreRender(e);
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
      this.vTb.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.variantToolbar_ButtonClicked);
      this.variantsGrid.ItemCreated += new DataGridItemEventHandler(variantsGrid_ItemCreated);

      this.resourcesPagerTop.Grid = this.resourceGrid;
      this.resourcesPagerTop.PageChanged += new EventHandler(resourcesPager_PageChanged);
      this.resourcesPagerBottom.Grid = this.resourceGrid;
      this.resourcesPagerBottom.PageChanged += new EventHandler(resourcesPager_PageChanged);
      this.libraryList.SelectedIndexChanged += new EventHandler(libraryList_SelectedIndexChanged);
    }
    #endregion

    #region Data load & bind
    private void BindDAM()
    {
      libraryList.Items.Clear();
      txtResourceLibraryValue.Items.Clear();
      foreach (Library library in UserLibraries)
      {
        libraryList.Items.Add(new ListItem(String.Format(libraryListPattern, library.Name, library.ResourceCount), library.Id.ToString()));
        txtResourceLibraryValue.Items.Add(new ListItem(library.Name, library.Id.ToString()));
      }
      if (Request["l"] != null)
      {
        _currentLibraryId = Convert.ToInt32(Request["l"]);
        _currentResourceId = -1;
        libraryList.SelectedValue = Request["l"].ToString();
      }
      if (CurrentLibrary != null && libraryList.Items.FindByValue(_currentLibrary.Id.ToString()) != null)
        libraryList.SelectedValue = _currentLibrary.Id.ToString();

      BindLibrary();
    }
    private void BindLibrary()
    {
      if (CurrentResource != null && !CurrentResource.Match(resourceFilter))
        _currentResourceId = -1;

      using (Database dbObj = new Database(SessionState.CacheComponents["DAM_DB"].ConnectionString))
      {
        string sqlQuery = "SELECT * FROM VIEW_DAM_Resources WHERE LibraryId = @LibraryId{0} ORDER BY ResourceName";
        sqlQuery = String.Format(sqlQuery, resourceFilter != null ? " AND (ResourceName LIKE '%' + @Filter+ '%' OR ResourceDescription LIKE '%' + @Filter+ '%')" : "");
        using (DataSet ds = dbObj.RunSQLReturnDataSet(sqlQuery, Database.NewSqlParameter("@LibraryId", SqlDbType.Int, _currentLibraryId), Database.NewSqlParameter("@Filter", SqlDbType.NVarChar, 500, resourceFilter)))
          if (ds != null && ds.Tables.Count == 1)
          {
            resourcesPagerTop.Visible = resourcesPagerBottom.Visible = resourceGrid.Visible = completePanel.Visible = ds.Tables[0].Rows.Count > 0;
            resourceGrid.Columns[1].Visible = (DisplayMode != Mode.Complete);
            completePanel.Visible = completePanel.Visible && DisplayMode == Mode.Complete;
            resourceGrid.DataSource = ds;
            if (resourceGrid.CurrentPageIndex * resourceGrid.PageSize > ds.Tables[0].Rows.Count)
              resourceGrid.CurrentPageIndex = 0;
            if (_currentResourceId >= 0)
            {
              ds.Tables[0].PrimaryKey = new DataColumn[] { ds.Tables[0].Columns[0] };
              int index = ds.Tables[0].Rows.IndexOf(ds.Tables[0].Rows.Find(_currentResourceId));
              resourceGrid.CurrentPageIndex = (int)Math.Floor((double)index / resourceGrid.PageSize);
            }
            resourceGrid.DataBind();
            emptyLibPanel.Visible = ds.Tables[0].Rows.Count == 0;
            if (_currentResourceId < 0 && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
            {
              _currentResourceId = (int)ds.Tables[0].Rows[0]["ResourceId"];
              chooseResource(0);
            }
          }
      }
      if (_currentResourceId >= 0)
      {
        //Response.Write(_currentResourceId + " " + resourceGrid.DataKeys.Count);
        for (int index = 0; index < resourceGrid.DataKeys.Count; index++)
          if ((int)resourceGrid.DataKeys[index] == _currentResourceId)
          {
            chooseResource(index);
          }

      }

      txtResourceTypeValue.DataSource = resourceTypes;
      resourceTypesAttributes.DataSource = resourceTypes;
      txtResourceTypeValue.DataBind();
      resourceTypesAttributes.DataBind();

      if (DisplayMode == Mode.Complete)
        BindResource();
    }

    protected void rGd_InitializeRow(object sender, RowEventArgs e)
    {
      UltraGridCell thumbnail = e.Row.Cells.FromKey("Thumbnail");
      thumbnail.Text = "<a href=\"" + thumbnail.Value + "><img src=\"" + thumbnail.Value + "?thumbnail=1&size=40&DAM_culture=" + CurrentCultureCode + "\" width=\"40\" height=\"40\" border=\"0\"/></a>";
    }
    protected void resourceGrid_ItemCreated(object sender, DataGridItemEventArgs e)
    {
      string text = "<img src=\"{0}\" border=\"0\"/>";
      if (e.Item.DataItem != null)
        text = String.Format(text, damHandlerURL + "/" + DataBinder.Eval(e.Item.DataItem, "LibraryName").ToString() + "/" + DataBinder.Eval(e.Item.DataItem, "ResourceName").ToString() + ".dam?thumbnail=1&size=40&DAM_culture=" + CurrentCultureCode);
      e.Item.Cells[1].Text = text;
    }

    private void BindResource()
    {
      variantsGrid.EditItemIndex = -1;
      if (CurrentResource != null)
      {
        resourceLbl.Text = CurrentResource.ParentLibraryName + " > " + CurrentResource.Name + " [" + CurrentResource.Type.Name + "]";
        if (txtResourceLibraryValue.Items.FindByValue(CurrentResource.ParentLibraryId.ToString()) != null)
          txtResourceLibraryValue.SelectedValue = CurrentResource.ParentLibraryId.ToString();
        txtResourceNameValue.Text = CurrentResource.Name;
        txtResourceDescriptionValue.Value = CurrentResource.Description;
        if (txtResourceTypeValue.Items.FindByValue(CurrentResource.TypeId.ToString()) != null)
          txtResourceTypeValue.SelectedValue = CurrentResource.TypeId.ToString();
        txtVariantCultureValue.Enabled = true;
        txtResourceURIValue.HRef = CurrentResource.URI;
        txtResourceURIValue.InnerText = CurrentResource.URI;
        // QC# 775.
        if (CurrentResource.TypeId == 2)
        {
            using (Database dbObj = new Database(SessionState.CacheComponents["DAM_DB"].ConnectionString))
            {
                DataSet dsImgAss = dbObj.RunSPReturnDataSet("DAM_GetProductsAttachedToResource", Database.NewSqlParameter("@LibraryId", SqlDbType.Int, CurrentResource.ParentLibraryId), Database.NewSqlParameter("@ResourceId", SqlDbType.Int, CurrentResource.Id));
                txtResSource.DataTextField = "ProductDetails";
                txtResSource.DataValueField = "ProductDetails";
                txtResSource.DataSource = dsImgAss.Tables[0];
                txtResSource.DataBind();
                if (dsImgAss.Tables[0].Rows.Count == 0)
                {
                    txtResSource.Items.Add("Not assigned to any Product");
                }
            }
        }
      }
      else
      {
        txtVariantCultureValue.SelectedValue = HyperCatalog.Shared.SessionState.MasterCulture.Code;
        txtResourceLibraryValue.SelectedValue = _currentLibraryId.ToString();
        txtResourceNameValue.Text = "";
        txtResourceDescriptionValue.Value = "";
        txtResourceTypeValue.ClearSelection();
        txtVariantCultureValue.Enabled = false;
        txtResourceURIValue.HRef = "";
        txtResourceURIValue.InnerText = "";
	txtResSource.Items.Clear(); 
      }

      BindVariantGrid();
    }
    private void BindVariantGrid()
    {
      addVariant = false;
      if (CurrentResource != null)
        variantsGrid.DataSource = CurrentResource.Variants;
      else
        variantsGrid.DataSource = null;
      variantsGrid.DataBind();
    }
    private void resourceTypesAttributes_ItemCreated(object sender, RepeaterItemEventArgs e)
    {
      Repeater variantAttributes = (Repeater)e.Item.FindControl("variantAttributes");
      if (e.Item.ItemIndex >= 0 && e.Item.ItemIndex < resourceTypes.Length)
      {
        ResourceType dataItem = resourceTypes[e.Item.ItemIndex];
        if (variantAttributes != null && dataItem != null)
        {
          VariantAttributeSet attributes = dataItem.Attributes;
          variantAttributes.DataSource = attributes;
          variantAttributes.DataBind();

          foreach (RepeaterItem item in variantAttributes.Items)
          {
            Label attrMsg = (Label)item.FindControl("attrMsg");
            TextBox attrValueBox = (TextBox)item.FindControl("attrValueBox");
            DropDownList attrValueList = (DropDownList)item.FindControl("attrValueList");

            VariantAttribute attrDef = attributes.GetAttributeDefinition(item.ItemIndex);
            if (attrDef != null)
            {
              attrMsg.Visible = !attrDef.UserDefined;

              string value = attributes[item.ItemIndex];
              if (attrDef.UserDefined)
              {
                if (attrValueBox != null)
                {
                  attrValueBox.Text = value;
                  attrValueBox.Visible = attrDef.PossibleValues.Count == 0;
                }
                if (attrValueList != null)
                {
                  attrValueList.DataSource = attrDef.PossibleValues;
                  attrValueList.DataBind();
                  attrValueList.SelectedValue = value != null ? value : attrDef.DefaultValue;
                  attrValueList.Visible = attrDef.PossibleValues.Count > 0;
                  //attrValueList.Enabled = attrDef.PossibleValues.Count>1;
                }
              }

            }
          }
        }
      }
    }
    private void variantsGrid_ItemCreated(object sender, DataGridItemEventArgs e)
    {
      ImageButton deleteVariant = (ImageButton)e.Item.FindControl("deleteVariant");
      if (deleteVariant != null && CurrentResource != null)
        deleteVariant.Attributes.Add("onclick", "return cfmDel('" + CurrentResource.Name + "');");
    }
    #endregion

    #region Events
    private void mainToolbar_ButtonClicked(object sender, ButtonEvent be)
    {
      switch (be.Button.Key)
      {
        case "Add":
          if (SessionState.User.HasCapability(Business.CapabilitiesEnum.MANAGE_RESOURCES))
          {
            emptyLibPanel.Visible = false;
            lastResourceId = resourceGrid.SelectedIndex;
            chooseResource(-1);
          }
          break;
        case "Export":
          using (Database dbObj = new Database(SessionState.CacheComponents["DAM_DB"].ConnectionString))
          {
            string sqlQuery = "SELECT ResourceName AS Name, ResourceDescription AS Description, ResourceCreatedOn AS CreatedOn FROM DAM_Resources WHERE LibraryId = @LibraryId{0} ORDER BY ResourceName";
            sqlQuery = String.Format(sqlQuery, resourceFilter != null ? " AND (ResourceName LIKE '%' + @Filter+ '%' OR ResourceDescription LIKE '%' + @Filter+ '%')" : "");
            using (DataSet ds = dbObj.RunSQLReturnDataSet(sqlQuery, Database.NewSqlParameter("@LibraryId", SqlDbType.Int, _currentLibraryId), Database.NewSqlParameter("@Filter", SqlDbType.NVarChar, 500, resourceFilter)))
              if (ds != null && ds.Tables.Count == 1)
                Utils.ExportDataSetToExcel(this.Page, ds, "Resources.xls");
              else
                SetMessage(propertiesMsgLbl, "Error occurred when creating the excel sheet.", MessageLevel.Error);
          }
          break;
        case "Filter":
          BindLibrary();
          break;
        case "Reload":
          resourceFilter = string.Empty;
          BindLibrary();
          break;
      }
    }
    private void propertiesToolbar_ButtonClicked(object sender, ButtonEvent be)
    {
      switch (be.Button.Key)
      {
        #region case "CRT":
        case "CRT":
          if (CurrentResource != null && CurrentResource.Type.Name == "Template")
          {
            Response.Redirect(CurrentResource.URI + "?CRT=1");
            //            HyperCatalog.Business.DAM.Specific.HOTTemplate template = new HyperCatalog.Business.DAM.Specific.HOTTemplate(currentResource.MasterVariant.CurrentFile);
            //            HyperComponents.IO.Streaming.SendStream(Response,template.CRTStream,currentResource.Name + ".pdf");
            Response.End();
          }
          break;
        #endregion
        #region case "Del":
        case "Del":
          if (SessionState.User.HasCapability(Business.CapabilitiesEnum.MANAGE_RESOURCES) && CurrentResource != null)
          {
            if (SessionState.User.IsReadOnly || CurrentResource.Delete())
            {
              SetMessage(propertiesMsgLbl, "Resource \"" + CurrentResource.Name + "\" deleted", MessageLevel.Information);
              libraryList.SelectedItem.Text = String.Format(libraryListPattern, CurrentLibrary.Name, CurrentLibrary.ResourceCount);
              _currentResourceId = -1;
              BindLibrary();
            }
            else
              SetMessage(propertiesMsgLbl, "Resource \"" + CurrentResource.Name + "\" could not be deleted", MessageLevel.Error);
          }
          break;
        #endregion
        #region case "Save":
        case "Save":
          if (SessionState.User.HasCapability(Business.CapabilitiesEnum.MANAGE_RESOURCES))
          {
            if (SessionState.User.IsReadOnly)
              return;

            if (resourceTypesAttributes.Items.Count > txtResourceTypeValue.SelectedIndex)
            {
              if (txtResourceNameValue.Text == null || txtResourceNameValue.Text.Length == 0)
              {
                SetMessage(propertiesMsgLbl, "Resource must have a name.", MessageLevel.Error);
                break;
              }
              string[][] attrs = getAttributeValues(resourceTypesAttributes.Items[txtResourceTypeValue.SelectedIndex]);

              if (CurrentResource != null)
              {
                HyperCatalog.Business.DAM.Resource existingResource = CurrentLibrary.Resources[txtResourceNameValue.Text];
                if (existingResource != null && CurrentResource.Id != existingResource.Id)
                {
                  SetMessage(propertiesMsgLbl, "The name \"" + txtResourceNameValue.Text + "\" is already in use in this library.", MessageLevel.Error);
                  return;
                }
                CurrentResource.Name = txtResourceNameValue.Text;
                CurrentResource.Description = txtResourceDescriptionValue.Value;
                if (CurrentResource.Save())
                {
                  SetMessage(propertiesMsgLbl, "Resource \"" + txtResourceNameValue.Text + "\" updated", MessageLevel.Information);

                  if (addVariant)
                  {
                    bool success = false;
                    string errorMsg = "Variant for \"" + txtResourceNameValue.Text + "\" could not be created";
                    try
                    {
                      success = CurrentResource.AddVariant(txtVariantCultureValue.SelectedValue, txtFileBinaryValue.PostedFile.InputStream, Path.GetExtension(txtFileBinaryValue.PostedFile.FileName), attrs) != null;
                    }
                    catch (AlreadyExistingVariantException)
                    {
                      errorMsg += ": this variant already exists. Please choose different culture or attributes.";
                    }
                    catch (FormatException exc)
                    {
                      errorMsg += ": " + exc.Message;
                    }

                    if (success)
                      SetMessage(propertiesMsgLbl, "Variant for \"" + txtResourceNameValue.Text + "\" created", MessageLevel.Information);
                    else
                      SetMessage(propertiesMsgLbl, errorMsg, MessageLevel.Error);
                  }

                  BindLibrary();
                }
                else
                  SetMessage(propertiesMsgLbl, "Resource \"" + txtResourceNameValue.Text + "\" could not be updated", MessageLevel.Error);
              }
              else
              {
                if (txtFileBinaryValue.PostedFile != null && txtFileBinaryValue.PostedFile.ContentLength == 0)
                {
                  SetMessage(propertiesMsgLbl, "You must provide a file", MessageLevel.Warning);
                  return;
                }
                if (CurrentLibrary.Resources[txtResourceNameValue.Text] != null)
                {
                  SetMessage(propertiesMsgLbl, "The name \"" + txtResourceNameValue.Text + "\" is already in use in this library. Please better use variant edition interface if you want to add a new variant to a resource.", MessageLevel.Error);
                  return;
                }

                HyperCatalog.Business.DAM.Resource newResource;
                try
                {
                  newResource = CurrentLibrary.AddResource(txtResourceNameValue.Text + Path.GetExtension(txtFileBinaryValue.PostedFile.FileName), txtResourceDescriptionValue.Value, Convert.ToInt32(txtResourceTypeValue.SelectedValue), txtVariantCultureValue.SelectedValue, txtFileBinaryValue.PostedFile.InputStream, attrs);
                }
                catch (FormatException exc)
                {
                  SetMessage(propertiesMsgLbl, "Resource \"" + txtResourceNameValue.Text + "\" could not be created: " + exc.Message, MessageLevel.Error);
                  break;
                }

                if (newResource != null)
                {
                  SetMessage(propertiesMsgLbl, "Resource \"" + txtResourceNameValue.Text + "\" created", MessageLevel.Information);
                  libraryList.SelectedItem.Text = String.Format(libraryListPattern, CurrentLibrary.Name, CurrentLibrary.ResourceCount);
                  _currentResourceId = newResource.Id;
                  BindLibrary();
                }
                else
                  SetMessage(propertiesMsgLbl, "Resource \"" + txtResourceNameValue.Text + "\" could not be created.", MessageLevel.Error);
              }
            }
          }
          break;
        #endregion
        #region case "Cancel":
        case "Cancel":
          if (addVariant)
            addVariant = false;
          else if (lastResourceId >= 0)
            chooseResource(lastResourceId);
          break;
        #endregion
      }
    }
    private void variantToolbar_ButtonClicked(object sender, ButtonEvent be)
    {
      switch (be.Button.Key)
      {
        case "Add":
          if (SessionState.User.HasCapability(Business.CapabilitiesEnum.MANAGE_RESOURCES))
            addVariant = true;
          break;
        case "Filter":
          BindResource();
          break;
        case "Reload":
          variantFilter = string.Empty;
          BindResource();
          break;
      }
    }

    private void libraryList_SelectedIndexChanged(object sender, EventArgs e)
    {
      _currentResourceId = -1;
      _currentLibrary = null;
      _resourceTypes = null;
      _currentLibraryId = Convert.ToInt32(libraryList.SelectedValue);
      BindLibrary();
    }
    protected void SelectResource_Click(object sender, System.EventArgs e)
    {
      Infragistics.WebUI.UltraWebGrid.CellItem cellItem = ((Infragistics.WebUI.UltraWebGrid.CellItem)((System.Web.UI.WebControls.LinkButton)sender).Parent);
      _currentResourceId = (int)cellItem.Cell.Row.DataKey;
      if (DisplayMode == Mode.Complete)
        BindResource();
      else if (DisplayMode == Mode.Minimal && CurrentResource != null && Request["openerfunc"] != null)
      {
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "1", "<script>window.returnValue='" + CurrentResource.RelativeURI + "';window.close();</script>");
        Page.ClientScript.RegisterStartupScript(this.GetType(), "1", "<script>" + Request["openerfunc"].ToString() + "('" + CurrentResource.RelativeURI + "');window.close();</script>");
      }
    }
    protected void ChooseResource(object sender, System.EventArgs e)
    {
      DataGridItem cellItem = ((DataGridItem)((System.Web.UI.WebControls.LinkButton)sender).Parent.Parent);
      _currentResourceId = Convert.ToInt32(resourceGrid.DataKeys[(int)cellItem.ItemIndex]);
      resourceGrid.SelectedIndex = cellItem.ItemIndex;
      if (DisplayMode == Mode.Complete)
        BindResource();
      else if (DisplayMode == Mode.Minimal && CurrentResource != null && Request["openerfunc"] != null)
      {
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "1", "<script>window.returnValue='" + CurrentResource.RelativeURI + "';window.close();</script>");
        Page.ClientScript.RegisterStartupScript(this.GetType(), "1", "<script>" + Request["openerfunc"].ToString() + "('" + CurrentResource.RelativeURI + "');window.close();</script>");
      }
    }
    private void resourcesPager_PageChanged(object sender, EventArgs e)
    {
      _currentResourceId = -1;
      BindLibrary();
    }

    #region Edit variant
    protected void EditVariant_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
      if (SessionState.User.HasCapability(Business.CapabilitiesEnum.MANAGE_RESOURCES))
      {
        DataGridItem item = (DataGridItem)((System.Web.UI.WebControls.ImageButton)sender).Parent.Parent.Parent;
        variantsGrid.EditItemIndex = item.ItemIndex;
        BindVariantGrid();
      }
    }
    protected void ValidateEditVariant_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
      if (SessionState.User.HasCapability(Business.CapabilitiesEnum.MANAGE_RESOURCES))
      {
        if (SessionState.User.IsReadOnly)
        {
          variantsGrid.EditItemIndex = -1;
          return;
        }

        DataGridItem item = (DataGridItem)((System.Web.UI.WebControls.ImageButton)sender).Parent.Parent;
        if (item != null && variantsGrid.EditItemIndex == item.ItemIndex)
        {
          Variant variant = CurrentResource.Variants[(int)variantsGrid.DataKeys[item.ItemIndex]];

          if (variant != null)
          {
            DropDownList editVariantCulture = (DropDownList)item.FindControl("editVariantCulture");
            DataGrid editVariantAttributes = (DataGrid)item.FindControl("editVariantAttributes");
            HtmlInputFile editVariantFileValue = (HtmlInputFile)item.FindControl("editVariantFileValue");
            WebDateChooser editVariantBOPValue = (WebDateChooser)item.FindControl("editVariantBOPValue");
            WebDateChooser editVariantEOPValue = (WebDateChooser)item.FindControl("editVariantEOPValue");

            if (editVariantCulture != null)
              variant.CultureCode = editVariantCulture.SelectedValue;
            if (editVariantAttributes != null)
              #region variant attributes
              foreach (DataGridItem paramItem in editVariantAttributes.Items)
              {
                Label editVariantAttributeName = (Label)paramItem.FindControl("editVariantAttributeName");
                if (editVariantAttributeName != null)
                {
                  string attrName = editVariantAttributeName.Text;
                  VariantAttribute attrDef = variant.Attributes.GetAttributeDefinition(attrName);
                  if (attrDef.UserDefined)
                  {
                    TextBox editVariantAttributeBox = (TextBox)paramItem.FindControl("editVariantAttributeBox");
                    DropDownList editVariantAttributeList = (DropDownList)paramItem.FindControl("editVariantAttributeList");
                    if (attrDef.PossibleValues.Count > 0 && editVariantAttributeList != null)
                      variant.Attributes[attrName] = editVariantAttributeList.SelectedValue;
                    else if (editVariantAttributeBox != null)
                      variant.Attributes[attrName] = editVariantAttributeBox.Text;
                  }
                }
              }
              #endregion
            if (editVariantFileValue != null && editVariantFileValue.PostedFile != null && editVariantFileValue.PostedFile.ContentLength > 0)
            {
              if (editVariantBOPValue != null && editVariantEOPValue != null)
              {
                //,editVariantBOPValue.Value!=null?(DateTime)editVariantBOPValue.Value:DateTime.MinValue,editVariantEOPValue.Value!=null?(DateTime)editVariantEOPValue.Value:DateTime.MaxValue);
                if (variant.AddFile(editVariantFileValue.PostedFile.InputStream, Path.GetExtension(editVariantFileValue.PostedFile.FileName)) == null)
                  SetMessage(propertiesMsgLbl, "Sorry, an error occured, file could not be updated.", MessageLevel.Error);
              }
              else if (variant.AddFile(editVariantFileValue.PostedFile.InputStream, Path.GetExtension(editVariantFileValue.PostedFile.FileName)) == null)
                SetMessage(propertiesMsgLbl, "Sorry, an error occured, file could not be updated.", MessageLevel.Error);
            }

            if (variant.Save())
            {
              variantsGrid.EditItemIndex = -1;
              BindVariantGrid();
            }
          }
        }
      }
    }
    protected void CancelEditVariant_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
      DataGridItem item = (DataGridItem)((System.Web.UI.WebControls.ImageButton)sender).Parent.Parent;
      if (variantsGrid.EditItemIndex == item.ItemIndex)
      {
        variantsGrid.EditItemIndex = -1;
        BindVariantGrid();
      }
    }
    protected void DeleteVariant_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
      DataGridItem item = (DataGridItem)((System.Web.UI.WebControls.ImageButton)sender).Parent.Parent.Parent;
      if (item != null)
      {
        Variant variant = CurrentResource.Variants[(int)variantsGrid.DataKeys[item.ItemIndex]];
        if (variant.Delete())
          BindVariantGrid();
      }
    }
    #endregion
    #endregion

    private void chooseResource(int itemIndex)
    {
      _currentResourceId = itemIndex >= 0 && itemIndex < resourceGrid.DataKeys.Count ? (int)resourceGrid.DataKeys[itemIndex] : -1;
      resourceGrid.SelectedIndex = itemIndex;
      BindResource();
    }
    private void oldchooseResource(int itemIndex)
    {
      _currentResourceId = itemIndex >= 0 ? (int)rGd.DataKeys[itemIndex] : -1;
      if (itemIndex >= 0)
        rGd.Rows[itemIndex].Activate();// = true;

      BindResource();
    }
    private string[][] getAttributeValues(RepeaterItem typeItem)
    {
      ResourceType type = resourceTypes[txtResourceTypeValue.SelectedIndex];

      string[][] attrs = new string[type.Attributes.Count][];
      if (typeItem != null)
      {
        Repeater variantAttributes = (Repeater)typeItem.FindControl("variantAttributes");
        if (variantAttributes != null)
          for (int i = 0; i < variantAttributes.Items.Count; i++)
          {
            VariantAttribute attrDef = type.Attributes.GetAttributeDefinition(i);
            TextBox attrValueBox = (TextBox)variantAttributes.Items[i].FindControl("attrValueBox");
            DropDownList attrValueList = (DropDownList)variantAttributes.Items[i].FindControl("attrValueList");

            if (attrDef != null && attrValueBox != null && attrValueList != null)
              attrs[i] = new string[] { attrDef.Name, attrDef.UserDefined ? attrDef.PossibleValues.Count == 0 ? attrValueBox.Text : attrValueList.SelectedValue : null };
          }
      }

      return attrs;
    }
  }

}
