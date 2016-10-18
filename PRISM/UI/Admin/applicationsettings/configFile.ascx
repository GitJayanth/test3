<%@ Control Language="C#" AutoEventWireup="true" Inherits="HyperCatalog.UI.Admin.Architecture.ConfigFileControl" CodeFile="configFile.ascx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
				<fieldset><legend>Application settings</legend>
	        <table cellspacing="0" cellpadding="0" width="100%" border="0">
		        <tr valign="middle">
			        <td class="editLabelCell" style="width:130px">
				        Path
			        </td>
			        <td class="ugd">
				        <%# configuration!=null?configuration.FilePath:"-" %>
			        </td>
		        </tr>
	          <asp:Repeater ID="rptKeys" runat="server" DataSource='<%# configuration.AppSettings.Settings.AllKeys%>'>
	            <ItemTemplate>
		            <tr valign="middle">
			            <td class="editLabelCell" style="width:130px">
				            <asp:Label ID="lbKeyName" runat="server" Text='<%# Container.DataItem %>'></asp:Label>
				            <asp:TextBox ID="txtKeyName" runat="server" Text='<%# Container.DataItem %>' Visible="false"></asp:TextBox>
			            </td>
			            <td class="ugd">
				            <asp:TextBox id="txtKeyValue" runat="server" Text='<%# System.Configuration.ConfigurationManager.AppSettings[(string)Container.DataItem] %>' width="400px"></asp:textbox>
			            </td>
		            </tr>
		          </ItemTemplate>
		        </asp:Repeater>
			    </table>
				</fieldset>
				<fieldset><legend>Locations</legend>
				  <asp:Repeater ID="rptLocations" runat="server" DataSource='<%# configuration!=null?configuration.Locations:null %>' OnItemCreated="rptLocations_ItemCreated">
				    <ItemTemplate>
		          <fieldset><legend onclick="showHideFieldset(this.parentElement);" style="cursor:hand;"><%# ((System.Configuration.ConfigurationLocation)Container.DataItem).Path %>...</legend>
        			  <asp:Panel id="subLocation" runat="server" style="display:none;">not implemented</asp:Panel>
  			      </fieldset>
			      </ItemTemplate>
			    </asp:Repeater>
				</fieldset>
				<fieldset><legend>System properties</legend>
				  <table cellspacing="0" cellpadding="0" width="100%" border="0">
				  <asp:Repeater ID="rptSystemWeb" runat="server" DataSource='<%# _systemWebSectionGroup.Sections%>'>
				    <ItemTemplate>
				      <asp:Panel ID="pnlSection" runat="server" Visible='<%# Container.DataItem.GetType().GetProperties().Length > 0%>'>
					      <tr valign="middle">
						      <td class="editLabelCell" style="width:130px">
							      <%# Container.DataItem.GetType().Name%>
						      </td>
						      <td class="ugd">
							      <asp:Repeater ID="rptSection" runat="server" DataSource='<%# Container.DataItem.GetType().GetProperties()%>'>
							        <ItemTemplate>
							          <asp:Panel ID="pnlProperties" runat="server" Visible='<%# !((System.Reflection.PropertyInfo)Container.DataItem).Name.StartsWith("Lock") && !((System.Reflection.PropertyInfo)Container.DataItem).Name.EndsWith("Information")%>'>
  							          <asp:Label ID="lblPropertyName" runat="server" 
  							            Text='<%# ((System.Reflection.PropertyInfo)Container.DataItem).Name %>'
  							            Visible='<%# !((System.Reflection.PropertyInfo)Container.DataItem).PropertyType.Equals(typeof(Boolean)) && !typeof(System.Collections.ICollection).IsInstanceOfType(((System.Reflection.PropertyInfo)Container.DataItem).GetValue(((System.Web.UI.WebControls.RepeaterItem)Container.NamingContainer.NamingContainer).DataItem, null))%>'></asp:Label>
  							          
							            <asp:TextBox ID="txtPropertyString" runat="server" Width="300px"
							              Text='<%# ((System.Reflection.PropertyInfo)Container.DataItem).GetValue(((System.Web.UI.WebControls.RepeaterItem)Container.NamingContainer.NamingContainer).DataItem,null) %>' 
							              Visible='<%# ((System.Reflection.PropertyInfo)Container.DataItem).PropertyType.Equals(typeof(System.String))%>'></asp:TextBox>
							            <asp:TextBox ID="txtPropertyTimeSpan" runat="server" Width="300px"
							              Text='<%# ((System.Reflection.PropertyInfo)Container.DataItem).GetValue(((System.Web.UI.WebControls.RepeaterItem)Container.NamingContainer.NamingContainer).DataItem,null) %>' 
							              Visible='<%# ((System.Reflection.PropertyInfo)Container.DataItem).PropertyType.Equals(typeof(System.TimeSpan))%>'></asp:TextBox>
							            <asp:TextBox ID="txtPropertyType" runat="server" Width="300px"
							              Text='<%# ((System.Reflection.PropertyInfo)Container.DataItem).GetValue(((System.Web.UI.WebControls.RepeaterItem)Container.NamingContainer.NamingContainer).DataItem,null) %>' 
							              Visible='<%# ((System.Reflection.PropertyInfo)Container.DataItem).PropertyType.Equals(typeof(System.Type))%>'></asp:TextBox>
							            <asp:CheckBox ID="chkPropertyBool" runat="server" OnDataBinding="chkProperty_CheckedChanged" 
							              Text='<%# DataBinder.Eval(Container.DataItem,"Name")%>' 
							              Visible='<%# ((System.Reflection.PropertyInfo)Container.DataItem).PropertyType.Equals(typeof(Boolean))%>'/>
							            <asp:DropDownList ID="ddlPropertyEncoding" runat="server" 
							              DataSource='<%# Encoding.GetEncodings() %>' DataTextField="DisplayName" DataValueField="CodePage" 
							              Visible='<%# ((System.Reflection.PropertyInfo)Container.DataItem).PropertyType.Equals(typeof(System.Text.Encoding))%>'></asp:DropDownList>
							            <igtxt:WebNumericEdit id="txtPropertyInt" runat="server" Width="60px"
							              ValueText='<%# ((System.Reflection.PropertyInfo)Container.DataItem).GetValue(((System.Web.UI.WebControls.RepeaterItem)Container.NamingContainer.NamingContainer).DataItem,null) %>'
							              Visible='<%# ((System.Reflection.PropertyInfo)Container.DataItem).PropertyType.Equals(typeof(System.Int32))%>'></igtxt:WebNumericEdit>
							            <asp:DropDownList ID="ddlPropertyHttpCookieMode" runat="server"
							              OnDataBound="ddl_DataBound"
							              DataSource='<%# Enum.GetNames(typeof(System.Web.HttpCookieMode)) %>'
							              Visible='<%# ((System.Reflection.PropertyInfo)Container.DataItem).PropertyType.Equals(typeof(System.Web.HttpCookieMode))%>'></asp:DropDownList>
							            <asp:DropDownList ID="ddlPropertyXhtmlConformanceMode" runat="server"
							              OnDataBound="ddl_DataBound"
							              DataSource='<%# Enum.GetNames(typeof(System.Web.Configuration.XhtmlConformanceMode)) %>'
							              Visible='<%# ((System.Reflection.PropertyInfo)Container.DataItem).PropertyType.Equals(typeof(System.Web.Configuration.XhtmlConformanceMode))%>'></asp:DropDownList>
							            <asp:DropDownList ID="ddlPropertyCustomErrorsMode" runat="server"
							              OnDataBound="ddl_DataBound"
							              DataSource='<%# Enum.GetNames(typeof(System.Web.Configuration.CustomErrorsMode)) %>'
							              Visible='<%# ((System.Reflection.PropertyInfo)Container.DataItem).PropertyType.Equals(typeof(System.Web.Configuration.CustomErrorsMode))%>'></asp:DropDownList>
							            <asp:DropDownList ID="ddlPropertyViewStateEncryptionMode" runat="server"
							              OnDataBound="ddl_DataBound"
							              DataSource='<%# Enum.GetNames(typeof(System.Web.UI.ViewStateEncryptionMode)) %>'
							              Visible='<%# ((System.Reflection.PropertyInfo)Container.DataItem).PropertyType.Equals(typeof(System.Web.UI.ViewStateEncryptionMode))%>'></asp:DropDownList>
							            <asp:DropDownList ID="ddlPropertyCompilationMode" runat="server"
							              OnDataBound="ddl_DataBound"
							              DataSource='<%# Enum.GetNames(typeof(System.Web.UI.CompilationMode)) %>'
							              Visible='<%# ((System.Reflection.PropertyInfo)Container.DataItem).PropertyType.Equals(typeof(System.Web.UI.CompilationMode))%>'></asp:DropDownList>
							            <asp:DropDownList ID="ddlPropertyTraceDisplayMode" runat="server"
							              OnDataBound="ddl_DataBound"
							              DataSource='<%# Enum.GetNames(typeof(System.Web.Configuration.TraceDisplayMode)) %>'
							              Visible='<%# ((System.Reflection.PropertyInfo)Container.DataItem).PropertyType.Equals(typeof(System.Web.Configuration.TraceDisplayMode))%>'></asp:DropDownList>
							            <asp:DropDownList ID="ddlPropertyAuthenticationMode" runat="server"
							              OnDataBound="ddl_DataBound"
							              DataSource='<%# Enum.GetNames(typeof(System.Web.Configuration.AuthenticationMode)) %>'
							              Visible='<%# ((System.Reflection.PropertyInfo)Container.DataItem).PropertyType.Equals(typeof(System.Web.Configuration.AuthenticationMode))%>'></asp:DropDownList>
							            <asp:DropDownList ID="ddlPropertyProcessModelLogLevel" runat="server"
							              OnDataBound="ddl_DataBound"
							              DataSource='<%# Enum.GetNames(typeof(System.Web.Configuration.ProcessModelLogLevel)) %>'
							              Visible='<%# ((System.Reflection.PropertyInfo)Container.DataItem).PropertyType.Equals(typeof(System.Web.Configuration.ProcessModelLogLevel))%>'></asp:DropDownList>
							            <asp:DropDownList ID="ddlPropertyProcessModelComAuthenticationLevel" runat="server"
							              OnDataBound="ddl_DataBound"
							              DataSource='<%# Enum.GetNames(typeof(System.Web.Configuration.ProcessModelComAuthenticationLevel)) %>'
							              Visible='<%# ((System.Reflection.PropertyInfo)Container.DataItem).PropertyType.Equals(typeof(System.Web.Configuration.ProcessModelComAuthenticationLevel))%>'></asp:DropDownList>
							            <asp:DropDownList ID="ddlPropertyProcessModelComImpersonationLevel" runat="server"
							              OnDataBound="ddl_DataBound"
							              DataSource='<%# Enum.GetNames(typeof(System.Web.Configuration.ProcessModelComImpersonationLevel)) %>'
							              Visible='<%# ((System.Reflection.PropertyInfo)Container.DataItem).PropertyType.Equals(typeof(System.Web.Configuration.ProcessModelComImpersonationLevel))%>'></asp:DropDownList>
							            <asp:DropDownList ID="ddlPropertySessionStateMode" runat="server"
							              OnDataBound="ddl_DataBound"
							              DataSource='<%# Enum.GetNames(typeof(System.Web.SessionState.SessionStateMode)) %>'
							              Visible='<%# ((System.Reflection.PropertyInfo)Container.DataItem).PropertyType.Equals(typeof(System.Web.SessionState.SessionStateMode))%>'></asp:DropDownList>
							              
							            <asp:Repeater ID="rptProperty" runat="server" 
							              DataSource='<%# typeof(System.Collections.ICollection).IsInstanceOfType(((System.Reflection.PropertyInfo)Container.DataItem).GetValue(((System.Web.UI.WebControls.RepeaterItem)Container.NamingContainer.NamingContainer).DataItem, null))?((System.Reflection.PropertyInfo)Container.DataItem).GetValue(((System.Web.UI.WebControls.RepeaterItem)Container.NamingContainer.NamingContainer).DataItem,null):null %>'
							              Visible='<%# typeof(System.Collections.ICollection).IsInstanceOfType(((System.Reflection.PropertyInfo)Container.DataItem).GetValue(((System.Web.UI.WebControls.RepeaterItem)Container.NamingContainer.NamingContainer).DataItem, null))%>'>
							              <HeaderTemplate><B><%# DataBinder.Eval(Container.NamingContainer.NamingContainer,"DataItem.Name")%></B><br /></HeaderTemplate>
							              <ItemTemplate>
							                <asp:Repeater ID="rptSubProperty" runat="server" DataSource='<%# Container.DataItem.GetType().GetProperties()%>'>
							                  <ItemTemplate>
    							                <%# ((System.Reflection.PropertyInfo)Container.DataItem).Name %>
							                  </ItemTemplate>
							                  <SeparatorTemplate><br /></SeparatorTemplate>
							                  <FooterTemplate><br /></FooterTemplate>
							                </asp:Repeater>
							              </ItemTemplate>
							              <SeparatorTemplate><br /></SeparatorTemplate>
							              <FooterTemplate><br /></FooterTemplate>
							            </asp:Repeater>
                        </asp:Panel>
							        </ItemTemplate>
							      </asp:Repeater>
						      </td>
					      </tr>
					    </asp:Panel>
					  </ItemTemplate>
				  </asp:Repeater>
					</table>
				</fieldset>
				<fieldset><legend>System properties</legend>
				  <table cellspacing="0" cellpadding="0" width="100%" border="0">
					  <tr valign="middle" id="trSection" runat="server" visible='<%# traceSection!=null%>'>
						  <td class="editLabelCell" style="width:130px">
							  Trace
						  </td>
						  <td class="ugd">
        				<asp:CheckBox ID="chkTraceEnabled" runat="server" Text="enabled" Checked='<%# traceSection!=null?traceSection.Enabled:false %>' /><br />
        				<asp:CheckBox ID="chkTraceLocalOnly" runat="server" Text="local only" Checked='<%# traceSection!=null?traceSection.LocalOnly:false %>' /><br />
        				<asp:CheckBox ID="chkTracePageOutput" runat="server" Text="page output" Checked='<%# traceSection!=null?traceSection.PageOutput:false %>' /><br />
        				<asp:CheckBox ID="chkTraceMostRecent" runat="server" Text="most recent" Checked='<%# traceSection!=null?traceSection.MostRecent:false %>' /><br />
        				<asp:CheckBox ID="chkTraceDiagnostics" runat="server" Text="diagnostics trace" Checked='<%# traceSection!=null?traceSection.WriteToDiagnosticsTrace:false %>' /><br />
        				Request limit: <asp:TextBox ID="txtTraceRequestLimit" runat="server" Text='<%# traceSection!=null?traceSection.RequestLimit.ToString():"" %>' /><br />
        				Mode: <asp:DropDownList ID="ddlTraceMode" runat="server" DataSource='<%# Enum.GetNames(typeof(System.Web.Configuration.TraceDisplayMode)) %>'></asp:DropDownList><br />
        				
        				<%# displayXml(traceSection.SectionInformation.GetRawXml())%>
						  </td>
					  </tr>
					  <tr valign="middle">
						  <td class="editLabelCell" style="width:130px">
							  Session
						  </td>
						  <td class="ugd">
        				Mode: <asp:DropDownList ID="ddlSessionMode" runat="server" DataSource='<%# Enum.GetNames(typeof(System.Web.SessionState.SessionStateMode)) %>'></asp:DropDownList><br />
        				Cookie: <asp:DropDownList ID="ddlSessionCookieLess" runat="server" DataSource='<%# Enum.GetNames(typeof(System.Web.HttpCookieMode)) %>'></asp:DropDownList><br />
        				Cookie name: <asp:TextBox ID="txtSessionCookieName" runat="server" Text='<%# sessionSection!=null?sessionSection.CookieName.ToString():"" %>'></asp:TextBox><br />
        				Custom provider: <asp:TextBox ID="txtSessionCustomProvider" runat="server" Text='<%# sessionSection!=null?sessionSection.CustomProvider.ToString():"" %>'></asp:TextBox><br />
        				Providers:
        				<asp:Repeater ID="rptSessionProviders" runat="server" DataSource='<%# sessionSection!=null?sessionSection.Providers:null %>'>
        				  <HeaderTemplate></HeaderTemplate>
        				  <ItemTemplate><%#((System.Configuration.ProviderSettings)Container.DataItem).Name%>type,parameters</ItemTemplate>
        				</asp:Repeater>
        				Partition resolver: <asp:TextBox ID="txtSessionPartitionResolverType" runat="server" Text='<%# sessionSection!=null?sessionSection.PartitionResolverType.ToString():"" %>'></asp:TextBox><br />
        				Time out: <asp:TextBox ID="txtSessionTimeout" runat="server" Text='<%# sessionSection!=null?sessionSection.Timeout.ToString():"" %>' Width="80px"></asp:TextBox><br />
        				<asp:CheckBox ID="chkSessionRegenerateExpiredSessionId" runat="server" Text="regenerate expired sessionId" Checked='<%# sessionSection!=null?sessionSection.RegenerateExpiredSessionId:false %>' /><br />
        				<asp:CheckBox ID="chkSessionAllowCustomSqlDB" runat="server" Text="custom database" Checked='<%# sessionSection!=null?sessionSection.AllowCustomSqlDatabase:false %>' /><br />
        				Connection: <asp:TextBox ID="txtSessionSqlConnectionString" runat="server" Text='<%# sessionSection!=null?sessionSection.SqlConnectionString:"" %>' Width="400px"></asp:TextBox><br />
        				SQL timeout: <asp:TextBox ID="txtSessionSqlCommandTimeout" runat="server" Text='<%# sessionSection!=null?sessionSection.SqlCommandTimeout.ToString():"" %>' /><br />
        				State connection: <asp:TextBox ID="txtSessionStateConnectionString" runat="server" Text='<%# sessionSection!=null?sessionSection.StateConnectionString:"" %>' Width="400px"></asp:TextBox><br />
        				State timeout: <asp:TextBox ID="txtSessionStateNetworkTimeout" runat="server" Text='<%# sessionSection!=null?sessionSection.StateNetworkTimeout.ToString():"" %>' /><br />
        				<asp:CheckBox ID="chkSessionUseHostingIdentity" runat="server" Text="use hosting identity" Checked='<%# sessionSection!=null?sessionSection.UseHostingIdentity:false %>' /><br />

        				<%# displayXml(sessionSection.SectionInformation.GetRawXml())%>
						  </td>
					  </tr>

					  <tr valign="middle">
						  <td class="editLabelCell" style="width:130px">
							  Custom errors
						  </td>
						  <td class="ugd">
        				Mode: <asp:DropDownList ID="ddlCustomErrorsMode" runat="server" DataSource='<%# Enum.GetNames(typeof(System.Web.Configuration.CustomErrorsMode)) %>'></asp:DropDownList><br />
        				Default redirection: <asp:TextBox ID="txtCustomErrorsDefaultRedirect" runat="server" Text='<%# customErrorsSection!=null?(string)customErrorsSection.DefaultRedirect:"" %>'></asp:TextBox><br />
        				Errors:<br />
        				<asp:Repeater ID="rptCustomErrorsErrors" runat="server" DataSource='<%# customErrorsSection!=null?customErrorsSection.Errors:null %>'>
        				  <ItemTemplate>
        				    &nbsp;&nbsp;<%# ((System.Web.Configuration.CustomError)Container.DataItem).StatusCode %> = <%# ((System.Web.Configuration.CustomError)Container.DataItem).Redirect %>
        				  </ItemTemplate>
        				  <SeparatorTemplate><br /></SeparatorTemplate>
        				  <FooterTemplate>
        				    <BR />
        				    &nbsp;&nbsp;<asp:DropDownList ID="ddlCustomErrorsNewCode" runat="server"></asp:DropDownList>
        				    <asp:TextBox ID="txtCustomErrorsNewRedirect" runat="server" Text=".aspx"></asp:TextBox>
        				  </FooterTemplate>
        				</asp:Repeater>

        				<%# displayXml(customErrorsSection.SectionInformation.GetRawXml())%>
						  </td>
					  </tr>
					  <tr valign="middle">
						  <td class="editLabelCell" style="width:130px">
							  Authentication
						  </td>
						  <td class="ugd">
        				Login page: <asp:TextBox ID="txtAuthenticationFormsLoginUrl" runat="server" Text='<%# authenticationSection.Forms.LoginUrl %>'></asp:TextBox><br />
        				Default page: <asp:TextBox ID="txtAuthenticationFormsDefaultUrl" runat="server" Text='<%# authenticationSection.Forms.DefaultUrl %>'></asp:TextBox><br />
        				Mode: <asp:DropDownList ID="ddlAuthenticationMode" runat="server" DataSource='<%# Enum.GetNames(typeof(System.Web.Configuration.AuthenticationMode)) %>'></asp:DropDownList><br />
        				Passport: <asp:TextBox ID="txtAuthenticationPassportRedirectUrl" runat="server" Text='<%# authenticationSection.Passport.RedirectUrl %>'></asp:TextBox><br />
        				<asp:CheckBox ID="chkAuthenticationFormsSlidingExp" runat="server" Text="sliding expiration" Checked='<%# authenticationSection!=null?authenticationSection.Forms.SlidingExpiration:false %>' /><br />
        				Domain: <asp:TextBox ID="txtAuthenticationFormsDomain" runat="server" Text='<%# authenticationSection.Forms.Domain %>'></asp:TextBox><br />
        				<asp:CheckBox ID="txtAuthenticationFormsEnableCrossAppRedirects" runat="server" Text="cross application redirects" Checked='<%# authenticationSection!=null?authenticationSection.Forms.EnableCrossAppRedirects:false %>' /><br />
        				<asp:CheckBox ID="txtAuthenticationFormsRequireSSL" runat="server" Text="require SSL" Checked='<%# authenticationSection!=null?authenticationSection.Forms.RequireSSL:false %>' /><br />
        				Path: <asp:TextBox ID="txtAuthenticationFormsPath" runat="server" Text='<%# authenticationSection.Forms.Path %>'></asp:TextBox><br />
        				Protection: <asp:DropDownList ID="ddlAuthenticationFormsProtection" runat="server" DataSource='<%# Enum.GetNames(typeof(System.Web.Configuration.FormsProtectionEnum)) %>'></asp:DropDownList><br />
        				Cookie: <asp:DropDownList ID="ddlAuthenticationFormsCookieLess" runat="server" DataSource='<%# Enum.GetNames(typeof(System.Web.HttpCookieMode)) %>'></asp:DropDownList><br />
        				Cookie name: <asp:TextBox ID="txtAuthenticationFormsName" runat="server" Text='<%# authenticationSection.Forms.Name %>'></asp:TextBox><br />
        				Timeout: <asp:TextBox ID="txtAuthenticationFormsTimeout" runat="server" Text='<%# authenticationSection.Forms.Timeout %>'></asp:TextBox><br />
        				Password format: <asp:DropDownList ID="ddlAuthenticationFormsCredentialsPasswordFormat" runat="server" DataSource='<%# Enum.GetNames(typeof(System.Web.Configuration.FormsAuthPasswordFormat)) %>'></asp:DropDownList><br />
        				Users:
        				<asp:Repeater ID="rptAuthenticationFomsCredentialsUsers" runat="server" DataSource='<%# authenticationSection!=null?authenticationSection.Forms.Credentials.Users:null %>'>
        				  <ItemTemplate>
        				    &nbsp;&nbsp;<asp:TextBox ID="txtAuthenticationFomsCredentialsUsersName" runat="server" Text='<%# ((System.Web.Configuration.FormsAuthenticationUser)Container.DataItem).Name%>'></asp:TextBox>
        				    &nbsp;&nbsp;<asp:TextBox ID="txtAuthenticationFomsCredentialsUsersPassword" runat="server" Text='<%# ((System.Web.Configuration.FormsAuthenticationUser)Container.DataItem).Password%>'></asp:TextBox>
        				  </ItemTemplate>
        				  <SeparatorTemplate><br /></SeparatorTemplate>
        				  <FooterTemplate>
        				    <br />
        				    &nbsp;&nbsp;<asp:TextBox ID="txtAuthenticationFomsCredentialsUsersNewName" runat="server"></asp:TextBox>
        				    &nbsp;&nbsp;<asp:TextBox ID="txtAuthenticationFomsCredentialsUsersNewPassword" runat="server"></asp:TextBox>
        				  </FooterTemplate>
        				</asp:Repeater>

        				<%# displayXml(authenticationSection.SectionInformation.GetRawXml())%>
						  </td>
					  </tr>
					  <tr valign="middle">
						  <td class="editLabelCell" style="width:130px">
							  Authorization
						  </td>
						  <td class="ugd">
        				<asp:Repeater ID="rptAuthorizationRules" runat="server" DataSource='<%# authorizationSection.Rules %>'>
        				  <ItemTemplate>
        				    action <asp:TextBox ID="txtAuthorizationRulesAction" runat="server" Text='<%# ((System.Web.Configuration.AuthorizationRule)Container.DataItem).Action %>'></asp:TextBox><br />
        				    &nbsp;&nbsp;users <asp:TextBox ID="txtAuthorizationRulesUsers" runat="server" Text='<%# ((System.Web.Configuration.AuthorizationRule)Container.DataItem).Users %>'></asp:TextBox><br />
        				    &nbsp;&nbsp;roles <asp:TextBox ID="txtAuthorizationRulesRoles" runat="server" Text='<%# ((System.Web.Configuration.AuthorizationRule)Container.DataItem).Roles %>'></asp:TextBox><br />
        				    &nbsp;&nbsp;verbs <asp:TextBox ID="txtAuthorizationRulesVerbs" runat="server" Text='<%# ((System.Web.Configuration.AuthorizationRule)Container.DataItem).Verbs %>'></asp:TextBox><br />
        				  </ItemTemplate>
        				  <SeparatorTemplate><BR /></SeparatorTemplate>
        				</asp:Repeater>

        				<%# displayXml(authorizationSection.SectionInformation.GetRawXml())%>
						  </td>
					  </tr>
					  <tr valign="middle">
						  <td class="editLabelCell" style="width:130px">
							  Compilation
						  </td>
						  <td class="ugd">
        				<asp:Repeater ID="rptCompilation" runat="server" DataSource='<%# compilationSection.Assemblies %>'>
        				  <ItemTemplate>
        				    <asp:TextBox ID="txtCompilationAssembliesAssembly" runat="server" Text='<%# ((System.Web.Configuration.AssemblyInfo)Container.DataItem).Assembly %>' Width="800px"></asp:TextBox>
        				  </ItemTemplate>
        				  <SeparatorTemplate><BR /></SeparatorTemplate>
        				  <FooterTemplate>
        				    <asp:TextBox ID="txtCompilationAssembliesNewAssembly" runat="server" Width="800px"></asp:TextBox>
        				  </FooterTemplate>
        				</asp:Repeater>

        				<%# displayXml(compilationSection.SectionInformation.GetRawXml())%>
						  </td>
					  </tr>
					  <tr valign="middle">
						  <td class="editLabelCell" style="width:130px">
							  XHTML Conformance
						  </td>
						  <td class="ugd">
        				Mode: <asp:DropDownList ID="xhtmlConformanceSectionMode" runat="server" DataSource='<%# Enum.GetNames(typeof(System.Web.Configuration.XhtmlConformanceMode)) %>'></asp:DropDownList><br />

        				<%# displayXml(xhtmlConformanceSection.SectionInformation.GetRawXml())%>
						  </td>
					  </tr>
					  <tr valign="middle">
						  <td class="editLabelCell" style="width:130px">
							  Http runtime
						  </td>
						  <td class="ugd">
						    <asp:CheckBox ID="chkHttpRuntimeEnable" runat="server" Text="enabled" Checked='<%# httpRunTimeSection!=null?httpRunTimeSection.Enable:false %>' /><br />
						    max request length: <asp:TextBox ID="txtHttpRuntimeMaxrequestlength" runat="server" Text='<%# httpRunTimeSection!=null?httpRunTimeSection.MaxRequestLength.ToString():"" %>' Width="80px"></asp:TextBox> bytes<br />
						    time out: <asp:TextBox ID="txtHttpRuntimeTimeout" runat="server" Text='<%# httpRunTimeSection!=null?httpRunTimeSection.ExecutionTimeout.ToString():"" %>' Width="80px"></asp:TextBox><br />

        				<%# displayXml(httpRunTimeSection.SectionInformation.GetRawXml())%>
						  </td>
					  </tr>
					  <tr valign="middle">
						  <td class="editLabelCell" style="width:130px">
							  Globalization
						  </td>
						  <td class="ugd">
						    File encoding: <asp:DropDownList ID="ddlGlobalizationFileEncoding" runat="server" DataSource='<%# Encoding.GetEncodings() %>' DataTextField="DisplayName" DataValueField="CodePage"></asp:DropDownList><br />
						    Request encoding: <asp:DropDownList ID="ddlGlobalizationRequestEncoding" runat="server" DataSource='<%# Encoding.GetEncodings() %>' DataTextField="DisplayName" DataValueField="CodePage"></asp:DropDownList><br />
						    Response encoding: <asp:DropDownList ID="ddlGlobalizationResponseEncoding" runat="server" DataSource='<%# Encoding.GetEncodings() %>' DataTextField="DisplayName" DataValueField="CodePage"></asp:DropDownList><br />

        				<%# displayXml(globalizationSection.SectionInformation.GetRawXml())%>
						  </td>
					  </tr>
					  <tr valign="middle">
						  <td class="editLabelCell" style="width:130px">
							  Pages
						  </td>
						  <td class="ugd">
						    time out: <asp:TextBox ID="txtPagesAsyncTimeout" runat="server" Text='<%# pagesSection!=null?pagesSection.AsyncTimeout.ToString():"" %>' Width="80px"></asp:TextBox><br />
						    <asp:CheckBox ID="chkPagesAutoEventWireup" runat="server" Text="auto event wireup" Checked='<%# pagesSection!=null?pagesSection.AutoEventWireup:false %>' /><br />
						    <asp:CheckBox ID="chkPagesBuffer" runat="server" Text="buffer" Checked='<%# pagesSection!=null?pagesSection.Buffer:false %>' /><br />
						    Compilation mode: <asp:DropDownList ID="ddlPagesCompilationMode" runat="server" DataSource='<%# Enum.GetNames(typeof(System.Web.UI.CompilationMode)) %>'></asp:DropDownList><br />
						    Controls:
						    <asp:Repeater ID="rptPagesControls" runat="server" DataSource='<%# pagesSection.Controls %>'>
        				  <ItemTemplate>
        				    <%# ((System.Web.Configuration.TagPrefixInfo)Container.DataItem).ToString() %>
        				  </ItemTemplate>
        				  <SeparatorTemplate><BR /></SeparatorTemplate>
        				</asp:Repeater>
						    EnableEventValidation bool
						    EnableSessionState bool
						    EnableViewState bool
						    EnableViewStateMac bool
						    MaintainScrollPositionOnPostBack bool
						    MasterPageFile string
						    MaxPageStateFieldLength int
						    Namespaces System.Web.Configuration.NamespaceCollection
						    PageBaseType string
						    PageParserFilterType string
						    SmartNavigation bool
						    StyleSheetTheme string
						    TagMapping System.Web.Configuration.TagMapCollection
						    Theme string
						    UserControlBaseType string
						    ValidateRequest bool
						    ViewStateEncryptionMode System.Web.UI.ViewStateEncryptionMode

        				<%# displayXml(pagesSection.SectionInformation.GetRawXml())%>
						  </td>
					  </tr>
					</table>
				</fieldset>
