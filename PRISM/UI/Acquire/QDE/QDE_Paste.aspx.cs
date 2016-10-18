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
using HyperCatalog.Shared;
using HyperCatalog.Shared.Defs;
using Infragistics.WebUI.UltraWebGrid;
#endregion

namespace HyperCatalog.UI.Acquire.QDE
{
	/// <summary>
	/// Description résumée de QDE_Paste.
	/// </summary>
	public partial class QDE_Paste : HCPage
	{
		#region Declarations

		private HyperCatalog.Business.Item item;
		//private System.Int64 itemId;
		#endregion

  
		protected void Page_Load(object sender, System.EventArgs e)
		{
			lbError.Visible = false;
      try
			{
				item = QDEUtils.GetItemIdFromRequest();
        cbTranslations.Visible = SessionState.Culture.Type != CultureType.Locale;
				if (item != null)
				{
					if (SessionState.User.HasItemInScope(item.Id) 
						&& SessionState.User.HasCapability(CapabilitiesEnum.EDIT_DELETE_DRAFT_CHUNKS)) 
					{
            if (!Page.IsPostBack)
						{
							uwToolbar.Items.FromKeyLabel("ItemName").Text = item.FullName;
							UpdateDataView();
						}
					}
					else
					{
						UITools.DenyAccess(DenyMode.Popup);
					}
				}
				else
				{
					UITools.DenyAccess(DenyMode.Popup);
				}
      }
      catch
      {
				UITools.JsCloseWin();
      }
    }

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
			this.Ultrawebtoolbar1.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.Ultrawebtoolbar1_ButtonClicked);

		}
		#endregion

    private void UpdateDataView()
    {
      using (ChunkList chunks = item.Chunks(SessionState.Clipboard.CultureCode))
      {       
        foreach (ClipboardItem clip in SessionState.Clipboard.Items)
        {
          string action = "<img src='/hc_v4/img/ed_newcontent.gif' title='New' border='0'>";
          foreach (Business.Chunk c in chunks)
          {
            if (c.ContainerId == clip.ContainerId)
            {
              action = "<img src='/hc_v4/img/ed_update.gif' title='Update' border='0'>";
              break;
            }
          }

          using (HyperCatalog.Business.Container currentContainer = HyperCatalog.Business.Container.GetByKey(clip.ContainerId))
          {
            if (currentContainer != null)
            {
              UltraGridRow newRow = new UltraGridRow(new object[] { currentContainer.Name, action });
              dg.Rows.Add(newRow);
            }
          }
        }
      }
    }

    private void Ultrawebtoolbar1_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      lbError.Text = string.Empty;
			lbError.Visible = false;
			dg.Visible = false;

      Item itemSource = Business.Item.GetByKey(SessionState.Clipboard.ItemId);
      int nbChunks  = 0;
      string comment = "Content pasted";
      if (itemSource != null)
        comment += " from " + itemSource.FullName + " (#" + itemSource.Id.ToString() + ")";
      foreach (ClipboardItem clip in SessionState.Clipboard.Items)
      {
        if (!Business.Chunk.DuplicateChunk(clip.ItemId, clip.ContainerId, clip.CultureCode, item.Id, SessionState.User.Id, comment, cbTranslations.Checked))
            lbError.Text = lbError.Text + Business.Chunk.LastError +"<br>";
          else
            nbChunks ++;
        /*Business.Chunk c = Business.Chunk.GetByKey(clip.ItemId, clip.ContainerId, clip.CultureCode);
        if (c!=null)
        {
					// Update item id, modify date and status (draft)
          c.ItemId = item.Id;
          c.ModifyDate = DateTime.UtcNow;
          c.Status = Business.ChunkStatus.Draft;

					// Update comment
          if (itemSource!=null)
            c.Comment = "Paste content from " + itemSource.FullName + " (#" + itemSource.Id.ToString() + ")";

					// Save current chunk
          if (!c.Save(SessionState.User.Id))
            lbError.Text = lbError.Text + Business.Chunk.LastError +"<br>";
          else
            nbChunks ++;
        }
         */
      }

      if (lbError.Text != string.Empty)
      {
				// Display error
        dg.Visible = false;
        lbError.Visible = true;
      }
      else
      {
				// Success
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"pasteok", "<script>alert('" + nbChunks.ToString() + " chunks pasted successfully');ReloadAndClose();</script>");
      }
    }
	}
}
