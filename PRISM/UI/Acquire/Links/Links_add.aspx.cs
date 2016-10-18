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
using HyperCatalog.Business;
using HyperCatalog.Shared;

namespace HyperCatalog.UI.Acquire.Links
{
	/// <summary>
	/// Tabs to add new links (treeview or editor)
	/// </summary>
	public partial class Links_addMethod : HCPage
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
      if (SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_LINKS))
			{
				System.Int64 itemId = -1; // selected item
				string linkFrom = string.Empty;
				int linkTypeId = -1; // id of the link type

				try
				{
					// get properties
					if (Request["f"] != null)
						linkFrom = Request["f"].ToString();
					if (Request["t"] != null)
						linkTypeId = Convert.ToInt32(Request["t"]);
					if (Request["i"] != null)
						itemId = Convert.ToInt64(Request["i"]);

          using (LinkType linkType = LinkType.GetByKey(linkTypeId))
          {

            // update title
            Infragistics.WebUI.UltraWebToolbar.TBLabel lblTitle = uwToolBarTitle.Items.FromKeyLabel("Title");
            lblTitle.Text = "New link";
            if (linkType != null)
              lblTitle.Text = lblTitle.Text + " [" + linkType.Name + "]";
          }
				}
				catch (FormatException fe)
				{
					UITools.DenyAccess(DenyMode.Popup);
				}

				// initialize tabs
        webTab.Tabs.FromKeyTab("Treeview").ContentPane.TargetUrl = webTab.Tabs.FromKeyTab("Treeview").ContentPane.TargetUrl + "?i=" + itemId + "&t=" + linkTypeId + "&f=" + linkFrom;
				webTab.Tabs.FromKeyTab("Editor").ContentPane.TargetUrl = webTab.Tabs.FromKeyTab("Editor").ContentPane.TargetUrl+"?i="+itemId+"&t="+linkTypeId+"&f="+linkFrom;
			}
			else
			{
				UITools.DenyAccess(DenyMode.Popup);
			}
		}
	}
}
