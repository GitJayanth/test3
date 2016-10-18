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
using System.Text;
using System.Collections.Generic;
using HyperCatalog.Business;
using System.Data.SqlClient;
#endregion

#region History
/*=============Modification Details====================================
      Mod#1 Add ItemId Search
      Description: eZ# 71445
      Modfication Date:07/05/2007
      Modified by: Ramachandran
      
      Mod#2 State Maintainence
      Description: QC RQ4101
      Modfication Date:12/10/2007
      Modified by: Jothi
    
      Mod#3 PRISM UI To PDB Changes
      Description: 
      Modfication Date:02/02/2013
      Modified by: Rekha Thomas*/

#endregion
namespace HyperCatalog.UI
{
    /// <summary>
    /// Description résumée de CustomSearch .
    /// </summary>
    public partial class CustomSearch : HCPage
    {
        protected ArrayList filters2
        {
            get
            {
                ArrayList _filters =  (ArrayList)ViewState["filters2"];
                if (_filters == null)
                    return filters2 = new ArrayList();
                return _filters;
            }
            set
            {
                ViewState["filters2"] = value;
            }
        }

        protected ArrayList filterNames
        {
            get
            {
                ArrayList _filterNames = (ArrayList)ViewState["filterNames"];
                if (_filterNames == null)
                    return filterNames = new ArrayList();
                return _filterNames;
            }
            set
            {
                ViewState["filterNames"] = value;
            }
        }

        /************PRISM UI TO PDB CHANGES BY REKHA THOMAS***************/
        
        public string ParamValues 
        { 
            get 
            {
                return (string)ViewState["paramValues"];
            } 
            set 
            {
                ViewState["paramValues"] = value; 
            }
        }

        //private int flag;
        public int Flag
        {
            get
            {
                return  (int)ViewState["flag"];
                
            }
            set
            {
                ViewState["flag"] = value;
            }
        }
        /************END PRISM UI TO PDB CHANGES BY REKHA THOMAS***************/
        
        #region protected string[][] filterDataSource = new string[][] {...};
        protected string[][] filterDataSource = new string[][] { 
      new string[] { "productName", "Product name", "StringNeverEmpty" }, 
      new string[] { "productNumber", "Product number", "StringNeverEmpty" }, 
      new string[] { "containerName", "Container name", "StringNeverEmpty" }, 
      new string[] { "containerValue", "Container value", "Chunk" },
      new string[] { "containerGroup", "Container group", "ContainerGroup"},
      new string[] { "contentTouch", "Content touched", "DateTime" }, 
      new string[] { "inputForm", "Input form", "StringNeverEmpty" }, 
      new string[] { "itemCreation", "Node created", "DateTime" }, 
      //eZ# 71445 - Start
      //addded ItemId 
      new string[] { "ItemId","ItemId", "Int" }, 
      //eZ# 71445 - End
      new string[] { "blindDate", SessionState.CacheParams["HeaderLongNameBlind"].Value.ToString(), "DateTime" }, 
      new string[] { "fullDate", SessionState.CacheParams["HeaderLongNameLive"].Value.ToString(), "DateTime" }, 
      new string[] { "obsoleteDate", SessionState.CacheParams["HeaderLongNameObsolete"].Value.ToString(), "DateTime" }, 
      new string[] { "removalDate", SessionState.CacheParams["HeaderLongNameRemoval"].Value.ToString(), "DateTime" }, 
      new string[] { "endOfSupportDate", SessionState.CacheParams["HeaderLongNameSupport"].Value.ToString(), "DateTime" }
    };
        #endregion
        /******* QC# RQ4101 **************/
        protected ListItemCollection lstCol = new ListItemCollection();

        protected DataTable cultures
        {
            get
            {
                if (ViewState["cultures"] == null)
                {
                    using (HyperComponents.Data.dbAccess.Database dbObj = Utils.GetMainDB())
                    {
                        ViewState["cultures"] = dbObj.RunSQLReturnDataSet("SELECT CultureCode AS Code, '<SPAN style=\"' + (CASE CultureTypeId WHEN 0 THEN 'color:blue;font-weight:bold;' WHEN 1 THEN 'color:red;' ELSE '' END) + '\">' + CultureCode + '</SPAN>' AS [Name] FROM CulturesSorted ORDER BY AbsoluteSort").Tables[0];
                    }
                }
                return (DataTable)ViewState["cultures"];
            }
        }

        #region Code généré par le Concepteur Web Form
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN : Cet appel est requis par le Concepteur Web Form ASP.NET.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        ///		le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);
            newFilter.FilterAdded += new HyperCatalog.UI.RangeFilter.AddFilter(newFilter_FilterAdded);
            newFilter.FilterRemoved += new HyperCatalog.UI.RangeFilter.RemoveFilter(newFilter_FilterRemoved);
            paramList.ItemCreated += new RepeaterItemEventHandler(paramList_ItemCreated);
            /******* QC# RQ4101 **************/
            paramList.ItemDataBound += new RepeaterItemEventHandler(paramList_ItemDataBound);
            Flag = 0;
            
        }
        #endregion

        //private string SQLQuery = string.Empty; Rekha
        private HyperComponents.Data.dbAccess.Database dbObj = new HyperComponents.Data.dbAccess.Database(SessionState.CacheComponents["Crystal_DB"].ConnectionString);

        protected override void OnLoad(EventArgs e)
        {
            UITools.CheckConnection(Page);
            message.Visible = false;

            uwToolbar.Items.FromKeyButton("Options").Selected = false;
            if (dbList.SelectedValue != "0")
                prodExpToProv.InputAttributes["disabled"] = "disabled";
            if (!Page.IsPostBack)
            {
                resetChoice.Attributes["onclick"] = "return resetFilter();";
                dbList.Attributes["onchange"] = "document.getElementById('" + prodExpToProv.ClientID + "').disabled=(this.options[this.selectedIndex].value!=\"0\");";
                prodExpToProv.Checked = (SessionState.CacheParams["Search_ProductAlreadyToProvDefault"].Value.Equals("1"));

                newFilter.DataSource = filterDataSource;
                BindLists();
            }

            else
            {
                filterToAnchor(itemClassList, itemClassAnchor);
                filterToAnchor(itemLevelList, itemLevelAnchor);
                filterToAnchor(plList, plAnchor);
                filterToAnchor(containerStatusList, containerStatusAnchor);
                filterToAnchor(containerTypeList, containerTypeAnchor);
                filterToAnchor(regionCountryList, regionCountryAnchor);
                filterToAnchor(itemStatusList, itemStatusAnchor);
                filterToAnchor(itemTypeList, itemTypeAnchor);
                filterToAnchor(new CheckBox[] { prodWOcompat, prodWOPL, prodExpToProv }, productWithoutAnchor);
            }
            lbClassName.Text = fieldFilter.Items[0].Text = SessionState.ItemLevels[1].Name;
        }
        protected override void OnLoadComplete(EventArgs e)
        {
            DataBind();
            base.OnLoadComplete(e);
        }
        protected override void OnDataBinding(EventArgs e)
        {
            /******* QC# RQ4101 **************/
            string paraName = "";
            string operators = "";
            for (int i = 0; i < paramList.Items.Count; i++)
            {
                paraName = ((Label)paramList.Items[i].FindControl("parameterName")).Text;
                operators = ((DropDownList)paramList.Items[i].FindControl("operator")).SelectedIndex.ToString();
                lstCol.Add(new ListItem(paraName, operators));
            }
            paramList.DataSource = filters2;
            paramList.DataBind();
            base.OnDataBinding(e);
        }

        private void filterToAnchor(CheckBoxList list, HtmlAnchor anchor)
        {
            string title = string.Empty;
            foreach (ListItem item in list.Items)
                if (item.Selected)
                    title += (title == string.Empty ? "" : ", ") + item.Text;
            anchor.InnerHtml = (title != string.Empty ? "Filtered" : "No filter") + " &gt;";
            if (title != string.Empty)
                anchor.Title = title;
            else
                anchor.Title = "No filter";

        }
        private void filterToAnchor(CheckBox[] list, HtmlAnchor anchor)
        {
            string title = string.Empty;
            foreach (CheckBox item in list)
                if (item.Checked)
                    title += (title == string.Empty ? "" : ", ") + item.Text;
            anchor.InnerHtml = (title != string.Empty ? "Filtered" : "No filter") + " &gt;";
            if (title != string.Empty)
                anchor.Title = title;
            else
                anchor.Title = "No filter";

        }
        private void filterToAnchor(PLWebTree control, HtmlAnchor anchor)
        {
            string title = string.Empty;
            foreach (HyperCatalog.Business.PL item in control.GetCheckedPLs())
                title += (title == string.Empty ? "" : ", ") + item.ToString();
            anchor.InnerHtml = (title != string.Empty ? "Filtered" : "No filter") + " &gt;";
            if (title != string.Empty)
                anchor.Title = title;
            else
                anchor.Title = "No filter";
        }


