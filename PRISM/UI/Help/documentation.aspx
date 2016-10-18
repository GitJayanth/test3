<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Help.Documentation"
  CodeFile="documentation.aspx.cs" %>

<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="uc" TagName="ResourceAddUpd" Src="~/UI/DAM/ResourceAddUpd.ascx" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
  Help</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" runat="Server">

  <script type="text/javascript" language="javascript" src="/hc_v4/js/hypercatalog_grid.js"></script>

  <script language="C#" runat="server">
    private string WrapString(string longText, int maxLength)
    {
      if (longText != null && longText.Length > maxLength)
      {
        if (maxLength > 4)
          maxLength = maxLength - 4;

        int lastSpace = longText.Substring(0, maxLength).LastIndexOf(' ');
        string wrappedStr = lastSpace > 0 ? longText.Substring(0, lastSpace) : longText.Substring(0, maxLength);
        return wrappedStr + (maxLength > 0 ? " [...]" : "");
      }
      return longText;
    }

    private string highlightFilter(string text, string filter)
    {
      if (text != null)
        return Utils.CReplace(text, filter, "<font color=red><b>" + filter + "</b></font>", 1);
      return text;
    }

    private string sizeStr(HyperCatalog.Business.DAM.FileVersion file)
    {
      return file == null ? "-" : convertBytes((long)file.BinarySize);
    }

    private static string convertBytes(long bytes)
    {
      //Converts bytes into a readable 
      if (bytes >= 1073741824)
        return String.Format(Convert.ToString(bytes / 1024 / 1024 / 1024), "#0.00") + " GB";
      else if (bytes >= 1048576)
        return String.Format(Convert.ToString(bytes / 1024 / 1024), "#0.00") + " MB";
      else if (bytes >= 1024)
        return String.Format(Convert.ToString(bytes / 1024), "#0.00") + " KB";
      else if (bytes > 0 && bytes < 1024)
        return bytes + " Bytes";
      else
        return "0 Byte";
    }
  </script>

  <script type="text/javascript"><!--
	  function mainToolBar_Click(oToolbar, oButton, oEvent)
	  {
		  if (oButton.Key=="Add")
		  {
			  oEvent.cancelPostBack=true;
			  showHideAddPanel();
		  }
		  //Nisha verma added the alert message for QC 6401 to not to show the message when user hasnt selected anything and clicking on delete button(start)  
		        if (oButton.Key == 'Delete')
                {
                    var check=false;
                    myForm = document.forms[0];
                    for (i=0; i<myForm.length; i++)
                    {
                        if ((myForm.elements[i].id.indexOf('g_sd')!=-1)) 
                        {
                            if (!myForm.elements[i].disabled && myForm.elements[i].checked)
                            {
                                check=true;
                                break;
                            }
                        }
                    }
                   if(check)
                  {
                        oEvent.cancelPostBack = !confirm("You are about to delete the selected file.");
                        document.getElementById("addPanel").style.display = 'none';
        	            document.getElementById("addPanel").style.visibility = 'hidden';
                  }
              }
  //Nisha verma added the alert message for QC 6401 to not to show the message when user hasnt selected anything and clicking on delete button(end)
	  }
	  
	  function showHideAddPanel()
	  {
	    var addPanel = document.getElementById("addPanel").style;
	    addPanel.display = (addPanel.display == 'none'?'':'none');
	  }
	  
	  document.body.onload = function(){document.getElementById("resourceTypeRow").display=(<%= "document.getElementById('" + txtResourceTypeValue.ClientID + "')"%>.options.length>1?'':'none');};
  //-->
  </script>

    <script language="JavaScript" type="text/javascript">                       
         function CheckAll( checkAllBox )                   
         {                                                  
            var frm = document.forms['aspnetForm'];
            if (!frm) {
                frm = document.aspnetForm;
            }                          
          var ChkState=checkAllBox.checked;                 
          for(i=0;i< frm.length;i++)                        
          {                                                 
                 e=frm.elements[i];                         
                if(e.type=='checkbox' && e.name.indexOf('g_sd') != -1)
                    e.checked= ChkState ;                        
          }                                                      
         }                                                       

        function CheckChanged()                                  
        {                                                        
          var frm = document.forms['aspnetForm'];
            if (!frm) {
                frm = document.aspnetForm;
           }                              
          var boolAllChecked;                                    
          boolAllChecked=true;                                   
          for(i=0;i< frm.length;i++)                             
          {                                                      
            e=frm.elements[i];                                   
          if ( e.type=='checkbox' && e.name.indexOf('g_sd') != -1 )
              if(e.checked== false)                                 
              {                                                     
                 boolAllChecked=false;                              
                 break;                                             
              }                                                     
          }                                                         
          for(i=0;i< frm.length;i++)                                
          {                                                         
            e=frm.elements[i];                                      
            if ( e.type=='checkbox' && e.name.indexOf('checkAll') != -1 )
            {                                                   
              if( boolAllChecked==false)                        
                 e.checked= false ;                             
                 else                                           
                 e.checked= true;                               
              break;                                            
            }                                                   
           }                                                    
         }                                                      
         </script>  

  <script><!--
