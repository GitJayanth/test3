#region uses
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HyperCatalog.Business;
using HyperCatalog.Shared;
using HyperComponents.Data.dbAccess;
#endregion

namespace HyperCatalog.UI.Acquire.QDE
{
	/// <summary>
	/// Description résumée de QDE_DelItem.
	/// </summary>
	public partial class QDE_DelItem : HCPage
	{
		#region Declarations
    private Item item;
    #endregion

		protected void Page_Load(object sender, System.EventArgs e)
		{
      try
      {
				System.Int64 itemId = -1;

				if (Request["i"] != null)
				{
					itemId = Convert.ToInt64(Request["i"]);
                    if (SessionState.User.HasItemInScope(itemId) && SessionState.User.HasCapability(CapabilitiesEnum.DELETE_ITEMS))
                    {
                        if (HyperCatalog.Business.ApplicationParameter.IOHeirachyStatus() == 0)
                        {
                            item = HyperCatalog.Business.Item.GetByKey(itemId);
                            lbError.Visible = false;

                            if (!Page.IsPostBack)
                            {
                                uwToolbar.Items.FromKeyLabel("ItemName").Text = item.FullName + " - Delete";
                                // Update list of consequences to delete item
                                UpdateDataView();
                            }
                        }
                        else
                        {
                            UITools.JsCloseWin("The Product Hierarchy Refresh job is in progress.  User cannot perform this action.  Please try once the job completes!");

                        }
                    }
                    else
                        UITools.DenyAccess(DenyMode.Popup);
				}
				else
					UITools.JsCloseWin();
      }
      catch
      {
        UITools.JsCloseWin();
      }
    }

		#region Code généré par le Concepteur Web Form
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN : Cet appel est requis par le Concepteur Web Form ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
		/// le contenu de cette méthode avec l'éditeur de code.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Ultrawebtoolbar1.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.Ultrawebtoolbar1_ButtonClicked);

		}
		#endregion

		private void UpdateDataView()
		{
      using (Database dbObj = Utils.GetMainDB())
      {
        using (DataSet ds = dbObj.RunSPReturnDataSet("dbo._Item_DelSteps", string.Empty, new SqlParameter("@ItemId", item.Id)))
        {
          dbObj.CloseConnection();
          if (dbObj.LastError.Length > 0)
          {
            lbError.CssClass = "hc_error";
            lbError.Text = dbObj.LastError;
            lbError.Visible = true;

            rConsequences.Visible = false;
          }
          else
          {
            if (ds != null)
            {
              rConsequences.DataSource = ds;
              rConsequences.DataBind();

              ds.Dispose();
            }
          }
        }
      }
		}

		private void Ultrawebtoolbar1_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
		{
      string btn = be.Button.Key.ToLower();
      if (btn == "apply")
      {
        System.Int64 parentId = item.ParentId;

        if (item.Delete(SessionState.User.Id))
        {
          SessionState.CurrentItem = HyperCatalog.Business.Item.GetByKey(parentId);
          SessionState.User.LastVisitedItem = parentId;
          if (SessionState.User.LastVisitedItemReadOnly == item.Id)
            SessionState.User.LastVisitedItemReadOnly = parentId;

          Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Exit", "<script>ReloadAndClose();</script>");
        }
        else
        {
          lbError.CssClass = "hc_error";
          lbError.Text = "Item cannot be Deleted";
          lbError.Visible = true;
        }
      }
		}
	}
}
