<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" AutoEventWireup="true"
  CodeFile="TR_Properties.aspx.cs" Inherits="UI_Globalize_TR_TR_Properties" %>

<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
  TR properties</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">

  <script>
    var commentField = null;
    function uwToolbar_Click(oToolbar, oButton, oEvent)
	  {
	    if (oButton.Key == 'DeleteLanguages')
			{
			  oEvent.cancelPostBack = !confirm('are you sure?');
	}
	//Added new function to delete TR with chunks for QC - 6162 by Nisha
	if (oButton.Key == 'DeleteChunks') {
	    if (document.getElementById(commentField).value != '') {
	        oEvent.cancelPostBack = !confirm('Are you sure you want to delete TR with Chunks?');
	    }
	    else {
	        oEvent.cancelPostBack = true;
	        var divDel = document.getElementById('divDelete');
	        var divView = document.getElementById('divView');
	        divView.style.display = 'none';
	        divView.style.visibility = 'hidden';
	        divDel.style.display = 'block';
	        divDel.style.visibility = 'visible';
	    }
	}
	    if (oButton.Key == 'Delete')
			{
			  if (document.getElementById(commentField).value != ''){
			      oEvent.cancelPostBack = !confirm('are you sure and do you want to keep the translated chunks?');
			  }
			  else{
			    oEvent.cancelPostBack = true;
			    var divDel = document.getElementById('divDelete');
			    var divView = document.getElementById('divView');
			    divView.style.display='none';
			    divView.style.visibility='hidden';			    
			    divDel.style.display='block';
			    divDel.style.visibility='visible';			    
			  }
    	}
      if (oButton.Key == 'Close')
		  {
			  oEvent.cancelPostBack = true;
			  window.close();
		  }
		}
		
	function ReloadParent()
 	{
		alert("Thanks, all users concerned will be notified by email in a few minutes\nThis window will now close");
	  top.opener.document.getElementById("action").value = "reload";
    top.opener.document.forms[0].submit();
		window.close();
	}
  </script>