function clipBd(t){window.clipboardData.setData("Text",t);window.status="Link copied to clipboard: \""+t+"\"";return false;}
function typeUpd(l){if(l!=null){var pre="attrRow";for(var i=0;i<l.options.length;i++){var j=0;var tr=document.getElementById(pre+i+"_"+j);while(tr!=null){tr.style.display=l.options[i].selected?"":"none";tr=document.getElementById(pre+i+"_"+(++j));}}}}
function ref(f){var v=f.value;var i=v.lastIndexOf("\\",v.length-1);v=v.substring(i>0?i+1:0,v.lastIndexOf(".",v.length-1));var r=document.getElementById("<%=txtResourceNameValue.ClientID%>");if (r!=null && !r.disabled && r.value==""){r.value=v;}}
function cfmDel(r){return confirm("Do you really want to delete this variant for the resource "+r+" ?");}
function pTb_Click(oT,oB,oE){if(oB.Key=="Del"){oE.cancelPostBack=!(confirm("You are about to delete this resource and all its variants."));}}
//-->
  </script>

<style>
.lbl_msgInformation { FONT-WEIGHT: bold; COLOR: green }
.lbl_msgWarning { FONT-WEIGHT: bold; COLOR: orange }
.lbl_msgError { FONT-WEIGHT: bold; COLOR: red }
      .editOptionalLabelCell{
        border-right: 1pt double;
        border-top: 1pt double;
        font-size: 8pt;
        border-left: 1pt double;
        border-bottom: silver 1pt double;
        padding-left: 1px;
        padding-right: 10px;
        color: black;
        background-color: lightgrey;
      }
  </style>
  
  <table style="width: 100%; height: 100%" cellspacing="0" cellpadding="0" border="0">
    <tr valign="top">
      <td class="sectionTitle">
        Documentation
      </td>
    </tr>
    <tr valign="top" height="1">
      <td>
        <igtbar:UltraWebToolbar ID="mainToolBar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px" 
          Width="100%" OnButtonClicked="mainToolBar_ButtonClicked">
          <HoverStyle CssClass="hc_toolbarhover">
          </HoverStyle>
          <DefaultStyle CssClass="hc_toolbardefault">
          </DefaultStyle>
          <SelectedStyle CssClass="hc_toolbarselected">
          </SelectedStyle>
          <Items>
            <igtbar:TBarButton Key="Add" Text="Add" Image="/hc_v4/img/ed_new.gif" ToggleButton="true">
            </igtbar:TBarButton>
            <igtbar:TBSeparator />
            <igtbar:TBarButton Key="Delete" Text="Delete Selected" Image="/hc_v4/img/ed_delete.gif">
            <DefaultStyle Width="120px"></DefaultStyle>
            </igtbar:TBarButton>
            <igtbar:TBLabel Text=" ">
              <DefaultStyle Width="100%">
              </DefaultStyle>
            </igtbar:TBLabel>
          </Items>
          <ClientSideEvents Click="mainToolBar_Click"></ClientSideEvents>
        </igtbar:UltraWebToolbar>
        <div id="addPanel" style="display: none;">
          <asp:Label ID="propertiesMsgLbl" runat="server"></asp:Label>
          <fieldset>
            <legend>Add a documentation file</legend>
            <uc:ResourceAddUpd ID="UCresourceAddUpd" runat="server" Visible="false" LibraryId='<%# CurrentLibrary.Id%>'>
            </uc:ResourceAddUpd>
            <igtbar:UltraWebToolbar ID="pTb" runat="server" CssClass="hc_toolbar" Width="100%"
              ItemWidthDefault="70px" >
              <HoverStyle CssClass="hc_toolbarhover">
              </HoverStyle>
              <DefaultStyle CssClass="hc_toolbardefault">
              </DefaultStyle>
              <SelectedStyle CssClass="hc_toolbarselected">
              </SelectedStyle>
              <Items>
                <igtbar:TBarButton Key="Save" Image="/hc_v4/img/ed_save.gif" Text="Save" ToolTip="Save properties changes">
                </igtbar:TBarButton>
                <igtbar:TBSeparator Key="SepAdd"></igtbar:TBSeparator>
                <igtbar:TBLabel Text=" ">
                  <DefaultStyle Width="100%">
                  </DefaultStyle>
                </igtbar:TBLabel>
              </Items>
            </igtbar:UltraWebToolbar>
            <table cellspacing="0" cellpadding="0" width="100%" border="0">
              <tr valign="middle">
                <td class="editOptionalLabelCell" width="130">
                  <asp:Label ID="lbResourceName" Text="Name" runat="server"></asp:Label></td>
                <td class="ugd">
                  <asp:TextBox ID="txtResourceNameValue" runat="server" Width="330px"></asp:TextBox></td>
              </tr>
              <tr valign="middle">
                <td class="editOptionalLabelCell" width="130">
                  <asp:Label ID="lbResourceDescription" Text="Description" runat="server"></asp:Label></td>
                <td class="ugd">
                  <textarea id="txtResourceDescriptionValue" style="vertical-align: top" rows="5" cols="51"
                    runat="server"></textarea>
                </td>
              </tr>
              <tr valign="middle" id="resourceTypeRow">
                <td class="editOptionalLabelCell" width="130">
                  <asp:Label ID="lbResourceType" Text="Type" runat="server"></asp:Label></td>
                <td class="ugd">
                  <asp:DropDownList ID="txtResourceTypeValue" runat="server" DataValueField="Id" DataTextField="Name"
                    onchange="typeUpd(this);">
                  </asp:DropDownList>
                </td>
              </tr>
              <asp:Panel ID="variantAddProperties" runat="server">
                <tr valign="middle">
                  <td class="editLabelCell" style="text-transform: capitalize;" width="130">
                  </td>
                  <td class="ugd">
                    <asp:DropDownList ID="attrValueList" runat="Server" Visible="false"  DataValueField="Id" DataTextField="Name">
                    </asp:DropDownList>
                  </td>
                </tr>
                <tr valign="middle">
                  <td class="editLabelCell" style="width: 130px">
                    <asp:Label ID="lbFileBinary" Text="File" runat="server"></asp:Label></td>
                  <td class="ugd">
                    <asp:FileUpload ID = "txtFileBinaryValue" onchange="javascript:ref(this);" runat="server" />
                  </td>
                </tr>
              </asp:Panel>
            </table>
          </fieldset>
        </div>
      </td>
    </tr>
    <tr valign="top">
      <td>
        <asp:Label ID="MsgLbl" runat="server"></asp:Label>
        <asp:DataGrid ID="documents" runat="server" Width="100%" BorderColor="LightGray"
          DataKeyField="Id" AllowSorting="True" PageSize="500" AllowPaging="True" EnableViewState="true"
          AutoGenerateColumns="False" ShowFooter="False" GridLines="Both" CellSpacing="0"
          CellPadding="1">
          <ItemStyle CssClass="ugd" />
          <HeaderStyle Height="15px" VerticalAlign="Top" CssClass="gh" BorderStyle="Solid"
            BorderWidth="1pt"></HeaderStyle>
          <SelectedItemStyle CssClass="ugo"></SelectedItemStyle>
          <AlternatingItemStyle CssClass="uga"></AlternatingItemStyle>
          <EditItemStyle HorizontalAlign="Left"></EditItemStyle>
          <FooterStyle BorderWidth="0px"></FooterStyle>
          <PagerStyle Visible="False" HorizontalAlign="Center" Height="15px" BackColor="#cccccc"
            Mode="NumericPages"></PagerStyle>
          <Columns>
            <asp:TemplateColumn>
              <ItemStyle VerticalAlign="Middle" Wrap="True" Width="25px"></ItemStyle>
                  <HeaderTemplate>
                        <input type="checkbox" id="checkAll" onclick="javascript:CheckAll(this);" runat="server" name="checkAll">
                 </HeaderTemplate>
                 <ItemTemplate>
                    <asp:CheckBox ID="g_sd" runat="server" onclick="javascript:CheckChanged();" ></asp:CheckBox>
                 </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn SortExpression="Name">
              <ItemStyle VerticalAlign="Top" Width="40px"></ItemStyle>
              <ItemTemplate>
                <a href="<%# ((HyperCatalog.Business.DAM.Resource)Container.DataItem).URI%>">
                  <img src="<%# ((HyperCatalog.Business.DAM.Resource)Container.DataItem).URI%>?thumbnail=1&size=40"
                    width="40" height="40" border="0" /></a>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn SortExpression="Name">
              <ItemStyle VerticalAlign="Top" Width="350px"></ItemStyle>
              <HeaderTemplate>
                Name
              </HeaderTemplate>
              <ItemTemplate>
                <a href="<%# ((HyperCatalog.Business.DAM.Resource)Container.DataItem).URI%>">
                  <%# ((HyperCatalog.Business.DAM.Resource)Container.DataItem).Name %>
                </a>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
              <HeaderStyle HorizontalAlign="left"></HeaderStyle>
              <HeaderTemplate>
                Size
              </HeaderTemplate>
              <ItemStyle VerticalAlign="top" HorizontalAlign="left"></ItemStyle>
              <ItemTemplate>
                <%# sizeStr(((HyperCatalog.Business.DAM.Resource)Container.DataItem).MasterVariant.CurrentFile)%>
              </ItemTemplate>
            </asp:TemplateColumn>
          </Columns>
        </asp:DataGrid>
      </td>
    </tr>
  </table>

  <script type="text/javascript"><!--
  typeUpd(document.getElementById('<%Response.Write(txtResourceTypeValue.ClientID);%>'));
  //-->
  </script>

</asp:Content>
