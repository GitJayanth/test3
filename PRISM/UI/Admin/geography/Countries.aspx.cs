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
using Infragistics.WebUI.UltraWebGrid;
using Infragistics.WebUI.WebDataInput;
using HyperCatalog.Business;

namespace HyperCatalog.UI.Admin.Architecture
{
    /// <summary>
    /// Description résumée de Countries.
    /// </summary>
    public partial class Countries : HCPage
    {
        #region Business objects
        private Business.Region rootRegion = Business.Region.GetRootRegion();
        private DataSet ds;

        private HyperCatalog.Business.Country _currentCountry;
        protected HyperCatalog.Business.Country currentCountry
        {
            get
            {
                if (ViewState["currentCountryCode"] == null)
                {
                    Debug.Trace("<br>CurrentCountry is null", DebugSeverity.Low);
                }
                else
                {
                    Debug.Trace("<br>CurrentCountry is " + ViewState["currentCountryCode"].ToString(), DebugSeverity.Low);
                }

                if (_currentCountry == null && ViewState["currentCountryCode"] != null)
                    _currentCountry = HyperCatalog.Business.Country.GetByKey((string)ViewState["currentCountryCode"]);
                return _currentCountry;
            }
            set
            {
                if (value != null)
                {
                    Debug.Trace("<br>SET COUNTRY for " + value.Code, DebugSeverity.Low);
                    ViewState["currentCountryCode"] = value.Code;
                    _currentCountry = value;
                }
                else
                {
                    Debug.Trace("<br>SET COUNTRY to NULL", DebugSeverity.Low);
                    ViewState["currentCountryCode"] = string.Empty;
                    _currentCountry = null;
                }
            }
        }

        private HyperCatalog.Business.CountryList _countries;
        private HyperCatalog.Business.CountryList countries
        {
            get
            {
                if (_countries == null)
                    _countries = regionList.SelectedValue != string.Empty ? Business.Region.GetByKey(regionList.SelectedValue).TotalCountries : HyperCatalog.Business.Country.GetActiveCountries();
                Debug.Trace("<br>Region= " + regionList.SelectedValue + ", nbCountries = " + _countries.Count.ToString(), DebugSeverity.Low);
                return _countries;
            }
        }

        #endregion

        protected override void OnLoad(EventArgs e)
        {
            UITools.CheckConnection(Page);
            propertiesMsgLbl.Visible = mainMsgLbl.Visible = false;

            //when first loading this page
            if (!IsPostBack)
            {
                loadRegionList();

                //selected region can be the one selected in the list or taken from the url parameters 
                if (Request["r"] != null)
                {
                    regionList.SelectedValue = Request["r"];
                    //keep region selection for the "add new country" interface
                    txtRegionCodeValue.SelectedValue = Request["r"];
                }

                loadCountries();
            }

            base.OnLoad(e);
        }
        protected override void OnPreRender(EventArgs e)
        {
            mainToolBar.Items.FromKeyButton("Add").Enabled =
              propertiesToolBar.Items.FromKeyButton("Delete").Enabled =
              propertiesToolBar.Items.FromKeyButton("Save").Enabled =
              !HyperCatalog.Shared.SessionState.User.IsReadOnly && HyperCatalog.Shared.SessionState.User.HasCapability(Business.CapabilitiesEnum.MANAGE_USERS) && rootRegion != null;

            base.OnPreRender(e);
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
            regionList = (DropDownList)mainToolBar.Items.FromKeyCustom("RegionFilter").FindControl("regionList");
            if (regionList != null)
                regionList.SelectedIndexChanged += new EventHandler(regionList_SelectedIndexChanged);
            this.mainToolBar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.mainToolBar_ButtonClicked);
            this.propertiesToolBar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(propertiesToolBar_ButtonClicked);
        }
        #endregion

