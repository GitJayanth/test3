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

namespace HyperCatalog.UI.Acquire.QDE
{
	/// <summary>
	/// 
	/// </summary>
	public partial class QDE_CrossSell : HCPage
	{
		#region Declarations

		private Int64 itemId = -1;
		private bool isCrossSell = false;
		#endregion

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
			this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);

		}
		#endregion

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (HyperCatalog.Shared.SessionState.User.HasCapability(HyperCatalog.Business.CapabilitiesEnum.MANAGE_CROSS_SELL))
			{
				if (HyperCatalog.Shared.SessionState.User.IsReadOnly)
				{
					uwToolbar.Items.FromKeyButton("Apply").Enabled = false;
				}

				try
				{
					// Retrieve parameters
					if (Request["i"] != null)
						itemId = Convert.ToInt64(Request["i"]);
					if (Request["a"] != null)
						isCrossSell = Convert.ToBoolean(Request["a"]);

					if (!HyperCatalog.Shared.SessionState.User.HasItemInScope(itemId))
					{
						UITools.DenyAccess(DenyMode.Popup);
						return;
					}

					if (!Page.IsPostBack)
					{
						UpdateDataView();
					}
				}
				catch (Exception fe)
				{
					string error = fe.ToString();
					UITools.DenyAccess(DenyMode.Popup);
				}
			}
			else
			{
				UITools.DenyAccess(DenyMode.Popup);
			}
		}

		private void UpdateDataView()
		{
			lbError.Visible = false;

			lbTitle.Text = "Are you sure to remove all Cross Sell?";
			cbIncludeManual.Text = "Include the cross sell created manually";
		}

		private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
		{
			string btn = be.Button.Key.ToLower();
			if (btn == "apply")
			{
				Apply();
			}
		}

		private void Apply()
		{
			bool includeManual = cbIncludeManual.Checked;
			if (!HyperCatalog.Business.Link.AddDelCrossSell(itemId, isCrossSell, includeManual))
			{
				lbError.CssClass = "hc_error";
				lbError.Text = HyperCatalog.Business.Link.LastError;
				lbError.Visible = false;
			}
			else
			{
				// Save flag IsCrossSell
				HyperCatalog.Business.Item item = HyperCatalog.Business.Item.GetByKey(itemId);
				item.IsCrossSell = isCrossSell;
				if (item.Save(HyperCatalog.Shared.SessionState.User.Id, true))
				{
					lbError.CssClass = "hc_success";
					lbError.Text = "All Cross Sell are removed!";
					lbError.Visible = true;
          HyperCatalog.Shared.SessionState.CurrentItem = item;
					Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"UpdateAndClose", "<script>UpdateAndClose();</script>");
				}
				else
				{
					lbError.CssClass = "hc_error";
					lbError.Text = HyperCatalog.Business.Item.LastError;
					lbError.Visible = false;
				}
			}
		}
	}
}
