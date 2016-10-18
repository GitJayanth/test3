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
using System.Data.SqlClient;
using Infragistics.WebUI.UltraWebGrid;
using HyperComponents.Data.dbAccess;

//Adding Export funtionality as part of PR665368 - Export Button by Nisha Verma on 21 st dec
namespace HyperCatalog.UI.Acquire.Links
{
	/// <summary>
	/// Editor to add new links
	/// </summary>
	public partial class Links_editor : HCPage
	{
		#region Declarations
		protected Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar uwToolBarTitle;

		private System.Int64 itemId = -1;
		private int linkTypeId = -1;
		private bool bLinkFrom = false;
    private int NbSKUs = 0;
    private int colErrorIndex=0;
    #endregion
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			UITools.CheckConnection(Page);
      colErrorIndex = dg.Columns.FromKey("Error").Index;
			if (SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_LINKS))
			{
                itemId = -1; // selected item
				try
				{
					// get properties
					if (Request["t"] != null)
						linkTypeId = Convert.ToInt32(Request["t"]);
					if (Request["f"] != null)
						bLinkFrom = Request["f"].ToString().Equals("1");
					if (Request["i"] != null)
					itemId = Convert.ToInt64(Request["i"]);
                    
                    //Code Added for Links Requirement (PR658943) - to load the Level dropdown on 18th Dec 2012
                    if (!Page.IsPostBack)
                    {
                        LoadLevelDDL();
                        dateValue.Value = SessionState.User.FormatUtcDate(DateTime.UtcNow);
                        //Disable Save button
                        uwToolbar.Items.FromKeyButton("Save").Enabled = false;

                        if (!bLinkFrom)  //Adding a Host; so disable the 'Include Obsolete' checkbox
                        {
                            CheckBox cb = (CheckBox)uwToolbar.Items.FromKeyCustom("chkObs").FindControl("chkObsolete");
                            cb.Enabled = false;
                        }
                    }

                    //Code added for Links Requirement (PR658943) - to set the min and max date of Effective Date calendar by Prachi on 15th Jan 2013 - start
                    dateValue.MinDate = SessionState.User.FormatUtcDate(DateTime.UtcNow);
                    dateValue.MaxDate = SessionState.User.FormatUtcDate(DateTime.UtcNow).AddYears(dateValue.CalendarLayout.DropDownYearsNumber);
                    //Code added for Links Requirement (PR658943) - to set the min and max date of Effective Date calendar by Prachi on 15th Jan 2013 - end

                    if (rblInput.SelectedValue.ToUpper().Equals("NAME"))
                    {
                        ddlLevel.Enabled = true;
                        lbLevel.Enabled = true;
                    }
                    else
                    {
                        ddlLevel.Enabled = false;
                        lbLevel.Enabled = false;
                    }
				}
				catch (FormatException fe)
				{
					UITools.DenyAccess(DenyMode.Popup);
				}     
			}
			else
			{
				UITools.DenyAccess(DenyMode.Popup);
			}
		}
        private void Analyze()
        {
            lbError.Visible = false;
            dg.Visible = false;
            bool showObsolete;

            string separator = rbList.SelectedValue;

            //Code modified for Links Requirement (PR664195) - to show/hide obsolete items based on "Include Obsolete" checkbox by Prachi on 18th Jan 2013
            if (bLinkFrom) //Analyzing List of Companion; so consider Include Obsolete option
            {
                CheckBox cb = (CheckBox)uwToolbar.Items.FromKeyCustom("chkObs").FindControl("chkObsolete");
                showObsolete = cb.Checked;
            }
            else
            {
                showObsolete = true;
            }

            //Code modified for Links Requirement (PR658943) - to allow analyze for higher level Node Names by Prachi on 19th Jan 2013
            string inputType = rblInput.SelectedValue.ToUpper();
            int levelId;
            if (inputType.Equals("NAME"))
                levelId = Convert.ToInt32(ddlLevel.SelectedValue);
            else
                levelId = 7; //In case of "SKU" selected in Input Type

            try
            {
                DataSet ds = Link.LinkAnalyze(itemId, SessionState.Culture.Code, txtSKUs.Text, Convert.ToByte(bLinkFrom), linkTypeId, separator, Convert.ToByte(showObsolete), inputType, levelId);

                if (ds == null || ds.Tables.Count == 0)
                {
                    lbError.CssClass = "hc_error";
                    lbError.Text = "Error in retrieving the Dataset";
                    lbError.Visible = true;
                }
                else
                {
                    // initialize
                    NbSKUs = 0;
                    if (inputType.Equals("SKU"))
                        txtSKUs.Text = string.Empty;

                    // Display grid
                    if (ds != null)
                    {
                        dg.DataSource = ds.Tables[0];
                        dg.DataBind();
                        dg.Visible = true;

                        ds.Dispose();
                    }

                    if (inputType.Equals("NAME"))
                    {
                        LoadLevelDDL();
                        lbLevel.Enabled = true;
                        ddlLevel.Enabled = true;
                        ddlLevel.Items.Remove("Disabled");
                        ddlLevel.SelectedValue = Convert.ToString(levelId);
                    }

                    // User can or cannot save
                    bool canProceed = false;
                    bool isHeaderChecked = false;
                    if (txtSKUs.Text.Length > 0)
                        txtSKUs.Text = txtSKUs.Text.Substring(0, txtSKUs.Text.Length);

                    foreach (UltraGridRow r in dg.Rows)
                    {
                        if ((r.Cells[colErrorIndex].Text == string.Empty) || (Convert.ToInt32(r.Cells.FromKey("SameLink").Value) == 2 && Convert.ToBoolean(r.Cells.FromKey("IsRelevant").Text)))
                        {
                            canProceed = true;
                            break;
                        }
                    }

                    TemplatedColumn colH = (TemplatedColumn)dg.Columns.FromKey("Select");
                    CheckBox cbH = (CheckBox)colH.HeaderItem.FindControl("g_ca");
                    // Display or not Save button
                    if (canProceed)
                    {
                        Ultrawebtoolbar2.Items.FromKeyButton("Export").Enabled = true;
                        cbH.Enabled = true;
                    }
                    else
                    {
                        Ultrawebtoolbar2.Items.FromKeyButton("Export").Enabled = true;
                        cbH.Enabled = false;
                    }

                    //selecting/unselecting the header checkbox
                    foreach (UltraGridRow r in dg.Rows)
                    {
                        CheckBox cbItem = (CheckBox)((CellItem)colH.CellItems[r.Index]).FindControl("g_sd");
                        if (!cbItem.Enabled && cbItem.Checked)
                        {
                            isHeaderChecked = true;
                        }
                        else if (cbItem.Enabled)
                        {
                            if (!cbItem.Checked)
                            {
                                isHeaderChecked = false;
                                break;
                            }
                            else
                                isHeaderChecked = true;
                        }
                    }
                    if (isHeaderChecked)
                        cbH.Checked = true;

                    // Count of SKUs to save
                    if (NbSKUs > 0)
                        LbNbSKUs.Text = NbSKUs.ToString() + "/" + dg.Rows.Count.ToString() + " component(s) found. Please make a selection and Click Save.";
                    else
                        LbNbSKUs.Text = "The analyze provided 0 possible components. One component at least is required. Please retry or cancel.";

                    LbNbSKUs.Text = "<br>" + LbNbSKUs.Text;
                    LbNbSKUs.Visible = true;

                    if (!bLinkFrom)  // Informative Message should be displayed only in case of Add Host
                    {
                        AddHostCnt.Value = NbSKUs.ToString();
                        lbMsg.Text = "You are trying to create " + AddHostCnt.Value + " link(s). These links will only be manageable at product level.";
                    }
                    else
                        AddHostCnt.Value = "-1";
                }
            }
            catch (Exception e)
            {
                lbError.CssClass = "hc_error";
                lbError.Text = "Input String is not in correct Format! Or separator is not used properly!";
                lbError.Visible = true;
                LbNbSKUs.Visible = false;
                Ultrawebtoolbar2.Items.FromKeyButton("Export").Enabled = false;
            }
        }

		private void Save()
		{
			if (dg!=null)
			{
				bool isOk = false;
                TemplatedColumn col = (TemplatedColumn)dg.Columns.FromKey("Select");

				foreach (UltraGridRow r in dg.Rows)
				{
                    //Code modified for Links Requirement (PR658943) - to create links for the selected Items by Prachi on 20th Jan 2013
					int isRelevant = Convert.ToInt32(r.Cells.FromKey("IsRelevant").Value);
                    int SameValidLink = Convert.ToInt32(r.Cells.FromKey("SameLink").Value);
                    CheckBox cb = (CheckBox)((CellItem)col.CellItems[r.Index]).FindControl("g_sd");

                    if (((isRelevant == 1) || (SameValidLink == 2)) && cb.Enabled && cb.Checked)
					{
						System.Int64 mainId = -1;
						System.Int64 subId = Convert.ToInt64(r.Cells.FromKey("ItemId").Value);
                        if (!bLinkFrom) // for links From -- Adding a Host
                        {
                            mainId = Convert.ToInt64(r.Cells.FromKey("ItemId").Value);
                            //Modified for Links Requirement (PR664364) - to add Host from Companions - Companion should always be SKU Level Item
                            subId = Convert.ToInt64(r.Cells.FromKey("MainItemId").Value);
                        }
                        else // for links To -- Adding a Companion
						{
							mainId = itemId;
							subId = Convert.ToInt64(r.Cells.FromKey("ItemId").Value);
						}
                        //prachi 5/5/2016 CR#6914

                            if (string.IsNullOrEmpty(Convert.ToString(dateValue.Value)))
                        {

                            dateValue.Value = DateTime.Now.ToUniversalTime().Date;
                        }
                            //prachi 5/5/2016 CR#6914

						Link link = new Link(mainId, subId, linkTypeId, SessionState.Culture.CountryCode, -1, SessionState.User.Id, DateTime.UtcNow, Convert.ToDateTime(dateValue.Value).ToUniversalTime().Date);
						if (!link.Save())
						{
							lbError.CssClass = "hc_error";
							lbError.Text = Link.LastError;
							lbError.Visible = true;

							isOk = false;
							return;
						}
						else
							isOk = true;
					}
				}

				if (isOk)
				{
					lbError.CssClass = "hc_success";
					lbError.Text = "Data saved!";
					lbError.Visible = true;
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "close", "<script>ReloadParent();</script>");
                }
            }
        }
        //Code Added for Links Requirement (PR658943) - to load the Level dropdown on 18th Dec 2012
        private void LoadLevelDDL()
        {
            int configLimit;
            using (LinkType linkType = HyperCatalog.Business.LinkType.GetByKey(linkTypeId))
            {
                configLimit = linkType.GetConfigLimit(itemId, Convert.ToByte(bLinkFrom));
            }
            Database dbObj;
            using (dbObj = Utils.GetMainDB())
            {

                using (DataSet ds = dbObj.RunSQLReturnDataSet("select LevelId, LevelName from dbo.ItemLevels where LevelId between " + configLimit + " and 7"))
                {
                    dbObj.CloseConnection();
                    ddlLevel.DataSource = ds.Tables[0];
                    ddlLevel.DataTextField = "LevelName";
                    ddlLevel.DataValueField = "LevelId";
                    ddlLevel.DataBind();

                }
            }
        }
        
        
    #region "Event methods"
    protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();

      if (btn.Equals("save"))
      {
        Save();
      }
      else if (btn.Equals("analyze"))
      {
        Analyze();
      }
    }

    protected void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
        //Code modified for Links Requirement (PR658943) - to allow analyze for higher level Node Names by Prachi on 19th Jan 2013 - start
        string inputType = rblInput.SelectedValue.ToUpper();
        string separator = rbList.SelectedValue; // separator

        TemplatedColumn col = (TemplatedColumn)e.Row.Cells.FromKey("Select").Column;
        CheckBox cb = (CheckBox)((CellItem)col.CellItems[e.Row.Index]).FindControl("g_sd");
        if (e.Row.Cells.FromKey("IsRelevant").Value != null)
        {
            cb.Enabled = Convert.ToBoolean(e.Row.Cells.FromKey("IsRelevant").Text);

            if (Convert.ToInt16(e.Row.Cells.FromKey("FlagExists").Value) == 1 && Convert.ToInt16(e.Row.Cells.FromKey("SameLink").Value) == 1)
            {
                cb.Checked = Convert.ToBoolean(e.Row.Cells.FromKey("FlagExists").Text);
            }
        }

        //Added for Links Requirement (PR664364) - to add Host from Companions - Companion should always be SKU Level Item - start
        if (!bLinkFrom) // for links From -- Adding a Host
        {
            dg.Columns.FromKey("ClassName").Hidden = true;
            dg.Columns.FromKey("MainItemPath").Hidden = false;
            dg.Columns.FromKey("ItemPath").Hidden = true;
            dg.Columns.FromKey("ItemName").Hidden = false;
        }
        else  // for links To -- Adding a Companion
        {
            dg.Columns.FromKey("ClassName").Hidden = false;
            dg.Columns.FromKey("MainItemPath").Hidden = true;
        }
        //Added for Links Requirement (PR664364) - to add Host from Companions - Companion should always be SKU Level Item - end

        if (inputType.Equals("SKU"))
        {
            dg.Columns.FromKey("ItemPath").Hidden = true;
            dg.Columns.FromKey("ItemName").Hidden = false;

            // If the proposed Sku is incorrect, change the cell style
            if (e.Row.Cells[colErrorIndex].Value != null && e.Row.Cells[colErrorIndex].Value.ToString().Length > 0)
            {
                if (!Convert.ToBoolean(e.Row.Cells.FromKey("IsRelevant").Value))
                {
                    e.Row.Cells.FromKey("ItemName").Style.CssClass = "hc_error";
                }

                if (!Convert.ToBoolean(e.Row.Cells.FromKey("IsRelevant").Value) && Convert.ToInt32(e.Row.Cells.FromKey("SameLink").Value) != 2)
                {
                    e.Row.Cells.FromKey("ItemName").Text = "<b>[" + e.Row.Cells.FromKey("ItemNumber").Text + "]</b> - " + e.Row.Cells[colErrorIndex].Value.ToString();
                }
                else
                {
                    e.Row.Cells.FromKey("ItemName").Text = e.Row.Cells.FromKey("ItemName").Text + " <b>[" + e.Row.Cells.FromKey("ItemNumber").Text + "] - " + e.Row.Cells[colErrorIndex].Value.ToString() + "</b>";
                }

                if (Convert.ToInt32(e.Row.Cells.FromKey("SameLink").Value) == 2 && Convert.ToBoolean(e.Row.Cells.FromKey("IsRelevant").Text))
                {
                    if (txtSKUs.Text.Length > 0 && !txtSKUs.Text.Contains(e.Row.Cells.FromKey("ItemNumber").Text))
                    {
                        if (separator.Equals("R"))
                            txtSKUs.Text += "\r\n";
                        else
                            txtSKUs.Text += separator;
                    }
                    if (!txtSKUs.Text.Contains(e.Row.Cells.FromKey("ItemNumber").Text))
                        txtSKUs.Text += e.Row.Cells.FromKey("ItemNumber").Text;
                    NbSKUs++;
                }
            }
            else
            {
                e.Row.Cells.FromKey("ItemName").Text = e.Row.Cells.FromKey("ItemName").Text + " <b>[" + e.Row.Cells.FromKey("ItemNumber").Text + "]</b>";

                // Update list of SKU
                if (txtSKUs.Text.Length > 0 && !txtSKUs.Text.Contains(e.Row.Cells.FromKey("ItemNumber").Text))
                {
                    if (separator.Equals("R"))
                        txtSKUs.Text += "\r\n";
                    else
                        txtSKUs.Text += separator;
                }
                if (!txtSKUs.Text.Contains(e.Row.Cells.FromKey("ItemNumber").Text))
                    txtSKUs.Text += e.Row.Cells.FromKey("ItemNumber").Text;
                NbSKUs++;
            }
        }
        else if (inputType.Equals("NAME"))
        {
            if (bLinkFrom)  //Adding a companion
            {
                dg.Columns.FromKey("ItemName").Hidden = true;
                dg.Columns.FromKey("ItemPath").Hidden = false;
            }

            // If the Sku is incorrect, change the cell style
            if (e.Row.Cells[colErrorIndex].Value != null && e.Row.Cells[colErrorIndex].Value.ToString().Length > 0)
            {
                if (bLinkFrom)
                {
                    if (!Convert.ToBoolean(e.Row.Cells.FromKey("IsRelevant").Value))
                    {
                        e.Row.Cells.FromKey("ItemPath").Style.CssClass = "hc_error";
                    }
                    if (!Convert.ToBoolean(e.Row.Cells.FromKey("IsRelevant").Value) && Convert.ToInt32(e.Row.Cells.FromKey("SameLink").Value) != 2)
                    {
                        if (e.Row.Cells.FromKey("ItemNumber").Value.ToString().Length > 0)
                        {
                            e.Row.Cells.FromKey("ItemPath").Text = e.Row.Cells.FromKey("ItemPath").Text + " <b>[" + e.Row.Cells.FromKey("ItemNumber").Text + "]</b> - " + e.Row.Cells[colErrorIndex].Value.ToString();
                        }
                        else
                        {
                            e.Row.Cells.FromKey("ItemPath").Text = e.Row.Cells.FromKey("ItemPath").Text + " - " + e.Row.Cells[colErrorIndex].Value.ToString();
                        }
                    }
                    else
                    {
                        e.Row.Cells.FromKey("ItemPath").Text = e.Row.Cells.FromKey("ItemPath").Text + " <b>[" + e.Row.Cells.FromKey("ItemNumber").Text + "] - " + e.Row.Cells[colErrorIndex].Value.ToString() + "</b>";
                    }
                }
                else
                {
                    if (!Convert.ToBoolean(e.Row.Cells.FromKey("IsRelevant").Value))
                    {
                        e.Row.Cells.FromKey("ItemName").Style.CssClass = "hc_error";
                    }

                    if (e.Row.Cells.FromKey("ItemNumber").Value != null && e.Row.Cells.FromKey("ItemNumber").Value.ToString().Length > 0)
                    {
                        e.Row.Cells.FromKey("ItemName").Text = e.Row.Cells.FromKey("ItemName").Text + " <b>[" + e.Row.Cells.FromKey("ItemNumber").Text + "]</b> - " + e.Row.Cells[colErrorIndex].Value.ToString();
                    }
                    else
                    {
                        e.Row.Cells.FromKey("ItemName").Text = e.Row.Cells.FromKey("ItemName").Text + " - " + e.Row.Cells[colErrorIndex].Value.ToString();
                    }
                }

                if (Convert.ToInt32(e.Row.Cells.FromKey("SameLink").Value) == 2 && Convert.ToBoolean(e.Row.Cells.FromKey("IsRelevant").Text))
                    NbSKUs++;
            }
            else
            {
                if (bLinkFrom)
                    e.Row.Cells.FromKey("ItemPath").Text = e.Row.Cells.FromKey("ItemPath").Text + " <b>[" + e.Row.Cells.FromKey("ItemNumber").Text + "]</b>";
                else
                {
                    if (e.Row.Cells.FromKey("ItemNumber").Value != null && e.Row.Cells.FromKey("ItemNumber").Value.ToString().Length > 0)
                    {
                        e.Row.Cells.FromKey("ItemName").Text = e.Row.Cells.FromKey("ItemName").Text + " <b>[" + e.Row.Cells.FromKey("ItemNumber").Text + "]</b>";
                    }
                    else
                    {
                        e.Row.Cells.FromKey("ItemName").Text = e.Row.Cells.FromKey("ItemName").Text;
                    }
                }
                NbSKUs++;
            }
        }
        //Code modified for Links Requirement (PR658943) - to allow analyze for higher level Node Names by Prachi on 19th Jan 2013 - end
    }
    #endregion

    //Adding Export funtionality as part of PR665368 - Export Button by Nisha Verma on 21 st dec 
    protected void Ultrawebtoolbar2_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
        string btn1 = be.Button.Key.ToLower();

        if (btn1 == "export")
        {
            Utils.ExportToExcel(dg, SessionState.CurrentItem.Name.ToString() + "_Analyze Result", SessionState.CurrentItem.Name.ToString() + "_Analyze Result");

        }
    }
  }
}
