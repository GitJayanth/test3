
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
//using HyperCatalog.Shared;
using HyperComponents.Data.dbAccess;
using HyperCatalog.Business;
//using HyperCatalog.UI.Tools;

public partial class QM_InputForms : System.Web.UI.Page
{
   DataSet dsItems = new DataSet();
   //DataTable objDT = new DataTable();
    protected DataTable objDT
    {
        get { return (DataTable)ViewState["objDT"]; }
        set { ViewState["objDT"] = value; }
    }
   string regionCode;
   
    string code;

  
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {

                string InputFormIds = Convert.ToString(Request.QueryString["InputFormIds"]);
                                                    

                string BizCode = Convert.ToString(Request.QueryString["BizCode"]);
                code = Convert.ToString(Request.QueryString["Code"]);
                string Biz = "";
                if (code.Equals("0"))
                {
                    Biz = "Selected OrgName";
                }
                else if (code.Equals("1"))
                {
                    Biz = "Selected GroupName";
                }
                else if (code.Equals("2"))
                {
                    Biz = "Selected GBUName";
                }
                //Label1.Text = Biz+;
                
                regionCode = Convert.ToString(Request.QueryString["regionCode"]);
                
                DataView dv = new DataView();
                                                                        
                Database dbObj = new Database(HyperCatalog.Business.ApplicationSettings.Components["Datawarehouse_DB"].ConnectionString);

                dsItems = dbObj.RunSPReturnDataSet("_Reports_QM_Compute_InputForms_Statistics",
                             new SqlParameter("@_Input_Form_Ids", InputFormIds),
                             new SqlParameter("@BizCode", BizCode),
                             new SqlParameter("@Code",code),
                             new SqlParameter("@RegionCode", regionCode));
                DataSet dsBizNames = dbObj.RunSPReturnDataSet("_GetSelectedBusinessNames",
                             new SqlParameter("@BizCode", BizCode),
                             new SqlParameter("@Code", code));
                string BizList = "";
                foreach (DataRow r in dsBizNames.Tables[0].Rows)
                {

                    BizList += r[0].ToString()+",";
                }
                BizList = BizList.Remove(BizList.Length - 1, 1);
                //Label2.Text = BizList;
                Label1.Text = Biz +": "+ BizList;
                Label1.Font.Size = 8;
                Label1.ForeColor = System.Drawing.Color.DarkSlateGray;
                Label3.Visible = false;
                //foreach(dsBizNames.Tables[0].Rows
                if (dsItems.Tables[0].Rows.Count != 0)
                {
                    QMReportsClass objQMReports = new QMReportsClass();
                    objDT = objQMReports.GetInputFormData(dsItems, BizCode, code, regionCode);
                    UltraWebGrid1.DataSource = objDT;
                    UltraWebGrid1.DataBind();
                    UltraWebGrid1.DisplayLayout.ViewType = Infragistics.WebUI.UltraWebGrid.ViewType.Hierarchical;
                    this.UltraWebGrid1.Height = Unit.Empty;
                    this.UltraWebGrid1.Width = Unit.Empty;
                    if (objDT.Rows.Count != InputFormIds.Split(',').Length)
                    {

                       // Response.Write("Records for some of the InputForms are not available");
                        Label3.Visible = true;
                        Label3.Text = "Records for some of the InputForms are not available";
                        Label3.Font.Size = 8;
                        Label3.ForeColor = System.Drawing.Color.DarkSlateGray;
                        
                    }
                }
                else
                {
                    //Response.Write("Records not available for the selected Inputs");
                    Label1.Text = "Records not available for the selected Inputs";
                    UltraWebGrid1.Visible = false;
                    btnExport.Visible = false;
                    Label1.Visible = true;
                }
            }            
            catch (Exception ex)
            {
               // Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "ClientScript", "alert('Records not available for the selected input');", true);
                Response.Write(ex.Message);
            }
        }
    
    }

    //This method initializes the format of the UltraWebGrid
    // In this method the multiple headers are drawn and also the header caption and the 
    // appearance of the columns are initialized      
    public void UltraWebGrid1_InitializeLayout(object sender,
      Infragistics.WebUI.UltraWebGrid.LayoutEventArgs e)
    {
        string regionCode = Convert.ToString(Request.QueryString["regionCode"]);
        string[] split_RegionCode = regionCode.Split(',');
        int counter = 0;
        int parentHeaderCount = 0;
        int childHeaderCount = 0;
        int parentHeaderOriginX = 0;
        int childHeaderOriginX = 0;
        int regionCodeCount = split_RegionCode.Length;
        foreach (Infragistics.WebUI.UltraWebGrid.UltraGridColumn c in e.Layout.Bands[0].Columns)
        {
            c.Header.RowLayoutColumnInfo.OriginY = 4;
        }

        string[] parentHeader = new string[] { "Live", "Future", "Obsolete" };
        //string[] childHeader = new string[] { "Final", "Final#", "FinalILB", "FinalILB#", "Draft", "Draft#", "DraftILB", "DraftILB#", "MisRej", "MisRej#", "MisRejILB", "MisRejILB#" };
        //string[] childHeader = new string[] { "Final", "#", "ILB", "#", "Draft", "#", "ILB", "#", "MisRej", "#", "ILB", "#" };
        string[] childHeader = new string[] { "/hc_v4/img/status2.gif", "#", "ILB", "#", "/hc_v4/img/status0.gif", "#", "/hc_v4/img/status-1.gif", "#" };
        /* Alternate for CR 5096(Removal of rejection functionality)--start
        string[] toolTip = new string[] { "Final Mandatory Chunks", "Draft Mandatory Chunks", "Rejected/Missing Mandatory Chunks" }; */
        string[] toolTip = new string[] { "Final Mandatory Chunks", "Draft Mandatory Chunks", "Missing Mandatory Chunks" };
        // Alternate for CR 5096(Removal of rejection functionality)--end
        int imgCount = 0;
        while (parentHeaderCount < parentHeader.Length)
        {
            Infragistics.WebUI.UltraWebGrid.ColumnHeader hh = new ColumnHeader(true);
            hh.Caption = parentHeader[parentHeaderCount];
            hh.Style.HorizontalAlign = HorizontalAlign.Center;
            hh.Style.VerticalAlign = VerticalAlign.Middle;
            hh.Style.Font.Bold = true;
         
            hh.RowLayoutColumnInfo.OriginY = 0;

            hh.RowLayoutColumnInfo.OriginX = parentHeaderOriginX + 1;
            hh.RowLayoutColumnInfo.SpanX = 4 * regionCodeCount + 4;
            parentHeaderOriginX = parentHeaderOriginX + hh.RowLayoutColumnInfo.SpanX;
            e.Layout.Bands[0].HeaderLayout.Add(hh);
            parentHeaderCount++;
            childHeaderCount = 0;
            imgCount = 0;
            while (childHeaderCount < childHeader.Length)
            {
                Infragistics.WebUI.UltraWebGrid.ColumnHeader ch = new ColumnHeader(true);
                //ch.Caption = childHeader[childHeaderCount];
                ch.Image.Url = childHeader[childHeaderCount];
                ch.Style.HorizontalAlign = HorizontalAlign.Center;
                ch.Style.VerticalAlign = VerticalAlign.Middle;
                ch.Style.Font.Bold = true;
                if (ch.HasImage)
                {
                    ch.Image.AlternateText = toolTip[imgCount];
                    imgCount++;
                }
                ch.RowLayoutColumnInfo.OriginY = 1;

                ch.RowLayoutColumnInfo.OriginX = childHeaderOriginX + 1;
                if (childHeaderCount == 0)
                {
                    ch.RowLayoutColumnInfo.SpanX = 2 * (regionCodeCount + 1);
                }
                else
                {
                    ch.RowLayoutColumnInfo.SpanX = regionCodeCount + 1;
                }
                
                childHeaderOriginX = childHeaderOriginX + ch.RowLayoutColumnInfo.SpanX;

                e.Layout.Bands[0].HeaderLayout.Add(ch);

                childHeaderCount++;
                Infragistics.WebUI.UltraWebGrid.ColumnHeader ch1 = new ColumnHeader(true);
                ch1.Caption = childHeader[childHeaderCount];
                ch1.Style.HorizontalAlign = HorizontalAlign.Center;
                ch1.Style.VerticalAlign = VerticalAlign.Middle;
                ch1.Style.Font.Bold = true;
                ch1.RowLayoutColumnInfo.OriginY = 2;

                ch1.RowLayoutColumnInfo.OriginX = ch.RowLayoutColumnInfo.OriginX + 1;

                ch1.RowLayoutColumnInfo.SpanX = regionCodeCount;


                e.Layout.Bands[0].HeaderLayout.Add(ch1);
                childHeaderCount++;
                if (childHeaderCount < childHeader.Length - 1)
                {
                    if (childHeader[childHeaderCount].Equals("ILB"))
                    {
                        Infragistics.WebUI.UltraWebGrid.ColumnHeader ch2 = new ColumnHeader(true);
                        ch2.Caption = childHeader[childHeaderCount];
                        ch2.Style.HorizontalAlign = HorizontalAlign.Center;
                        ch2.Style.VerticalAlign = VerticalAlign.Middle;
                        ch2.Style.Font.Bold = true;
                        ch2.RowLayoutColumnInfo.OriginY = 2;
                        ch2.RowLayoutColumnInfo.OriginX = ch1.RowLayoutColumnInfo.OriginX + regionCodeCount;
                        ch2.RowLayoutColumnInfo.SpanX = regionCodeCount + 1;

                        e.Layout.Bands[0].HeaderLayout.Add(ch2);

                        childHeaderCount++;
                        Infragistics.WebUI.UltraWebGrid.ColumnHeader ch3 = new ColumnHeader(true);
                        ch3.Caption = childHeader[childHeaderCount];
                        ch3.Style.HorizontalAlign = HorizontalAlign.Center;
                        ch3.Style.VerticalAlign = VerticalAlign.Middle;
                        ch3.Style.Font.Bold = true;
                        ch3.RowLayoutColumnInfo.OriginY = 3;
                        ch3.RowLayoutColumnInfo.OriginX = ch2.RowLayoutColumnInfo.OriginX + 1;
                        ch3.RowLayoutColumnInfo.SpanX = regionCodeCount;

                        e.Layout.Bands[0].HeaderLayout.Add(ch3);
                        childHeaderCount++;
                    }
                }

            }
        }

        
        foreach (Infragistics.WebUI.UltraWebGrid.UltraGridColumn c in e.Layout.Bands[0].Columns)
        {
            
            
            if (c.Key.Contains("InputForm"))
            {
                c.Header.RowLayoutColumnInfo.OriginY = 0;
                c.Header.RowLayoutColumnInfo.SpanY = 5;
               
                

            }
            else if ((c.Key.Contains("ILB") && !c.Key.Contains("~")) || (!c.Key.Contains("ILB") && c.Key.Contains("~")))
            {
                c.Header.RowLayoutColumnInfo.OriginY = 3;
                c.Header.RowLayoutColumnInfo.SpanY = 2;
                //c.Header.Caption = c.Key.Split('~')[1];
            }

            else if (c.Key.Contains("#"))
            {
                c.Header.RowLayoutColumnInfo.OriginY = 2;
                c.Header.RowLayoutColumnInfo.SpanY = 1;
            }
            else if (c.Key.Contains("Per") && (!c.Key.Contains("ILB")))
            {
                c.Header.RowLayoutColumnInfo.OriginY = 2;
                c.Header.RowLayoutColumnInfo.SpanY = 3;

            }
            if (c.Key.Contains("~"))
            {

                c.Header.Caption = c.Key.Split('~')[1].Split('-')[0].ToUpper();
            }
            if (c.Key.Contains("Per"))
            {

                c.Header.Caption = "%";
                c.CellStyle.Font.Bold = true;
            }

            if(c.Key.Contains("~"))
            {
                c.CellStyle.ForeColor = System.Drawing.Color.Green;
            }

            c.Width = Unit.Pixel(40);
            c.Header.ClickAction = HeaderClickAction.Select;
            c.Header.Style.HorizontalAlign = HorizontalAlign.Center;
            c.Header.Style.VerticalAlign = VerticalAlign.Middle;
            c.Header.Style.Font.Bold = true;
            c.AllowResize = AllowSizing.Fixed;
            c.AllowUpdate = AllowUpdate.No;
            if (c.Key.Contains("InputFormName"))
            {
                //c.CellStyle.BackColor = System.Drawing.Color.LightGray;
                //c.CellStyle.ForeColor = System.Drawing.Color.Red;
                c.CellStyle.Font.Bold = true;
                
            }
            
        }
    }
    
    protected void btnExport_Click(object sender, EventArgs e)
    {

        regionCode = Convert.ToString(Request.QueryString["regionCode"]);
        QMReportsClass.ExportInputForm(this.objDT, Page, regionCode, Label1.Text, Label3.Text);
    }

}


