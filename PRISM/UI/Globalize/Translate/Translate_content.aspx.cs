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

using HyperCatalog.Shared;
using HyperCatalog.Business;
using HyperComponents.Data.dbAccess;
using System.Data.SqlClient;

public partial class Translate_content : HCPage
{
	#region Protected vars
	#endregion

	#region Privates vars
	private string itemId=string.Empty;
	#endregion

	protected void Page_Load(object sender, System.EventArgs e)
	{
    if (SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_TRANSLATION_SETTING))
    {			
      if (Request["i"] != null)
				itemId = Request["i"].ToString();
			else
				itemId = "-1";

			if (!itemId.Equals("-1"))
			{
        using (Item item = Item.GetByKey(Convert.ToInt64(itemId)))
        {
          if (item.Sku != string.Empty)
            lblTitleItem.Text = "[" + item.Sku + " ] - " + item.Name;
          else
            lblTitleItem.Text = item.Name;
        }
			}
			UpdateDataView();
		}
		else
			UITools.DenyAccess(DenyMode.Frame); 
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
			this.dg.InitializeRow += new Infragistics.WebUI.UltraWebGrid.InitializeRowEventHandler(this.dg_InitializeRow);

		}
		#endregion

	private void UpdateDataView()
	{
		if ((itemId!=string.Empty) && (!itemId.Equals("-1")))
		{
			// get missing chunks in database
			SqlParameter[] parameters = {new SqlParameter("@UserId", SessionState.User.Id.ToString()),
										 new SqlParameter("@ItemId", itemId),
										 new SqlParameter("@CultureCode", SessionState.Culture.Code),
										 new SqlParameter("@Type", 1)};
      using (Database dbObj = Utils.GetMainDB())
      {
        using (DataSet ds = dbObj.RunSPReturnDataSet("dbo.PM_GetMissingChunks", "Chunks", parameters))
        {
          if (dbObj.LastError == string.Empty)
          {
            if (ds != null)
            {
              dg.Visible = ds.Tables["Chunks"].Rows.Count > 0;
              if (ds.Tables["Chunks"].Rows.Count > 0)
              {
                dg.DataSource = ds.Tables["Chunks"];
                Utils.InitGridSort(ref dg);
                dg.DataBind();
              }
              ds.Dispose();
            }
          }
          else
          {
            Response.Write(dbObj.LastError);
            Response.End();
          }
        }
      }
		}
		else
			dg.Visible=false;
	}

	private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
	{
		// Status
		string altStatus=string.Empty;
		string status=e.Row.Cells.FromKey("ChunkStatus").Text;
		switch (status)
		{
			case "M": altStatus="Missing";break;
			//case "R": altStatus="Rejected";break; --Commented for alternate for CR 5096
			case "D": altStatus="Draft";break;
			case "F": altStatus="Final";break;
			default: altStatus=string.Empty;break;
		}
		e.Row.Cells.FromKey("ChunkStatus").Text = "<img src='/hc_v4/img/S"+status+".gif' align='center' valign='middle' alt='"+altStatus+"'/>";

		// Value
		if (e.Row.Cells.FromKey("Value").Text == Chunk.BlankValue)
			e.Row.Cells.FromKey("Value").Text = Chunk.BlankText;

		//Display Edit Link in Container Name
		string onclick = "ed('"+dg.ClientID+"', "+e.Row.Index.ToString()+", '"+SessionState.Culture.Code+"', '"+itemId+"')";
		e.Row.Cells.FromKey("ContainerName").Text = "<a href='javascript://' onclick=\""+onclick+"\">"+e.Row.Cells.FromKey("ContainerName").Text+" ["+e.Row.Cells.FromKey("Tag").Text+"]</a>";
	}
}
