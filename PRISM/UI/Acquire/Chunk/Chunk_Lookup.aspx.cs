#region Copyright (c)  Hewlett-Packard. All Rights Reserved
/* ---------------------------------------------------------------------*
*        THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE.       *
* --------------------------------------------------------------------- *
 * History Section
 * Date             Name            Description                                         Ref
 * June 15 2009     S.Balakumar     ACQ 10 - ILB O/M + ILB at region level              #ACQ10.0
 * June 15 2009     Mahiba          Removal of Rejection functionality                  #ROR
 * Aug  05 2009     S.Balakumar     UAT: ##BLANK## not appearing at top of Multi-Choice selection #QC2813
 * Aug  11 2009     S.Balakumar     cannot select choice list values for a choice list container - no radio buttons appear #QC2843
 * --------------------------------------------------------------------- *
*/
#endregion
#region uses
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HyperCatalog.Shared;
using HyperCatalog.Business;
using Infragistics.WebUI.UltraWebGrid;
#endregion

namespace HyperCatalog.UI.Acquire.Chunk
{
  /// <summary>
  /// Description résumée de Chunk_Lookup.
  /// </summary>
  public partial class Chunk_Lookup : HCPage
  {
    #region Declarations
    private long itemId = -1;
    private int containerId = -1;
    private bool isMandatory = false;
    protected HyperCatalog.Business.Chunk chunk;
    protected HyperCatalog.Business.Item item;
    protected HyperCatalog.Business.Culture culture;
    protected InputFormContainer ifContainer;
    bool bMultiChoice = false;
    private int nbSelected = 0;
    #endregion

    #region Code généré par le Concepteur Web Form
    override protected void OnInit(EventArgs e)
    {
      //
      // CODEGEN : Cet appel est requis par le Concepteur Web Form ASP.NET.
      //
      InitializeComponent();
      base.OnInit(e);
      ChunkButtonBar.DraftClick += new ChunkButtonBarClickEventHandler(Chunk_DraftClick);
      ChunkButtonBar.FinalClick += new ChunkButtonBarClickEventHandler(Chunk_FinalClick);
      ChunkButtonBar.DeleteClick += new ChunkButtonBarClickEventHandler(Chunk_DeleteClick);
      ChunkButtonBar.CopyClick += new ChunkButtonBarClickEventHandler(Chunk_CopyClick);
      //ChunkButtonBar.RejectClick += new ChunkButtonBarClickEventHandler(Chunk_RejectClick); #ROR - Commented
    }

    /// <summary>
    /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
    /// le contenu de cette méthode avec l'éditeur de code.
    /// </summary>
    private void InitializeComponent()
    {
      this.dg.InitializeRow += new Infragistics.WebUI.UltraWebGrid.InitializeRowEventHandler(this.dg_InitializeRow);
      this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);

    }
    #endregion

    //QC# 7028 Fix by Rekha Thomas. Added OnUpdateCell event handler to retain check box value
    protected void dg_UpdateCell(object sender, EventArgs e)
    {

    }

    protected void Page_Load(object sender, System.EventArgs e)
    {
      try
      {
        containerId = Convert.ToInt32(Request["d"]);
        if (Request["m"] != null)
          isMandatory = Convert.ToBoolean(Request["m"]);
        culture = QDEUtils.UpdateCultureCodeFromRequest();
        item = QDEUtils.GetItemIdFromRequest();
        itemId = item.Id;
        chunk = ChunkWindow.GetChunk(itemId, containerId, culture.Code);
        uwToolbar.Enabled = Request["ui"] != null;

        if (Request["ifid"] != null)
        {
          ifContainer = InputFormContainer.GetByKey(Convert.ToInt32(Request["ifid"]));
        }
        /*#ACQ10.0 Starts --Commented
         Commented to bring out ILB for all catalogue irespective of mandatory status
        if (!isMandatory || culture.Type == CultureType.Regionale)
        {
        UITools.HideToolBarButton(uwToolbar, "ilb");
        }
          */
        UITools.HideToolBarButton(uwToolbar, "ilb");
        //#ACQ10.0 Ends

        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "blanktext", "<script>ILBText = '" + HyperCatalog.Business.Chunk.BlankText + "';</script>");

        //Modified this line for QCs# 839 and 1028
        ChunkButtonBar.Chunk = chunk;
        //Modified this line for QCs# 839 and 1028
        
        ChunkButtonBar.Container = SessionState.QDEContainer;
        ChunkButtonBar.User = SessionState.User;
        ChunkButtonBar.Culture = culture;
        ChunkButtonBar.Item = item;
        if (!Page.IsPostBack)
        {
          
          //Added these lines for QCs# 839 and 1028
          ChunkComment1.Chunk = chunk;
          ChunkModifier1.Chunk = chunk;
          //Added these lines for QCs# 839 and 1028

          lbResult.Text = string.Empty;
          UpdateDataView();
        }
        else
        {
          if (Request["__EVENTTARGET"] != null) // Check if user is trying to sort a group
          {
            if (Request["__EVENTTARGET"].ToString() == "rowup" || Request["__EVENTTARGET"].ToString() == "rowdown")
            {
              SortRow(Request["__EVENTTARGET"].ToString() == "rowup", Convert.ToInt32(Request["__EVENTARGUMENT"]));
            }
          }
        }
        
      }

