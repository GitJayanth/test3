namespace HyperCatalog.UI.Acquire.QDE
{
  #region uses
  using System;
  using System.Data;
  using System.Drawing;
  using System.Web;
  using System.Web.UI.WebControls;
  using System.Web.UI.HtmlControls;
  using HyperCatalog.Business;
  using HyperCatalog.Shared;
  using Infragistics.WebUI.UltraWebGrid;
  #endregion
  /// <summary>
  /// Description résumée de controlItemSort.
  /// </summary>
  public partial class controlItemSort : System.Web.UI.UserControl
  {

    #region published properties of the control
    public HyperCatalog.Business.Item Item
    {
      set { ViewState["Item"] = value; FillBox(); }
      get { return ViewState["Item"] == null ? null : (HyperCatalog.Business.Item)ViewState["Item"]; }
    }
    public bool HasChanges
    {
      get { return ViewState["HasChanges"] == null ? false : (bool)ViewState["HasChanges"]; }
      set { ViewState["HasChanges"] = value; }
    }
    public int ItemListSort
    {
      get { return (int)ViewState["ItemListSort"]; }
      set { ViewState["ItemListSort"] = value; }
    }
    public ListItemCollection Items
    {
      get { return lstBoxSiblingItems.Items; }
    }

    public int SelectedIndex
    {
      get { return lstBoxSiblingItems.SelectedIndex; }
    }
    public bool Enabled
    {
      set { lstBoxSiblingItems.Disabled = !value; BtnDown.Enabled = BtnUp.Enabled = value; }
    }
    #endregion


    protected void Page_Load(object sender, System.EventArgs e)
    {
      if (!Page.IsPostBack)
      {
        ItemListSort = -1;
        lstBoxSiblingItems.Items.Clear();
      }
      EnableSortButtons();
    }

    private void FillBox()
    {
      ItemListSort = -1;
      lstBoxSiblingItems.Items.Clear();
      if (Item != null)
      {
        using (HyperComponents.Data.dbAccess.Database dbObj = Utils.GetMainDB())
        {
          using (DataTable siblingItems = dbObj.RunSPReturnDataSet("_Item_GetSiblingItems", new System.Data.SqlClient.SqlParameter("@ItemId", Item.Id)).Tables[0])
          {
            dbObj.CloseConnection();
            if (siblingItems != null && siblingItems.Rows.Count > 0)
            {
              lstBoxSiblingItems.Size = siblingItems.Rows.Count + 1;
            }
            if (siblingItems.Rows.Count > 14) lstBoxSiblingItems.Size = 15;
            for (int i = 0; i < siblingItems.Rows.Count; i++)
            {
              string itemName = siblingItems.Rows[i]["ItemName"].ToString();
              string itemNumber = siblingItems.Rows[i]["ItemNumber"].ToString();
              itemName = itemNumber.Length > 0 ? "[" + itemNumber + "] - " + itemName : itemName;
              ListItem newItem = new ListItem("[" + siblingItems.Rows[i]["LevelId"].ToString() + "] - " + itemName, siblingItems.Rows[i]["ItemId"].ToString().ToString());
              if (newItem.Text.Length > 80) newItem.Text = newItem.Text.Substring(0, 49) + "...";
              lstBoxSiblingItems.Items.Add(newItem);
            }
          }
        }
      }
    }


    public void SelectItem(System.Int64 itemId)
    {
      foreach (ListItem item in lstBoxSiblingItems.Items)
      {
        item.Attributes.Clear();
      }

      if (lstBoxSiblingItems.Items.FindByValue(itemId.ToString()) != null)
      {
        lstBoxSiblingItems.Items.FindByValue(itemId.ToString()).Selected = true;
        lstBoxSiblingItems.Items.FindByValue(itemId.ToString()).Attributes.Add("class", "lstSelectedItem");
        ItemListSort = SelectedIndex;
        EnableSortButtons();
      }
    }
    public void AddItem(System.Int64 itemId, string itemName)
    {
      ListItem newItem = new ListItem(itemName, itemId.ToString());
      if (newItem.Text.Length > 80) newItem.Text = newItem.Text.Substring(0, 49) + "...";
      if (ItemListSort < 0 || ItemListSort >= lstBoxSiblingItems.Items.Count)
      {
        lstBoxSiblingItems.Items.Add(newItem);
        ItemListSort = lstBoxSiblingItems.Items.Count - 1;
      }
      else
      {
        lstBoxSiblingItems.Items.Insert(ItemListSort, newItem);
      }
      lstBoxSiblingItems.SelectedIndex = ItemListSort;
      SelectItem(itemId);
    }

    protected void BtnUp_Click(object sender, System.EventArgs e)
    {
      if (ItemListSort > 0)
      {
        ListItem moveItem = lstBoxSiblingItems.Items[ItemListSort];
        lstBoxSiblingItems.Items.RemoveAt(ItemListSort);
        ItemListSort--;
        lstBoxSiblingItems.Items.Insert(ItemListSort, moveItem);
        BtnDown.Enabled = true;
        HasChanges = true;
      }
      if (ItemListSort == 0)
      {
        BtnUp.Enabled = false;
      }
      lstBoxSiblingItems.SelectedIndex = ItemListSort;
    }

    protected void BtnDown_Click(object sender, System.EventArgs e)
    {
      if (ItemListSort < lstBoxSiblingItems.Items.Count)
      {
        ListItem moveItem = lstBoxSiblingItems.Items[ItemListSort];
        lstBoxSiblingItems.Items.RemoveAt(ItemListSort);
        ItemListSort++;
        lstBoxSiblingItems.Items.Insert(ItemListSort, moveItem);
        BtnUp.Enabled = true;
        HasChanges = true;
      }
      if (ItemListSort == lstBoxSiblingItems.Items.Count - 1)
      {
        BtnDown.Enabled = false;
      }
      lstBoxSiblingItems.SelectedIndex = ItemListSort;
      BtnUp.Enabled = ItemListSort > 0;
      BtnDown.Enabled = ItemListSort < lstBoxSiblingItems.Items.Count - 1;
    }

    public void SaveSort()
    {
      // Ensure sort is correct
      for (int i = this.Items.Count - 1; i >= 0; i--)
      {
        System.Int64 sortId = Convert.ToInt64(this.Items[i].Value);
        Item.ChangeSort(sortId, i, SessionState.User.Id);
      }
    }
    private void EnableSortButtons()
    {
      #region Enable/Disable the sorting buttons
      if (!lstBoxSiblingItems.Disabled)
      {
        BtnUp.Enabled = ItemListSort > 0;
        BtnDown.Enabled = ItemListSort < lstBoxSiblingItems.Items.Count - 1;
      }
      #endregion
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
    ///		Méthode requise pour la prise en charge du concepteur - ne modifiez pas
    ///		le contenu de cette méthode avec l'éditeur de code.
    /// </summary>
    private void InitializeComponent()
    {

    }
    #endregion
  }
}
