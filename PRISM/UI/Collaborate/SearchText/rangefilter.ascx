<%@ Control Language="c#" Inherits="HyperCatalog.UI.RangeFilter" CodeFile="RangeFilter.ascx.cs"%>

<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
  <script type="text/javascript" src="../../../ajaxcore/ajax.js"></script>
  <script type="text/javascript" src="../../../ajaxcontrols/AdvancedSearch.js"></script>
<script type="text/javascript" language="javascript">
	  var getGroupListAction = {
      delay: 100,
      queueMultiple: false,
      prepare: function(options)
        {
          if (options.length==2)
          {
            var value = options[0].options[options[0].selectedIndex].value;
            return value!=""?value:-2;
          }
          else if (options.length==3)
          {
            var value = options[2];
            return value!=""?value:-2;
          }
        },
      call: proxies.AdvancedSearch.GetContainerGroups,
      finish: function (status,options) {
        if (status==null)
        {
          options[0].options.length = 1;
          options[0].options[0] = new Option("empty","");
          options[0].disabled = true;
        }
        else
        {
          options[0].disabled = false;
          options[0].parentElement.innerHTML = "<SELECT name="+options[0].name+" id="+options[0].id+">"+status+"</SELECT>";
        }
        if (options.length==2)
        {
          options[1].disabled = options[0].disabled;
          options[1].parentElement.getElementsByTagName("INPUT")[1].disabled = false;
        }
        else
          options[1].parentElement.getElementsByTagName("INPUT")[0].disabled = options[0].disabled;
      },
      onException: proxies.alertException
    } // getGroupListAction
    
    function addContainerGroup(clientId, addButton)
    {
      var list = addButton.parentElement.firstChild.firstChild;
      eval("if ("+clientId+"FilterValueContainerGroup!=null)" +
      "{" +
        clientId+"HiddenContainerGroupIds.value+=\"/\"+list.options[list.selectedIndex].value;"+
        clientId+"FilterValueContainerGroup.value+=\"/\"+list.options[list.selectedIndex].text;"+
      "}");
      ajax.Start(getGroupListAction,[list,addButton]);
    }
    function delContainerGroup(clientId, delButton)
    {
      var list = delButton.parentElement.firstChild.firstChild;
      eval("if ("+clientId+"FilterValueContainerGroup!=null)" +
      "{" +
        "var hidden = "+clientId+"HiddenContainerGroupIds;" +
        "var value = "+clientId+"FilterValueContainerGroup;" +
        "var split = hidden.value.split(new RegExp(\"[/]\",\"g\"));" +
        "if (split.length > 1)" +
        "{" +
          "ajax.Start(getGroupListAction,[list,delButton,split[split.length-2]]);" +
          "hidden.value=hidden.value.substring(0,hidden.value.length - (split[split.length-1].length+1));" +
          
          "var index = value.value.lastIndexOf('/');" +
          "value.value=index>0?value.value.substr(0,index):\"\";" +

          "delButton.disabled = (value.value==\"\");" +
        "}}");
    }
