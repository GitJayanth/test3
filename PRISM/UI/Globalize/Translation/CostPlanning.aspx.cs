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

namespace HyperCatalog.UI.Globalize.Translation
{
	/// <summary>
	/// Description résumée de CostPlanning.
	/// </summary>
	public partial class CostPlanning : HCPage
	{
    #region Business
    private HyperCatalog.Business.TRClassList _Classes
    {
      get
      {
        return (HyperCatalog.Business.TRClassList)ViewState["_Classes"];
      }
      set
      {
        ViewState["_Classes"] = value;
      }
    }

    protected HyperCatalog.Business.TRClassList Classes
    {
      get
      {
        if (_Classes == null)
          _Classes = HyperCatalog.Business.TRClass.GetAll(String.Format(" dbo.IsItemInUserScope(ClassId,{0}) = 1 ",Shared.SessionState.User.Id));
        return _Classes;
      }
    }
    private Business.TRCostPlanningSettings costSettings
    {
      get
      {
        return (Business.TRCostPlanningSettings)ViewState["costSettings"];
      }
      set
      {
        ViewState["costSettings"] = value;
      }
    }

    protected int PeriodRange
    {
      get
      {
        if (forecastRangeValue.SelectedIndex < 0)
          forecastRangeValue.SelectedIndex = 0;
        return Convert.ToInt32(forecastRangeValue.SelectedValue);
      }
    }
    protected bool PeriodRangeChanged = false;
    protected ArrayList datasources
    {
      get
      {
        ArrayList _ds = (ArrayList)ViewState["datasources"];
        if (_ds == null)
          ViewState["datasources"] = _ds = new ArrayList();
        return _ds;
      }
    }
    protected int projects
    {
      get
      {
        return (int)ViewState["projects"];
      }
      set
      {
        ViewState["projects"] = value;
      }
    }

    protected int languages
    {
      get
      {
        return (int)ViewState["languages"];
      }
      set
      {
        ViewState["languages"] = value;
      }
    }
    
    protected bool translationManager
    {
      get
      {
         return Shared.SessionState.User.HasCapability(Business.CapabilitiesEnum.TRANSLATION_COST_PLANNING);
      }
    }

    #endregion

    protected override void OnLoad(EventArgs e)
    {
      if (!IsPostBack)
      {
        costSettings = Business.TRCostPlanningSettings.Get();
        projectCostValue.Text = costSettings.ProjectCost.ToString("G29");
        wordCostValue.Text = costSettings.WordCost.ToString("G29");
        forecastRangeValue.SelectedValue = costSettings.PeriodRange.ToString();

        BindClasses();
      }

      base.OnLoad (e);
    }

    protected override void OnPreRender(EventArgs e)
    {
      bool TRmanager = translationManager;
      mainToolBar.Visible = TRmanager;

      projectCostValue.Enabled = TRmanager;
      wordCostValue.Enabled = TRmanager;
      forecastRangeValue.Enabled = TRmanager;

      if (HyperCatalog.Shared.SessionState.User.IsReadOnly)
      {
        mainToolBar.Items.FromKeyButton("Save").Enabled = forecastRangeValue.Enabled =
          wordCostValue.Enabled = projectCostValue.Enabled = false;
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
      classRepeater.ItemCreated += new RepeaterItemEventHandler(classRepeater_ItemCreated);
      classRepeater.ItemDataBound += new RepeaterItemEventHandler(classRepeater_ItemDataBound);
      forecastRangeValue.SelectedIndexChanged += new EventHandler(forecastRangeValue_SelectedIndexChanged);
      mainToolBar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(mainToolBar_ButtonClicked);
		}
    #endregion

    #region Data load & bind
    private void BindClasses()
    {
      //get all TR Classes that are in current user scope
      classRepeater.DataSource = Classes;
      classRepeater.DataBind();
    }

    private void classRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
      Business.Debug.Trace("WEBUI", "Begin classRepeater_ItemDataBound", Business.DebugSeverity.Low);

      DataGrid costGrid = (DataGrid)e.Item.FindControl("costGrid");
      Label avgProjects = (Label)e.Item.FindControl("avgProjects");
      Label avgLanguages = (Label)e.Item.FindControl("avgLanguages");
      if (costGrid!=null)
      {
        if (!IsPostBack || PeriodRangeChanged)
        {
          int _projects = 0, _languages = 0;
          Business.Debug.Trace("WEBUI", "Get planning values", Business.DebugSeverity.Low);
          DataSet ds = Classes[e.Item.ItemIndex].GetPlanningValues(Shared.SessionState.User.Id, PeriodRange, translationManager, out _projects, out _languages);
          projects = _projects;
          languages = _languages;
          if (!IsPostBack)
            datasources.Add(ds);
          else
            datasources[e.Item.ItemIndex] = ds;

          costGrid.DataSource = ds;
          Business.Debug.Trace("WEBUI", "Bind cost grid", Business.DebugSeverity.Low);
          costGrid.DataBind();

          avgProjects.Text = projects.ToString();
          avgLanguages.Text = languages.ToString();
        }

        Calculate(costGrid);
      }

      Business.Debug.Trace("WEBUI", "End classRepeater_ItemDataBound", Business.DebugSeverity.Low);
    }

