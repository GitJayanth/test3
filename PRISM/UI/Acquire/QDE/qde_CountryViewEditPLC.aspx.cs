#region Uses
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using HyperCatalog.Business;
using HyperCatalog.Shared;
using HyperComponents.Data.dbAccess;
using System.Data.SqlClient;
#endregion

namespace HyperCatalog.UI.Acquire.QDE
{
    /// <summary>
    /// Edit PLC for an item and a country
    /// </summary>
  public partial class qde_CountryViewEditPLC : HCPage
  {
    #region Declarations
    private System.Int64 itemId;
    private string curCountryCode = string.Empty;
    private string curLanguageCode = string.Empty;
    private string curSku = string.Empty;
    private int curItemType = 0;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
      if (SessionState.User.IsReadOnly)
      {
        uwToolbar.Items.FromKeyButton("Save").Enabled =
          wdcAnnouncementDate.Enabled = wdcBlindDate.Enabled = wdcPID.Enabled = wdcObsoleteDate.Enabled =
          wdcRemovalDate.Enabled = cbAnnL.Enabled = cbBlindL.Enabled = cbObsoL.Enabled =
          cbPIDL.Enabled = cbRemovL.Enabled = cboxPID.Enabled = cboxRemovalDate.Enabled =
          cboxBlindDate.Enabled = cboxAnnouncementDate.Enabled = cboxObsoleteDate.Enabled = false;
      }

      Culture c=null;
      try
      {
        using (Item item = QDEUtils.GetItemIdFromRequest())
        {
          if (!SessionState.User.HasItemInScope(item.Id))
          {
            UITools.HideToolBarButton(uwToolbar, "Save");
            UITools.HideToolBarSeparator(uwToolbar, "SaveSep");
          }
          else
          {
            UITools.ShowToolBarButton(uwToolbar, "Save");
            UITools.ShowToolBarSeparator(uwToolbar, "SaveSep"); 
          }

          itemId = item.Id;
          curSku = item.Sku;
          curItemType = item.TypeId; 
          int PLCUserId = PLC.PLCUser(itemId);
          if (Request["co"] != null)
          {
            // Country code is passed directly
            using (CultureList cul = HyperCatalog.Business.Culture.GetAll("CountryCode = '" + Request["co"].ToString() + "'"))
            {
              if (cul.Count > 0)
              {
                c = cul[0];
              }
              else
              {
                UITools.FrameAlertAndBack("Invalid Country Code [" + Request["co"].ToString() + "'");
              }
            }
          }
          else
          {
            c = QDEUtils.UpdateCultureCodeFromRequest();
          }
          curLanguageCode = c.LanguageCode;
          curCountryCode = c.CountryCode; 
          if (Request["dg"] != null)
          {
            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "InitVarNames",
              "<script>dg = '" + Request["dg"].ToString() + "';" + Environment.NewLine +
              "col = " + Request["col"].ToString() + ";" + Environment.NewLine +
              "row = " + Request["row"].ToString() + ";</script>");
          }
          Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "InitVarNames2",
            "<script>var cbPIDL = '" + cbPIDL.ClientID + "';" + Environment.NewLine +
            "var cbObsoL = '" + cbObsoL.ClientID + "';" + Environment.NewLine +
            "var cbBlindL = '" + cbBlindL.ClientID + "';" + Environment.NewLine +
            "var cbAnnL = '" + cbAnnL.ClientID + "';" + Environment.NewLine +
            "var cbRemovL = '" + cbRemovL.ClientID + "';" + Environment.NewLine +
            "var cboxPID = '" + cboxPID.ClientID + "';" + Environment.NewLine +
            "var cboxObsoleteDate = '" + cboxObsoleteDate.ClientID + "';" + Environment.NewLine +
            "var cboxBlindDate = '" + cboxBlindDate.ClientID + "';" + Environment.NewLine +
            "var cboxAnnouncementDate = '" + cboxAnnouncementDate.ClientID + "';" + Environment.NewLine +
            "var cboxRemovalDate = '" + cboxRemovalDate.ClientID + "';" + Environment.NewLine +
            "var wdcObsoleteDate = '" + wdcObsoleteDate.ClientID + "';" + Environment.NewLine +
            "var wdcPID = '" + wdcPID.ClientID + "';" + Environment.NewLine +
            "var wdcRemovalDate = '" + wdcRemovalDate.ClientID + "';" + Environment.NewLine +
            "var wdcBlindDate = '" + wdcBlindDate.ClientID + "';" + Environment.NewLine +
            "var wdcAnnouncementDate = '" + wdcAnnouncementDate.ClientID + "';" + Environment.NewLine +
              "</script>");
          if (c.CountryCode != string.Empty)
          {
            uwToolbarTitle.Items.FromKeyLabel("Culture").Image = "/hc_v4/img/flags/" + curCountryCode + ".gif";
          }
          uwToolbarTitle.Items.FromKeyLabel("Culture").Text = c.Country.Name + "&nbsp;";          
          uwToolbarTitle.Items.FromKeyLabel("Sku").Text = "[" + item.Sku + "]";

