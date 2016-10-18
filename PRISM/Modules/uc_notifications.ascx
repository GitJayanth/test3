<%@ Register TagPrefix="MM" Namespace="HyperCatalog.MasterModules" Assembly="HyperCatalog.MasterModules" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Control Language="c#" Inherits="hc_homePageModules.uc_notifications" CodeFile="uc_notifications.ascx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<script type="text/javascript">
<!--
		function uwgNotifications_CellClickHandler(gridName, cellId, button)
		{
			if (button==0) // left button
			{
				// get notification type of selected row
				var notificationTypeId=igtbl_getRowById(cellId).getCellFromKey("NotificationTypeId").getValue();
			
				// go to specified location
				if (notificationTypeId==2) // Acquisition
					document.location="UI/Acquire/Complete.aspx";
				else if (notificationTypeId==3) // Validation
					document.location="UI/Collaborate/Proofread.aspx";
				else if (notificationTypeId==4) // Globalization
					document.location="UI/Globalize/Translate.aspx";
				else if (notificationTypeId==5) // Translation
					document.location="UI/Globalize/Translate.aspx";
				//else if (notificationTypeId==6) // Localization
				//	document.location="";
				//else if (notificationTypeId==7) // Publication
				//	document.location="";
			}
		}
-->
</script>
<MM:mastermodule id="Module" runat="server" ModuleName="Notifications" ModuleTitle="Notifications">
	<table width="100%" cellspacing="0" cellpadding="0" border="0">
		<TR>
			<td>
				<igtbar:ultrawebtoolbar id="uwtNotifications" runat="server" ItemWidthDefault="80px" Width="100%" CssClass="hc_toolbar" OnButtonClicked="uwtNotifications_ButtonClicked">
					<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
					<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
					<Items>
						<igtbar:TBarButton Key="Previous" ToolTip="Previous notifications" DisabledImage="/hc_v4/img/btn_left_arrow_disable.gif"
							Image="/hc_v4/img/btn_left_arrow.gif">
							<DefaultStyle Width="20px"></DefaultStyle>
						</igtbar:TBarButton>
						<igtbar:TBarButton Key="Next" ToolTip="Next notifications" DisabledImage="/hc_v4/img/btn_right_arrow_disable.gif"
							Image="/hc_v4/img/btn_right_arrow.gif">
							<DefaultStyle Width="20px"></DefaultStyle>
						</igtbar:TBarButton>
						<igtbar:TBSeparator></igtbar:TBSeparator>
						<igtbar:TBLabel Text="&lt;b&gt;Notification date: &lt;/b&gt;">
							<DefaultStyle Width="130px"></DefaultStyle>
						</igtbar:TBLabel>
						<igtbar:TBLabel Text="lblNotificationDate" Key="lblNotificationDate"></igtbar:TBLabel>
					</Items>
					<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
				</igtbar:ultrawebtoolbar></td>
		</tr>
		<TR>
			<td>
				<igtbl:ultrawebgrid id="uwgNotifications" runat="server" Height="150px">
					<DisplayLayout ColWidthDefault="" AutoGenerateColumns="False" RowHeightDefault="100%" Version="4.00"
						SelectTypeRowDefault="Single" RowSelectorsDefault="No" Name="xctl0uwgNotifications" CellClickActionDefault="RowSelect"
						NoDataMessage="No notifications to display">
						<HeaderStyleDefault Width="150px" VerticalAlign="Middle" HorizontalAlign="Left" CssClass="gridHeader">
							<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
						</HeaderStyleDefault>
						<FrameStyle Width="100%" BorderStyle="None" Height="150px" CssClass="dataTable"></FrameStyle>
						<ClientSideEvents CellClickHandler="uwgNotifications_CellClickHandler" MouseOverHandler="dg_MouseOverHandler"
							MouseOutHandler="dg_MouseOutHandler"></ClientSideEvents>
						<ActivationObject AllowActivation="False"></ActivationObject>
						<SelectedRowStyleDefault CssClass="ugs"></SelectedRowStyleDefault>
						<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
						<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" Wrap="True" CssClass="ugd"></RowStyleDefault>
					</DisplayLayout>
					<Bands>
						<igtbl:UltraGridBand>
							<Columns>
								<igtbl:UltraGridColumn Key="NotificationTypeId" Hidden="True" BaseColumnName="NotificationTypeId">
									<Footer Key="NotificationTypeId"></Footer>
									<Header Key="NotificationTypeId"></Header>
								</igtbl:UltraGridColumn>
								<igtbl:UltraGridColumn HeaderText="Notification type" Key="NotificationType" Width="20%" BaseColumnName="NotificationType">
									<Footer Key="NotificationType"></Footer>
									<Header Key="NotificationType" Caption="Notification type"></Header>
								</igtbl:UltraGridColumn>
								<igtbl:UltraGridColumn HeaderText="Item name" Key="ItemName" Width="20%" BaseColumnName="ItemName">
									<CellStyle Wrap="True"></CellStyle>
									<Footer Key="ItemName"></Footer>
									<Header Key="ItemName" Caption="Item name"></Header>
								</igtbl:UltraGridColumn>
								<igtbl:UltraGridColumn HeaderText="Description" Key="Body" Width="60%" BaseColumnName="Body">
									<CellStyle Wrap="True"></CellStyle>
									<Footer Key="Body"></Footer>
									<Header Key="Body" Caption="Description"></Header>
								</igtbl:UltraGridColumn>
							</Columns>
						</igtbl:UltraGridBand>
					</Bands>
				</igtbl:ultrawebgrid></td>
		</tr>
	</table>
</MM:mastermodule>
