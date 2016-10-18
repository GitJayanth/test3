# region Uses
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HyperCatalog.Business;
using HyperComponents.Data.dbAccess;
using HyperCatalog.Shared;
using System.Data.SqlClient;
using System.Text;

# endregion
# region History
// Purpose - To Generate a Report for Links
// Creation - Sateesh - 12/10/2007
# endregion
public partial class UI_Collaborate_LinksReport : HCPage
{
    #region Declarations
    string curCulture = string.Empty;
    CultureType curCultureType = CultureType.Master;
    int MaxRows = 0;
  
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            /// Retrieve all Users Cultures
            using (CultureList dsCultures = HyperCatalog.Business.User.GetByKey(SessionState.User.Id).Cultures)
            {
                dsCultures.Sort("Type");
                foreach (HyperCatalog.Business.Culture c in dsCultures)
                {
                    if (c.Country == null)
                    {
                        DDL_Countries.Items.Add(new ListItem(c.Name, c.Code));
                    }
                    else
                    {
                        if (DDL_Countries.Items.FindByText(c.Country.Name) == null)
                        {
                            DDL_Countries.Items.Add(new ListItem(c.Country.Name, c.Code));
                        }
                        c.Dispose();
                    }
                }
                BindLists();
                
                UpdateDataView();
            }

           
        }
    }
    private void BindLists()
    {
        Bind_itemClasses();
        Bind_linkTypes();
        Bind_itemStatuses();
        Bind_itemLimitValue();
    }
    private void Bind_itemLimitValue()
    {
        Links_Num.Items.Add(new ListItem("ALL"));
        Links_Num.Items.Add(new ListItem("5"));
        Links_Num.Items.Add(new ListItem("10"));
        Links_Num.Items.Add(new ListItem("20"));
        Links_Num.Items.Add(new ListItem("30"));
        Links_Num.Items.Add(new ListItem("50"));
        Links_Num.ToolTip = "Select the limit value for links";
    }
    private void Bind_linkTypes()
    {
        using (HyperCatalog.Business.CollectionView linkTypeList = new HyperCatalog.Business.CollectionView(HyperCatalog.Business.LinkType.GetAll()))
        {
            
            linkTypeList.Sort("Name");
            Link_Type.ToolTip = "Select required link Type";
            Link_Type.DataTextField = "Name";
            Link_Type.DataValueField = "Id";
            Link_Type.DataSource = linkTypeList;
            Link_Type.DataBind();
        }
    }
    private void Bind_itemClasses()
    {
        using (HyperCatalog.Business.CollectionView itemList = new HyperCatalog.Business.CollectionView(HyperCatalog.Business.Item.GetAll(" LevelId = 1 ")))
        {
            itemList.Sort("Name");
            
            Prod_Type.DataTextField = "Name";
            Prod_Type.DataValueField = "Id";
          
            Prod_Type.DataSource = itemList;
            Prod_Type.DataBind();
          
            Prod_Type.Items.Add(new ListItem("All", "0")); // Since all Items are children to Item '0'
            Prod_Type.ToolTip = "Select required class";
            
        }
    }
   
    private void Bind_itemStatuses()
    {
        statusFilter.Items.Clear();
        foreach (HyperCatalog.Business.ItemStatus status in Enum.GetValues(typeof(HyperCatalog.Business.ItemStatus)))
            statusFilter.Items.Add(new ListItem(status.ToString(), HyperCatalog.Business.Item.GetStatusFromEnum(status)));
        statusFilter.Items[1].Selected = true ;  //By default 'Live' will be selected
        statusFilter.ToolTip = "Select any combination of Statuses.Must select atleast one value";
    }
    #region GridUpdation
    private void UpdateDataView()
    {
        try
        {
            using (Database dbObj = new Database(HyperCatalog.Business.ApplicationSettings.Components["Datawarehouse_DB"].ConnectionString, 3600))
            {
                using (HyperCatalog.Business.Culture c = HyperCatalog.Business.Culture.GetByKey(DDL_Countries.SelectedValue))
                {
                    string columns = "";
                    foreach (ListItem item in statusFilter.Items)
                        if (item.Selected)
                            columns += "," + item.Value;
                    if (columns.Length > 0)
                        columns = columns.Substring(1, columns.Length - 1);
                    columns = columns.Replace(",", "','");
                    columns = "'" + columns + "'";
                    int inh = (inheritanceFilter.Checked) ? 1 : 0;
                    MaxRows = Convert.ToInt32(SessionState.CacheParams["MaxSearchQueryDisplayedRows"].Value);

                    using (DataSet ds = dbObj.RunSPReturnDataSet("_Reports_Links",
                      new SqlParameter("@CountryCode", c.CountryCode),
                      new SqlParameter("@ClassId", Prod_Type.SelectedItem.Value),
                      new SqlParameter("@LinkTypeId", Link_Type.SelectedItem.Value),
                      new SqlParameter("@PLCStatus", columns),
                      new SqlParameter("@LimitValuePerItem", Links_Num.SelectedItem.Value),
                      new SqlParameter("@InheritanceNeeded", inh),
                      new SqlParameter("@MaxRows", MaxRows)))
                    {
                        dbObj.CloseConnection();
                        if (dbObj.LastError != string.Empty)
                        {
                            lbMessage.Text = "[ERROR] _Reports_Links -> " + dbObj.LastError;
                            lbMessage.CssClass = "hc_error";
                            lbMessage.Visible = true;
                        }
                        else
                        {
                            using (HyperCatalog.Business.Culture selCul = HyperCatalog.Business.Culture.GetByKey(DDL_Countries.SelectedValue))
                            {
                                curCultureType = selCul.Type;
                            }
                            #region Results
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                dg.DataSource = ds;
                                lbMessage.Visible = false;
                                Utils.InitGridSort(ref dg);
                                dg.DataBind();
                                dg.Visible = true;
                                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count == 1)
                                {
                                    int count = Convert.ToInt32(ds.Tables[1].Rows[0]["ProductCount"]);
                                    lbMessage.CssClass = "hc_success";
                                    if (count <= MaxRows)
                                        lbMessage.Text = "Product count: " + count.ToString() + "<br />";
                                    else
                                        lbMessage.Text = "Product count: " + count.ToString() + " (" + MaxRows.ToString() + " products are displayed)<br />Your report is returning too many rows (max = " + MaxRows.ToString() + ")<br />";
                                    lbMessage.Visible = true;
                                }

                            }
                            #endregion
                            #region No result
                            else
                            {
                                lbMessage.CssClass = "hc_success";
                                lbMessage.Text = "No compatibilities found";
                                lbMessage.Visible = true;
                                dg.Visible = false;
                            }
                            #endregion
                        }
                    }
                }
            }
        }
        catch(Exception ex)
        {
            lbMessage.CssClass = "hc_error";
            lbMessage.Text = ex.ToString();
            lbMessage.Visible = true;
            dg.Visible = false;
        }
    }

    #endregion 
    
    #region Different Dataset for Export
    private void UpdateExport()
    {
        try
        {

            using (Database dbObj = new Database(HyperCatalog.Business.ApplicationSettings.Components["Datawarehouse_DB"].ConnectionString, 3600))
            {
                using (HyperCatalog.Business.Culture c = HyperCatalog.Business.Culture.GetByKey(DDL_Countries.SelectedValue))
                {
                    string columns = "";
                    foreach (ListItem item in statusFilter.Items)
                        if (item.Selected)
                            columns += "," + item.Value;
                    if (columns.Length > 0)
                        columns = columns.Substring(1, columns.Length - 1);
                    columns = columns.Replace(",", "','");
                    columns = "'" + columns + "'";
                    int inh = (inheritanceFilter.Checked) ? 1 : 0;


                    using (DataSet ds = dbObj.RunSPReturnDataSet("_Reports_Links",
                      new SqlParameter("@CountryCode", c.CountryCode),
                      new SqlParameter("@ClassId", Prod_Type.SelectedItem.Value),
                      new SqlParameter("@LinkTypeId", Link_Type.SelectedItem.Value),
                      new SqlParameter("@PLCStatus", columns),
                      new SqlParameter("@LimitValuePerItem", Links_Num.SelectedItem.Value),
                      new SqlParameter("@InheritanceNeeded", inh),
                      new SqlParameter("@MaxRows", -1)))
                    {
                        char csvSeparator = ',';
                        dbObj.CloseConnection();
                        if (dbObj.LastError != string.Empty)
                        {
                            lbMessage.Text = "[ERROR] _Reports_Links -> " + dbObj.LastError;
                            lbMessage.CssClass = "hc_error";
                            lbMessage.Visible = true;
                        }
                        else
                        {
                            using (HyperCatalog.Business.Culture selCul = HyperCatalog.Business.Culture.GetByKey(DDL_Countries.SelectedValue))
                            {
                                curCultureType = selCul.Type;
                            }
                            #region Results
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                ListItemCollection lstItemCol = new ListItemCollection();
                                lstItemCol.Add(new ListItem("Selected Country :", DDL_Countries.SelectedItem + ""));
                                lstItemCol.Add(new ListItem("Selected Class :", Prod_Type.SelectedItem + ""));
                                lstItemCol.Add(new ListItem("Selected LinkType :", Link_Type.SelectedItem + ""));
                                lstItemCol.Add(new ListItem("Selected PLCStatuses :", columns + ""));
                                lstItemCol.Add(new ListItem("Selected Limit Value :", Links_Num.SelectedValue + ""));
                                lstItemCol.Add(new ListItem("Need Inheritance/Fallback :", inh + ""));

                                DataTable dt = ds.Tables[0];
                                CSVExport(dt,csvSeparator.ToString(),lstItemCol,"Links Report");

                                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count == 1)
                                {
                                    int count = Convert.ToInt32(ds.Tables[1].Rows[0]["ProductCount"]);
                                    lbMessage.CssClass = "hc_success";
                                    lbMessage.Text = "Product count: " + count.ToString() + " links are exported<br />";
                                    lbMessage.Visible = true;
                                }
                            }
                            #endregion
                            #region No result
                            else
                            {
                                lbMessage.CssClass = "hc_success";
                                lbMessage.Text = "No compatibilities found";
                                lbMessage.Visible = true;
                                dg.Visible = false;
                            }
                            #endregion
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lbMessage.CssClass = "hc_error";
            lbMessage.Text = ex.ToString();
            lbMessage.Visible = true;
            dg.Visible = false;
        }
    }
    #endregion

    #region Anomalies
    private void ExportOrderAnomalies()
    {
        try
        {
            using (Database dbObj = new Database(HyperCatalog.Business.ApplicationSettings.Components["Datawarehouse_DB"].ConnectionString, 3600))
            {
                using (DataSet ds = dbObj.RunSPReturnDataSet("Link_OrderAnomalies"))
                {
                    char csvSeparator = ',';
                    dbObj.CloseConnection();
                    if (dbObj.LastError != String.Empty)
                    {
                        lbMessage.CssClass = "hc_error";
                        lbMessage.Text = "[ERROR] Link_OrderAnomalies -> " + dbObj.LastError;
                        lbMessage.Visible = true;
                    }
                    else
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataTable dt = ds.Tables[0];
                            CSVExport(dt, csvSeparator.ToString(), null, "OrderAnomalies Report");
                        }
                        #region No result
                        else
                        {
                            lbMessage.CssClass = "hc_success";
                            lbMessage.Text = "No anomalies found";
                            lbMessage.Visible = true;
                            dg.Visible = false;
                        }
                        #endregion
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lbMessage.CssClass = "hc_error";
            lbMessage.Text = ex.ToString();
            lbMessage.Visible = true;
            dg.Visible = false;
        }
    }
    #endregion

    #region Common For Export
    private void CSVExport(DataTable dt, string sep, ListItemCollection lstItemCol, string reportTitle)
    {
        foreach (DataRow dr in dt.Rows)
        {

            foreach (DataColumn dc in dt.Columns)
            {
                if (dc.ColumnName.Contains("Name") && dr[dc].ToString().Contains(","))

                    dr[dc] = dr[dc].ToString().Replace(",", ";");

            }

        }

        string s = Utils.ExportDataTableToCSVForSpecificReport(dt,sep, lstItemCol, reportTitle).ToString();

        #region Export Using Responce
        // Creating DefaultEncoding Object [parameter 0]
        System.Text.Encoding encoding = System.Text.Encoding.GetEncoding(0);
        //System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        byte[] contentBytes = encoding.GetBytes(s);
        Response.ContentEncoding = Encoding.Default;
        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();
        Response.AddHeader("Accept-Header", contentBytes.Length.ToString());

        Response.ContentType = "application/text";
        Response.AppendHeader("Content-Disposition", "attachment;filename=\""+reportTitle.Replace(" ","")+".csv\"; " +
                          "size=" + contentBytes.Length.ToString() + "; " +
                          "creation-date=" + DateTime.Now.ToString("R") + "; " +
                          "modification-date=" + DateTime.Now.ToString("R") + "; " +
                         "read-date=" + DateTime.Now.ToString("R"));
        // Response.Write(s);
        Response.OutputStream.Write(
        contentBytes, 0,
        Convert.ToInt32(contentBytes.Length));
        Response.Flush();

        try { Response.End(); }
        catch { }
    }
    #endregion
    #endregion
    protected void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
        Infragistics.WebUI.UltraWebGrid.UltraGridCell fromName = null;
        Infragistics.WebUI.UltraWebGrid.UltraGridCell toName = null;
        if (e.Row.Cells.FromKey("LinkFrom_ItemName") != null)
            fromName = e.Row.Cells.FromKey("LinkFrom_ItemName");
        if (e.Row.Cells.FromKey("LinkTo_ItemName") != null)
            toName = e.Row.Cells.FromKey("LinkTo_ItemName");


        #region Product name
        if (fromName != null)
        {
            if (curCultureType == HyperCatalog.Business.CultureType.Locale)
                fromName.Text = "<a href='../../redirect.aspx?i=" + e.Row.Cells.FromKey("ItemId").Text + "&c=" + DDL_Countries.SelectedValue + "&p=UI/Globalize/qdetranslate.aspx' target='_BLANK'\">" + fromName.Text + "</a>";
            else
                fromName.Text = "<a href='../../redirect.aspx?p=UI/Acquire/qde.aspx&i=" + e.Row.Cells.FromKey("ItemId").Text + "' target='_BLANK'\">" + fromName.Text + "</a>";
        }
        if (toName != null)
        {
             if (curCultureType == HyperCatalog.Business.CultureType.Locale)
                 toName.Text = "<a href='../../redirect.aspx?i=" + e.Row.Cells.FromKey("SubItemId").Text + "&c=" + DDL_Countries.SelectedValue + "&p=UI/Globalize/qdetranslate.aspx' target='_BLANK'\">" + toName.Text + "</a>";
            else
                toName.Text = "<a href='../../redirect.aspx?p=UI/Acquire/qde.aspx&i=" + e.Row.Cells.FromKey("SubItemId").Text + "' target='_BLANK'\">" + toName.Text + "</a>";
        
        }
        #endregion
    }

    protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
        string btn = be.Button.Key.ToLower();
        // checking PLCStatus filter
        if (statusFilter.SelectedValue == string.Empty || statusFilter.SelectedValue == "")
        {
            dg.Visible = false;
            lbMessage.CssClass = "hc_error";
            lbMessage.Text = "Please select atleast one of the values for PLC Status.Its a mandatory field";
        }
        else
        {
            #region Export
            if (btn == "export")
            {
                UpdateExport();


            }

            #endregion

            #region Generate
            if (btn == "generate")
            {

                UpdateDataView();

            }
            #endregion
            #region OrderAnomalies
            if (btn == "anomalies")
            {

                ExportOrderAnomalies();

            }
             #endregion
        }
    }
}
