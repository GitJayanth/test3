<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Globalize.Translation.CostPlanning" CodeFile="CostPlanning.aspx.cs"%>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
    <table cellspacing="0" cellpadding="0" width="100%" border="0">
      <tr>
        <td class="hc_pagetitle">Translation cost planning</td>
        <td>
          <asp:ImageButton id="backButton" runat="server" visible="False" ImageUrl="/hc_v4/img/ed_back.gif" ToolTip="Back"></asp:ImageButton>
        </td>
      </tr>
    </table>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
  <script language="javascript" type="text/javascript">
	<!--
	  function calculate()
	  {
	    //var grid = 
	  }
	  
	  var grids = new Array();
	  function declareGrid(index,gridName)
	  {
	    //var grid = igtbl_getGridById(gridName);
/*	    if (index==0)
	      alert(grid);
	   */ //grids[index] = grid;
	  }
	  
    function valueChangeInEdit(oEdit, oldValue, oEvent)
    {
      //alert(oEdit.getValue());
      //oEvent.needPostBack = true;
/*      if(oldValue == "ok" && oEdit.getValue() == "")
    	  if(oEdit.getText() == "post me to server")
    		  oEvent.needPostBack = true;*/
    }
	//-->
	</script>

<table style="WIDTH: 100%; HEIGHT: 100%" cellspacing="0" cellpadding="0" border="0">
  <tr valign="top">
    <td>
      <asp:Label id="mainMsgLbl" runat="server"></asp:Label>
      <fieldset><legend>Average value settings</legend>
      <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr><td colspan="2">
          <igtbar:ultrawebtoolbar id="mainToolBar" runat="server" ItemWidthDefault="80px" CssClass="hc_toolbar">
				    <HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
				    <DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
				    <SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
				    <Items>
					    <igtbar:TBarButton Key="Save" Text="Save" Image="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
				    </Items>
			    </igtbar:ultrawebtoolbar>
        </td></tr>
        <tr valign="middle">
          <td class="editLabelCell" style="width:130px">
            <asp:label id="lbProjectCost" Text="Cost per project" Runat="server"></asp:label></td>
          <td class="ugd">
            <asp:TextBox id="projectCostValue" runat="server"></asp:TextBox>$ 
          </td></tr>
        <tr valign="middle">
          <td class="editLabelCell" style="width:130px">
            <asp:label id="lbWordCost" Text="Cost per word" Runat="server"></asp:label></td>
          <td class="ugd">
            <asp:TextBox id="wordCostValue" runat="server"></asp:TextBox>$ 
          </td></tr>
        <tr valign="middle">
          <td class="editLabelCell" style="width:130px">
            <asp:label id="lbForecastRange" Text="Forecast range (months)" Runat="server"></asp:label></td>
          <td class="ugd">
            <asp:RadioButtonList id="forecastRangeValue" runat="server" autopostback="true" repeatdirection="horizontal">
					    <asp:ListItem text="3" value="3"></asp:ListItem>
					    <asp:ListItem text="6" value="6"></asp:ListItem>
					    <asp:ListItem text="9" value="9"></asp:ListItem>
					    <asp:ListItem text="12" value="12"></asp:ListItem>
					  </asp:RadioButtonList>
					 </td>
				</tr>
			</table>
		</fieldset> 
		
		<hr/>
          <asp:Repeater id="classRepeater" runat="server">
            <ItemTemplate>
              <table cellpadding="0" cellspacing="0" width="100%"><tr><td class="sectionSubTitle"><%# DataBinder.Eval(Container.DataItem,"Name")%>(#<%# DataBinder.Eval(Container.DataItem,"Id")%>)</td></tr></table> 
              <asp:Panel id="invisible" runat="server" visible="true">
                Average projects:  
                <asp:Label id="avgProjects" runat="server"></asp:Label>
                <br/>
                Average languages: 
                <asp:Label id="avgLanguages" runat="server"></asp:Label>
              </asp:Panel>
              <table border="0" cellpadding="0" cellspacing="0"><tr><td>
              <igtbar:ultrawebtoolbar id="gridToolBar" runat="server" ItemWidthDefault="80px" CssClass="hc_toolbar">
				        <HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
				        <DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
				        <SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
				        <Items>
				          <igtbar:TBarButton Key="Calculate" Text="Calculate" Image="/hc_v4/img/ed_sql.gif"></igtbar:TBarButton>
					        <igtbar:TBarButton Key="Save" Text="Save" Image="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
				        </Items>
			        </igtbar:ultrawebtoolbar>
			        </td></tr><tr><td>
			        <asp:DataGrid id="costGrid" runat="server" AutoGenerateColumns="False" Visible="true">
			          <headerstyle CssClass="dg_gh"/>
			          <itemstyle CssClass="ugd"/>
			          <AlternatingItemStyle CssClass="uga"/>
			          <Columns>
			            <asp:BoundColumn DataField="LevelName" HeaderText="Level"></asp:BoundColumn>
			            <asp:BoundColumn DataField="AvgWordCount" HeaderText="WordCount" Visible="True"></asp:BoundColumn>
			            <asp:TemplateColumn>
			              <HeaderTemplate>Total</HeaderTemplate>
			              <ItemTemplate><asp:Label id="totalTxt" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Total")%>' ></asp:Label>$</ItemTemplate>
			            </asp:TemplateColumn>
			          </Columns>
			        </asp:DataGrid>
			        </td></tr></table>
			      </ItemTemplate>
            <SeparatorTemplate><br/></SeparatorTemplate>
          </asp:Repeater></td></tr></table><br/>
  <script type="text/javascript" language="javascript">
	<!--
	  calculate();
	//-->
	</script>

</asp:Content>