        private void CreateSQLQueries(out bool mandatory)
        {
            bool useCrystalDB = dbList.SelectedValue != "0";
            bool caseSensitive = false;
            string datePattern = new System.Globalization.CultureInfo("en-US", true).DateTimeFormat.ShortDatePattern;
            #region Datatable to pass the SP parameters to SearchResult.aspx
            /***********PRISM UI TO PDB CHANGES BY REKHA THOMAS ******/
            string delimiter = ",";
            bool joinItems = false;
            bool joinPLC = false;
            bool hasContentTouch = false;
            DataTable objDTCustomParameters = new DataTable();
            objDTCustomParameters.Columns.Add(new DataColumn("ParameterName"));
            objDTCustomParameters.Columns.Add(new DataColumn("ParameterValue"));
            objDTCustomParameters.Columns.Add(new DataColumn("Opertor"));
            DataRow CustomParametersRow = null;

            DataTable objDTBusinessParameters = new DataTable();
            objDTBusinessParameters.Columns.Add(new DataColumn("ParameterName"));
            objDTBusinessParameters.Columns.Add(new DataColumn("ParameterValue"));
            DataRow BusinessParametersRow = null;

            #region custom filters
            foreach (RepeaterItem param in paramList.Items)
            {
                Repeater filterList = (Repeater)param.FindControl("filterList2");
                if (filterList.Items.Count > 0)
                {
                    #region getting the operator
                    DropDownList op = (DropDownList)param.FindControl("operator");
                    string opstr = "";
                    switch (op.SelectedValue.ToUpper())
                    {
                        case "AND":
                            opstr = "AND";
                            break;
                        case "OR":
                            opstr = "OR";
                            break;
                        case "NOR":
                            opstr = "NOT";
                            break;
                        default:
                            opstr = op.SelectedValue.ToUpper();
                            break;
                    }
                    #endregion

                    string join1 = string.Empty;
                    string join2 = string.Empty;
                    string join3 = string.Empty;
                    string condition1 = string.Empty;
                    string condition2 = string.Empty;
                    string condition3 = string.Empty;
                    //StringBuilder sb = new StringBuilder();
                    string condition = string.Empty;
                   
                    //StringBuilder sb = new StringBuilder(condition);
                    
                    foreach (RepeaterItem item in filterList.Items)
                    {
                        if (condition1 != string.Empty)
                            condition1 += " " + opstr + " ";
                        if (condition2 != string.Empty)
                            condition2 += " " + opstr + " ";
                        if (condition3 != string.Empty)
                            condition3 += " " + opstr + " ";

                        RangeFilter.FilterStruct filter = (RangeFilter.FilterStruct)((ArrayList)filters2[param.ItemIndex])[item.ItemIndex];

                        #region filter type
                        switch (filter.ID)
                        {
                            case "productName":
                                caseSensitive = true;
                                BuildCondition(ref condition, filter, useCrystalDB, datePattern, caseSensitive);
                                CustomParametersRow = objDTCustomParameters.NewRow();
                                CustomParametersRow["ParameterName"] = filter.ID.ToString();
                                CustomParametersRow["ParameterValue"] = condition.ToString();
                                CustomParametersRow["Opertor"] = opstr.ToString();
                                objDTCustomParameters.Rows.Add(CustomParametersRow);
                                break;
                            case "productNumber":
                                caseSensitive = true;
                                BuildCondition(ref condition, filter, useCrystalDB, datePattern, caseSensitive);
                                CustomParametersRow = objDTCustomParameters.NewRow();
                                CustomParametersRow["ParameterName"] = filter.ID.ToString();
                                CustomParametersRow["ParameterValue"] = condition.ToString();
                                CustomParametersRow["Opertor"] = opstr.ToString();
                                objDTCustomParameters.Rows.Add(CustomParametersRow);
                                break;
                            //eZ# 71445 - Start
                            //ItemId Query Building 
                            case "ItemId":
                                BuildCondition(ref condition, filter, useCrystalDB, datePattern, caseSensitive);
                                CustomParametersRow = objDTCustomParameters.NewRow();
                                CustomParametersRow["ParameterName"] = filter.ID.ToString();
                                CustomParametersRow["ParameterValue"] = condition.ToString();
                                CustomParametersRow["Opertor"] = opstr.ToString();
                                objDTCustomParameters.Rows.Add(CustomParametersRow);
                                break;
                            //eZ# 71445 - End
                            case "containerName":
                                BuildCondition(ref condition, filter, useCrystalDB, datePattern, caseSensitive);
                                CustomParametersRow = objDTCustomParameters.NewRow();
                                CustomParametersRow["ParameterName"] = filter.ID.ToString();
                                CustomParametersRow["ParameterValue"] = condition.ToString();
                                CustomParametersRow["Opertor"] = opstr.ToString();
                                objDTCustomParameters.Rows.Add(CustomParametersRow);
                                break;
                            case "containerValue":
                                BuildCondition(ref condition, filter, useCrystalDB, datePattern, caseSensitive);
                                CustomParametersRow = objDTCustomParameters.NewRow();
                                CustomParametersRow["ParameterName"] = filter.ID.ToString();
                                CustomParametersRow["ParameterValue"] = condition.ToString();
                                CustomParametersRow["Opertor"] = opstr.ToString();
                                objDTCustomParameters.Rows.Add(CustomParametersRow);
                                break;
                            case "containerGroup":
                                string fullPath = filter.Value;
                                string[] splitted = fullPath.Split('/');
                                string group = splitted[splitted.Length - 1];
                                string subPath = fullPath.Substring(0, fullPath.Length - group.Length);
                                
                                BuildCondition(ref condition, filter, useCrystalDB, datePattern, caseSensitive);
                                CustomParametersRow = objDTCustomParameters.NewRow();
                                CustomParametersRow["ParameterName"] = filter.ID.ToString();
                                if (useCrystalDB == true)
                                {
                                    ////if (!string.IsNullOrEmpty(opstr))
                                    if (Flag > 1) // Flag will be 1 for single select and "null" for multi select.
                                    {
                                        group = "N'" + group + "'"; // Since this is multi select, we  need to add extra quotes.
                                    }
                                    
                                }
                                else
                                {
                                    group = "='" + group + "'";
                                }
                                CustomParametersRow["ParameterValue"] = group.ToString();
                                CustomParametersRow["Opertor"] = opstr.ToString();
                                objDTCustomParameters.Rows.Add(CustomParametersRow);

                                CustomParametersRow = objDTCustomParameters.NewRow();
                                CustomParametersRow["ParameterName"] = "subPath";
                                if (useCrystalDB == true)
                                {
                                    if (Flag > 1)
                                    {
                                        subPath = "N'" + subPath + "'";
                                    }
                                }
                                else
                                {
                                    subPath = "='" + subPath + "'";
                                }
                                CustomParametersRow["ParameterValue"] = subPath.ToString();
                                CustomParametersRow["Opertor"] = opstr.ToString();
                                objDTCustomParameters.Rows.Add(CustomParametersRow);

                                CustomParametersRow = objDTCustomParameters.NewRow();
                                CustomParametersRow["ParameterName"] = "fullPath";
                                if (useCrystalDB == true)
                                {
                                    fullPath = fullPath + "/%";
                                    if (Flag > 1)
                                    {
                                        fullPath = "N'" + fullPath + "'";
                                    }
                                }
                                else
                                {
                                    fullPath = "LIKE '" + fullPath + "/%'";
                                }
                                
                                CustomParametersRow["ParameterValue"] = fullPath.ToString();
                                CustomParametersRow["Opertor"] = opstr.ToString();
                                objDTCustomParameters.Rows.Add(CustomParametersRow);
                                break;
                            case "inputForm":
                                BuildCondition(ref condition, filter, useCrystalDB, datePattern, caseSensitive);
                                CustomParametersRow = objDTCustomParameters.NewRow();
                                CustomParametersRow["ParameterName"] = filter.ID.ToString();
                                CustomParametersRow["ParameterValue"] = condition.ToString();
                                CustomParametersRow["Opertor"] = opstr.ToString();
                                objDTCustomParameters.Rows.Add(CustomParametersRow);
                                break;
                            case "itemCreation":
                                BuildCondition(ref condition, filter, useCrystalDB, datePattern, caseSensitive);
                                CustomParametersRow = objDTCustomParameters.NewRow();
                                CustomParametersRow["ParameterName"] = filter.ID.ToString();
                                CustomParametersRow["ParameterValue"] = condition.ToString();
                                CustomParametersRow["Opertor"] = opstr.ToString();
                                objDTCustomParameters.Rows.Add(CustomParametersRow);
                                break;
                            case "contentTouch":
                                BuildCondition(ref condition, filter, useCrystalDB, datePattern, caseSensitive);
                                CustomParametersRow = objDTCustomParameters.NewRow();
                                CustomParametersRow["ParameterName"] = filter.ID.ToString();
                                CustomParametersRow["ParameterValue"] = condition.ToString();
                                CustomParametersRow["Opertor"] = opstr.ToString();
                                objDTCustomParameters.Rows.Add(CustomParametersRow);
                                break;
                            case "blindDate":
                                BuildCondition(ref condition, filter, useCrystalDB, datePattern, caseSensitive);
                                CustomParametersRow = objDTCustomParameters.NewRow();
                                CustomParametersRow["ParameterName"] = filter.ID.ToString();
                                CustomParametersRow["ParameterValue"] = condition.ToString();
                                CustomParametersRow["Opertor"] = opstr.ToString();
                                objDTCustomParameters.Rows.Add(CustomParametersRow);
                                break;
                            case "fullDate":
                                BuildCondition(ref condition, filter, useCrystalDB, datePattern, caseSensitive);
                                CustomParametersRow = objDTCustomParameters.NewRow();
                                CustomParametersRow["ParameterName"] = filter.ID.ToString();
                                CustomParametersRow["ParameterValue"] = condition.ToString();
                                CustomParametersRow["Opertor"] = opstr.ToString();
                                objDTCustomParameters.Rows.Add(CustomParametersRow);
                                break;
                            case "obsoleteDate":
                                BuildCondition(ref condition, filter, useCrystalDB, datePattern, caseSensitive);
                                CustomParametersRow = objDTCustomParameters.NewRow();
                                CustomParametersRow["ParameterName"] = filter.ID.ToString();
                                CustomParametersRow["ParameterValue"] = condition.ToString();
                                CustomParametersRow["Opertor"] = opstr.ToString();
                                objDTCustomParameters.Rows.Add(CustomParametersRow);
                                break;
                            case "removalDate":
                                BuildCondition(ref condition, filter, useCrystalDB, datePattern, caseSensitive);
                                CustomParametersRow = objDTCustomParameters.NewRow();
                                CustomParametersRow["ParameterName"] = filter.ID.ToString();
                                CustomParametersRow["ParameterValue"] = condition.ToString();
                                CustomParametersRow["Opertor"] = opstr.ToString();
                                objDTCustomParameters.Rows.Add(CustomParametersRow);
                                //Testing Starteam
                                break;
                            case "endOfSupportDate":
                                BuildCondition(ref condition, filter, useCrystalDB, datePattern, caseSensitive);
                                CustomParametersRow = objDTCustomParameters.NewRow();
                                CustomParametersRow["ParameterName"] = filter.ID.ToString();
                                CustomParametersRow["ParameterValue"] = condition.ToString();
                                CustomParametersRow["Opertor"] = opstr.ToString();
                                objDTCustomParameters.Rows.Add(CustomParametersRow);
                                break;
                        }
                        #endregion filtertype
                        objDTCustomParameters.AcceptChanges();
                        Session["SelectedCustomParameters"] = objDTCustomParameters.Rows.Count == 0 ? null : objDTCustomParameters;
                    }
                    opstr = string.Empty;
                    //op.SelectedValue = "";
                   
                }
            }
            #endregion custom filters
            #region business filters
            HyperCatalog.Business.PLList pls = plList.GetCheckedPLs();

            string itemList = CreateDelimitedStringList(itemClassList, delimiter);
            BusinessParametersRow = objDTBusinessParameters.NewRow();
            BusinessParametersRow["ParameterName"] = "itemClassList";
            BusinessParametersRow["ParameterValue"] = itemList;
            objDTBusinessParameters.Rows.Add(BusinessParametersRow);

            string itemClassLevelList = CreateDelimitedStringList(itemLevelList, delimiter);
            BusinessParametersRow = objDTBusinessParameters.NewRow();
            BusinessParametersRow["ParameterName"] = "itemClassLevelList";
            BusinessParametersRow["ParameterValue"] = itemClassLevelList;
            objDTBusinessParameters.Rows.Add(BusinessParametersRow);

            string productLineList = CreateDelimitedStringList(pls, delimiter);
            BusinessParametersRow = objDTBusinessParameters.NewRow();
            BusinessParametersRow["ParameterName"] = "productLineList";
            BusinessParametersRow["ParameterValue"] = productLineList;
            objDTBusinessParameters.Rows.Add(BusinessParametersRow);

            string contentStatusList = CreateDelimitedStringList(containerStatusList, delimiter);
            BusinessParametersRow = objDTBusinessParameters.NewRow();
            BusinessParametersRow["ParameterName"] = "contentStatusList";
            BusinessParametersRow["ParameterValue"] = contentStatusList;
            objDTBusinessParameters.Rows.Add(BusinessParametersRow);

            string containerType1List = CreateDelimitedStringList(containerTypeList, delimiter);
            BusinessParametersRow = objDTBusinessParameters.NewRow();
            BusinessParametersRow["ParameterName"] = "containerTypeList";
            BusinessParametersRow["ParameterValue"] = containerType1List;
            objDTBusinessParameters.Rows.Add(BusinessParametersRow);

            bool missingChunks = false;

            missingChunks = (!useCrystalDB && containerStatusList.Items.FindByValue(Business.ChunkBase.GetStatusFromEnum(Business.ChunkStatus.Missing)).Selected);
            if (missingChunks && hasContentTouch)
            {
                message.Text = "Missing content cannot have been modified. Hence, \"Content status\" filter as missing is not taken into account.";
                message.Visible = true;
                missingChunks = false;
            }
            string nodeStatusList = CreateDelimitedStringList(itemStatusList, delimiter);
            BusinessParametersRow = objDTBusinessParameters.NewRow();
            BusinessParametersRow["ParameterName"] = "nodeStatusList";
            BusinessParametersRow["ParameterValue"] = nodeStatusList;
            objDTBusinessParameters.Rows.Add(BusinessParametersRow);

            string itemTypeConditionList = CreateDelimitedStringList(itemTypeList, delimiter);
            BusinessParametersRow = objDTBusinessParameters.NewRow();
            BusinessParametersRow["ParameterName"] = "itemTypeList";
            BusinessParametersRow["ParameterValue"] = itemTypeConditionList;
            objDTBusinessParameters.Rows.Add(BusinessParametersRow);
            joinItems = joinItems || (itemTypeConditionList != String.Empty);

            string cultureList = CreateDelimitedStringListForCulture(regionCountryList, delimiter, useCrystalDB);
            if (cultureList == string.Empty)
            {
                cultureList = SessionState.MasterCulture.Code.ToLower();
                if (useCrystalDB == false)
                {
                    //'in ('||''''||'ww-en'||''''||','||''''||'us-en'||''''||')'
                    cultureList = "'in ('||''''||'" + cultureList + "'||''''||')'";
                }
                else
                {
                    cultureList = "'" + cultureList + "'";
                }
            }
            BusinessParametersRow = objDTBusinessParameters.NewRow();
            BusinessParametersRow["ParameterName"] = "cultureList";
            BusinessParametersRow["ParameterValue"] = cultureList;
            objDTBusinessParameters.Rows.Add(BusinessParametersRow);

            joinItems = joinItems || prodWOPL.Checked;
            //if (prodWOPL.Checked)
            //{
                BusinessParametersRow = objDTBusinessParameters.NewRow();
                BusinessParametersRow["ParameterName"] = "prodWOPL";
                BusinessParametersRow["ParameterValue"] = prodWOPL.Checked;
                objDTBusinessParameters.Rows.Add(BusinessParametersRow);
            //}
            BusinessParametersRow = objDTBusinessParameters.NewRow();
            BusinessParametersRow["ParameterName"] = "joinItems";
            BusinessParametersRow["ParameterValue"] = joinItems;
            objDTBusinessParameters.Rows.Add(BusinessParametersRow);

            BusinessParametersRow = objDTBusinessParameters.NewRow();
            BusinessParametersRow["ParameterName"] = "joinPLC";
            BusinessParametersRow["ParameterValue"] = joinPLC;
            objDTBusinessParameters.Rows.Add(BusinessParametersRow);

            //if (prodExpToProv.Checked && !useCrystalDB)
            //{
                BusinessParametersRow = objDTBusinessParameters.NewRow();
                BusinessParametersRow["ParameterName"] = "prodExpToProv";
                BusinessParametersRow["ParameterValue"] = prodExpToProv.Checked;
                objDTBusinessParameters.Rows.Add(BusinessParametersRow);
            //}
           // if (prodWOcompat.Checked)
            //{
                BusinessParametersRow = objDTBusinessParameters.NewRow();
                BusinessParametersRow["ParameterName"] = "prodWOcompat";
                BusinessParametersRow["ParameterValue"] = prodWOcompat.Checked;
                objDTBusinessParameters.Rows.Add(BusinessParametersRow);
            //}
            objDTBusinessParameters.AcceptChanges();
            Session["SelectedBusinessParameters"] = objDTBusinessParameters.Rows.Count == 0 ? null : objDTBusinessParameters;
            mandatory = (!useCrystalDB && containerStatusList.Items.FindByValue("Mandatory Containers").Selected);

            #endregion businessfilters
            #endregion Datatable to pass the SP parameters to SearchResult.aspx
        }

