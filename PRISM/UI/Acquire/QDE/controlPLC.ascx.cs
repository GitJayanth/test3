namespace HyperCatalog.UI.Acquire.QDE
{
  #region uses
  using System;
  using System.Data;
  using System.Drawing;
  using System.Web;
  using System.Web.UI.WebControls;
  using System.Web.UI.HtmlControls;
  using HyperCatalog.Business;
  using HyperCatalog.Shared;
  using Infragistics.WebUI.UltraWebGrid;
  #endregion
  
	#region History
	// 22/06/2006: Add EndOfSupportDate in PLC constructor (Mickael CHARENSOL)
	#endregion


  /// <summary>
  /// Description résumée de controlPLC.
  /// </summary>

  public partial class controlPLC : System.Web.UI.UserControl
  {
    #region Declarations
    int rowCount = 0;
    string _LastError = string.Empty;
    PLCList _AddUpdatePLCList = new PLCList();
    PLCList _UnchangedPLCList = new PLCList();
    PLCList _DeletedPLCList = new PLCList();
    bool _Enabled = true;
    int _PLCUserId = HyperCatalog.Shared.SessionState.User.Id;

    string LongNamePID = SessionState.CacheParams["HeaderLongNameLive"].Value.ToString();
    string ShortNamePID = SessionState.CacheParams["HeaderShortNameLive"].Value.ToString();
    string LongNamePOD = SessionState.CacheParams["HeaderLongNameObsolete"].Value.ToString();
    string ShortNamePOD = SessionState.CacheParams["HeaderShortNameObsolete"].Value.ToString();
    string LongNameBlind = SessionState.CacheParams["HeaderLongNameBlind"].Value.ToString();
    string ShortNameBlind = SessionState.CacheParams["HeaderShortNameBlind"].Value.ToString();
    string LongNameAnnouncement = SessionState.CacheParams["HeaderLongNameAnnouncement"].Value.ToString();
    string ShortNameAnnouncement = SessionState.CacheParams["HeaderShortNameAnnouncement"].Value.ToString();
    string LongNameRemoval = SessionState.CacheParams["HeaderLongNameRemoval"].Value.ToString();
    string ShortNameRemoval = SessionState.CacheParams["HeaderShortNameRemoval"].Value.ToString();
    #endregion

    #region published properties of the control
    public HyperCatalog.Business.Item Item
    {
      set { ViewState["Item"] = value; LoadPLCGrid(); }
      get { return ViewState["Item"] == null ? null : (Item)ViewState["Item"]; }
    }
    public string LastError
    {
      get { return _LastError; }
    }
    public PLCList AddUpdatePLCList
    {
      get { return _AddUpdatePLCList; }
    }
    public PLCList UnchangedPLCList
    {
      get { return _UnchangedPLCList; }
    }
    public PLCList DeletedPLCList
    {
      get { return _DeletedPLCList; }
    }
    public bool Enabled
    {
      get { return _Enabled; }
      set 
      { 
        _Enabled = value;
        if (!_Enabled && dg != null)
        {
          foreach (UltraGridRow r in dg.Rows)
          {
            r.Cells.FromKey("Blind").AllowEditing = AllowEditing.No;
            r.Cells.FromKey("Blind").Style.CssClass = "ro";
            r.Cells.FromKey("PID").AllowEditing = AllowEditing.No;
            r.Cells.FromKey("PID").Style.CssClass = "ro";
            r.Cells.FromKey("POD").AllowEditing = AllowEditing.No;
            r.Cells.FromKey("POD").Style.CssClass = "ro";
            r.Cells.FromKey("Announcement").AllowEditing = AllowEditing.No;
            r.Cells.FromKey("Announcement").Style.CssClass = "ro";
            r.Cells.FromKey("Removal").AllowEditing = AllowEditing.No;
            r.Cells.FromKey("Removal").Style.CssClass = "ro";
          }
        }
      }
    }
    #endregion
    protected void Page_Load(object sender, System.EventArgs e)
    {
      System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
      ci.DateTimeFormat.ShortDatePattern = SessionState.User.FormatDate;
      ci.DateTimeFormat.LongDatePattern = SessionState.User.FormatDate;
      wd.CalendarLayout.Culture = ci;

      if (!Page.IsPostBack)
      {
        UITools.UpdatePLCGridHeader(dg);
      }
    }
    //Fix for QC# 7029 by Rekha Thomas. Added  OnUpdateCell event handler 
    //protected void dg_UpdateCell(object sender, EventArgs e)
    //{
    //}

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

    }
    #endregion
    public void LoadPLCGrid()
    {
      Trace.Write("Enter LoadPLCGrid");
      dg.Rows.Clear();
      rowCount = 0;

      using (PLCList plc = Item != null ? Item.PLCDates : null)
      {
        if (SessionState.Culture.Type == CultureType.Locale)
        {
          // Country
          using (CountryList countries = new CountryList())
          {
            countries.Add(SessionState.Culture.Country);
            AddCountrieRows(countries, plc, 0);
          }
        }
        else
        {
          // Region or master region
          using (HyperCatalog.Business.Region rootRegion = HyperCatalog.Business.Region.GetByShortCode(SessionState.Culture.CountryCode))
          {
            AddRegion(rootRegion, plc, 0);
          }
        }

        Utils.InitGridSort(ref dg, false);
        if (rowCount > 0)
        {
          if (rowCount == 1)
          {
            // Add empty row
            UltraGridRow newRow = new UltraGridRow(new object[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 
                                                                string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 
                                                                string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty});
            dg.Rows.Add(newRow);
            newRow.Cells.FromKey("Region").AllowEditing = AllowEditing.No;
            newRow.Cells.FromKey("Region").Style.CssClass = "ro";
            newRow.Cells.FromKey("Blind").AllowEditing = AllowEditing.No;
            newRow.Cells.FromKey("Blind").Style.CssClass = "ro";
            newRow.Cells.FromKey("PID").AllowEditing = AllowEditing.No;
            newRow.Cells.FromKey("PID").Style.CssClass = "ro";
            newRow.Cells.FromKey("POD").AllowEditing = AllowEditing.No;
            newRow.Cells.FromKey("POD").Style.CssClass = "ro";
            newRow.Cells.FromKey("Announcement").AllowEditing = AllowEditing.No;
            newRow.Cells.FromKey("Announcement").Style.CssClass = "ro";
            newRow.Cells.FromKey("Removal").AllowEditing = AllowEditing.No;
            newRow.Cells.FromKey("Removal").Style.CssClass = "ro";

            rowCount++;
          }

          int height = (rowCount + 1) * 20; // (nb rows + header)
          if (height < 300)
            dg.Height = Unit.Pixel(height);
        }
        dg.DisplayLayout.CellClickActionDefault = CellClickAction.Edit;
        dg.DisplayLayout.AllowSortingDefault = AllowSorting.No;
        dg.DisplayLayout.UseFixedHeaders = true;
        dg.Columns.FromKey("ApplyChilds").Header.Fixed = true;
        dg.Columns.FromKey("Region").Header.Fixed = true;
        //Commented for fix QC 7304: Crystal UAT - In the Product Life Cycle section Delete functionality still available
        //dg.Columns.FromKey("ApplyChilds").Hidden = !SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_PLC);
        dg.Columns.FromKey("ApplyChilds").Hidden = true;

        Trace.Write("Exit LoadPLCGrid");
      }
    }

    private void AddRegion(HyperCatalog.Business.Region region, PLCList plcDates, int offSet)
    {
      Trace.Write("Enter Add Region [" + region.Code + "]");
      //Fix for eZilla #70617 ( GEMSTONE: WW & NA PLC dates are not visible in UI) - Prabhu
      PLC p = new PLC();
      p.CountryCode = region.Code;
      #region identify if region is in Item PLC list
      if (plcDates != null)
      {
          foreach (PLC plc in plcDates)
          {
              if (plc.CountryCode == region.Code2)
              {
                  p = plc;
                  break;
              }
          }
      }
      #endregion
      //UltraGridRow newRow = new UltraGridRow(new object[] { "", region.Code + " - " + region.Name, "", null, null, null, null, null, "R", region.Code, region.ParentCode, offSet });
      UltraGridRow newRow = new UltraGridRow(new object[] { "", region.Code + " - " + region.Name, "", p.BlindDate, p.FullDate, p.ObsoleteDate, p.AnnouncementDate, p.RemovalDate, "R", region.Code, region.ParentCode, offSet, "", 
                                 p.BlindLocked, p.FullLocked, p.ObsoleteLocked, p.AnnouncementLocked, p.RemovalLocked, p.EndOfSupportLocked,
                                 p.DiscontinueLocked, p.EndOfLifeLocked, p.BlindDateType, p.FullDateType, p.ObsoleteDateType, p.AnnouncementDateType,
                                 p.RemovalDateType, p.EndOfSupportDateType, p.DiscontinueDateType, p.EndOfLifeDateType, p.Excluded, p.ExcludeModifierId, p.EndOfSupportDate, p.DiscontinueDate, p.EndOfLifeDate});
      //Fix for eZilla #70617 ( GEMSTONE: WW & NA PLC dates are not visible in UI) - Prabhu

      dg.Rows.Add(newRow);
      rowCount++;
      newRow.Cells.FromKey("Region").Style.Padding.Left = Unit.Parse((offSet * 5).ToString() + "px");
      newRow.Cells.FromKey("Region").Style.CssClass = "hc_region";
      newRow.DataKey = region.Code;

      #region Add extra button at the end of the row
      newRow.Cells.FromKey("ApplyChilds").Text = "<IMG title='Clear all sub countries' src='/hc_v4/img/recursive.gif' onclick='CSC(\"" + dg.ClientID + "\", " + newRow.Index.ToString() + ");' BORDER=0 VALIGN=MIDDLE BORDER=0 STYLE='cursor:hand'></IMG>";
      #endregion
      AddCountrieRows(region.Countries, plcDates, offSet + 1);
      foreach (HyperCatalog.Business.Region subRegion in region.SubRegions)
      {
        //Fix for eZilla #70617 ( GEMSTONE: WW & NA PLC dates are not visible in UI) - Prabhu
          if (subRegion.SubRegions.Count > 0 || subRegion.Countries.Count > 0 || Item.NodeOID > 0)
          {
              AddRegion(subRegion, plcDates, offSet + 1);
          }
      }
      Trace.Write("Exit Add Region [" + region.Code + "]");
    }

    private void AddCountrieRows(HyperCatalog.Business.CountryList countries, PLCList plcDates, int offSet)
    {
      if (countries == null) return;
      foreach (Country country in countries)
      {
        Trace.Write("  CountryCode = " + country.Code);
        UltraGridRow newRow;
        /*DateTime? blind=null, full=null, obsolete=null, announce=null, remove=null;
        bool blindLocked = false, fullLocked = false, obsoleteLocked = false, announceLocked = false, removeLocked = false;
         * */
        PLC p = new PLC();
        p.CountryCode = country.Code;
        #region identify if country is in Item PLC list
        if (plcDates != null)
        {
          foreach (PLC plc in plcDates)
          {
            if (plc.CountryCode == country.Code)
            {
              p = plc;
              break;
            }
          }
        }
        #endregion
        newRow = new UltraGridRow(new object[] { "", country.Code + " - " + country.Name, "", p.BlindDate, p.FullDate, p.ObsoleteDate, p.AnnouncementDate, p.RemovalDate, "C", country.Code, country.RegionCode, offSet, "", 
                                 p.BlindLocked, p.FullLocked, p.ObsoleteLocked, p.AnnouncementLocked, p.RemovalLocked, p.EndOfSupportLocked,
                                 p.DiscontinueLocked, p.EndOfLifeLocked, p.BlindDateType, p.FullDateType, p.ObsoleteDateType, p.AnnouncementDateType,
                                 p.RemovalDateType, p.EndOfSupportDateType, p.DiscontinueDateType, p.EndOfLifeDateType, p.Excluded, p.ExcludeModifierId, p.EndOfSupportDate, p.DiscontinueDate, p.EndOfLifeDate});
        dg.Rows.Add(newRow);
        #region formatting
        dg.Rows[dg.Rows.Count - 1].Cells.FromKey("Blind").Column.Format = SessionState.User.FormatDate;
        dg.Rows[dg.Rows.Count - 1].Cells.FromKey("PID").Column.Format = SessionState.User.FormatDate;
        dg.Rows[dg.Rows.Count - 1].Cells.FromKey("POD").Column.Format = SessionState.User.FormatDate;
        dg.Rows[dg.Rows.Count - 1].Cells.FromKey("Announcement").Column.Format = SessionState.User.FormatDate;
        dg.Rows[dg.Rows.Count - 1].Cells.FromKey("Removal").Column.Format = SessionState.User.FormatDate;
        dg.Rows[dg.Rows.Count - 1].Cells.FromKey("EndOfSupport").Column.Format = SessionState.User.FormatDate;
        dg.Rows[dg.Rows.Count - 1].Cells.FromKey("Discontinue").Column.Format = SessionState.User.FormatDate;
        dg.Rows[dg.Rows.Count - 1].Cells.FromKey("EndOfLife").Column.Format = SessionState.User.FormatDate;
        #endregion
        rowCount++;
        newRow.Cells.FromKey("Region").Text = "<img src='/hc_v4/img/flags/" + country.Code + ".gif' border=0 valign='middle'/>&nbsp;" + newRow.Cells.FromKey("Region").Text;
        newRow.Cells.FromKey("Region").Style.Padding.Left = Unit.Parse((offSet * 5).ToString() + "px");
        newRow.Cells.FromKey("Region").Style.CustomRules = "class='hc_country'";
        newRow.Cells.FromKey("ApplyChilds").Text = "<IMG title='Clear this line' src='/hc_v4/img/clear.gif' onclick='CC(\"" + dg.ClientID + "\", " + newRow.Index.ToString() + ");' BORDER=0 VALIGN=MIDDLE BORDER=0 STYLE='cursor:hand'></IMG>";
        // Consider Locks coming from country level 
        //Trace.Write(country.Code + " - " + country.Name, "FULL Locked = " + p.FullLocked.ToString());
        //newRow.Cells.FromKey("Blind").AllowEditing = (p.BlindLocked || p.IsMinimized || p.IsFrozen) ? AllowEditing.No : AllowEditing.Yes;
        //newRow.Cells.FromKey("Blind").Style.CssClass = (p.BlindLocked || p.IsMinimized || p.IsFrozen) ? "ro" : "";
        //newRow.Cells.FromKey("PID").AllowEditing = (p.FullLocked || p.IsMinimized || p.IsFrozen) ? AllowEditing.No : AllowEditing.Yes;
        //newRow.Cells.FromKey("PID").Style.CssClass = (p.FullLocked || p.IsMinimized || p.IsFrozen) ? "ro" : "";
        //newRow.Cells.FromKey("POD").AllowEditing = (p.ObsoleteLocked || p.IsMinimized) ? AllowEditing.No : AllowEditing.Yes;
        //newRow.Cells.FromKey("POD").Style.CssClass = (p.ObsoleteLocked || p.IsMinimized) ? "ro" : "";
        //newRow.Cells.FromKey("Announcement").AllowEditing = (p.AnnouncementLocked || p.IsMinimized || p.IsFrozen) ? AllowEditing.No : AllowEditing.Yes;
        //newRow.Cells.FromKey("Announcement").Style.CssClass = (p.AnnouncementLocked || p.IsMinimized || p.IsFrozen) ? "ro" : "";
        //newRow.Cells.FromKey("Removal").AllowEditing = (p.RemovalLocked || p.IsMinimized) ? AllowEditing.No : AllowEditing.Yes;
        //newRow.Cells.FromKey("Removal").Style.CssClass = (p.RemovalLocked || p.IsMinimized) ? "ro" : "";

        // Changes for CR 4516 - Prabhu
        Trace.Write(country.Code + " - " + country.Name, "FULL Locked = " + p.FullLocked.ToString());
        newRow.Cells.FromKey("Blind").AllowEditing = (p.BlindLocked) ? AllowEditing.No : AllowEditing.Yes;
        newRow.Cells.FromKey("Blind").Style.CssClass = (p.BlindLocked) ? "ro" : "";
        newRow.Cells.FromKey("PID").AllowEditing = (p.FullLocked) ? AllowEditing.No : AllowEditing.Yes;
        newRow.Cells.FromKey("PID").Style.CssClass = (p.FullLocked) ? "ro" : "";
        newRow.Cells.FromKey("POD").AllowEditing = (p.ObsoleteLocked) ? AllowEditing.No : AllowEditing.Yes;
        newRow.Cells.FromKey("POD").Style.CssClass = (p.ObsoleteLocked) ? "ro" : "";
        newRow.Cells.FromKey("Announcement").AllowEditing = (p.AnnouncementLocked) ? AllowEditing.No : AllowEditing.Yes;
        newRow.Cells.FromKey("Announcement").Style.CssClass = (p.AnnouncementLocked) ? "ro" : "";
        newRow.Cells.FromKey("Removal").AllowEditing = (p.RemovalLocked) ? AllowEditing.No : AllowEditing.Yes;
        newRow.Cells.FromKey("Removal").Style.CssClass = (p.RemovalLocked) ? "ro" : "";

          newRow.DataKey = country.Code;
      }
    }

    private string AddErrorToolTip(string description)
    {
      return "<A onmouseover='doTooltip(event,\"" + description + "\")' onmouseout='hideTip()' href='javascript://' border='0'><IMG class='middle' src='/hc_v4/img/ed_info.gif' border='0' valign='top'></A>";
    }

    public PLCErrorEnum ValidatePLC()
    {
      bool result = true;
      bool atLeastOnePLC = false;
      _AddUpdatePLCList.Clear();
      _UnchangedPLCList.Clear();
      foreach (UltraGridRow ur in dg.Rows)
      {
        foreach (UltraGridCell c in ur.Cells)
        {
          c.Style.BackColor = Color.Empty;
        }
        if (ur.Cells.FromKey("RegionType").Text == "C") //country
        {
          UltraGridCell resultCell = ur.Cells.FromKey("Err");
          resultCell.Text = string.Empty;
          DateTime? blind = (DateTime?)ur.Cells.FromKey("Blind").Value;
          DateTime? full = (DateTime?)ur.Cells.FromKey("PID").Value;
          DateTime? obsolete = (DateTime?)ur.Cells.FromKey("POD").Value;
          DateTime? announcement = (DateTime?)ur.Cells.FromKey("Announcement").Value;
          DateTime? removal = (DateTime?)ur.Cells.FromKey("Removal").Value;
          DateTime? endSupport = (DateTime?)ur.Cells.FromKey("EndOfSupport").Value;
          DateTime? discontinue = (DateTime?)ur.Cells.FromKey("Discontinue").Value;
          DateTime? endoflife = (DateTime?)ur.Cells.FromKey("EndOfLife").Value;
          if (blind.HasValue || full.HasValue || obsolete.HasValue || announcement.HasValue || removal.HasValue)
          {
            Trace.Write("Validating " + ur.Cells.FromKey("RegionCode").Text + " starts", "");
            atLeastOnePLC = true;

            int excludeModifierId = (ur.Cells.FromKey("ExcludeModifierId").Value) != null ? (int)(ur.Cells.FromKey("ExcludeModifierId").Value) : 0;
            bool bExcluded = Convert.ToBoolean(ur.Cells.FromKey("Excluded").Value);
            bool blindLocked = (bool)(ur.Cells.FromKey("BlindLocked").Value);
            char? blindDateType = (Char?)(ur.Cells.FromKey("BlindDateType").Value);
            bool fullLocked = (bool)(ur.Cells.FromKey("FullLocked").Value);
            char? fullDateType = (Char?)(ur.Cells.FromKey("FullDateType").Value);
            bool obsoleteLocked = (bool)(ur.Cells.FromKey("ObsoleteLocked").Value);
            char? obsoleteDateType = (Char?)(ur.Cells.FromKey("ObsoleteDateType").Value);
            bool announcementLocked = (bool)(ur.Cells.FromKey("AnnouncementLocked").Value);
            char? announcementDateType = (Char?)(ur.Cells.FromKey("AnnouncementDateType").Value);
            bool removalLocked = (bool)(ur.Cells.FromKey("RemovalLocked").Value);
            char? removalDateType = (Char?)(ur.Cells.FromKey("RemovalDateType").Value);
            bool endOfSupportLocked = (bool)(ur.Cells.FromKey("EndOfSupportLocked").Value);
            char? endOfSupportDateType = (Char?)(ur.Cells.FromKey("EndOfSupportDateType").Value);
            bool discontinueLocked = (bool)(ur.Cells.FromKey("DiscontinueLocked").Value);
            char? discontinueDateType = (Char?)(ur.Cells.FromKey("DiscontinueDateType").Value);
            bool endOfLifeLocked = (bool)(ur.Cells.FromKey("DiscontinueLocked").Value);
            char? endOfLifeDateType = (Char?)(ur.Cells.FromKey("DiscontinueDateType").Value);

            string m = string.Empty;
            if (ur.Cells.FromKey("Upd").Text == "u")
            {
              if (!full.HasValue)
              {
                m = LongNamePID + " (" + ShortNamePID + ") " + " cannot be null";
                Trace.Warn(m);
                resultCell.Text = AddErrorToolTip(m);
                ur.Cells.FromKey("PID").Style.BackColor = Color.Red;
                result = false;
                continue;
              }
              if (!obsolete.HasValue)
              {
                m = LongNamePOD + " (" + ShortNamePOD + ") " + " Date cannot be null";
                Trace.Warn(m);
                resultCell.Text = AddErrorToolTip(m);
                ur.Cells.FromKey("POD").Style.BackColor = Color.Red;
                result = false;
                continue;
              }
              if (blind >= full)
              {
                m = LongNamePID + " (" + ShortNamePID + ") " + " must occur after " + LongNameBlind + " (" + ShortNameBlind + ")";
                Trace.Warn(m);
                resultCell.Text = AddErrorToolTip(m);
                ur.Cells.FromKey("Blind").Style.BackColor = Color.Red;
                ur.Cells.FromKey("PID").Style.BackColor = Color.Red;
                result = false;
                continue;
              }
              if (full >= obsolete)
              {
                m = LongNamePOD + " (" + ShortNamePOD + ") " + " must occur after " + LongNamePID + " (" + ShortNamePID + ")";
                Trace.Warn(m);
                resultCell.Text = AddErrorToolTip(m);
                ur.Cells.FromKey("POD").Style.BackColor = Color.Red;
                ur.Cells.FromKey("PID").Style.BackColor = Color.Red;
                result = false;
                continue;
              }
              if (announcement >= obsolete)
              {
                m = LongNameAnnouncement + " (" + ShortNameAnnouncement + ") " + " must occur before " + LongNamePOD + " (" + ShortNamePOD + ")";
                Trace.Warn(m);
                resultCell.Text = AddErrorToolTip(m);
                ur.Cells.FromKey("Announcement").Style.BackColor = Color.Red;
                ur.Cells.FromKey("POD").Style.BackColor = Color.Red;
                result = false;
                continue;
              }
              if (removal <= announcement)
              {
                m = LongNameRemoval + " (" + ShortNameRemoval + ") " + " must occur after " + LongNameAnnouncement + " (" + ShortNameAnnouncement + ")";
                Trace.Warn(m);
                resultCell.Text = AddErrorToolTip(m);
                ur.Cells.FromKey("Removal").Style.BackColor = Color.Red;
                ur.Cells.FromKey("POD").Style.BackColor = Color.Red;
                result = false;
                continue;
              }


              if (result)
              {
                _AddUpdatePLCList.Add(new PLC(-1, ur.Cells.FromKey("RegionCode").Text,
                  blind, null, blindLocked, _PLCUserId, blindDateType,
                  full, null, fullLocked, _PLCUserId, fullDateType,
                  obsolete, null, obsoleteLocked, _PLCUserId, obsoleteDateType,
                  announcement, null, announcementLocked, _PLCUserId, announcementDateType,
                  removal, null, removalLocked, _PLCUserId, removalDateType,
                  bExcluded, null, excludeModifierId,
                  endSupport, null, endOfSupportLocked, _PLCUserId, endOfSupportDateType,
                  discontinue, null, discontinueLocked, _PLCUserId, discontinueDateType,
                  endoflife, null, endOfLifeLocked, _PLCUserId, endOfLifeDateType));
              }
            }
            else
            {
              _UnchangedPLCList.Add(new PLC(-1, ur.Cells.FromKey("RegionCode").Text,
                  blind, null, blindLocked, _PLCUserId, blindDateType,
                  full, null, fullLocked, _PLCUserId, fullDateType,
                  obsolete, null, obsoleteLocked, _PLCUserId, obsoleteDateType,
                  announcement, null, announcementLocked, _PLCUserId, announcementDateType,
                  removal, null, removalLocked, _PLCUserId, removalDateType,
                  bExcluded, null, excludeModifierId,
                  endSupport, null, endOfSupportLocked, _PLCUserId, endOfSupportDateType,
                  discontinue, null, discontinueLocked, _PLCUserId, discontinueDateType,
                  endoflife, null, endOfLifeLocked, _PLCUserId, endOfLifeDateType));
            }
          }
          else
          {
            if (ur.DataChanged == DataChanged.Modified || ur.Cells.FromKey("Upd").Text == "u")
            {
              _DeletedPLCList.Add(new PLC(-1, ur.Cells.FromKey("RegionCode").Text,
                null, null, false, _PLCUserId, (Char?)null,
                null, null, false, _PLCUserId, (Char?)null,
                null, null, false, _PLCUserId, (Char?)null,
                null, null, false, _PLCUserId, (Char?)null,
                null, null, false, _PLCUserId, (Char?)null,
                false, null, 0));
            }
          }
          Trace.Write("Validating " + ur.Cells.FromKey("RegionCode").Text + " Ends", "Data Changed= [" + ur.Cells.FromKey("Upd").Text + "] and result = " + result.ToString());
        }
      }
      if (result)
      {
        if (atLeastOnePLC) return PLCErrorEnum.None;
        return PLCErrorEnum.NoDates;
      }
      dg.Columns.FromKey("Err").ServerOnly = false;
      return PLCErrorEnum.CorruptedData;
    }

    private string AddToolTip(string description)
    {
      return "<A onmouseover='doTooltip(event,\"" + description + "\")' onmouseout='hideTip()' href='javascript://' border='0'><IMG class='middle' src='/hc_v4/img/ed_info.gif' border='0' valign='top'></A>";
    }
  }
}