    private void classRepeater_ItemCreated(object sender, RepeaterItemEventArgs e)
    {
      Business.Debug.Trace("WEBUI", "Begin classRepeater_ItemCreated", Business.DebugSeverity.Low);

      Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar gridToolBar = (Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar)e.Item.FindControl("gridToolBar");
      DataGrid costGrid = (DataGrid)e.Item.FindControl("costGrid");
      Label avgProjects = (Label)e.Item.FindControl("avgProjects");
      Label avgLanguages = (Label)e.Item.FindControl("avgLanguages");
      if (costGrid!=null)
      {
        gridToolBar.Visible = !translationManager;
        gridToolBar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(gridToolBar_ButtonClicked);
        //add necessary columns
        for (int i = 1 ; i <= PeriodRange ; i++)
        {
          TemplateColumn newCol = new TemplateColumn();
          DateTime date = DateTime.UtcNow.AddMonths(i);
          string columnName = "M" + date.Year.ToString() + date.Month.ToString();
          
          newCol.HeaderText = DateTime.UtcNow.AddMonths(i).ToString("y");
          CostTemplate iTemplate = new CostTemplate(columnName,!translationManager);
          newCol.ItemTemplate = iTemplate;
          costGrid.Columns.AddAt(i,newCol);
        }
      }

      Business.Debug.Trace("WEBUI", "End classRepeater_ItemCreated", Business.DebugSeverity.Low);
    }

    #endregion

    #region Events
    private void forecastRangeValue_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (translationManager)
      {
        PeriodRangeChanged = true;
        BindClasses();
      }
    }

    private void mainToolBar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      //"Save"
      if (translationManager && costSettings!=null)
      {
        costSettings.Save(Convert.ToDecimal(projectCostValue.Text),Convert.ToDecimal(wordCostValue.Text),Convert.ToInt32(forecastRangeValue.SelectedValue));
        foreach (RepeaterItem item in classRepeater.Items)
          Calculate((DataGrid)item.FindControl("costGrid"));
      }
    }

    private void gridToolBar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      if (!translationManager)
      {
        RepeaterItem item = (RepeaterItem)((Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar)sender).NamingContainer;
        DataGrid costGrid = (DataGrid)item.FindControl("costGrid");
        switch (be.Button.Key)
        {
          case "Save":
            Business.TRClass classe = Classes[item.ItemIndex];
            if (costGrid!=null)
              foreach (DataGridItem rowItem in costGrid.Items)
              {
                int levelId = (int)((DataSet)datasources[item.ItemIndex]).Tables[0].Rows[rowItem.DataSetIndex].ItemArray[1];
                for (int i = 1 ; i <= PeriodRange ; i++)
                {
                  Infragistics.WebUI.WebDataInput.WebNumericEdit numEdit = (Infragistics.WebUI.WebDataInput.WebNumericEdit)rowItem.Cells[i].Controls[0];
                  classe.SetPlanningValue(levelId, Shared.SessionState.User.Id, DateTime.UtcNow.AddMonths(i), Convert.ToInt32(numEdit.ValueInt));
                }
              }

            break;
        }
        Calculate(costGrid);
      }
    }
    #endregion

    private void Calculate(DataGrid costGrid)
    {
      Business.Debug.Trace("WEBUI", "Begin Calculate", Business.DebugSeverity.Medium);

      if (costGrid != null)
      {
        //for each class
        foreach (DataGridItem rowItem in costGrid.Items)
        {
          int wordCount = (int)((DataSet)datasources[0]).Tables[0].Rows[rowItem.DataSetIndex]["AvgWordCount"];
          Decimal total = 0;
          //for each month
          for (int i = 1 ; i <= PeriodRange ; i++)
          {
            Control control = (Control)rowItem.Cells[i].Controls[0];
            int value = 0;
            if (control is Infragistics.WebUI.WebDataInput.WebNumericEdit)
              value = Convert.ToInt32(((Infragistics.WebUI.WebDataInput.WebNumericEdit)control).ValueInt);
            else if (control is Label)
              value = Convert.ToInt32(((Label)control).Text);
            total += value * wordCount * languages * Convert.ToDecimal(wordCostValue.Text);
          }
          Business.Debug.Trace("WEBUI", "Total for this row = " + total.ToString(), Business.DebugSeverity.Medium);
          //add global cost of projects
          if (total > 0)
          {
            total += projects * Convert.ToDecimal(projectCostValue.Text);
            Business.Debug.Trace("WEBUI", "Total for this row after project computation= " + total.ToString(), Business.DebugSeverity.Medium);

          }
          ((Label)rowItem.Cells[rowItem.Cells.Count-1].FindControl("totalTxt")).Text = total.ToString();
        }
      }

      Business.Debug.Trace("WEBUI", "End Calculate", Business.DebugSeverity.Medium);
    }
  }

}
