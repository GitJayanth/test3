#region Copyright (c)  Hewlett-Packard. All Rights Reserved
/* ---------------------------------------------------------------------*
*        THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE.       *
* --------------------------------------------------------------------- *
 * History Section
 * Date             Name            Description                                         Ref
 * June 15 2009     S.Balakumar     ACQ 10 - ILB O/M + ILB at region level              #ACQ10.0
 * June 15 2009     Mahiba          Removal of Rejection functionality                  #ROR
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
  /// Description r�sum�e de Chunk_Date.
  /// </summary>
  public partial class Chunk_Date : HCPage
  {
    #region Declarations
    protected Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar Ultrawebtoolbar1;

    private long itemId = -1;
    private int containerId = -1;
    private bool isMandatory = false;
    protected HyperCatalog.Business.Chunk chunk;
    protected HyperCatalog.Business.Container container;
    protected HyperCatalog.Business.Item item;
    protected HyperCatalog.Business.Culture culture;
    #endregion

    #region Code g�n�r� par le Concepteur Web Form
    override protected void OnInit(EventArgs e)
    {
      //
      // CODEGEN�: Cet appel est requis par le Concepteur Web Form ASP.NET.
      //
      InitializeComponent();
      base.OnInit(e);
      ChunkButtonBar.DraftClick += new ChunkButtonBarClickEventHandler(Chunk_DraftClick);
      ChunkButtonBar.FinalClick += new ChunkButtonBarClickEventHandler(Chunk_FinalClick);
      ChunkButtonBar.DeleteClick += new ChunkButtonBarClickEventHandler(Chunk_DeleteClick);
      ChunkButtonBar.CopyClick += new ChunkButtonBarClickEventHandler(Chunk_CopyClick);
      //ChunkButtonBar.RejectClick += new ChunkButtonBarClickEventHandler(Chunk_RejectClick); #ROR Commented
    }

    /// <summary>
    /// M�thode requise pour la prise en charge du concepteur - ne modifiez pas
    /// le contenu de cette m�thode avec l'�diteur de code.
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
        // Fix for QC#7852 - Assigned the correct item Id to the itemID variable before retrieving the Chunk..
        itemId = item.Id;
        chunk = ChunkWindow.GetChunk(itemId, containerId, culture.Code);
        container = SessionState.QDEContainer;
        uwToolbar.Enabled = Request["ui"] != null;        
        System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");
        ci.DateTimeFormat.ShortDatePattern = SessionState.User.FormatDate;
        ci.DateTimeFormat.LongDatePattern = SessionState.User.FormatDate;
        dateValue.CalendarLayout.Culture = ci;
        System.Threading.Thread.CurrentThread.CurrentCulture = ci;
        //ACQ10.0 Starts
        //if (!isMandatory || culture.Type == CultureType.Regionale)
        //{
        //  UITools.HideToolBarButton(uwToolbar, "ilb");
        //}
        //ACQ10.0 Ends
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

      //#ACQ10.0 Starts
      if (chunk != null)
      {
          if (chunk.Text == HyperCatalog.Business.Chunk.BlankValue)
          {
              Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VarBlankChunk", "<script>var isBlankChunk = true;</script>");
              //dateValue.Enabled = false;
              dateValue.Value = HyperCatalog.Business.Chunk.BlankText;
          }
      }
      //#ACQ10.0 Ends
      if (!Page.IsPostBack)
      {

        //Added these line for QCs# 839 and 1028
        ChunkComment1.Chunk = chunk;
        ChunkModifier1.Chunk = chunk;
        //Added this line for QCs# 839 and 1028

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
            //#ACQ10.0 Starts
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VarBlankChunk", "<script>var isBlankChunk = true;</script>");
            //dateValue.Enabled = false;  //Commented
            //dateValue.Value = null; //Commented
            dateValue.Value = HyperCatalog.Business.Chunk.BlankText; 
            //#ACQ10.0 Ends
        }
        else
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VarBlankChunk", "<script>var isBlankChunk = false;</script>"); //#ACQ10.0
            string[] parts = chunk.Text.Split(Convert.ToChar("/"));
            dateValue.Value = new DateTime(Convert.ToInt32(parts[2]), Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]));
        }
        imgStatus.ImageUrl = "/hc_v4/img/S" + HyperCatalog.Business.Chunk.GetStatusFromEnum(chunk.Status) + ".gif";
        lbStatus.Text = chunk.Status.ToString();
        ChunkModifier1.Visible = true;
      }
      else
      {
        imgStatus.ImageUrl = "/hc_v4/img/SM.gif";
        lbStatus.Text = " [Missing]";
        dateValue.Value = null;
        ChunkModifier1.Visible = false;
      }
    }

    private void SaveChunk(ChunkStatus status, bool lockTranslations)
    {
      string error = string.Empty;
      string Value;
      if (uwToolbar.Items.FromKeyButton("ilb").Selected)
      {
        Value = HyperCatalog.Business.Chunk.BlankValue;
      }
      else
      {
        DateTime v = ((DateTime)dateValue.Value);
        Value = v.Month.ToString() + "/" + v.Day.ToString() + "/" + v.Year.ToString();
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
        if (Value != chunk.Text || ChunkComment1.Comment != chunk.Comment || status != chunk.Status)
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
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "update", "<script>UpdateGrid('" + HyperCatalog.Business.Chunk.GetStatusFromEnum(chunk.Status) + "', '" + Value + "');</script>");
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
        string[] parts = c.Text.Split(Convert.ToChar("/"));
        dateValue.Value = new DateTime(Convert.ToInt32(parts[2]), Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]));
        //dateValue.Value = Convert.ToDateTime(c.Text);
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
    /*
     * #ROR Starts
    private void Chunk_RejectClick(object sender, ChunkButtonBarEventArgs e)
    {
    }
     * #ROR Ends
    */
    #endregion
}
}

