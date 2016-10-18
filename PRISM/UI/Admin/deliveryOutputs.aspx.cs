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
using System.IO;
using HyperCatalog.Business.DAM;
using HyperComponents.Data.dbAccess;
using HyperCatalog.Shared;

namespace HyperCatalog.UI.Delivery
{
	/// <summary>
  /// Description résumée de DeliveryOutputs.
	/// </summary>
	public partial class DeliveryOutputs : HCPage
	{
    private Business.DAM.Library _CurrentLibrary;
    protected Business.DAM.Library CurrentLibrary
    {
      get
      {
        if (_CurrentLibrary == null)
          _CurrentLibrary = HyperCatalog.Business.DAM.Library.GetByKey(SessionState.CacheParams["Delivery_DAMLibrary"].Value.ToString());
        return _CurrentLibrary;
      }
    }
    private ResourceType[] _resourceTypes;
    protected ResourceType[] resourceTypes
    {
      get
      {
        if (_resourceTypes == null)
          _resourceTypes = CurrentLibrary != null ? ResourceType.GetAllForLibrary(_CurrentLibrary.Id) : ResourceType.GetAll();
        return _resourceTypes;
      }
    }
    protected string damHandlerURL;

    protected override void OnLoad(EventArgs e)
    {
      damHandlerURL = HyperCatalog.Shared.SessionState.CacheComponents["DAM_Provider"].URI;
      if (!IsPostBack)
        BindGrid();

      base.OnLoad(e);
    }

    private void BindGrid()
    {
      Business.DAM.Library lib = CurrentLibrary;
      if (lib != null)
      {
        using (Database dbObj = new Database(SessionState.CacheComponents["DAM_DB"].ConnectionString))
        {
          using (DataSet ds = dbObj.RunSPReturnDataSet("DL_GetDAMPackages", new System.Data.SqlClient.SqlParameter("@LibraryId", CurrentLibrary.Id)))
            if (ds != null && ds.Tables.Count == 2)
            {
              ds.Relations.Add("files", ds.Tables[0].Columns["ResourceId"], ds.Tables[1].Columns["ResourceId"]);
              documents.DataSource = ds;
              if (documents.CurrentPageIndex * documents.PageSize > ds.Tables[0].Rows.Count)
                documents.CurrentPageIndex = 0;
              documents.DataBind();
            }
        }
      }
    }
    protected void documents_ItemCreated(object sender, DataGridItemEventArgs e)
    {
      if (e.Item.DataSetIndex >= 0 && e.Item.DataItem!=null)
      {
        DataRow[] rows = ((System.Data.DataRowView)e.Item.DataItem).Row.GetChildRows("files");
        e.Item.Cells[1].FindControl("pnlFiles").Visible = rows.Length > 0;
        if (rows.Length > 0)
        {
          ((Repeater)e.Item.Cells[1].FindControl("olderFiles")).DataSource = rows;
          ((HyperLink)e.Item.Cells[1].FindControl("lnkEdit")).NavigateUrl = damHandlerURL + CurrentLibrary.Name + "/" + DataBinder.Eval(e.Item.DataItem, "ResourceName") + rows[0]["FileExtension"];
          e.Item.Cells[2].Text = rows[0]["FileCreatedOn"].ToString();
          e.Item.Cells[3].Text = convertBytes((long)Convert.ToDouble(rows[0]["FileSize"]));
          e.Item.Cells[0].Text = "<a href=\"" + damHandlerURL + CurrentLibrary.Name + "/" + DataBinder.Eval(e.Item.DataItem, "ResourceName") + rows[0]["FileExtension"] + "\" target=\"_blank\"><img src=\"" + damHandlerURL + "/" + CurrentLibrary.Name + "/" + DataBinder.Eval(e.Item.DataItem, "ResourceName") + ".zip?thumbnail=1&size=40\" width=\"40\" height=\"40\" border=\"0\"/></a>";
        }
      }
    }
    protected static string convertBytes(long bytes)
    {
      //Converts bytes into a readable 
      if (bytes >= 1073741824)
        return String.Format(Convert.ToString(bytes / 1024 / 1024 / 1024), "#0.00") + " GB";
      else if (bytes >= 1048576)
        return String.Format(Convert.ToString(bytes / 1024 / 1024), "#0.00") + " MB";
      else if (bytes >= 1024)
        return String.Format(Convert.ToString(bytes / 1024), "#0.00") + " KB";
      else if (bytes > 0 && bytes < 1024)
        return bytes + " Bytes";
      else
        return "0 Byte";
    }
    protected void pnlOlderFiles_OnInit(object sender, EventArgs e)
    {
      ((WebControl)sender).Attributes["style"] = "display:none;";
    }
    protected void mtb_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      switch (be.Button.Key)
      {
        case "List":
          Response.Redirect("./Delivery.aspx");
          break;
      }
    }

    private enum MessageLevel : int
    {
      Information = 0,
      Warning = 1,
      Error = 2
    }
    private static string[] cssClassLevels = { "lbl_msgInformation", "lbl_msgWarning", "lbl_msgError" };
    private static void SetMessage(Label label, string text, MessageLevel level)
    {
      label.Page.ClientScript.RegisterStartupScript(label.Page.GetType(), "start", "document.getElementById('addPanel').style.display='';", true);
      label.Text = text;
      label.CssClass = cssClassLevels[(int)level];
      label.Visible = true;
    }
  }
}