        #region Data load & bind
        /// <summary>
        /// Load region lists
        /// </summary>
        private void loadRegionList()
        {
            //empty the current lists of regions
            regionList.Items.Clear();
            txtRegionCodeValue.Items.Clear();

            //add recursively each region from the root one
            loadRegionItem(HyperCatalog.Business.Region.GetRootRegion(), Server.HtmlDecode("&nbsp;"));

            //add the "no-selection" region at the top
            regionList.Items.Insert(0, new ListItem("-- All regions  --", ""));
        }

        /// <summary>
        /// Recursively add regions in the list of regions
        /// </summary>
        /// <param name="region">Current region to add</param>
        /// <param name="offset">Text offset to prefix the region name (responsible for indentation along the hierarchy)</param>
        private void loadRegionItem(HyperCatalog.Business.Region region, string initialOffset)
        {
            loadRegionItem(region, initialOffset, string.Empty);
        }
        private void loadRegionItem(HyperCatalog.Business.Region region, string initialOffset, string offset)
        {
            if (region != null && regionList != null)
            {
                //add the region in the lists
                regionList.Items.Add(new ListItem(offset + (region.Name == null || region.Name.Equals(string.Empty) ? region.Code : region.Name) + " (" + region.TotalCountryCount + ")", region.Code));
                txtRegionCodeValue.Items.Add(new ListItem(region.Name == null || region.Name.Equals(string.Empty) ? region.Code : region.Name, region.Code));

                //load child regions
                foreach (HyperCatalog.Business.Region subRegion in region.SubRegions)
                    loadRegionItem(subRegion, initialOffset, offset + initialOffset);
            }
        }

        /// <summary>
        /// Load country list
        /// </summary>
        private void loadCountries()
        {
            _countries = null;

            //empty country list
            countriesGrid.Rows.Clear();

            foreach (HyperCatalog.Business.Country country in countries)
            {
                HyperCatalog.Business.Region region = country.Region;

                //build a new row for this country
                UltraGridRow newRow = new UltraGridRow(new object[] { country.Code, country.Name, region != null ? region.Code + (region.Name != null ? " - " + region.Name : "") : "" });
                newRow.DataKey = country.Code;

                //add this row to the grid
                countriesGrid.Rows.Add(newRow);
            }
        }
        #endregion

        #region Events
        #region Toolbar events
        private void mainToolBar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
        {
            switch (be.Button.Key)
            {
                case "Add":
                    //display "add new country" interface
                    UpdateDataEdit(string.Empty);
                    setPropertiesVisible(true);
                    break;
                case "Export":
                    Utils.ExportToExcel(countriesGrid, "Countries", "Countries");
                    break;
            }
        }

