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
using HyperCatalog.UI.Acquire.Chunk;
using HyperCatalog.Business;  
using HyperCatalog.DataAccessLayer;
#endregion

namespace HyperCatalog.UI.Globalize
{
  /// <summary>
  /// Description résumée de Preview.
  /// </summary>
  public partial class Preview : System.Web.UI.Page
  {

    protected void Page_Load(object sender, System.EventArgs e)
    {
      // Placer ici le code utilisateur pour initialiser la page
      try
      {
        PanelChunk.Visible = ContainerInfoPanel.Visible = false;
        string key = System.Web.HttpUtility.HtmlEncode(Request["key"].ToString()).Replace(" ", "+");
        key = SqlTools.DecryptString(key);
        //Response.Write(key);
        string[] parameters = key.Split(',');
        switch (parameters[0])
        {
          case "d":
            ContainerInfoPanel.ContainerId = Convert.ToInt32(parameters[1]);
            ContainerInfoPanel.Visible = true;
            PanelChunk.Visible = PanelChunkDetail.Visible = false;
            break;
          case "i":
            Page.Response.Redirect("TR_Properties.aspx?tr=" + parameters[1]);
            break;
          case "c":
            Item item = Item.GetByKey(Convert.ToInt64(parameters[1]));
            Chunk chunk = Chunk.GetByKey(Convert.ToInt64(parameters[1]), Convert.ToInt32(parameters[2]), HyperCatalog.Shared.SessionState.MasterCulture.Code);
            HyperCatalog.Business.Culture culture = HyperCatalog.Business.Culture.GetByKey(parameters[3].ToString());
            if (Request["tab"] == null)
            {
              PanelChunk.Visible = true;
              PanelChunkDetail.Visible = false;
              webTab.Tabs.GetTab(0).ContentPane.TargetUrl = "Preview.aspx?key=" + key + "&tab=1";
              webTab.Tabs.GetTab(1).ContentPane.TargetUrl = "../../Acquire/qde/QDE_FormContent.aspx?i=" + item.Id + "&f=IF_-1&c=" + culture.Code;
              if (item != null)
              {
                webTab.Tabs[0].Text = chunk.ContainerName;
                uwToolbarTitle.Items.FromKeyLabel("ItemName").Text = item.Name;
              }
              if (culture != null)
              {
                uwToolbarTitle.Items.FromKeyLabel("Culture").Image = "/hc_v4/img/flags/" + culture.CountryCode + ".gif";
                uwToolbarTitle.Items.FromKeyLabel("Culture").Text = culture.Name + "&nbsp;";
              }
            }
            else
            {
              PanelChunkDetail.Visible = true;
              if (chunk != null)
              {
                txtValue.Text = chunk.Text;
                if (chunk.Text == HyperCatalog.Business.Chunk.BlankValue)
                {
                  txtValue.Text = HyperCatalog.Business.Chunk.BlankText;
                }
                imgStatus.ImageUrl = "/hc_v4/img/S" + HyperCatalog.Business.Chunk.GetStatusFromEnum(chunk.Status) + ".gif";
                lbStatus.Text = "[" + chunk.Status.ToString() + "]";
                ChunkComment1.Chunk = ChunkModifier1.Chunk = chunk;
                PanelChunkDetail.Visible = true;
                ChunkComment1.Visible = chunk.Comment != string.Empty;
              }
              else
              {
                Business.Debug.Trace("ASP", key, Business.DebugSeverity.Medium);
                UITools.JsCloseWin("Chunk not found (possible reason is deletion)");
              }
            }
            break;
          default :
            UITools.JsCloseWin("Incorrect query!");
            break;
        }
      }
      catch (Exception ex)
      {
        Business.Debug.Trace("ASP",ex.Message,Business.DebugSeverity.High);
        UITools.JsCloseWin("Unauthorized access - " + ex.ToString());
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