          if (!Page.IsPostBack)
          {
            RetrievePLC();
            lError.Visible = false;
          }
            //Modified for CR 4516/4537 - Prabhu
          //if (!c.Country.CanLocalizePLC || item.IsMinimizedByCulture(SessionState.Culture.Code) || item.IsRoll || PLCUserId < 1)
          if (!c.Country.CanLocalizePLC || item.IsRoll || PLCUserId < 1)
          {
            UITools.HideToolBarButton(uwToolbar, "Save");
            UITools.HideToolBarSeparator(uwToolbar, "SaveSep");
            wdcAnnouncementDate.Enabled = wdcBlindDate.Enabled = wdcPID.Enabled = wdcObsoleteDate.Enabled =
              wdcRemovalDate.Enabled = cbAnnL.Enabled = cbBlindL.Enabled = cbObsoL.Enabled =
              cbPIDL.Enabled = cbRemovL.Enabled = cboxPID.Enabled = cboxRemovalDate.Enabled =
              cboxBlindDate.Enabled = cboxAnnouncementDate.Enabled = cboxObsoleteDate.Enabled = false;

            if (!c.Country.CanLocalizePLC)
            {
              lError.Text = "This country cannot localize PLC";
            }
            else if (item.IsRoll)
            {
              lError.Text = "This product is roll";
            }
            //else if (item.IsMinimizedByCulture(SessionState.Culture.Code))
            //{
            //  lError.Text = "This product is minimized";
            //}
            //QC 1639: CRYS: PLC section becomes editable after the PLC is updated through UI (using reports from Localize menu)
            // Fixed by Prabhu - 02 Mar 08
            // Comment: Added PLCUserId check to filter PMG sourced products being edited in UI
            else if (PLCUserId < 1)
            {
              lError.Text = "The PLC for this product is sourced from PMG. User cannot edit it.";
            }
            lError.CssClass = "hc_error";
            lError.Visible = true;
          }
          System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
          ci.DateTimeFormat.ShortDatePattern = SessionState.User.FormatDate;
          ci.DateTimeFormat.LongDatePattern = SessionState.User.FormatDate;
          wdcAnnouncementDate.CalendarLayout.Culture = ci;
          wdcBlindDate.CalendarLayout.Culture = ci;
          wdcPID.CalendarLayout.Culture = ci;
          wdcObsoleteDate.CalendarLayout.Culture = ci;
          wdcRemovalDate.CalendarLayout.Culture = ci;
        }
      }
      catch (Exception fe)
      {
        UITools.FrameAlertAndBack(fe.ToString().Replace(Environment.NewLine, "."));
      }
      finally
      {
        if (c != null) { c.Dispose(); }
      }
    }

    /// <summary>
    /// Retrieve PLC for the item and culture requested
    /// </summary>
    private void RetrievePLC()
    {
      #region Update PLC Label
      lbPID.Text = UITools.PIDLabel;
      lbPOD.Text = UITools.PODLabel;
      lbBlind.Text = UITools.BlindLabel;
      lbAnnouncement.Text = UITools.AnnouncementLabel;
      lbRemoval.Text = UITools.RemovalLabel;
      #endregion
      using (PLC itemPLC = PLC.GetByKey(itemId, curCountryCode))
      {
        if (itemPLC != null)
        {
          using (Country plcCountry = itemPLC.Country)
          {
            #region Announcement date
            wdcAnnouncementDate.Value = itemPLC.AnnouncementDate ?? null;
            iRegionAnnouncement.ImageUrl = itemPLC.AnnouncementDateType == 'R' ? "/hc_v4/img/flags/" + plcCountry.MainRegionCode + ".gif" : "/hc_v4/img/flags/" + curCountryCode + ".gif";
            iRegionAnnouncement.Visible = itemPLC.AnnouncementDate != null;
            iRegionAnnouncement.ToolTip = itemPLC.AnnouncementDateType == 'R' ? "Regional date" : "Country date";
            cboxAnnouncementDate.Visible = (itemPLC.AnnouncementDateType == 'R');
            cboxAnnouncementDate.Checked = (itemPLC.AnnouncementDateType == 'C');
            cbAnnL.Checked = itemPLC.AnnouncementLocked;
            #endregion
            #region Blind date
            wdcBlindDate.Value = itemPLC.BlindDate ?? null;
            iRegionBlind.ImageUrl = itemPLC.BlindDateType == 'R' ? "/hc_v4/img/flags/" + plcCountry.MainRegionCode + ".gif" : "/hc_v4/img/flags/" + curCountryCode + ".gif";
            iRegionBlind.Visible = itemPLC.BlindDate != null;
            iRegionBlind.ToolTip = itemPLC.BlindDateType == 'R' ? "Regional date" : "Country date";
            cboxBlindDate.Visible = (itemPLC.BlindDateType == 'R');
            cboxBlindDate.Checked = (itemPLC.BlindDateType == 'C');
            cbBlindL.Checked = itemPLC.BlindLocked;
            #endregion
            #region PID
            wdcPID.Value = itemPLC.FullDate ?? null;
            iRegionPID.ImageUrl = itemPLC.FullDateType == 'R' ? "/hc_v4/img/flags/" + plcCountry.MainRegionCode + ".gif" : "/hc_v4/img/flags/" + curCountryCode + ".gif";
            iRegionPID.Visible = itemPLC.FullDate != null;
            iRegionPID.ToolTip = itemPLC.FullDateType == 'R' ? "Regional date" : "Country date";
            cboxPID.Visible = (itemPLC.FullDateType == 'R');
            cboxPID.Checked = (itemPLC.FullDateType == 'C');
            cbPIDL.Checked = itemPLC.FullLocked;
            #endregion
            #region Obsolete date
            wdcObsoleteDate.Value = itemPLC.ObsoleteDate ?? null;
            iRegionObso.ImageUrl = itemPLC.ObsoleteDateType == 'R' ? "/hc_v4/img/flags/" + plcCountry.MainRegionCode + ".gif" : "/hc_v4/img/flags/" + curCountryCode + ".gif";
            iRegionObso.Visible = itemPLC.ObsoleteDate != null;
            iRegionObso.ToolTip = itemPLC.ObsoleteDateType == 'R' ? "Regional date" : "Country date";
            cboxObsoleteDate.Visible = (itemPLC.ObsoleteDateType == 'R');
            cboxObsoleteDate.Checked = (itemPLC.ObsoleteDateType == 'C');
            cbObsoL.Checked = itemPLC.ObsoleteLocked;
            //Prabhu - Fix for HPeZilla Bug 70814  - CRYS: Issue with the "PLC" Edit Window Date Fields
            //Commented out the if condition as it not needed
            //if ((Convert.ToDateTime(wdcObsoleteDate.Value).AddDays(30) < DateTime.Now) && (itemPLC.ObsoleteDate != null))
            //{
            //  wdcObsoleteDate.Enabled = false;
            //  cboxObsoleteDate.Enabled = false;
            //  cbObsoL.Enabled = false;
            //}
            #endregion
            #region Removal date
            wdcRemovalDate.Value = itemPLC.RemovalDate ?? null;
            iRegionRemoval.ImageUrl = itemPLC.RemovalDateType == 'R' ? "/hc_v4/img/flags/" + plcCountry.MainRegionCode + ".gif" : "/hc_v4/img/flags/" + curCountryCode + ".gif";
            iRegionRemoval.Visible = itemPLC.RemovalDate != null;
            iRegionRemoval.ToolTip = itemPLC.RemovalDateType == 'R' ? "Regional date" : "Country date";
            cboxRemovalDate.Visible = (itemPLC.RemovalDateType == 'R');
            cboxRemovalDate.Checked = (itemPLC.RemovalDateType == 'C');
            cbRemovL.Checked = itemPLC.RemovalLocked;
            #endregion
            #region "Ugly PMT code"
            // Easy Content awesome requirement
            // Display a warning when PMT date is more recent and different from current obso and full date
            if (itemPLC.ObsoleteLocked || itemPLC.FullLocked)
            {
              bool bShowFullPMT = false, bShowObsoletePMT = false;
              using (Database dbObj = new Database(SessionState.CacheComponents["Inbound_DB"].ConnectionString))
              {
                if (dbObj != null)
                {
                  using (DataSet dsPMT = dbObj.RunSQLReturnDataSet("Select FileTimeStamp FROM Interfaces WHERE InterfaceId = 3"))
                  {
                    if (dsPMT != null && dsPMT.Tables.Count == 1 && dsPMT.Tables[0].Rows.Count == 1 && dsPMT.Tables[0].Rows[0][0] != DBNull.Value)
                    {
                      DateTime? pmtFileDate = Utils.PMTConvertToDate(dsPMT.Tables[0].Rows[0][0].ToString());
                      if (pmtFileDate != null)
                      {
                        using (DataSet ds = dbObj.RunSPReturnDataSet("QDE_GetPMTInfo", new SqlParameter("@CountryCode", curCountryCode)
                        , new SqlParameter("@LanguageCode", curLanguageCode)
                        , new SqlParameter("@Sku", curSku)))
                        {
                          if (ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 1)
                          {
                            DateTime? pmtFull = HyperCatalog.DataAccessLayer.SqlDataAccessLayer.GetProperDate(ds.Tables[0].Rows[0]["FullDate"]);
                            DateTime? pmtObso = HyperCatalog.DataAccessLayer.SqlDataAccessLayer.GetProperDate(ds.Tables[0].Rows[0]["ObsoleteDate"]);
                            bShowFullPMT = pmtFull.HasValue && itemPLC.FullLocked && pmtFileDate > itemPLC.FullModifyDate && itemPLC.FullDate != pmtFull;
                            bShowObsoletePMT = pmtObso.HasValue && itemPLC.ObsoleteLocked && pmtFileDate > itemPLC.ObsoleteModifyDate && itemPLC.ObsoleteDate != pmtObso;
                            if (bShowFullPMT)
                            {
                              lbPID.Text = lbPID.Text + "<a href='javascript://' title='" + SessionState.User.FormatUtcDate(pmtFull.Value, false, SessionState.User.FormatDate).Substring(0, 10) + "'><font color=red><b>(PMT)</b></font></a>";
                            }
                            if (bShowObsoletePMT)
                            {
                              lbPOD.Text = lbPOD.Text + "<a href='javascript://' title='" + SessionState.User.FormatUtcDate(pmtObso.Value, false, SessionState.User.FormatDate).Substring(0, 10) + "'><font color=red><b>(PMT)</b></font></a>";
                            }

                          }
                          else
                          {
                            Trace.Warn("Error running [Select * FROM PMT WHERE CountryCode = '" + curCountryCode + "' AND LanguageCode = '" + curLanguageCode + "' AND ProductNumber = '" + curSku + "'] query :" + dbObj.LastError);
                          }
                        }
                      }
                      else
                      {
                        Trace.Warn("Error : pmtFileDate return invalid datetime");
                      }
                    }
                    else
                    {
                      Trace.Warn("Error : Interface Id (#3) not found");
                    }
                  }
                }
                else
                {
                  Trace.Warn("Error : Cannot connect to Inbound DB [" + SessionState.CacheComponents["Inbound_DB"].ConnectionString + "]");
                }
              }
            }
            #endregion
            // stored in JS the current value of announcement and the type of announcement date, use for the rule
            // stored in JS the current value of blind and the type of blind date, use for the rule

			//Commented for #2807 -- ITG GS: Error in "Products Nearly Obsolete" Page 
            //string script = "var annR='" + itemPLC.AnnouncementDateType.Value + "';var blindR='" + itemPLC.BlindDateType.Value + "';";
            // Fix for 2807 and 71549 are the same (Chardonnay and PRISM fixes)
		string script = "var annR='" + itemPLC.AnnouncementDateType + "';var blindR='" + itemPLC.BlindDateType + "';";

            if (itemPLC.AnnouncementDate.HasValue)
            {
              int m = itemPLC.AnnouncementDate.Value.Month - 1;
              script = script + "var pann= new Date(" + itemPLC.AnnouncementDate.Value.Year.ToString() + ", " + m.ToString() + ", " + itemPLC.AnnouncementDate.Value.Day.ToString() + ");";
            }
            else
            {
              script = script + "var pann= null;";
            }
            if (itemPLC.BlindDate.HasValue)
            {
              int m = itemPLC.BlindDate.Value.Month - 1;
              script = script + "var pblind= new Date(" + itemPLC.BlindDate.Value.Year.ToString() + ", " + m.ToString() + ", " + itemPLC.BlindDate.Value.Day.ToString() + ");";
            }
            else
            {
              script = script + "var pblind= null;";
            }
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script>" + script + "</script>");
          }
        }
      }

    }

    /// <summary>
    /// Save PLC
    /// </summary>
    private void SavePLC()
    {
      using (PLC itemPLC = PLC.GetByKey(itemId, curCountryCode))
      {
        if (itemPLC != null)
        {
          #region Bundles rule
          // public intro date for bundles cannot be before the latest public intro dates of all the components.
          // Obsolescence dates cannot be set later than the earliest obsolescence date of the components
         if (curItemType == (int)ItemTypesEnum.BUNDLE)
          {
            if (!itemPLC.CheckPLCforBundle(curCountryCode , (DateTime?)wdcPID.Value, (DateTime?)wdcObsoleteDate.Value))
            {
              lError.Text = PLC.LastError;
              lError.Visible = true;
              return;
            }
          }
          #endregion
          // all PLC are unlock before changes, prevent API rules
          #region PID
          itemPLC.FullLocked = false;
          itemPLC.FullDate = (DateTime?)wdcPID.Value;
          itemPLC.FullDateType = cboxPID.Checked ? 'C' : 'R';
          itemPLC.FullLocked = cbPIDL.Checked;
          #endregion
          #region Announcement date
          itemPLC.AnnouncementLocked = false;
          itemPLC.AnnouncementDate = (DateTime?)wdcAnnouncementDate.Value;
          itemPLC.AnnouncementDateType = cboxAnnouncementDate.Checked ? 'C' : 'R';
          itemPLC.AnnouncementLocked = cbAnnL.Checked;
        #endregion
          #region Blind date
          itemPLC.BlindLocked = false;
          itemPLC.BlindDate = (DateTime?)wdcBlindDate.Value;
          itemPLC.BlindDateType = cboxBlindDate.Checked ? 'C' : 'R';
          itemPLC.BlindLocked = cbBlindL.Checked;
        #endregion
          #region Obsolete date
          itemPLC.ObsoleteLocked = false;
          itemPLC.ObsoleteDate = (DateTime?)wdcObsoleteDate.Value;
          itemPLC.ObsoleteDateType = cboxObsoleteDate.Checked ? 'C' : 'R';
          itemPLC.ObsoleteLocked = cbObsoL.Checked;
        #endregion
          #region Removal date
          itemPLC.RemovalLocked = false;
          itemPLC.RemovalDate = (DateTime?)wdcRemovalDate.Value;
          itemPLC.RemovalDateType = cboxRemovalDate.Checked ? 'C' : 'R';
          itemPLC.RemovalLocked = cbRemovL.Checked;
        #endregion
          if (itemPLC.Save(SessionState.User.Id))
          {
              //Sateesh -- Language Scope Management - ACQ 3.6 - 27/05/2009
            Item.UpdateWorkingTables(itemPLC.ItemId,SessionState.Culture.Code, true);
            if (Request["dg"] == null)
            {
              Page.ClientScript.RegisterStartupScript(this.GetType(), "reloadParent", "<script>ReloadParent();</script>");
            }
            else // We have been opened by another page that contains an Infragistics Grid
            // Call parent refresh function to reflect the changes.
            {
              if (Request["row"] != null && Request["col"] != null)
              {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "reloadParent", "<script>top.window.close()</script>");
              }
            }
          }
          else
          {
            lError.Text = PLC.LastError;
            lError.Visible = true;
          }
        }
      }
    }

    protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string bt = be.Button.Key.ToLower();
      if (bt == "save")
      {
        SavePLC();
      }
    }
}
}