</asp:Content>
<%--Removed the width property to fix enlarge buttons QC-7703 By Radha S--%>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
  <table style="width: 100%" border="0" cellpadding="0 " cellspacing="0">
    <tr height="1" valign="top">
      <td>
        <igtbar:UltraWebToolbar ID="uwToolbar" runat="server" ItemWidthDefault="80px"
          CssClass="hc_toolbar" BackgroundImage="" ImageDirectory="" OnButtonClicked="uwToolbar_ButtonClicked">
          <HoverStyle CssClass="hc_toolbarhover">
          </HoverStyle>
          <DefaultStyle CssClass="hc_toolbardefault">
          </DefaultStyle>
          <SelectedStyle CssClass="hc_toolbarselected">
          </SelectedStyle>
          <ClientSideEvents Click="uwToolbar_Click"></ClientSideEvents>
          <Items>
            <igtbar:TBarButton Key="Delete" Text="Delete TR" Image="/hc_v4/Img/ed_delete.gif">
            </igtbar:TBarButton>
            <igtbar:TBSeparator />
               <%--Added new Button delete TR with chunks for QC - 6162 by Nisha--%>
            <igtbar:TBarButton Key="DeleteChunks" Text="Delete TR With Chunks" Image="/hc_v4/Img/ed_delete.gif">
            <DefaultStyle Width="180px">
              </DefaultStyle>
            </igtbar:TBarButton>
            <igtbar:TBSeparator />
            <igtbar:TBarButton Key="DeleteLanguages" Text="Delete languages selected" Image="/hc_v4/Img/ed_delete.gif">
              <DefaultStyle Width="180px">
              </DefaultStyle>
            </igtbar:TBarButton>
            <igtbar:TBSeparator />
            <igtbar:TBarButton Key="Close" ToolTip="Close" Image="/hc_v4/Img/ed_cancel.gif">
            </igtbar:TBarButton>
          </Items>
        </igtbar:UltraWebToolbar>
      </td>
    </tr>
    <tr valign="top">
      <td>
        <div id="divDelete" style="display: none; visibility: hidden;">
          Please provide the reason why you need to delete this TR.<br />
          Please, do not abuse of this function.<br />
          <asp:TextBox Columns="80" TextMode="MultiLine" ID="txtDeleteComment" runat="server"
            Rows="3"></asp:TextBox>
        </div>
        <div id="divView" style="display: block; visibility: visible;">
          <asp:Label ID="lError" runat="server" Text="lError" CssClass="hc_error"></asp:Label>
          <table cellspacing="0" cellpadding="0" width="100%">
            <tr valign="middle">
              <td class="editLabelCell" width="100">
                <asp:Label ID="Label8" runat="server">TR Id</asp:Label></td>
              <td class="uga">
                <asp:Label ID="txtId" runat="server"></asp:Label></td>
            </tr>
            <tr valign="middle">
              <td class="editLabelCell" width="100">
                <asp:Label ID="Label4" runat="server">TR Name</asp:Label></td>
              <td class="uga">
                <asp:Label ID="txtName" runat="server"></asp:Label></td>
            </tr>
            <%--Modified by Deepak - PCF1 FrameWork project--%>
            <tr valign="middle">
              <td class="editLabelCell" width="100">
                <asp:Label ID="Label5" runat="server">Region Code</asp:Label></td>
              <td class="uga">
                <asp:Label ID="txtRegionCode" runat="server"></asp:Label></td>
            </tr>
            <tr valign="middle">
              <td class="editLabelCell" width="100">
                <asp:Label ID="Label2" runat="server">Creation date</asp:Label></td>
              <td class="ugd">
                <asp:Label ID="txtCreateDate" runat="server"></asp:Label></td>
            </tr>
            <tr valign="middle">
              <td class="editLabelCell" width="100">
                <asp:Label ID="Label7" runat="server">Comment</asp:Label></td>
              <td class="uga">
                <asp:Label ID="txtComment" runat="server"></asp:Label></td>
            </tr>
            <tr>
              <td class="editLabelCell" width="100">
                <asp:Label ID="label1" runat="server">Type</asp:Label></td>
              <td class="ugd">
                <asp:Label ID="txtType" runat="server"></asp:Label></td>
            </tr>
            <tr>
              <td class="editLabelCell" width="100">
                <asp:Label ID="Label3" runat="server">Instant</asp:Label></td>
              <td class="uga">
                <asp:CheckBox ID="cbInstant" runat="server" Enabled="false" />
            </tr>
            <tr>
              <td class="editLabelCell" width="100">
                <asp:Label ID="label6" runat="server">Instant comment</asp:Label></td>
              <td class="ugd">
                <asp:Label ID="txtInstantComment" runat="server"></asp:Label>&nbsp;
              </td>
            </tr>
            <tr valign="middle">
              <td colspan="2">
              <%--Removed the css property and added inline property to fix gridline issue QC-7703 By Radha S--%>
                <br />
                <igtbl:UltraWebGrid ID="dgLanguages" runat="server" Width="100%">
                  <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="Yes"
                    Version="4.00" SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle"
                    EnableInternalRowsManagement="True" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
                    CellClickActionDefault="RowSelect" NoDataMessage="No data to display">
                    <HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" Cursor="Hand" BackColor="LightGray" Font-Bold="true"> <%--CssClass="gh">--%>
                        <BorderDetails  StyleBottom="Solid" StyleRight="Solid" WidthRight="1pt" WidthBottom="1pt"/>
                    </HeaderStyleDefault>
                    <FrameStyle Width="100%" CssClass="dataTable">
                        <BorderDetails  StyleBottom="Solid" StyleRight="Solid" WidthRight="1pt" WidthBottom="1pt"/>
                    </FrameStyle>
                    <RowAlternateStyleDefault CssClass="uga">
                        <BorderDetails  StyleBottom="Solid" StyleRight="Solid" WidthRight="1pt" WidthBottom="1pt"/>
                    </RowAlternateStyleDefault>
                    <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                        <BorderDetails  StyleBottom="Solid" StyleRight="Solid" WidthRight="1pt" WidthBottom="1pt"/>
                    </RowStyleDefault>
                    <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd"
                ></ClientSideEvents>
                  </DisplayLayout>
                  <Bands>
                    <igtbl:UltraGridBand>
                      <Columns>
                        <igtbl:TemplatedColumn FooterText="" Key="Select" Width="20px">
                          <CellTemplate>
                            <asp:CheckBox ID="g_sd" onclick="javascript:return g_su(this);" runat="server"></asp:CheckBox>
                          </CellTemplate>
                          <HeaderTemplate>
                            <asp:CheckBox ID="g_ca" onclick="javascript:return g_su(this);" runat="server"></asp:CheckBox>
                          </HeaderTemplate>
                          <CellStyle VerticalAlign="Middle" Wrap="True">
                          </CellStyle>
                        </igtbl:TemplatedColumn>
                        <igtbl:UltraGridColumn BaseColumnName="LanguageCode" Hidden="true" Key="LanguageCode">
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="LanguageName" HeaderText="Language" Key="Language"
                          Width="100%">
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="ChunkCount" HeaderText="Chunk count" Width="80px">
                          <CellStyle HorizontalAlign="Center">
                          </CellStyle>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="WordCount" HeaderText="Word count" Key="WordCount"
                          Width="80px">
                          <CellStyle HorizontalAlign="Center">
                          </CellStyle>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="UploadCount" HeaderText="Upload count" Key="UploadCount"
                          Width="80px">
                          <CellStyle HorizontalAlign="Center">
                          </CellStyle>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="EndOfTranslation" HeaderText="End of translation"
                          Key="EndOfTranslation" Width="120px">
                        </igtbl:UltraGridColumn>
                      </Columns>
                    </igtbl:UltraGridBand>
                  </Bands>
                </igtbl:UltraWebGrid>
              </td>
            </tr>
            <tr valign="top" height="1">
              <td colspan="2">
                <fieldset>
                  <legend>
                    <asp:Label ID="Label11" runat="server">Created by</asp:Label>
                  </legend>
                  <asp:HyperLink ID="hlCreator" runat="server">HyperLink</asp:HyperLink>&nbsp; [<asp:Label
                    ID="lbOrganizationCreator" runat="server">Organization</asp:Label>
                ]
              </td>
            </tr>
          </table>
        </div>
      </td>
    </tr>
  </table>
    <script language="javascript" src="/hc_v4/js/hypercatalog_grid.js"></script>

  <script>
    window.onload=g_i
  </script>

</asp:Content>