        //private void buildCondition(ref string condition, RangeFilter.FilterStruct filter, bool useCrystalDB, string datePattern, bool caseSensitive)
        //{
        //    buildCondition(ref condition, filter, useCrystalDB, datePattern, caseSensitive, false);
        //}

        //private void buildCondition(ref string condition, RangeFilter.FilterStruct filter, bool useCrystalDB, string datePattern, bool caseSensitive, bool byPassComparator)
        //{
        //    if (!byPassComparator)
        //        #region comparison type
        //        switch (filter.Comparative)
        //        {
        //            case "contains":
        //                condition += " LIKE ''%''+{0}+''%''";
        //                break;
        //            case "startswith":
        //                condition += " LIKE {0}+''%''";
        //                break;
        //            case "endswith":
        //                condition += " LIKE ''%''+{0}";
        //                break;
        //            case "equals":
        //                condition += "={0}";
        //                break;
        //            case "different":
        //                condition += "<>{0}";
        //                break;
        //            case "lesser":
        //                condition += "<{0}";
        //                break;
        //            case "greater":
        //                condition += ">{0}";
        //                break;
        //            case "ILB":
        //                condition += "=NCHAR(0)";
        //                break;
        //            case "notILB":
        //                condition += "<>NCHAR(0)";
        //                break;
        //            case "empty":
        //                condition += " IS NULL";
        //                break;
        //            case "notempty":
        //                condition += " NOT IS NULL";
        //                break;
        //            case "inlist":
        //                condition += " IN ({0})";
        //                break;
        //        }
        //        #endregion
        //    #region value to search
        //    //eZ# 71445 - Start
        //    string value = filter.Comparative == "inlist" ? splitList(caseSensitive ? filter.Value : filter.Value.ToLower(), filter.Delimiter, filter.Type) : caseSensitive ? filter.Value : filter.Value.ToLower();
        //    //eZ# 71445 - End
        //    switch (filter.Type)
        //    {
        //        case RangeFilter.ValueType.String:
        //            condition = String.Format(condition, "N''" + value + "''");
        //            break;
        //        case RangeFilter.ValueType.StringNeverEmpty:
        //            condition = String.Format(condition, "N''" + value + "''");
        //            break;
        //        case RangeFilter.ValueType.Int:
        //            condition = String.Format(condition, value);
        //            break;
        //        case RangeFilter.ValueType.DateTime:
        //            condition = String.Format(condition, "N''" + DateTime.FromBinary((long)Convert.ToDouble(value)).ToString(datePattern) + "''");
        //            break;
        //        case RangeFilter.ValueType.Chunk:
        //            condition = String.Format(condition, "N''" + value + "''");
        //            break;
        //        case RangeFilter.ValueType.Undefined:
        //            condition = String.Format(condition, "N''" + value + "''");
        //            break;
        //    }
        //    #endregion
        //}

