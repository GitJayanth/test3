#region Copyright (c)  Hewlett-Packard. All Rights Reserved
/* ---------------------------------------------------------------------*
*        THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE.       *
* --------------------------------------------------------------------- *
 * History Section
 * Date             Name            Description                                         Ref
 * June 15 2009     S.Balakumar     ACQ 10 - ILB O/M + ILB at region level              #ACQ10.0
 * June 15 2009     Mahibah         Removal of Rejection functionality                  #ROR
 * Nov 17th 2011    Prachi          To Validate ChunkEdit Window for XSS Vulnerability
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
#endregion

namespace HyperCatalog.UI.Acquire.Chunk
{
    /// <summary>
    /// Description résumée de Chunk_Text.
    /// </summary>
    public partial class Chunk_Text : HCPage
    {
        #region Declarations

        private long itemId = -1;
        private int containerId = -1;
        private bool isMandatory = false;
        protected HyperCatalog.Business.Chunk chunk;
        protected HyperCatalog.Business.Container container;
        protected HyperCatalog.Business.Item item;
        protected HyperCatalog.Business.Culture culture;
        //code added on 17th November 2011 for Chunk Edit Validation for XSS Vulnerability Fix
        public string keyword = String.Empty;
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
            //ChunkButtonBar.RejectClick +=new ChunkButtonBarClickEventHandler(Chunk_RejectClick); ROR Commmented 
        }

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            if (System.IO.File.Exists(SessionState.CacheParams["DictionarySpecificPath"].Value.ToString()))
            {
                WebSpellChecker1.UserDictionaryFile = SessionState.CacheParams["DictionarySpecificPath"].Value.ToString();
            }
        }

        #endregion

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
                container = SessionState.QDEContainer;
                uwToolbar.Enabled = Request["ui"] != null;
                
                //code added on 17th November 2011 for Chunk Edit Validation for XSS Vulnerability Fix - start
                if(SessionState.CacheParams.Exists ("XSS_RestrictedHTMLTags"))
                    keyword = SessionState.CacheParams["XSS_RestrictedHTMLTags"].Value.ToString();
                //code added on 17th November 2011 for Chunk Edit Validation for XSS Vulnerability Fix - end

                #region Spell Checker
                string masterLanguage = string.Empty;
                masterLanguage = HyperCatalog.Shared.SessionState.MasterCulture.LanguageCode;
                if (culture.LanguageCode != masterLanguage)
                {
                    UITools.HideToolBarSeparator(uwToolbar, "spellsep");
                    UITools.HideToolBarButton(uwToolbar, "spell");
                }
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VarsForSpellChecker", "<script>var itemId=" + itemId.ToString() + ";var containerId=" + containerId + ";</script>");
                #endregion

                /*#ACQ10.0 Starts
        // Commented to bring out ILB for all catalogue irespective of mandatory status
	    // Only allow ILB for mandatory chunks and only at sku level for push down
        if (!isMandatory  || culture.Type == CultureType.Regionale) 
                {
                    UITools.HideToolBarSeparator(uwToolbar, "ilbSep");
                    UITools.HideToolBarButton(uwToolbar, "ilb");
                }
         * #ACQ10.0 Ends
        */
            }
            catch
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "close", "<script>alert('Chunk not found!');window.close()</script>");
            }

            ChunkComment1.Chunk = ChunkModifier1.Chunk = ChunkButtonBar.Chunk = chunk;
            ChunkButtonBar.Container = container;
            ChunkButtonBar.User = SessionState.User;
            ChunkButtonBar.Culture = culture;
            ChunkButtonBar.Item = item;
            if (!Page.IsPostBack)
            {
                lbResult.Text = string.Empty;
                WebSpellChecker1.WebSpellCheckerDialogPage += "?i=" + itemId.ToString() + "&c=" + containerId.ToString();
                UpdateDataView();
            }
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
        }

        private void UpdateDataView()
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "blanktext", "<script>ILBText = '" + HyperCatalog.Business.Chunk.BlankText + "';</script>");

            txtValue.TextMode = TextBoxMode.MultiLine;
            txtValue.Rows = 5;
            if (container.DataType.InputType.ToLower() == "line")
            {
                txtValue.TextMode = TextBoxMode.SingleLine;
            }
            if (container.DataType.RegularExpression != string.Empty)
            {
                regValidate.ValidationExpression = container.DataType.RegularExpression;
                regValidate.Enabled = true;
            }

            if (chunk != null) // If chunk exists
            {
                if (chunk.Text == HyperCatalog.Business.Chunk.BlankValue)
                {
                    uwToolbar.Items.FromKeyButton("ilb").Selected = true;
                    txtValue.Enabled = false;
                    txtValue.Text = HyperCatalog.Business.Chunk.BlankText;
                    regValidate.Enabled = false; //ACQ10.0
                }
                else
                {
                    txtValue.Enabled = true;
                    txtValue.Text = chunk.Text;
                }
                imgStatus.ImageUrl = "/hc_v4/img/S" + HyperCatalog.Business.Chunk.GetStatusFromEnum(chunk.Status) + ".gif";
                lbStatus.Text = chunk.Status.ToString();
                ChunkModifier1.Visible = true;
                // Force culture to draft
                string masterLanguage = HyperCatalog.Shared.SessionState.MasterCulture.Code;
                if (culture.Code == masterLanguage)
                {
                    //Prabhu R S - 12/12/2007    
                    //Updated for HAA UI Impacts
                    //ContainerId 1 & 2 should not be editable in master catalogue
                    //Making ChunkValue as ReadOnly & Disabling Comment
                    if (((container.Id == 1) || (container.Id == 2)) && SessionState.CurrentItem.NodeOID.ToString() != null && SessionState.CurrentItem.NodeOID.ToString() != "-1")
                    {
                        txtValue.ReadOnly = true;
                        ChunkComment1.Visible = false;
                    }
                    if (Request["ht"] != null)
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "culture", "<script>enforceCulture = true;</script>");
                    }
                }
            }
            else // the chunk is not existing, hide delete button
            {
                imgStatus.ImageUrl = "/hc_v4/img/SM.gif";
                lbStatus.Text = " [Missing]";
                txtValue.Text = string.Empty;
                ChunkModifier1.Visible = false;
            }

            if (container.MaxLength > 0)
            {
                // *************************************************************************
                // If this chunk is supposed to respect a maximum length, add the javascript
                // *************************************************************************
                txtValue.MaxLength = container.MaxLength;
                txtValue.Attributes.Add("onkeyup", "TrackCount(this,'textcount'," + Convert.ToString(container.MaxLength) + ")");
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "limit", "setInterval('var txtId = document.getElementById(\"" + txtValue.ClientID + "\");TrackCount(txtId,\"textcount\"," + Convert.ToString(container.MaxLength) + ")',500);var respectMaxLen=true;", true);
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "limit", "setInterval('var txtId = document.getElementById(\"" + txtValue.ClientID + "\");CheckChanges(txtId)',500);", true);
            }
            // Test if chunk is empty. If yes, force display.
            if (txtValue.Text == HyperCatalog.Business.Chunk.BlankValue)
            {
                txtValue.Text = HyperCatalog.Business.Chunk.BlankValue;
                txtValue.Enabled = false;
                uwToolbar.Items.FromKeyButton("ilb").Pressed(true);
            }
            if (culture.Language.Rtl)
            {
                txtValue.CssClass = "hc_rtledit";
            }


        }

        private void SaveChunk(ChunkStatus status, bool lockTranslations)
        {
            string error = string.Empty;
            string Value = txtValue.Text;
            if (uwToolbar.Items.FromKeyButton("ilb").Selected)
            {
                Value = HyperCatalog.Business.Chunk.BlankValue;
            }
            //ACQ10.0 Starts
            if (Value.Length <= 0)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "<script>alert('Please, enter a value!');</script>");
                return;
            }
            //ACQ10.0 Ends

            if (chunk != null)
            // Test if user has made a change that allows database update
            {
                if (txtValue.Text != chunk.Text || ChunkComment1.Comment != chunk.Comment || status != chunk.Status)
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
                lbResult.Text = "<br/>Chunk saved!";
                lbResult.CssClass = "hc_success";
                if (Value == HyperCatalog.Business.Chunk.BlankValue)
                {
                    Value = HyperCatalog.Business.Chunk.BlankText;
                }
                if (!lockTranslations)
                {
                    chunk.ForceTranslationsTo(SessionState.User.Id, ChunkStatus.Draft);
                }
                SessionState.QDEChunk = chunk;
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "update", "<script>UpdateGrid('" + HyperCatalog.Business.Chunk.GetStatusFromEnum(chunk.Status) + "', '" + UITools.CleanJSString(Value) + "');</script>");
            }
            else
            {
                lbResult.Text = "<br/>Error: " + HyperCatalog.Business.Chunk.LastError;
                lbResult.CssClass = "hc_error";
            }
            lbResult.Visible = true;
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

        #region Chunk Event handlers
        private void Chunk_DraftClick(object sender, ChunkButtonBarEventArgs e)
        {
            //if (validateHTML())
            //{
            SaveChunk(ChunkStatus.Draft, e.LockTranslations);
            SessionState.QDEChunk = chunk;
            //}
            //UpdateDataView();        
        }

        private void Chunk_FinalClick(object sender, ChunkButtonBarEventArgs e)
        {
            //if (validateHTML())
            //{
            SaveChunk(ChunkStatus.Final, e.LockTranslations);
            SessionState.QDEChunk = chunk;
            //}
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
                txtValue.Text = c.Text;
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
        /*#ROR Starts 
        //Commented since we are removing the functionality 
        private void Chunk_RejectClick(object sender, ChunkButtonBarEventArgs e)
        {
          HyperCatalog.Business.Chunk c = HyperCatalog.Business.Chunk.GetByKey(itemId, containerId, culture.FallbackCode);
          if (c != null)
          {
            c.CultureCode = culture.Code;
            c.Status = ChunkStatus.Rejected;
            if (c.Save(SessionState.User.Id))
            {
              txtValue.Text = c.Text;
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
