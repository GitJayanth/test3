#region uses
using System;
using System.Xml;
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
using HyperCatalog.UI.Acquire.QDE;
#endregion

namespace HyperCatalog.UI.Acquire.Chunk
{
  /// <summary>
  /// Description résumée de Chunk_Container.
  /// </summary>
  public partial class Chunk_PreviewPhoto : HCPage
  {
    protected ContainerInfo ContainerInfoPanel;

    protected HyperCatalog.Business.Container container;
		
    protected void Page_Load(object sender, System.EventArgs e)
    {      
      panelInfo.Visible = false;
      if (SessionState.QDEChunk != null)
      {
        XmlDocument xmlInfo = new XmlDocument();
        string imgPath = HCPage.WSDam.ResourceGetByPath(SessionState.QDEChunk.Text);
        if (imgPath!=string.Empty)
        {
          try
          {
            xmlInfo.LoadXml(imgPath);
            System.Xml.XmlNode node = xmlInfo.DocumentElement;
            System.Xml.XmlNode fileNode = node.FirstChild;
            string fullPath =  node.Attributes["uri"].InnerText;
            imgPreview.ImageUrl = fullPath + "?thumbnail=1&culture=" + SessionState.Culture.Code;
            imgPreview.ToolTip = SessionState.QDEChunk.Text;
            lbName.Text = node.Attributes["name"].InnerText;
            lbLibrary.Text = node.Attributes["library"].InnerText;
            lbHeight.Text = fileNode.Attributes["pxHeight"].InnerText;
            lbWidth.Text = fileNode.Attributes["pxWidth"].InnerText;
            int binarySize = Convert.ToInt32(fileNode.Attributes["binarySize"].InnerText);
            lbSize.Text = Convert.ToDecimal(binarySize / 1024).ToString();
            lbResolutionV.Text = fileNode.Attributes["vRes"].InnerText;
            lbResolutionH.Text = fileNode.Attributes["hRes"].InnerText;
            lbCreatedOn.Text = node.Attributes["createdOn"].InnerText;
            panelInfo.Visible = true;
          }
          catch (Exception ex)
          {
            imgPreview.ImageUrl = "/hc_v4/img/ed_notfound.gif";
            imgPreview.ToolTip = "An exception occurred: " + ex.Message;
            Trace.Warn("DAM", "Exception processing DAM: " + ex.Message );
          }
        }
        else
        {
          imgPreview.ImageUrl = "/hc_v4/img/ed_notfound.gif";
          imgPreview.ToolTip = "Not found";
        }

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

    }
    #endregion
  }
}