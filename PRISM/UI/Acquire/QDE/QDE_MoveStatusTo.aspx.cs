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
using System.Data.SqlClient;
using HyperCatalog.Business;

namespace HyperCatalog.UI.Acquire.QDE
{
	/// <summary>
	/// This class change status all contant for a given item (with children if necessary) 
	/// </summary>
	public partial class QDE_MoveStatusTo : HCPage
	{
		#region Declarations
		private HyperCatalog.Business.Item item = null;
		private HyperCatalog.Business.User user = null;
		private HyperCatalog.Business.Culture culture = null;
		private int inputFormId = -1;
        private const int MPD_CONTAINER_ID = 3;
        private string containerMATFList = string.Empty;
        private string warningMessage = string.Empty;
        //Added by Prabhu for CR 5160 & ACQ 8.12 (PCF1: Auto TR Redesign)-- 21/May/09
        bool isAutoTRButton = false;
		#endregion

		protected void Page_Load(object sender, System.EventArgs e)
    {
      System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
      ci.DateTimeFormat.ShortDatePattern = SessionState.User.FormatDate;
      ci.DateTimeFormat.LongDatePattern = SessionState.User.FormatDate;
      wdMasterPublishing.CalendarLayout.Culture = ci;
      wdMasterPublishing.MinDate = DateTime.UtcNow;

      warningMessage = "<font color='red' size='2'><b>Error:</b> Regional MATF is not possible.</font><br><br>" +
                      "This item has project with BOR date in future. Hence this item cannot be region validated. <br>" +
                      "Requesting user to try again after BOR date has reached.";

      if (SessionState.User.IsReadOnly)
      {
         uwToolbar.Items.FromKeyButton("Save").Enabled = false;
      }

      // Hide or show button switch capabilities
      if (!SessionState.User.HasCapability(CapabilitiesEnum.EDIT_DELETE_FINAL_CHUNKS_MARKET_SEGMENT))
      {
        UITools.HideToolBarButton(uwToolbar, "Save");
        UITools.HideToolBarSeparator(uwToolbar, "SaveSep");
      }

			// Retrieve current user and current culture
			user = SessionState.User;
			culture = SessionState.Culture;

			try
			{
				// Get parameters
				if (Request["i"] != null)
					item = HyperCatalog.Business.Item.GetByKey(Convert.ToInt64(Request["i"]));
				else if (QDEUtils.GetItemIdFromRequest() != null)
					item = QDEUtils.GetItemIdFromRequest();

                if (Request["l"] != null)
                  culture = QDEUtils.UpdateCultureCodeFromRequest("l");

		        if (Request["f"] != null)
			        inputFormId = Convert.ToInt32(Request["f"]);

                //Added by Prabhu for CR 5160 & ACQ 8.12 (PCF1: Auto TR Redesign)-- 21/May/09
                if (Request["isATRButton"] != null)
                    isAutoTRButton = Convert.ToBoolean(Request["isATRButton"]);

                if (inputFormId > 0)
                {
                  containerMATFList = Request["c"].ToString();
                  if (culture.Type == CultureType.Regionale && isAutoTRButton)
                  {
                      lblChkBoxAlert.Visible = true;
                      lblChkBoxAlert.Text = "<font color='red'><b>Note:</b> Move status to will be performed on locally authored content only.</font>";
                  }
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
					if (!Page.IsPostBack)
					{
                        DateTime today = DateTime.UtcNow;
                        item.RegionCode = culture.Code;
                        // check if this item or its inherited item has BOR date in future.
                        if (item.Milestones != null && culture.Type == CultureType.Regionale && inputFormId < 0)
                        {
                            if (!item.Milestones.Inherited)
                            {
                                if ((item.Milestones.BeginningOfRegionalization != null && item.Milestones.BeginningOfRegionalization.Value > today))
                                {
                                    panelMATF.Visible = false;
                                    lblWarningMessage.Visible = true;
                                    lblWarningMessage.Text = warningMessage;
                                }
                                else
                                    UpdateDataView();
                            }
                            else
                            {
                                item.Milestones.InheritedItem.RegionCode = culture.Code;
                                if ((item.Milestones.InheritedItem.Milestones.BeginningOfRegionalization != null && item.Milestones.InheritedItem.Milestones.BeginningOfRegionalization.Value > today))
                                {
                                    panelMATF.Visible = false;
                                    lblWarningMessage.Visible = true;
                                    lblWarningMessage.Text = warningMessage;
                                }
                                else
                                    UpdateDataView();
                            }
                            
                        }
                        else
                            UpdateDataView();
					}
				}
				else
				{
					UITools.DenyAccess(DenyMode.Popup);
					return;
				}
			}
			catch (Exception excep)
			{
        //Response.Write(excep.ToString());
				UITools.DenyAccess(DenyMode.Popup);
			}
		}
		override protected void OnInit(EventArgs e)
		{
			cbMasterPublishing.Attributes.Add("onClick", "CheckedChanged();");
			base.OnInit(e);
		}
		private void UpdateDataView()
		{
      // Default value
			lbError.Visible = false;
			rbFinal.Checked = true;
      pnlSoftRoll.Visible = false;
      pnlChildren.Visible = false;
      pnlMasterPublishing.Visible = false;
      pnlForceTranslationToDraft.Visible = false;
      pnlReport.Visible = false;

			// Update culture title
			if (culture != null)
			{
				uwToolbar.Items.FromKeyLabel("Culture").Text = culture.Name;
				uwToolbar.Items.FromKeyLabel("Culture").Image = "/hc_v4/img/flags/"+culture.CountryCode+".gif";
				uwToolbar.Items.FromKeyLabel("Culture").ToolTip = culture.CountryCode;
			}

			if (item != null)
			{
				// update title
				lbTitle.Text = item.FullName;
				if (lbTitle.Text.Length > 50)
					lbTitle.Text = lbTitle.Text.Substring(0,49) + "...";

        bool isUgd = true;

        if (inputFormId == -1)
        {
            #region "display or not panel to include children"
            pnlChildren.Visible = item.Childs.Count > 0;
            if (isUgd)
                tdChildren.Attributes["class"] = "ugd";
            else
                tdChildren.Attributes["class"] = "uga";
            isUgd = !isUgd;
            #endregion
            #region "display or not panel to soft roll"
            Item itemRoll = item.GetRoll();
            // item has a soft roll (that is initialized for regional or local culture) or item is a soft roll
            if (item.IsRoll || (itemRoll != null && (culture.Type == CultureType.Master || (culture.Type != CultureType.Master && itemRoll.IsInitialized))))
            {
                pnlSoftRoll.Visible = true;
                if (isUgd)
                    tdSoftRoll.Attributes["class"] = "ugd";
                else
                    tdSoftRoll.Attributes["class"] = "uga";
                isUgd = !isUgd;
            }
            #endregion
        }
        #region "display if current language is Master"
        wdMasterPublishing.Value = GetExistingMasterPublishingDate();
        if (SessionState.Culture.Type == HyperCatalog.Business.CultureType.Master && rbFinal.Checked)
        {
          pnlMasterPublishing.Visible = true;
          if (isUgd)
          {
            tdMasterPublishing.Attributes["class"] = "ugd";
            tdCalendar.Attributes["class"] = "ugd";
          }
          else
          {
            tdMasterPublishing.Attributes["class"] = "uga";
            tdCalendar.Attributes["class"] = "uga";
          }
          isUgd = !isUgd;
        }
        #endregion
        #region "display panel to force translation"
        pnlForceTranslationToDraft.Visible = true;
        if (isUgd)
          tdForceTranslation.Attributes["class"] = "ugd";
        else
          tdForceTranslation.Attributes["class"] = "uga";
        isUgd = !isUgd;
        #endregion
        #region "display panel to create report"
        pnlReport.Visible = true;
        if (isUgd)
          tdReport.Attributes["class"] = "ugd";
        else
          tdReport.Attributes["class"] = "uga";
        isUgd = !isUgd;
        #endregion
      }
		}
		private void Save()
		{
      using (HyperComponents.Data.dbAccess.Database dbObj = Utils.GetMainDB())
      {

        // Default value
        lbError.Visible = false;
        lbError.Text = string.Empty;

        if (pnlMasterPublishing.Visible)
          cbMasterPublishing.Visible = false;

        bool isClosed = false;
        string s = string.Empty;
        if (rbDraft.Checked)
          s = HyperCatalog.Business.ChunkStatus.Draft.ToString();
        else if (rbFinal.Checked)
          s = HyperCatalog.Business.ChunkStatus.Final.ToString();

        Item curItem = item;
        curItem.RegionCode = culture.Code;

        if (item.GetRoll() != null && rdSoftRoll.Checked)
          curItem = item.GetRoll();

        if (s.Length > 0)
        {
          #region "Save Master publishing date if necessary"
          if (cbMasterPublishing.Checked)
          {
            if (wdMasterPublishing.Value != null)
            {
              #region "Save Master Publishing Date Chunk"
              if (SessionState.Culture.Type == HyperCatalog.Business.CultureType.Master)
              {
                // 69605 issue fix
                // Use a SP to directly add/update the MPD chunk is ALL childs
                DateTime d = (DateTime)wdMasterPublishing.Value;
                string mpd = d.Month.ToString() + '/' + d.Day.ToString() + '/' + d.Year.ToString();
                dbObj.RunSPReturnInteger("_Item_MPDAddUpd", new SqlParameter("@ItemId", curItem.Id), new SqlParameter("@IncludeChildren", cbWithChildren.Checked), new SqlParameter("@MasterPublishingDate", mpd), new SqlParameter("@UserId", SessionState.User.Id));
              }
              #endregion
            }
            else
            {
              // Error: missing Master Publishing Date
              Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MissingMasterPublishingDate", "<script>alert('Please provide a valid [master publishing date]');</script>");
              return;
            }
          }
          #endregion

          //// Modified by Prabhu for ACQ 3.0 (PCF1: Regional Project Management)-- 18/May/09
          //// Comment out as not needed after Regional Project Management
          //// You cannot override a region project date from master.
          #region "Save Project Dates"
          /*
          // if item is already a project
          curItem.Milestones.ItemId = curItem.Id;
          curItem.Milestones.RegionCode = culture.Code;
          if (curItem.Milestones != null && culture.Type == CultureType.Regionale && inputFormId < 0)
          {
              if (!curItem.Milestones.Inherited)
              {
                  bool updateProjectDate = false;
                  // Force Master Acquisition Date to be NOW since the validation is done
                  // The product can now be visibile at regional level
                  if (curItem.Milestones.BeginningOfRegionalization == null)
                  {
                      curItem.Milestones.BeginningOfRegionalization = DateTime.UtcNow;
                      updateProjectDate = true;
                  }
                  if ((curItem.Milestones.EndOfRegionalization.HasValue && curItem.Milestones.EndOfRegionalization > DateTime.UtcNow) || curItem.Milestones.EndOfRegionalization == null)
                  {
                      curItem.Milestones.EndOfRegionalization = DateTime.UtcNow;
                      updateProjectDate = true;
                  }
                  if(updateProjectDate)
                  {
                      if (!curItem.Milestones.Save(user.Id))
                      {
                          lbError.CssClass = "hc_error";
                          lbError.Text = HyperCatalog.Business.ItemDates.LastError;
                          lbError.Visible = true;
                          return;
                      }
                  }
              }
          }
          */
          #endregion

          //#region "Move status Draft, Rejected --> Final or Final, Rejected --> Draft, if possible" --Alternate for CR 5096
          #region "Move status Draft --> Final or Final --> Draft, if possible"
          HyperCatalog.Business.ChunkStatus status = (HyperCatalog.Business.ChunkStatus)Enum.Parse(typeof(HyperCatalog.Business.ChunkStatus), s);
          if (!curItem.MoveContentToStatus(culture.Code, status, user.Id, cbWithChildren.Checked, inputFormId, containerMATFList))
          {
            lbError.CssClass = "hc_error";
            lbError.Text = HyperCatalog.Business.Item.LastError;
            lbError.Visible = true;
            return;
          }
          else
          {
            if (culture.Code == SessionState.MasterCulture.Code && inputFormId < 0)
            {
              string includeChildren = cbWithChildren.Checked?"1":"0";
              using (DataSet ds = dbObj.RunSQLReturnDataSet("NOTIFICATION_NotifyRegionalUsersMasterValidationOnMATF " + item.Id + ", "  + includeChildren))
              {
                dbObj.CloseConnection();
                if (dbObj.LastError != string.Empty)
                {
                  lbError.CssClass = "hc_error";
                  lbError.Text = "System was not able to notify regional users [" + dbObj.LastError + "]";
                  lbError.Visible = true;
                }
                else
                {

                  if (ds != null && ds.Tables.Count == 1)
                  {
                    string message = "New products are available at regional level, you can click <a href='";
                    message += HyperCatalog.Business.ApplicationSettings.Components["Crystal_UI"].URI;
                    message += "/UI/Collaborate/NPIReport.aspx'>here</a> to access the full list.";

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                      Utils.SendMail(dr["Email"].ToString(), dr["FirstName"].ToString() + " " + dr["LastName"].ToString(), "New products in region notification", message, false);
                    }
                  }
                }
              }

            }
          }
          #endregion

          // ds contains all content
          using (DataSet ds = dbObj.RunSPReturnDataSet("dbo._Item_GetAllContent", "Content",
            new SqlParameter("@ItemId", curItem.Id),
            new SqlParameter("@CultureCode", culture.Code),
            new SqlParameter("@InputFormId", inputFormId),
            new SqlParameter("@WithChildren", cbWithChildren.Checked),
            new SqlParameter("@MoveStatus", 1)))
          {
            dbObj.CloseConnection();
            if (dbObj.LastError.Length > 0)
            {
              lbError.CssClass = "hc_error";
              lbError.Text = HyperCatalog.Business.Item.LastError;
              lbError.Visible = true;
            }
            else
            {

              #region "Force translation to draft"
              if (cbForceTranslation.Checked && ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
              {
                string containerList = string.Empty;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                  if (dr["ContainerId"] != null)
                  {
                    if (containerList.Length > 0)
                      containerList += ",";
                    containerList += dr["ContainerId"].ToString();
                  }
                }

                if (containerList.Length > 0)
                {
                  if (!curItem.ForceTranslationsToDraft(culture.Code, containerList, user.Id))
                  {
                    lbError.CssClass = "hc_error";
                    lbError.Text = HyperCatalog.Business.Item.LastError;
                    lbError.Visible = true;
                    return;
                  }
                }
              }
              #endregion

              #region "Generate report"
              if (cbReport.Checked && ds != null)
              {
                // Create report
                GenerateReport(ds, curItem.Id);


                // Refresh frame content and close window
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "closePopup", "<script>window.close();</script>");
              }
            }
              #endregion
          }

          SessionState.CurrentItem = null;

          // Refresh frame content and close window
          if (SessionState.CurrentItem != null && SessionState.User.LastVisitedItem != SessionState.CurrentItem.Id)
          {
            SessionState.User.LastVisitedItem = curItem.Id;
            SessionState.User.QuickSave();
          }
          if (SessionState.QDEChunk != null)
          {
            SessionState.QDEChunk.Dispose();
          }
          if (SessionState.QDEContainer != null)
          {
            SessionState.QDEContainer.Dispose();
          }
          Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "updateGrid", "<script>UpdateAndClose(1, " + inputFormId.ToString() +");</script>");
        }
        else
        {
          Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MissingStatus", "<script>alert('Select the status (Draft or Final)');</script>");
        }
      }
		}

    protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();

      if (btn.Equals("save"))
      {
        Save();
      }
    }

		private void GenerateReport(DataSet ds, Int64 itemId)
		{
			if (ds != null)
			{
        ExportUtils.ExportReportOfMoveStatus(ds, itemId, inputFormId, this.Page);
				if (ds != null)
					ds.Dispose();
			}
		}
    protected void rbDraft_CheckedChanged(object sender, EventArgs e)
    {
      bool isUgd = (tdMasterPublishing.Attributes["class"] == "ugd");
      pnlMasterPublishing.Visible = false;

      if (pnlForceTranslationToDraft.Visible)
      {
        if (isUgd)
          tdForceTranslation.Attributes["class"] = "ugd";
        else
          tdForceTranslation.Attributes["class"] = "uga";
        isUgd = !isUgd;
      }
      if (pnlReport.Visible)
      {
        if (isUgd)
          tdReport.Attributes["class"] = "ugd";
        else
          tdReport.Attributes["class"] = "uga";
      }
    }
    protected void rbFinal_CheckedChanged(object sender, EventArgs e)
    {
      pnlMasterPublishing.Visible = true;
      bool isUgd = (tdMasterPublishing.Attributes["class"] == "uga");

      if (pnlForceTranslationToDraft.Visible)
      {
        if (isUgd)
          tdForceTranslation.Attributes["class"] = "ugd";
        else
          tdForceTranslation.Attributes["class"] = "uga";
        isUgd = !isUgd;
      }
      if (pnlReport.Visible)
      {
        if (isUgd)
          tdReport.Attributes["class"] = "ugd";
        else
          tdReport.Attributes["class"] = "uga";
      }
    }
    private object GetExistingMasterPublishingDate()
    {
      Item curItem = item;
      if (item.GetRoll() != null && rdSoftRoll.Checked)
        curItem = item.GetRoll();

      using (HyperCatalog.Business.Chunk mpd = HyperCatalog.Business.Chunk.GetByKey(curItem.Id, 3, SessionState.Culture.Code))
      {
        if (mpd != null)
        {
          System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
          return Convert.ToDateTime(mpd.Text, ci.DateTimeFormat);
        }
      }
      return null;
    }
  }
}
