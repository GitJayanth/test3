using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Infragistics.WebUI.UltraWebGrid;
using System.Web.UI.HtmlControls;
using HyperCatalog.Business;
using HyperCatalog.Shared;
using System.Data.SqlClient;
using System.Web.SessionState;
using HyperComponents.Data.dbAccess;
using System.Web.UI.HtmlControls;
using Infragistics.WebUI.UltraWebGrid;
using Infragistics.WebUI.WebDataInput;
using HyperCatalog.WebServices.EventLoggerWS;


public partial class UI_Admin_jobstatus_jobscheduleedit : System.Web.UI.Page
{
    protected HyperCatalog.Business.CapabilitiesEnum updateCapability = HyperCatalog.Business.CapabilitiesEnum.EXTEND_CONTENT_MODEL;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["j"] != null)
            {UpdateDataView(Request["j"]);}
            else
            {UpdateDataView(string.Empty);}
        }
    }
    
    protected override void OnPreRender(EventArgs e)
    {
        #region Enabling/Disabling Save button based on the User Access

        uwToolbar.Items.FromKeyButton("Save").Enabled = !HyperCatalog.Shared.SessionState.User.IsReadOnly && HyperCatalog.Shared.SessionState.User.HasCapability(updateCapability);

        #endregion Enabling/Disabling Save button based on the User Access
    }
    
    protected void UpdateDataView(string jobName)
    {
        #region User Access Value

        bool res = !HyperCatalog.Shared.SessionState.User.IsReadOnly && HyperCatalog.Shared.SessionState.User.HasCapability(updateCapability);
        
        #endregion User Access Value

        #region Loading the Values in the form containers
        cmbJobType.Items.Add("Inbound");
        cmbJobType.Items.Add("Operational");
        cmbJobType.Items.Add("OutBound");
        cmbScheduledDay.Items.Add("Daily");
        cmbScheduledDay.Items.Add("Sunday");
        cmbScheduledDay.Items.Add("Monday");
        cmbScheduledDay.Items.Add("Tuesday");
        cmbScheduledDay.Items.Add("Wednesday");
        cmbScheduledDay.Items.Add("Thursday");
        cmbScheduledDay.Items.Add("Friday");
        cmbScheduledDay.Items.Add("Saturday");
        #endregion Loading the Values in the form containers

        if (jobName != string.Empty)
        {
            #region Displaying the Data

            Database dbCrystal = Utils.GetMainDB();
            string strQuery = "SELECT AppComponentId,JobName,JobType,ScheduledDay,ScheduledTime,EstimatedFinishTime,CutOffTime,IsActive FROM JobSchedule WITH (NOLOCK) WHERE JobName='" + jobName + "'";
            DataSet ds = dbCrystal.RunSQLReturnDataSet(strQuery, new SqlParameter[] { });
            if (ds != null)
            {
                DataTable dtJS = ds.Tables[0];
                if (dtJS != null)
                {
                    if (dtJS.Rows.Count > 0)
                    {
                        #region Load Data in the controls

                        txtAppComponentId.Text = dtJS.Rows[0]["AppComponentId"].ToString();
                        txtJobName.Text = dtJS.Rows[0]["JobName"].ToString();
                        cmbJobType.SelectedIndex = cmbJobType.Items.IndexOf(new ListItem(dtJS.Rows[0]["JobType"].ToString()));
                        cmbScheduledDay.SelectedIndex = cmbScheduledDay.Items.IndexOf(new ListItem(dtJS.Rows[0]["JobType"].ToString()));
                        txtScheduleTime.Text = dtJS.Rows[0]["ScheduledTime"].ToString();
                        txtEstimatedTime.Text = dtJS.Rows[0]["EstimatedFinishTime"].ToString();
                        txtCutOffTime.Text = dtJS.Rows[0]["CutOffTime"].ToString();
                        if (dtJS.Rows[0]["IsActive"].ToString().ToLower() == "true")
                        { chIsActive.Checked = true; }
                        else
                        { chIsActive.Checked = false; }

                        #region Enabling/Disabling Controls based on the User Access

                        cmbJobType.Enabled = res;
                        cmbScheduledDay.Enabled=res;
                        txtScheduleTime.Enabled = res;
                        txtEstimatedTime.Enabled = res;
                        txtCutOffTime.Enabled = res;
                        chIsActive.Enabled = res;

                        #endregion Enabling/Disabling Controls based on the User Access

                        txtAppComponentId.Enabled = false;
                        txtJobName.Enabled = false;

                        #endregion Load Data in the controls

                    }
                }
            }

            #endregion Displaying the Data
        }
      
    }
    protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
        string btn = be.Button.Key.ToLower();
        bool status = false;

        if (btn == "save")
        {
            try
            {
                if (ValidateTime(txtScheduleTime.Text.Trim()) && ValidateTime(txtCutOffTime.Text.Trim()))
                {
                    if (ValidateDuration(txtEstimatedTime.Text.Trim()))
                    { SaveData(); }
                    else
                    { 
                        DisplayError("Please enter the valid duration");
                        txtEstimatedTime.Focus();
                    }
                }
                else
                { DisplayError("Please enter the Time in Valid Format"); }
            }
            catch(Exception e)
            {DisplayError("Please enter the value in valid format");}
        }
    }

    protected void SaveData()
    {
        #region Saving the Data

        string strQuery = string.Empty;
        int retval = 1;
        int appComponentId = -1;
        Database dbCrystal = Utils.GetMainDB();
        try
        {
            #region Validating Component Id
            
            SqlDataReader DR;
            DR = dbCrystal.RunSQLReturnRS("Select Count(*) FROM Hypersettings..appComponents WITH (NOLOCK) WHERE AppComponentId=" + txtAppComponentId.Text.Trim());
            while (DR.Read())
            {appComponentId = Convert.ToInt32(DR[0].ToString()); }

            #endregion Validating Component Id

            if (appComponentId > 0)
            {
                retval = dbCrystal.RunSPReturnInteger("_AddUpd_JobSchedule", new SqlParameter[] { new SqlParameter("@AppComponentId", txtAppComponentId.Text.Trim()), 
                                           new SqlParameter("@JobName",txtJobName.Text.Trim()),new SqlParameter("@JobType",cmbJobType.SelectedValue.ToString().Trim()),
                                           new SqlParameter("@ScheduledDay",cmbScheduledDay.SelectedValue.ToString().Trim()),new SqlParameter("@ScheduledTime",txtScheduleTime.Text.Trim()),
                                           new SqlParameter("@EstimatedFinishTime",txtEstimatedTime.Text.Trim()),new SqlParameter("@CutOffTime",txtCutOffTime.Text.Trim()),
                                           new SqlParameter("@IsActive",chIsActive.Checked),new SqlParameter("@ModifierId",SessionState.User.Id ) });
                if (retval == 0)
                {
                    lblMsg.Text = "Data saved Successfully";
                    lblMsg.Visible = true;
                    lblMsg.Font.Bold = true;
                    lblMsg.ForeColor = System.Drawing.Color.Green;

                    if (Session["JobLst"] != null)
                    {
                        Session["JobLst"]=dbCrystal.RunSQLReturnDataSet("SELECT AppComponentId, JobName FROM JobSchedule WHERE IsActive = 1", new SqlParameter[0]);
                        dbCrystal.CloseConnection();
                        dbCrystal.Dispose();
                    }


                }
                else
                { DisplayError("Data Cannot be Saved. Please check db logs for more information"); }
            }
             else
            { DisplayError("Data Cannot be Saved. Invalid AppComponentId."); }
        }
        catch (Exception e)
        {
            #region Handle the exception

            DisplayError("Data Cannot be Saved. Please check db logs for more information");
            const int CRYSTAL_UI_COMPONENT_ID = 2;
            Guid errorGuid = Guid.NewGuid();
            HyperCatalog.EventLogger.EventLogger.AddEventLog(CRYSTAL_UI_COMPONENT_ID, errorGuid, 0, HyperCatalog.EventLogger.Severity.INFORMATION, "Job Schedule Edit Error", "", e.Message.ToString());
            dbCrystal.Dispose();

            #endregion Handle the exception
        }
        finally
        {dbCrystal.Dispose();}
        #endregion Displaying the Data
    }

    protected bool ValidateTime(string strTime)
    {
        bool retval = false;
        Array strSplit;
        strSplit=strTime.Split(':');
        try
        {
            if (strSplit.Length != 0 || strSplit.Length < 3)
            {
                if ((Convert.ToInt32(strSplit.GetValue(0))) <= 23 && (Convert.ToInt32(strSplit.GetValue(0))) >= 0 && (Convert.ToInt32(strSplit.GetValue(1))) <= 59 && (Convert.ToInt32(strSplit.GetValue(1))) >= 0)//&& (strSplit.GetValue(2).ToString().ToLower() == "am" || strSplit.GetValue(2).ToString().ToLower() == "pm"))
                { retval = true; }
                else
                { retval = false; }
            }
            else
            { retval = false; }
        }
        catch (Exception e)
        {retval = false;}

        return retval;
    }

    #region commented
    //protected bool ValidateDay(string strDay)
    //{
    //    bool retval = false;
    //    ArrayList strFormat = new ArrayList();
    //    strFormat.AddRange(new String[]{"daily","sun","mon","tue","wed","thu","fri","sat"});
    //    if (strFormat.Contains(strDay.ToLower()))
    //    { retval = true; }
    //    return retval;
    //}
    #endregion commented

    protected bool ValidateDuration(string strTime)
    {
        bool retval = false;
        try
        {
            if (Convert.ToInt64(strTime.Trim()) > 0)
            { retval = true; }
            else
            { retval = false; }

        }
        catch (Exception e)
        { retval = false; }

        return retval;
    }
    protected void DisplayError(string Message)
    {
        lblMsg.Text = Message;
        lblMsg.Visible = true;
        lblMsg.Font.Bold = true;
        lblMsg.ForeColor = System.Drawing.Color.Red;
    }

}
