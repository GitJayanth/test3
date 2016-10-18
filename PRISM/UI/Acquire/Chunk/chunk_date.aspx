<%@ Reference Page="~/ui/acquire/chunk/chunk.aspx" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.Chunk.Chunk_Date" CodeFile="Chunk_Date.aspx.cs" %>
<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="hc" TagName="chunkcomment" Src="Chunk_Comment.ascx"%>
<%@ Register TagPrefix="hc" TagName="chunkModifier" Src="Chunk_Modifier.ascx"%>
<%@ Register TagPrefix="hc" TagName="chunkbuttonbar" Src="Chunk_ButtonBar.ascx"%>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Chunk_date</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
      <script>
      top.window.focus();
      var toolBar;
		  function uwToolbar_Click(oToolbar, oButton, oEvent)
		  {
			  if (oButton.Key=='ilb')
			  {		
			    myForm = document.forms[0];
			    if (!oButton.getSelected())
			    {
			    
                 var ReqValidator = document.getElementById('ctl00_HOCP_requiredValue');	//2815
                 ReqValidator.enabled = true;//2815
                    for (i=0; i<myForm.length; i++)
                    {
                    //Enable all DateValue type so the value will be saved 
                     if ((myForm.elements[i].id.indexOf('dateValue')!=-1))
                      myForm.elements[i].disabled = false;    
                      
                        if ((myForm.elements[i].id.indexOf('ctl00_HOCP_dateValue_input')!=-1))
                        {
                            var date = new Date();
                            var d  = date.getDate();
                            var day = (d < 10) ? '0' + d : d;
                            var m = date.getMonth() + 1;
                            var month = (m < 10) ? '0' + m : m;
                            var yy = date.getYear();
                            var year = (yy < 1000) ? yy + 1900 : yy;
                            myForm.elements[i].disabled = false;                        
                            myForm.elements[i].value = month + '/' + day + '/' + year;
                            myForm.elements[i].text = month + '/' + day + '/' + year;
                            myForm.elements[i].focus();
                            isBlankChunk = false;
                            if (typeof(top.ChunkIsChanged == 'function'))
                                {     
                                top.ChunkIsChanged();
                                }
                         }  
                    }
                 }
                 else
                 {
                 //ACQ10.0 Starts
                 var ReqValidator = document.getElementById('ctl00_HOCP_requiredValue');	//QC2815
                 ReqValidator.enabled = false;//QC2815
                 for (i=0; i<myForm.length; i++)
                    {
                    if ((myForm.elements[i].id.indexOf('dateValue')!=-1))
                        {
                        myForm.elements[i].disabled = true;
                        myForm.elements[i].value = "##BLANK##"; 
                 //ACQ10.0 Ends
                        isBlankChunk = true;
                        }  
                    }
                 }
			    return;			    
			  }
		  }
		  
      //ACQ10.0 Starts
	  function webdatechooser_Change()
	    {
	      var ReqValidator = document.getElementById('ctl00_HOCP_requiredValue');	//QC 2815
	        if (isBlankChunk == false)
	        {
	            //"Not a blank value so allowed to edit");
	             ReqValidator.enabled = true;//QC 2815
	            return true;
	        }
	        else
	        {
	            //"blank value so cannot edit");
	            
                ReqValidator.enabled = false;//QC 2815
	            myForm = document.forms[0];
	              for (i=0; i<myForm.length; i++)
                        {
	                     if ((myForm.elements[i].id.indexOf('ctl00_HOCP_dateValue_input')!=-1))
                            {
                            myForm.elements[i].disabled = true;
                            myForm.elements[i].value = "##BLANK##";
                            isBlankChunk = true;
                            }  
                        }
	            return false;
	        }
	    }
	    //ACQ10.0 Ends
      </script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">

<script id="Infragistics" type="text/javascript">
<!--
function dateValue_ValueChanged(oDropDown, newValue, oEvent)
 {
  if (typeof(top.ChunkIsChanged == 'function')){     
    top.ChunkIsChanged();
  }
}// -->
</script>
<script>
  document.body.onload = function(){
  try
   {
    webdatechooser_Change();
    }
    catch(e){}
    };
