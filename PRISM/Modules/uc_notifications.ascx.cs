namespace hc_homePageModules
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	/// This class represents Notification control
	/// </summary>
	public partial class uc_notifications : System.Web.UI.UserControl
	{
		#region Private vars
		private DateTime? _NotifDate = null;
		#endregion

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!HyperCatalog.Shared.SessionState.NotificationDate.HasValue)
			{
				// update notification date
				_NotifDate=DateTime.UtcNow;

				// set notification date in session
				HyperCatalog.Shared.SessionState.NotificationDate = _NotifDate;
			}
			else
			{
				// update notification date
				_NotifDate = HyperCatalog.Shared.SessionState.NotificationDate;
			}

			if (!Page.IsPostBack)
			{
				// Obtain data
				BindData();
			}
		}
		private void BindData()
		{
			// update tool bar
			UpdateToolBar();

			ModuleDB.NotificationsDB notificationsDB=new ModuleDB.NotificationsDB();
      using (DataSet ds = notificationsDB.GetNotifications(_NotifDate.Value))
      {
        if (ds != null)
        {
          uwgNotifications.DataSource = ds;
          uwgNotifications.DataBind();

          ds.Dispose();
        }
      }
		}
		private void UpdateToolBar()
		{
			// get objects in tool bar
			Infragistics.WebUI.UltraWebToolbar.TBLabel tbLblNotificationDate=uwtNotifications.Items.FromKeyLabel("lblNotificationDate");
			Infragistics.WebUI.UltraWebToolbar.TBarButton tbBtnNext=uwtNotifications.Items.FromKeyButton("Next");
			Infragistics.WebUI.UltraWebToolbar.TBarButton tbBtnPrevious=uwtNotifications.Items.FromKeyButton("Previous");

			// update date label
      tbLblNotificationDate.Text = _NotifDate.Value.ToString(HyperCatalog.Shared.SessionState.User.FormatDate);

			// update buttons
			TimeSpan tsDateMin=_NotifDate.Value.Subtract(DateTime.MinValue);
			tbBtnPrevious.Enabled=tsDateMin.Days>0;
      //tbBtnPrevious.Enabled=!(_NotifDate.ToString(SessionState.User.FormatDate).Equals(DateTime.MinValue.ToString(SessionState.User.FormatDate)));
			TimeSpan tsDateNow=_NotifDate.Value.Subtract(DateTime.Now);
			TimeSpan tsDateMax=_NotifDate.Value.Subtract(DateTime.MaxValue);
			tbBtnNext.Enabled=((tsDateMax.Days<0) && (tsDateNow.Days<0));
      //tbBtnNext.Enabled=!(_NotifDate.ToString(SessionState.User.FormatDate).Equals(DateTime.MaxValue.ToString(SessionState.User.FormatDate))) && !(_NotifDate.ToString(SessionState.User.FormatDate).Equals(DateTime.Now.ToString(SessionState.User.FormatDate)));
		}
		protected void uwtNotifications_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
		{
			if (be.Button.Key.Equals("Next"))
			{
				// button 'Next'
				TimeSpan tsDateNow=_NotifDate.Value.Subtract(DateTime.Now);
				TimeSpan tsDateMax=_NotifDate.Value.Subtract(DateTime.MaxValue);
				if ((tsDateMax.Days<0) && (tsDateNow.Days<0)) 
					_NotifDate=_NotifDate.Value.AddDays(1);
				HyperCatalog.Shared.SessionState.NotificationDate = _NotifDate;
				BindData();
			}
			else if (be.Button.Key.Equals("Previous"))
			{
				// button 'Previous'
				TimeSpan tsDateMin=_NotifDate.Value.Subtract(DateTime.MinValue);
				if (tsDateMin.Days>0)
					_NotifDate=_NotifDate.Value.AddDays(-1);
				HyperCatalog.Shared.SessionState.NotificationDate = _NotifDate;
				BindData();
			}
		}
	}
}