        private void propertiesToolBar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
        {
            switch (be.Button.Key)
            {
                case "List":
                    //close "add new country" interface
                    setPropertiesVisible(false);
                    break;
                case "Delete":
                    if (!HyperCatalog.Shared.SessionState.User.IsReadOnly && HyperCatalog.Shared.SessionState.User.HasCapability(Business.CapabilitiesEnum.MANAGE_USERS))
                    {
                        if (currentCountry != null)
                        {
                            if (currentCountry.Delete())
                            {
                                Tools.UITools.SetMessage(mainMsgLbl, "Country \"" + txtCountryCodeValue.Text + "\" deleted.", Tools.UITools.MessageLevel.Information);
                                setPropertiesVisible(false);
                                loadCountries();
                            }
                            else
                            {
                                Tools.UITools.SetMessage(propertiesMsgLbl, HyperCatalog.Business.Country.LastError, Tools.UITools.MessageLevel.Error);
                            }
                        }
                    }
                    break;
                case "Save":
                    if (!HyperCatalog.Shared.SessionState.User.IsReadOnly && HyperCatalog.Shared.SessionState.User.HasCapability(Business.CapabilitiesEnum.MANAGE_USERS))
                    {
                        if (currentCountry != null)
                        {
                            if (currentCountry.Update(txtCountryCodeValue.Text, txtCountryNameValue.Text, txtRegionCodeValue.SelectedValue, cbIsActive.Checked, cbEditPLC.Checked, cbEditContent.Checked, cbEditCS.Checked, cbEditMS.Checked, cbEditCP.Checked, cbEditPublishers.Checked, cbTranslationPLC.Checked, cbPublishable.Checked, cbFallBackToEnglish.Checked))
                            {
                                if (SavePassiveApproval())
                                {
                                    //                  Tools.UITools.SetMessage(mainMsgLbl, "Country \"" + txtCountryCodeValue.Text + "\" updated.", Tools.UITools.MessageLevel.Information);
                                    Tools.UITools.SetMessage(propertiesMsgLbl, "Country \"" + txtCountryCodeValue.Text + "\" updated.", Tools.UITools.MessageLevel.Information);
                                    //setPropertiesVisible(false);
                                    loadCountries();
                                }
                            }
                            else
                            {
                                Tools.UITools.SetMessage(propertiesMsgLbl, HyperCatalog.Business.Country.LastError, Tools.UITools.MessageLevel.Error);
                            }
                        }
                        else if (rootRegion != null)
                        {
                            if (txtCountryCodeValue.Text != string.Empty)
                            {
                                if (!rootRegion.ContainsCountry(txtCountryCodeValue.Text))
                                {
                                    Country country = HyperCatalog.Business.Region.GetByKey(txtRegionCodeValue.SelectedValue).Countries.Add(txtCountryCodeValue.Text, txtCountryNameValue.Text, txtRegionCodeValue.SelectedValue, cbIsActive.Checked, cbEditPLC.Checked, cbEditContent.Checked, cbEditCS.Checked, cbEditMS.Checked, cbEditCP.Checked, cbEditPublishers.Checked, cbTranslationPLC.Checked,cbPublishable.Checked, cbFallBackToEnglish.Checked);
                                    string errorMessage = Country.LastError;
                                    currentCountry = country;
                                    if ((currentCountry != null) & (errorMessage == string.Empty))
                                    {
                                        if (SavePassiveApproval())
                                        {
                                            Tools.UITools.SetMessage(mainMsgLbl, "Country \"" + txtCountryCodeValue.Text + "\" created with success.", Tools.UITools.MessageLevel.Information);
                                            setPropertiesVisible(false);
                                            loadCountries();
                                        }
                                    }
                                    else
                                    {
                                        Tools.UITools.SetMessage(propertiesMsgLbl, errorMessage, Tools.UITools.MessageLevel.Error);
                                        txtCountryCodeValue.Enabled = true;
                                    }
                                }
                                else
                                {
                                    Tools.UITools.SetMessage(propertiesMsgLbl, "This country code is already used for another country.", Tools.UITools.MessageLevel.Warning);
                                    txtCountryCodeValue.Enabled = true;
                                }
                            }
                            else
                                Tools.UITools.SetMessage(propertiesMsgLbl, "You must provide a country code.", Tools.UITools.MessageLevel.Warning);
                        }
                        else
                            Tools.UITools.SetMessage(propertiesMsgLbl, "You need to create at least one region before creating countries.", Tools.UITools.MessageLevel.Warning);
                    }
                    else
                        Tools.UITools.SetMessage(propertiesMsgLbl, "You are not allowed to create or modify countries.", Tools.UITools.MessageLevel.Warning);
                    break;
            }
        }
        #endregion

