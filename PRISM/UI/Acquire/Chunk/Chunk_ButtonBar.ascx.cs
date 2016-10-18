namespace HyperCatalog.UI.Acquire.Chunk
{
  #region Uses
  using System;
  using System.Data;
  using System.Data.SqlClient;
  using System.Drawing;
  using System.Web;
  using System.Web.UI.WebControls;
  using System.Web.UI.HtmlControls;
  using HyperCatalog.Business;
  using HyperCatalog.Shared;
  using HyperComponents.Data.dbAccess;
  #endregion

  public delegate void ChunkButtonBarClickEventHandler(object sender, ChunkButtonBarEventArgs e);

  /// <summary>
  /// Description résumée de Chunk_ButtonBar.
  /// </summary>
  public partial class Chunk_ButtonBar : System.Web.UI.UserControl
  {
    #region Declarations

    private HyperCatalog.Business.Chunk _Chunk = null;
    private HyperCatalog.Business.Container _Container = null;
    private HyperCatalog.Business.User _User = null;
    private HyperCatalog.Business.Item _Item = null;
    private HyperCatalog.Business.Culture _Culture = null;
    #endregion

    public HyperCatalog.Business.Chunk Chunk
    {
      get { return _Chunk; }
      set { _Chunk = value; }
    }
    public HyperCatalog.Business.Container Container
    {
      get { return _Container; }
      set { _Container = value; }
    }
    public HyperCatalog.Business.User User
    {
      get { return _User; }
      set { _User = value; }
    }
    public HyperCatalog.Business.Item Item
    {
      get { return _Item; }
      set { _Item = value; }
    }

    public HyperCatalog.Business.Culture Culture
    {
      get { return _Culture; }
      set { _Culture = value; }
    }

    protected void Page_Load(object sender, System.EventArgs e)
    {
      if (!Page.IsPostBack)
      {
        Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "save", "if (top.saveRowIndex){top.SaveCurrentRow();}", true);
        DrawToolBar();
      }
    }
    public void DrawToolBar()
    {
      /* Alternate for CR 5096(Removal of rejection functionality)--start
      panelSepCopy.Visible = panelSepReject.Visible = PanelSep.Visible = wBtDraft.Visible = wbtCopy.Visible = wBtFinal.Visible = wbtReject.Visible = wBtDelete.Visible = cbLockTranslations.Visible = false; */
      panelSepCopy.Visible = PanelSep.Visible = wBtDraft.Visible = wbtCopy.Visible = wBtFinal.Visible = wBtDelete.Visible = cbLockTranslations.Visible = false;
      // Alternate for CR 5096(Removal of rejection functionality)--end

      if (_User != null && _Container != null)
      {
        if (!_User.IsReadOnly)
        {
          if (_Container.ReadOnly)
          {
            lReadOnly.Visible = true;
          }
          else
          {
            lReadOnly.Visible = false;
            bool bIsItemInScope = Request["ui"] != null ;
            if (_Item != null && bIsItemInScope
              && _Item.IsCountrySpecific
              && _Culture.Type == CultureType.Locale
              && _Culture.Country.CanLocalizeContent
              && _User.HasCapability(CapabilitiesEnum.LOCALIZE_CHUNKS))
            {
              wBtFinal.Visible = true;
              wBtDraft.Visible = true;//Changed for build 85 upon business requirements
              if (_Chunk != null)
                wBtDelete.Visible = true;
            }
            else // No country specific
            {
              // Check if non regionalizable attribute for culture of type Regional
              if (!_Container.Regionalizable && _Culture.Type == CultureType.Regionale)
              {
                lNonRegionalizable.Visible = true;
              }
              else
              {
                lNonRegionalizable.Visible = false;
                if (bIsItemInScope)
                {
                  wBtDraft.Visible = _User.HasCapability(CapabilitiesEnum.EDIT_DELETE_FINAL_CHUNKS_MARKET_SEGMENT) || _User.HasCapability(CapabilitiesEnum.EDIT_DELETE_DRAFT_CHUNKS);
                  wBtFinal.Visible = PanelSep.Visible = _User.HasCapability(CapabilitiesEnum.EDIT_DELETE_FINAL_CHUNKS_MARKET_SEGMENT) || (_User.HasCapability(CapabilitiesEnum.LOCALIZE_CHUNKS) && (_Culture.Type == CultureType.Locale));
                  if (_Chunk != null)
                  {
                    wBtDelete.Visible = (_User.HasCapability(CapabilitiesEnum.EDIT_DELETE_DRAFT_CHUNKS) && _Chunk.Status == ChunkStatus.Draft) || _User.HasCapability(CapabilitiesEnum.EDIT_DELETE_FINAL_CHUNKS_MARKET_SEGMENT);
                  }
                  else
                  {
                    /* Alternate for CR 5096(Removal of rejection functionality)--start
                    panelSepCopy.Visible = wbtCopy.Visible = panelSepReject.Visible = wbtReject.Visible = Request["hf"] != null; // if the Master Chunk window is passing this parameter, it means that there is a fallabck on master that we can reject or copy
                     */
                      panelSepCopy.Visible = wbtCopy.Visible = Request["hf"] != null; // if the Master Chunk window is passing this parameter, it means that there is a fallabck on master that we can copy
                    // Alternate for CR 5096(Removal of rejection functionality)--end
                  }
                  cbLockTranslations.Visible = PanelSep.Visible = false;
                  if (Request["ht"]!=null && _Container.Translatable)
                  {
                    cbLockTranslations.Visible = PanelSep.Visible = true;
                  }

                  if (_Container.Id <= 3) // Cannot delete Item name or Sku or Master publishing Date
                  {
                    wBtDraft.Visible = false;
                    if (_Chunk != null && (_Chunk.CultureCode == SessionState.MasterCulture.Code || _Chunk.UserId==0)){ // Localized chunks can be deleted
                      wBtDelete.Visible = false;
                    }
                  }
                  /////////////////////////////////////////////////////////////
                  //Determine which kind of interface we should show
                  /////////////////////////////////////////////////////////////
                  switch (Convert.ToChar(_Container.DataTypeCode))
                  {
                    case 'D': goto case 'N';
                    /* 
                     * Fix for QC 2238: 01 Triage PRIS: List data type does not allow save as empty
                     * Fixed by Jothi & Prabhu
                     * Date: 28 Jan 09
                     * Release: PRISM 7.0.01
                     * Code Change History:
                     * case 'L': goto case 'N';
                    */
                    case 'L': goto case 'T';
                    case 'B': goto case 'N';
                    case 'N': cbLockTranslations.Visible = PanelSep.Visible = false;
                      break;
                    case 'P':
                      cbLockTranslations.Visible = wBtDraft.Visible = PanelSep.Visible = false;
                      break;
                    case 'T':
                      if (wBtFinal.Visible && _Culture.Type != CultureType.Locale)
                      {
                        wBtDraft.ClientSideEvents.Click = "wBtDraft_Click";
                        wBtFinal.ClientSideEvents.Click = "wBtFinal_Click";
                      }
                      break;
                  }
                  // IF containe type is Photo, override dataTyping
                  if (_Container.ContainerTypeCode == 'P' || _Container.ContainerTypeCode == 'L')
                  {
                    cbLockTranslations.Visible = wBtDraft.Visible = PanelSep.Visible = false;
                  }
                  // If the current culture is a Locale one (country language), we must look 
                  // at the country capabilities settings for editing
                  if (_Culture.Type == CultureType.Locale)
                  {
                    wBtDraft.Visible = false;
                    
                    //code added on 24th Nov 2011 for Chunk Edit Validation (XSS Vulnerability Fix) for Country Catalog
                    wBtFinal.ClientSideEvents.Click = "wBtFinal_Click1";

                    if (!_Culture.Country.CanLocalizeContent)
                    {
                      wBtDraft.Visible = wBtFinal.Visible = wBtDelete.Visible = cbLockTranslations.Visible = PanelSep.Visible = false;
                      lNonEditableForCountry.Visible = true;
                    }
                  }
                }
              }
            }
            //Prabhu R S - 12/12/2007    
            //Updated for HAA UI Impacts
            //ContainerId 1 & 2 should not be editable in master catalogue
            //Disabling all the buttons
              if (_Culture.Code == SessionState.MasterCulture.Code)
              {
                  if (((_Container.Id == 1) || (_Container.Id == 2)) && SessionState.CurrentItem.NodeOID.ToString() != null && SessionState.CurrentItem.NodeOID.ToString() != "-1")
                  {
                      wBtDraft.Visible = wBtFinal.Visible = wBtDelete.Visible = cbLockTranslations.Visible = PanelSep.Visible = false;
                  }
              }
          }
        }
      }
      // Bug 69000 fix
      txtInTranslation.Visible = false;
      if (_Culture.Type != CultureType.Locale){
        using (Database dbObj = Utils.GetMainDB())
        {
          using (IDataReader rs = dbObj.RunSPReturnRS("_Chunk_GetOnGoingTRs", new SqlParameter("@RegionCode", _Culture.Code),
                                                      new SqlParameter("@ItemId", _Item.Id),
                                                      new SqlParameter("@ContainerId", _Container.Id)))
          {
            if (dbObj.LastError == string.Empty)
            {
              if (rs.Read())
              {
                txtInTranslation.Text = "This chunk is part of [" + rs[1].ToString() + "] " + rs[0].ToString() + " created on " + SessionState.User.FormatUtcDate((DateTime)rs[2], SessionState.User.FormatDate);
                txtInTranslation.Visible = true;
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "intr", "var isInTR=true;", true);
              }
              else
              {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "intr", "var isInTR=false;", true);
              }
              rs.Close();
            }
            else
            {
              Page.ClientScript.RegisterStartupScript(Page.GetType(), "intr", "var isInTR=false;", true);
              txtInTranslation.Text = "Error while retrieving on-going translations: " + dbObj.LastError;
              txtInTranslation.Visible = true;
            }
          }
        }
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
    ///		Méthode requise pour la prise en charge du concepteur - ne modifiez pas
    ///		le contenu de cette méthode avec l'éditeur de code.
    /// </summary>
    private void InitializeComponent()
    {
      this.wBtDraft.Click += new Infragistics.WebUI.WebDataInput.ClickHandler(this.wBtDraft_Click);
      this.wBtFinal.Click += new Infragistics.WebUI.WebDataInput.ClickHandler(this.wBtFinal_Click);
      this.wBtDelete.Click += new Infragistics.WebUI.WebDataInput.ClickHandler(this.wBtDelete_Click);

    }
    #endregion

    public event ChunkButtonBarClickEventHandler DraftClick;
    public event ChunkButtonBarClickEventHandler FinalClick;
    public event ChunkButtonBarClickEventHandler DeleteClick;
    /* Alternate for CR 5096(Removal of rejection functionality)
    public event ChunkButtonBarClickEventHandler RejectClick; */
    public event ChunkButtonBarClickEventHandler CopyClick;

    private void wBtDraft_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
      if (DraftClick != null)
      {
        DraftClick(this, new ChunkButtonBarEventArgs(cbLockTranslations.Checked && cbLockTranslations.Visible));
        Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "reloadparent", "<script>if (top.document.forms){top.document.forms[0].submit()}</script>");
      }
    }

    private void wBtFinal_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
      if (FinalClick != null)
      {
        FinalClick(this, new ChunkButtonBarEventArgs(cbLockTranslations.Checked && cbLockTranslations.Visible));
        //DrawToolBar();
        Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "reloadparent", "<script>if (top.document.forms){top.document.forms[0].submit()}</script>");
      }
    }

    private void wBtDelete_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
      if (DeleteClick != null)
      {
        bool forceRefresh = false;
        int nbInheritance = 0;
        Chunk fallBackChunk = null;
        HyperCatalog.DataAccessLayer.SqlDataAccessLayer DBLayer = new HyperCatalog.DataAccessLayer.SqlDataAccessLayer();
        if (SessionState.Culture.Fallback == null && _Container.InheritanceMethodId != 0)
        {
          // Fixed
          using (DataSet ds = DBLayer.GetChunkHeritage(SessionState.User.Id, _Item.Id, _Container.Id, _Culture.Code))
          {
            nbInheritance = ds.Tables[0].Rows.Count;
          }
        }
        DeleteClick(this, new ChunkButtonBarEventArgs(false));
        if (SessionState.Culture.Fallback != null || nbInheritance > 0)
        {
          Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "reloadparentgrid", "<script>if (top){top.ReloadParent();}</script>");
        }
        else
        {
          Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "reloadparent", "<script>if (top.document.forms){top.document.forms[0].submit()}</script>");
        }
      }
    }
    protected void wbtCopy_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
      if (CopyClick != null)
      {
        CopyClick(sender, new ChunkButtonBarEventArgs(false));
      }
    }

    /* Alternate for CR 5096(Removal of rejection functionality)
    protected void wbtReject_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
      if (RejectClick != null)
      {
        RejectClick(sender, new ChunkButtonBarEventArgs(false));
        Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "reloadparentgrid", "<script>if (top){top.ReloadParent();}</script>");
      }
    } */

  }

}
