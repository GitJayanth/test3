<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="InputForms_ContainersEditValues"
  CodeFile="InputForms_ContainersEditValues.aspx.cs" %>

<%@ Register TagPrefix="ajax" TagName="Lookup" Src="~/ajaxcontrols/LookUp.ascx" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="ig_spell" Namespace="Infragistics.WebUI.WebSpellChecker" Assembly="Infragistics2.WebUI.WebSpellChecker.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
  Container edit possible values</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">

  <script type="text/javascript" src="../../../ajaxcore/ajax.js"></script>
  <script type="text/javascript" src="../../../ajaxcontrols/LookupTerm.js"></script>
  
  <script>



      function dgValues_ColumnHeaderClickHandler(gridName, columnId, button) {

          //Add code to handle your event here.
          var chValue = document.getElementById('chTrans');
          var myCol = igtbl_getColumnById(columnId);

          if (myCol.Key == "IsTranslateHeader") {
              if (chValue.value == 0)
                  chValue.value = 1;
              else
                  chValue.value = 0;
              var myGrid = igtbl_getGridById(gridName);

              for (i = 0; i < myGrid.Rows.length; i++) {
                  if (chValue.value == 1 && myGrid.Rows.getRow(i).getCellFromKey("IsTranslateHeader").isEditable()) {
                      //                var myObj = myGrid.Rows.getRow(i).getCellFromKey("IsTranslateHeader");
                      //                if(i==0)
                      //                {
                      //                    for (myKey in myObj)
                      //                    {
                      //                        alert ("myObj["+myKey +"] = "+myObj[myKey]);
                      //                    }
                      //                }

                      myGrid.Rows.getRow(i).getCellFromKey("IsTranslateHeader").setValue(1);

                  }
                  else if (chValue.value == 0 && myGrid.Rows.getRow(i).getCellFromKey("IsTranslateHeader").isEditable()) {

                      myGrid.Rows.getRow(i).getCellFromKey("IsTranslateHeader").setValue(0);
                  }
              }
              //alert(chValue.value);

          }

      }
      function uwToolbar_Click(oToolbar, oButton, oEvent) {
          var Checked = 0; ///#ACQ8.20
          if (oButton.Key == 'Delete') {
              oEvent.cancelPostBack = !confirm("Are you sure?");
          }
          if (oButton.Key == 'Close') {
              parent.close();
              oEvent.cancelPostBack = true;
          }
          ///#ACQ8.20 Start
          if (oButton.Key == 'Save') {
              myForm = document.forms[0];
              for (i = 0; i < myForm.length; i++) {
                  if ((myForm.elements[i].id.indexOf('IsTranslateRows') != -1)) {
                      if (!(myForm.elements[i].disabled) && myForm.elements[i].checked)
                          Checked = Checked + 1;
                  }
              }
          }
          if (Checked > 0)
          //oEvent.cancelPostBack = !confirm("Setting IsTranslate to Yes to a term value\n will be impacting all existing Inputform ?");                
              oEvent.cancelPostBack = !confirm("Setting IsTranslate to Yes to a term value\n will impact all existing Inputform(s).\nWould you like to continue ?"); //QC2723
          ///#ACQ8.20 Ends
      }
      var object = '';
      function SC() {
          object = document.getElementById('ctl00$HOCP$txtValue');
          specialCharacter(object);
          return false;
      }
      function SP() {
          object = document.getElementById('ctl00$HOCP$txtValue');
          octl00_HOCP_WebSpellChecker1.checkSpelling(object.value, myReturnFunc);
          return false;
      }
      function myReturnFunc(checkedText) {
          object.value = checkedText;
      }

      ///#ACQ8.20 Start
      //////////////////////////////////
      function IsTranslateHeader_Click(obj)
      //////////////////////////////////
      //grid: select/unselect all
      {
          var chkVal = obj.checked;
          var idVal = obj.id;
          // user is selecting/unselecting one checkbox

          if (chkVal) {
              myForm = document.forms[0];
              for (i = 0; i < myForm.length; i++) {
                  if ((myForm.elements[i].id.indexOf('IsTranslateRows') != -1)) {
                      if (!myForm.elements[i].disabled)
                          myForm.elements[i].checked = true;
                  }
              }
          }
          else {
              myForm = document.forms[0];
              for (i = 0; i < myForm.length; i++) {
                  if ((myForm.elements[i].id.indexOf('IsTranslateRows') != -1)) {
                      if (!myForm.elements[i].disabled)
                          myForm.elements[i].checked = false;
                  }
              }
          }
      }
      ///#ACQ8.20 End			
  </script>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" runat="Server">
  <table class="main" cellspacing="0" cellpadding="0">
    <tr valign="top" style="height: 1px">
      <td>
      <input id="chTrans" type="hidden" value = "0" />
        <igtbar:UltraWebToolbar ID="uwToolbar" runat="server" ImageDirectory=" " Width="100%"
          ItemWidthDefault="120px" CssClass="hc_toolbar" OnButtonClicked="uwToolbar_ButtonClicked">
          <HoverStyle CssClass="hc_toolbarhover">
          </HoverStyle>
          <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click">
          </ClientSideEvents>
          <SelectedStyle CssClass="hc_toolbarselected">
          </SelectedStyle>
          <Items>
            <igtbar:TBarButton Key="Save" Text="Apply Changes" Image="/hc_v4/img/ed_save.gif">
            </igtbar:TBarButton>
            <igtbar:TBSeparator Key="SaveSep"></igtbar:TBSeparator>
            <igtbar:TBarButton Key="Delete" Text="Delete selected" Image="/hc_v4/img/ed_delete.gif">
            </igtbar:TBarButton>
            <igtbar:TBSeparator Key="DeleteSep"></igtbar:TBSeparator>
            <igtbar:TBarButton Key="Close" Text="Close" Image="/hc_v4/img/ed_cancel.gif">
              <DefaultStyle Width="80px">
              </DefaultStyle>
            </igtbar:TBarButton>
          </Items>
          <DefaultStyle CssClass="hc_toolbardefault">
          </DefaultStyle>
        </igtbar:UltraWebToolbar>
      </td>
    </tr>
    <tr valign="top" style="height: 1px">
      <td>
        <asp:Label ID="lbError" runat="server" CssClass="hc_error" Visible="False">Error message</asp:Label></td>
    </tr>
    <tr height="1">
      <td>
          <igtbl:UltraWebGrid ID="dgValues" runat="server" Width="100%" ImageDirectory="/ig_common/Images/"
            OnInitializeRow="dgValues_InitializeRow">
            <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" SelectTypeRowDefault="Single"
              RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"  CellClickActionDefault="RowSelect"
              NoDataMessage="No input forms attached to this container">
              <HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                </BorderDetails>
              </HeaderStyleDefault>
              <FrameStyle Width="100%" CssClass="dataTable">
              </FrameStyle>
              <RowAlternateStyleDefault CssClass="uga">
              </RowAlternateStyleDefault>
              <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px">
              </RowStyleDefault>
              <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd"
                InitializeLayoutHandler="dg_InitializeLayoutHandler"></ClientSideEvents>
            </DisplayLayout>
            <Bands>
              <igtbl:UltraGridBand AllowSorting="No">
                <Columns>
                  <igtbl:TemplatedColumn Key="Select" Width="22px" FooterText="">
                    <CellStyle VerticalAlign="Top" Wrap="True">
                    </CellStyle>
                    <HeaderTemplate>
                      <asp:CheckBox ID="g_ca" onclick="javascript:return g_su(this);" runat="server"></asp:CheckBox>
                    </HeaderTemplate>
                    <CellTemplate>
                      <asp:CheckBox ID="g_sd" onclick="javascript:return g_su(this);" runat="server"></asp:CheckBox>
                    </CellTemplate>
                  </igtbl:TemplatedColumn>  
                  <igtbl:UltraGridColumn Key="InputFormValueId" Hidden="True" BaseColumnName="Id">
                  </igtbl:UltraGridColumn>
                  <igtbl:UltraGridColumn Key="TermId" Hidden="True" BaseColumnName="TermId">
                  </igtbl:UltraGridColumn>
                  <igtbl:UltraGridColumn HeaderText="Value" Key="ValueEdit" Width="271px" BaseColumnName="Text"
                    AllowUpdate="Yes" CellMultiline="Yes">
                    <CellStyle Wrap="True">
                    </CellStyle>
                    <Header Key="ValueEdit" Caption="Value">
                    </Header>
                  </igtbl:UltraGridColumn>
                  <igtbl:UltraGridColumn Key="CommentEdit" HeaderText="Comment" Width="240px" BaseColumnName="Comment"
                    AllowUpdate="Yes" CellMultiline="Yes">
                    <CellStyle Wrap="True">
                    </CellStyle>
                  </igtbl:UltraGridColumn>
                  <igtbl:UltraGridColumn    HeaderText="IsTranslateHeader" Key="IsTranslateHeader" Width="100px" Type="CheckBox" CellButtonDisplay ="Always"  DataType="System.Boolean"                  
										BaseColumnName="IsTranslatable" AllowUpdate="Yes">
										<CellStyle VerticalAlign="Middle" HorizontalAlign="Center"></CellStyle>
										<Header Caption="IsTranslatable"   Title="Click here to Check/Uncheck all" > </Header>
										
				  </igtbl:UltraGridColumn>
                 <%-- <igtbl:TemplatedColumn Key="IsTranslateRow" Width="120px" FooterText="" BaseColumnName="IsTranslatable">
                    <CellStyle VerticalAlign="Top" Wrap="True">
                    </CellStyle>
                    <HeaderTemplate>
                      <asp:CheckBox ID="IsTranslateHeader" onclick ="javascript:IsTranslateHeader_Click(this);" runat="server" AutoPostBack=false>
                      </asp:CheckBox>IsTranslatable
                    </HeaderTemplate>
                    <CellTemplate>