        private void BuildCondition(ref string condition, RangeFilter.FilterStruct filter, bool useCrystalDB, string datePattern, bool caseSensitive)
        {
            if (dbList.SelectedValue.Equals("0")) // "0" means PDB
            {
                PDBBuildCondition(ref condition, filter, useCrystalDB, datePattern, caseSensitive, false);
                //CrystalBuildCondition(ref condition, filter, useCrystalDB, datePattern, caseSensitive, false);
            }
            else
            {
                CrystalBuildCondition(ref condition, filter, useCrystalDB, datePattern, caseSensitive, false);
            }
        }
        private void CrystalBuildCondition(ref string condition, RangeFilter.FilterStruct filter, bool useCrystalDB, string datePattern, bool caseSensitive, bool byPassComparator)
        {
            if (!byPassComparator)
                #region comparison type
                switch (filter.Comparative)
                {
                    case "contains":
                        //condition += " LIKE ''%''+{0}+''%''";
                        condition = " LIKE ''%''+{0}+''%''"; //'LIKE ' || '''' || '%'|| '293121-B21'||'%' ||''''

                        //condition = " LIKE ' + '''' +   ";
                        break;
                    case "startswith":
                        //condition += " LIKE {0}+''%''";
                        condition = " LIKE {0}+''%''";
                        break;
                    case "endswith":
                        //condition += " LIKE ''%''+{0}";
                        condition = " LIKE ''%''+{0}";
                        break;
                    case "equals":
                        //condition += "={0}";
                        condition = "= {0}";
                        break;
                    case "different":
                        //condition = "<> {0}";
                        condition = "!= {0}"; // Different from is passed with just a single symbol, to avoid issues in SP.This is done to prevent an issue in SP.
                        break;
                    case "lesser":
                        condition = "< {0}";
                        break;
                    case "greater":
                        condition = "> {0}";
                        break;
                    case "ILB":
                        condition = "= NCHAR(0)";
                        break;
                    case "notILB":
                        condition = "<> NCHAR(0)";
                        break;
                    case "empty":
                        condition = " IS NULL";
                        break;
                    case "notempty":
                        condition = " NOT IS NULL";
                        break;
                    case "inlist":
                        condition = " IN ({0}) ";
                        break;
                }
                #endregion
            #region value to search
            //eZ# 71445 - Start
            //string value = filter.Comparative == "inlist" ? splitList(caseSensitive ? filter.Value : filter.Value.ToLower(), filter.Delimiter, filter.Type) : caseSensitive ? filter.Value : filter.Value.ToLower();
            string value = filter.Comparative == "inlist" ? splitList(caseSensitive ? filter.Value : filter.Value, filter.Delimiter, filter.Type) : caseSensitive ? filter.Value : filter.Value;
            //eZ# 71445 - End
            switch (filter.Type)
            {
                case RangeFilter.ValueType.String:
                    condition = String.Format(condition, "N''" + value + "''");
                    break;
                case RangeFilter.ValueType.StringNeverEmpty:
                    condition = String.Format(condition, "N''" + value + "''");
                    break;
                case RangeFilter.ValueType.Int:
                    condition = String.Format(condition, value);
                    break;
                case RangeFilter.ValueType.DateTime:
                    condition = String.Format(condition, "N''" + DateTime.FromBinary((long)Convert.ToDouble(value)).ToString(datePattern) + "''");
                    int index = condition.IndexOf("N");
                    string subStr = condition.Substring(0, index);
                    condition = condition.Replace(subStr, "");
                    condition = string.Format("{0}" + "{1}" + condition + "{2}", subStr, "CONVERT(DATETIME,", ")");
                    condition = condition.Replace("''", "'");
                    break;
                case RangeFilter.ValueType.Chunk:
                    condition = String.Format(condition, "N''" + value + "''");
                    break;
                case RangeFilter.ValueType.Undefined:
                    condition = String.Format(condition, "N''" + value + "''");
                    break;
            }
            #endregion
        }

