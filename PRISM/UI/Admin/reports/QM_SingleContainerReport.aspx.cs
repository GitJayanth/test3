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

public partial class QM_SingleContainerReport : System.Web.UI.Page{

    DataSet dsContainer = new DataSet();
    //DataTable objDT = new DataTable();
    protected string regionCode
    {
        get { return (string)ViewState["regionCode"]; }
        set { ViewState["regionCode"] = value; }
    }
    protected DataTable objDT
    {
        get { return (DataTable)ViewState["objDT"]; }
        set { ViewState["objDT"] = value; }
    }
    //string regionCode;
    string ContainerId;
    string BizCode;
    protected void Page_Load(object sender, EventArgs e)
    {                                                                          
        //ContainerId=" + contNameId + "&BizName=" + BizName + "&Code="+code+ "&regionCode=" + regionCode);
        ContainerId = Convert.ToString(Request.QueryString["ContainerId"]);
        BizCode = Convert.ToString(Request.QueryString["BizCode"]);
        string Code = Convert.ToString(Request.QueryString["Code"]);
        this.regionCode = Convert.ToString(Request.QueryString["regionCode"]);
        try
        {

            Database dbObj = new Database(HyperCatalog.Business.ApplicationSettings.Components["Datawarehouse_DB"].ConnectionString, 5000);
           
            dsContainer = dbObj.RunSPReturnDataSet("_Reports_QM_Compute_Statistics_Single_Container", new SqlParameter("@ContainerId", ContainerId),
                                             new SqlParameter("@BizCode", BizCode),
                                             new SqlParameter("@Code",Code),
                                             new SqlParameter("@RegionCode", this.regionCode));
            string Biz = "";
            if (Code.Equals("0"))
            {
                Biz = "Selected OrgName";
            }
            else if (Code.Equals("1"))
            {
                Biz = "Selected GroupName";
            }
            else if (Code.Equals("2"))
            {
                Biz = "Selected GBUName";
            }
            DataSet dsBizNames = dbObj.RunSPReturnDataSet("_GetSelectedBusinessNames",
                                         new SqlParameter("@BizCode", BizCode),
                                         new SqlParameter("@Code", Code));
            string BizList = "";
            foreach (DataRow r in dsBizNames.Tables[0].Rows)
            {

                BizList += r[0].ToString() + ",";
            }
            BizList = BizList.Remove(BizList.Length - 1, 1);
            //Label2.Text = BizList;
            Label1.Text = Biz + ": " + BizList;
            Label1.Font.Size = 8;
            Label1.ForeColor = System.Drawing.Color.DarkSlateGray;
            if (dsContainer.Tables[0].Rows.Count != 0)
            {

                QMReportsClass objQMReports = new QMReportsClass();
                objDT = objQMReports.GetConsolidatedData(dsContainer, this.regionCode);

                UltraWebGrid1.DisplayLayout.ViewType = Infragistics.WebUI.UltraWebGrid.ViewType.Hierarchical;

                UltraWebGrid1.DataSource = objDT;
                UltraWebGrid1.DataBind();
                this.UltraWebGrid1.Height = Unit.Empty;
                this.UltraWebGrid1.Width = Unit.Empty;

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
        catch (Exception ee)
        {
            //Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "ClientScript", "alert('Records not available for the selected input. Please select Inputs once again ');", true);
            Response.Write(ee.Message.ToString());
        }
    }

    //This method initializes the format of the UltraWebGrid
    // In this method the multiple headers are drawn and also the header caption and the 
    // appearance of the columns are initialized
    public void UltraWebGrid1_InitializeLayout(object sender, 
    Infragistics.WebUI.UltraWebGrid.LayoutEventArgs e)
    {
        QMReportsClass objQMReports = new QMReportsClass();
        objQMReports.ContainerViewUltraGrid_Initialize(e, this.regionCode);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        QMReportsClass objQMReports = new QMReportsClass();
        string InputFormName = null;
        QMReportsClass.ExportContainer(objDT, Page, this.regionCode,Label1.Text, InputFormName);
    }
    

}
   
