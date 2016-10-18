using System;

using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HyperCatalog.UI.WebControls
{
  /// <summary>
  /// Description résumée de DayFlagsChooser.
  /// </summary>
	public class CheckBoxGroupedList : WebControl, IPostBackDataHandler
	{
    public GroupsCollection Groups
    {
      get
      {
        GroupsCollection _groups = (GroupsCollection)ViewState["Groups"];
        if (_groups == null)
          Groups = new GroupsCollection();

        return _groups;
      }
      set
      {
        ViewState["Groups"] = value;
      }
    }


    protected override void CreateChildControls()
    {
      base.CreateChildControls ();

      int i = 0;
      foreach (ListItemGroup group in Groups)
      {
        JSCheckBox groupChkBox = new JSCheckBox();
        groupChkBox.ID = ClientID + "_" + i;
        Controls.Add(groupChkBox);
        groupChkBox.JSOnClick = "javascript:CheckBoxGroupedList_checkGroup('"+groupChkBox.ClientID +"',"+group.Items.Count+",this.checked);";
        groupChkBox.Text = group.Text;
        bool groupSelected = true;

        Controls.Add(new LiteralControl("<BR/><TABLE style='float:left;' cellpadding='0' cellspacing='0'><TR><TD width='10px' height='10px'></TD><TD width='10px' height='10px' style='border-left:1px solid black;border-bottom:1px solid black;font-size:0%;'>&nbsp;</TD></TR><TR><TD width='10px' height='10px'></TD><TD width='10px' height='10px'></TD></TR></TABLE>"));

        int j = 0;
        foreach (ListItem item in group.Items)
        {
          JSCheckBox itemChkBox = new JSCheckBox();
          itemChkBox.ID = ClientID + "_" + i + "_" + j;
          Controls.Add(itemChkBox);
          itemChkBox.JSOnClick = "javascript:CheckBoxGroupedList_checkItem(this.id,this.checked);";
          itemChkBox.Text = item.Text;
          itemChkBox.Checked = item.Selected;

          groupSelected = groupSelected && item.Selected;

          j++;
        }
        groupChkBox.Checked = group.Selected || groupSelected;

        Controls.Add(new LiteralControl("<BR/>"));

        i++;
      }
    }

    protected override void OnPreRender(EventArgs e)
    {
      Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"CheckBoxGroupedList","<script>function CheckBoxGroupedList_checkGroup(id,count,checked){for (var i=0;i<count;i++)document.getElementById(id + \"_\" + i).checked = checked;} function CheckBoxGroupedList_checkItem(id,checked){if (!checked){document.getElementById(id.substring(0,id.lastIndexOf('_'))).checked = false;}}</script>");
      Page.RegisterRequiresPostBack(this);

      base.OnPreRender (e);
    }


    public void ClearSelection()
    {
      foreach (ListItemGroup group in Groups)
      {
        group.Selected = false;
        foreach (ListItem item in group.Items)
          item.Selected = false;
      }
    }

    public void SelectAll()
    {
      foreach (ListItemGroup group in Groups)
      {
        group.Selected = true;
        foreach (ListItem item in group.Items)
          item.Selected = true;
      }
    }

    public bool Select(string value)
    {
      object item = FindByValue(value);
      if (item!=null)
      {
        if (item is ListItemGroup)
          ((ListItemGroup)item).Selected = true;
        else
          ((ListItem)item).Selected = true;
        return true;
      }
      return false;
    }

    public ListItemGroup FindGroupByValue(string value)
    {
      foreach (ListItemGroup group in Groups)
        if (group.Value == value)
          return group;
      return null;
    }

    public object FindByValue(string value)
    {
      foreach (ListItemGroup group in Groups)
        if (group.Value == value)
          return group;
        else
        {
          ListItem item = group.Items.FindByValue(value);
          if (item!=null)
            return item;
        }
      return null;
    }


    #region Membres de IPostBackDataHandler

    public void RaisePostDataChangedEvent()
    {
      // TODO : ajoutez l'implémentation de CheckBoxGroupedList.RaisePostDataChangedEvent
    }

    public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
    {
      int i = 0;
      foreach (ListItemGroup group in Groups)
      {
        group.Selected = (postCollection.Get(FindControl(ClientID+"_"+i).UniqueID)!=null);
        int j = 0;
        foreach (ListItem item in group.Items)
        {
          item.Selected = (postCollection.Get(FindControl(ClientID+"_"+i+"_"+j).UniqueID)!=null);
          j++;
        }
        i++;
      }

      return false;
    }

    #endregion
  }

  public class GroupsCollection : /*IList, */ICollection, IEnumerable, IStateManager
  {
    private ArrayList itemLists = new ArrayList();

    #region Membres de IList

    public bool IsReadOnly
    {
      get
      {
        return itemLists.IsReadOnly;
      }
    }

    public ListItemGroup this[int index]
    {
      get
      {
        return (ListItemGroup)itemLists[index];
      }
      set
      {
        itemLists[index] = value;
      }
    }

    public void RemoveAt(int index)
    {
      itemLists.RemoveAt(index);
    }

    public void Insert(int index, ListItemGroup value)
    {
      itemLists.Insert(index,value);
    }

    public void Remove(ListItemGroup value)
    {
      itemLists.Remove(value);
    }

    public bool Contains(ListItemGroup value)
    {
      return itemLists.Contains(value);
    }

    public void Clear()
    {
      itemLists.Clear();
    }

    public int IndexOf(ListItemGroup value)
    {
      return itemLists.IndexOf(value);
    }

    public int Add(ListItemGroup value)
    {
      return itemLists.Add(value);
    }

    public bool IsFixedSize
    {
      get
      {
        return itemLists.IsFixedSize;
      }
    }

    #endregion

    #region Membres de ICollection

    public bool IsSynchronized
    {
      get
      {
        return itemLists.IsSynchronized;
      }
    }

    public int Count
    {
      get
      {
        return itemLists.Count;
      }
    }

    public void CopyTo(Array array, int index)
    {
      itemLists.CopyTo(array,index);
    }

    public object SyncRoot
    {
      get
      {
        return itemLists.SyncRoot;
      }
    }

    #endregion

    #region Membres de IEnumerable

    public IEnumerator GetEnumerator()
    {
      return itemLists.GetEnumerator();
    }

    #endregion

    #region Membres de IStateManager

    public void TrackViewState()
    {
      _isTrackingViewState = true;
    }

    private bool _isTrackingViewState = false;
    public bool IsTrackingViewState
    {
      get
      {
        return _isTrackingViewState;
      }
    }

    public object SaveViewState()
    {
      ArrayList viewStates = new ArrayList();
      foreach (ListItemCollection items in this)
        viewStates.Add(((IStateManager)items).SaveViewState());
      return viewStates;
    }

    public void LoadViewState(object state)
    {
      if (state is ArrayList)
      {
        int i = 0;
        foreach (object obj in (ArrayList)state)
          ((IStateManager)this[i]).LoadViewState(obj);
      }
    }

    #endregion
  }

  public class ListItemGroup : WebControl
  {
    private string _text;
    public string Text
    {
      get
      {
        return _text;
      }
      set
      {
        _text = value;
      }
    }

    private string _value;
    public string Value
    {
      get
      {
        return _value == null?Text:_value;
      }
      set
      {
        _value = value;
      }
    }

    private bool _selected;
    public bool Selected
    {
      get
      {
        return _selected;
      }
      set
      {
        _selected = value;
      }
    }


    private ListItemCollection _items = new ListItemCollection();
    public ListItemCollection Items
    {
      get
      {
        return _items;
      }
      set
      {
        _items = value;
      }
    }


    protected override void CreateChildControls()
    {
      base.CreateChildControls ();
    }


    #region Membres de IStateManager
    protected override object SaveViewState()
    {
      return new Pair(Text,((IStateManager)Items).SaveViewState());
    }

    protected override void LoadViewState(object state)
    {
      if (state is Pair)
        if (((Pair)state).First is string)
        {
          Text = (string)((Pair)state).First;
          ((IStateManager)Items).LoadViewState(((Pair)state).Second);
        }
    }

    #endregion
  }
}
