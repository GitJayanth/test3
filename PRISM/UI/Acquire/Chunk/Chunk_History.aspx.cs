using System;
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
using HyperCatalog.Shared;
using HyperCatalog.Business;


namespace HyperCatalog.UI.Acquire.Chunk
{
    /// <summary>
    /// Description résumée de Chunk_History.
    /// </summary>
    public partial class Chunk_History : HCPage
    {
        protected string classTD = string.Empty;
        private System.Int64 itemId;
        private int containerId;
        private string cultureCode;
        private HyperCatalog.Business.Chunk chunk = null;

        protected void Page_Load(object sender, System.EventArgs e)
        {

            try
            {
                containerId = Convert.ToInt32(Request["d"]);
                cultureCode = QDEUtils.UpdateCultureCodeFromRequest().Code;
                itemId = QDEUtils.GetQueryItemIdFromRequest();
                chunk = ChunkWindow.GetChunk(itemId, containerId, cultureCode);

                if (!Page.IsPostBack)
                {
                    ShowHistory();
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "close", "<script>alert('Error: " + UITools.CleanJSString(ex.ToString()) + "'");
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

        private void ShowHistory()
        {
            if (SessionState.Culture.Language.Rtl)
            {
                classTD = "rtl";
            }
            repHistory.DataSource = chunk.History();
            repHistory.DataBind();
        }
        protected string CheckValue(object s)
        {
            try
            {
                if (s.ToString() == HyperCatalog.Business.Chunk.BlankValue)
                {
                    return HyperCatalog.Business.Chunk.BlankText;
                }
                else
                {
                    return UITools.HtmlEncode(s.ToString()).Replace(Environment.NewLine, "<br/>");
                }
            }
            catch
            {
                return s.ToString();
            }
        }


    }

}