        #region PDBBuildCondition
        /// <summary>
        /// PDB Parameter building function
        /// </summary>
        /// <param name="pls"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        /// 
        private void PDBBuildCondition(ref string condition, RangeFilter.FilterStruct filter, bool useCrystalDB, string datePattern, bool caseSensitive, bool byPassComparator)
        {
            if (!byPassComparator)
                #region comparison type
                switch (filter.Comparative)
                {
                    case "contains":
                        //condition = " 'LIKE ' || '''' || '%'||{0}||'%' ||'''' ";
                        condition = " LIKE '%{0}%' ";
                        
                        break;
                    case "startswith":
                        //condition = " 'LIKE' || '''' || {0} ||'%'|| '''' ";
                        condition = "LIKE '{0}%'";
                        break;
                    case "endswith":
                        condition = "LIKE '%{0}'";
                        break;
                    case "equals":
                        condition = "='{0}'";
                        break;
                    case "different":
                        condition = "<>'{0}'";
                        break;
                    case "lesser":
                        condition = "< '{0}'";
                        break;
                    case "greater":
                        //condition = " '> '|| {0} ";
                        condition = "> '{0}'";
                        break;
                    case "ILB":
                        condition = "= NCHAR(0)";
                        break;
                    case "notILB":
                        condition = "<> NCHAR(0)";
                        break;
                    case "empty":
                        condition = " IS NULL";
                        break;
                    case "notempty":
                        condition = " NOT IS NULL";
                        break;
                    case "inlist":
                        condition = " IN ('{0}')";
                        break;
                }
                #endregion
            #region value to search
            //eZ# 71445 - Start
            //string value = filter.Comparative == "inlist" ? SplitListPDB(caseSensitive ? filter.Value : filter.Value, filter.Delimiter, filter.Type) : caseSensitive ? filter.Value : filter.Value;
            string value = filter.Comparative == "inlist" ? splitList(caseSensitive ? filter.Value : filter.Value, filter.Delimiter, filter.Type) : caseSensitive ? filter.Value : filter.Value;
            //eZ# 71445 - End
            switch (filter.Type)
            {
                case RangeFilter.ValueType.String:
                    //condition = String.Format(condition, "'" + value + "'");
                    condition = String.Format(condition, value);
                    break;
                case RangeFilter.ValueType.StringNeverEmpty:
                    //condition = String.Format(condition, "'" + value + "'");
                    condition = String.Format(condition, value);
                    break;
                case RangeFilter.ValueType.Int:
                    condition = String.Format(condition, value);
                    condition = condition.Replace("'", "");
                    break;
                case RangeFilter.ValueType.DateTime:
                    // '>'||'to_date('||''''||'3/10/2009'||''''||','||''''||'mm/dd/yyyy'||''''||')'
                    //'to_date('4/15/2013','mm/dd/yyyy')'  
                    condition = String.Format(condition, DateTime.FromBinary((long)Convert.ToDouble(value)).ToString(datePattern));
                    // ='4/15/2013'
                    int index = condition.IndexOf("'");
                    string subStr = condition.Substring(0, index);
                    condition = condition.Replace(subStr, "");
                    condition = string.Format("{0}" + "{1}" + condition + "{2}", subStr, "to_date(", ",'mm/dd/yyyy')");
                    //condition = condition.Replace("N", "");
                    break;
                case RangeFilter.ValueType.Chunk:
                    condition = String.Format(condition, value);
                    break;
                case RangeFilter.ValueType.Undefined:
                    condition = String.Format(condition, value);
                    break;
            }
            #endregion
        }

        #endregion PDBBuildCondition

        private string CreateDelimitedStringList(HyperCatalog.Business.PLList pls, string delimiter)
        {
            string conditionStr = string.Empty;
            string returnValue = string.Empty;
            foreach (HyperCatalog.Business.PL item in pls)
            {
                //conditionStr += (conditionStr == String.Empty ? " AND (" : " OR ") + String.Format(pattern, item.Code);
                if (dbList.SelectedValue == "1")
                {
                    if (conditionStr == string.Empty)
                    {
                        conditionStr += "(";
                    }
                   conditionStr = conditionStr + "'" + item.Code + "'" + delimiter;
                }
                else
                {
                    if (conditionStr == string.Empty)
                    {
                        conditionStr += "IN (";
                    }
                    conditionStr = conditionStr + "'" + item.Code + "'" + delimiter;
                                        
                }
            }

            if (conditionStr != string.Empty)
            {
                returnValue = conditionStr.Remove(conditionStr.Length - 1, 1);
                returnValue += ")";
            }
            return returnValue;
        }

        private string CreateDelimitedStringListForCulture(CheckBoxList list, string delimiter, bool isCrystal)
        {
            string conditionStr = string.Empty;
            string returnValue = string.Empty;
            if (isCrystal)
            {
                foreach (ListItem item in list.Items)
                    if (item.Selected)
                    {
                        if (item.Value == string.Empty)
                        {
                            conditionStr = string.Empty;
                            break;
                        }
                        conditionStr = conditionStr + "'" + item.Value + "'" + delimiter;
                    }
                if (conditionStr != string.Empty)
                {
                    returnValue = conditionStr.Remove(conditionStr.Length - 1, 1);
                }
                return returnValue;
            }
            else
            {
                foreach (ListItem item in list.Items)
                {
                    if (item.Selected)
                    {
                        if (item.Value == string.Empty)
                        {
                            conditionStr = string.Empty;
                            break;
                        }
                        else
                            if (string.IsNullOrEmpty(conditionStr))
                            {
                                conditionStr = "'IN (";
                            }
                        //'in ('||''''||'ww-en'||''''||','||''''||'us-en'||''''||')'
                        conditionStr += "'||''''||'" + item.Value + "'||''''||'";
                        conditionStr += delimiter;
                    }
                }
                if (conditionStr != string.Empty)
                {
                    returnValue = conditionStr.Remove(conditionStr.Length - 1, 1);
                    returnValue += ")'";
                }
                return returnValue;
            }

        }

