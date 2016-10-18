<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Help.About" CodeFile="about.aspx.cs" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">About</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
  <script type="text/C#" language="C#" runat="server">
    private string displayDBURI(HyperCatalog.Business.ApplicationComponent component)
    {
      string s = component.HardwareComponent != null ? GetHostName(component.HardwareComponent.URI) : GetHostName(component.URI);
      return WarnNotSet(s);
    }
    private string GetHostName(string IP)
    {
      try
      {
        return System.Net.Dns.GetHostEntry(IP).HostName != IP ? System.Net.Dns.GetHostEntry(IP).HostName + " [" + IP + "]" : IP;
      }
      catch
      {
        return IP;
      }
    }
    private string WarnNotSet(string message)
    {
      return (message == null || message == String.Empty) ? "<font style=\"color:red;\">Not set</font>" : message;
    }
  </script>
  <script type="text/javascript" language="javascript">
    document.body.scroll = "no";
  </script>
      <table style="WIDTH: 100%; HEIGHT: 100%;" cellspacing="0" cellpadding="0" border="0">
        <tr valign="top" style="height: 1px" align="left">
          <td>
            <table cellspacing="0" cellpadding="0" width="100%" border="0">
              <tr align="left" bgcolor="#003366">
                <td style="width: 31px; height: 30px"><img alt="" src='<%= "/hc_v4/img/" + (string)HyperCatalog.Shared.SessionState.CacheParams["CustomerLogo"].Value%>' border="0" /></td>
              </tr>
            </table>
          </td>
        </tr>
        <tr valign="top" style="height: auto">
          <td>
            <div style="overflow:scroll;width:100%;height:100%">
							<FIELDSET><LEGEND>General</LEGEND>
								<table cellspacing=0 cellpadding=0 width="100%" border=0>
									<tr valign=middle>
										<td class=editLabelCell width=130>
											Application
										</td>
										<td class=ugd>
											<%= (string)HyperCatalog.Shared.SessionState.CacheParams["AppName"].Value%>
										</td>
									</tr>
									<tr valign=middle>
										<td class=editLabelCell width=130>
											Instance
										</td>
										<td class=ugd>
											<%= Page.Request.ApplicationPath.Replace("/", "")%>
										</td>
									</tr>
									<tr valign=middle>
										<td class=editLabelCell width=130>
											Build number
										</td>
										<td class=ugd>
											<%= HyperCatalog.Business.Settings.BuildNumber%>
											[<%= HyperCatalog.Business.Settings.BuildDate.ToShortDateString()%>]
										</td>
									</tr>
									<tr valign=middle>
										<td class=editLabelCell width=130>
											Machine
										</td>
										<td class=ugd>
											<%= Page.Server.MachineName%>
											[<%= Page.Request.ServerVariables["LOCAL_ADDR"]%>]
										</td>
									</tr>
							  </table>
							</FIELDSET>
							<asp:Panel ID="unvisible" runat="server" Visible='<%# ((string)HyperCatalog.Shared.SessionState.CacheParams["AppIsProd"].Value)=="1"%>'>
							<FIELDSET><LEGEND>Database</LEGEND>
								<table cellspacing=0 cellpadding=0 width="100%" border=0>
								  <asp:Repeater ID="DBRep" runat="server">
								    <ItemTemplate>
									    <tr valign=middle>
										    <td class=editLabelCell width=130>
											    <%# ((HyperCatalog.Business.ApplicationComponent)Container.DataItem).Name%>
										    </td>
										    <td class=ugd>
											    <%# ((HyperCatalog.Business.ApplicationComponent)Container.DataItem).VersionNumber + " [" + ((HyperCatalog.Business.ApplicationComponent)Container.DataItem).VersionDate.ToShortDateString() + "]"%>
											    <BR />
											    <%# displayDBURI((HyperCatalog.Business.ApplicationComponent)Container.DataItem)%>
										    </td>
									    </tr>
									  </ItemTemplate>
									</asp:Repeater>
							  </table>
							</FIELDSET>
							<FIELDSET><LEGEND>WebServices</LEGEND>
								<table cellspacing=0 cellpadding=0 width="100%" border=0>
								  <asp:Repeater ID="WSRep" runat="server">
								    <ItemTemplate>
									    <tr valign=middle>
										    <td class=editLabelCell width=130>
											    <%# ((HyperCatalog.Business.ApplicationComponent)Container.DataItem).Name%>
										    </td>
										    <td class=ugd>
											    <%# ((HyperCatalog.Business.ApplicationComponent)Container.DataItem).VersionNumber + " [" + ((HyperCatalog.Business.ApplicationComponent)Container.DataItem).VersionDate.ToShortDateString() + "]"%>
											    <BR />
											    <a target="_blank" href='<%# ((HyperCatalog.Business.ApplicationComponent)Container.DataItem).URI%>'><%# WarnNotSet(((HyperCatalog.Business.ApplicationComponent)Container.DataItem).URI)%></a>
										    </td>
									    </tr>
									  </ItemTemplate>
									</asp:Repeater>
							  </table>
							</FIELDSET>
							<FIELDSET><LEGEND>Assemblies</LEGEND>
								<table cellspacing=0 cellpadding=0 width="100%" border=0>
								  <asp:Repeater ID="DLLRep" runat="server">
								    <ItemTemplate>
							        <tr valign=middle>
								        <td colspan="2" style="font-weight:bold;" width=130><%# (string)((object[])Container.DataItem)[0] %></td>
								      </tr>
								      <asp:Repeater ID="apiDLLRep" datasource='<%# (ArrayList)((object[])Container.DataItem)[1]%>' runat="server">
								        <ItemTemplate>
									        <tr valign=middle>
										        <td class=editLabelCell width=130>
											        <%# System.IO.Path.GetFileNameWithoutExtension((string)Container.DataItem)%>
										        </td>
										        <td class=ugd>
											        <%# System.Reflection.Assembly.LoadFrom((string)Container.DataItem).FullName.Replace(",","</BR>")%>
										        </td>
									        </tr>
									      </ItemTemplate>
									    </asp:Repeater>
									  </ItemTemplate>
									</asp:Repeater>
							  </table>
							</FIELDSET>
							</asp:Panel>
            </div>  
					</td>
        </tr>
      </table>
</asp:Content>