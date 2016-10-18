<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="QDE_Toolbar" CodeFile="QDE_Toolbar.aspx.cs" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">QDE_Toolbar</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOJS" runat="server">
      <script type="text/javascript" language="javascript">
      function UpdateFrames(c, title)
      {
				if (parent)
				{
					if (parent.frametv)
					{
						parent.frametv.location = '/hc_v4/dummy.htm';
						parent.frametv.location = "qde_tv.aspx?c="+c;
					}
					if (parent.framecontent)
					{
						parent.framecontent.location = '/hc_v4/dummy.htm';
						parent.framecontent.location = 'QDE_FormRoll.aspx?c='+c;
					}
        }  
      }           
      
      function UpdateTitle(title)
      {
				if (parent && parent.parent)
				{
					var divTitle = parent.parent.document.getElementById("QDEtitle");
					if (divTitle)
					{
						divTitle.innerHTML = title;
					}
				}
      }
      top.document.body.scroll = 'no';
      </script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
      <table class="selectlanguage" style="width: 100%; height: 100%" cellpadding="2" cellspacing="0" border="0">
        <tr valign="middle">
          <td align="right">
            <asp:DropDownList id="DDL_Cultures" runat="server" Width="99%" DataTextField="Name" DataValueField="Code"
              AutoPostBack="True" onselectedindexchanged="DDL_Cultures_SelectedIndexChanged"></asp:DropDownList>
          </td>
        </tr>
      </table>
</asp:Content>