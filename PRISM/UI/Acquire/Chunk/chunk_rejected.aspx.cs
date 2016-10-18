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
using HyperCatalog.Shared;
using HyperCatalog.Business;
#endregion

namespace HyperCatalog.UI.Acquire.Chunk
{
  /// <summary>
  /// Description résumée de Chunk_Date.
  /// </summary>
  public partial class Chunk_Rejected : HCPage
  {
		#region Declarations

    private long itemId = -1;
    private int containerId = -1;
		private bool isMandatory = false;
    protected HyperCatalog.Business.Chunk chunk;
    protected HyperCatalog.Business.Container container;
		protected HyperCatalog.Business.Item item;
    protected HyperCatalog.Business.Culture culture;

		#endregion

    protected void Page_Load(object sender, System.EventArgs e)
    {
      try
      {
				if (Request["m"] != null)
					isMandatory = Convert.ToBoolean(Request["m"]);
				containerId = Convert.ToInt32(Request["d"]);
        culture = QDEUtils.UpdateCultureCodeFromRequest();
        item = QDEUtils.GetItemIdFromRequest();
        itemId = item.Id;
        chunk = ChunkWindow.GetChunk(itemId, containerId, culture.Code);
        container = SessionState.QDEContainer;
        if (!Page.IsPostBack)
        {
          lbResult.Text = HyperCatalog.Business.Chunk.LastError;
          UpdateDataView();
        }
      }
      catch (Exception ex)
      {
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "close", "<script>alert('" +UITools.CleanJSString(ex.ToString()) + "');top.window.close();</script>");
      }
    }

    private void UpdateDataView()
    {
      ChunkModifier1.Chunk = chunk;
      lbChunkValue.Text = chunk.Text==HyperCatalog.Business.Chunk.BlankValue ? HyperCatalog.Business.Chunk.BlankText : UITools.HtmlEncode(chunk.Text);
    }

    protected void wBtAccept_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
      if (chunk.Delete(SessionState.User.Id))
      {
        try
        {
            
          chunk = HyperCatalog.Business.Chunk.GetByKey(itemId, containerId, culture.FallbackCode);
            // Updating the session- 3.5 Release Start
          SessionState.QDEChunk = chunk;
          // Updating the session- 3.5 Release End
          if (chunk.Status == ChunkStatus.Final)
          {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "update", "<script>if (top.opener){if (top.opener.document.getElementById('action')){top.opener.document.getElementById('action').value = 'reload';top.opener.document.forms[0].submit();}}top.window.close();</script>");
          }
          else
          {
            chunk = null;
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "update", "<script>UpdateGrid('" + HyperCatalog.Business.Chunk.GetStatusFromEnum(ChunkStatus.Missing) + "', '');</script>");
          }
        }
        catch (Exception ex)
        {
          lbResult.Text = "<br/>Error: " + ex.ToString();
          lbResult.CssClass = "hc_error";
          //chunk = null;
          //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "update", "<script>UpdateGrid('" + HyperCatalog.Business.Chunk.GetStatusFromEnum(ChunkStatus.Missing) + "', '');</script>");
        }       
      }
      else
      {
        lbResult.Text = "<br/>Error: " + HyperCatalog.Business.Chunk.LastError;
        lbResult.CssClass = "hc_error";
      }
    }
}
}