        private string CreateDelimitedStringList(CheckBoxList list, string delimiter)
        {
            string conditionStr = string.Empty;
            string returnValue = string.Empty;

            foreach (ListItem item in list.Items)
                if (item.Selected)
                {
                    if (item.Value == string.Empty)
                    {
                        conditionStr = string.Empty;
                        break;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(conditionStr))
                        {
                            if (dbList.SelectedValue == "0")
                            {
                                conditionStr = "IN (";
                            }
                            else
                            {
                                conditionStr = "(";
                            }
                        }
                    }
                    //conditionStr += "''" + item.Value + "''";
                    conditionStr += "'" + item.Value + "'";
                    conditionStr += delimiter;
                }
            if (conditionStr != string.Empty)
            {
                returnValue = conditionStr.Remove(conditionStr.Length - 1, 1);
                returnValue += ")";
            }
            return returnValue;
        }


        /* END PRISM UI TO PDB CHANGES */
        //////1174 start
        ////private string splitList(string value, string delimiter)
        ////{
        ////    string delimiterStr = delimiter.ToString();
        ////    if (delimiter == "CR")
        ////    {
        ////        return value == null || value == string.Empty ? "" : value.Trim().Replace("\r", ",").Replace("\n", "''N''");
        ////        //return value == null || value == string.Empty ? "" : value.Trim().Replace("\n", "'',N''").Replace("\r", string.Empty);
        ////    }
        ////    else
        ////    {
        ////        return value == null || value == string.Empty ? "" : value.Trim().Replace(delimiterStr, "'',N''");
        ////    }
        ////}
        /////// <summary>
        /////// SplitListfunction dulicated with modifications for PDB.
        /////// </summary>
        /////// <param name="value"></param>
        /////// <param name="delimiter"></param>
        /////// <param name="valueType"></param>
        /////// <returns></returns>

        ////private string SplitListPDB(string value, string delimiter, RangeFilter.ValueType valueType)
        ////{
        ////    string delimiterStr = delimiter.ToString().ToLower();
        ////    delimiterStr = ",";
        ////    //if (valueType == RangeFilter.ValueType.Int)
        ////    //{
        ////    //    return value.Trim().Replace(delimiterStr, "','");
        ////    //}
        ////    if (valueType == RangeFilter.ValueType.StringNeverEmpty || valueType == RangeFilter.ValueType.String)
        ////    {
        ////        return value == null || value == string.Empty ? "" : value.Trim().Replace(delimiterStr + " ", "','");
        ////    }
        ////    else
        ////    {
        ////        return value.Trim().Replace(delimiterStr, ",");
        ////    }
        ////}

        //////eZ# 71445 - Start
        ////// Method splitList is overloaded to handle the condition for ItemId in the list

        ////private string splitList(string value, string delimiter, RangeFilter.ValueType valueType)
        ////{
        ////    string delimiterStr = delimiter.ToString().ToLower();
        ////    delimiterStr = ",";
        ////    /****** I comment the below lines, since delimiter is set to comma and below lines become redundant By Rekha*******/

        ////    //if ((delimiterStr == "CR") || (delimiterStr == "cr") && (valueType == RangeFilter.ValueType.Int))
        ////    //{
        ////    //    //  return value.Trim().Replace(delimiterStr, ",");
        ////    //    return value == null || value == string.Empty ? "" : value.Trim().Replace("\r", ",").Replace("\n", string.Empty);

        ////    //}
        ////    //else if (((delimiterStr == "CR") || (delimiterStr == "cr")))
        ////    //{
        ////    //    //return value == null || value == string.Empty ? "" : value.Trim().Replace("\r", "',N'").Replace("\n", string.Empty);
        ////    //    return value == null || value == string.Empty ? "" : value.Trim().Replace(delimiterStr, "'',N''").Replace("\n", string.Empty);

        ////    //}
        ////    if (valueType == RangeFilter.ValueType.Int)
        ////    {
        ////        return value.Trim().Replace(delimiterStr, ",");
        ////    }

        ////    else if (valueType == RangeFilter.ValueType.StringNeverEmpty || valueType == RangeFilter.ValueType.String)
        ////    {
        ////        return value == null || value == string.Empty ? "" : value.Trim().Replace(delimiterStr, "'',N''");
        ////    }

        ////    else
        ////    {
        ////        // Eventhough the following code has nothing to do with eZ# 71445 I found out that 
        ////        // the CR delimiter is not working also everything fails when carriage returns are present
        ////        //if (delimiter == "CR")
        ////        //{
        ////        //    return value == null || value == string.Empty ? "" : value.Trim().Replace("\n", ",").Replace("\r", string.Empty);
        ////        //}
        ////        //else
        ////        //{
        ////        //    return value == null || value == string.Empty ? "" : value.Trim().Replace("\r\n", ",").Replace(delimiterStr, "'',N''");
        ////        //}
        ////        return value.Trim().Replace(delimiterStr, ",");
        ////    }
        ////}
        //////1174 end
        //////eZ# 71445 - End

        //1174 start
        private string splitList(string value, string delimiter)
        {
            string delimiterStr = delimiter.ToString();
            if (delimiter == "CR")
            {
                return value == null || value == string.Empty ? "" : value.Trim().Replace("\r", ",").Replace("\n", "''N''");
                //return value == null || value == string.Empty ? "" : value.Trim().Replace("\n", "'',N''").Replace("\r", string.Empty);
            }
            else
            {
                return value == null || value == string.Empty ? "" : value.Trim().Replace(delimiterStr, "'',N''");
            }
        }
        //eZ# 71445 - Start
        // Method splitList is overloaded to handle the condition for ItemId in the list

        private string splitList(string value, string delimiter, RangeFilter.ValueType valueType)
        {
            string delimiterStr = delimiter.ToString().ToLower();
            if ((delimiterStr == "CR") || (delimiterStr == "cr") && (valueType == RangeFilter.ValueType.Int))
            {
                //  return value.Trim().Replace(delimiterStr, ",");
                return value == null || value == string.Empty ? "" : value.Trim().Replace("\r", ",").Replace("\n", string.Empty);

            }
            else if (((delimiterStr == "CR") || (delimiterStr == "cr")))
            {
                if(dbList.SelectedValue.Equals("1"))
                {
                return value == null || value == string.Empty ? "" : value.Trim().Replace("\r", "',N'").Replace("\n", string.Empty);
                }
                else
                {
                    return value == null || value == string.Empty ? "" : value.Trim().Replace("\r", "','").Replace("\n", string.Empty);
                }

                //return value == null || value == string.Empty ? "" : value.Trim().Replace("\n", ",").Replace("\r", string.Empty);
            }
            else if (valueType == RangeFilter.ValueType.Int)
            {
                return value.Trim().Replace(delimiterStr, ",");
            }


            else if (valueType == RangeFilter.ValueType.StringNeverEmpty || valueType == RangeFilter.ValueType.String)
            {

                if (dbList.SelectedValue.Equals("0")) //PDB
                {
                    //return value == null || value == string.Empty ? "" : value.Trim().Replace(delimiterStr + " ", "','");
                    return value == null || value == string.Empty ? "" : value.Trim().Replace(delimiterStr, "','");
                }
                else // live DB
                {
                    return value == null || value == string.Empty ? "" : value.Trim().Replace(delimiterStr, "'',N''");
                }
                
            }

            else
            {
                // Eventhough the following code has nothing to do with eZ# 71445 I found out that 
                // the CR delimiter is not working also everything fails when carriage returns are present
                //if (delimiter == "CR")
                //{
                //    return value == null || value == string.Empty ? "" : value.Trim().Replace("\n", ",").Replace("\r", string.Empty);
                //}
                //else
                //{
                //    return value == null || value == string.Empty ? "" : value.Trim().Replace("\r\n", ",").Replace(delimiterStr, "'',N''");
                //}
                return value.Trim().Replace(delimiterStr, ",");
            }
        }
        //1174 end
        //eZ# 71445 - End
        private string createSQLCondition(CheckBoxList list, string pattern)
        {
            string conditionStr = string.Empty;
            foreach (ListItem item in list.Items)
                if (item.Selected)
                {
                    if (item.Value == string.Empty)
                    {
                        conditionStr = string.Empty;
                        break;
                    }
                    else
                        conditionStr += (conditionStr == String.Empty ? " AND (" : " OR ") + String.Format(pattern, item.Value);
                }
            if (conditionStr != string.Empty)
                conditionStr += ")";
            return conditionStr;
        }
        private string createSQLCondition(HyperCatalog.Business.PLList pls, string pattern)
        {
            string conditionStr = string.Empty;
            foreach (HyperCatalog.Business.PL item in pls)
                conditionStr += (conditionStr == String.Empty ? " AND (" : " OR ") + String.Format(pattern, item.Code);
            if (conditionStr != string.Empty)
                conditionStr += ")";
            return conditionStr;
        }
        private string createCultureSQLCondition(CheckBoxList list, string pattern)
        {
            string conditionStr = string.Empty;
            foreach (ListItem item in list.Items)
                if (item.Selected)
                {
                    if (item.Value == string.Empty)
                    {
                        conditionStr = string.Empty;
                        break;
                    }
                    else
                        conditionStr += (conditionStr == String.Empty ? " AND (" : " OR ") + String.Format(pattern, item.Value);
                }
            if (conditionStr != string.Empty)
                conditionStr += ")";
            return conditionStr;
        }
        #region BindLists
        private void BindLists()
        {
            Bind_itemClasses();
            Bind_itemLevels();
            Bind_productLines();
            Bind_containerStatuses();
            Bind_containerTypes();
            Bind_regionsCountries();
            Bind_itemStatuses();
            Bind_itemTypes();
        }
        private void Bind_itemClasses()
        {
            //using (HyperCatalog.Business.CollectionView itemList = new HyperCatalog.Business.CollectionView(Business.Item.GetAll(" LevelId = 1 ")))
            //Added by  Venkata 10/05/16
            using (HyperCatalog.Business.CollectionView itemList = new HyperCatalog.Business.CollectionView(GetAllItems(" LevelId = 1 ", string.Empty, SessionState.CompanyName)))
            {
                itemList.Sort("Name");
                itemClassList.DataSource = itemList;
                itemClassList.DataBind();
            }
            //Venkata
        }
        private void Bind_itemLevels()
        {
            itemLevelList.DataSource = Business.ItemLevel.GetAll();
            itemLevelList.DataBind();
        }
        private void Bind_productLines()
        {
            //plList.DataSource = Business.PL.GetAll("IsActive = 1");
            //plList.DataBind();
        }
        private void Bind_containerStatuses()
        {
            containerStatusList.Items.Clear();
            foreach (Business.ChunkStatus status in Enum.GetValues(typeof(Business.ChunkStatus)))
            {
                if (status.ToString() == "Missing")
                {
                    if (dbList.SelectedValue.Equals("0"))// 0 means Datawarehouse
                        containerStatusList.Items.Add(new ListItem(status.ToString(), Business.Chunk.GetStatusFromEnum(status)));
                }
                else
                    containerStatusList.Items.Add(new ListItem(status.ToString(), Business.Chunk.GetStatusFromEnum(status)));
            }

            if (dbList.SelectedValue.Equals("0"))
                containerStatusList.Items.Add(new ListItem("Mandatory Containers", "Mandatory Containers"));
        }
        private void Bind_containerTypes()
        {
            containerTypeList.DataSource = Business.ContainerType.GetAll();
            containerTypeList.DataBind();
        }
        private void Bind_regionsCountries()
        {
            regionCountryList.DataSource = cultures;
            regionCountryList.DataBind();
            regionCountryList.SelectedValue = SessionState.MasterCulture.Code;
            regionCountryAnchor.InnerText = regionCountryAnchor.InnerText.Replace("No filter", "Filtered");
            regionCountryAnchor.Title = SessionState.MasterCulture.Code;
        }
        private void Bind_itemStatuses()
        {
            itemStatusList.Items.Clear();
            foreach (Business.ItemStatus status in Enum.GetValues(typeof(Business.ItemStatus)))
                itemStatusList.Items.Add(new ListItem(status.ToString(), Business.Item.GetStatusFromEnum(status)));
        }
        private void Bind_itemTypes()
        {
            itemTypeList.DataSource = HyperCatalog.Business.ItemType.GetAll();
            itemTypeList.DataBind();
        }
        # endregion

        //region start - Added by Venkata 10/05/16
        public ItemList GetAllItems(string sqlFilter, string sortOrder, string companyName)
        {
            Item.CleanError();
            if (sqlFilter == null)
                throw (new ArgumentNullException("sqlFilter", "SQLDataAccessLayer: GetAllItems"));
            if (sortOrder == null)
                throw (new ArgumentNullException("sortOrder", "SQLDataAccessLayer: GetAllItems"));

            Debug.Trace("[DL] Enter GetAllItems", DebugSeverity.Low);
            using (dbObj = Utils.GetMainDB())
            {
                using (IDataReader rs = dbObj.RunSPReturnRS("dbo._Item_GetAll",
                  new SqlParameter("@Filter", sqlFilter),
                  new SqlParameter("@SortOrder", sortOrder),
                  new SqlParameter("@Company", companyName)
                  ))
                {
                    if (dbObj.LastError != null && dbObj.LastError.Length == 0)
                    {
                        HyperCatalog.DataAccessLayer.SqlDataAccessLayer.GenerateCollectionFromReader col = new HyperCatalog.DataAccessLayer.SqlDataAccessLayer.GenerateCollectionFromReader(GenerateItemCollectionFromReader);
                        ItemList objCol = (ItemList)col(rs);
                        rs.Close();
                        Debug.Trace("[DL] Exit GetAllItems", DebugSeverity.Low);
                        return objCol;
                    }
                    Debug.Trace("[DL] Exit GetAllItems [ERROR]", DebugSeverity.Low);
                    return null;
                }
            }
        }

        private CollectionBase GenerateItemCollectionFromReader(IDataReader rs)
        {
            ItemList col = new ItemList();
            while (rs.Read())
            {
                Item obj = new Item(
                  Convert.ToInt64(rs["ItemId"]),
                  Convert.ToInt64(NullToEmpty(rs["ParentId"], "-1")),
                  Convert.ToInt32(rs["LevelId"]),
                    //PCF1--DIT
                    //Convert.ToBoolean(rs["IsProject"]),
                  Convert.ToBoolean(rs["IsDeal"]),
                  rs["ItemName"].ToString().Trim(),
                  rs["Sku"].ToString().Trim(),
                  Convert.ToDouble(rs["Sort"]),
                  Item.GetStatusFromString(rs["Status"].ToString().Trim()),
                  rs["Icon"].ToString().Trim(),
                  Convert.ToInt32(rs["ItemTypeId"]),
                  Convert.ToInt32(rs["CreatorId"]),
                  GetProperDate(rs["CreateDate"]),
                  rs["ModifierId"] != DBNull.Value ? Convert.ToInt32(rs["ModifierId"]) : -1,
                  GetProperDate(rs["ModifyDate"]),
                  Convert.ToBoolean(rs["IsRoll"]),
                  rs["RollId"] != DBNull.Value ? Convert.ToInt32(rs["RollId"]) : -1,
                  Convert.ToBoolean(rs["IsCrossSell"]),
                  rs["RefItemId"] != DBNull.Value ? Convert.ToInt64(rs["RefItemId"]) : -1,
                    //PCF1--DIT
                    //rs["TranslationMode"].ToString().Trim().ToLower() == "p" ? TRClassTranslationMode.PL : TRClassTranslationMode.Standard,
                  rs["HashNodeOID"] != DBNull.Value ? rs["HashNodeOID"].ToString().Trim() : null,
                  rs["NodeOID"] != DBNull.Value ? Convert.ToInt64(rs["NodeOID"]) : -1,
                  rs["PLCode"] != DBNull.Value ? rs["PLCode"].ToString().Trim() : null,
                  GetProperDate(rs["PMDeleted"]),
                  Convert.ToBoolean(rs["IsInitialized"]),
                  rs["CountrySpecific"].ToString().Trim()
                  );
                col.Add(obj);
            }
            return (col);
        }

        private static DateTime? GetProperDate(object d)
        {
            try
            {
                if (d == DBNull.Value)
                {
                    return null;
                }
                else
                {
                    return new DateTime?((DateTime)d);
                }
            }
            catch
            {
                return null;
            }
        }

        private string NullToEmpty(object v, string defaultResult)
        {
            if (v == null || v == DBNull.Value)
            {
                return defaultResult;
            }
            return v.ToString();
        }
        //region start - Added by Venkata 

        private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
        {
            //      foreach(object b in uwToolbar.Items)
            //        if(b is Infragistics.WebUI.UltraWebToolbar.TBarButton)
            //          ((Infragistics.WebUI.UltraWebToolbar.TBarButton)b).Enabled = true;
            switch (be.Button.Key.ToLower())
            {
                case "search":
                    newFilter.AddFilterBtn_Click(null, null);
                    message.Visible = false;
                    string columns = "";
                    foreach (ListItem item in fieldFilter.Items)
                        if (item.Selected)
                            columns += "," + item.Value;
                    if (columns.Length > 0)
                        columns = columns.Substring(1, columns.Length - 1);

                    bool mandatory;
                    /********* PRISM UI TO PDB CHANGES BY REKHA THOMAS*/
                    CreateSQLQueries(out mandatory);
                    /********* END PRISM UI TO PDB CHANGES BY REKHA THOMAS*/
                    Session["Mandatory"] = mandatory;
                    /************Start 3.5 Release for exporting *******************/
                    //ListItemCollection lstItemCol = new ListItemCollection();
                    DataTable objDTBussinessFilters = new DataTable();
                    objDTBussinessFilters.Columns.Add(new DataColumn("FilterName"));
                    objDTBussinessFilters.Columns.Add(new DataColumn("FilterValue"));
                    string PName = string.Empty;
                    string selOperator = string.Empty;
                    DataTable objDTCustomFilters = new DataTable();
                    objDTCustomFilters.Columns.Add(new DataColumn("ParameterName"));
                    objDTCustomFilters.Columns.Add(new DataColumn("Operator"));
                    objDTCustomFilters.Columns.Add(new DataColumn("FilterText"));
                    objDTCustomFilters.Columns.Add(new DataColumn("FilterValue"));
                    DataRow objDR = null;
                    foreach (RepeaterItem paramItem in paramList.Items)
                    {
                        PName = ((Label)paramItem.FindControl("parameterName")).Text + "";
                        DropDownList op = (DropDownList)paramItem.FindControl("operator");
                        Repeater filterList = (Repeater)paramItem.FindControl("filterList2");
                        foreach (RepeaterItem item in filterList.Items)
                        {
                            objDR = objDTCustomFilters.NewRow();
                            objDR["ParameterName"] = PName;
                            if (op != null && op.Visible == true)
                            {
                                objDR["Operator"] = "[" + op.SelectedItem.Text + "]";

                            }
                            else
                            {
                                objDR["Operator"] = string.Empty;
                            }
                            RangeFilter.FilterStruct filter = (RangeFilter.FilterStruct)((ArrayList)filters2[paramItem.ItemIndex])[item.ItemIndex];
                            objDR["FilterText"] = filter.Comparative + "";
                            if (filter.Type == RangeFilter.ValueType.DateTime)
                                objDR["FilterValue"] = DateTime.FromBinary((long)Convert.ToDouble(filter.Value)).ToShortDateString();
                            else
                                objDR["FilterValue"] = filter.Value + "";
                            objDTCustomFilters.Rows.Add(objDR);
                        }
                    }
                    objDTCustomFilters.AcceptChanges();
                    SelectedBussinessFilters(itemClassList, "Product Type", objDTBussinessFilters);
                    SelectedBussinessFilters(itemLevelList, "Hierarchy level", objDTBussinessFilters);
                    SelectedBussinessFilters(plList, "Product line", objDTBussinessFilters);
                    SelectedBussinessFilters(containerStatusList, "Content status", objDTBussinessFilters);
                    SelectedBussinessFilters(containerTypeList, "Container type", objDTBussinessFilters);
                    SelectedBussinessFilters(regionCountryList, "Catalogs", objDTBussinessFilters);
                    SelectedBussinessFilters(itemStatusList, "Node status", objDTBussinessFilters);
                    SelectedBussinessFilters(itemTypeList, "Item type", objDTBussinessFilters);
                    SelectedBussinessFilters(new CheckBox[] { prodWOcompat, prodWOPL, prodExpToProv }, "Product...", objDTBussinessFilters);
                    Session["selectedCustomFilters"] = objDTCustomFilters.Rows.Count == 0 ? null : objDTCustomFilters;
                    Session["selectedBusinessFilters"] = objDTBussinessFilters.Rows.Count == 0 ? null : objDTBussinessFilters;
                    Session["SearchType"] = "Custom Search";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "popup", "<script>OpenModalWindow('./SearchResult.aspx?cdb=" + dbList.SelectedValue + "&cols=" + columns + "', 'dosearch', 500, 800, 'yes', 1);</script>");
                    /************End 3.5 Release for exporting *******************/
                    break;
            }
        }
        /************Start 3.5 Release for exporting *******************/
        private void SelectedBussinessFilters(CheckBoxList list, string filterName, DataTable ObjDTBussFilters)
        {
            string conditionStr = string.Empty;

            foreach (ListItem item in list.Items)
                if (item.Selected)
                {
                    if (item.Value != string.Empty)
                    {
                        conditionStr = conditionStr + item.Text + ",";
                    }
                }
            if (conditionStr != string.Empty)
            {
                conditionStr = conditionStr.Remove(conditionStr.Length - 1, 1);
                //lstItemCol.Add(new ListItem(filterName, conditionStr));
                DataRow objDR = ObjDTBussFilters.NewRow();
                objDR[0] = filterName;
                objDR[1] = conditionStr;
                ObjDTBussFilters.Rows.Add(objDR);
                ObjDTBussFilters.AcceptChanges();

            }
        }
        private void SelectedBussinessFilters(CheckBox[] list, string filterName, DataTable ObjDTBussFilters)
        {
            string conditionStr = string.Empty;
            foreach (CheckBox item in list)
                if (item.Checked)
                {
                    if (item.Text != string.Empty)
                    {
                        conditionStr = conditionStr + item.Text + ",";
                    }
                }
            if (conditionStr != string.Empty)
            {
                conditionStr = conditionStr.Remove(conditionStr.Length - 1, 1);
                //lstItemCol.Add(new ListItem(filterName, conditionStr));
                DataRow objDR = ObjDTBussFilters.NewRow();
                objDR[0] = filterName;
                objDR[1] = conditionStr;
                ObjDTBussFilters.Rows.Add(objDR);
                ObjDTBussFilters.AcceptChanges();
            }

        }
        private void SelectedBussinessFilters(PLWebTree control, string filterName, DataTable ObjDTBussFilters)
        {
            string conditionStr = string.Empty;
            foreach (HyperCatalog.Business.PL item in control.GetCheckedPLs())
                conditionStr = conditionStr + item.Code + ",";
            if (conditionStr != string.Empty)
            {
                conditionStr = conditionStr.Remove(conditionStr.Length - 1, 1);
                //lstItemCol.Add(new ListItem(filterName, conditionStr));
                DataRow objDR = ObjDTBussFilters.NewRow();
                objDR[0] = filterName;
                objDR[1] = conditionStr;
                ObjDTBussFilters.Rows.Add(objDR);
                ObjDTBussFilters.AcceptChanges();
            }
        }
        /************End 3.5 Release for exporting *******************/
        private void paramList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            foreach (ListItem lst in lstCol)
            {
                if (lst.Text == ((Label)e.Item.FindControl("parameterName")).Text)
                {
                    ((DropDownList)e.Item.FindControl("operator")).SelectedIndex = int.Parse(lst.Value);
                }
            }
            //RequiredFieldValidator val = (RequiredFieldValidator)e.Item.FindControl("operatorRequiredFieldValidator");
            //DropDownList dl = (DropDownList)e.Item.FindControl("operator");
            //val.ControlToValidate = dl.ID;
        }
        private void paramList_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            Repeater _filterList2 = (Repeater)e.Item.FindControl("filterList2");
            _filterList2.ItemCreated += new RepeaterItemEventHandler(_filterList2_ItemCreated);
        }
        private void _filterList2_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            RangeFilter _filter = (RangeFilter)e.Item.FindControl("filter2");
            _filter.DataSource = filterDataSource;
            _filter.FilterRemoved += new HyperCatalog.UI.RangeFilter.RemoveFilter(newFilter_FilterRemoved);
            _filter.FilterChanged += new HyperCatalog.UI.RangeFilter.ChangeFilter(newFilter_FilterChanged);
        }
        public void newFilter_FilterAdded(object sender, RangeFilter.FilterStruct filter)
        {
            string filterMessage = filter.Check();
            if (filterMessage != null)
            {
                message.Text = filterMessage;
                message.Visible = true;
            }
            else
            {
                if (!filterNames.Contains(filter.Name))
                {
                    filterNames.Add(filter.Name);
                    filters2.Add(new ArrayList());
                }
                int index = filterNames.IndexOf(filter.Name);
                if (index >= 0)
                {
                    ((ArrayList)filters2[index]).Add(filter);
                    if (filter.Name == "Container group")
                    {
                        Flag = Flag + 1;
                    }
                }
            }
            //int parameterIndex = ((RepeaterItem)((Control)sender).Parent.Parent.Parent).ItemIndex; //testing
        }
        public void newFilter_FilterRemoved(object sender)
        {
            if (Flag > 0 && filterNames.Contains("Container group"))
            {
                Flag = Flag - 1;
            }

            int parameterIndex = ((RepeaterItem)((Control)sender).Parent.Parent.Parent).ItemIndex;
            int filterIndex = ((RepeaterItem)((Control)sender).Parent).ItemIndex;
            ((ArrayList)filters2[parameterIndex]).RemoveAt(filterIndex);
            
            if (((ArrayList)filters2[parameterIndex]).Count == 0)
            {
                filterNames.RemoveAt(parameterIndex);
                filters2.RemoveAt(parameterIndex);
            }
            //check if count ==1, if one then opstr drop down needs to be reset.
            //if (((ArrayList)filters2[parameterIndex]).Count == 1)
            //{
            //    Flag = 1;
            //}
           
        }
        private void newFilter_FilterChanged(object sender, HyperCatalog.UI.RangeFilter.FilterStruct filter)
        {
            string filterMessage = filter.Check();
            if (filterMessage != null)
            {
                message.Text = filterMessage;
                message.Visible = true;
            }
            else
            {
                int parameterIndex = ((RepeaterItem)((Control)sender).Parent.Parent.Parent).ItemIndex;
                int filterIndex = ((RepeaterItem)((Control)sender).Parent).ItemIndex;
                ((ArrayList)filters2[parameterIndex])[filterIndex] = filter;
            }
        }


        //<Summary>
        //OnSelectedIndexChanged Event in dbList to 
        //refresh the "Content Status"
        //</Summary>
        public void dbList_SelectedIndexChange(Object sender, EventArgs e)
        {
            Bind_containerStatuses();
        }

    }
}


