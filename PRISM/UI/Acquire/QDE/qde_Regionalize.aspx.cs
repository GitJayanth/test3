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
using System.Xml;

namespace HyperCatalog.UI.Acquire.QDE
{
	/// <summary>
	/// This class allows duplicate or set to blank several chunks 
	/// </summary>
	public partial class QDE_Regionalize : HCPage
	{
		#region Declarations

		private Item _Item = null;
		private int _InputformId = -1;
		private HyperCatalog.Business.Culture _FallBackCulture, _FallBackCulture2, _FallBackCulture3;
		private string currentGroup = string.Empty;
		private int groupCount = 0;
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
			// Init variables
			_Item = null;
			_InputformId = -1;

			try
			{
				// retrieve item and input form (if inputform is -1 then it is content tab)
				_Item = QDEUtils.GetItemIdFromRequest();
				if (Request["f"] != null)
					_InputformId = Convert.ToInt32(Request["f"]);
        QDEUtils.UpdateCultureCodeFromRequest();
      }
			catch {}

			// Update fallback
			if (SessionState.Culture.Type == CultureType.Regionale)
			{
				_FallBackCulture = SessionState.Culture.Fallback;
			}
			if (SessionState.Culture.Type == CultureType.Locale)
			{
				_FallBackCulture = SessionState.Culture.Fallback;
				_FallBackCulture2 = _FallBackCulture.Fallback;
			}

			// Update grid
			if (_Item != null)
			{
				if (SessionState.User.HasItemInScope(_Item.Id)) // item is in user's scope
				{
					if (!Page.IsPostBack)
						UpdateDataView();
				}
				else
					UITools.DenyAccess(DenyMode.Popup);
			}
			else
				UITools.DenyAccess(DenyMode.Popup);
		}

		private void UpdateDataView()
		{
			lbError.Visible = false;

			if (SessionState.User.HasCapability(CapabilitiesEnum.EDIT_DELETE_DRAFT_CHUNKS))
			{
				UITools.ShowToolBarSeparator(uwToolbar, "DuplicateSep");
				UITools.ShowToolBarButton(uwToolbar, "Duplicate");
                /* Alternate for CR 5096(Removal of rejection functionality)--start
                UITools.ShowToolBarSeparator(uwToolbar, "RejectSep");
                UITools.ShowToolBarButton(uwToolbar, "Reject"); */
                UITools.ShowToolBarSeparator(uwToolbar, "ilbSep");
                UITools.ShowToolBarButton(uwToolbar, "ilb");
                // Alternate for CR 5096(Removal of rejection functionality)--end
			}
			else
			{
				UITools.HideToolBarSeparator(uwToolbar, "DuplicateSep");
				UITools.HideToolBarButton(uwToolbar, "Duplicate");
                /* Alternate for CR 5096(Removal of rejection functionality)--start
                UITools.HideToolBarSeparator(uwToolbar, "RejectSep");
                UITools.HideToolBarButton(uwToolbar, "Reject"); */
                UITools.HideToolBarSeparator(uwToolbar, "ilbSep");
                UITools.HideToolBarButton(uwToolbar, "ilb");
                // Alternate for CR 5096(Removal of rejection functionality)--end

			}

			if (_Item != null)
			{
				InputFormChunkList chunks = InputFormChunk.GetByInputForm(_Item.Id, _InputformId, SessionState.Culture.Code);
				InputFormChunkList chunksList = new InputFormChunkList();
				chunksList = UpdateCandidate(chunks);
				if (chunksList.Count > 0)
				{
					dg.DataSource = chunksList;
					Utils.InitGridSort(ref dg, false);
					dg.DataBind();
					dg.DisplayLayout.AllowSortingDefault = AllowSorting.No;
					dg.DisplayLayout.Pager.AllowPaging = false;
					InitializeGridGrouping();

					dg.Visible = true;
					lbResult.Visible = false;
				}
				else
				{
					dg.Visible = false;
					lbResult.Visible = true;
				}
			}
		}

