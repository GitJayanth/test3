#region Uses
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
using HyperCatalog.Shared;
using HyperComponents.Data.dbAccess;
using HyperCatalog.Business;
using System.Data.SqlClient;
using System.Data.OleDb;
#endregion

public partial class UI_Globalize_TranslationReport : HCPage
{
  protected void Page_Load(object sender, EventArgs e)
  {
    if (!Page.IsPostBack)
    {
      dgResults.Visible = lbNotes.Visible = lbMessage.Visible = false;
      StartDate.Value = DateTime.UtcNow.AddMonths(-6);
      EndDate.Value = DateTime.UtcNow;
      EndDate.MaxDate = DateTime.UtcNow;
    }
    else
    {
      dgResults.Visible = lbNotes.Visible = lbMessage.Visible = false;
      if (DateTime.Parse(StartDate.Value.ToString()) < DateTime.UtcNow.AddMonths(-6))
      {
        StartDate.Value = DateTime.UtcNow.AddMonths(-6);
      }
      if (DateTime.Parse(EndDate.Value.ToString()) > DateTime.UtcNow)
      {
        EndDate.Value = DateTime.UtcNow;
      }

    }        
  }
  protected void btSearch_Click(object sender, EventArgs e)
  {
    lbMessage.Visible = false;
    #region Build TypeList
    string typeList = string.Empty;
    foreach (ListItem t in cbType.Items)
    {
      if (t.Selected)
      {
        typeList += t.Value.ToString() + ",";
      }
    }
    if ((StartDate.Text == string.Empty) || (EndDate.Text == string.Empty) || (typeList == string.Empty))
    {
      if (typeList == string.Empty)
      {
      lbMessage.Text = "You must select at least one type.";
      lbMessage.Visible = true;
      }
      if ((StartDate.Text == string.Empty) || (EndDate.Text == string.Empty))
      {
        lbMessage.Text = "You must select the start and the end date to search Translations.";
        lbMessage.Visible = true;
      }
    }
    else
    {
      typeList = typeList.Substring(0, typeList.Length - 1);
      lbMessage.Text = typeList;
      #endregion
      using (Database dbObj = Utils.GetMainDB())
      {
        using (DataSet ds = dbObj.RunSPReturnDataSet("dbo._Report_Translation",
          new SqlParameter("@DateStart", DateTime.Parse(StartDate.Text)),
          new SqlParameter("@DateEnd", DateTime.Parse(EndDate.Text)),
          new SqlParameter("@TypeList", typeList),
          new SqlParameter("@Company", SessionState.CompanyName)))
        {
          dbObj.CloseConnection();
          if (dbObj.LastError != string.Empty)
          {
            lbMessage.Text = "[ERROR] _Report_Translation -> " + dbObj.LastError;
            lbMessage.Visible = true;
          }
          else
          {
            #region Results
            if (ds.Tables[0].Rows.Count > 0)
            {
              try
              {
                DataColumn parentColumn = ds.Tables[0].Columns["ClassId"];
                DataColumn childColumn = ds.Tables[1].Columns["ClassId"];
                DataRelation relation = new System.Data.DataRelation("Report", parentColumn, childColumn);
                ds.Relations.Add(relation);
              }
              catch (Exception ex)
              {
                lbMessage.Text = ex.ToString();
                lbMessage.Visible = true;
              }
              dgResults.DataSource = ds.Tables[0].DefaultView;
              dgResults.DataBind();
              Utils.InitGridSort(ref dgResults, false);
              lbMessage.Visible = false;
              dgResults.Bands[1].Columns.FromKey("CreationDate").Format = SessionState.User.FormatDate;
              dgResults.Bands[1].Columns.FromKey("CompletionDate").Format = SessionState.User.FormatDate;
              dgResults.Visible = true;
              lbNotes.Visible = true;

            }
            #endregion
            #region No result
            else
            {
              lbMessage.Text = "No result found";
              lbMessage.Visible = true;
            }
            #endregion
          }
        }
      }
    }
  }

  protected void dgResults_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
  {
    e.Row.Cells.FromKey("ClassName").Text = "[" + e.Row.Cells.FromKey("ClassId").Text + "] " + e.Row.Cells.FromKey("ClassName").Text;
  }
  protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
  {
    string btn = be.Button.Key.ToLower();
    #region Export
    if (btn == "export")
    {
      Utils.ExportToExcel(dgResults, "TranslationReport", "TranslationReport");
    }
    #endregion
  }
}

