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
using System.Web.Caching;
//using NetSpell.SpellChecker.Dictionary;
using HyperCatalog.SpellChecker;
using HyperCatalog.Shared;
using HyperCatalog.Business;
using System.Configuration;
using HyperComponents.Data.dbAccess;
using System.Data.SqlClient;
using HyperComponents.WebUI.CustomSpellChecker;
using Infragistics.WebUI.UltraWebGrid;
#endregion

namespace HyperCatalog.UI.Acquire.QDE
{
	/// <summary>
	/// Spell checker return a report of node content (with or not children content)
	/// </summary>
	public partial class QDE_SpellChecker : HCPage
	{
		#region Declaration

		private HyperCatalog.Business.Culture culture = null;
		private HyperCatalog.Business.User user = null;
		private HyperCatalog.Business.Item item = null;
		private int inputformId = -1;
    string currentItem = string.Empty;
    int itemCount = 0;


		#endregion

		#region Code généré par le Concepteur Web Form
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN : Cet appel est requis par le Concepteur Web Form ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
		/// le contenu de cette méthode avec l'éditeur de code.
		/// </summary>
		private void InitializeComponent()
		{    
			this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);
			this.dg.InitializeRow += new Infragistics.WebUI.UltraWebGrid.InitializeRowEventHandler(this.dg_InitializeRow);

		}
		#endregion

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Get current culture
			culture = SessionState.Culture;
			user = SessionState.User;

			try
			{
				// Get parameters
  			item = QDEUtils.GetItemIdFromRequest();
        culture = QDEUtils.UpdateCultureCodeFromRequest();

				if (Request["f"] != null)
					inputformId = Convert.ToInt32(Request["f"]);

			}
			catch
			{
				UITools.DenyAccess(DenyMode.Popup);
				return;
			}

