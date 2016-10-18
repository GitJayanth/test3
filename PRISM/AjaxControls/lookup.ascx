<%@ Control Language="c#" EnableViewState="true" ClassName="LookUp" Description="Implementation of an INPUT field with a LookUp helper" Inherits="HyperCatalog.UI.AjaxControls.LookUp" CodeFile="LookUp.ascx.cs" %>
<%@ Implements Interface="System.Web.UI.IPostBackDataHandler" %>
<%
  // Lookup.ascx
  // Implementation of the LookUp AJAX Control
  // Copyright by Matthias Hertel, http://www.mathertel.de
  // This work is licensed under a Creative Commons Attribution 2.0 Germany License.
  // See http://creativecommons.org/licenses/by/2.0/de/
  // ---
  // 12.08.2005 created
%>
<%// UI for the control %>
<input id="<%=this.UniqueID %>" autocomplete="on" pageproperty="<%=this.pageproperty %>"
lookupservice="<%=this.lookupservice %>" name="<%=this.UniqueID%>"
  value="<%=this.Value %>" nosubmit="<%=this.nosubmit %>" onblur="jcl.DataConnections.Raise(this.pageproperty, this.value)"
  onchange="jcl.DataConnections.Raise(this.name, this.value)" style="width:<%=this.Width%>px" />
<script defer="defer" type="text/javascript">
jcl.LoadBehaviour("<%=this.UniqueID %>", LookUpBehaviour);
</script>
