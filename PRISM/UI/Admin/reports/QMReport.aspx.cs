using System;
using System.Data;
using System.Text; 
using System.Data.SqlClient; 
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HyperComponents.Data.dbAccess;
using Infragistics.WebUI.UltraWebGrid;

public partial class QMReport : System.Web.UI.Page
{
    //public int code;
    protected int code
    {
        get { return (int)ViewState["code"]; }
        set { ViewState["code"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {



            if (!Page.IsPostBack)
            {

                //code = 0;
                this.code = 0;
                OrgName.Checked = true;
                rbtnInputForm.Checked = true;
                populateLstBoxBusinessUnitName();
                Database dbObj = new Database(HyperCatalog.Business.ApplicationSettings.Components["Datawarehouse_DB"].ConnectionString);
                //Response.Write(getSelectedValues(LstBoxBusinessUnitName));        
                LstBoxRegionName.DataSource = dbObj.RunSPReturnDataSet("dbo._GetRegion");
                LstBoxRegionName.DataValueField = "ShortCode";
                LstBoxRegionName.DataTextField = "RegionName";
                LstBoxRegionName.DataBind();
                LstBoxRegionName.Items.Insert(0, "All");
                LstBoxRegionName.SelectedIndex = 0;
                dbObj.CloseConnection();
                dbObj.Dispose();
                populateInputFormLstBox();

                //pnlInputForm.Visible = true;

            }
            //populateLstBoxBusinessUnitName();
            if (rbtnInputForm.Checked)
            {
                pnlInputForm.Visible = true;
                pnlSearchRes.Visible = false;
            }
            else if (rbtnContainer.Checked)
            {
                pnlInputForm.Visible = false;
                pnlSearchRes.Visible = true;


            }
        }
        catch (Exception ex)
        {
            // Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "ClientScript", "alert('Records not available for the selected input');", true);
            Response.Write(ex.ToString());
        }
    }



    protected void btnNext_Click(object sender, EventArgs e)
    {
        try
        {
            QMReportsClass objQMReports = new QMReportsClass();
            string InputFormID = getSelectedInputForm(lstboxInputFormName);
            string BizCode = objQMReports.getSelectedValues(LstBoxBusinessUnitName);
            string regionCode = objQMReports.getSelectedValues(LstBoxRegionName);
            if (BizCode == null || regionCode == null || InputFormID == null)
            {
                StringBuilder errormsg = new StringBuilder();
                errormsg.Append(String.Empty);
                errormsg.Append("Please Select :");
                if (BizCode == null)
                {
                    
                    errormsg.Append("  " + Label1.Text);
                }
                if (regionCode == null)
                {
                    errormsg.Append("  regionCode");
                }
                if (InputFormID == null)
                {

                    errormsg.Append(" InputFormName");
                }
                 Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "ClientScript", "alert('" + errormsg + "');", true);
                btnNext.Enabled = false;
            }
            else{
            
                Response.Redirect("QM_InputForms.aspx?InputFormIds=" + InputFormID + "&BizCode=" + BizCode + "&Code=" + code + "&regionCode=" + regionCode);
            }
           


        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }
    protected void rbtnInputForm_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (rbtnInputForm.Checked)
            {
                pnlInputForm.Visible = true;
                pnlContainer.Visible = false;
               QMReportsClass objQMReports = new QMReportsClass();
                
                string BizCode = objQMReports.getSelectedValues(LstBoxBusinessUnitName);

                populateInputFormLstBox();
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());

        }
    }
    protected void rbtnContainer_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnContainer.Checked)
        {
            pnlContainer.Visible = true;
            pnlInputForm.Visible = false;
        }
    }
    protected void btnSearchCont_Click(object sender, EventArgs e)
    {
       
        pnlSearchRes.Visible = true;
        refreshContainerSearch();

    }

    protected void rbtnCheck_CheckedChanged(object sender, EventArgs e)
    {

       
    }

    protected void LstBoxBusinessUnitName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbtnInputForm.Checked)
        {
            pnlInputForm.Visible = true;
            pnlContainer.Visible = false;
            populateInputFormLstBox();
        }
        else if (rbtnContainer.Checked)
        {
            pnlContainer.Visible = true;
            if (uwgSearchRes.Visible == true)
            {
                refreshContainerSearch();
            }
        }

        }
    
    protected void gvSearchRes_RowCreated(object sender, GridViewRowEventArgs e)
    {


    }

    public string getSelectedInputForm(ListBox lstBox)
    {
        string selValues = string.Empty;
        for (int i = 0; i <= lstBox.Items.Count - 1; i++)
        {
            if (lstBox.Items[i].Selected)
                selValues += lstBox.Items[i].Value.Trim() + ",";
        }
        if (selValues.Length == 0)
        {
            return null;
        }
        else
        {
            selValues = selValues.Remove(selValues.Length - 1, 1);
            return selValues.Trim();
        }
    }

        
   

    protected void GBUName_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (GBUName.Checked)
            {
                this.code = 2;
                Label1.Text = "Select GBUName:";

                populateLstBoxBusinessUnitName();
                // rbtnCheck_CheckedChanged(object sender, EventArgs e);
                if (rbtnInputForm.Checked)
                {
                    pnlInputForm.Visible = true;
                    pnlContainer.Visible = false;
                   QMReportsClass objQMReports = new QMReportsClass();
                    string BizCode = objQMReports.getSelectedValues(LstBoxBusinessUnitName);
                    //OrgCode = OrgCode.Trim();
                    btnNext.Enabled = true;
                    populateInputFormLstBox();
                    //Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "ClientScript", "alert('No InputForm is available for. Please select another );", true);
                }
                else if (rbtnContainer.Checked)
                {
                    pnlContainer.Visible = true;

                }
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());

        }
    }

    protected void GroupName_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (GroupName.Checked)
            {
                this.code = 1;
                Label1.Text = "Select GroupName:";

                populateLstBoxBusinessUnitName();
                if (rbtnInputForm.Checked)
                {
                    pnlInputForm.Visible = true;
                    pnlContainer.Visible = false;
                    btnNext.Enabled = true;
                    populateInputFormLstBox();

                }
                else if (rbtnContainer.Checked)
                {
                    pnlContainer.Visible = true;

                }

            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }


    protected void OrgName_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (OrgName.Checked)
            {
                this.code = 0;
                Label1.Text = "Select OrgName:";
                populateLstBoxBusinessUnitName();
                if (rbtnInputForm.Checked)
                {
                    pnlInputForm.Visible = true;
                    pnlContainer.Visible = false;
                   QMReportsClass objQMReports = new QMReportsClass();
                    string BizCode = objQMReports.getSelectedValues(LstBoxBusinessUnitName);
                    btnNext.Enabled = true;
                    populateInputFormLstBox();
                }
                else if (rbtnContainer.Checked)
                {
                    pnlContainer.Visible = true;

                }
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message.ToString());
        }
    }



    protected void populateLstBoxBusinessUnitName()
    {
        Database dbObj = new Database(HyperCatalog.Business.ApplicationSettings.Components["Datawarehouse_DB"].ConnectionString);
        LstBoxBusinessUnitName.DataSource = dbObj.RunSPReturnDataSet("dbo._GetAllBusinessNames", new SqlParameter[] { new SqlParameter("@Code", code) });
        LstBoxBusinessUnitName.DataValueField = "Code";
        LstBoxBusinessUnitName.DataTextField = "BizName";
        LstBoxBusinessUnitName.DataBind();
        LstBoxBusinessUnitName.Items.Insert(0, "All");
        LstBoxBusinessUnitName.SelectedIndex = 0;
        if (LstBoxBusinessUnitName.Items.Count < 8)
            LstBoxBusinessUnitName.Height = LstBoxBusinessUnitName.Items.Count * 13;
        else
            LstBoxBusinessUnitName.Height = Unit.Pixel(90);
        //if (dbObj.RunSPReturnDataSet("dbo._GetAllBusinessNames").Tables.Count < 6)
        //    LstBoxBusinessUnitName.Height = dbObj.RunSPReturnDataSet("dbo._GetAllBusinessNames").Tables.Count * 13;
        //else
        //    LstBoxBusinessUnitName.Height = Unit.Pixel(75);

    }

    //This method populates the InputForm ListBox
    protected void populateInputFormLstBox()
    {
        
        rbtnInputForm.Enabled = true;
        string Biz = "";
        Database dbObj = new Database(HyperCatalog.Business.ApplicationSettings.Components["Datawarehouse_DB"].ConnectionString);
       QMReportsClass objQMReports = new QMReportsClass();
        string BizCode = objQMReports.getSelectedValues(LstBoxBusinessUnitName);
        if (BizCode != null)
        {
            DataSet ds = dbObj.RunSPReturnDataSet("dbo._Reports_QM_GetInputFormsByOrganization", new SqlParameter("@BizCode", BizCode), new SqlParameter("@Code", this.code));
            
            if (ds.Tables[0].Rows.Count != 0)
            {
                //*******************************************************************************
                //    code added by kanthi.J to shrink the sizeof of the inputform ListBox so that 
                //    the ListBox box doesnot display blank space if the rows returned by the dataset is 
                //    less than the 15.(180 px can accomodate 14 rows)
                //*******************************************************************************

                if (ds.Tables[0].Rows.Count < 15)
                    lstboxInputFormName.Height = ds.Tables[0].Rows.Count * 13;
                else
                    lstboxInputFormName.Height = Unit.Pixel(180);

                lstboxInputFormName.DataSource = ds;
                lstboxInputFormName.DataValueField = "InputFormId";
                lstboxInputFormName.DataTextField = "Name";
                lstboxInputFormName.DataBind();
              
                lstboxInputFormName.SelectedIndex = 0;
                dbObj.CloseConnection();
                dbObj.Dispose();
            }
            else
            {
                BizCode = objQMReports.getSelectedValues(LstBoxBusinessUnitName);
                if (code == 0)
                {
                    Biz = "OrgName";
                }
                else if (code == 1)
                {
                    Biz = "GroupName";
                }
                else if (code == 2)
                {
                    Biz = "GBUName";
                }
                StringBuilder sb = new StringBuilder();
                sb.Append("alert('No InputForm is available for the selected Input. Please select another " + Biz + "');");
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "ClientScript", sb.ToString(), true);
                lstboxInputFormName.Items.Clear();
                btnNext.Enabled = false;
            }
        }
        else
        {
            string BizUnit = "";

            if (this.code == 0)
            {
                BizUnit = "OrgName";
            }
            else if (this.code == 1)
            {
                BizUnit = "GroupName";
            }
            else if (this.code == 2)
            {
                BizUnit = "GBUName";
            }
            string ErrorMessage = "Please Select atleast one " + BizUnit;
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "ClientScript", "alert('" + ErrorMessage + "' );", true);
            lstboxInputFormName.Items.Clear();
            btnNext.Enabled = false;
        }
    }
  
    //This method initializes the Ultra Grid for Container Search Results
    public void uwgSearchRes_InitializeLayout(object sender,
     Infragistics.WebUI.UltraWebGrid.LayoutEventArgs e)
    {

        foreach (Infragistics.WebUI.UltraWebGrid.UltraGridColumn c in e.Layout.Bands[0].Columns)
        {
            //  c.Header.RowLayoutColumnInfo.OriginY = 4;
            if (c.Header.Caption.Equals("ContainerName") || c.Header.Caption.Equals("Select a Container"))
            {
                c.Header.Caption = "Select a Container";
                c.Hidden = false;
               // c.CellStyle.ForeColor = System.Drawing.Color.PowderBlue;
                c.CellStyle.Font.Bold = true;
            }
            else
            {
                c.Hidden = true;
            }
        }

    }
    protected void lstboxInputFormName_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnNext.Enabled = true;
    }
    protected void LstBoxRegionName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (btnNext.Visible)
        {
            btnNext.Enabled = true;
        }
        else
        {
            refreshContainerSearch();
        }
    }

    //This method refreshes the ContainerSerach Results
    protected void refreshContainerSearch()
    {
        uwgSearchRes.Visible = false;
        Database dbObj = new Database(HyperCatalog.Business.ApplicationSettings.Components["Datawarehouse_DB"].ConnectionString);
        if (txtContainer.Text != null)
        {
            txtContainer.Text = txtContainer.Text.Trim();
    }
    if (txtContainer.Text.Length != 0)
        {
            DataSet dsSearchResult = dbObj.RunSPReturnDataSet("dbo._Reports_QM_ContainerSearch", new SqlParameter[] { new SqlParameter("@InputText", txtContainer.Text.Trim() + "*") });

            string contNameId = "";
            QMReportsClass objQMReports = new QMReportsClass();
            string regionCode = objQMReports.getSelectedValues(LstBoxRegionName);
            string BizCode = objQMReports.getSelectedValues(LstBoxBusinessUnitName);
            //OrgCode = OrgCode.Trim();
            string url = "";
            DataSet dsContainerResults;

            if (BizCode != null && regionCode != null)
            {
                if (dsSearchResult.Tables[0].Rows.Count != 0)
                {
                    for (int i = 0; i < dsSearchResult.Tables[0].Rows.Count; i++)
                    {
                        contNameId = Convert.ToString(dsSearchResult.Tables[0].Rows[i]["ContainerId"]).Trim();
                        dsSearchResult.Tables[0].Rows[i]["ContainerName"] = "<a href='QM_SingleContainerReport.aspx?ContainerId=" + contNameId + "&BizCode=" + BizCode + "&Code=" + code + "&regionCode=" + regionCode + "'>" + Convert.ToString(dsSearchResult.Tables[0].Rows[i]["ContainerName"]) + "</a>";
                        dsSearchResult.AcceptChanges();
                        uwgSearchRes.DataSource = dsSearchResult;
                        uwgSearchRes.DataBind();
                        uwgSearchRes.Visible = true;
                    }
                }
                else
                {
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "ClientScript", "alert('No Mandatory Containers are found for the search. Please try once again' );", true);
                    uwgSearchRes.Visible = false;
                }
            }
            else
            {
                StringBuilder ErrorMessage = new StringBuilder();
                if (BizCode == null)
                {
                    string BizUnit = "";
                    if (this.code == 0)
                    {
                        BizUnit = "OrgName";
                    }
                    else if (this.code == 1)
                    {
                        BizUnit = "GroupName";
                    }
                    else if (this.code == 2)
                    {
                        BizUnit = "GBUName";
                    }
                    ErrorMessage.Append("Please Select atleast one " + BizUnit + "\\n");
                }
                if (regionCode == null)
                {
                    ErrorMessage.Append("Please Select atleast one region");

                } 
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "ClientScript", "alert('" + ErrorMessage.ToString() + "' );", true);
                uwgSearchRes.Visible = false;
            }
        }
        else
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "ClientScript", "alert('Please enter Container Name' );", true);
            uwgSearchRes.Visible = false;
        }
    }
      
}
   