      catch (Exception ex)
      {
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "close", "<script>alert('" + UITools.CleanJSString(ex.ToString()) + "');top.window.close();</script>");
      }
    }

    #region Binding methods
    private string GetSortJs()
    {
      // Call this method and then rely on index to process sort!
      if (dg.Page != null)
      {
        return "<img title=\"move down (immediated save!)\" src=\"/hc_v4/img/ed_down.gif\" onclick=\"cg_down()\">" +
               "<img title=\"move up (immediated save!)\" src=\"/hc_v4/img/ed_up.gif\" onclick=\"cg_up()\">";
      }
      return string.Empty;
    }


    public static bool EnableSort(ref UltraWebGrid dg, int sortColIndex)
    {
      // Call this method and then rely on index to process sort!
      if (dg.Page != null)
      {
        if (dg.Columns.FromKey("s_a") != null)
        {
          dg.Columns.Remove(dg.Columns.FromKey("s_a"));
        }
        UltraGridColumn sortCol = new UltraGridColumn("s_a", "", ColumnType.NotSet, null);
        dg.Columns.Insert(sortColIndex, sortCol);
        dg.Columns.FromKey("s_a").Width = Unit.Pixel(35);
        return true;
      }
      return false;
    }

    private void UpdateDataView()
    {
      if (ifContainer != null)
      {
        bMultiChoice = ifContainer.Type == InputFormContainerType.MultiChoiceList;
        if (bMultiChoice)
        {
          EnableSort(ref dg, Convert.ToInt32(txtSortColPos.Value));
          dg.DisplayLayout.ClientSideEvents.AfterCellUpdateHandler = "dg_AfterCellUpdateHandler";
        }
        dg.Columns.FromKey("cChoose").ServerOnly = bMultiChoice;
        dg.Columns.FromKey("InScope").ServerOnly = !bMultiChoice;
        BindPossibleValuesTable();
      }
      else
      {
        bMultiChoice = SessionState.QDEContainer.Lookup.MultiChoice;
        if (bMultiChoice)
        {
          dg.DisplayLayout.ClientSideEvents.AfterCellUpdateHandler = "dg_AfterCellUpdateHandler";
          EnableSort(ref dg, Convert.ToInt32(txtSortColPos.Value));
        }
        dg.DisplayLayout.AllowSortingDefault = Infragistics.WebUI.UltraWebGrid.AllowSorting.No;
        dg.Columns.FromKey("cChoose").ServerOnly = bMultiChoice;
        dg.Columns.FromKey("InScope").ServerOnly = !bMultiChoice;
        BindLookupTable();
      }
      if (bMultiChoice)
      {
        Page.ClientScript.RegisterClientScriptInclude("grid", "/hc_v4/js/hypercatalog_grid.js");
        Page.ClientScript.RegisterStartupScript(this.GetType(), "load", "<script>window.onload=g_i</script>");
        BubbleCheckedValues();
      }
      if (chunk != null)
      {
        imgStatus.ImageUrl = "/hc_v4/img/S" + HyperCatalog.Business.Chunk.GetStatusFromEnum(chunk.Status) + ".gif";
        lbStatus.Text = chunk.Status.ToString();
        ChunkModifier1.Visible = true;
      }
      else
      {
        imgStatus.ImageUrl = "/hc_v4/img/SM.gif";
        lbStatus.Text = " [Missing]";
        ChunkModifier1.Visible = false;
      }
    }

    private void BindLookupTable()
    {
      ViewState["Source"] = "Lookup";
      using (LookupGroup lg = HyperCatalog.Business.Container.GetByKey(containerId).Lookup)
      {
        if (lg != null)
        {
          if (lg.Values != null)
          {
            dg.DataSource = lg.Values;
            Utils.InitGridSort(ref dg, false);
            dg.DataBind();
            //ACQ10.0 Starts
            UltraGridRow row = new UltraGridRow();
            dg.Rows.Insert(0, row);
            dg.Rows[0].Cells.FromKey("cChoose").Text = "<input id='rd' type='radio' name='rd' value='##BLANK##' onclick='dc()'/>##BLANK##";
            dg.Rows[0].Cells.FromKey("Value").Text = "##BLANK##";
            dg.Rows[0].Cells.FromKey("Comment").Text = "To authour ILB ";
            //ACQ10.0 Ends

            if (!Page.IsPostBack)
            {
                if (chunk != null)
                {
                    if (chunk.Text == HyperCatalog.Business.Chunk.BlankValue || chunk.Text == HyperCatalog.Business.Chunk.BlankText)
                    {
                        dg.Rows[0].Cells.FromKey("cChoose").Text = "<input id='rd' type='radio' name='rd' value='##BLANK##' checked onclick='dc()'/>";
                        if (bMultiChoice)
                        {
                            //dg.Columns.FromKey("InScope").ServerOnly = uwToolbar.Items.FromKeyButton("ilb").Selected = true;
                            dg.Rows[0].Cells.FromKey("InScope").Value = true;
                        }
                        else
                        {
                            //dg.Columns.FromKey("cChoose").ServerOnly = uwToolbar.Items.FromKeyButton("ilb").Selected = true;
                        }
                    }
                }
            }
          }
        }
      }
    }

    private void BindPossibleValuesTable()
    {
      ViewState["Source"] = "PossibleValues";
      using (InputFormValueList lg = ifContainer.PossibleValues){
        if (lg != null)
        {

          dg.DataSource = lg;  
          Utils.InitGridSort(ref dg, false);
          dg.DataBind();
          //ACQ10.0 Starts
          UltraGridRow row = new UltraGridRow();
          dg.Rows.Insert(0, row);
          dg.Rows[0].Cells.FromKey("Id").Value = 0;
          dg.Rows[0].Cells.FromKey("Id").Text = "0";
          dg.Rows[0].Cells.FromKey("InScope").Value = false;
          dg.Rows[0].Cells.FromKey("cChoose").Text = "<input id='rd' type='radio' name='rd' value='##BLANK##' onclick='dc()'/>";
          dg.Rows[0].Cells.FromKey("Value").Text = "##BLANK##";
          dg.Rows[0].Cells.FromKey("Comment").Text = "To authour ILB ";
          //ACQ10.0 Ends
          if (!Page.IsPostBack)
          {
              if (chunk != null)
              {
                  if (chunk.Text == HyperCatalog.Business.Chunk.BlankValue || chunk.Text == HyperCatalog.Business.Chunk.BlankText)
                  {
                      if (bMultiChoice)
                      {
                          //dg.Columns.FromKey("InScope").ServerOnly = uwToolbar.Items.FromKeyButton("ilb").Selected = true;
                          dg.Rows[0].Cells.FromKey("cChoose").Text = "<input id='rd' type='radio' name='rd' value='##BLANK##' checked onclick='dc()'/>";
                          dg.Rows[0].Cells.FromKey("InScope").Value = true;
                      }
                      else
                      {
                          //dg.Columns.FromKey("cChoose").ServerOnly = uwToolbar.Items.FromKeyButton("ilb").Selected = true;
                          dg.Rows[0].Cells.FromKey("cChoose").Text = "<input id='rd' type='radio' name='rd' value='##BLANK##' checked onclick='dc()'/>";
                      }
                  }
              }
          }  
        }
      }
    }

    #endregion
    #region "Sort/Bubble"
    private void BubbleCheckedValues()
    {

      Trace.Warn("BubbleCheckedValues start");
      if (chunk != null)
      {
        Trace.Warn("  Chunk Value = " + chunk.Text);
        char separator = ';';
        string[] values = chunk.Text.Split(new char[] { separator });
        // #QC2813
        //Commmented the is code becaues there will ##BLANK## as first value and should not be sorted based on selection
        // setting this value to 1 will traverse the rest of the rows and 
        //int nbFound = 0; 
        int nbFound = 1; 
        Trace.Warn("  Nb Values = " + values.Length.ToString());
        for (int i = 0; i < values.Length; i++)
        {
          //for (int j = 0; j < dg.Rows.Count; j++) // #QC2813 travers from the second row
          for (int j = 1; j < dg.Rows.Count; j++) 
          {
            if (dg.Rows[j].Cells.FromKey("Value").ToString().Trim() == values[i].Trim())
            {
              Trace.Warn("Found value = " + dg.Rows[j].Cells.FromKey("Value").ToString().Trim() + " moved to position [" + nbFound.ToString() + "]");
              UltraGridRow r = dg.Rows[j];
              dg.Rows.Remove(r);
              dg.Rows.Insert(nbFound, r);
              nbFound++;
              j = dg.Rows.Count + 1;
            }
          }
        }
      }
      else
      {
        Trace.Warn("  Chunk is null = (itemId=" + itemId + ", containerId=" + containerId + ", culture.Code=" + culture.Code + ")");
      }
    }
    private void SortRow(bool moveUp, int activeRowIndex)
    {
      int i = activeRowIndex;
      int targetRowIndex = -1;
      if (moveUp)
      {
        targetRowIndex = i - 1;
        if (targetRowIndex >= 0)
        {
          UltraGridRow r = dg.Rows[activeRowIndex];
          dg.Rows.Remove(r);
          dg.Rows.Insert(targetRowIndex, r);
          Page.ClientScript.RegisterStartupScript(Page.GetType(), "datachange", "dc()", true);
        }
      }
      else
      {
        targetRowIndex = i + 1;
        if (targetRowIndex < dg.Rows.Count)
        { // Move Up the group we have found
          SortRow(true, targetRowIndex);
        }
      }
    }

    #endregion
    #region Object events
    private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      if (!bMultiChoice)
      {
        if (chunk != null)
        {
          if (e.Row.Cells.FromKey("Value").Text == chunk.Text)
          {
            e.Row.Cells.FromKey("cChoose").Text = "<input id='rd' type='radio' name='rd' value='" + e.Row.Cells.FromKey("Id").Text + "' checked onclick='dc()'/>";
            e.Row.Selected = true;
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "scrollview", "<script>rowIndex = " + e.Row.Index.ToString() + ";</script>");
          }
          else
          {
            e.Row.Cells.FromKey("cChoose").Text = "<input id='rd' type='radio' name='rd' value='" + e.Row.Cells.FromKey("Id").Text + "' onclick='dc()'/>";
          }
        }
        else
        {
          if (e.Row.Index == 0)
          {
            e.Row.Cells.FromKey("cChoose").Text = "<input id='rd' type='radio' name='rd' value='" + e.Row.Cells.FromKey("Id").Text + "' checked onclick='dc()'/>";
          }
          else
          {
            e.Row.Cells.FromKey("cChoose").Text = "<input id='rd' type='radio' name='rd' value='" + e.Row.Cells.FromKey("Id").Text + "' onclick='dc()'/>";
          }
        }
      }
      else
      {
        e.Row.DataKey = e.Row.Cells.FromKey("Id").Value;
        if (dg.Columns.FromKey("s_a") != null)
        {
          e.Row.Cells.FromKey("s_a").Value = GetSortJs();
        }

        if (chunk != null)
        {
          char separator = ';';
          string[] values = chunk.Text.Split(new char[] { separator });
          for (int i = 0; i < values.Length; i++)
          {
            if (values[i].Trim() == e.Row.Cells.FromKey("Value").Text.Trim())
            {
              e.Row.Cells.FromKey("InScope").Value = true;
              Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "scrollview", "<script>rowIndex = " + e.Row.Index.ToString() + ";</script>");
              break;
            }
          }
        }
        else
        {
          if (e.Row.Index == 0)
          {
            e.Row.Cells.FromKey("InScope").Value = true;
          }
        }
      }
      Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;
    }

    protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      if (be.Button.Key == "ilb")
      {
        if (bMultiChoice)
        {
          dg.Columns.FromKey("InScope").ServerOnly = be.Button.Selected;
        }
        else
        {
          dg.Columns.FromKey("cChoose").ServerOnly = be.Button.Selected;
        }

        if (be.Button.Selected)
        {
          if (dg.Columns.FromKey("s_a") != null)
          {
            dg.Columns.FromKey("s_a").ServerOnly = true;
          }
          Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "blank", "<script>isBlankChunk = true;</script>");
        }
        else
        {
          if (dg.Columns.FromKey("s_a") != null)
          {
            dg.Columns.FromKey("s_a").ServerOnly = false;
          }
        }
      }
      else
      {
        UpdateDataView();
      }
    }
    #endregion
    #region Save/Delete Methods
    private void SaveChunk(ChunkStatus status, bool lockTranslations)
    {
      string error = string.Empty;
      string Value = string.Empty;
      if (Request["rd"] != null || !dg.Columns.FromKey("InScope").ServerOnly || uwToolbar.Items.FromKeyButton("ilb").Selected)
      {
          //#ACQ10.0 Starts
          if (Request["rd"] == HyperCatalog.Business.Chunk.BlankText)
          {
              uwToolbar.Items.FromKeyButton("ilb").Pressed(true);
              uwToolbar.Items.FromKeyButton("ilb").Selected = true;
          }
          //#ACQ10.0 Ends
        if (uwToolbar.Items.FromKeyButton("ilb").Selected)
        {
          Value = HyperCatalog.Business.Chunk.BlankValue;
        }
        else if (Request["rd"] != null) // --> radion button (single choice)
        {
          if (ViewState["Source"].ToString() == "Lookup")
          {
            LookupValue lValue = LookupValue.GetByKey(Convert.ToInt32(Request["rd"]));
            Value = lValue.Text;
          }
          else
          {
            InputFormValue lValue = InputFormValue.GetByKey(Convert.ToInt32(Request["rd"]));
            Value = lValue.Text;
          }
        }
        else // (multi choice)
        {
          string separator = "; ";
          bool success = true;
          string curText = string.Empty;
          if (dg != null && dg.Rows != null && dg.Rows.Count > 0)
          {
            foreach (UltraGridRow r in dg.Rows)
            {
              if (Convert.ToBoolean(r.Cells.FromKey("InScope").Value))
              {
                  //#ACQ10.0 Stats  Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "scrollview", "<script>rowIndex = " + e.Row.Index.ToString() + ";</script>");
                  if ((Convert.ToInt32(r.Cells.FromKey("Id").Value) != 0) && (Value.IndexOf(HyperCatalog.Business.Chunk.BlankValue) < 0))
                  {
                   //#ACQ10.0 Ends
                      if (ViewState["Source"].ToString() == "Lookup")
                      {
                          LookupValue lValue = LookupValue.GetByKey(Convert.ToInt32(r.Cells.FromKey("Id").Value));
                          curText = lValue.Text;
                      }
                      else
                      {
                          InputFormValue lValue = InputFormValue.GetByKey(Convert.ToInt32(r.Cells.FromKey("Id").Value));
                          curText = lValue.Text;
                      }
                      if (Value.Length > 0)
                          Value += separator.ToString();
                      Value += curText;
                  }
                  else
                  {
                      Value = HyperCatalog.Business.Chunk.BlankValue;
                  }
              }
            }
          }
        }
          //ACQ10.0 Starts
          //If the value is empty the user will get a message asking the select a value and no value will be saved into application
        if (Value.Length <= 0)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "<script>alert('Please, select a value!');</script>");
            return;
        }
          //ACQ10.0 Ends

        if (chunk != null)
        // Test if user has made a change that allows database update
        {
          if (Value != chunk.Text || ChunkComment1.Comment != chunk.Comment || status != chunk.Status)
          {
            chunk.Text = Value;
            chunk.Comment = ChunkComment1.Comment;
            chunk.Status = status;
          }
        }
        else
        {
          chunk = new HyperCatalog.Business.Chunk(itemId, containerId, culture.Code, Value, ChunkComment1.Comment, status, SessionState.User.Id);
        }
        if (chunk.Save(SessionState.User.Id))
        {

          //Added this line for QCs# 839 and 1028
          chunk.ModifyDate = DateTime.UtcNow;
          //Added this line for QCs# 839 and 1028

          lbResult.Text = "<br/>Chunk saved!";
          lbResult.CssClass = "hc_success";
          lbResult.Visible = true;
          if (chunk.Text == HyperCatalog.Business.Chunk.BlankValue)
          {
             chunk.Text = HyperCatalog.Business.Chunk.BlankText;
          }
          Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "update", "<script>UpdateGrid('" + HyperCatalog.Business.Chunk.GetStatusFromEnum(chunk.Status) + "', '" + UITools.CleanJSString(chunk.Text) + "');</script>");
          if (!lockTranslations)
          {
            chunk.ForceTranslationsTo(SessionState.User.Id, ChunkStatus.Draft);
            SessionState.QDEChunk = chunk;
          }
          
          //Added this line for QCs# 839 and 1028
          ChunkModifier1.Chunk = chunk;
          //Added this line for QCs# 839 and 1028

        }
        else
        {
          lbResult.Text = "<br/>Error: " + HyperCatalog.Business.Chunk.LastError;
          lbResult.CssClass = "hc_error";
        }
      }
      else
      {
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "<script>alert('Please, select a value!');</script>");
      }
      
    }


    private void DeleteChunk()
    {
      if (chunk.Delete(SessionState.User.Id))
      {
        chunk = null;
        uwToolbar.Items.FromKeyButton("ilb").Pressed(false);
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "update", "<script>UpdateGrid('" + HyperCatalog.Business.Chunk.GetStatusFromEnum(ChunkStatus.Missing) + "', '');</script>");
      }
      else
      {
        lbResult.Text = "<br/>Error: " + HyperCatalog.Business.Chunk.LastError;
        lbResult.CssClass = "hc_error";
      }
    }
    #endregion

    #region Chunk Event handlers
    private void Chunk_DraftClick(object sender, ChunkButtonBarEventArgs e)
    {
      SaveChunk(ChunkStatus.Draft, e.LockTranslations);
      SessionState.QDEChunk = chunk;
      //UpdateDataView();
    }

    private void Chunk_FinalClick(object sender, ChunkButtonBarEventArgs e)
    {
      SaveChunk(ChunkStatus.Final, e.LockTranslations);
      SessionState.QDEChunk = chunk;
      //UpdateDataView();
    }

    private void Chunk_DeleteClick(object sender, ChunkButtonBarEventArgs e)
    {
      DeleteChunk();
      SessionState.QDEChunk = chunk;
      //UpdateDataView();
    }
    private void Chunk_CopyClick(object sender, ChunkButtonBarEventArgs e)
    {
      HyperCatalog.Business.Chunk c = HyperCatalog.Business.Chunk.GetByKey(itemId, containerId, culture.FallbackCode);
      if (c != null)
      {
        // TO DO
        //txtValue.Text = c.Text;
        lbResult.Text = "Valued copied from master";
        lbResult.CssClass = "hc_success";
        lbResult.Visible = true;
      }
      else
      {
        lbResult.Text = "No master value found";
        lbResult.CssClass = "hc_error";
        lbResult.Visible = true;
      }
    }
    /*
     * #ROR Starts - Commented 
    private void Chunk_RejectClick(object sender, ChunkButtonBarEventArgs e)
    {
      HyperCatalog.Business.Chunk c = HyperCatalog.Business.Chunk.GetByKey(itemId, containerId, culture.FallbackCode);
      if (c != null)
      {
        c.CultureCode = culture.Code;
        c.Status = ChunkStatus.Rejected;
        if (c.Save(SessionState.User.Id))
        {
          // TO DO
          //txtValue.Text = c.Text;
          lbResult.Text = "Valued rejected with success";
          lbResult.CssClass = "hc_success";
          lbResult.Visible = true;
        }
        else
        {
          lbResult.Text = "Cannot reject " + HyperCatalog.Business.Chunk.LastError;
          lbResult.CssClass = "hc_error";
          lbResult.Visible = true;
        }
      }
    }
     * #ROR Ends
      */
    #endregion
}
}