		private InputFormChunkList UpdateCandidate(InputFormChunkList chunks)
		{
			InputFormChunkList newList = new InputFormChunkList();
			foreach (InputFormChunk ifc in chunks)
			{
        if (ifc.Regionalizable && ifc.Status == ChunkStatus.Final && ifc.CultureCode == HyperCatalog.Shared.SessionState.MasterCulture.Code)
				{
          if (!ifc.Text.Equals(HyperCatalog.Business.Chunk.BlankValue))
					  newList.Add(ifc);
				}
			}
			return newList;
		}

    private void Duplicate(HyperCatalog.Business.ChunkStatus status)
		{
			if (dg != null)
			{
				string sChunks = string.Empty;
        string containerList = string.Empty;
				bool isSelected = false;

				TemplatedColumn col = (TemplatedColumn)dg.Columns.FromKey("Select");
				((CheckBox)(col.HeaderItem.FindControl("g_ca"))).Checked = false;

				int currentIndex = -1;
				for (int i=0;i<col.CellItems.Count;i++)
				{
					currentIndex = ((CellItem)col.CellItems[i]).Cell.Row.Index;
					CheckBox cb = (CheckBox)((CellItem)col.CellItems[i]).FindControl("g_sd");
					if (cb.Checked)
					{
            if (sChunks.Length > 0)
            {
              sChunks += "|";
              containerList += ",";
            }
						sChunks += dg.Rows[currentIndex].Cells.FromKey("ItemId").Value.ToString();
						sChunks += ",";
						sChunks += dg.Rows[currentIndex].Cells.FromKey("ContainerId").Value.ToString();
						sChunks += ",";
						sChunks += "'"+dg.Rows[currentIndex].Cells.FromKey("SourceCode").Value.ToString()+"'";

            containerList += dg.Rows[currentIndex].Cells.FromKey("ContainerId").Value.ToString();
					}
				}

				if (sChunks.Length > 0)
				{
					if (!_Item.Regionalize(SessionState.Culture.Code, SessionState.User.Id, sChunks, status))
					{
						lbError.Visible = false;
						lbError.Text = Item.LastError;
						lbError.Visible = true;
					}
					else
					{
            if (containerList.Length > 0)
            {
              // Force translation to draft
              if (!_Item.ForceTranslationsToDraft(SessionState.Culture.Code, containerList, SessionState.User.Id))
              {
                lbError.Visible = false;
                lbError.Text = Item.LastError;
                lbError.Visible = true;
                return;
              }
            }
						Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"Update grid", "<script>UpdateGrid("+_Item.Id.ToString()+","+_InputformId.ToString()+",\""+SessionState.Culture.Code+"\");</script>");
					}
				}
				UpdateDataView();
			}
		}


		private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
		{
			string btnKey = be.Button.Key.ToLower();
			if (btnKey.Equals("duplicate"))
			{
				Duplicate(HyperCatalog.Business.ChunkStatus.Draft);
			}
            /* Alternate for CR 5096(Removal of rejection functionality)--start
			else if (btnKey.Equals("reject"))
			{
                Duplicate(HyperCatalog.Business.ChunkStatus.Rejected);
			} */
            else if (btnKey.Equals("ilb"))
            {
                Duplicate(HyperCatalog.Business.ChunkStatus.Final);
            }

		}

		private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
		{
			// Init parameters
			bool isMandatory = false; 
			bool isInherited = false;
			bool isResource = false;
			bool isBoolean = false; 
			bool isRegionalizable = false; 
			bool rtl = false;
			string sourceCode = string.Empty;
			string containerGroup = string.Empty;
			string currentChunkValue = string.Empty;
			ChunkStatus currentChunkStatus = ChunkStatus.Missing;
			System.Int64 currentItemId = -1;

			// Retrieve parameters
			if (e.Row.Cells.FromKey("IsMandatory") != null && e.Row.Cells.FromKey("IsMandatory").Value != null)
				isMandatory = Convert.ToBoolean(e.Row.Cells.FromKey("IsMandatory").Value);
			if (e.Row.Cells.FromKey("Inherited") != null && e.Row.Cells.FromKey("Inherited").Value != null)
				isInherited = Convert.ToBoolean(e.Row.Cells.FromKey("Inherited").Value);
			if (e.Row.Cells.FromKey("IsResource") != null && e.Row.Cells.FromKey("IsResource").Value != null)
				isResource = Convert.ToBoolean(e.Row.Cells.FromKey("IsResource").Value);
			if (e.Row.Cells.FromKey("IsBoolean") != null && e.Row.Cells.FromKey("IsBoolean").Value != null)
				isBoolean = Convert.ToBoolean(e.Row.Cells.FromKey("IsBoolean").Value);
			if (e.Row.Cells.FromKey("SourceCode") != null && e.Row.Cells.FromKey("SourceCode").Value != null)
				sourceCode = e.Row.Cells.FromKey("SourceCode").Value.ToString();
			if (e.Row.Cells.FromKey("Status") != null && e.Row.Cells.FromKey("Status").Value != null)
				currentChunkStatus = (ChunkStatus)Enum.Parse(typeof(ChunkStatus),e.Row.Cells.FromKey("Status").Value.ToString());
			if (e.Row.Cells.FromKey("Rtl") != null && e.Row.Cells.FromKey("Rtl").Value != null)
				rtl = Convert.ToBoolean(e.Row.Cells.FromKey("Rtl").Value);
			if (e.Row.Cells.FromKey("Path") != null && e.Row.Cells.FromKey("Path").Value != null)
				containerGroup = e.Row.Cells.FromKey("Path").Value.ToString();
			if (e.Row.Cells.FromKey("ItemId") != null && e.Row.Cells.FromKey("ItemId").Value != null)
				currentItemId = Convert.ToInt64(e.Row.Cells.FromKey("ItemId").Value);
			if (e.Row.Cells.FromKey("Value") != null && e.Row.Cells.FromKey("Value").Value != null)
				currentChunkValue = e.Row.Cells.FromKey("Value").Value.ToString();;
			
			//If RTL languages, ensure correct display        
			if (rtl)
				e.Row.Cells.FromKey("Value").Style.CustomRules = "direction: rtl;";//unicode-bidi:bidi-override;";

			// Update CultureCode column
			e.Row.Cells.FromKey("CultureCode").Value = SessionState.Culture.Code;

			// Container group
			if (currentGroup != containerGroup)
			{
				currentGroup = containerGroup;
				groupCount++;
			}

			//Display Mandatory logo
			e.Row.Cells.FromKey("Mandatory").Style.CssClass = "ptb1";
			e.Row.Cells.FromKey("Mandatory").Text = string.Empty; // by default
			if (isMandatory)
				e.Row.Cells.FromKey("Mandatory").Style.CssClass = "SCM"; // Status Chunk Mandatory

			//Display Status logo
			e.Row.Cells.FromKey("Status").Style.CssClass = "S"+HyperCatalog.Business.Chunk.GetStatusFromEnum(currentChunkStatus);
			e.Row.Cells.FromKey("Status").Value = string.Empty;

			// Check if value is inherited
			e.Row.Cells.FromKey("Value").Style.CssClass= "ptb3"; // by default
			e.Row.Cells.FromKey("Value").Style.Wrap = true; // by default
			if (isInherited)
			{
				e.Row.Cells.FromKey("Value").Style.CssClass= "overw";
				e.Row.Cells.FromKey("Value").Style.Wrap = true;
			}

			// Ensure multiline is kept
			if (currentChunkValue.Length > 0)
			{
				// if chunk is resource, try do display it
				if (isResource)
				{
					try
					{
						// Call HyperPublisher WebMethod to convert URL to absolute URL
						XmlDocument xmlInfo = new XmlDocument();
            xmlInfo.LoadXml(HCPage.WSDam.ResourceGetByPath(currentChunkValue));
						System.Xml.XmlNode node = xmlInfo.DocumentElement;
						string fullPath =  node.Attributes["uri"].InnerText;
						if (fullPath.ToLower().IndexOf("notfound") > 0 || fullPath == string.Empty)
							fullPath = "/hc_v4/img/ed_notfound.gif";

            e.Row.Cells.FromKey("Value").Text = "<img src='" + fullPath + "?thumbnail=1&size=40' title='" + currentChunkValue + "' border=0/>";
					}
					catch (Exception ex)
					{
						Trace.Warn("DAM", "Exception processing DAM: " + ex.Message);
					}
				}
				else if (currentChunkValue == HyperCatalog.Business.Chunk.BlankValue) // BLANK Value is replace by Readable sentence
				{
					e.Row.Cells.FromKey("Value").Text = HyperCatalog.Business.Chunk.BlankText;
					e.Row.Cells.FromKey("Value").Style.CustomRules = string.Empty;
				}
				else if (isBoolean && currentChunkValue.Length > 0) // Boolean
				{
					try
					{
						if (Convert.ToBoolean(currentChunkValue))
							e.Row.Cells.FromKey("Value").Text = "Yes";
						else
							e.Row.Cells.FromKey("Value").Text = "No";
					}
					catch {} // Value is not boolean!
				}
						

				// Display Fallback on cultures if current Culture <> "master"
				string sourceCultureCode = e.Row.Cells.FromKey("SourceCode").Text;
				if (sourceCultureCode != SessionState.Culture.Code)
				{
					if (_FallBackCulture != null && _FallBackCulture.Code == sourceCultureCode) // apply fallback one time
						e.Row.Cells.FromKey("Value").Style.CustomRules = e.Row.Cells.FromKey("Value").Style.CustomRules + "color:blue;font:italic;";
					else if (_FallBackCulture2 != null && _FallBackCulture2.Code == sourceCultureCode) // apply fallback two times
						e.Row.Cells.FromKey("Value").Style.CustomRules = e.Row.Cells.FromKey("Value").Style.CustomRules + "color:red;font:italic;";
					else if (_FallBackCulture3 != null && _FallBackCulture3.Code == sourceCultureCode) // apply fallback three times
						e.Row.Cells.FromKey("Value").Style.CustomRules = e.Row.Cells.FromKey("Value").Style.CustomRules + "color:green;font:italic;";
				}
            }
            TemplatedColumn col = (TemplatedColumn)dg.Columns.FromKey("Select");
            CheckBox c = (CheckBox)((CellItem)col.CellItems[e.Row.Index]).FindControl("g_sd");
            c.Enabled = true;
		}

		private void InitializeGridGrouping()
		{
            int i = 0;
            int x = dg.Rows.Count;
            TemplatedColumn col = (TemplatedColumn)dg.Columns.FromKey("Select");
			groupCount = 0;
			while (i < dg.Rows.Count)
			{        
				string containerGroup = dg.Rows[i].Cells.FromKey("Path").Value.ToString();
				if (i==0 || currentGroup != containerGroup)
				{
					currentGroup = containerGroup;
					dg.Rows.Insert(i, new UltraGridRow());
                    CheckBox c = (CheckBox)((CellItem)col.CellItems[x]).FindControl("g_sd");
                    c.Visible = false;
                    Label l = (Label)((CellItem)col.CellItems[x]).FindControl("grp_lbl");
                    l.Visible = true;
                    l.Text = currentGroup;
                    l.CssClass = "ptbgroup";
                    l.BorderStyle = BorderStyle.None;
                    UltraGridRow groupRow = dg.Rows[i];
                    UltraGridCell groupCellMax = groupRow.Cells[dg.Columns.Count - 1];
					foreach (UltraGridCell cell in groupRow.Cells)
					{
						cell.Style.CssClass = string.Empty;
					}
                    dg.Rows[i].Style.CssClass = "ptbgroup";
                    UltraGridCell groupCell = groupRow.Cells.FromKey("Select");
					groupCell.Text = containerGroup;
					groupCell.ColSpan = dg.Columns.Count - 1 - groupCell.Column.Index;
					i++;
                    x++;
				}
				i++;
			}
		}
	}
}