<%--                      <asp:CheckBox ID="IsTranslateRows" runat="server" AutoPostBack=false Enabled=<%# !Convert.ToBoolean(Container.Cell.Text) %> Checked=<%# Convert.ToBoolean(Container.Cell.Text) %> ></asp:CheckBox>
                   </CellTemplate>
                    </igtbl:TemplatedColumn>--%>
                </Columns>
              </igtbl:UltraGridBand>
            </Bands>
          </igtbl:UltraWebGrid>
      </td>
    </tr>
    <tr valign="top">
      <td>
        <table style="border-right: 1px solid; border-top: 1px solid; border-left: 1px solid;
          border-bottom: 1px solid" cellpadding="0" cellspacing="1" width="100%" class="datatable">
          <tr align="left">
            <td width="18" nowrap>
              &nbsp;</td>
            <td width="271">
              <ajax:Lookup ID="txtValue" runat="server" lookupservice="LookupTerm" pageproperty="Value"
                        nosubmit="true" Width="271" /></td>
            <td width="240">
              <asp:TextBox MaxLength="100" ID="txtComment" runat="server" Width="240px"></asp:TextBox>&nbsp;</td>
            <td valign="middle" align="center">
              <asp:ImageButton ID="btAddRow" runat="server" ImageAlign="Middle" ImageUrl="/hc_v4/img/ed_new.gif" OnClick="btnAddRow_Click">
              </asp:ImageButton>&nbsp;</td>
            <td valign="middle" align="Center">
              <img alt="Insert a special character" src="/hc_v4/img/ed_specialchars.gif" style="cursor: hand"
                onclick="SC()" />
            </td>
            <td valign="middle" align="Center">
              <img alt="Spell check" src="/hc_v4/img/ed_spellcheck.gif" style="cursor: hand" onclick="SP()" />&nbsp;
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </table>
  <ig_spell:WebSpellChecker ID="WebSpellChecker1" runat="server" TextComponentId="txtValue"
    WebSpellCheckerDialogPage="../../Acquire/SpellChecker/IGSpellCheck.Aspx">
    <SpellOptions AllowWordsWithDigits="True" IncludeUserDictionaryInSuggestions="True">
      <PerformanceOptions ConsiderationRange="80" />
    </SpellOptions>
    <DialogOptions ShowNoErrorsMessage="True" WindowWidth="440" WindowHeight="320" />
  </ig_spell:WebSpellChecker>
  <input type="hidden" name="txtSortColPos" id="txtSortColPos" runat="server" value="6">

  <script>
      try { document.getElementById('ctl00$HOCP$txtValue').focus(); } catch (e) { }
  </script>

</asp:Content>