			// Check
			//	- user is valid
			//	- culture is valid
			//	- item is valid
			//	- user has the current culture in its scope
			//	- user has the item in its scope
			if (user != null && culture != null && item != null 
				&& user.HasCultureInScope(culture.Code) 
				&& user.HasItemInScope(item.Id))
			{
        Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "initVars", "var cultureCode='" + culture.Code + "';", true);
				if (!Page.IsPostBack)
				{
					lbError.Visible = false;
					dg.Visible = false;
					lbResult.Visible = false;

					// Update title (current item name)
					if (item != null)
					{
						lbTitle.Text = item.FullName;
						if (lbTitle.Text.Length > 50)
							lbTitle.Text = lbTitle.Text.Substring(0,49) + "...";

						if (inputformId > -1)
						{
							pnlChildren.Visible = false;
							UITools.HideToolBarSeparator(uwToolbar, "AnalyzeSep");
							UITools.HideToolBarButton(uwToolbar, "Analyze");

							Analyze(); // analyse the content for this input form

							// Retrieve input form name
							string inputFormName = HyperCatalog.Business.InputForm.GetByKey(Convert.ToInt32(inputformId)).Name;
							Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"InitFormName", "<script>inputFormName='"+inputFormName+"';</script>");
						}
						else
						{
							pnlChildren.Visible = true;
							UITools.ShowToolBarSeparator(uwToolbar, "AnalyzeSep");
							UITools.ShowToolBarButton(uwToolbar, "Analyze");
						}
					}
					else
					{
						UITools.DenyAccess(DenyMode.Popup);
					}
				}
			}
			else
			{
				UITools.DenyAccess(DenyMode.Popup);
			}
		}

		private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
		{
			string btn = be.Button.Key.ToLower();

			if (btn.Equals("analyze"))
			{
				Analyze();
			}
		}


    private void Analyze()
    {
      dg.Visible = false;
      lbResult.Visible = false;
      lbError.Visible = false;

      if (item != null)
      {
        try
        {
          DataSet ds = item.GetContent(culture.Code, inputformId, cbWithChildren.Checked);

          if (ds != null)
          {
            if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
            {
              // Add column containing the count of error
              ds.Tables[0].Columns.Add("Error", Type.GetType("System.String"));
              try
              {
                HyperComponents.WebUI.CustomSpellChecker.SpellChecker c = UITools.GetSpellChecker();
                #region debugging
/*                Trace.Warn("c.AllowCaseInsensitiveSuggestions =" + c.AllowCaseInsensitiveSuggestions.ToString());
                Trace.Warn("c.AllowMixedCase =" + c.AllowMixedCase.ToString());
                Trace.Warn("c.AllowWordsWithDigits =" + c.AllowWordsWithDigits.ToString());
                Trace.Warn("c.AllowXML =" + c.AllowXML.ToString());
                Trace.Warn("c.CheckHyphenatedText =" + c.CheckHyphenatedText.ToString());
                Trace.Warn("c.SetIncludeUserDictionaryInSuggestions= " + c.GetIncludeUserDictionaryInSuggestions().ToString());
                Trace.Warn("c.LanguageParser" + c.LanguageParser.ToString());
                Trace.Warn("c.SetAllowCapitalizedWords = " + c.GetAllowCapitalizedWords().ToString());
                Trace.Warn("c.CheckCompoundWords =" + c.CheckCompoundWords.ToString());
                Trace.Warn("c.SetConsiderationRange=" + c.GetConsiderationRange().ToString());
                Trace.Warn("c.SplitWordThreshold = " + c.SplitWordThreshold.ToString());
                Trace.Warn("c.SuggestSplitWords = " + c.SuggestSplitWords.ToString());
                Trace.Warn("c.SetSeparateHyphenWords=" + c.GetSeparateHyphenWords().ToString());
                Trace.Warn("c.SetSuggestionsMethod=" + c.GetSuggestionsMethod().ToString());
                Trace.Warn("c.userDictionary.dictFile=" + c.userDictionary.dictFile);
                */
                /*
                                                c.AllowCaseInsensitiveSuggestions = true;
                                                c.AllowMixedCase = false;
                                                c.AllowWordsWithDigits = true;
                                                c.AllowXML = true;
                                                c.CheckHyphenatedText = true;
                                                c.SetIncludeUserDictionaryInSuggestions(true);
                                                c.LanguageParser = LanguageType.English;
                                                c.SetAllowCapitalizedWords(true);
                                                c.CheckCompoundWords = false;
                                                c.SetConsiderationRange(-1);
                                                c.SplitWordThreshold = 3;
                                                c.SuggestSplitWords = true;
                                                c.SetSeparateHyphenWords(false);*/
                #endregion
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                  //spellTest.HasErrors(dr["ChunkValue"].ToString(), true);
                  Trace.Warn(dr["ChunkValue"].ToString());
                  dr["Error"] = UITools.TextHasErrors(ref c, dr["ChunkValue"].ToString());
                }

                dg.DataSource = ds.Tables[0].DefaultView;
                Utils.InitGridSort(ref dg, true);
                dg.DataBind();
                dg.DisplayLayout.AllowSortingDefault = Infragistics.WebUI.UltraWebGrid.AllowSorting.No;
                InitializeGridGrouping();
                //dg.DisplayLayout.Pager.AllowPaging = true;

                if (dg.Rows.Count > 0)
                {
                  dg.Visible = true;
                  lbResult.Visible = false;
                }
                else
                {
                  dg.Visible = false;
                  lbResult.Text = "No errors found.";
                  lbResult.Visible = true;
                }
              }
              catch (Exception e)
              {
                lbError.CssClass = "hc_error";
                lbError.Text = e.ToString();
                lbError.Visible = true;
              }
              finally
              {
                if (ds != null)
                  ds.Dispose();
              }
            }
            else // chunks count equals 0
            {
              lbResult.Visible = true;

              if (ds != null)
                ds.Dispose();
            }
          }
          else // ds is null
          {
            lbError.CssClass = "hc_error";
            lbError.Text = "DataSet is null";
            lbError.Visible = true;
          }
        }
        catch (DataException de)
        {
          lbError.CssClass = "hc_error";
          lbError.Text = de.ToString();
          lbError.Visible = true;
        }
      }
    }
    private void InitializeGridGrouping()
    {
      int i = 0;
      currentItem = string.Empty;
      if (dg.Rows.Count > 0)
      {
        int colIndexId = dg.Rows[i].Cells.FromKey("ItemId").Column.Index;
        int colIndexName = dg.Rows[i].Cells.FromKey("ItemName").Column.Index;
        int colIndexNumber = dg.Rows[i].Cells.FromKey("ItemNumber").Column.Index;
        while (i < dg.Rows.Count)
        {
          string rowItem = dg.Rows[i].Cells[colIndexId].Value.ToString();
          string itemName = dg.Rows[i].Cells[colIndexName].ToString();
          if (dg.Rows[i].Cells[colIndexNumber].Value != null)
          {
            itemName = "[" + dg.Rows[i].Cells[colIndexNumber].Value.ToString() + "] " + itemName;
          }
          if (i == 0 || currentItem != rowItem)
          {
            currentItem = rowItem;
            dg.Rows.Insert(i, new UltraGridRow());
            UltraGridRow itemRow = dg.Rows[i];
            UltraGridCell itemCellMax = itemRow.Cells[dg.Columns.Count - 1]; // initialize all cells for this row
            foreach (UltraGridCell cell in itemRow.Cells)
            {
              cell.Style.CssClass = string.Empty;
            }
            dg.Rows[i].Style.CssClass = "ptbgroup";
            UltraGridCell itemCell = itemRow.Cells[10];
            itemCell.ColSpan = 4;
            itemCell.Text = itemName;
            i++;
          }
          i++;
        }
      }
    }

		private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
		{
			System.Int64 itemId = Convert.ToInt64(e.Row.Cells.FromKey("ItemId").Value);
			int containerId = Convert.ToInt32(e.Row.Cells.FromKey("ContainerId").Value);

			bool keep = true;

			// Error field
			e.Row.Cells.FromKey("Error").Style.HorizontalAlign = HorizontalAlign.Center;
			if (e.Row.Cells.FromKey("Error").Text == "0")
			{
				e.Row.Delete();
				keep = false;
			}
			else
			{
				if (e.Row.Cells.FromKey("Error").Text == "1")
					e.Row.Cells.FromKey("Error").Text = e.Row.Cells.FromKey("Error").Text+" error";
				else
					e.Row.Cells.FromKey("Error").Text = e.Row.Cells.FromKey("Error").Text+" errors";
				e.Row.Cells.FromKey("Error").Style.CssClass = "hc_error";
			}

			if (keep)
			{
				// Index
				e.Row.Cells.FromKey("Index").Value = e.Row.Index;

				// Read only container
				if (Convert.ToBoolean(e.Row.Cells.FromKey("ReadOnly").Value))
				{
					e.Row.Cells.FromKey("ContainerName").Text = e.Row.Cells.FromKey("ContainerName").Text + " <img src='/hc_v4/img/ed_glasses.gif'/>";
				}

				//If RTL languages, ensure correct display
				if ((bool)e.Row.Cells.FromKey("Rtl").Value)
					e.Row.Cells.FromKey("Value").Style.CustomRules = "direction: rtl;";//unicode-bidi:bidi-override;";
        string rowItem = e.Row.Cells.FromKey("ItemId").Text;
        if (currentItem != rowItem)
        {
          currentItem = rowItem;
          itemCount++;
        }
        int index = e.Row.Index + itemCount;
				//Display Edit Link in Container Name
				e.Row.Cells.FromKey("ContainerName").Text = "<a href='javascript://' onclick=\"ed("+index+", " + e.Row.Cells.FromKey("ItemId").ToString() +")\">"+e.Row.Cells.FromKey("ContainerName").Text+"</a>";
				
				//Display Status logo
				if (e.Row.Cells.FromKey("Status").Text != null)
				{
					e.Row.Cells.FromKey("Status").Style.CssClass = "S" + e.Row.Cells.FromKey("Status").Text;
					e.Row.Cells.FromKey("Status").Value = string.Empty;
				}
				
				// BLANK Value is replace by Readable sentence
        if (e.Row.Cells.FromKey("Value").Text == HyperCatalog.Business.Chunk.BlankValue)
        {
          e.Row.Cells.FromKey("Value").Text = HyperCatalog.Business.Chunk.BlankText;
          e.Row.Cells.FromKey("Value").Style.CustomRules = string.Empty;
        }
        else
        {
          e.Row.Cells.FromKey("Value").Text = UITools.HtmlEncode(e.Row.Cells.FromKey("Value").Text);
        }
			}
		}
	}
}
