#region Copyright (c)  Hewlett-Packard. All Rights Reserved
/* ---------------------------------------------------------------------*
*        THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE.       *
* --------------------------------------------------------------------- *
 * History Section
 * Date             Name            Description                                         Ref
 * June 02 2009     S.Balakumar     ACQ - 11 (ACQ - 8.20) - Translating Choice Lists    #ACQ8.20
 * June 29 2009     S.Balakumar     QC 2691               - Term check if it alreayd exists has been added 
 * July 07 2009     S.Balakumar     QC 2712               - Not able to add new value in Value tab of choice list in inputform containers #2712
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
using HyperCatalog.Business;
using Infragistics.WebUI.UltraWebGrid;
using HyperCatalog.Shared;
using Infragistics.WebUI.WebDataInput;
#endregion uses

public partial class InputForms_ContainersEditValues : HCPage
{
    #region Declarations
    public HyperCatalog.Business.InputFormContainer ifcObj;
    private System.Int64 InputFormContainerId;
    private int InputFormId;
    private bool IsTranslateDefaultOption = false;//ACQ8.20
    #endregion

    private void InitializeComponent()
    {
        if (System.IO.File.Exists(SessionState.CacheParams["DictionarySpecificPath"].Value.ToString()))
        {
            WebSpellChecker1.UserDictionaryFile = SessionState.CacheParams["DictionarySpecificPath"].Value.ToString();
        }
    }

    protected void Page_Load(object sender, System.EventArgs e)
    {
        //#ACQ8.20 Starts
        try
        {
            if ("1".Equals(Convert.ToString(ApplicationSettings.Parameters["TermsIsTranslatableDefault"].Value)))
                IsTranslateDefaultOption = true;
            else
                IsTranslateDefaultOption = false;
        }
        catch (Exception ex) { }
        //#ACQ8.20 Ends
        Page.Form.Attributes["autocomplete"] = "off";
        #region Spell Checker
        WebSpellChecker1.SpellOptions.AllowCaseInsensitiveSuggestions = true;
        WebSpellChecker1.SpellOptions.AllowMixedCase = false;
        WebSpellChecker1.SpellOptions.AllowWordsWithDigits = true;
        WebSpellChecker1.SpellOptions.AllowXML = true;
        WebSpellChecker1.SpellOptions.CheckHyphenatedText = true;
        WebSpellChecker1.SpellOptions.IncludeUserDictionaryInSuggestions = true;
        WebSpellChecker1.SpellOptions.PerformanceOptions.AllowCapitalizedWords = true;
        WebSpellChecker1.SpellOptions.PerformanceOptions.CheckCompoundWords = false;
        WebSpellChecker1.SpellOptions.PerformanceOptions.ConsiderationRange = -1;
        WebSpellChecker1.SpellOptions.PerformanceOptions.SplitWordThreshold = 3;
        WebSpellChecker1.SpellOptions.PerformanceOptions.SuggestSplitWords = true;
        WebSpellChecker1.SpellOptions.SeparateHyphenWords = false;
        #endregion

        if (SessionState.User.IsReadOnly)
        {
            uwToolbar.Items.FromKeyButton("Save").Enabled = false;
            uwToolbar.Items.FromKeyButton("Delete").Enabled = false;
        }
        if (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_CARTOGRAPHY))
        {
            UITools.HideToolBarSeparator(uwToolbar, "SaveSep");
            UITools.HideToolBarButton(uwToolbar, "Save");
            UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
            UITools.HideToolBarButton(uwToolbar, "Delete");
        }
        try
        {
            if (Request["c"] != null)
                InputFormContainerId = Convert.ToInt64(Request["c"]);
            ifcObj = InputFormContainer.GetByKey(InputFormContainerId);
            if (Request["f"] != null)
                InputFormId = Convert.ToInt32(Request["f"]);

            if (!Page.IsPostBack)
            {
                UpdateDataEdit();
                WebSpellChecker1.WebSpellCheckerDialogPage += "?i=0&c=" + ifcObj.ContainerId.ToString();
            }
            dgValues.DisplayLayout.ClientSideEvents.ColumnHeaderClickHandler = "dgValues_ColumnHeaderClickHandler";
        }
        catch
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "close", "<script>parent.close();</script>");
        }
    }

    // Display data
    protected void UpdateDataEdit()
    {
        using (InputFormValueList valuesList = InputFormValue.GetAll("InputFormContainerId = " + InputFormContainerId))
        {
            dgValues.DataSource = valuesList;
            Utils.InitGridSort(ref dgValues, false);
            dgValues.DataBind();
            Utils.EnableIntelligentSort(ref dgValues, Convert.ToInt32(txtSortColPos.Value));

            if (valuesList.Count > 0)
            {
                if (SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_CARTOGRAPHY))
                {
                    UITools.ShowToolBarButton(uwToolbar, "Delete");
                    UITools.ShowToolBarSeparator(uwToolbar, "DeleteSep");
                }
            }
            else
            {
                UITools.HideToolBarButton(uwToolbar, "Delete");
                UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
            }
        }
    }
    /// <summary>
    /// Toolbar actions
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="be"></param>
    protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
        string btn = be.Button.Key.ToLower();
        if (btn == "save")
        {
            Save();
        }
        if (btn == "delete")
        {
            Delete();
        }
    }

    /// <summary>
    /// Save IFContainer value
    /// </summary>
    private void Save()
    {
        bool isOk = false;
        for (int i = 0; i < dgValues.Rows.Count; i++)
        {
            if (dgValues.Rows[i].Cells.FromKey("ValueEdit").Value == null)
            {
                isOk = false;
            }
            else
            {
                //////////
                string sValue = dgValues.Rows[i].Cells.FromKey("ValueEdit").Value.ToString();
                string sComment = (dgValues.Rows[i].Cells.FromKey("CommentEdit").Value == null ? string.Empty : dgValues.Rows[i].Cells.FromKey("CommentEdit").Value.ToString());
                string termId = dgValues.Rows[i].Cells.FromKey("TermId").Text;
                //#ACQ8.20 Starts////

                //TemplatedColumn col = (TemplatedColumn)dgValues.Rows[i].Cells.FromKey("IsTranslateRow").Column;
                //CheckBox cb = (CheckBox)((CellItem)col.CellItems[i]).FindControl("IsTranslateRows");
                bool isTranslatable = (bool)dgValues.Rows[i].Cells.FromKey("IsTranslateHeader").Value;
                //#ACQ8.20 Ends
                string inputFormValueId = dgValues.Rows[i].Cells.FromKey("InputFormValueId").Text;

                //InputFormValue IFValue = new InputFormValue(Convert.ToInt64(inputFormValueId),
                //InputFormContainerId, Convert.ToInt64(termId),
                //i, sValue, sComment,, SessionState.User.Id, SessionState.User.Id, DateTime.UtcNow, DateTime.UtcNow);//ACQ8.20 Commented

                InputFormValue IFValue = new InputFormValue(Convert.ToInt64(inputFormValueId),
                  InputFormContainerId, Convert.ToInt64(termId),
                  i, sValue, sComment, isTranslatable, SessionState.User.Id, SessionState.User.Id, DateTime.UtcNow, DateTime.UtcNow);//ACQ8.20

                if (!IFValue.Save())
                {
                    isOk = false;
                    lbError.CssClass = "hc_error";
                    lbError.Text = InputFormValue.LastError;
                    lbError.Visible = true;
                    break;
                }
                else
                    isOk = true;
            }
            if (isOk) // Success
            {
                // Display message of success
                lbError.CssClass = "hc_success";
                lbError.Text = "Data saved!";
                lbError.Visible = true;
            }
        }

        UpdateDataEdit();
    }

    /// <summary>
    /// Delete IFContainer value
    /// </summary>
    private void Delete()
    {
        for (int i = 0; i < dgValues.Rows.Count; i++)
        {
            TemplatedColumn col = (TemplatedColumn)dgValues.Rows[i].Cells.FromKey("Select").Column;
            CheckBox cb = (CheckBox)((CellItem)col.CellItems[i]).FindControl("g_sd");
            if (cb.Checked)
            {
                InputFormValue ifValueObj = InputFormValue.GetByKey(Convert.ToInt32(dgValues.Rows[i].Cells.FromKey("InputFormValueId").Text));
                if (!ifValueObj.Delete(HyperCatalog.Shared.SessionState.User.Id))
                {
                    lbError.CssClass = "hc_error";
                    lbError.Text = InputFormValue.LastError;
                    lbError.Visible = true;
                    break;
                }
            }
        }
        UpdateDataEdit();
    }

  


    protected void dgValues_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
        //		TemplatedColumn col;
        //		// Display Value
        //		col = (TemplatedColumn)e.Row.Cells.FromKey("ValueEdit").Column;                    
        //		TextBox txtValue = (TextBox)((CellItem)col.CellItems[e.Row.Index]).FindControl("ValueThis");
        //    txtValue.Text = e.Row.Cells.FromKey("TermValue").Text;
        //    TextBox txtInputFormValueId = (TextBox)((CellItem)col.CellItems[e.Row.Index]).FindControl("InputFormValueId");
        //    txtInputFormValueId.Text = e.Row.Cells.FromKey("InputFormValueId").Text;
        //    TextBox txtTermId = (TextBox)((CellItem)col.CellItems[e.Row.Index]).FindControl("TermId");
        //    txtTermId.Text = e.Row.Cells.FromKey("TermId").Text;
        //		col = (TemplatedColumn)e.Row.Cells.FromKey("CommentEdit").Column;                    
        //		TextBox txtComment = (TextBox)((CellItem)col.CellItems[e.Row.Index]).FindControl("CommentThis");
        //		txtComment.Text = e.Row.Cells.FromKey("Comment").Text;


        if ((bool)e.Row.Cells[5].Value)
            e.Row.Cells[5].AllowEditing = AllowEditing.No;

    }

    protected void btnAddRow_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        lbError.Visible = false;
        if (txtValue.Value.Length > 0)
        {
            if (ifcObj.Type == InputFormContainerType.MultiChoiceList && txtValue.Value.ToString().IndexOf(';') >= 0)
            {
                lbError.CssClass = "hc_error";
                lbError.Text = "Value cannot contain the ';' character";
                lbError.Visible = true;
            }
            else
            {
                //#ACQ8.20 Starts
                //InputFormValue IFValue = new InputFormValue(-1, InputFormContainerId, -1, dgValues.Rows.Count + 1, txtValue.Value, txtComment.Text, SessionState.User.Id, -1, DateTime.UtcNow, null);
                //QC2691 Check for validation of Terms if it already exists was not there so added 
                TermList Termdetails = Term.GetAll("TermValue = '" + txtValue.Value + "' AND TermTypeCode='C'");
                InputFormValue IFValue;
                if (Termdetails == null || Termdetails.Count == 0) //#2712 
                //if (Termdetails == null) //#2712 Commented
                {
                    IFValue = new InputFormValue(-1, InputFormContainerId, -1, dgValues.Rows.Count + 1, txtValue.Value, txtComment.Text, IsTranslateDefaultOption, SessionState.User.Id, -1, DateTime.UtcNow, null);
                }
                else
                {
                    IFValue = new InputFormValue(-1, InputFormContainerId, -1, dgValues.Rows.Count + 1, txtValue.Value, txtComment.Text, Termdetails[0].IsTranslatable, SessionState.User.Id, -1, DateTime.UtcNow, null);
                }

                //#ACQ8.20 Ends


                if (!IFValue.Save())
                {
                    lbError.CssClass = "hc_error";
                    lbError.Text = InputFormValue.LastError;
                    lbError.Visible = true;
                }
                else
                {
                    txtComment.Text = string.Empty;
                    txtValue.Value = string.Empty;
                    UpdateDataEdit();
                }
            }
        }
        else
        {
            lbError.CssClass = "hc_error";
            lbError.Text = "Value cannot be empty";
            lbError.Visible = true;
        }
    }
}