</script>
     <hc:chunkbuttonbar id="ChunkButtonBar" runat="server"></hc:chunkbuttonbar>
      <table style="WIDTH: 100%; BORDER-COLLAPSE: collapse;" cellspacing="0" cellpadding="0"
        border="0">
        <tr valign="top" style="height:1px">
          <td>
            <fieldset>
              <legend><asp:label id="Label2" runat="server">Value is</asp:label>&nbsp;<asp:label id="lbStatus" runat="server">Label</asp:label>&nbsp;<asp:image id="imgStatus" runat="server" AlternateText="status" ImageAlign="Middle"></asp:image>&nbsp;
                <%-- uncommented the below line for ACQ10.0 so we cannot authore '' value from  --%>
                 <asp:requiredfieldvalidator id="requiredValue" runat="server" ErrorMessage="Please enter a value" ControlToValidate="dateValue"
                  Display="Dynamic"></asp:requiredfieldvalidator></legend> 
              <table cellspacing="0" cellpadding="0" width="100%" border="0" height=1>
                <tr valign="top" bgColor="#d6d3ce">
                  <td><igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ItemWidthDefault="80px" width="100%" ImageDirectory=" "
                      CssClass="hc_toolbar2">
                      <HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
                      <DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
                      <SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
                      <ClientSideEvents Click="uwToolbar_Click"></ClientSideEvents>
                      <SelectedStyle BorderWidth="1px" BorderStyle="Inset" ForeColor="Red" BackColor="Gainsboro"></SelectedStyle>
                      <Items>
                        <%--ACQ10.0
                        <igtbar:TBarButton ToggleButton="True" Key="ilb" ToolTip="Force this chunk to be intentionaly left blank"
                          Text="Blank" Image="/hc_v4/img/SM.gif"></igtbar:TBarButton> 
                          --%>
                          <igtbar:TBarButton ToggleButton="True" Key="ilb" ToolTip="Force this chunk to be intentionaly left blank"
                          Text="Blank" Image="/hc_v4/img/ed_blank.gif"></igtbar:TBarButton> 
                      </Items>
                    </igtbar:ultrawebtoolbar></td>
                </tr>
                <tr valign="top">
                  <td><igsch:webdatechooser id="dateValue" runat="server" Font-Size="xx-small" Height="8px" Width="90px" NullDateLabel=" " onclick="javascript:webdatechooser_Change();">
                      <CalendarLayout cellspacing="1" PrevMonthImageUrl="ig_cal_grayP.gif"
                        ShowTitle="False" NextMonthImageUrl="ig_cal_grayN.gif">
                        <DayStyle BorderWidth="0px" BorderColor="White" BorderStyle="Solid" BackColor="#D0D0D0"></DayStyle>
                        <FooterStyle Height="12pt" BorderWidth="1px" Font-Size="XX-Small" BorderColor="#D8D8D8" BorderStyle="Solid"
                          ForeColor="#303030" BackColor="#B0B0B0">
                          <BorderDetails ColorBottom="0, 96, 96, 96" ColorRight="0, 96, 96, 96"></BorderDetails>
                        </FooterStyle>
                        <SelectedDayStyle Font-Bold="True" ForeColor="White" BackColor="#606060"></SelectedDayStyle>
                        <OtherMonthDayStyle ForeColor="Gray"></OtherMonthDayStyle>
                        <NextPrevStyle Width="10px" BorderWidth="1px" BorderColor="#D8D8D8" BorderStyle="Solid" BackColor="#B0B0B0">
                          <BorderDetails ColorBottom="0, 112, 112, 112" ColorRight="0, 112, 112, 112" WidthBottom="2px"></BorderDetails>
                        </NextPrevStyle>
                        <CalendarStyle BorderWidth="1px" Font-Size="XX-Small" Font-Names="Tahoma" BorderColor="#505050" BorderStyle="Solid"
                          ForeColor="Black" BackColor="#B0B0B0"></CalendarStyle>
                        <WeekendDayStyle ForeColor="#505050"></WeekendDayStyle>
                        <TodayDayStyle Font-Bold="True" BackColor="Silver"></TodayDayStyle>
                        <DayHeaderStyle Height="1pt" Font-Size="XX-Small" Font-Italic="True" BorderStyle="None" BackColor="#B0B0B0"></DayHeaderStyle>
                        <TitleStyle Height="16px" Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#909090">
                          <BorderDetails StyleBottom="Solid" ColorBottom="0, 128, 128, 128" WidthBottom="1px"></BorderDetails>
                        </TitleStyle>
                      </CalendarLayout>
                      <DropButton ImageUrl2="/hc_v4/img/igsch_xpbluedn.gif" ImageUrl1="/hc_v4/img/igsch_xpblueup.gif"></DropButton>
                    <ClientSideEvents ValueChanged="dateValue_ValueChanged" />
                    </igsch:webdatechooser>
                    </td>
                </tr>
              </table>
              <div id="textcount"></div>
            </fieldset>
          </td>
        </tr>
        <tr valign="top" style="height:1px">
          <td><hc:chunkcomment id="ChunkComment1" runat="server"></hc:chunkcomment></td>
        </tr>
        <tr valign="top" style="height:1px">
          <td><hc:chunkmodifier id="ChunkModifier1" runat="server"></hc:chunkmodifier>
            <center><asp:label id="lbResult" runat="server" Font-Size="7pt">Result message</asp:label></center>
          </td>
        </tr>
      </table>
</asp:Content>