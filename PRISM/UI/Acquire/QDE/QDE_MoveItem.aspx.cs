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
using HyperComponents.Data.dbAccess;
using HyperCatalog.Shared;
using Infragistics.WebUI.UltraWebGrid;
using HyperCatalog.Business;
using System.Data.SqlClient;
#endregion

namespace HyperCatalog.UI.Acquire
{
  /// <summary>
  /// Move a product
  /// </summary>
  public partial class QDE_MoveItem : HCPage
  {
    #region Declarations

    public HyperCatalog.Business.Item itemObj;
    protected System.Web.UI.WebControls.Label Label1;
    protected System.Web.UI.WebControls.Table Table1;
    protected System.Web.UI.WebControls.CheckBox CheckBox1;
    private int levelCount;
    #endregion

    delegate bool ItemUpdateWorkingTables(System.Int64 itemId, bool isSku);

    #region Code généré par le Concepteur Web Form
    override protected void OnInit(EventArgs e)
    {
      InitializeComponent();
      base.OnInit(e);
    }

    /// <summary>
    /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
    /// le contenu de cette méthode avec l'éditeur de code.
    /// </summary>
    private void InitializeComponent()
    {
      this.MainToolBar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.MainToolBar_ButtonClicked);
      this.rPossibleParents.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rPossibleParents_ItemDataBound);

    }
    #endregion

    public StepEnum Step
    {
      get { return (StepEnum)ViewState["Step"]; }
      set { ViewState["Step"] = value; }
    }

    public int StepNumber
    {
      get { return (int)ViewState["StepNumber"]; }
      set { ViewState["StepNumber"] = value; }
    }

    public int NbSteps
    {
      get { return (int)ViewState["NbSteps"]; }
      set { ViewState["NbSteps"] = value; }
    }

    public enum StepEnum : int
    {
      None = 0,
      MainInfo = 1,
      InputFormsInfo = 2
    }

    protected void Page_Load(object sender, System.EventArgs e)
    {


      #region Retrieve Item information
      try
      {
        if (Request["i"] != null)
          itemObj = HyperCatalog.Business.Item.GetByKey(Convert.ToInt64(Request["i"]));
        else if (QDEUtils.GetItemIdFromRequest() != null)
          itemObj = QDEUtils.GetItemIdFromRequest();

        if (itemObj != null)
          Ultrawebtoolbar1.Items.FromKeyLabel("ItemName").Text = "Move product - " + itemObj.FullName;

        if (Request["c"] != null)
          QDEUtils.UpdateCultureCodeFromRequest();
      }
      catch
      {
        throw new ArgumentException("Item Id was not provided");
      }
      #endregion

      if (itemObj != null)
      {
        #region Capability
        if ((SessionState.User.IsReadOnly) && (SessionState.User.HasCapability(CapabilitiesEnum.MOVE_ITEMS)) &&
            (itemObj.LevelId < Convert.ToInt32(SessionState.CacheParams["Item_MoveMaxLevel"].Value)))
        {
          UITools.JsCloseWin();
        }
        #endregion
        if (HyperCatalog.Business.ApplicationParameter.IOHeirachyStatus() == 1)
        {
            UITools.JsCloseWin("The Product Hierarchy Refresh job is in progress.  User cannot perform this action.  Please try once the job completes!");

        }

        try
        {
          if (!Page.IsPostBack)
          {
            Step = StepEnum.MainInfo;
            NbSteps = 2;
            StepNumber = 1;
            ProcessStep();
          }
        }
        catch
        {
          Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "<script>window.Close();</script>");
        }
      }
    }

    private void MoveItem()
    {
      System.Int64 parentId = Convert.ToInt64(ViewState["NewParentId"]);
      System.Int64 oldParentId = itemObj.ParentId;
      if (parentId > 0)
      {
        using (Database dbObj = Utils.GetMainDB())
        {
          if (dbObj != null)
          {
            dbObj.TimeOut = 300;
            int r = dbObj.RunSPReturnInteger("dbo._Item_Move",
              new SqlParameter("@ItemId", itemObj.Id),
              new SqlParameter("@NewParentId", parentId),
              new SqlParameter("@UserId", SessionState.User.Id));
            dbObj.CloseConnection();

            if (r < 0) // error
            {
              lbError.CssClass = "hc_error";
              lbError.Text = dbObj.LastError;
              lbError.Visible = true;
            }
            else
            {
              HyperCatalog.Shared.SessionState.CurrentItem = itemObj;
              SessionState.User.LastVisitedItem = itemObj.Id;
              SessionState.User.QuickSave();
              HyperCatalog.Business.Item.UpdateAllItemProductLines();

              ItemUpdateWorkingTables dlgt1 = new ItemUpdateWorkingTables(QDEUtils.wsTranslationObj.ItemUpdateWorkingTables);
              // call BeginInvoke to initiate the asynchronous operation
              IAsyncResult ar1 = dlgt1.BeginInvoke(itemObj.Id, itemObj.Sku != string.Empty, null, null);

              ItemUpdateWorkingTables dlgt2 = new ItemUpdateWorkingTables(QDEUtils.wsTranslationObj.ItemUpdateWorkingTables);
              // call BeginInvoke to initiate the asynchronous operation
              IAsyncResult ar2 = dlgt2.BeginInvoke(oldParentId, false, null, null);

              Item rollItem = itemObj.GetRoll();
              if (rollItem != null)
              {
                ItemUpdateWorkingTables dlgt3 = new ItemUpdateWorkingTables(QDEUtils.wsTranslationObj.ItemUpdateWorkingTables);
                // call BeginInvoke to initiate the asynchronous operation
                IAsyncResult ar3 = dlgt3.BeginInvoke(rollItem.Id, rollItem.Sku != string.Empty, null, null);
              }

              Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "UpdateTV", "<script>UpdateTV();</script>");
            }
          }
          else // dbObj is null
          {
            lbError.CssClass = "hc_error";
            lbError.Text = "Object database is null";
            lbError.Visible = true;
          }
        }
      }
      else // parentId = 0
      {
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "missingParent", "<script>alert('Missing parent');</script>");
      }
    }

    private void ProcessStep()
    {
      pMain.Visible = pInputForms.Visible = false;
      UITools.ShowToolBarButton(MainToolBar, "Apply");
      UITools.ShowToolBarButton(MainToolBar, "Prev");
      UITools.ShowToolBarButton(MainToolBar, "Next");
      switch (Step)
      {
        case StepEnum.MainInfo:
          lbWizardTitle.Text = "Item categorization";
          pMain.Visible = true;
          StepNumber = 1;
          UITools.HideToolBarButton(MainToolBar, "Apply");
          UITools.HideToolBarButton(MainToolBar, "Prev");
          RetrievePossibleParents();
          break;
        case StepEnum.InputFormsInfo:
          lbWizardTitle.Text = "Inputforms attached";
          pInputForms.Visible = true;
          UITools.HideToolBarButton(MainToolBar, "Next");
          InputFormsInfo();
          break;
      }
      if (Step != StepEnum.MainInfo)
      {
        lbWizardTitle.Text = lbWizardTitle.Text + " [step " + StepNumber.ToString() + "/" + NbSteps.ToString() + "]";
      }
    }

    private void MainToolBar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      switch (be.Button.Key.ToLower())
      {
        #region Apply button
        case "apply":
          {
            MoveItem();
            break;
          }
        #endregion
        #region Prev button
        case "prev":
          {
            if (StepNumber > 0) StepNumber--;
            switch (Step)
            {
              case StepEnum.InputFormsInfo:
                Step = StepEnum.MainInfo;
                break;
            }
            ProcessStep();
            break;
          }
          #endregion
        #region Next button
        case "next":
          {
            if (StepNumber < NbSteps) StepNumber++;
            switch (Step)
            {
              case StepEnum.MainInfo:
                if (Request.Form["Parent"] != null)
                {
                  ViewState["NewParentId"] = Request.Form["Parent"];
                  Step = StepEnum.InputFormsInfo;
                }
                else
                {
                  Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "<SCRIPT>alert('you must select a parent to continue !');</SCRIPT>");
                }
                break;
            }
            ProcessStep();
            break;
          }
          #endregion
      }
    }


    #region Main
    private void RetrievePossibleParents()
    {
      if (itemObj != null)
      {
        lbParents.Text = "You are moving a product which level is " + itemObj.Level.Name + ".</br> The program is showing to you the different possible destination parents to move it.";
        if (itemObj.Id > 0)
        {
          // list of possible parents
          using (Database dbObj = Utils.GetMainDB())
          {
            if (dbObj != null)
            {
              DataSet ds = dbObj.RunSPReturnDataSet("dbo._Item_GetPossibleParentsForCloningMoving", "Items",
                new SqlParameter("@ItemId", itemObj.Id),
                new SqlParameter("@UserId", SessionState.User.Id));
              dbObj.CloseConnection();
              if (ds != null)
              {
                if (dbObj.LastError.Length > 0)
                {
                  // Error
                  lbError.CssClass = "hc_error";
                  lbError.Text = dbObj.LastError;
                  lbError.Visible = true;
                }
                else
                {
                  // display list
                  rPossibleParents.DataSource = ds;
                  rPossibleParents.DataBind();
                }

                ds.Dispose();
              }
              else
              {
                throw new Exception(dbObj.LastError);
              }
            }
          }
        }
      }
      else
      {
        // Error;
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "<script>window.Close();</script>");
      }
    }

    private void rPossibleParents_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
    {
      Label lbRadioButton = (Label)e.Item.FindControl("lbRabioButton");
      Label lbParent = (Label)e.Item.FindControl("lbParent");

      DataRowView dr = (DataRowView)e.Item.DataItem;
      if (dr != null)
      {
        int levelId = Convert.ToInt32(dr["LevelId"]);
        bool canMove = Convert.ToBoolean(dr["CanClone"]);
        string name = dr["ItemName"].ToString();
        System.Int64 id = Convert.ToInt64(dr["ItemId"]);


        if (lbParent != null)
        {
          lbParent.Text = "|";
          for (int i = 0; i < levelId; i++)
          {
            lbParent.Text += "--";
          }
          lbParent.Text += name + " [" + dr["LevelName"].ToString() + "]"; //+" (" + itemObj.LevelId.ToString() + " - "  + dr["LevelId"].ToString() + ")";
        }
        if (lbRadioButton != null && canMove)
        {
          lbRadioButton.Text = "<input type='radio' name='Parent' value='" + id.ToString() + "' />";
          lbRadioButton.Visible = true;
        }

        if (dr["ItemId"].ToString() == itemObj.ParentId.ToString() || dr["ParentId"].ToString() == itemObj.ParentId.ToString())
        {
          e.Item.Visible = false;
        }
        if (!canMove) // For all the items that cannot be selected
        {
          e.Item.Visible = Convert.ToInt32(dr["LevelId"]) < itemObj.LevelId;
        }
      }
    }

    #endregion

    #region InputForms

    private void InputFormsInfo()
    {
      // Get input forms attached to the current and target item
      using (Database dbObj = Utils.GetMainDB())
      {
        dbObj.TimeOut = 300;
        using (DataSet ds = dbObj.RunSPReturnDataSet("_Itemmove_InputForm_GetAttached", new SqlParameter("@OriginItemId", itemObj.Id),
          new SqlParameter("@TargetParentId", ViewState["NewParentId"])))
        {
          dbObj.CloseConnection();

          string OldParentName = HyperCatalog.Business.Item.GetByKey(Convert.ToInt64(itemObj.ParentId)).FullName;
          string NewParentName = HyperCatalog.Business.Item.GetByKey(Convert.ToInt64(ViewState["NewParentId"])).FullName;

          if (dbObj.LastError == string.Empty)
          {
            lInputforms.Text = "<TABLE WIDTH='100%' class='datatable'><TR class='gh'><TD>Name</TD><TD>Description</TD><TD>Is active</TD>";
            lInputforms.Text += "<TR><TD COLSPAN='3' class='ugaf'>Current Input Forms</TD></TR>";
            if (ds.Tables[0].Rows.Count > 0)
            {
              for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
              {
                DataRow dr = ds.Tables[0].Rows[i];
                lInputforms.Text += "<TR>";
                lInputforms.Text += "<TD>" + dr["Name"].ToString() + "</TD>";
                lInputforms.Text += "<TD>" + dr["Description"].ToString() + "</TD>";
                lInputforms.Text += "<TD align='center'><INPUT Type='CheckBox' Disabled Checked='" + dr["IsActive"].ToString() + "'/></TD>";
                lInputforms.Text += "</TR>";
              }
            }
            else
            {
              lInputforms.Text += "<TR><TD COLSPAN='3'>No input form attached</TD></TR>";
            }
            lInputforms.Text += "<TR><TD COLSPAN='3' class='ugaf'>Future Input Forms under [" + NewParentName + "]</TD></TR>";
            if (ds.Tables[1].Rows.Count > 0)
            {
              for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
              {
                DataRow dr = ds.Tables[1].Rows[j];
                lInputforms.Text += "<TR>";
                lInputforms.Text += "<TD>" + dr["Name"].ToString() + "</TD>";
                lInputforms.Text += "<TD>" + dr["Description"].ToString() + "</TD>";
                lInputforms.Text += "<TD align='center'><INPUT Type='CheckBox' Disabled Checked='" + dr["IsActive"].ToString() + "'/></TD>";
                lInputforms.Text += "</TR>";
              }
            }
            else
            {
              lInputforms.Text += "<TR><TD COLSPAN='3'>No input form attached</TD></TR>";
            }
            lInputforms.Text += "</TABLE>";
          }
          else
          {
            lbError.CssClass = "hc_error";
            lbError.Text = dbObj.LastError;
            lbError.Visible = true;
          }

        }
      }
    }

    #endregion

  }
}
