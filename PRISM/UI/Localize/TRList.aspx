<%@ Page MasterPageFile="~/HyperCatalog.Empty.master" Language="C#" AutoEventWireup="true" CodeFile="TRList.aspx.cs" Inherits="UI_Localize_TRList" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="ignav" Namespace="Infragistics.WebUI.UltraWebNavigator" Assembly="Infragistics2.WebUI.UltraWebNavigator.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="Main" ContentPlaceHolderID="hocp" runat="server">
    <table style="WIDTH: 100%; HEIGHT: 100%" cellspacing=0 cellpadding=0 border=0>
			<tr valign=top height=1>
			
		    <td>
				<igtbar:ultrawebtoolbar id=mainToolBar runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px" width="100%">
					<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
					<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
					<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
					<Items>
						<igtbar:TBarButton Key="refresh" Text="Refresh" Image="/hc_v4/img/ed_refresh.gif"></igtbar:TBarButton>
						<igtbar:TBLabel Text=" ">
							<DefaultStyle Width="100%"></DefaultStyle>
						</igtbar:TBLabel>
					</Items>
				</igtbar:ultrawebtoolbar>
			</td>
			
		</tr>
		<tr valign=top>
			<td>
        <ignav:UltraWebTree ID="TreeView" Cursor="Hand" Editable="True" runat="server" BackImageUrl="" CollapseImage="" DefaultImage="" DefaultIslandClass="" DefaultItemClass="" DefaultSelectedImage="" DisabledClass="" ExpandImage="" FileUrl="" HiliteClass="" HoverClass="" Indentation="20" JavaScriptFilename="" JavaScriptFileNameCommon="" LeafNodeImageUrl="" NodeEditClass="" ParentNodeImageUrl="" RootNodeImageUrl="" TargetFrame="" TargetUrl="" WebTreeTarget="HierarchicalTree">
            <Nodes>
                <ignav:Node CssClass="" HiliteClass="" HoverClass="" ImageUrl="" IslandClass="" SelectedImageUrl=""
                    TagString="" TargetFrame="" TargetUrl="" Text="Root Node" ToolTip="">
                    <Nodes>
                        <ignav:Node CssClass="" HiliteClass="" HoverClass="" ImageUrl="" IslandClass="" SelectedImageUrl=""
                            TagString="" TargetFrame="" TargetUrl="" Text="Country" ToolTip="">
                            <Nodes>
                                <ignav:Node CssClass="" HiliteClass="" HoverClass="" ImageUrl="" IslandClass="" SelectedImageUrl=""
                                    TagString="" TargetFrame="" TargetUrl="" Text="TR" ToolTip="">
                                </ignav:Node>
                            </Nodes>
                        </ignav:Node>
                    </Nodes>
                </ignav:Node>
            </Nodes>
            <ClientSideEvents AfterBeginNodeEdit="" AfterEndNodeEdit="" AfterNodeSelectionChange=""
                AfterNodeUpdate="" BeforeBeginNodeEdit="" BeforeEndNodeEdit="" BeforeNodeSelectionChange=""
                BeforeNodeUpdate="" DemandLoad="" Drag="" DragEnd="" DragEnter="" DragLeave=""
                DragOver="" DragStart="" Drop="" EditKeyDown="" EditKeyUp="" InitializeTree=""
                KeyDown="" KeyUp="" NodeChecked="" NodeClick="" NodeCollapse="" NodeExpand="" />
            <Levels>
                <ignav:Level CheckboxColumnName="" ColumnName="" ImageColumnName="" Index="0" LevelClass=""
                    LevelHiliteClass="" LevelHoverClass="" LevelImage="" LevelIslandClass="" LevelKeyField=""
                    RelationName="" TargetFrameName="" TargetUrlName="" />
                <ignav:Level CheckboxColumnName="" ColumnName="" ImageColumnName="" Index="1" LevelClass=""
                    LevelHiliteClass="" LevelHoverClass="" LevelImage="" LevelIslandClass="" LevelKeyField=""
                    RelationName="" TargetFrameName="" TargetUrlName="" />
                <ignav:Level CheckboxColumnName="" ColumnName="" ImageColumnName="" Index="2" LevelClass=""
                    LevelHiliteClass="" LevelHoverClass="" LevelImage="" LevelIslandClass="" LevelKeyField=""
                    RelationName="" TargetFrameName="" TargetUrlName="" />
            </Levels>
        </ignav:UltraWebTree>
        &nbsp;
        <asp:GridView ID="GridView1" runat="server">
        </asp:GridView>
        <asp:XmlDataSource ID="XmlDS" runat="server"></asp:XmlDataSource>
			</td>
		</tr>
  </table>
</asp:Content>