namespace HyperCatalog.UI
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
  using HyperCatalog.Shared;

    #region History
    /*=============Modification Details====================================
      Mod#1 Add ItemId Search
      Description: eZ# 71445
      Modfication Date:07/05/2007
      Modified by: Ramachandran*/
    #endregion
	
    /// <summary>
	/// Description résumée de RangeFilter.
	/// </summary>
	public partial class RangeFilter : System.Web.UI.UserControl
	{
    public enum ValueType
    {
      Undefined=-1,
      String=0,
      StringNeverEmpty=1,
      Int=2,
      DateTime=3,
      Chunk=4,
      ContainerGroup=5
    }

    [Serializable]
    public class FilterStruct
    {
      public string ID;
      public string Name;
      public ValueType Type;
      public string Comparative;
      public string Value;
      public string ContainerGroupPath;
      public string Delimiter;

      public FilterStruct(string id, string name, ValueType type, string comparative, string value, string delimiter)
      {
        ID = id;
        Name = name;
        Type = type;
        Comparative = comparative;
        Value = value;
        Delimiter = delimiter;
      }
      public string Check()
      {
        string message = null;

        if (Comparative != "empty" && Comparative != "notempty" &&
          Comparative != "ILB" && Comparative != "notILB" &&
          (Value == null || Value == string.Empty))
          message = "Filter cannot be empty.";
        else if (Comparative == "inlist" && Delimiter == null)
          message = "Delimiter cannot be empty.";

        return message;
      }
    }

    public string FilterName
    {
      get
      {
        return (string)ViewState["FilterName"];
      }
      set
      {
        ViewState["FilterName"] = value;
        if (value != null)
        {
          ListItem item = filterNames.Items.FindByText(value);
          if (item != null)
            item.Selected = true;
        }
      }
    }
    public string Comparative
    {
      get
      {
        return (String)ViewState["Comparative"];
      }
      set
      {
        ViewState["Comparative"] = value;
        if (value != null && Type != ValueType.Undefined)
          comparatives[(int)Type].SelectedValue = value;
      }
    }
    public ValueType Type
    {
      get
      {
        if (ViewState["Type"]==null)
          return Type = ValueType.Undefined;
        return (ValueType)ViewState["Type"];
      }
      set
      {
        ViewState["Type"] = value;
      }
    }
    public string Value
    {
      get
      {
        return (string)ViewState["value"];
      }
      set
      {
        ViewState["value"] = value;
      }
    }
    public int ContainerGroupParentId
    {
      get
      {
        return ViewState["ContainerGroupParentId"]!=null?(int)ViewState["ContainerGroupParentId"]:-1;
      }
      set
      {
        ViewState["ContainerGroupParentId"] = value;
      }
    }
    public string Delimiter
    {
      get
      {
        return (string)ViewState["delimiter"];
      }
      set
      {
        ViewState["delimiter"] = value;
        if (value != null)
          FilterValueStringListDelimiter.SelectedValue = value;
      }
    }
    public FilterStruct Filter
    {
      get
      {
        return (FilterStruct)ViewState["Filter"];
      }
      set
      {
        ViewState["Filter"] = value;
        if (value != null)
        {
          FilterName = value.Name;
          Type = value.Type;
          Comparative = value.Comparative;
          Value = value.Value;
          string[] split = value.ContainerGroupPath.Split('/');
          ContainerGroupParentId = Convert.ToInt32(split[split.Length-1]);
          Delimiter = value.Delimiter;
        }
      }
    }

    private bool _datasourceChanged = false;
    public object DataSource
    {
      get
      {
        return ViewState["datasource"];
      }
      set
      {
        ViewState["datasource"] = value;
        _datasourceChanged = true;
      }
    }
    private ValueType[] filterComparatives
    {
      get
      {
        ValueType[] _filterComparatives = (ValueType[])ViewState["filterComparatives"];
        if (_filterComparatives==null)
          return filterComparatives = new ValueType[0];
        return _filterComparatives;
      }
      set
      {
        ViewState["filterComparatives"] = value;
      }
    }
    private DropDownList[] comparatives;

    protected override void OnLoad(EventArgs e)
    {
      Comparative_ContainerGroup.SelectedValue = null;
      comparatives = new DropDownList[] { Comparative_String, Comparative_StringNeverEmpty, Comparative_Int, Comparative_DateTime, Comparative_Chunk, Comparative_ContainerGroup };

      System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
      ci.DateTimeFormat.ShortDatePattern = SessionState.User.FormatDate;
      ci.DateTimeFormat.LongDatePattern = SessionState.User.FormatDate;
      FilterValueDateTime.CalendarLayout.Culture = ci;

      base.OnLoad(e);
    }
    public override void DataBind()
    {
      Comparative_ContainerGroup.Items.Add("");

      base.DataBind();
       
      Comparative_ContainerGroup.DataSource = Business.ContainerGroup.GetAll("ContainerGroupParentId = " + ContainerGroupParentId.ToString());
      Comparative_ContainerGroup.DataBind();

      if (Comparative_ContainerGroup.Items.Count == 0)
      {
        Comparative_ContainerGroup.Items.Add(new ListItem("Empty", ""));
        Comparative_ContainerGroup.Enabled = false;
        addContainerGroupBtn.Disabled = true;
      }
      delContainerGroupBtn.Disabled = (FilterValueContainerGroup.Text == "");
    }

    protected override void OnPreRender(EventArgs e)
    {
      string script = "  var " + this.ID + "basicFilters=new Array(";
      for (int i=0;i<filterComparatives.Length;i++)
        script += (i==0?"":",") + (int)filterComparatives[i];
      script +=
        ");\r\n";

      #region Event thrown when the first DDL selection changed
      script +=
        "  function " + this.ID + "basicFilterChanged(clientID,list){\r\n" +
        "    var comparative=document.getElementById(clientID + 'comparative');\r\n" +

        // Show the appropriate comparative list
        "    for (var i=0;i<comparative.childNodes.length/2;i++)\r\n" +
        "      comparative.childNodes[i*2].style.display=(i==" + this.ID + "basicFilters[list.selectedIndex]?'':'none');\r\n" +
        "    if (comparative.childNodes[" + this.ID + "basicFilters[list.selectedIndex]*2].onchange!=null)\r\n" +
        "      eval(comparative.childNodes[" + this.ID + "basicFilters[list.selectedIndex]*2].onchange());\r\n" +
        "    else\r\n" +
        "      " + this.ID + "Comparative_Changed(clientID,comparative.childNodes[" + this.ID + "basicFilters[list.selectedIndex]*2]);\r\n" +
        "  }\r\n";
      #endregion
      #region Event thrown when the second DDL selection changed
      script +=
        "  function " + this.ID + "Comparative_Changed(clientID,list){\r\n" +
        "    list.parentElement.nextSibling.style.visibility='visible';\r\n" +
        "    if (list.tagName.toLowerCase() == 'select')\r\n" +
        "    {\r\n" +
        "      eval(\"if (\" + clientID + \"FilterValueContainerGroup != null)\" + clientID + \"FilterValueContainerGroup.style.display=\\'none\\';\");\r\n" +
        "      var inlist = list.options[list.options.length-1].selected;\r\n";
      if (Filter == null)
        script +=
          // Show/hide the string input
          "      eval(\"if (\" + clientID + \"FilterValueString != null)\" + clientID + \"FilterValueString.style.display=(" + this.ID + "basicFilters[\" + clientID + \"filterNames.selectedIndex]!=" + ((int)ValueType.DateTime).ToString() + " && !inlist?\\'\\':\\'none\\');\");\r\n" +
          // Show/hide the list input
          "      eval(\"if (\" + clientID + \"FilterValueStringList != null)\" + clientID + \"FilterValueStringList.style.display=(" + this.ID + "basicFilters[\" + clientID + \"filterNames.selectedIndex]!=" + ((int)ValueType.DateTime).ToString() + " && inlist?\\'\\':\\'none\\');\");\r\n" +
          // Show/hide the datetime input
          "      eval(\"if (\" + clientID + \"FilterValueDateTime != null)\" + clientID + \"FilterValueDateTime.style.display=(" + this.ID + "basicFilters[\" + clientID + \"filterNames.selectedIndex]==" + ((int)ValueType.DateTime).ToString() + "?\\'\\':\\'none\\');\");\r\n";
      else
        script +=
          // Show/hide the string input
          "      eval(\"if (\" + clientID + \"FilterValueString != null)\" + clientID + \"FilterValueString.style.display=(!inlist?\\'\\':\\'none\\');\");\r\n" +
          // Show/hide the list input
          "      eval(\"if (\" + clientID + \"FilterValueStringList != null)\" + clientID + \"FilterValueStringList.style.display=(inlist?\\'\\':\\'none\\');\");\r\n";
      script +=
        "    }\r\n" +
        "    else\r\n" +
        "    {\r\n" +
        // Hide the string input
        "      eval(\"if (\" + clientID + \"FilterValueString != null)\" + clientID + \"FilterValueString.style.display=\\'none\\';\");\r\n" +
        // Hide the list input
        "      eval(\"if (\" + clientID + \"FilterValueStringList != null)\" + clientID + \"FilterValueStringList.style.display=\\'none\\';\");\r\n" +
        // Hide the datetime input
        "      eval(\"if (\" + clientID + \"FilterValueDateTime != null)\" + clientID + \"FilterValueDateTime.style.display=\\'none\\';\");\r\n" +
        // Show the container group input
        "      eval(\"if (\" + clientID + \"FilterValueContainerGroup != null)\" + clientID + \"FilterValueContainerGroup.style.display=\\'\\';\");\r\n" +
        "    }\r\n";
      script += "  }\r\n";
      #endregion
      #region Event thrown when the second DDL selection changed when type is String or Chunk
      script +=
        "  function " + this.ID + "Comparative_Changed2(clientID,list){\r\n" +
        "    " + this.ID + "Comparative_Changed(clientID,list);\r\n" +
        "    list.parentElement.nextSibling.style.visibility=(list.options[list.options.length-2].selected || list.options[list.options.length-3].selected?'hidden':'visible');" +
        "  }\r\n";
      #endregion

      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), this.ID.ToString(), script, true);
      if (Filter == null)
        Page.ClientScript.RegisterStartupScript(this.GetType(), this.ClientID + "startup", this.ID + "basicFilterChanged('" + this.ClientID + "'," + this.ClientID + "filterNames);", true);
      else
        Page.ClientScript.RegisterStartupScript(this.GetType(), this.ClientID + "startup", this.ID + "Comparative_Changed('" + this.ClientID + "',document.getElementById('" + this.ClientID + "comparative').firstChild);", true);

      base.OnPreRender (e);
    }
    protected override void OnDataBinding(EventArgs e)
    {
      if (_datasourceChanged)
      {
        if (DataSource is string[][])
        {
          string[][] values = (string[][])DataSource;
          filterComparatives = new ValueType[values.Length];
          filterNames.Items.Clear();

          int i = 0;
          foreach (string[] v in values)
          {
            filterNames.Items.Add(new ListItem(v[1],v[0]));
            filterComparatives[i++] = (ValueType)Enum.Parse(typeof(ValueType),v[2]);
          }
        }
      }

      base.OnDataBinding (e);

      if (Value != String.Empty && Type == ValueType.DateTime)
        FilterValueDateTime.Value = DateTime.FromBinary((long)Convert.ToDouble(Value));
      //eZ# 71445 - Start 
      if (Type == ValueType.Int)
        FilterValueDateTime.Value = Value;
      //eZ# 71445 - End
    }

    public delegate void AddFilter(object sender, FilterStruct filter);
    public event AddFilter FilterAdded;
    public delegate void RemoveFilter(object sender);
    public event RemoveFilter FilterRemoved;
    public delegate void ChangeFilter(object sender, FilterStruct filter);
    public event ChangeFilter FilterChanged;

    private FilterStruct createFilter()
    {
      DropDownList comparative = comparatives[Filter==null?(int)filterComparatives[filterNames.SelectedIndex]:(int)Filter.Type];
      ValueType type = (ValueType)Enum.Parse(typeof(ValueType),comparative.ID.Split('_')[1]);
      string value = string.Empty;
      if (type == ValueType.DateTime)
        value = FilterValueDateTime.Value == null ? null : ((DateTime)FilterValueDateTime.Value).ToBinary().ToString();
      else if (type == ValueType.ContainerGroup)
        value = FilterValueContainerGroup.Text;
      else if (comparative.SelectedValue == "inlist")
        value = FilterValueStringList.Text;
      else 
        value = FilterValueString.Text;
      FilterStruct newFilterStruct = new FilterStruct(filterNames.SelectedValue, filterNames.SelectedItem.Text, type, comparative.SelectedValue, value, FilterValueStringListDelimiter.Text.Length > 0 ? FilterValueStringListDelimiter.SelectedValue : null);
      newFilterStruct.ContainerGroupPath = HiddenContainerGroupIds.Value;
      return newFilterStruct;
    }

    public void AddFilterBtn_Click(object sender, EventArgs e)
    {
      if (FilterAdded != null)
        FilterAdded(this, createFilter());
    }
    protected void DelFilterBtn_Click(object sender, EventArgs e)
    {
      if (FilterRemoved!=null)
        FilterRemoved(this);
    }
    protected void Filter_Changed(object sender, EventArgs e)
    {
      if (FilterChanged != null)
        FilterChanged(this, createFilter());
    }
    protected void Filter_DateTimeChanged(object sender, Infragistics.WebUI.WebSchedule.WebDateChooser.WebDateChooserEventArgs e)
    {
      if (FilterChanged != null)
        FilterChanged(this, createFilter());
    }
  }
}
