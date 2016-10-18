#region Copyright (c)  Hewlett-Packard. All Rights Reserved
/* ---------------------------------------------------------------------*
*        THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE.       *
* --------------------------------------------------------------------- *
 * History Section
 * Date             Name            Description                                         Ref
 * June 15 2009     S.Balakumar     ACQ 10 - ILB O/M + ILB at region level              #ACQ10.0
 * June 15 2009     Mahibah         Removal of Rejection functionality                  #ROR
 * --------------------------------------------------------------------- *
*/
#endregion
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
  /// Description résumée de Chunk_Number.
  /// </summary>
  public partial class Chunk_Number : HCPage
  {
    #region Declarations
    protected ContainerInfo ContainerInfoPanel;

    private long itemId = -1;
    private int containerId = -1;
    private bool isMandatory = false;
    protected HyperCatalog.Business.Chunk chunk;
    protected HyperCatalog.Business.Container container;
    protected HyperCatalog.Business.Item item;
    protected HyperCatalog.Business.Culture culture;
    #endregion

    #region Code généré par le Concepteur Web Form
    override protected void OnInit(EventArgs e)
    {
      //
      // CODEGEN : Cet appel est requis par le Concepteur Web Form ASP.NET.
      //
      InitializeComponent();
      base.OnInit(e);
      ChunkButtonBar.DraftClick += new ChunkButtonBarClickEventHandler(Chunk_DraftClick);
      ChunkButtonBar.FinalClick += new ChunkButtonBarClickEventHandler(Chunk_FinalClick);
      ChunkButtonBar.DeleteClick += new ChunkButtonBarClickEventHandler(Chunk_DeleteClick);
      ChunkButtonBar.CopyClick += new ChunkButtonBarClickEventHandler(Chunk_CopyClick);
      //ChunkButtonBar.RejectClick += new ChunkButtonBarClickEventHandler(Chunk_RejectClick); #ROR -Commented
    }

    /// <summary>
    /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
    /// le contenu de cette méthode avec l'éditeur de code.
    /// </summary>
    private void InitializeComponent()
    {

    }
    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
    {
      try
      {
        containerId = Convert.ToInt32(Request["d"]);
        if (Request["m"] != null)
          isMandatory = Convert.ToBoolean(Request["m"]);
        culture = QDEUtils.UpdateCultureCodeFromRequest();
        item = QDEUtils.GetItemIdFromRequest();
        itemId = item.Id;
        uwToolbar.Enabled = Request["ui"] != null;
        chunk = ChunkWindow.GetChunk(itemId, containerId, culture.Code);
        container = SessionState.QDEContainer;
        /*#ACQ10.0 Starts
        // Commented to bring out ILB for all catalogue irespective of mandatory status
        if (!isMandatory || culture.Type == CultureType.Regionale)
        {
          UITools.HideToolBarButton(uwToolbar, "ilb");
        }
         * #ACQ10.0 Ends
        */
    }
      catch
      {
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "close", "<script>window.close()</script>");
      }
      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "blanktext", "<script>ILBText = '" + HyperCatalog.Business.Chunk.BlankText + "';</script>");


      //Modified this line for QCs# 839 and 1028
      ChunkButtonBar.Chunk = chunk;
      //Modified this line for QCs# 839 and 1028


      ChunkButtonBar.Container = container;
      ChunkButtonBar.Item = item;
      ChunkButtonBar.User = SessionState.User;
      ChunkButtonBar.Culture = culture;
      if (!Page.IsPostBack)
      {
        lbResult.Text = string.Empty;
        UpdateDataView();
      }
    }

    private void UpdateDataView()
    {
      if (chunk != null)
      {
        if (chunk.Text == HyperCatalog.Business.Chunk.BlankValue)
        {
          uwToolbar.Items.FromKeyButton("ilb").Selected = true;
          txtValue.Enabled = false;
          //txtValue.Text = null; //ACQ10.0 commented
          txtValue.Text ="0"; //ACQ10.0
        }
        else
        {
          txtValue.Text = chunk.Text;
        }
        imgStatus.ImageUrl = "/hc_v4/img/S" + HyperCatalog.Business.Chunk.GetStatusFromEnum(chunk.Status) + ".gif";
        lbStatus.Text = chunk.Status.ToString();
        ChunkComment1.Chunk = ChunkModifier1.Chunk = chunk;
        ChunkModifier1.Visible = true;
      }
      else
      {
        imgStatus.ImageUrl = "/hc_v4/img/SM.gif";
        lbStatus.Text = " [Missing]";
        txtValue.Text = string.Empty;
        ChunkModifier1.Visible = false;
      }
      txtValue.MaxLength = container.MaxLength;
      // Test if chunk is empty. If yes, force display.
      if (txtValue.Text == HyperCatalog.Business.Chunk.BlankValue)
      {
        txtValue.Value = null;
        txtValue.Enabled = false;
        uwToolbar.Items.FromKeyButton("ilb").Pressed(true);
      }
    }

    private void SaveChunk(ChunkStatus status, bool lockTranslations)
    {
      string error = string.Empty;
      string Value = txtValue.Text;
      if (uwToolbar.Items.FromKeyButton("ilb").Selected)
      {
        Value = HyperCatalog.Business.Chunk.BlankValue;
      }
      //ACQ10.0 Starts
      if (Value.Length <= 0)
      {
          Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "<script>alert('Please, enter a value!');</script>");
          return;
      }
      //ACQ10.0 Ends
      if (chunk != null)
      // Test if user has made a change that allows database update
      {
         // 1035 fix (Deepak S) START -- Unable to set ##BLANK##
        //if (txtValue.Text != chunk.Text || ChunkComment1.Comment != chunk.Comment || status != chunk.Status)
        if (Value != chunk.Text || ChunkComment1.Comment != chunk.Comment || status != chunk.Status)
        // 1035 fix (Deepak S) END -- Unable to set ##BLANK##
        {
          chunk.Text = Value;
          chunk.Comment = ChunkComment1.Comment;
          chunk.Status = status;
        }
      }
      else
      {
        chunk = new HyperCatalog.Business.Chunk(itemId, containerId, culture.Code, Value, ChunkComment1.Comment, status, SessionState.User.Id);
      }
      if (chunk.Save(SessionState.User.Id))
      {

        //Added this line for QCs# 839 and 1028
        chunk.ModifyDate = DateTime.UtcNow;
        //Added this line for QCs# 839 and 1028


        lbResult.Text = "<br/>Chunk saved!";
        lbResult.CssClass = "hc_success";
        if (Value == HyperCatalog.Business.Chunk.BlankValue)
        {
          Value = HyperCatalog.Business.Chunk.BlankText;
        }
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "update", "<script>UpdateGrid('" + HyperCatalog.Business.Chunk.GetStatusFromEnum(chunk.Status) + "', '" + Value.Replace("'", "\\\'") + "');</script>");
        if (!lockTranslations)
        {
          chunk.ForceTranslationsTo(SessionState.User.Id, ChunkStatus.Draft);
          SessionState.QDEChunk = chunk;
        }

        //Added this line for QCs# 839 and 1028
        ChunkModifier1.Chunk = chunk;
        //Added this line for QCs# 839 and 1028

      }
      else
      {
        lbResult.Text = "<br/>Error: " + HyperCatalog.Business.Chunk.LastError;
        lbResult.CssClass = "hc_error";
      }
    }

    private void DeleteChunk()
    {
      if (chunk.Delete(SessionState.User.Id))
      {
        chunk = null;
        uwToolbar.Items.FromKeyButton("ilb").Pressed(false);
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "update", "<script>UpdateGrid('" + HyperCatalog.Business.Chunk.GetStatusFromEnum(ChunkStatus.Missing) + "', '');</script>");
      }
      else
      {
        lbResult.Text = "<br/>Error: " + HyperCatalog.Business.Chunk.LastError;
        lbResult.CssClass = "hc_error";
      }
    }

    #region Chunk Event handlers
    private void Chunk_DraftClick(object sender, ChunkButtonBarEventArgs e)
    {
      SaveChunk(ChunkStatus.Draft, e.LockTranslations);
      SessionState.QDEChunk = chunk;
      UpdateDataView();
    }

    private void Chunk_FinalClick(object sender, ChunkButtonBarEventArgs e)
    {
      SaveChunk(ChunkStatus.Final, e.LockTranslations);
      SessionState.QDEChunk = chunk;
      UpdateDataView();
    }

    private void Chunk_DeleteClick(object sender, ChunkButtonBarEventArgs e)
    {
      DeleteChunk();
      SessionState.QDEChunk = chunk;
      UpdateDataView();
    }
    private void Chunk_CopyClick(object sender, ChunkButtonBarEventArgs e)
    {
      HyperCatalog.Business.Chunk c = HyperCatalog.Business.Chunk.GetByKey(itemId, containerId, culture.FallbackCode);
      if (c != null)
      {
        txtValue.Text = c.Text;
        lbResult.Text = "Valued copied from master";
        lbResult.CssClass = "hc_success";
        lbResult.Visible = true;
      }
      else
      {
        lbResult.Text = "No master value found";
        lbResult.CssClass = "hc_error";
        lbResult.Visible = true;
      }
    }
      /*#ROR Starts
        private void Chunk_RejectClick(object sender, ChunkButtonBarEventArgs e)
        {
        }
       * #ROR Ends
      */

    #endregion

  }
}

