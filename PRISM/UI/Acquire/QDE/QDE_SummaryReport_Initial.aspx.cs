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
using HyperComponents.Data.dbAccess;
using System.Data.SqlClient;
using Infragistics.WebUI.UltraWebNavigator;

namespace HyperCatalog.UI.Acquire.QDE
{
    /// <summary>
    /// QDE_CloneItem class clones item
    /// </summary>
    public partial class QDE_SummaryReport_Initial : HCPage
    {
        #region Declarations
        public HyperCatalog.Business.Item itemObj;
        int skuLevelId = 7;
        Culture cul;
        bool displaySoftRoll = false;
        #endregion

        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region Retrieve Item information
            try
            {
                itemObj = QDEUtils.GetItemIdFromRequest();
                skuLevelId = HyperCatalog.Shared.SessionState.SkuLevel.Id;
                cul = QDEUtils.UpdateCultureCodeFromRequest();

                if (Request["r"] != null)
                {
                    displaySoftRoll = Convert.ToBoolean(Request["r"]);
                }
            }
            catch
            {
                throw new ArgumentException("Item Id was not provided");
            }
            #endregion

            #region Capability
            //if ((SessionState.User.IsReadOnly) && (SessionState.User.HasCapability(CapabilitiesEnum.EXPORT_ITEMS)) &&
            //  (itemObj.LevelId < Convert.ToInt32(SessionState.CacheParams["Item_ExportMaxLevel"].Value)))
            //{
            //    UITools.DenyAccess(DenyMode.Popup);
            //}
            #endregion

            try
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Initialize", "<script>cbFilterID='" + cbFilter.ClientID + "'</script>");

                if (!Page.IsPostBack)
                {
                    UpdateDataView();
                }
            }
            catch (Exception ex)
            {
                UITools.JsCloseWin(Utils.jsReplace(ex.ToString()));
            }
        }
        private void AddChild(Node n, HyperCatalog.Business.Item obj)
        {
            if ((displaySoftRoll && (obj.IsRoll || obj.GetRoll() == null))
              || (!displaySoftRoll && !obj.IsRoll))
            {
                Node child = new Node();
                child.DataKey = obj.Id;
                child.Text = string.Empty;
                if (obj.IsRoll)
                    child.Text += "<img src='/hc_v4/img/ed_roll.gif'> ";
                child.Text += obj.FullName;
                child.ImageUrl = child.SelectedImageUrl = "/hc_v4/ig/s_" + obj.Status.ToString().Substring(0, 1) + ".gif";
                if (obj.LevelId == skuLevelId)
                {
                    child.ImageUrl = "/hc_v4/img/type_" + obj.TypeId.ToString() + ".png";
                    if (obj.Sku.ToUpper().EndsWith("T")) // Top Value
                    {
                        child.ImageUrl = "/hc_v4/img/type_1.png";
                    }
                    child.SelectedImageUrl = child.ImageUrl;
                    switch (obj.Status.ToString())
                    {
                        case ("O"):
                            child.Text = "<font color=gray>[O] </font>" + child.Text;
                            break;
                        case ("F"):
                            child.Text = "<font color=green>[F] </font>" + child.Text;
                            break;
                    }
                }
                else
                {
                    if (obj.LevelId > skuLevelId)
                    {
                        child.SelectedImageUrl = child.ImageUrl = "/hc_v4/img/option.png";
                    }
                }
                n.Nodes.Add(child);

                foreach (HyperCatalog.Business.Item subItem in obj.Childs)
                {
                    AddChild(child, subItem);
                }
            }
        }

        private void UpdateDataView()
        {
            // Check if main item has a roll in its descendants
            if (itemObj.HasRollDescendant() || itemObj.GetRoll() != null)
            {
                cbFilter.Visible = true;
                UITools.ShowToolBarLabel(uwToolbar, "lbSoftRoll");
                UITools.ShowToolBarSeparator(uwToolbar, "CloseSep");
            }
            else
            {
                cbFilter.Visible = false;
                UITools.HideToolBarLabel(uwToolbar, "lbSoftRoll");
                UITools.HideToolBarSeparator(uwToolbar, "CloseSep");
            }

            cbFilter.Checked = displaySoftRoll;

            //lbTitle.Text = "Export product - " + itemObj.FullName;
            //lbError.Visible = false;
            webTree.ClearAll();
            Node mainNode = webTree.Nodes.Add("Select the Items for Summary Report");
            mainNode.CheckBox = CheckBoxes.False;
            if (displaySoftRoll && itemObj.GetRoll() != null)
                itemObj = itemObj.GetRoll();
            AddChild(mainNode, itemObj);
            webTree.ExpandAll();
        }
        protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
        {
            string btn = be.Button.Key.ToLower();
            if (btn.Equals("submit"))
            {
                Submit();
            }
        }
        private void Submit()
        {
            string itemList = string.Empty;
            for (int i = 0; i < webTree.CheckedNodes.Count; i++)
            {
                if (i > 0) itemList += ",";
                itemList += ((Node)webTree.CheckedNodes[i]).DataKey;
            }

            if (itemList.Length > 0)
            {
                if (itemList.Split(',').Length < Convert.ToInt32(ApplicationSettings.Parameters["MaxItems_SummaryReport"].Value))
                {  
                  Response.Redirect("../../Admin/Reports/SummaryReport.aspx?ItemIds=" + itemList + "&CultureCode=" + cul.Code + "&IsRoll=" + displaySoftRoll);
                }
                else
                {
                 int  MaxLevel =  Convert.ToInt32(ApplicationSettings.Parameters["MaxItems_SummaryReport"].Value);

                 Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "ClientScript", "alert('The Max Limit for Summary Report is " + MaxLevel + ".Please generate the Summary Report for a lesser number of Items');", true);
                }
            }
            else
            {
                lbError.CssClass = "hc_error";
                lbError.Text = "Invalid selection for Summary Report";
                lbError.Visible = true;
            }
        }

    }
}
