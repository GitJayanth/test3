<%@ Register TagPrefix="MM" Namespace="HyperCatalog.MasterModules" Assembly="HyperCatalog.MasterModules" %>
<%@ Control Language="c#" Inherits="hc_homePageModules.uc_todaysactivity" CodeFile="uc_todaysactivity.ascx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<MM:mastermodule id="Module" runat="server" ModuleName="TodaysActivity" ModuleTitle="Today's activity (updated every 30 min.)">
<asp:Label ID="lbTodayActivity" runat="server"></asp:Label>
	<br /><br />
</MM:mastermodule>