</script>
<TABLE>
  <TR>
    <td width='<%= FilterName!=null?"30":"200"%>' valign="top">
      <asp:dropdownlist id="filterNames" Visible='<%# FilterName==null%>' runat="server" onchange='<%# this.ID + "basicFilterChanged(\""+this.ClientID+"\",this);"%>'>
      </asp:dropdownlist>
    </td>
    <td id='<%# this.ClientID + "comparative"%>' valign="top">
        <asp:dropdownlist id="Comparative_String" runat="server" Visible='<%# Type==ValueType.Undefined || Type==ValueType.String%>' onChange='<%# this.ID + "Comparative_Changed2(\""+this.ClientID+"\",this);"%>' OnSelectedIndexChanged="Filter_Changed">
          <asp:ListItem Text="Contains" Value="contains"/>
          <asp:ListItem Text="Starts with" Value="startswith"/>
          <asp:ListItem Text="Ends with" Value="endswith"/>
          <asp:ListItem Text="Equals" Value="equals"/>
          <asp:ListItem Text="Different from" Value="different"/>
          <asp:ListItem Text="Is empty" Value="empty"/>
          <asp:ListItem Text="Is not empty" Value="notempty"/>
          <asp:ListItem Text="Is in this list" Value="inlist"/>
        </asp:dropdownlist>
        <asp:dropdownlist id="Comparative_StringNeverEmpty" runat="server" Visible='<%# Type==ValueType.Undefined || Type==ValueType.StringNeverEmpty%>' onChange='<%# this.ID + "Comparative_Changed(\""+this.ClientID+"\",this);"%>' OnSelectedIndexChanged="Filter_Changed">
          <asp:ListItem Text="Contains" Value="contains"/>
          <asp:ListItem Text="Starts with" Value="startswith"/>
          <asp:ListItem Text="Ends with" Value="endswith"/>
          <asp:ListItem Text="Equals" Value="equals"/>
          <asp:ListItem Text="Different from" Value="different"/>
          <asp:ListItem Text="Is in this list" Value="inlist"/>
        </asp:dropdownlist>
        <asp:dropdownlist id="Comparative_Int" runat="server" Visible='<%# Type==ValueType.Undefined || Type==ValueType.Int%>' onChange='<%# this.ID + "Comparative_Changed(\""+this.ClientID+"\",this);"%>' OnSelectedIndexChanged="Filter_Changed">
          <asp:ListItem Text="Lesser than" Value="lesser"/>
          <asp:ListItem Text="Greater than" Value="greater"/>
          <asp:ListItem Text="Equals" Value="equals"/>
          <asp:ListItem Text="Different from" Value="different"/>
          <asp:ListItem Text="Is in this list" Value="inlist"/>
        </asp:dropdownlist>
        <asp:dropdownlist id="Comparative_DateTime" runat="server" Visible='<%# Type==ValueType.Undefined || Type==ValueType.DateTime%>' OnSelectedIndexChanged="Filter_Changed">
          <asp:ListItem Text="Before" Value="lesser"/>
          <asp:ListItem Text="After" Value="greater"/>
          <asp:ListItem Text="Equals" Value="equals"/>
          <asp:ListItem Text="Different from" Value="different"/>
        </asp:dropdownlist>
        <asp:dropdownlist id="Comparative_Chunk" runat="server" Visible='<%# Type==ValueType.Undefined || Type==ValueType.Chunk%>' onChange='<%# this.ID + "Comparative_Changed2(\""+this.ClientID+"\",this);"%>' OnSelectedIndexChanged="Filter_Changed">
          <asp:ListItem Text="Contains" Value="contains"/>
          <asp:ListItem Text="Starts with" Value="startswith"/>
          <asp:ListItem Text="Ends with" Value="endswith"/>
          <asp:ListItem Text="Equals" Value="equals"/>
          <asp:ListItem Text="Different from" Value="different"/>
          <asp:ListItem Text="ILB" Value="ILB"/>
          <asp:ListItem Text="Not ILB" Value="notILB"/>
          <asp:ListItem Text="Is in this list" Value="inlist"/>
        </asp:dropdownlist>
        <div id="ContainerGroupSpan" style="display:inline;vertical-align:top;" runat="server" Visible='<%# Type==ValueType.Undefined || Type==ValueType.ContainerGroup%>'>
          <span id="Comparative_ContainerGroupDiv" runat="server">
            <asp:dropdownlist id="Comparative_ContainerGroup" runat="server" AppendDataBoundItems="false" EnableViewState="false" DataTextField="Name" DataValueField="Id"></asp:dropdownlist>
          </span>
          &nbsp;
          <input type="button" id="addContainerGroupBtn" runat="server" value="+" onclick='<%# "addContainerGroup(\"" + ClientID + "\",this);"%>' />
          &nbsp;
          <input type="button" id="delContainerGroupBtn" runat="server" value="-" onclick='<%# "delContainerGroup(\"" + ClientID + "\",this);"%>' />
          <input type="hidden" id="HiddenContainerGroupIds" runat="server" value='<%# Filter!=null?Filter.ContainerGroupPath:"/"+ContainerGroupParentId.ToString()%>' />
        </div>
    </td>
    <td valign="top" style="padding-left:10px;">
      <asp:TextBox ID="FilterValueString" Runat="server" Visible='<%# Type==ValueType.Undefined || Type==ValueType.String || Type==ValueType.StringNeverEmpty || Type==ValueType.Int || Type==ValueType.Chunk%>' Text='<%# Value%>' OnTextChanged="Filter_Changed"></asp:TextBox>
      <span id="FilterValueStringListDiv" runat="server" Visible='<%# Type==ValueType.Undefined || Type==ValueType.String || Type==ValueType.StringNeverEmpty || Type==ValueType.Int || Type==ValueType.Chunk%>'>
        <asp:TextBox id="FilterValueStringList" runat="server" Text='<%# Value%>' Rows="8" Style="width:250px;" TextMode="MultiLine" OnTextChanged="Filter_Changed"></asp:TextBox>
        <asp:dropdownlist ID="FilterValueStringListDelimiter" Runat="server" Width="40" Style="vertical-align:top;" OnSelectedIndexChanged="Filter_Changed">
          <asp:ListItem Text="," Value=","></asp:ListItem>
          <asp:ListItem Text=";" Value=";"></asp:ListItem>
          <asp:ListItem Text="|" Value="|"></asp:ListItem>
          <asp:ListItem Text="CR" Value="CR"></asp:ListItem>
        </asp:dropdownlist>
      </span>
      <div id="FilterValueDateTimeDiv" runat="server" Visible='<%# Type==ValueType.Undefined || Type==ValueType.DateTime%>'>
        <igsch:WebDateChooser id="FilterValueDateTime" runat="server" Value='<%# Value%>' NullDateLabel="Empty" Editable="false" OnValueChanged="Filter_DateTimeChanged"></igsch:WebDateChooser>
      </div>
      <asp:TextBox ID="FilterValueContainerGroup" Runat="server" Visible='<%# Type==ValueType.Undefined || Type==ValueType.ContainerGroup%>' Text='<%# Value%>' Width="260px" OnTextChanged="Filter_Changed"></asp:TextBox>
    </td>
    <td width="150" valign="top" style="padding-left:10px;">
      <asp:Button ID="AddFilterBtn" Runat="server" Text="ADD  +" Visible='<%# FilterName==null%>' OnClick="AddFilterBtn_Click"></asp:Button>
      <asp:Button ID="DelFilterBtn" Runat="server" Text="REMOVE" Visible='<%# FilterName!=null%>' OnClick="DelFilterBtn_Click"></asp:Button>
    </td>
  </tr>
</table>
<script language="javascript">
<!--
  var <%= this.ClientID%>filterNames=document.getElementById('<%= filterNames.ClientID%>');
  var <%= this.ClientID%>FilterValueString=document.getElementById('<%= FilterValueString.ClientID%>');
  var <%= this.ClientID%>FilterValueStringList=document.getElementById('<%= FilterValueStringListDiv.ClientID%>');
  var <%= this.ClientID%>FilterValueDateTime=document.getElementById('<%= FilterValueDateTimeDiv.ClientID%>');
  var <%= this.ClientID%>FilterValueContainerGroup=document.getElementById('<%= FilterValueContainerGroup.ClientID%>');
  var <%= this.ClientID%>HiddenContainerGroupIds=document.getElementById('<%= HiddenContainerGroupIds.ClientID%>');
//-->
</script>