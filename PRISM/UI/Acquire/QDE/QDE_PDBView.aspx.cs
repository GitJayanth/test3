//#region Uses
using System;
using System.Xml;
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
using HyperComponents.Data.dbAccess;
using HyperCatalog.Business;
using Infragistics.WebUI.UltraWebGrid;
using System.Data.SqlClient;
using System.Data.OracleClient;
using HyperCatalog.WebServices.EventLoggerWS;

//#endregion

namespace HyperCatalog.UI.Acquire.QDE
{

    public partial class QDE_PDBView : HCPage
    {
        #region Declarations
        protected int stat_total, stat_totalMandatory, stat_nbFinal, stat_nbDraft, stat_nbMissing, stat_nbRejected;
        protected int stat_nbFinal_inh, stat_nbDraft_inh, stat_nbRejected_inh;
        private System.Int64 itemId;
        private string currentGroup = string.Empty;
        private string currentLinkGroup = string.Empty;
        private int groupCount = 0;
        #endregion

        //#region Code généré par le Concepteur Web Form
        override protected void OnInit(EventArgs e)
        {
            txtFilter.AutoPostBack = false;
            txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);"); Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "<script defer=true>var txtFilterField;txtFilterField = document.getElementById('" + txtFilter.ClientID + "');</script>");
            base.OnInit(e);
        }
        //#endregion

        protected void Page_Load(object sender, System.EventArgs e)
        {
            // *************************************************************************
            // Retrieve Product information
            // *************************************************************************

            #region Declaration

            int InstanceId;
            string CultureCode = SessionState.Culture.Code;
            string CountryCode = SessionState.Culture.Code.Substring(0, 2).ToUpper();
            string Publish = null;
            string PublishableFlag = null;
            string MarketSegment = null;
            string ProductLine = null;
            string ItemPublish = null;
            string PDBRefresh = null;
            string PDBQuery = null;
            string Country_id = null;
            string Culture_id = null;
            OracleConnection oracleConn = new OracleConnection();
            OracleCommand sqlCountryCommand;
            OracleCommand sqlPublishableCommand;
            OracleDataReader drC;
            OracleDataReader drP;
            SessionState.QDETab = "tb_PDBVIew";
            PublishableFlag = "NO";


            #endregion Declaration

            if (Request["filter"] != null)
            { txtFilter.Text = Request["filter"].ToString(); }

            //Code modified for Links Requirement (PR664327) - to display "Republsih Links" button only for ADMIN Users by Prachi on 28th Jan 2013
	if (SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_ITEM_PDB_REPUBLISH))
            {
                uwToolbar.Items.FromKeyButton("RePublish").Visible = true;
                uwToolbar.Items.FromKeyButton("LinksRePublish").Visible = true;
            }
            else
            {
                uwToolbar.Items.FromKeyButton("RePublish").Visible = false;
                UITools.HideToolBarSeparator(uwToolbar, "RePublishSep");
                uwToolbar.Items.FromKeyButton("LinksRePublish").Visible = false;
                UITools.HideToolBarSeparator(uwToolbar, "LinksRePublishSep");
            }


            stat_total = stat_totalMandatory = stat_nbFinal = stat_nbDraft = stat_nbMissing = stat_nbRejected = 0;
            stat_nbDraft_inh = stat_nbFinal_inh = stat_nbRejected_inh = 0;
            QDEUtils.GetItemIdFromRequest();
            itemId = SessionState.CurrentItem.Id;

            hfLevel.Value = SessionState.CurrentItem.Level.Name;
            hfName.Value = SessionState.CurrentItem.Name;

         
            //  Changes incorporated for Crystal-Gemstone convergence ( Deepa , 15/03/15)
            if (CultureCode.ToUpper() == "US-EN" || CultureCode.ToUpper() == "NA-EN")
            {
                InstanceId = 1;
            }
            else
            { 
                InstanceId = 2; 
            }
           
            //  Deepa , 15/03/15 - End


