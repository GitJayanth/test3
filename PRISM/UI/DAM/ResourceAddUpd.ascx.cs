using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class UI_DAM_ResourceAddUpd : System.Web.UI.UserControl
{
  private int _LibraryId = -1;
  private int _WorkspaceId = -1;
  private int _ResourceId = -1;
  private HyperCatalog.Business.DAM.ResourceType[] _resourceTypes;

  public int LibraryId
  {
    get { return _LibraryId; }
    set { _LibraryId = value; }
  }
  public int WorkspaceId
  {
    get { return _WorkspaceId; }
    set { _WorkspaceId = value; }
  }
  public int ResourceId
  {
    get { return _ResourceId; }
    set { _ResourceId = value; }
  }
  public HyperCatalog.Business.DAM.ResourceType[] resourceTypes
  {
    get
    {
      if (_resourceTypes == null)
        _resourceTypes = HyperCatalog.Business.DAM.ResourceType.GetAllForLibrary(_LibraryId);
      return _resourceTypes;
    }
  }

  protected void Page_Load(object sender, EventArgs e)
  {
    if (!IsPostBack)
      data_Load();
    data_Bind();

    resourceInformation.Visible = ddlCulture.Enabled = ResourceId >= 0;
    pTb.Items.FromKeyButton("Del").Visible = ResourceId >= 0;
    pTb.Items.FromKeyButton("CRT").Visible = ResourceId >= 0;// TODO: && isTemplate;
  }

  #region load & bind
  private void data_Load()
  {
    ddlResourceLibrary.DataSource = _WorkspaceId >= 0 ? HyperCatalog.Business.DAM.Library.GetAll(_WorkspaceId) : HyperCatalog.Business.DAM.Library.GetAll();
    ddlResourceType.DataSource = resourceTypes;
    rptResourceTypes.DataSource = resourceTypes;
    ddlCulture.DataSource = HyperCatalog.Business.Culture.GetAll();
  }
  private void data_Bind()
  {
    DataBind();
    if (_LibraryId >= 0)
      ddlResourceLibrary.SelectedValue = _LibraryId.ToString();
    ddlCulture.SelectedValue = HyperCatalog.Shared.SessionState.MasterCulture.Code;
  }
  protected void rptResourceTypes_ItemDataBound(object sender, RepeaterItemEventArgs e)
  {
    Repeater rptAttributes = (Repeater)e.Item.FindControl("rptAttributes");
    HyperCatalog.Business.DAM.ResourceType dataItem = (HyperCatalog.Business.DAM.ResourceType)e.Item.DataItem;
    if (rptAttributes != null && dataItem != null)
    {
      HyperCatalog.Business.DAM.VariantAttributeSet attributes = dataItem.Attributes;
      rptAttributes.DataSource = attributes;
      rptAttributes.DataBind();

      foreach (RepeaterItem item in rptAttributes.Items)
      {
        Label lblAutomatic = (Label)item.FindControl("lblAutomatic");
        TextBox txtAttribute = (TextBox)item.FindControl("txtAttribute");
        DropDownList ddlAttribute = (DropDownList)item.FindControl("ddlAttribute");

        HyperCatalog.Business.DAM.VariantAttribute attrDef = attributes.GetAttributeDefinition(item.ItemIndex);
        if (attrDef != null)
        {
          lblAutomatic.Visible = !attrDef.UserDefined;

          string value = attributes[item.ItemIndex];
          if (attrDef.UserDefined)
          {
            if (txtAttribute != null)
            {
              txtAttribute.Text = value;
              txtAttribute.Visible = attrDef.PossibleValues.Count == 0;
            }
            if (ddlAttribute != null)
            {
              ddlAttribute.DataSource = attrDef.PossibleValues;
              ddlAttribute.DataBind();
              ddlAttribute.SelectedValue = value != null ? value : attrDef.DefaultValue;
              ddlAttribute.Visible = attrDef.PossibleValues.Count > 0;
              //attrValueList.Enabled = attrDef.PossibleValues.Count>1;
            }
          }

        }
      }
    }
  }
  #endregion

  #region events
  protected void pTb_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
  {
    //Response.Write("<span style=\"color:red;\">pTb_ButtonClicked</span><BR/>");
    switch (be.Button.Key)
    {
      case "Save":
        if (HyperCatalog.Shared.SessionState.User.HasCapability(HyperCatalog.Business.CapabilitiesEnum.MANAGE_RESOURCES))
        {
          if (HyperCatalog.Shared.SessionState.User.IsReadOnly)
            return;

          if (rptResourceTypes.Items.Count > ddlResourceType.SelectedIndex)
          {
            if (txtResourceNameValue.Text == null || txtResourceNameValue.Text.Length == 0)
            {
              SetMessage(propertiesMsgLbl, "Resource must have a name", MessageLevel.Error);
              break;
            }
            string[][] attrs = getAttributeValues(rptResourceTypes.Items[ddlResourceType.SelectedIndex]);

            HyperCatalog.Business.DAM.Resource newResource = HyperCatalog.Business.DAM.Library.GetByKey(_LibraryId).AddResource(txtResourceNameValue.Text + System.IO.Path.GetExtension(txtFileBinaryValue.PostedFile.FileName), txtResourceDescriptionValue.Value, Convert.ToInt32(ddlResourceType.SelectedValue), HyperCatalog.Shared.SessionState.MasterCulture.Code, txtFileBinaryValue.PostedFile.InputStream, attrs);
            if (newResource != null)
            {
              SetMessage(propertiesMsgLbl, "Resource \"" + txtResourceNameValue.Text + "\" created", MessageLevel.Information);
              data_Load();
            }
            else
              SetMessage(propertiesMsgLbl, "Resource \"" + txtResourceNameValue.Text + "\" could not be created", MessageLevel.Error);
          }
        }
        break;
    }
  }
  #endregion

  private string[][] getAttributeValues(RepeaterItem typeItem)
  {
    HyperCatalog.Business.DAM.ResourceType type = resourceTypes[ddlResourceType.SelectedIndex];

    string[][] attrs = new string[type.Attributes.Count][];
    if (typeItem != null)
    {
      Repeater rptAttributes = (Repeater)typeItem.FindControl("rptAttributes");
      if (rptAttributes != null)
        for (int i = 0; i < rptAttributes.Items.Count; i++)
        {
          HyperCatalog.Business.DAM.VariantAttribute attrDef = type.Attributes.GetAttributeDefinition(i);
          TextBox txtAttribute = (TextBox)rptAttributes.Items[i].FindControl("txtAttribute");
          DropDownList ddlAttribute = (DropDownList)rptAttributes.Items[i].FindControl("ddlAttribute");

          if (attrDef != null && txtAttribute != null && ddlAttribute != null)
            attrs[i] = new string[] { attrDef.Name, attrDef.UserDefined ? attrDef.PossibleValues.Count == 0 ? txtAttribute.Text : ddlAttribute.SelectedValue : null };
        }
    }

    return attrs;
  }

  private enum MessageLevel : int
  {
    Information = 0,
    Warning = 1,
    Error = 2
  }
  private static string[] cssClassLevels = { "lbl_msgInformation", "lbl_msgWarning", "lbl_msgError" };
  private static void SetMessage(Label label, string text, MessageLevel level)
  {
    label.Page.ClientScript.RegisterStartupScript(label.Page.GetType(), "start", "document.getElementById('addPanel').style.display='';", true);
    label.Text = text;
    label.CssClass = cssClassLevels[(int)level];
    label.Visible = true;
  }
}
