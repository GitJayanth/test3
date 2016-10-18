using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Data.OleDb;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Infragistics.WebUI.UltraWebGrid;
using HyperCatalog.Shared;
using HyperComponents.Data.dbAccess;
using HyperCatalog.Business;
using HyperCatalog.UI.Tools;
using System.Xml;


//Authur: Ramachandran
public partial class SummaryReport : System.Web.UI.Page
{
    string ItemIds;
    DataSet objDS;
    DataTable objDT_ItemName;
    DataTable objDT;
    string CultureCode;
    Boolean IsRoll;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Database dbObj = new Database(HyperCatalog.Business.ApplicationSettings.Components["Crystal_DB"].ConnectionString);
            ItemIds = Convert.ToString(Request.QueryString["ItemIds"]);
            CultureCode = Convert.ToString(Request.QueryString["CultureCode"]);
            IsRoll = Convert.ToBoolean(Request.QueryString["IsRoll"]);
            Label1.Text = "Selected Culture: " + CultureCode;
            Label1.Font.Size = 8;
            Label1.ForeColor = System.Drawing.Color.DarkSlateGray;
            if (IsRoll)
            {
                Label2.Visible = true;
                Label2.Text = "Generated for SoftRoll Items";
                Label2.Font.Size = 8;
                Label2.ForeColor = System.Drawing.Color.DarkSlateGray;
            }
            else
            {
                Label2.Width = 0;
                Label2.Height = 0;
                Label2.Visible = false;
            }
            objDS = dbObj.RunSPReturnDataSet("_GetSummaryReport",
                             new SqlParameter("@ItemIds", ItemIds),
                             new SqlParameter("@CultureCode", CultureCode));
            UltraWebGrid1.DataSource = createSummaryReportData(objDS, ItemIds); ;
            UltraWebGrid1.DataBind();
            UltraWebGrid1.DisplayLayout.ViewType = Infragistics.WebUI.UltraWebGrid.ViewType.Hierarchical;
            UltraWebGrid1.Height = Unit.Empty;
            UltraWebGrid1.Width = Unit.Empty;
            int count = 1;
            foreach (UltraGridRow UR in UltraWebGrid1.Rows)
            {
                count = 1;
                // Code for UI Gimmicks 
                while (count < UR.Cells.Count)
                {
                    if (UR.Cells[count].Key.Contains("M"))
                    {

                        if (UR.Cells[count].Text.Equals("True"))
                        {
                            UR.Cells[count].Text = "<img src='/hc_v4/img/M.gif' border=0/>";

                        }
                        else
                        {
                            UR.Cells[count].Text = String.Empty;
                        }

                    }
                    if (UR.Cells[count].Key.Contains("S") && !UR.Cells[count].Key.Contains("Source"))
                    {
                        if (UR.Cells[count].Text.Equals("M"))
                        {
                            UR.Cells[count].Text = "<img src='/hc_v4/img/SM.gif' border=0/>";

                        }
                        else if (UR.Cells[count].Text.Equals("F"))
                        {
                            UR.Cells[count].Text = "<img src='/hc_v4/img/Sf.gif' border=0/>";
                        }
                        /* Alternate for CR 5096(Removal of rejection functionality)
                        else if (UR.Cells[count].Text.Equals("R"))
                        {
                            UR.Cells[count].Text = "<img src='/hc_v4/img/SR.gif' border=0/>";
                        } */
                        else
                        {
                            UR.Cells[count].Text = "<img src='/hc_v4/img/SD.gif' border=0/>";
                        }
                    }
                    if (UR.Cells[count].Key.Contains("ChunkValue"))
                    {
                        if (UR.Cells[count].Text.Split(new Char[] { '~' }, 2)[0].Equals("True"))
                        {
                            UR.Cells[count].Text = UR.Cells[count].Text.Split(new Char[] { '~' }, 2)[1];
                            UR.Cells[count].Style.BackColor = System.Drawing.Color.Gainsboro;
                            UR.Cells[count].Style.Font.Italic = true;
                        }
                        else
                        {
                            UR.Cells[count].Text = UR.Cells[count].Text.Split(new Char[] { '~' }, 2)[1];
                        }

                    }
                    count++;
                }
            }

        }
        catch (Exception ex)
        {
            Response.Write(ex.Message.ToString());
        }
    }

    public object[] GetDistinctValues(DataRow[] dRowCol, string colName)
    {
        Hashtable hTable = new Hashtable();
        foreach (DataRow drow in dRowCol)
            if (!hTable.ContainsKey(drow[colName]))
                hTable.Add(drow[colName], string.Empty);
        object[] objArray = new object[hTable.Keys.Count];
        hTable.Keys.CopyTo(objArray, 0);
        return objArray;
    }

    public DataTable createSummaryReportData(DataSet objDS, string ItemIds)
    {
        objDT = new DataTable();

        string[] ItemId = ItemIds.Split(',');
        object[] objContainerIds = GetDistinctValues(objDS.Tables[1].Select(), "ContainerId");
        objDT.Columns.Add(new DataColumn("ContainerName"));
        for (int i = 0; i < ItemId.Length; i++)
        {
            objDT.Columns.Add(new DataColumn(ItemId[i] + "~M"));
            objDT.Columns.Add(new DataColumn(ItemId[i] + "~S"));
            objDT.Columns.Add(new DataColumn(ItemId[i] + "~Source"));
            objDT.Columns.Add(new DataColumn(ItemId[i] + "~ChunkValue"));
            
            
            //objDT.Columns.Add(new DataColumn(ItemId[i] + "~I"));
        }
        DataRow[] drCurrent = null;
        for (int i = 0; i < objContainerIds.Length; i++)
        {
            DataRow dr = objDT.NewRow();
            drCurrent = objDS.Tables[1].Select("ContainerId ='" + objContainerIds[i].ToString() + "'");
            dr["ContainerName"] = drCurrent[0]["ContainerName"];

            for (int cnt = 0; cnt < ItemId.Length; cnt++)
            {

                drCurrent = objDS.Tables[1].Select("ContainerId = '" + objContainerIds[i].ToString() + "' and ItemId = '" + ItemId[cnt] + "'");
                if (drCurrent.Length != 0)
                {
                    dr[ItemId[cnt] + "~S"] = drCurrent[0]["ChunkStatus"];
                    dr[ItemId[cnt] + "~M"] = drCurrent[0]["IsMandatory"];
                    dr[ItemId[cnt] + "~Source"] = drCurrent[0]["CultureCode"];
                    //QC 870- handling the ILBs in the code
                    //Modified by Kanthi.J
                    if (drCurrent[0]["ChunkValue"].ToString().Equals("\0"))
                        drCurrent[0]["ChunkValue"] = "##BLANK##";
                    dr[ItemId[cnt] + "~ChunkValue"] = drCurrent[0]["Inherited"] + "~" + drCurrent[0]["ChunkValue"];
                   
                    
                }
                else
                {
                    //If the Item doesnot have the Container then its initialized with the default value here
                    dr[ItemId[cnt] + "~S"] = "M";
                    dr[ItemId[cnt] + "~M"] = "False";
                    dr[ItemId[cnt] + "~Source"] = "ww-en";
                    dr[ItemId[cnt] + "~ChunkValue"] = "False~" + String.Empty;
                }

            }
            objDT.Rows.Add(dr);
        }
        objDT.AcceptChanges();
        return objDT;
    }
    public void UltraWebGrid1_InitializeLayout(object sender,
Infragistics.WebUI.UltraWebGrid.LayoutEventArgs e)
    {
        string[] ItemId = ItemIds.Split(',');
        foreach (Infragistics.WebUI.UltraWebGrid.UltraGridColumn c in e.Layout.Bands[0].Columns)
        {
            c.Width = Unit.Pixel(10);
            c.Header.ClickAction = HeaderClickAction.Select;
            c.Header.Style.HorizontalAlign = HorizontalAlign.Center;
            c.Header.Style.VerticalAlign = VerticalAlign.Middle;
            c.Header.Style.Font.Bold = true;
            c.AllowResize = AllowSizing.NotSet;
            c.AllowUpdate = AllowUpdate.No;
            c.CellStyle.Wrap = true;
            if (c.Key.Equals("ContainerName"))
            {
                c.Header.RowLayoutColumnInfo.SpanY = 2;
            }
            else
            {
                c.Header.RowLayoutColumnInfo.OriginY = 1;
            }
            if (c.Key.Contains("~"))
            {
                c.Header.Caption = c.Key.Split('~')[1];
            }    
            if (c.Key.Contains("ChunkValue"))
            {
                c.Width = Unit.Pixel(300);
            }

        }
        objDT_ItemName = new DataTable();
        objDT_ItemName.Columns.Add("ItemName");
        DataRow[] drCurrent = null;
        for (int i = 0; i < ItemId.Length; i++)
        {
            DataRow dr = objDT_ItemName.NewRow();
            drCurrent = objDS.Tables[0].Select("ItemId = '" + ItemId[i] + "'");
            if (drCurrent[0]["LevelId"].ToString().Equals("7"))
            {
                dr["ItemName"] = drCurrent[0]["ItemNumber"] + " [" + drCurrent[0]["LevelName"].ToString() + "]";
            }
            else
            {
                dr["ItemName"] = drCurrent[0]["ItemName"] + " [" + drCurrent[0]["LevelName"].ToString() + "]";
            }
            objDT_ItemName.Rows.Add(dr);
        }
        objDT_ItemName.AcceptChanges();
        int count = 1;
        int headerindex = 0;
        foreach (DataRow r in objDT_ItemName.Rows)
        {
            Infragistics.WebUI.UltraWebGrid.ColumnHeader ch = new ColumnHeader(true);
            ch.Style.Width = Unit.Pixel(0);
            ch.Caption = r["ItemName"].ToString();
            ch.Style.HorizontalAlign = HorizontalAlign.Center;
            ch.Style.VerticalAlign = VerticalAlign.Middle;
            ch.Style.Font.Bold = true;
            ch.Style.Width = Unit.Pixel(330);
            ch.RowLayoutColumnInfo.OriginY = 0;
            ch.RowLayoutColumnInfo.OriginX = count;
            ch.RowLayoutColumnInfo.SpanX = 4;
            //count takes care of SpanX for the ItemName/ItemNumber Header
            count = ch.RowLayoutColumnInfo.OriginX + ch.RowLayoutColumnInfo.SpanX;
            e.Layout.Bands[0].HeaderLayout.Add(ch);
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        // Generate Logic for Summary Report
        System.Text.StringBuilder sb = new System.Text.StringBuilder(string.Empty);
        sb.Append("<html><head>");
        // fix for QC 2356 - Prabhu
        sb.Append("<meta http-equiv='Content-Type' content='text/html; charset=utf-8'>");
        sb.Append("</head><body>");
        sb.Append("<table border = '1'>");
        sb.Append("SummaryReport<br>");
        sb.Append("Selected Culture: " + CultureCode + "<br>");
        if (IsRoll)
        {
            sb.Append(Label2.Text + "<br>");
        }
        sb.Append(("Exported On:" + SessionState.User.FormatUtcDate(DateTime.UtcNow, true, "MM/dd/yyyy:HH:mm:ss") + "<br>"));
        sb.Append("Legends :<br>");
        sb.Append("    M Mandatory<br>");
        sb.Append("    S Status<br>");
        sb.Append("</tr><tr>");
        //DataColumnCollection tableColumns_ItemName = objDT_ItemName.Columns;
        DataColumnCollection tableColumns = objDT.Columns;
        //DataRowCollection tableRows = ds3.Tables[0].Rows;
                
        sb.Append("<th rowspan='2'>ContainerName</th>");
        foreach (DataRow r in objDT_ItemName.Rows)
        {
            sb.Append("<th colspan = '4'>" + r["ItemName"].ToString() + "</th>");
        }
        sb.Append("</tr><tr>");
        foreach (DataColumn c in tableColumns)
        {
            if (!c.ColumnName.ToString().Equals("ContainerName"))
            {
                sb.Append("<th>" + c.ColumnName.ToString().Split('~')[1] + "</th>");
            }
        }
        sb.Append("</tr>");
        foreach (DataRow dr in objDT.Rows)
        {
            sb.Append("<tr>");
            foreach (DataColumn dc in objDT.Columns)
            {
                if (dc.ColumnName.Contains("ChunkValue"))
                {
                    if (dr[dc].ToString().Split(new char[] { '~' }, 2)[0].Equals("True"))
                    {
                        sb.Append("<td bgcolor = 'Gainsboro'><I>" + dr[dc].ToString().Split(new char[] { '~' }, 2)[1] + "</I></td>");
                    }
                    else
                    {
                        sb.Append("<td>" + dr[dc].ToString().Split(new char[] { '~' }, 2)[1] + "</td>");
                    }
                }
                else
                {
                    sb.Append("<td>" + dr[dc].ToString() + "</td>");
                }
            }
            sb.Append("</tr>");
        }

        sb.Append("</table>");
        sb.Append("</body></html>");
        string fileName = string.Empty;
        fileName += "SummaryReport.xls";

        string exportContent = sb.ToString();
        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();
        Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
        Response.ContentType = "application/vnd.ms-excel";
        //Fix for CR 5109 - Prabhu R S
        Response.ContentEncoding = System.Text.Encoding.UTF8;
        Response.AppendHeader("Content-Length", System.Text.Encoding.UTF8.GetByteCount(exportContent).ToString());
        EnableViewState = false;
        Response.Write(exportContent);
        Response.End();
    }
}
