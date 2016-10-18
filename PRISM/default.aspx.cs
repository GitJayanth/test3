#region uses
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
using HyperCatalog.Shared;
#endregion

namespace HyperCatalog.UI.Login
{
	/// <summary>
	/// Default page (Home page)
	/// </summary>
	public partial class _default : HCPage
	{
		#region Event methods
       
    protected void Page_Load(object sender, System.EventArgs e)
    {

        LogNet.log.Info("Default Page_debug: Page_Load : Start Time=" + DateTime.Now);
      if (!Page.IsPostBack){
        BuildPortal();
        LogNet.log.Info("Default Page_debug: Page_Load : End Time=" + DateTime.Now);
      }
    }		
		#endregion

		#region Private methods
		/// <summary>
		/// Build personalized portal (with specified modules of current user)
		/// </summary>
		private void BuildPortal()
		{
            LogNet.log.Debug("Default Page_debug: BuildPortal() : Start Time=" + DateTime.Now);
			string[] paneNames={"LeftPane", "ContentPane", "RightPane"};

			// load modules
      using (DataSet ds = Configuration.LoadUserModules())
      {
        if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
        {
          // Philippe added this for debug in VS 2005
          DataRow[] rows = GetModuleByPane(ds, 0);
          foreach (DataRow currentRow in rows)
          {
            leftPane.Controls.Add(LoadControl("Modules/" + currentRow["Control"].ToString()));
          }
          leftPane.Visible = (rows.Length > 0);

          rows = GetModuleByPane(ds, 1);
          foreach (DataRow currentRow in rows)
          {
            contentPane.Controls.Add(LoadControl("Modules/" + currentRow["Control"].ToString()));
          }
          contentPane.Visible = (rows.Length > 0);

          rows = GetModuleByPane(ds, 2);
          foreach (DataRow currentRow in rows)
          {
            rightPane.Controls.Add(LoadControl("Modules/" + currentRow["Control"].ToString()));
          }
          rightPane.Visible = (rows.Length > 0);
        }
      }
      LogNet.log.Debug("Default Page_debug: BuildPortal() : End Time=" + DateTime.Now);
    }

		/// <summary>
		/// Retrieve all module for a given pane name
		/// </summary>
		/// <param name="ds">DataSet object containing all modules of current user</param>
		/// <param name="paneId">Pane identifier</param>
		/// <returns>Returns DataRow list</returns>
		private DataRow[] GetModuleByPane(DataSet ds, int paneId)
		{
			// Obtain modules for one pane
			DataRow[] rows = ds.Tables[0].Select("PaneId="+paneId, "Sort");
			return rows;
		}

		/// <summary>
		/// Retrieve control from pane identifier
		/// </summary>
		/// <param name="iPane">Pane identifier</param>
		/// <returns>Returns control</returns>
//		private System.Web.UI.HtmlControls.HtmlTableCell GetPane(string paneName)
		private System.Web.UI.WebControls.Panel GetPane(string paneName)
		{
			// Obtain pane
			//return (System.Web.UI.HtmlControls.HtmlTableCell)Page.FindControl(paneName);
      return (System.Web.UI.WebControls.Panel)Page.FindControl(paneName);
    }
		#endregion
	}
}
