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

public partial class QM_MultiContainerReport : System.Web.UI.Page
{
    DataSet dsContainers=new DataSet();
   // DataTable objDT = new DataTable();
    string regionCode;
    string InputFormId;
   string BizCode;
    string Code;
    protected DataTable objDT
    {
        get { return (DataTable)ViewState["objDT"]; }
        set { ViewState["objDT"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            InputFormId = Convert.ToString(Request.QueryString["FormId"]);
            BizCode = Convert.ToString(Request.QueryString["BizCode"]);
            Code = Convert.ToString(Request.QueryString["Code"]);
            regionCode = Convert.ToString(Request.QueryString["regionCode"]);
            
            Database dbObj = new Database(HyperCatalog.Business.ApplicationSettings.Components["Datawarehouse_DB"].ConnectionString);

            dsContainers = dbObj.RunSPReturnDataSet("_Reports_QM_Compute_InputForms_Containers",
                             new SqlParameter("@_Input_Form_Id", InputFormId),
                             new SqlParameter("@BizCode", BizCode),
                             new SqlParameter("@Code",Code),
                             new SqlParameter("@RegionCode", regionCode));
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
            //SqlDataReader IFName = dbObj.RunSQLReturnRS("select Name from InputForms where InputFormId = " + InputFormId);
            SqlDataReader IFName = dbObj.RunSQLReturnRS("select Name from InputForms where InputFormId ="+ @InputFormId, HyperComponents.Data.dbAccess.Database.NewSqlParameter("@InputFormId", System.Data.SqlDbType.Text, InputFormId.Length,InputFormId));
            
            
            string InputFormName = string.Empty;
            while (IFName.Read())
            {
                InputFormName = IFName.GetString(0);
            }
            Label2.Text = "Selected InputForm: " + InputFormName;
            Label2.Font.Size = 8;
            Label2.ForeColor = System.Drawing.Color.DarkSlateGray;
            if (dsContainers.Tables[0].Rows.Count != 0)
            {
                QMReportsClass objQMReports = new QMReportsClass();
                this.objDT = objQMReports.GetConsolidatedData(dsContainers, regionCode);


                UltraWebGrid1.DisplayLayout.ViewType = Infragistics.WebUI.UltraWebGrid.ViewType.Hierarchical;

                UltraWebGrid1.DataSource = objDT;
                UltraWebGrid1.DataBind();
                this.UltraWebGrid1.Height = Unit.Empty;
                this.UltraWebGrid1.Width = Unit.Empty;
            }
            else
            {
               // Response.Write("Records not available for the selected Inputs");
                Label1.Text = "Records not available for the selected Inputs";
                UltraWebGrid1.Visible = false;
                btnExport.Visible = false;
                Label1.Visible = true;
            }
            IFName.Close();
        }
        catch (Exception ex)
        {

            //Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "ClientScript", "alert('Records not available for the selected input');", true);
            Response.Write(ex.Message.ToString());
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
        InputFormId = Convert.ToString(Request.QueryString["FormId"]);
        regionCode = Convert.ToString(Request.QueryString["regionCode"]);
        QMReportsClass.ExportContainer(this.objDT, Page, regionCode, Label1.Text, Label2.Text);

        
    }
}