        #region Grid events
        protected void UpdateGridItem(object sender, System.EventArgs e)
        {
            Infragistics.WebUI.UltraWebGrid.CellItem cellItem = ((Infragistics.WebUI.UltraWebGrid.CellItem)((System.Web.UI.WebControls.LinkButton)sender).Parent);
            Debug.Trace("<br>UpdateGridItem for " + cellItem.Cell.Row.Cells.FromKey("CountryCode").Text, DebugSeverity.Low);
            UpdateDataEdit(cellItem.Cell.Row.Cells.FromKey("CountryCode").Text);
        }
        private void UpdateDataEdit(string countryCode)
        {
            if (countryCode != string.Empty)
            {
                currentCountry = Business.Country.GetByKey(countryCode);
            }
            else
            {
                currentCountry = null;
            }

            txtRegionCodeValue.ClearSelection();
            //using (ItemList classes = Item.GetAll("LevelId=1"))
            //{

            //  classes.Sort("Sort");
            //  dgPVGrid.DataSource = classes;
            using (HyperComponents.Data.dbAccess.Database dbObj = Utils.GetMainDB())
            {
                //using (ds = dbObj.RunSQLReturnDataSet("SELECT * From CountryApprovalTrigger WHERE CountryCode = @CountryCode", HyperComponents.Data.dbAccess.Database.NewSqlParameter("@CountryCode", System.Data.SqlDbType.NVarChar, 50, countryCode)))
                //{
                string sql = "select I.ItemId as ClassId, dbo.GetItemName(I.ItemId) as [PRODUCT TYPE NAME] from  Items I where I.LevelId = 1    ";
                //sql += "SELECT * From CountryApprovalTrigger WHERE CountryCode = '" + countryCode + "'";
                sql += "SELECT W.ItemId AS ClassId, W.PLCode AS PLCode, W.PLCode +' [' + BP.PLName + ']' AS ProductLine, CA.PassiveApprovalDay as [Trigger],CA.ActiveApproval as IsActive FROM Work_Item_ProductLines W ";
                sql += "INNER JOIN Items I ON I.ItemId = W.ItemId AND I.LevelId = 1 ";
                sql += "LEFT OUTER JOIN [CountryApprovalTrigger] CA ON CA.ClassId = W.ItemId AND CA.PLCode = W.PLCode AND CA.CountryCode = '" + countryCode + "' ";
                sql += "INNER JOIN BPL BP ON BP.PLCode = W.PLCode";

                //using (ds = dbObj.RunSQLReturnDataSet(sql.ToString()))
                //{
                //dbObj.CloseConnection();
                ds = dbObj.RunSQLReturnDataSet(sql.ToString());
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataColumn parentColumn;
                    DataColumn childColumn;
                    DataRelation relation;
                    parentColumn = ds.Tables[0].Columns["ClassId"];

                    childColumn = ds.Tables[1].Columns["ClassId"];


                    relation = new System.Data.DataRelation("ClassId", parentColumn, childColumn);
                    ds.Relations.Add(relation);
                }
            }
            if (currentCountry != null)
            {
                Debug.Trace("<br>txtCountryNameValue.Text=" + currentCountry.Name + ", txtRegionCodeValue.SelectedValue = " + currentCountry.RegionCode, DebugSeverity.Low);

                txtCountryNameValue.Text = currentCountry.Name;
                txtCountryCodeValue.Text = currentCountry.Code;
                cbEditContent.Checked = currentCountry.CanLocalizeContent;
                cbEditCP.Checked = currentCountry.CanCreateProductInLocalLanguage;
                cbEditPLC.Checked = currentCountry.CanLocalizePLC;
                cbEditMS.Checked = currentCountry.CanLocalizeMarketSegments;
                cbEditCS.Checked = currentCountry.CanLocalizeCrossSells;
                cbEditPublishers.Checked = currentCountry.CanLocalizePublishers;
                cbTranslationPLC.Checked = currentCountry.PLCDrivenTranslation;
                cbIsActive.Checked = currentCountry.IsActive;
                cbPublishable.Checked = currentCountry.Publishable;
                cbFallBackToEnglish.Checked = currentCountry.FallBackToEnglish;
                txtRegionCodeValue.SelectedValue = currentCountry.RegionCode;
                _countries = Business.Region.GetByKey(currentCountry.RegionCode).Countries;
                txtCountryCodeValue.Enabled = false;

                UITools.ShowToolBarButton(propertiesToolBar, "Delete");
                UITools.ShowToolBarSeparator(propertiesToolBar, "DeleteSep");
            }
            else
            {
                cbIsActive.Checked = true;
                txtCountryNameValue.Text = null;
                txtCountryCodeValue.Text = null;
                UITools.HideToolBarButton(propertiesToolBar, "Delete");
                UITools.HideToolBarSeparator(propertiesToolBar, "DeleteSep");
                if (regionList.SelectedValue != string.Empty)
                    txtRegionCodeValue.SelectedValue = regionList.SelectedValue;
                txtCountryCodeValue.Enabled = true;
            }
            dgPVGrid.DataSource = ds.Tables[0].DefaultView;
            //dgPVGrid.DataSource = ds;
            dgPVGrid.DataBind();
            dgPVGrid.Height = Unit.Empty;
            dgPVGrid.Width = Unit.Empty;
            Utils.InitGridSort(ref dgPVGrid, false);
            foreach(UltraGridColumn use1 in dgPVGrid.Bands[1].Columns)
            {
                Response.Write(use1.Header.Title);

            }
            //}
            dgPVGrid.Bands[0].Columns[2].Hidden = true;
            dgPVGrid.Bands[1].Columns[5].Hidden = true;
            setPropertiesVisible(true);
        }
        #endregion

        private void regionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadCountries();
        }
        #endregion

        #region Private methods
        private void setPropertiesVisible(bool visible)
        {
            panelGrid.Visible = !visible;
            panelProperties.Visible = visible;
        }
        #endregion

        protected void dgPVGrid_InitializeRow(object sender, RowEventArgs e)
        {
            if (e.Row.Band.Key == "Table1")
            {
               
            }
        }
        private bool SavePassiveApproval()
        {
            string countryCode = txtCountryCodeValue.Text;
            bool success = true;
            if (dgPVGrid != null && dgPVGrid.Rows != null && dgPVGrid.Rows.Count > 0)
            {
                using (HyperComponents.Data.dbAccess.Database dbObj = Utils.GetMainDB())
                {                   

                    System.Text.StringBuilder sbSQL = new System.Text.StringBuilder(string.Empty);
                    string classId = string.Empty;
                    string PLCode = string.Empty;
                    bool isActive = false;
                    int PassiveApprovalDay = 0;
                    bool ActiveApproval = false;
                    
                    UltraGridRowsEnumerator ub = dgPVGrid.Bands[1].GetRowsEnumerator();
                    while (ub.MoveNext())
                    {

                        UltraGridRow dr = (UltraGridRow)ub.Current;
                        classId = dr.Cells.FromKey("ClassId").ToString();
                        if (dr.Cells.FromKey("Trigger").Value != null)
                        {
                            PassiveApprovalDay = (int)dr.Cells.FromKey("Trigger").Value;
                        }
                        else
                        {
                            PassiveApprovalDay = 0;
                        }
                        
                        if (dr.Cells.FromKey("IsActive") != null)
                        {
                            ActiveApproval = (bool)dr.Cells.FromKey("IsActive").Value;
			    
			    //Included for the QC 6660 for chardonnay release
                            if (ActiveApproval == true)
                            {
                                dr.Cells.FromKey("Trigger").Reset();
                            }	
                        }
                        else
                        {
                            ActiveApproval = false;
                        }
                        PLCode = dr.Cells.FromKey("PLCode").ToString();
			//Modified the below code to eliminate the null data in the passive trigger
                        //if (ActiveApproval == false && (PassiveApprovalDay == 0 || PassiveApprovalDay == null))
			if (ActiveApproval == false && PassiveApprovalDay == null)
                        {
                            sbSQL.Append("DELETE FROM CountryApprovalTrigger WHERE CountryCode = '");
                            sbSQL.Append(countryCode);
                            sbSQL.Append("' AND ClassId = '");
                            sbSQL.Append(classId);
                            sbSQL.Append("' AND PLCode = '");
                            sbSQL.Append(PLCode);
                            sbSQL.Append("';");
                        }
                        else //if (ActiveApproval != false || PassiveApprovalDay != 0) Commented since this condition is of no use
                        {   
							if (ActiveApproval == true)
                            {
                                PassiveApprovalDay = 0;
                            }
                            sbSQL.Append("DELETE FROM CountryApprovalTrigger WHERE CountryCode = '");
                            sbSQL.Append(countryCode);
                            sbSQL.Append("' AND ClassId = '");
                            sbSQL.Append(classId);
                            sbSQL.Append("' AND PLCode = '");
                            sbSQL.Append(PLCode);
                            sbSQL.Append("';");
                            sbSQL.Append("INSERT INTO CountryApprovalTrigger VALUES('");
                            sbSQL.Append(countryCode);
                            sbSQL.Append("', ");
                            sbSQL.Append(classId);
                            sbSQL.Append(", ");
                            sbSQL.Append(PassiveApprovalDay);
                            sbSQL.Append(",");
                            sbSQL.Append(Convert.ToByte(ActiveApproval).ToString());
                            sbSQL.Append(",'");
                            sbSQL.Append(PLCode);
                            sbSQL.Append("');");


                        }
                    }


                        if (sbSQL != null && sbSQL.Length > 0)
                        {
                            Debug.Trace("Update CountryApprovalTrigger [" + sbSQL.ToString() + "]", DebugSeverity.Low);
                            dbObj.RunSQLQuery(sbSQL.ToString());
                            if (dbObj.LastError != null && dbObj.LastError.Length > 0)
                            {
                                Tools.UITools.SetMessage(mainMsgLbl, "Country \"" + txtCountryCodeValue.Text + "\" error when updating CountryApprovalTrigger [" + dbObj.LastError + "]", Tools.UITools.MessageLevel.Error);
                                return false;
                            }
                        }
                    }
                }
                return true;
              }
        public void dgPVGrid_InitializeLayout(object sender,
Infragistics.WebUI.UltraWebGrid.LayoutEventArgs e)
        {
            dgPVGrid.Bands[0].Columns[0].Hidden = true;
            //dgPVGrid.Bands[0].Columns[1].CellStyle.Font.Size = 8;
            dgPVGrid.Bands[0].Columns[1].AllowUpdate = AllowUpdate.No;
            //dgPVGrid.Bands[0].Columns[2].Hidden = true;
            dgPVGrid.Bands[1].Columns[0].Hidden = true;
            dgPVGrid.Bands[1].Columns[1].Hidden = true;
            //dgPVGrid.Bands[1].Columns[2].Hidden = true;
            dgPVGrid.Bands[1].Columns[2].AllowUpdate = AllowUpdate.No;
            dgPVGrid.Bands[1].Columns[2].CellStyle.BorderStyle = BorderStyle.Solid;
            dgPVGrid.Bands[1].Columns[2].Width = Unit.Percentage(100);
            dgPVGrid.Bands[1].Columns[3].CellStyle.BorderStyle = BorderStyle.Ridge;
            //dgPVGrid.Bands[1].Columns[5].Hidden = true;
            //dgPVGrid.Bands[0].Columns[dgPVGrid.Bands[0].Columns.Count-1].Hidden = true;
           // dgPVGrid.Bands[1].Columns[dgPVGrid.Bands[1].Columns.Count].Hidden = true;
            dgPVGrid.DisplayLayout.TableLayout = TableLayout.Auto;
            dgPVGrid.Bands[0].CellSpacing = 5;
            dgPVGrid.Width = Unit.Empty;
           // dgPVGrid.Bands[1].Columns[2].Width = 100;
        }
    }
}