            try
            {
                oracleConn.ConnectionString = SessionState.CacheParams["PDBConnectionString"].Value.ToString();
                //Opening the oracle connection.
                oracleConn.Open();

                PDBQuery = "select DISTINCT country_id  from hp_pdb_geography_" + InstanceId + "_mv where country_code = '" + CountryCode + "' and Instance_Id =  " + InstanceId + "";
                sqlCountryCommand = new OracleCommand(PDBQuery, oracleConn);
                drC = sqlCountryCommand.ExecuteReader();
                if (drC != null && drC.HasRows == true )
                {
                    drC.Read();
                    Country_id = drC[0].ToString();
                    drC.Dispose();
                    sqlCountryCommand.Dispose();

                    PDBQuery = "select DISTINCT culture_id  from hp_pdb_geography_" + InstanceId + "_mv where culture_code = '" + CultureCode + "' and Instance_Id =  " + InstanceId + "";
                    sqlCountryCommand = new OracleCommand(PDBQuery, oracleConn);
                    drC = sqlCountryCommand.ExecuteReader();
                    if (drC != null && drC.HasRows == true)
                    {
                        drC.Read();
                        Culture_id = drC[0].ToString();
                        drC.Dispose();
                        sqlCountryCommand.Dispose();

                        #region Publishable Info

                        uwToolbar.Items.FromKeyLabel("Published").Text = "Published : NO";

                        if (Country_id != null && Culture_id != null)
                        {
                            PDBQuery = GetViewQuery("Publishable");
                            //PDBQuery = " select PUBLISHABLE_FLAG from hp_pdb_item_Cultures_" + InstanceId + "_mv where Item_Id  = " + itemId + " and culture_id = " + Culture_id + " and instance_id = " + InstanceId;
                            PDBQuery = GetViewQuery("Publishable");
                            PDBQuery = PDBQuery.Replace("$INSTANCEID", InstanceId.ToString());
                            PDBQuery = PDBQuery.Replace("$ITEMID", itemId.ToString());
                            PDBQuery = PDBQuery.Replace("$CULTUREID", Culture_id);

                            sqlPublishableCommand = new OracleCommand(PDBQuery, oracleConn);
                            drP = sqlPublishableCommand.ExecuteReader();

                            if (drP != null && drP.HasRows)
                            {
                                drP.Read();
                                Publish = drP[0].ToString();
                                if (Publish == "1")
                                { PublishableFlag = "YES"; }
                                else
                                { PublishableFlag = "NO"; }
                            }
                            #region Disposing all Data Object

                            oracleConn.Close();
                            //drC.Dispose();
                            drP.Dispose();
                            ///sqlCountryCommand.Dispose();
                            sqlPublishableCommand.Dispose();

                            #endregion Disposing all Data Object
                        }

                        #endregion Publishable Info

                        if (PublishableFlag == "YES")
                        {
                            #region retrieve Product info

                            PDBView(Country_id, InstanceId);

                            #endregion retrieve Product info
                        }
                        else
                        {
                            #region Product info Unavailable

                            labelLinks.Visible = false;
                            labelPLC.Visible = false;
                            Market.Visible = false;
                            Label2.Visible = false;
                            Label3.Visible = false;
                            ProductLines.Visible = false;
                            Publishers.Visible = false;
                            Label1.Visible = false;
                            NotPublishable.Visible = true;
                            NotPublishable.Text = "The content for this node cannot be published/viewed";
                            UITools.HideToolBarButton(uwToolbar, "Export");

                            #endregion Product info Unavailable
                        }
                    }
                    else
                    {
                        #region Culture Unavailable
                        labelLinks.Visible = false;
                        labelPLC.Visible = false;
                        Market.Visible = false;
                        Label2.Visible = false;
                        Label3.Visible = false;
                        ProductLines.Visible = false;
                        Publishers.Visible = false;
                        Label1.Visible = false;
                        NotPublishable.Visible = true;
                        NotPublishable.Text = "Cannot retrieve the data for this Culture at this moment.  Please try again later";
                        HyperCatalog.WebServices.EventLoggerWS.WSEventLogger _EventLog = HyperCatalog.WebServices.WSInterface.EventLogger;
                        const int CRYSTAL_UI_COMPONENT_ID = 2;
                        Guid errorGuid = Guid.NewGuid();
                        HyperCatalog.EventLogger.EventLogger.AddEventLog(CRYSTAL_UI_COMPONENT_ID, errorGuid, 0, HyperCatalog.EventLogger.Severity.INFORMATION, "Crystal UI PDB View Error", "", "The Culture Id is not available for this Culture in the MV");
                        #endregion Culture Unavailable
                    }
                }
                else
                {
                    #region Country Unavailable
                    labelLinks.Visible = false;
                    labelPLC.Visible = false;
                    Market.Visible = false;
                    Label2.Visible = false;
                    Label3.Visible = false;
                    ProductLines.Visible = false;
                    Publishers.Visible = false;
                    Label1.Visible = false;
                    NotPublishable.Visible = true;
                    NotPublishable.Text = "Cannot retrieve the data for this Country at this moment.  Please try again later";
                    HyperCatalog.WebServices.EventLoggerWS.WSEventLogger _EventLog = HyperCatalog.WebServices.WSInterface.EventLogger;
                    const int CRYSTAL_UI_COMPONENT_ID = 2;
                    Guid errorGuid = Guid.NewGuid();
                    HyperCatalog.EventLogger.EventLogger.AddEventLog(CRYSTAL_UI_COMPONENT_ID, errorGuid, 0, HyperCatalog.EventLogger.Severity.INFORMATION, "Crystal UI PDB View Error", "", "The Country Id is not available for this country in the MV");
                    #endregion Country Unavailable
                }
            }
            catch (Exception ex)
            {
                #region Set PDBView Unavailable

                labelLinks.Visible = false;
                labelPLC.Visible = false;
                Market.Visible = false;
                Label2.Visible = false;
                Label3.Visible = false;
                ProductLines.Visible = false;
                Publishers.Visible = false;
                Label1.Visible = false;
                NotPublishable.Visible = true;
                NotPublishable.Text = "Cannot connect to PDB at this moment.  Please try again later";
                const int CRYSTAL_UI_COMPONENT_ID = 2;
                Guid errorGuid = Guid.NewGuid();
                HyperCatalog.EventLogger.EventLogger.AddEventLog(CRYSTAL_UI_COMPONENT_ID, errorGuid, 0, HyperCatalog.EventLogger.Severity.ERROR, "Crystal UI PDB View Error", 0, -1, "", 0, ex.Message, ex.StackTrace); 
                #endregion Set PDBView Unavailable
            }
            finally
            { oracleConn.Close(); }
            
        }
        private string GetViewQuery(string ViewName)
        {
            Database dbCrystal = Utils.GetMainDB();
            SqlDataReader drQuery;
            string strQuery=string.Empty;
            drQuery=dbCrystal.RunSQLReturnRS ("SELECT Query FROM PDBQueries WITH (NOLOCK) WHERE ViewName='"+ViewName.Trim()+"'");
            if (drQuery.HasRows)
            {
            drQuery.Read();
            strQuery=drQuery[0].ToString();
            }
            dbCrystal.Dispose();
            drQuery.Dispose();
            return strQuery;
        }

        void PDBView(string CountryId, int InstanceId)
        {
            #region Declarations

            string CultureCode = SessionState.Culture.Code;
            string CountryCode = SessionState.Culture.Code.Substring(0, 2).ToUpper();
            string Publish = null;
            string PublishableFlag = null;
            string MarketSegment = null;
            string ProductLine = null;
            string ItemPublish = null;
            string PDBRefresh = null;
            string PDBQuery = string.Empty;
            string Country_id = CountryId;
            int Instance_Id = InstanceId;
            OracleConnection oracleConn = new OracleConnection();

            #endregion Declarations

            oracleConn.ConnectionString = SessionState.CacheParams["PDBConnectionString"].Value.ToString();
            oracleConn.Open();

            #region Retrieve PDB Refresh info

            //// PDB Last Refreshed on ///////
            //PDBQuery="select MAX(data_update_time) from hp_pdb_delta_timestamps where cycle_completed = 'Y' and instance_id = " + InstanceId ;
            PDBQuery = GetViewQuery("RefreshTime");
            PDBQuery = PDBQuery.Replace("$INSTANCEID", InstanceId.ToString());

            OracleCommand sqlPDBRefreshcommand = new OracleCommand(PDBQuery, oracleConn);
            OracleDataReader drPDB = sqlPDBRefreshcommand.ExecuteReader();
            drPDB.Read();
            if (drPDB.HasRows)
            { PDBRefresh = drPDB[0].ToString(); }
            uwToolbar.Items.FromKeyLabel("Published").Text = "Published : YES";
            uwToolbar.Items.FromKeyLabel("LastRefreshed").Text = "LastPDBRefreshTime (GMT) : " + PDBRefresh + "";

            #endregion Retrieve PDB Refresh info

            #region Disposing all DataObjects
            oracleConn.Close();
            drPDB.Dispose();
            #endregion Disposing all DataObjects

            QDEUtils.UpdateCultureCodeFromRequest();
            if (!Page.IsPostBack)
            { UpdateDataView(false); }
            SessionState.QDETab = "tb_PDBVIew";
        }

        private void UpdateDataView(bool export)
        {

            #region Declarations

            string Publish = null;
            string PublishableFlag = null;
            string MarketSegment = null;
            string ProductLine = null;
            string ItemPublish = null;
            string Country_id = null;
            string Culture_id = null;
            string PLCStatus = null;
            string PDBQuery = string.Empty;
            int clobcheck;
            string ClobError = string.Empty;
            int InstanceId;
            string CultureCode = SessionState.Culture.Code;
            string CountryCode = SessionState.Culture.Code.Substring(0, 2).ToUpper();
            OracleConnection oracleConn = new OracleConnection();

            DataSet dsPub = new DataSet();
            DataSet ds = new DataSet();
            DataTable dtChunks = new DataTable();
            DataTable dtLinks = new DataTable();
            DataTable dtPLC = new DataTable();
            DataTable dtPL = new DataTable();
            DataTable dtMkt = new DataTable();
            DataTable dtPub = new DataTable();

            #endregion Declarations
                    
            //  Changes incorporated for Crystal-Gemstone convergence ( Deepa , 15/03/15)
            if (CultureCode.ToUpper() == "US-EN" || CultureCode.ToUpper() == "NA-EN")
            {
                InstanceId = 1;
            }
            else
            {
                InstanceId = 2;
            }
            //  Deepa , 15/03/15 - End

            oracleConn.ConnectionString = SessionState.CacheParams["PDBConnectionString"].Value.ToString();
            oracleConn.Open();

            #region Get CountryId

            OracleCommand sqlCountryCommand = new OracleCommand(" select DISTINCT Country_id  from hp_pdb_geography_" + InstanceId + "_mv where Country_code = '" + CountryCode + "' and Instance_Id =  " + InstanceId + "", oracleConn);
            OracleDataReader drC = sqlCountryCommand.ExecuteReader();
            drC.Read();
            Country_id = drC[0].ToString();
            sqlCountryCommand.Dispose();
            drC.Dispose();


            #endregion Get CountryId

            #region Get CultureId

            sqlCountryCommand = new OracleCommand(" select DISTINCT culture_id  from hp_pdb_geography_" + InstanceId + "_mv where culture_code = '" + CultureCode + "' and Instance_Id =  " + InstanceId + "", oracleConn);
            drC = sqlCountryCommand.ExecuteReader();
            drC.Read();
            Culture_id = drC[0].ToString();
            sqlCountryCommand.Dispose();
            drC.Dispose();

            #endregion Get CultureId

            #region Retrieve Market Segments info

            //PDBQuery=" select Content_data AS MarketSegments from HP_pdb_item_meta_data_" + InstanceId + "_mv WHERE Country_id= " + Country_id + " and ITem_id =" + SessionState.CurrentItem.Id + " and CONTENT_TYPE = 'Market Segments'"
            PDBQuery = GetViewQuery("MarketSegments");
            PDBQuery = PDBQuery.Replace("$INSTANCEID", InstanceId.ToString());
            PDBQuery = PDBQuery.Replace("$COUNTRYID", Country_id.ToString());
            PDBQuery = PDBQuery.Replace("$ITEMID", SessionState.CurrentItem.Id.ToString());
            OracleCommand sqlMarketCommand = new OracleCommand(PDBQuery, oracleConn);
            OracleDataAdapter ODAMkt = new OracleDataAdapter(sqlMarketCommand);
            ODAMkt.Fill(dtMkt);
            ds.Tables.Add(dtMkt);
            ds.Tables[0].TableName = "MarketSegments";
            OracleDataReader drM = sqlMarketCommand.ExecuteReader();
            if (drM.HasRows)
            {
                while (drM.Read())
                { MarketSegment = MarketSegment + drM[0].ToString() + ";"; }
                Market.Text = MarketSegment;
            }

            #endregion Retrieve Market Segments info

            #region retrieve PLCode info
            //PDBQuery="select Content_data AS ProductLines from HP_pdb_item_meta_data_" + InstanceId + "_mv WHERE Country_id= " + Country_id + " and ITem_id =" + SessionState.CurrentItem.Id + " and CONTENT_TYPE = 'Product Lines'";
            PDBQuery = GetViewQuery("PLCode");
            PDBQuery = PDBQuery.Replace("$INSTANCEID", InstanceId.ToString());
            PDBQuery = PDBQuery.Replace("$COUNTRYID", Country_id.ToString());
            PDBQuery = PDBQuery.Replace("$ITEMID", SessionState.CurrentItem.Id.ToString());
            OracleCommand sqlProductLinesCommand = new OracleCommand(PDBQuery, oracleConn);
            OracleDataAdapter ODAPL = new OracleDataAdapter(sqlProductLinesCommand);
            ODAPL.Fill(dtPL);
            ds.Tables.Add(dtPL);
            ds.Tables[1].TableName = "ProductLines";

            OracleDataReader drPL = sqlProductLinesCommand.ExecuteReader();
            if (drPL.HasRows)
            {
                while (drPL.Read())
                { ProductLine = ProductLine + drPL[0].ToString() + ";"; }
                ProductLines.Text = ProductLine;
            }

            #endregion retrieve PLCode info

            #region Retrieve Publishers Info
            //PDBQuery = "select  Content_data AS Publishers from "
            //+ " HP_pdb_item_meta_data_" + InstanceId + "_mv WHERE Country_id= " + Country_id + " and Item_id =" + SessionState.CurrentItem.Id + " and CONTENT_TYPE = 'Item Publishers'";

            PDBQuery = GetViewQuery("Publishers");
            PDBQuery = PDBQuery.Replace("$INSTANCEID", InstanceId.ToString());
            PDBQuery = PDBQuery.Replace("$COUNTRYID", Country_id.ToString());
            PDBQuery = PDBQuery.Replace("$ITEMID", SessionState.CurrentItem.Id.ToString());
            OracleCommand sqlItemPublisherscommand = new OracleCommand(PDBQuery, oracleConn);
            OracleDataAdapter ODAPub = new OracleDataAdapter(sqlItemPublisherscommand);
            ODAPub.Fill(dtPub);
            ds.Tables.Add(dtPub);
            ds.Tables[2].TableName = "Publishers";
            OracleDataReader drIP = sqlItemPublisherscommand.ExecuteReader();
            if (drIP.HasRows)
            {
                while (drIP.Read())
                { ItemPublish = ItemPublish + drIP[0].ToString() + ";"; }
                Publishers.Text = ItemPublish;
            }

            #endregion Retrieve Publishers Info

            #region Retrieve Chunk info

            PDBQuery = GetViewQuery("Chunks");
            PDBQuery = PDBQuery.Replace("$INSTANCEID", InstanceId.ToString());
            PDBQuery = PDBQuery.Replace("$CULTUREID", Culture_id.ToString());
            PDBQuery = PDBQuery.Replace("$ITEMID", SessionState.CurrentItem.Id.ToString());
            PDBQuery = PDBQuery.Replace("$CULTURECODE", SessionState.Culture.Code.ToString());
         
            OracleCommand sqlChunksCommmand = new OracleCommand(PDBQuery, oracleConn);
            OracleDataAdapter ODAChunks = new OracleDataAdapter(sqlChunksCommmand);

            try
            {
                ODAChunks.Fill(dtChunks);
                ds.Tables.Add(dtChunks);
                ds.Tables[3].TableName = "Chunks";
                clobcheck = 1;
            }
            catch (Exception E)
            {
                #region Handling ORA-22835 Error

                ClobError = E.Message.ToString();
                if (E.Message.Contains("ORA"))
                {
                    ODAChunks = null;
                    dtChunks = null;
                    PDBQuery = GetViewQuery("ChunksClob");
                    PDBQuery = PDBQuery.Replace("$INSTANCEID", InstanceId.ToString());
                    PDBQuery = PDBQuery.Replace("$CULTUREID", Culture_id.ToString());
                    PDBQuery = PDBQuery.Replace("$ITEMID", SessionState.CurrentItem.Id.ToString());
                    PDBQuery = PDBQuery.Replace("$CULTURECODE", SessionState.Culture.Code.ToString());
                    ODAChunks = new OracleDataAdapter(PDBQuery, oracleConn);
                    dtChunks = new DataTable();
                    ODAChunks.Fill(dtChunks);
                    ds.Tables.Add(dtChunks);
                    ds.Tables[3].TableName = "Chunks";
                    clobcheck = 1;
                }
                else
                {clobcheck = 0; }

                #endregion Handling ORA-22835 Error
            }
            
            if (clobcheck==0)
                throw new Exception(ClobError);

            dgChunks.DataSource = dtChunks;
            Utils.InitGridSort(ref dgChunks, false);
            dgChunks.DataBind();
            InitializeChunksGridGrouping();
            dgChunks.DisplayLayout.AllowSortingDefault = AllowSorting.No;
            dgChunks.DisplayLayout.ReadOnly = ReadOnly.PrintingFriendly;

            #endregion Retrieve Chunk info

            #region Retrieve PLC info
            //PDBQuery="SELECT Item_id as ItemId, Country_code as CountryCode, status_name as Status,blind_date as BlindDate,full_date as FullDate , Obsolete_date as ObsoleteDate, announcement_date as AnnouncementDate, Removal_date as RemovalDate  FROM hp_pdb_item_plc_" + InstanceId + "_mv WHERE Country_id = " + Country_id + " and  Instance_id = '" + InstanceId + "' and  Item_Id= '" + itemId + "' ";
            PDBQuery = GetViewQuery("PLC");
            PDBQuery = PDBQuery.Replace("$INSTANCEID", InstanceId.ToString());
            PDBQuery = PDBQuery.Replace("$COUNTRYID", Country_id.ToString());
            PDBQuery = PDBQuery.Replace("$ITEMID", SessionState.CurrentItem.Id.ToString());
            OracleCommand sqlPLCCommmand = new OracleCommand(PDBQuery,oracleConn);
            OracleDataAdapter ODAPLC = new OracleDataAdapter(sqlPLCCommmand);
            ODAPLC.Fill(dtPLC);
            ds.Tables.Add(dtPLC);
            ds.Tables[4].TableName = "PLC";
            if (dtPLC.Rows.Count > 0)
            {
                dgPLC.DataSource = dtPLC;
                Utils.InitGridSort(ref dgPLC, false);
                dgPLC.DataBind();
                UITools.UpdatePLCGridHeader(dgPLC);
                dgPLC.DisplayLayout.AllowSortingDefault = AllowSorting.No;
                dgPLC.DisplayLayout.ReadOnly = ReadOnly.PrintingFriendly;
                if (dgPLC.Columns.FromKey("PID") != null)
                    dgPLC.Columns.FromKey("PID").Format = SessionState.User.FormatDate;
                if (dgPLC.Columns.FromKey("POD") != null)
                    dgPLC.Columns.FromKey("POD").Format = SessionState.User.FormatDate;
                if (dgPLC.Columns.FromKey("Blind") != null)
                    dgPLC.Columns.FromKey("Blind").Format = SessionState.User.FormatDate;
                if (dgPLC.Columns.FromKey("Announcement") != null)
                    dgPLC.Columns.FromKey("Announcement").Format = SessionState.User.FormatDate;
                if (dgPLC.Columns.FromKey("Removal") != null)
                    dgPLC.Columns.FromKey("Removal").Format = SessionState.User.FormatDate;
                labelPLC.Visible = true;
                dgPLC.Visible = true;
            }
            else
            {
                labelPLC.Visible = false;
                dgPLC.Visible = false;
            }

            #endregion Retrieve PLC info

            #region Retrieve Link info
            //PDBQuery="SELECT distinct MV.Item_Id, W.Item_Name AS SubItemName ,Host_Item_number  AS SubItemSKU, Sub_Item_Id, WS.Item_name AS ItemName,Compatible_Item_number AS ItemSKU,Link_Type_Id as LinkTypeId, Country_Code as CountryCode, Sub_Item_Sort, Recommended  FROM hp_pdb_Links_" + InstanceId + "_mv MV , Work_Categorization  W  ,Work_Categorization  WS  WHERE  Country_id = " + Country_id + " and   W.Item_Id = MV.Item_Id and W.Instance_id =" + InstanceId + " and ws.Item_id = mv.sub_item_id  and W.Instance_id = WS.Instance_id and W.Item_Id = " + itemId + "  ORDER BY Link_Type_Id, Sub_Item_Sort  ";
            PDBQuery = GetViewQuery("Links");
            PDBQuery = PDBQuery.Replace("$INSTANCEID", InstanceId.ToString());
            PDBQuery = PDBQuery.Replace("$COUNTRYID", Country_id.ToString());
            PDBQuery = PDBQuery.Replace("$ITEMID", itemId.ToString());
            OracleCommand sqlLinksComm = new OracleCommand(PDBQuery, oracleConn);
            OracleDataAdapter ODALinks = new OracleDataAdapter(sqlLinksComm);
            ODALinks.Fill(dtLinks);
            ds.Tables.Add(dtLinks);
            ds.Tables[5].TableName = "Links";
            if (dtLinks != null)
            {
                dgLinks.DataSource = dtLinks;
                Utils.InitGridSort(ref dgLinks, false);
                dgLinks.DataBind();
                dgLinks.DisplayLayout.AllowSortingDefault = AllowSorting.No;
                dgLinks.DisplayLayout.ReadOnly = ReadOnly.PrintingFriendly;
                labelLinks.Visible = true;
                dgLinks.Visible = true;
                InitializeLinksGridGrouping();
            }
            else
            {
                labelLinks.Visible = false;
                dgLinks.Visible = false;
            }

            #endregion Retrieve Link info

            if (export)
            { Utils.ExportDataSetToExcel(this, ds, "Delivery.xls"); }

            #region Disposing all DataObjects

            oracleConn.Close();
            ODAMkt.Dispose();
            drM.Dispose();
            dtMkt.Dispose();
            sqlMarketCommand.Dispose();
            drPL.Dispose();
            dtPL.Dispose();
            ODAPL.Dispose();
            sqlProductLinesCommand.Dispose();
            ODAPub.Dispose();
            dtPub.Dispose();
            dsPub.Dispose();
            sqlItemPublisherscommand.Dispose();
            drIP.Dispose();
            ODAChunks.Dispose();
            dtChunks.Dispose();
            sqlChunksCommmand.Dispose();
            sqlPLCCommmand.Dispose();
            dtPLC.Dispose();
            ODAPLC.Dispose();
            sqlLinksComm.Dispose();
            ODALinks.Dispose();
            dtLinks.Dispose();

            #endregion Disposing all DataObjects

        }

        private void InitializeLinksGridGrouping()
        {
            #region Declaration
            int i = 0;
            int groupLinkTypeCount = 0;
            int groupLinkFromCount = 0;
            int groupFamilyCount = 0;
            bool currentLinkFrom = false;
            string currentFamily = string.Empty;
            LinkType currentLinkType = null;
            bool newLinkType = true;
            bool newLinkFrom = true;
            #endregion Declaration

            while (i < dgLinks.Rows.Count)
            {
                #region Group by LinkType
                int linkTypeId = Convert.ToInt32(dgLinks.Rows[i].Cells.FromKey("LinkTypeId").Value);
                if (i == 0 || (currentLinkType != null && currentLinkType.Id != linkTypeId))
                {
                    currentLinkType = LinkType.GetByKey(linkTypeId);
                    newLinkType = true;
                    dgLinks.Rows.Insert(i, new UltraGridRow());
                    UltraGridRow groupRow = dgLinks.Rows[i];
                    UltraGridCell groupCellMax = groupRow.Cells[dgLinks.Columns.Count - 1]; // initialize all cells for this row
                    foreach (UltraGridCell cell in groupRow.Cells)
                    {
                        cell.Style.CssClass = string.Empty;
                    }
                    dgLinks.Rows[i].Style.CssClass = "ptbgroup";
                    UltraGridCell groupCell = groupRow.Cells.FromKey("Class");
                    groupCell.Text = HyperCatalog.Business.LinkType.GetByKey(linkTypeId).Name;
                    groupCell.ColSpan = 4;
                    i++;
                }
                #endregion

                if (currentLinkType != null && currentLinkType.IsBidirectional && dgLinks.Rows[i].Cells.FromKey("LinkFrom") == null)
                {
                    #region Group by LinkFrom
                    bool linkFrom = Convert.ToBoolean(dgLinks.Rows[i].Cells.FromKey("LinkFrom").Value);
                    if (newLinkType || currentLinkFrom != linkFrom)
                    {
                        currentLinkFrom = linkFrom;
                        newLinkFrom = true;
                        newLinkType = false;
                        dgLinks.Rows.Insert(i, new UltraGridRow());
                        UltraGridRow groupRow = dgLinks.Rows[i];
                        UltraGridCell groupCellMax = groupRow.Cells[dgLinks.Columns.Count - 1]; // initialize all cells for this row          
                        foreach (UltraGridCell cell in groupRow.Cells)
                        {
                            cell.Style.CssClass = string.Empty;
                        }
                        dgLinks.Rows[i].Style.CssClass = "ptb4";
                        UltraGridCell groupCell = groupRow.Cells.FromKey("Class");
                        if (linkFrom)
                            groupCell.Text = "Companion list";
                        else
                            //Code Modifued to change 'Hardware' to 'Host'
                            //groupCell.Text = "Hardware list";
                            groupCell.Text = "Host list";

                        groupCell.ColSpan = 4;
                        i++;
                    }
                    #endregion

                    #region Group by Family
                    if (dgLinks.Rows[i].Cells.FromKey("Family") != null && dgLinks.Rows[i].Cells.FromKey("Family").Value != null)
                    {
                        string family = string.Empty;
                        if (linkFrom) // Companion list
                            family = dgLinks.Rows[i].Cells.FromKey("SubFamily").Value.ToString();
                        else // Hardware list
                            family = dgLinks.Rows[i].Cells.FromKey("Family").Value.ToString();
                        if (newLinkType || newLinkFrom || currentFamily != family)
                        {
                            currentFamily = family;
                            newLinkType = false;
                            newLinkFrom = false;
                            dgLinks.Rows.Insert(i, new UltraGridRow());
                            UltraGridRow groupRow = dgLinks.Rows[i];
                            UltraGridCell groupCellMax = groupRow.Cells[dgLinks.Columns.Count - 1]; // initialize all cells for this row
                            foreach (UltraGridCell cell in groupRow.Cells)
                            {
                                cell.Style.CssClass = string.Empty;
                            }
                            dgLinks.Rows[i].Style.CssClass = "ptb5";
                            UltraGridCell groupCell = groupRow.Cells.FromKey("Class");
                            groupCell.Text = family;
                            groupCell.ColSpan = 4;
                            i++;
                        }
                    }
                    #endregion
                }

                #region bidirectional link type
                if (dgLinks.Rows[i].Cells.FromKey("Bidirectional") != null && dgLinks.Rows[i].Cells.FromKey("Bidirectional").Value != null)
                {
                    bool isBidirectional = Convert.ToBoolean(dgLinks.Rows[i].Cells.FromKey("Bidirectional").Value);
                    if (!isBidirectional && dgLinks.Rows[i].Cells.FromKey("Name") != null) // Cross sell, Bundle, ...
                    {
                        dgLinks.Rows[i].Cells.FromKey("Name").ColSpan = 2;
                    }
                }
                #endregion

                i++;
            }
        }

        private void InitializeChunksGridGrouping()
        {
            int i = 0;
            groupCount = 0;
            while (i < dgChunks.Rows.Count)
            {
                string containerGroup = dgChunks.Rows[i].Cells.FromKey("Path").Value.ToString();
                if (i == 0 || currentGroup != containerGroup)
                {
                    currentGroup = containerGroup;
                    dgChunks.Rows.Insert(i, new UltraGridRow());
                    UltraGridRow groupRow = dgChunks.Rows[i];
                    UltraGridCell groupCellMax = groupRow.Cells[dgChunks.Columns.Count - 1]; // initialize all cells for this row
                    foreach (UltraGridCell cell in groupRow.Cells)
                    {
                        cell.Style.CssClass = string.Empty;
                    }
                    dgChunks.Rows[i].Style.CssClass = "ptbgroup";
                    UltraGridCell groupCell = groupRow.Cells.FromKey("Mandatory");
                    groupCell.Text = containerGroup;
                    groupCell.ColSpan = 5;
                    i++;
                }
                i++;
            }
        }

        private void InitializeLinksbbGridGrouping()
        {
            #region Declaration

            int i = 0;
            int groupLinkTypeCount = 0;
            int groupLinkFromCount = 0;
            int groupFamilyCount = 0;
            bool currentLinkFrom = false;
            string currentFamily = string.Empty;
            LinkType currentLinkType = null;
            bool newLinkType = true;
            bool newLinkFrom = true;

            #endregion Declaration

            while (i < dgLinks.Rows.Count)
            {
                //#region Group by LinkType
                int linkTypeId = Convert.ToInt32(dgLinks.Rows[i].Cells.FromKey("LinkTypeId").Value);
                if (i == 0 || (currentLinkType != null && currentLinkType.Id != linkTypeId))
                {
                    currentLinkType = LinkType.GetByKey(linkTypeId);

                    newLinkType = true;
                    dgLinks.Rows.Insert(i, new UltraGridRow());
                    UltraGridRow groupRow = dgLinks.Rows[i];
                    UltraGridCell groupCellMax = groupRow.Cells[dgLinks.Columns.Count - 1]; // initialize all cells for this row
                    foreach (UltraGridCell cell in groupRow.Cells)
                    {
                        cell.Style.CssClass = string.Empty;
                    }
                    dgLinks.Rows[i].Style.CssClass = "ptbgroup";
                    i++;
                }


                if (currentLinkType != null)
                {
                    if (newLinkType)
                    {

                        newLinkType = false;
                        dgLinks.Rows.Insert(i, new UltraGridRow());
                        UltraGridRow groupRow = dgLinks.Rows[i];
                        UltraGridCell groupCellMax = groupRow.Cells[dgLinks.Columns.Count - 1]; // initialize all cells for this row          
                        foreach (UltraGridCell cell in groupRow.Cells)
                        {
                            cell.Style.CssClass = string.Empty;
                        }
                        dgLinks.Rows[i].Style.CssClass = "ptb4";

                        i++;
                    }

                    string family = string.Empty;
                    if (newLinkType)
                    {
                        currentFamily = family;
                        newLinkType = false;
                        //newLinkFrom = false;
                        dgLinks.Rows.Insert(i, new UltraGridRow());
                        UltraGridRow groupRow = dgLinks.Rows[i];
                        UltraGridCell groupCellMax = groupRow.Cells[dgLinks.Columns.Count - 1]; // initialize all cells for this row
                        foreach (UltraGridCell cell in groupRow.Cells)
                        {
                            cell.Style.CssClass = string.Empty;
                        }
                        dgLinks.Rows[i].Style.CssClass = "ptb5";

                        i++;

                    }

                }
                dgLinks.Rows[i].Cells.FromKey("Name").ColSpan = 2;
                i++;
            }
        }

        protected void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
        {
            #region Declaration

            CellsCollection cells = e.Row.Cells;
            string filter = txtFilter.Text.Trim();
            bool keep = true;
            bool isMandatory = Convert.ToBoolean(cells.FromKey("IsMandatory").Value);
            bool isInherited = Convert.ToBoolean(cells.FromKey("Inherited").Value);
            bool isResource = Convert.ToBoolean(cells.FromKey("IsResource").Value);
            bool isBoolean = Convert.ToBoolean(cells.FromKey("IsBoolean").Value);
            bool readOnly = Convert.ToBoolean(cells.FromKey("ReadOnly").Value);

            #endregion Declaration

            if (filter.Length > 0)
            {
                keep = false;
                foreach (Infragistics.WebUI.UltraWebGrid.UltraGridCell c in cells)
                {
                    if (!c.Column.Hidden && c.Value != null && c.Text != HyperCatalog.Business.Chunk.BlankValue)
                    {
                        if (c.Text.ToLower().IndexOf(filter.ToLower()) >= 0)
                        {
                            c.Text = Utils.CReplace(c.Text, filter, "<font color=red><b>" + filter + "</b></font>", 1);
                            keep = true;
                        }
                    }
                }
            }
            if (!keep)
            {
                e.Row.Delete();
            }
            else
            {

                cells.FromKey("CultureCode").Text = SessionState.Culture.Code;

                // Container group
                string containerGroup = cells.FromKey("Path").Text;
                if (currentGroup != containerGroup)
                {
                    currentGroup = containerGroup;
                    groupCount++;
                }

                // Check if ReadOnly container
                if (readOnly)
                    cells.FromKey("ContainerName").Text = cells.FromKey("ContainerName").Text + " <img src='/hc_v4/img/ed_glasses.gif'/>";

                //Display Mandatory logo
                UltraGridCell aCell = cells.FromKey("Mandatory");
                aCell.Style.CssClass = "ptb1";
                aCell.Text = string.Empty; // by default
                if (isMandatory)
                {
                    aCell.Style.CssClass = "SCM"; // Status Chunk Mandatory
                }

                // Update Item column
                aCell = cells.FromKey("ItemId");
                if (aCell.Text != itemId.ToString())
                    aCell.Value = itemId.ToString();

                //Display Status logo
                aCell = cells.FromKey("Status");
                if (aCell.Value != null)
                {
                    ChunkStatus cStatus = (ChunkStatus)Enum.Parse(typeof(ChunkStatus), HyperCatalog.Business.Chunk.GetStatusFromString(cells.FromKey("Status").Value.ToString()).ToString());
                    string status = HyperCatalog.Business.Chunk.GetStatusFromEnum(cStatus);
                    aCell.Style.CssClass = "S" + status;
                    aCell.Value = string.Empty;
                }

                // Check if value is inherited
                aCell = cells.FromKey("Value");
                aCell.Style.CssClass = "ptb3"; // by default
                aCell.Style.Wrap = true; // by default
                if (isInherited)
                {
                    aCell.Style.CssClass = "overw";
                    aCell.Style.Wrap = true;
                }

                // Ensure multiline is kept
                if (aCell.Value != null)
                {
                    // if chunk is resource, try do display it
                    if (isResource && aCell.Text != string.Empty)
                    {
                        try
                        {
                            string sUrl = aCell.Text;
                            // Call HyperPublisher WebMethod to convert URL to absolute URL
                            XmlDocument xmlInfo = new XmlDocument();
                            xmlInfo.LoadXml(HCPage.WSDam.ResourceGetByPath(sUrl));
                            System.Xml.XmlNode node = xmlInfo.DocumentElement;
                            string fullPath = node.Attributes["uri"].InnerText;
                            if (fullPath.ToLower().IndexOf("notfound") > 0 || fullPath == string.Empty)
                            {
                                fullPath = "/hc_v4/img/ed_notfound.gif";
                            }
                            aCell.Text = "<img src='" + fullPath + "?thumbnail=1&size=40' title='" + aCell.Text + "' border=0/>";
                        }
                        catch (Exception ex)
                        {
                            Trace.Warn("DAM", "Exception processing DAM: " + ex.Message);
                        }
                    }
                    else
                    {
                        // BLANK Value is replace by Readable sentence
                        if (aCell.Text == HyperCatalog.Business.Chunk.BlankValue)
                        {
                            aCell.Text = HyperCatalog.Business.Chunk.BlankText;
                            aCell.Style.CustomRules = string.Empty;
                        }
                        else
                        {
                            if (isBoolean && aCell.Text != string.Empty)
                            {
                                try
                                {
                                    if (Convert.ToBoolean(aCell.Value))
                                    {
                                        aCell.Text = "Yes";
                                    }
                                    else
                                    {
                                        aCell.Text = "No";
                                    }
                                }
                                catch { } // Value is not boolean!
                            }
                            else
                            {
                                //New code added for IM19833011, Arabic language issue fix
                                #region If RTL languages, ensure correct display

                                if (cells.FromKey("CultureCode").Text.Contains("-ar"))
                                    aCell.Style.CustomRules = "direction: rtl;";//unicode-bidi:bidi-override;";
                                #endregion    

                                aCell.Text = UITools.HtmlEncode(aCell.Text);
                            }
                        }

                        // Display Fallback on cultures if current Culture <> "master"
                        string sourceCultureCode = cells.FromKey("SourceCode").Text;
                        if (sourceCultureCode != SessionState.Culture.Code)
                        {
                            aCell.Style.CustomRules = aCell.Style.CustomRules + "color:blue;font:italic;";
                        }
                    }
                }
            }
        }
        protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
        {
            string btn = be.Button.Key.ToLower();
            if (btn == "export")
            {
                UpdateDataView(true);
            }
            if (btn == "republish")
            {
                PDBRefresh();
                lblError.Text = " Item '" + SessionState.CurrentItem.Name.ToString() + "' Republished successfully !";
                lblError.Visible = true;
            }

            //Code added for Links Requirement (PR664327) - to trigger adhoc delivery of links for the selected item by Prachi on 28th Jan 2013
            if (btn == "linksrepublish")
            {
                int r = LinksRepublish();
                if (r >= 1)
                {
                    lblError.Text = " Links for Item '" + SessionState.CurrentItem.Name.ToString() + "' triggered successfully for Republishing !";
                    lblError.Visible = true;
                }
                else if (r == 0)
                {
                    lblError.Text = " No Link exists for Item '" + SessionState.CurrentItem.Name.ToString() + "' !";
                    lblError.Visible = true;
                }
            }
        }

        protected void PDBRefresh()
        { 
         const string CST_PARAM_USERID="@UserId";
         const string CST_PARAM_CULTURE_CODE = "@CultureCode";
         const string CST_PARAM_ITEMID = "@ItemId";
         const string CST_SP_PDBRefresh_NAME = "_PDB_Refresh";
         
        try
         {
             using (Database db = Utils.GetMainDB())
             {
                 SqlParameter[] parameters ={ new SqlParameter(CST_PARAM_USERID, SessionState.User.Id), new SqlParameter(CST_PARAM_CULTURE_CODE, SessionState.Culture.Code), new SqlParameter(CST_PARAM_ITEMID, SessionState.CurrentItem.Id.ToString()) };
                 int result = db.RunSPReturnInteger(CST_SP_PDBRefresh_NAME, parameters);
             }
         }
         catch (Exception ex)
         {
             const int CRYSTAL_UI_COMPONENT_ID = 2;
             Guid errorGuid = Guid.NewGuid();
             HyperCatalog.EventLogger.EventLogger.AddEventLog(CRYSTAL_UI_COMPONENT_ID, errorGuid, 0, HyperCatalog.EventLogger.Severity.INFORMATION, "Crystal UI PDB Refresh Error", "", "Crystal UI PDB Refresh Error");
         }
        }
        protected void dgLinks_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
        {
            CellsCollection cells = e.Row.Cells;
            if (cells.FromKey("IsExcluded") != null && Convert.ToBoolean(cells.FromKey("IsExcluded").Value))
            {
                e.Row.Delete();
            }
            else
            {
                // Retrieve country code
                if (cells.FromKey("CountryCode") != null && cells.FromKey("ImageCountry") != null)
                {
                    string countryCode = string.Empty;
                    if (cells.FromKey("CountryCode") != null && cells.FromKey("CountryCode").Value != null)
                        countryCode = cells.FromKey("CountryCode").ToString();

                    // Update image for current country
                    if (countryCode.Length > 0 && cells.FromKey("ImageCountry") != null)
                        cells.FromKey("ImageCountry").Text = "<img title=\"" + countryCode + "\" src=\"/hc_v4/img/flags/" + countryCode.ToLower() + ".gif\">";
                }

                if (cells.FromKey("LinkFrom") != null)
                {
                    bool linkFrom = Convert.ToBoolean(cells.FromKey("LinkFrom").Value);
                    if (!linkFrom) // Hardware list
                    {
                        if (cells.FromKey("Name") != null && cells.FromKey("ItemName") != null && cells.FromKey("ItemName").Value != null)
                            cells.FromKey("Name").Text = cells.FromKey("ItemName").Value.ToString();
                        if (cells.FromKey("SKU") != null && cells.FromKey("ItemSKU") != null && cells.FromKey("ItemSKU").Value != null)
                            cells.FromKey("SKU").Text = cells.FromKey("ItemSKU").Value.ToString();
                        if (cells.FromKey("Class") != null && cells.FromKey("ClassName") != null && cells.FromKey("ClassName").Value != null)
                            cells.FromKey("Class").Text = cells.FromKey("ClassName").Value.ToString();
                    }
                    else // Companion list
                    {
                        if (cells.FromKey("Name") != null && cells.FromKey("SubItemName") != null && cells.FromKey("SubItemName").Value != null)
                            cells.FromKey("Name").Text = cells.FromKey("SubItemName").Value.ToString();
                        if (cells.FromKey("SKU") != null && cells.FromKey("SubItemSKU") != null && cells.FromKey("SubItemSKU").Value != null)
                            cells.FromKey("SKU").Text = cells.FromKey("SubItemSKU").Value.ToString();
                        if (cells.FromKey("Class") != null && cells.FromKey("SubClassName") != null && cells.FromKey("SubClassName").Value != null)
                            cells.FromKey("Class").Text = cells.FromKey("SubClassName").Value.ToString();
                    }

                    if (cells.FromKey("Name") != null)
                        cells.FromKey("Name").Style.Wrap = true;
                    if (cells.FromKey("SKU") != null)
                        cells.FromKey("SKU").Style.Wrap = true;
                    if (cells.FromKey("Class") != null)
                        cells.FromKey("Class").Style.Wrap = true;
                }
            }
        }

        //Code added for Links Requirement (PR664327) - to trigger adhoc delivery of links for the selected item by Prachi on 28th Jan 2013
        protected int LinksRepublish()
        {
            const string CST_PARAM_CULTURE_CODE = "@CultureCode";
            const string CST_PARAM_ITEMID = "@ItemId";
            const string CST_SP_LinksRepublish_NAME = "_PDB_LinksRepublish";

            try
            {
                using (Database db = Utils.GetMainDB())
                {
                    SqlParameter[] parameters = { new SqlParameter(CST_PARAM_CULTURE_CODE, SessionState.Culture.Code), new SqlParameter(CST_PARAM_ITEMID, SessionState.CurrentItem.Id) };
                    int result = db.RunSPReturnInteger(CST_SP_LinksRepublish_NAME, parameters);
                    return result;
                }
            }
            catch (Exception ex)
            {
                const int CRYSTAL_UI_COMPONENT_ID = 2;
                Guid errorGuid = Guid.NewGuid();
                HyperCatalog.EventLogger.EventLogger.AddEventLog(CRYSTAL_UI_COMPONENT_ID, errorGuid, 0, HyperCatalog.EventLogger.Severity.INFORMATION, "Crystal UI PDB Links Republish Error", "", "Crystal UI PDB LinksRepublish Error");
                return -1;
            }
        }
    }
}
