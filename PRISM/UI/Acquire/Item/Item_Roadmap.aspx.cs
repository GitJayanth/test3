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

namespace HyperCatalog.UI.ItemManagement
{
	/// <summary>
	/// This class builds roadmap for one product and its descendants
	/// </summary>
	public partial class Item_Roadmap : HCPage
	{
		#region Declarations
		private Item item = null;
    private Item PLCItem = null;
		private DateTime startDate;
		private int nbMonths;
		#endregion

    #region Constantes
    private const int MONTH_WIDTH = 30;
    #endregion

    #region Code généré par le Concepteur Web Form
    override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN : Cet appel est requis par le Concepteur Web Form ASP.NET.
			//
			InitializeComponent();
      rDates.ItemDataBound += new RepeaterItemEventHandler(rDates_ItemDataBound);
			base.OnInit(e);
		}
		
		/// <summary>
		/// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
		/// le contenu de cette méthode avec l'éditeur de code.
		/// </summary>
		private void InitializeComponent()
		{    
			this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);
		}
		#endregion

		protected void Page_Load(object sender, System.EventArgs e)
		{
      System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
      ci.DateTimeFormat.ShortDatePattern = SessionState.User.FormatDate;
      ci.DateTimeFormat.LongDatePattern = SessionState.User.FormatDate;
      wdCalendar.CalendarLayout.Culture = ci;

      if (SessionState.Culture != null && SessionState.Culture.Type == CultureType.Locale)
      {
        string countryCode = SessionState.Culture.CountryCode;
        string parameters = "i=" + Request["i"].ToString()+"&c=" + countryCode;
        string url = "./item_roadmap_country.aspx?" + parameters;
        Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "", "window.location='" + url + "';", true);
      }
      else
      {
        try
        {
          // Display start date
          if (wdCalendar.Text.Length == 0)
            wdCalendar.Value = DateTime.Now;

          #region "Retrieve parameters"
          if (Request["i"] != null)
          {
            Int64 itemId = Convert.ToInt64(Request["i"]);
            item = Item.GetByKey(itemId);
          }
          #endregion

          if (item != null)
          {
            #region "Retrieve country for PLC"
            // Show countries for the selected item
            if (Request["__EVENTTARGET"] != null && Request["__EVENTTARGET"].ToString() == "expCount")
            {
              ViewState["SelectedItemId"] = Request["__EVENTARGUMENT"];
            }
            // Hide countries for the selected item
            if (Request["__EVENTTARGET"] != null && Request["__EVENTTARGET"].ToString() == "colCount")
            {
              ViewState["SelectedItemId"] = null;
            }

            // Retrieve the selected item if necessary
            if (ViewState["SelectedItemId"] != null)
            {
              Int64 PLCItemId = Convert.ToInt64(ViewState["SelectedItemId"]);
              PLCItem = Item.GetByKey(PLCItemId);
            }
            #endregion

            UpdateDataView();
          }
          else
          {
            UITools.DenyAccess(DenyMode.Popup);
          }
        }
        catch (Exception ex)
        {
          Page.Response.Write(ex.ToString());
          Page.Response.End();
        }
      }
		}
		private void UpdateDataView()
		{
      lbError.Visible = false;
      lbResult.Visible = false;

			if (item != null)
      {
        // Update style for countries
        if (PLCItem != null && PLCItem.PLCDates != null)
        {
          ArrayList countryCodes = new ArrayList();
          string styles = string.Empty;
          foreach (PLC plc in PLCItem.PLCDates)
          {
            if (plc != null && plc.CountryCode != null && countryCodes != null && (countryCodes.IndexOf(plc.CountryCode) < 0))
              styles += ".ctry" + plc.CountryCode.ToLower() + "{ background-image: url(/hc_v4/img/flags/" + plc.CountryCode.ToUpper() + ".gif); background-repeat: no-repeat; margin-left:0; background-position: top left; padding-left: 22px;}\n";

            countryCodes.Add(plc.CountryCode);
          }
          if (styles.Length > 0)
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "UpdateStyle", "<style>" + styles + "</style>");
        }

        // Retrieve the beginning of date
        startDate = DateTime.Now;
        if (wdCalendar.Value != null)
          startDate = (DateTime)wdCalendar.Value;
        // Retrieve the period
        nbMonths = Convert.ToInt32(DDL_Period.SelectedValue);

				// Retrieve RoadMap
				DataSet ds = item.GetRoadMap(SessionState.Culture.Code);
        if (ds != null)
        {
          if (ds.Tables != null && ds.Tables.Count > 0)
          {
            lbResult.CssClass = "hc_success";
            lbResult.Text = "Row count: "+ds.Tables[0].Rows.Count;
            lbResult.Visible = true;

            // create repeater
            rDates.DataSource = ds;
            rDates.DataBind();

            ds.Dispose();
          }
        }
        else
        {
          lbResult.CssClass = "hc_success";
          lbResult.Text = "No result";
          lbResult.Visible = true;
        }
			}
		}

		#region Event methods
		private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be){}
    void rDates_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
      if (e != null && e.Item != null)
      {
        if (e.Item.ItemType.Equals(ListItemType.Header))
        {
          #region "Header"
          Label lbHeader = (Label)e.Item.FindControl("lbHeader");
          if (lbHeader != null)
          {
            lbHeader.Text = "<tr valign='top'>";
            lbHeader.Text += "<td class='h'>Name</td>";
            DateTime curDateTitle = startDate;
            for (int i = 0; i <= nbMonths; i++)
            {
              lbHeader.Text += "<td class='v' width='" + ComputeWidth().ToString() + "px'>" + curDateTitle.ToString(SessionState.User.FormatDate) + "&nbsp;</td>";
              curDateTitle = curDateTitle.AddMonths(1);
            }

            lbHeader.Text += "<td class='bs'>&nbsp;</td>";
            lbHeader.Text += "</tr>";
          }
          #endregion
        }
        else if (e.Item.ItemType.Equals(ListItemType.Item) || e.Item.ItemType.Equals(ListItemType.AlternatingItem))
        {
          #region Item
          int index = e.Item.ItemIndex;
          Label lbBar = (Label)e.Item.FindControl("lbBar");

          if (lbBar != null && rDates != null)
          {
            DataSet ds = (DataSet)rDates.DataSource;
            DataRow dr = null;
            string curItemSku = string.Empty;
            string curCountryCode = string.Empty;
            string curItemId = string.Empty;
            string curLevelId = string.Empty;
            string curIsSKU = string.Empty;
            string curItemName = string.Empty;

            if (index >= 0 && ds.Tables != null && ds.Tables.Count > 0 && index < ds.Tables[0].Rows.Count)
            {
              dr = ds.Tables[0].Rows[index];
              if (dr != null)
              {
                lbBar.Text = "<tr>";

                #region "Display name"
                if (dr["ItemId"] != null)
                  curItemId = dr["ItemId"].ToString();
                if (dr["ItemSku"] != null)
                  curItemSku = dr["ItemSku"].ToString();
                if (dr["LevelId"] != null)
                  curLevelId = dr["LevelId"].ToString();
                if (dr["ItemName"] != null)
                  curItemName = dr["ItemName"].ToString();
                if (dr["SKULevel"] != null)
                  curIsSKU = dr["SKULevel"].ToString();

                lbBar.Text += "<td class='n'>";
                lbBar.Text += GenerateSpaces(curLevelId)+"[" + curLevelId + "]";

                if (HasPLC(curItemId, curIsSKU, ds))
                {
                  lbBar.Text += "<a href='javascript:__doPostBack(\"";
                  lbBar.Text += PLCItem != null ? (curItemId.Equals(PLCItem.Id.ToString()) ? "colCount" : "expCount") : "expCount";
                  lbBar.Text += "\", " + curItemId.ToString() + ");' title='";
                  lbBar.Text += PLCItem != null ? (curItemId.Equals(PLCItem.Id.ToString()) ? "Hide" : "Show") : string.Empty;
                  lbBar.Text += " PLC dates for this product'>";

                  if (curItemSku != null && curItemSku.Length > 0)
                    lbBar.Text += "[" + curItemSku + "] ";
                  lbBar.Text += curItemName + "</a>";
                }
                else
                {
                  if (curItemSku != null && curItemSku.Length > 0)
                    lbBar.Text += "[" + curItemSku + "] ";
                  lbBar.Text += curItemName;
                  lbBar.Text += curIsSKU.Equals("True") ? " <img src='/hc_v4/img/ed_warning.gif' alt='NO PLC!' />" : "";
                }
                lbBar.Text += "</td>";
                #endregion

                #region "Draw row"
                lbBar.Text += "<td class='bb' colspan='" + (nbMonths + 1).ToString() + "'>";
                lbBar.Text += DrawRow(dr);
                lbBar.Text += "</td>";
                #endregion

                lbBar.Text += "<td class='bs'>&nbsp;</td>";
                lbBar.Text += "</tr>";

                if (PLCItem != null && curItemId.Equals(PLCItem.Id.ToString()) && ds != null && ds.Tables.Count > 1)
                {
                  Label lbBarPLC = (Label)e.Item.FindControl("lbBarPLC");
                  lbBarPLC.Text = string.Empty;
                  string countryCodePLC = string.Empty;
                  string countryNamePLC = string.Empty;
                  string curPlcItemId = string.Empty;
                  foreach (DataRow drPLC in ds.Tables[1].Rows)
                  {
                    if (drPLC["ItemId"] != null)
                      curPlcItemId = drPLC["ItemId"].ToString();

                    if (curPlcItemId.Equals(PLCItem.Id.ToString()))
                    {
                      if (drPLC["CountryCode"] != null)
                        countryCodePLC = drPLC["CountryCode"].ToString();
                      if (drPLC["CountryName"] != null)
                        countryNamePLC = drPLC["CountryName"].ToString();

                      lbBarPLC.Text += "<tr>";

                      #region "Display country name"
                      lbBarPLC.Text += "<td class='n ctry" + countryCodePLC.ToLower() + "'>";
                      lbBarPLC.Text += countryNamePLC;
                      lbBarPLC.Text += "</td>";
                      #endregion

                      #region "Draw bar for PLC"
                      lbBarPLC.Text += "<td class='bb' colspan='" + (nbMonths + 1).ToString() + "'>";
                      lbBarPLC.Text += DrawRow(drPLC);
                      lbBarPLC.Text += "</td>";
                      #endregion

                      lbBarPLC.Text += "<td class='bs'>&nbsp;</td>";
                      lbBarPLC.Text += "</tr>";
                    }
                  }

                  lbBarPLC.Visible = (lbBarPLC.Text.Length > 0);
                }
              }
            }
          }
          #endregion
        }
      }
    }
		#endregion

    #region Private methods
    private string DrawRow(DataRow dr)
    {
      string drawing = string.Empty;
      if (dr != null)
      {
        drawing += "<table class='b'><tr>";
        drawing += "<td class='mr'></td>";
        drawing += "<td class='p' width='" + (ComputeWidth() * nbMonths).ToString() + "px'>";
        drawing += "<A title=\"" + ShowHintInfo(dr) + "\" href='javascript://'>";
        drawing += "<table class='rc'><tr>";
        drawing += CreateBar(dr);
        drawing += "</tr>";
        drawing += "</table>";
        drawing += "</A>";
        drawing += "</td>";
        drawing += "</tr>";
        drawing += "</table>";
      }
      return drawing.Replace("\n", "");
    }
    private double ComputeWidth()
    {
      switch (nbMonths)
      {
        case 6:
          return MONTH_WIDTH * 2.3;
        case 9:
          return MONTH_WIDTH * 2;
        case 12:
          return MONTH_WIDTH * 1.7;
        case 15:
          return MONTH_WIDTH * 1.5;
        case 18:
          return MONTH_WIDTH * 1.3;
        default:
          return MONTH_WIDTH;
      }
    }
    private string CreateBar(DataRow dr)
    {
      string bar = string.Empty;

      // Build date list
      ArrayList dateListSorted = BuildDateList(dr);

      // Create bar
      if (dateListSorted != null && dateListSorted.Count > 0)
      {
        int nbDays;
        double colWidth;
        TimeSpan difDate;

        DateTime endDate = startDate.AddMonths(nbMonths);
        dateListSorted = RetrieveIncludePeriod(dateListSorted, startDate, endDate); // Contains (startDate, d1, d2, ..., endDate)

        for (int i = 0; i < dateListSorted.Count; i++)
        {
          RoadMapDate currentDate = (RoadMapDate)dateListSorted[i];
          if (i < dateListSorted.Count - 1)
          {
            RoadMapDate nextDate = (RoadMapDate)dateListSorted[i + 1];
            nbDays = RetrieveNbDays(startDate, nbMonths);

            difDate = nextDate.Date.Subtract(currentDate.Date);
            colWidth = Math.Round((difDate.TotalDays * 100) / nbDays, 0);
            if (colWidth == 0)
              colWidth = 1;

            if (nextDate.Date == endDate)
            {
              bar += "<td class='" + currentDate.CssClass + "' width='*'>&nbsp;</td>";
            }
            else
            {
              bar += "<td class='" + currentDate.CssClass + "' width='" + colWidth.ToString() + "%'>&nbsp;</td>";
            }
          }
        }
      }
      else
      {
        bar += "<td class='rg' width='100%'>&nbsp;</td>";
      }

      return bar;
    }
    private ArrayList BuildDateList(DataRow dr)
    {
      DateTime? blindDate = null;
      DateTime? fullDate = null;
      DateTime? liveDate = null;
      DateTime? obsoleteDate = null;
      DateTime? removalDate = null;
      DateTime? announcementDate = null;
      ArrayList dateList = new ArrayList();
      string status = string.Empty;
      string workflowStatus = string.Empty;

      if (dr != null)
      {
        // Retrieve all dates
        if (dr.Table.Columns.IndexOf("BlindDate") > -1 && dr["BlindDate"] != null && dr["BlindDate"] != DBNull.Value)
          blindDate = Convert.ToDateTime(dr["BlindDate"]);
        if (dr.Table.Columns.IndexOf("FullDate") > -1 && dr["FullDate"] != null && dr["FullDate"] != DBNull.Value)
          fullDate = Convert.ToDateTime(dr["FullDate"]);
        if (dr.Table.Columns.IndexOf("LiveDate") > -1 && dr["LiveDate"] != null && dr["LiveDate"] != DBNull.Value)
          liveDate = Convert.ToDateTime(dr["LiveDate"]);
        if (dr.Table.Columns.IndexOf("ObsoleteDate") > -1 && dr["ObsoleteDate"] != null && dr["ObsoleteDate"] != DBNull.Value)
          obsoleteDate = Convert.ToDateTime(dr["ObsoleteDate"]);
        if (dr.Table.Columns.IndexOf("RemovalDate") > -1 && dr["RemovalDate"] != null && dr["RemovalDate"] != DBNull.Value)
          removalDate = Convert.ToDateTime(dr["RemovalDate"]);
        if (dr.Table.Columns.IndexOf("AnnouncementDate") > -1 && dr["AnnouncementDate"] != null && dr["AnnouncementDate"] != DBNull.Value)
          announcementDate = Convert.ToDateTime(dr["AnnouncementDate"]);

        string cssClass = string.Empty;
        if (blindDate.HasValue && fullDate.HasValue && blindDate.Value >= fullDate.Value) { cssClass = "ri";}
        if (blindDate.HasValue && liveDate.HasValue && blindDate.Value >= liveDate.Value) { cssClass = "ri";}
        if (obsoleteDate.HasValue && fullDate.HasValue && fullDate.Value >= obsoleteDate.Value) cssClass = "ri";
        if (obsoleteDate.HasValue && liveDate.HasValue && liveDate.Value >= obsoleteDate.Value) cssClass = "ri";
        if (blindDate.HasValue && obsoleteDate.HasValue && blindDate.Value > obsoleteDate.Value) cssClass = "ri";
        if (liveDate.HasValue && obsoleteDate.HasValue && liveDate.Value > obsoleteDate.Value) cssClass = "ri";
        if (removalDate.HasValue && fullDate.HasValue && removalDate.Value < fullDate.Value) cssClass = "ri";
        if (removalDate.HasValue && announcementDate.HasValue && removalDate.Value <= announcementDate.Value) cssClass = "ri";
        if (announcementDate.HasValue && obsoleteDate.HasValue && announcementDate.Value >= obsoleteDate.Value) cssClass = "ri";

        // Build date list
        if (cssClass.Length == 0)
        {
          if (blindDate.HasValue)
            dateList.Add(new RoadMapDate(blindDate.Value, "rb"));

          if (liveDate.HasValue)
            dateList.Add(new RoadMapDate(liveDate.Value, "rl"));

          if (fullDate.HasValue && announcementDate.HasValue && !blindDate.HasValue && announcementDate.Value > fullDate.Value)
            dateList.Add(new RoadMapDate(fullDate.Value, "rg"));
          else if (fullDate.HasValue && announcementDate.HasValue && blindDate.HasValue && announcementDate.Value > fullDate.Value)
            dateList.Add(new RoadMapDate(fullDate.Value, "rb"));
          else if (fullDate.HasValue)
            dateList.Add(new RoadMapDate(fullDate.Value, "rl"));

          if (announcementDate.HasValue)
            dateList.Add(new RoadMapDate(announcementDate.Value, "rl"));

          if (obsoleteDate.HasValue && removalDate.HasValue && (fullDate.HasValue || announcementDate.HasValue) && removalDate.Value > obsoleteDate.Value)
            dateList.Add(new RoadMapDate(obsoleteDate.Value, "rl"));
          else if (obsoleteDate.HasValue && removalDate.HasValue && !fullDate.HasValue && !announcementDate.HasValue && removalDate.Value > obsoleteDate.Value)
            dateList.Add(new RoadMapDate(obsoleteDate.Value, "rb"));
          else if (obsoleteDate.HasValue)
            dateList.Add(new RoadMapDate(obsoleteDate.Value, "rg"));

          if (removalDate.HasValue)
            dateList.Add(new RoadMapDate(removalDate.Value, "rg"));
        }
        else
        {
          if (blindDate.HasValue)
            dateList.Add(new RoadMapDate(blindDate.Value, cssClass));
          if (announcementDate.HasValue)
            dateList.Add(new RoadMapDate(announcementDate.Value, cssClass));
          if (fullDate.HasValue)
            dateList.Add(new RoadMapDate(fullDate.Value, cssClass));
          if (liveDate.HasValue)
            dateList.Add(new RoadMapDate(liveDate.Value, cssClass));
          if (obsoleteDate.HasValue)
            dateList.Add(new RoadMapDate(obsoleteDate.Value, cssClass));
          if (removalDate.HasValue)
            dateList.Add(new RoadMapDate(removalDate.Value, cssClass));
        }

        if (dateList != null && dateList.Count > 0)
        {
          // Sort list
          dateList = Sort(dateList);
        }

        if (dateList != null && dateList.Count > 0)
        {
          // Update css of the last date
          ((RoadMapDate)dateList[dateList.Count - 1]).CssClass = "rg";
        }
      }

      return dateList;
    }
    private string ShowHintInfo(DataRow dr)
    {
      string info = string.Empty;
      string newLine = "&#013;";
      if (dr != null)
      {
        if (dr.Table.Columns.IndexOf("ItemSku") > -1 && dr["ItemSku"] != null && dr["ItemSku"] != DBNull.Value)
          info += "[" + dr["ItemSku"].ToString() + "]" + newLine;
        if (dr.Table.Columns.IndexOf("ItemName") > -1 && dr["ItemName"] != null && dr["ItemName"] != DBNull.Value)
          info += dr["ItemName"].ToString() + newLine;
        if (dr.Table.Columns.IndexOf("CountryName") > -1 && dr["CountryName"] != null && dr["CountryName"] != DBNull.Value)
          info += dr["CountryName"].ToString().ToUpper() + newLine;

        if (info.Length > 0)
          info += newLine;

        string dateType = string.Empty;
        if (dr.Table.Columns.IndexOf("LiveDate") > -1 && dr["LiveDate"] != null && dr["LiveDate"] != DBNull.Value)
        {
          if (dr.Table.Columns.IndexOf("LiveDateType") > -1 && dr["LiveDateType"] != null && dr["LiveDateType"] != DBNull.Value)
            dateType = " (" + dr["LiveDateType"].ToString().ToUpper() + ")";

          info += "Live date: " + Convert.ToDateTime(dr["LiveDate"]).ToString(SessionState.User.FormatDate) + dateType + newLine;
        }
        if (dr.Table.Columns.IndexOf("BlindDate") > -1 && dr["BlindDate"] != null && dr["BlindDate"] != DBNull.Value)
        {
          dateType = string.Empty;
          if (dr.Table.Columns.IndexOf("BlindDateType") > -1 && dr["BlindDateType"] != null && dr["BlindDateType"] != DBNull.Value)
            dateType = " (" + dr["BlindDateType"] .ToString().ToUpper()+ ")";
          info += "Blind date: " + Convert.ToDateTime(dr["BlindDate"]).ToString(SessionState.User.FormatDate) + dateType + newLine;
        }
        if (dr.Table.Columns.IndexOf("FullDate") > -1 && dr["FullDate"] != null && dr["FullDate"] != DBNull.Value)
        {
          dateType = string.Empty;
          if (dr.Table.Columns.IndexOf("FullDateType") > -1 && dr["FullDateType"] != null && dr["FullDateType"] != DBNull.Value)
            dateType = " (" + dr["FullDateType"].ToString().ToUpper() + ")";
          info += "Full date: " + Convert.ToDateTime(dr["FullDate"]).ToString(SessionState.User.FormatDate) + dateType + newLine;
        }
        if (dr.Table.Columns.IndexOf("ObsoleteDate") > -1 && dr["ObsoleteDate"] != null && dr["ObsoleteDate"] != DBNull.Value)
        {
          dateType = string.Empty;
          if (dr.Table.Columns.IndexOf("ObsoleteDateType") > -1 && dr["ObsoleteDateType"] != null && dr["ObsoleteDateType"] != DBNull.Value)
            dateType = " (" + dr["ObsoleteDateType"].ToString().ToUpper() + ")";
          info += "Obsolete date: " + Convert.ToDateTime(dr["ObsoleteDate"]).ToString(SessionState.User.FormatDate) + dateType + newLine;
        }
        if (dr.Table.Columns.IndexOf("AnnouncementDate") > -1 && dr["AnnouncementDate"] != null && dr["AnnouncementDate"] != DBNull.Value)
        {
          dateType = string.Empty;
          if (dr.Table.Columns.IndexOf("AnnouncementDateType") > -1 && dr["AnnouncementDateType"] != null && dr["AnnouncementDateType"] != DBNull.Value)
            dateType = " (" + dr["AnnouncementDateType"].ToString().ToUpper() + ")";
          info += "Announcement date: " + Convert.ToDateTime(dr["AnnouncementDate"]).ToString(SessionState.User.FormatDate) + dateType + newLine;
        }
        if (dr.Table.Columns.IndexOf("RemovalDate") > -1 && dr["RemovalDate"] != null && dr["RemovalDate"] != DBNull.Value)
        {
          dateType = string.Empty;
          if (dr.Table.Columns.IndexOf("RemovalDateType") > -1 && dr["RemovalDateType"] != null && dr["RemovalDateType"] != DBNull.Value)
            dateType = " (" + dr["RemovalDateType"].ToString().ToUpper() + ")";
          info += "Removal date: " + Convert.ToDateTime(dr["RemovalDate"]).ToString(SessionState.User.FormatDate) + dateType + newLine;
        }

        info = info.Substring(0, (info.Length - newLine.Length));
      }

      return info;
    }
    private ArrayList Sort(ArrayList sourceList)
    {
      if (sourceList != null && sourceList.Count > 1)
      {
        ArrayList resultList = new ArrayList();
        foreach (RoadMapDate sd in sourceList)
        {
          bool isOk = false;
          int i = 0;
          while (i < resultList.Count && !isOk)
          {
            RoadMapDate rd = (RoadMapDate)resultList[i];
            if (sd.Date <= rd.Date)
            {
              resultList.Insert(i, sd);
              isOk = true;
            }
            i++;
          }
          if (!isOk)
            resultList.Add(sd);
        }
        return resultList;
      }
      else
        return sourceList;
    }
    private ArrayList RetrieveIncludePeriod(ArrayList sourceList, DateTime startD, DateTime endD)
    {
      // The source list is sorted by datetime
      ArrayList resultList = new ArrayList();
      resultList.Add(new RoadMapDate(startD, "rg")); // by default the bar is empty
      if (sourceList != null)
      {
        foreach (RoadMapDate d in sourceList)
        {
          if (d.Date <= startDate)
          {
            resultList.Clear();
            resultList.Add(new RoadMapDate(startD, d.CssClass));
          }
          else if (d.Date > startD && d.Date < endD)
          {
            resultList.Add(d);
          }
        }
      }
      resultList.Add(new RoadMapDate(endD, "rg"));

      return resultList;
    }
    private int RetrieveNbDays(DateTime startD, int nbM)
    {
      int nbDays = 0;
      DateTime curDate = startD;
      for (int i = 0; i < nbM; i++)
      {
        nbDays += DateTime.DaysInMonth(curDate.Year, curDate.Month);
        curDate.AddMonths(1);
      }
      return nbDays;
    }
    private bool HasPLC(string curItemId, string SKULevel, DataSet ds)
    {
      bool hasPLC = false;
      if (ds != null && ds.Tables.Count > 1 && SKULevel.Equals("True"))
      {
        foreach (DataRow dr in ds.Tables[1].Rows)
        {
          if (dr["ItemId"] != null && curItemId.Equals(dr["ItemId"].ToString()))
          {
            hasPLC = true;
            break;
          }
        }
      }

      return hasPLC;
    }
    private string GenerateSpaces(string curLevelId)
    {
      string spaces = string.Empty;
      if (curLevelId != null && curLevelId.Length > 0)
      {
        int levelId = Convert.ToInt32(curLevelId);
        for (int i = item.LevelId; i < levelId; i++)
        {
          spaces = "&nbsp;&nbsp;";
        }
      }
      return spaces;
    }
    #endregion

    private class RoadMapDate
    {
      private DateTime _Date;
      private string _CssClass;

      public DateTime Date
      {
        get { return _Date; }
        set { _Date = value; }
      }

      public string CssClass
      {
        get { return _CssClass; }
        set { _CssClass = value; }
      }

      public RoadMapDate(DateTime date, string cssClass)
      {
        _Date = date;
        _CssClass = cssClass;
      }
    }
  }
}
