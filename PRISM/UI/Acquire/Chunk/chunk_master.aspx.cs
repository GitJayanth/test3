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
  /// Description résumée de chunk_master.
  /// </summary>
  public partial class chunk_master : HCPage
  {
    private string containerName;
    private int containerId;
    private long itemId;
    protected HyperCatalog.Business.Chunk chunk;
    protected HyperCatalog.Business.Container container;
    protected HyperCatalog.Business.Item item;
    protected HyperCatalog.Business.Culture culture;

    
    protected void Page_Load(object sender, System.EventArgs e)
    {
      // Placer ici le code utilisateur pour initialiser la page
      
      if (Request["d"]!=null)
      {
        containerId = Convert.ToInt32(Request["d"]);
        culture = QDEUtils.UpdateCultureCodeFromRequest();
        item = QDEUtils.GetItemIdFromRequest();
        container = SessionState.QDEContainer;
        itemId = item.Id;
        if (!Page.IsPostBack)
        {
          ShowChunk();
        }
      }
    }

    private void ShowChunk()
    {
      // *************************************************************************
      // Retrieve information about the current container
      // *************************************************************************
      string Value=string.Empty , comment=string.Empty;
      string cultureCode = culture.FallbackCode;
      while (cultureCode != null && cultureCode != string.Empty)
      {
        chunk = HyperCatalog.Business.Chunk.GetByKey(itemId, containerId, cultureCode);
        if (chunk != null)
          cultureCode = string.Empty;
        else
          cultureCode = HyperCatalog.Business.Culture.GetByKey(cultureCode).FallbackCode;
      }

      if (chunk != null)
      {
        txtValue.Text = chunk.Text;
        if (chunk.Text==HyperCatalog.Business.Chunk.BlankValue)
        {
          txtValue.Text = HyperCatalog.Business.Chunk.BlankText;
        }
        imgStatus.ImageUrl = "/hc_v4/img/S" + HyperCatalog.Business.Chunk.GetStatusFromEnum(chunk.Status) +".gif";
        lbStatus.Text = "[" + chunk.Status.ToString() +"]";
        ChunkComment1.Chunk = ChunkModifier1.Chunk = chunk;
        ChunkModifier1.Visible = true;

        if (chunk.Text != HyperCatalog.Business.Chunk.BlankValue && (chunk.Container.DataTypeCode == 'P' || chunk.Container.ContainerTypeCode == 'P'))
        {
          System.Xml.XmlDocument xmlInfo = new System.Xml.XmlDocument();
          string imgPath = HCPage.WSDam.ResourceGetByPath(chunk.Text);
          if (imgPath != string.Empty)
          {
            try
            {
              xmlInfo.LoadXml(imgPath);
              System.Xml.XmlNode node = xmlInfo.DocumentElement;
              System.Xml.XmlNode fileNode = node.FirstChild;
              string fullPath = node.Attributes["uri"].InnerText;
              imgPreview.ImageUrl = fullPath + "?thumbnail=1&size=40&culture=" + cultureCode;
              imgPreview.ToolTip = chunk.Text;
            }
            catch (Exception ex)
            {
              imgPreview.ImageUrl = "/hc_v4/img/ed_notfound.gif";
              imgPreview.ToolTip = "An exception occurred: " + ex.Message;
              Trace.Warn("DAM", "Exception processing DAM: " + ex.Message);
            }
          }
        }
        else
          imgPreview.Visible = false;
      }
      else
      {
        //Response.Write("<script>top.window.close();</script>");
        Response.Write("culture.Code=" + culture.Code);
        Response.Write("containerId=" + containerId.ToString());
        Response.Write("ItemId=" + itemId.ToString());
        Response.End();
      }
    }
  }
}