<%@ Reference Page="~/ui/acquire/chunk/chunk.aspx" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.Chunk.Chunk_Rejected" CodeFile="Chunk_Rejected.aspx.cs"%>
<%@ Register TagPrefix="hc" TagName="chunkModifier" Src="Chunk_Modifier.ascx"%>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HOPT" runat="server">Chunk Text</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HOJS" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HOCP" runat="server">
      <table style="WIDTH: 100%" cellspacing="0" cellpadding="0" border="0">
		<tr valign="bottom" style="HEIGHT:26px" class="hc_toolbar">
			<td colspan="2">
				<table border="0">
					<tr>
						<td>
						<igtxt:webimagebutton id="wBtAccept" runat="server" Text="Accept value" Height="16px" Width="100px" OnClick="wBtAccept_Click">
								<Alignments VerticalImage="Middle"></Alignments>
								<PressedAppearance>
									<Style CssClass="hc_toolbarselected"></Style>
								</PressedAppearance>
								<HoverAppearance>
									<Style CssClass="hc_toolbarhover"></Style>
								</HoverAppearance>
								<Appearance>
									<Style BorderWidth="0px" CssClass="hc_toolbardefault"></Style>
									<Image Url="/hc_v4/img/ed_savefinal.gif" AlternateText="Accept master chunk"></Image>
								</Appearance>
							</igtxt:webimagebutton>
						</td>
						</tr>
						</table></td>
						</tr>
						
        <tr valign="top" style="height:1px">
          <td>
            <fieldset>
              <legend><asp:label id="Label1" runat="server">Value is</asp:label>&nbsp;<asp:label id="lbStatus" runat="server">Rejected</asp:label>&nbsp;<asp:image id="Image1" runat="server" AlternateText="status" ImageAlign="Middle" ImageUrl="/hc_v4/img/sr.gif"></asp:image>&nbsp;</legend>
              <table cellspacing="0" cellpadding="0" width="100%" border="0">
                <tr valign="top">
                  <td>
                  <asp:Label ID="lbChunkValue" runat="server"></asp:Label></td>
                </tr>
              </table>
            </fieldset>
          </td>
        </tr>
        <tr valign="top" style="height:1px">
          <td><hc:chunkmodifier id="ChunkModifier1" runat="server"></hc:chunkmodifier>
<center><asp:label id="lbResult" runat="server" Font-Size="7pt">Result message</asp:label></center>
          </td>
        </tr>
        <tr valign="bottom" height="*">
          <td colspan="2"></td>
        </tr>
      </table>
</asp:Content>