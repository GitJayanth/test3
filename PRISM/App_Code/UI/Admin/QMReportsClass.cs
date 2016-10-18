using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Data.OleDb;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Infragistics.WebUI.UltraWebGrid;
using HyperCatalog.Shared;
using HyperComponents.Data.dbAccess;
using HyperCatalog.Business;
using HyperCatalog.UI.Tools;
/// <summary>
/// Summary description for QMReportsClass
/// </summary>
public class QMReportsClass
{
    public QMReportsClass()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    // The following code Consolidates data got from StoredProcedures so that the data can 
    // be displayed in Ultrawebgrid. 
    // This method is called by both the QM_SingleContainerReport.aspx.cs and 
    // QM_MultiContainerReport.aspx.cs
    #region 71517
    //Modified by: Ramachandran
    //The same piece of code is modified in GetInputFormData
    //Date: 06/11/2007
    #endregion
    public DataTable GetConsolidatedData(DataSet dsContainers, string regionCode)
    {
        string tempstr = regionCode.Replace("','", ",");
        string[] rCodes = tempstr.Split(',');
        DataRow drNewRow = null;
        //71517: the UI is not in synch with the export
        string[] objRegions = new string[rCodes.Length];
        for (int i = 0; i < rCodes.Length; i++)
        {
            if (dsContainers.Tables[0].Select("CultureCode='" + rCodes[i].ToLower() + "-en'").Length == 0)
            {
                drNewRow = dsContainers.Tables[0].NewRow();
                drNewRow[0] = dsContainers.Tables[0].Rows[0][0];
                drNewRow[1] = dsContainers.Tables[0].Rows[0][1];
                drNewRow[2] = dsContainers.Tables[0].Rows[0][2];
                drNewRow[3] = 0;
                drNewRow[4] = rCodes[i].ToLower() + "-en";
                dsContainers.Tables[0].Rows.Add(drNewRow);
            }
            // 71517: the issue is solved by making the UI read the Regions in the same order as in the export
            objRegions.SetValue(rCodes[i].ToLower() + "-en", i);
        }
        dsContainers.AcceptChanges();
        object[] objContainerIds = GetDistinctValues(dsContainers.Tables[0].Select(), "ContainerId");

        DataTable objDT = new DataTable();
        objDT.Columns.Add(new DataColumn("ContainerId"));
        objDT.Columns.Add(new DataColumn("ContainerName"));
        //string[] strColumns = new string[] { "F", "FILB", "D", "DILB", "M/R", "M/RILB" };
        string[] strColumns = new string[] { "F", "FILB", "D", "M/R" };
        //71517: The Below Code is commented b'coz it was giving random order of regions 
        //which was not in synch with the export
        //object[] objRegions = GetDistinctValues(dsContainers.Tables[0].Select(), "CultureCode");
        object[] objRegionColumns = new object[strColumns.Length * objRegions.Length];
        int regIndex = 0;
        for (int strIndex = 0; strIndex < strColumns.Length; strIndex++)
        {
            objDT.Columns.Add(new DataColumn(strColumns[strIndex] + "Per"));
            for (int index = 0; index < objRegions.Length; index++)
            {
                objRegionColumns[regIndex] = strColumns[strIndex] + "~" + objRegions[index];
                objDT.Columns.Add(new DataColumn(objRegionColumns[regIndex].ToString()));
                regIndex++;
            }
        }

        DataRow[] drCurrent = null;
        for (int index = 0; index < objContainerIds.Length; index++)
        {
            DataRow dr = objDT.NewRow();
            dr["ContainerId"] = objContainerIds[index].ToString();

            drCurrent = dsContainers.Tables[0].Select("ContainerId ='" + objContainerIds[index].ToString().Trim() + "'");
            dr["ContainerName"] = drCurrent[0]["ContainerName"];
            float FinalTotal = float.Parse(Convert.ToString(dsContainers.Tables[0].Compute("Sum(ChunkCount)", "ContainerId ='" + objContainerIds[index].ToString().Trim() + "' and ChunkStatus='F'")) + "0") / 10;
            float FinalILB = float.Parse(Convert.ToString(dsContainers.Tables[0].Compute("Sum(ChunkCount)", "ContainerId ='" + objContainerIds[index].ToString().Trim() + "' and ChunkStatus='FILB'")) + "0") / 10;
            float RealFinalTot = FinalTotal - FinalILB;

            float DraftTotal = float.Parse(Convert.ToString(dsContainers.Tables[0].Compute("Sum(ChunkCount)", "ContainerId ='" + objContainerIds[index].ToString().Trim() + "' and ChunkStatus='D'")) + "0") / 10;
            //float DraftILB = float.Parse(Convert.ToString(dsContainers.Tables[0].Compute("Sum(ChunkCount)", "ContainerId ='" + objContainerIds[index].ToString().Trim() + "' and ChunkStatus='DILB'")) + "0") / 10;
            //float RealDraftTot = DraftTotal - DraftILB;

            float MisRejTotal = float.Parse(Convert.ToString(dsContainers.Tables[0].Compute("Sum(ChunkCount)", "ContainerId ='" + objContainerIds[index].ToString().Trim() + "' and (ChunkStatus='M' or ChunkStatus='R')")) + "0") / 10;
            //float MisRejILB = float.Parse(Convert.ToString(dsContainers.Tables[0].Compute("Sum(ChunkCount)", "ContainerId ='" + objContainerIds[index].ToString().Trim() + "' and (ChunkStatus='MILB' or ChunkStatus='RILB')")) + "0") / 10;
            //float RealMisRejTot = MisRejTotal - MisRejILB;

            float total = FinalTotal + DraftTotal + MisRejTotal;

            if (total != 0)
            {
                float value = (FinalTotal * 100) / total;
                dr["FPer"] = value.ToString("F2");
            }
            else
                dr["FPer"] = (FinalTotal * 100);

            if (FinalTotal != 0)
            {
                float value = (FinalILB * 100) / FinalTotal;
                dr["FILBPer"] = value.ToString("F2");
            }
            else
                dr["FILBPer"] = (FinalILB * 100);

            if (total != 0)
            {
                float value = (DraftTotal * 100) / total;
                dr["DPer"] = value.ToString("F2");
            }
            else
                dr["DPer"] = (DraftTotal * 100);

            //if (DraftTotal != 0)
            //{
            //    float value = (DraftILB * 100) / DraftTotal;
            //    dr["DILBPer"] = value.ToString("F2");
            //}
            //else
            //    dr["DILBPer"] = (DraftILB * 100);


            if (total != 0)
            {
                float value = (MisRejTotal * 100) / total;
                dr["M/RPer"] = value.ToString("F2");
            }
            else
                dr["M/RPer"] = (MisRejTotal * 100);

            //if (MisRejTotal != 0)
            //{
            //    float value = (MisRejILB * 100) / MisRejTotal;
            //    dr["M/RILBPer"] = value.ToString("F2");
            //}
            //else
            //    dr["M/RILBPer"] = (MisRejILB * 100);


            string[] strArr = null;
            int Count1 = 0;
            int Count2 = 0;
            for (int cur = 0; cur < objRegionColumns.Length; cur++)
            {
                strArr = objRegionColumns[cur].ToString().Split('~');
                //if (strArr[0] == "M/R" || strArr[0] == "M/RILB")
                //{
                if (strArr[0] == "M/R")
                {
                    Count1 = int.Parse(Convert.ToString(dsContainers.Tables[0].Compute("Sum(ChunkCount)", "ContainerId ='" + objContainerIds[index].ToString().Trim() + "' and (ChunkStatus='M' or ChunkStatus='R') and CultureCode='" + strArr[1] + "'")) + "0") / 10;
                    // Count2 = int.Parse(Convert.ToString(dsContainers.Tables[0].Compute("Sum(ChunkCount)", "ContainerId ='" + objContainerIds[index].ToString().Trim() + "' and (ChunkStatus='MILB' or ChunkStatus='RILB') and CultureCode='" + strArr[1] + "'"))+"0")/10;
                }
                //else
                //{
                //    Count1 = int.Parse(Convert.ToString(dsContainers.Tables[0].Compute("Sum(ChunkCount)", "ContainerId ='" + objContainerIds[index].ToString().Trim() + "' and (ChunkStatus='MILB' or ChunkStatus='RILB') and CultureCode='" + strArr[1] + "'"))+"0")/10;
                //   // Count2 = 0;
                //}
                //}
                else
                {
                    Count1 = int.Parse(Convert.ToString(dsContainers.Tables[0].Compute("Sum(ChunkCount)", "ContainerId ='" + objContainerIds[index].ToString().Trim() + "' and ChunkStatus='" + strArr[0] + "' and CultureCode='" + strArr[1] + "'")) + "0") / 10;
                    Count2 = int.Parse(Convert.ToString(dsContainers.Tables[0].Compute("Sum(ChunkCount)", "ContainerId ='" + objContainerIds[index].ToString().Trim() + "' and ChunkStatus='" + strArr[0] + "ILB' and CultureCode='" + strArr[1] + "'")) + "0") / 10;
                }
                //int FinalChCount = int.Parse(dsContainers.Tables[0].Select("ContainerId ='" + objContainerIds[index].ToString().Trim() + "' and ChunkStatus='F'  + [1].ToString() + "'")[0]["ChunkCount"] + "" + "0") / 10;
                dr[objRegionColumns[cur].ToString()] = Count1 - Count2;
                //dr[objRegionColumns[cur].ToString()] = Count1;
            }


            objDT.Rows.Add(dr);
        }

        objDT.AcceptChanges();
        objDT.Columns.RemoveAt(0);
        objDT.AcceptChanges();
        return sortDataTable(objDT,"ContainerName");
    }

    public object[] GetDistinctValues(DataRow[] dRowCol, string colName)
    {
        Hashtable hTable = new Hashtable();
        foreach (DataRow drow in dRowCol)
            if (!hTable.ContainsKey(drow[colName]))
                hTable.Add(drow[colName], string.Empty);
        object[] objArray = new object[hTable.Keys.Count];
        hTable.Keys.CopyTo(objArray, 0);
        return objArray;
    }

    // The following code Consolidates data got from StoredProcedures so that the data can 
    // be displayed in Ultrawebgrid. 
    // This method is called by QM_InputForms.aspx.cs
    public DataTable GetInputFormData(DataSet dsContainers, string BizCode, string Code, string RegionCode)
    {
        string tempstr = RegionCode.Replace("','", ",");
        string[] rCodes = tempstr.Split(',');
        DataRow drNewRow = null;
        string[] objRegions = new string[rCodes.Length];
        for (int i = 0; i < rCodes.Length; i++)
        {
            if (dsContainers.Tables[0].Select("CultureCode='" + rCodes[i].ToLower() + "-en'").Length == 0)
            {
                drNewRow = dsContainers.Tables[0].NewRow();
                drNewRow["InputFormId"] = dsContainers.Tables[0].Rows[0]["InputFormId"];
                drNewRow["InputFormName"] = dsContainers.Tables[0].Rows[0]["InputFormName"];
                drNewRow["Status"] = dsContainers.Tables[0].Rows[0]["Status"];
                drNewRow["ChunkStatus"] = dsContainers.Tables[0].Rows[0]["ChunkStatus"];
                drNewRow["ChunkCount"] = 0;
                drNewRow["CultureCode"] = rCodes[i].ToLower() + "-en";
                dsContainers.Tables[0].Rows.Add(drNewRow);
            }
            objRegions.SetValue(rCodes[i].ToLower() + "-en", i);
        }
        dsContainers.AcceptChanges();

        object[] objContainerIds = GetDistinctValues(dsContainers.Tables[0].Select(), "InputFormId");
        string[] status = new string[] { "L", "F", "O" };

        DataTable objDT = new DataTable();
        objDT.Columns.Add(new DataColumn("InputFormId"));
        objDT.Columns.Add(new DataColumn("InputFormName"));
        string[] strColumns = new string[] { "F", "FILB", "D", "M/R" };
        //object[] objRegions = GetDistinctValues(dsContainers.Tables[0].Select(), "CultureCode");
        object[] objRegionColumns = new object[strColumns.Length * objRegions.Length * 3];
        int regIndex = 0;
        for (int statusInd = 0; statusInd < status.Length; statusInd++)
        {
            for (int strIndex = 0; strIndex < strColumns.Length; strIndex++)
            {
                objDT.Columns.Add(new DataColumn(status[statusInd] + ":" + strColumns[strIndex] + "Per"));
                for (int index = 0; index < objRegions.Length; index++)
                {
                    objRegionColumns[regIndex] = status[statusInd] + ":" + strColumns[strIndex] + "~" + objRegions[index];
                    objDT.Columns.Add(new DataColumn(objRegionColumns[regIndex].ToString()));
                    regIndex++;
                }
            }
        }

        DataRow[] drCurrent = null;
        for (int index = 0; index < objContainerIds.Length; index++)
        {
            DataRow dr = objDT.NewRow();
            dr["InputFormId"] = objContainerIds[index].ToString();

            drCurrent = dsContainers.Tables[0].Select("InputFormId ='" + objContainerIds[index].ToString().Trim() + "'");
            //dr["InputFormName"] = drCurrent[0]["InputFormName"];
            dr["InputFormName"] = "<a href=QM_MultiContainerReport.aspx?FormId=" + objContainerIds[index].ToString() + "&BizCode=" + BizCode + "&Code=" + Code + "&regionCode=" + RegionCode + ">" + drCurrent[0]["InputFormName"] + "</a>";

            for (int statusInd = 0; statusInd < status.Length; statusInd++)
            {
                float FinalTotal = float.Parse(Convert.ToString(dsContainers.Tables[0].Compute("Sum(ChunkCount)", "InputFormId ='" + objContainerIds[index].ToString().Trim() + "' and ChunkStatus='F' and Status='" + status[statusInd] + "'")) + "0") / 10;
                float FinalILB = float.Parse(Convert.ToString(dsContainers.Tables[0].Compute("Sum(ChunkCount)", "InputFormId ='" + objContainerIds[index].ToString().Trim() + "' and ChunkStatus='FILB' and Status='" + status[statusInd] + "'")) + "0") / 10;
                float RealFinalTot = FinalTotal - FinalILB;

                float DraftTotal = float.Parse(Convert.ToString(dsContainers.Tables[0].Compute("Sum(ChunkCount)", "InputFormId ='" + objContainerIds[index].ToString().Trim() + "' and ChunkStatus='D' and Status='" + status[statusInd] + "'")) + "0") / 10;
                //float DraftILB = float.Parse(Convert.ToString(dsContainers.Tables[0].Compute("Sum(ChunkCount)", "InputFormId ='" + objContainerIds[index].ToString().Trim() + "' and ChunkStatus='DILB' and Status='" + status[statusInd] + "'")) + "0") / 10;
                //float RealDraftTot = DraftTotal - DraftILB;

                float MisRejTotal = float.Parse(Convert.ToString(dsContainers.Tables[0].Compute("Sum(ChunkCount)", "InputFormId ='" + objContainerIds[index].ToString().Trim() + "' and (ChunkStatus='M' or ChunkStatus='R') and Status='" + status[statusInd] + "'")) + "0") / 10;
                //float MisRejILB = float.Parse(Convert.ToString(dsContainers.Tables[0].Compute("Sum(ChunkCount)", "InputFormId ='" + objContainerIds[index].ToString().Trim() + "' and (ChunkStatus='MILB' or ChunkStatus='RILB') and Status='" + status[statusInd] + "'")) + "0") / 10;
                //float RealMisRejTot = MisRejTotal - MisRejILB;

                float total = FinalTotal + DraftTotal + MisRejTotal;

                if (total != 0)
                {
                    float value = (FinalTotal * 100) / total;
                    dr[status[statusInd] + ":FPer"] = value.ToString("F2");
                }
                else
                    dr[status[statusInd] + ":FPer"] = (FinalTotal * 100);

                if (FinalTotal != 0)
                {
                    float value = (FinalILB * 100) / FinalTotal;
                    dr[status[statusInd] + ":FILBPer"] = value.ToString("F2");
                }
                else
                    dr[status[statusInd] + ":FILBPer"] = (FinalILB * 100);

                if (total != 0)
                {
                    float value = (DraftTotal * 100) / total;
                    dr[status[statusInd] + ":DPer"] = value.ToString("F2");
                }
                else
                    dr[status[statusInd] + ":DPer"] = (DraftTotal * 100);


                if (total != 0)
                {
                    float value = (MisRejTotal * 100) / total;
                    dr[status[statusInd] + ":M/RPer"] = value.ToString("F2");
                }
                else
                    dr[status[statusInd] + ":M/RPer"] = (MisRejTotal * 100);

                //if (MisRejTotal != 0)
                //{
                //    float value = (MisRejILB * 100) / MisRejTotal;
                //    dr[status[statusInd] + ":M/RILBPer"] = value.ToString("F2");
                //}
                //else
                //    dr[status[statusInd] + ":M/RILBPer"] = (MisRejILB * 100);

            }

            string[] strArr = null;
            int Count1 = 0;
            int Count2 = 0;
            string sts = string.Empty;
            for (int cur = 0; cur < objRegionColumns.Length; cur++)
            {
                strArr = objRegionColumns[cur].ToString().Split('~');
                sts = strArr[0].Split(':')[0];
                strArr[0] = strArr[0].Split(':')[1];
                //if (strArr[0] == "M/R" || strArr[0] == "M/RILB")
                //{
                if (strArr[0] == "M/R")
                {
                    Count1 = int.Parse(Convert.ToString(dsContainers.Tables[0].Compute("Sum(ChunkCount)", "InputFormId ='" + objContainerIds[index].ToString().Trim() + "' and (ChunkStatus='M' or ChunkStatus='R') and CultureCode='" + strArr[1] + "' and Status='" + sts + "'")) + "0") / 10;
                    //Count2 = int.Parse(Convert.ToString(dsContainers.Tables[0].Compute("Sum(ChunkCount)", "InputFormId ='" + objContainerIds[index].ToString().Trim() + "' and (ChunkStatus='MILB' or ChunkStatus='RILB') and CultureCode='" + strArr[1] + "' and Status='" + sts + "'")) + "0") / 10;
                }
                //else
                //{
                //    Count1 = int.Parse(Convert.ToString(dsContainers.Tables[0].Compute("Sum(ChunkCount)", "InputFormId ='" + objContainerIds[index].ToString().Trim() + "' and (ChunkStatus='MILB' or ChunkStatus='RILB') and CultureCode='" + strArr[1] + "' and Status='" + sts + "'")) + "0") / 10;
                //    //Count2 = 0;
                //}
                // }
                else
                {
                    Count1 = int.Parse(Convert.ToString(dsContainers.Tables[0].Compute("Sum(ChunkCount)", "InputFormId ='" + objContainerIds[index].ToString().Trim() + "' and ChunkStatus='" + strArr[0] + "' and CultureCode='" + strArr[1] + "' and Status='" + sts + "'")) + "0") / 10;
                    Count2 = int.Parse(Convert.ToString(dsContainers.Tables[0].Compute("Sum(ChunkCount)", "InputFormId ='" + objContainerIds[index].ToString().Trim() + "' and ChunkStatus='" + strArr[0] + "ILB' and CultureCode='" + strArr[1] + "' and Status='" + sts + "'")) + "0") / 10;
                }
                //int FinalChCount = int.Parse(dsContainers.Tables[0].Select("ContainerId ='" + objContainerIds[index].ToString().Trim() + "' and ChunkStatus='F'  + [1].ToString() + "'")[0]["ChunkCount"] + "" + "0") / 10;
                dr[objRegionColumns[cur].ToString()] = Count1 - Count2;
                //dr[objRegionColumns[cur].ToString()] = Count1;
            }


            objDT.Rows.Add(dr);
        }

        objDT.AcceptChanges();
        objDT.Columns.RemoveAt(0);
        objDT.AcceptChanges();
        return sortDataTable(objDT, "InputFormName");
    }

    #region sortDataTable
    //<Summary>
    // This Method is used by both GetConsolidatedData
    // and GetInputFormData for sorting the datatable
    //</Summary>
    public DataTable sortDataTable(DataTable objDT, string sortExp)
    {
        DataRow[] objDTSorted = null;
        
        if(sortExp.Equals("InputFormName"))
        {
            objDT.Columns.Add("Name");//InputFormName containers HyperLink to Multi- Container View
            //So having a new column "Name" to store the real IF Name 
            // this is for sorting
            foreach (DataRow dr in objDT.Rows)
            {
                foreach (DataColumn dc in objDT.Columns)
                {
                    if (dc.Caption.Equals("Name"))
                    {
                        Array a = dr["InputFormName"].ToString().Split('>');
                        string tempString = a.GetValue(1).ToString();
                        int indx = tempString.IndexOf('<');
                        tempString = tempString.Remove(indx);
                        dr[dc] = tempString;
                    }
                }
            }
            sortExp = "Name";
        }
        DataTable objDTSortedTable = new DataTable();
        objDTSortedTable = objDT.Clone();
        objDTSorted = objDT.Select("", sortExp);//Sorting
        
        
        foreach (DataRow r in objDTSorted)
        {
            objDTSortedTable.ImportRow(r);
        }
        objDTSortedTable.AcceptChanges();
        if(sortExp.Equals("Name"))
        {
            objDTSortedTable.Columns.Remove("Name");
            objDTSortedTable.AcceptChanges();
        }
        return objDTSortedTable;
    }
    #endregion
    

    //Infragistics has a limitation. The Export to Excel functionality will not export the grids with multiple headers
    // To get around the problem a html table is created with multi-headers and then it is exported to excel
    //ExportInputForm does the export for InputForm Report
    //ExportContainer does the export for Singel and Mult Container Export
    public static void ExportInputForm(System.Data.DataTable dt, Page page, string regionCode, string BizName, string info)
    {
        // string contains html code to export

        System.Text.StringBuilder sb = new System.Text.StringBuilder(string.Empty);
        string[] split_RegionCode = regionCode.Split(',');
        string[] childHeader3 = new string[split_RegionCode.Length + 2];
        int i;
        for (i = 0; i < split_RegionCode.Length; i++)
        {
            childHeader3[i] = split_RegionCode[i];
        }
        childHeader3[split_RegionCode.Length] = "%";
        childHeader3[split_RegionCode.Length + 1] = "#";
        int regionCodeCount = split_RegionCode.Length;
        sb.Append("<html><head>");
        //   sb.Append("<meta http-equiv='Content-Type' content='text/html; charset=" + page.Request.ContentEncoding.WebName+ "'>");
        sb.Append("</head><body style='font-size: 14; font-family:Arial Unicode MS'>");
        // item name
        sb.Append("<table style='font-size: 14; font-family:Arial Unicode MS' border = '1'>");
        // sb.Append("<tr>");
        sb.Append("InputFormViewReport");
        //sb.Append("</TR><TR>");
        sb.Append("<br>" + BizName + "<br>");
        // sb.Append("</TR><TR>");
        sb.Append(("Exported On:" + SessionState.User.FormatUtcDate(DateTime.UtcNow, true, "MM/dd/yyyy:HH:mm:ss" +"<br>")));
        sb.Append("Generated By:" + SessionState.User.FullName);
        /// sb.Append("</tr><tr>");
        if (info != null)
        {
            // sb.Append("</tr>");

            sb.Append("<br>" + info);
            // sb.Append("</tr><tr>");
        }
        sb.Append("<TH rowspan='5'>InputFormName</th>");
        string[] parentHeader = new string[] { "Live", "Future", "Obsolete" };

        /* Alternate for CR 5096(Removal of rejection functionality)--start
        string[] childHeader = new string[] { "Final Mandatory Chunks", "Draft Mandatory Chunks", "Rejected/Missing Mandatory Chunks" }; */
        string[] childHeader = new string[] { "Final Mandatory Chunks", "Draft Mandatory Chunks", "Missing Mandatory Chunks" };
        // Alternate for CR 5096(Removal of rejection functionality)--end

        string[] childHeader2 = new string[] { "%", "#" };
        int imgCount = 0;
        int parentHeaderOriginX = 0;
        int parentHeaderCount = 0;
        int childHeaderCount;
        //int childHeader2Count;
        while (parentHeaderCount < parentHeader.Length)
        {
            sb.Append("<TH colspan = " + 4 * (regionCodeCount + 1) + ">" + parentHeader[parentHeaderCount] + "</TH>");
            //parentHeaderOriginX =  6 * (regionCodeCount + 1);
            parentHeaderCount++;
        }
        int overAllCounter = 4 * (regionCodeCount + 1) * parentHeader.Length;
        sb.Append("</TR><TR>");
        int colspanCount = 0;
        childHeaderCount = 0;
        int span = regionCodeCount + 1;
        while (colspanCount < overAllCounter)
        {
            //childHeaderCount = 0;
            // parentHeaderOriginX = parentHeaderOriginX + 6 * (regionCodeCount + 1);
            if ((colspanCount == 0) || (colspanCount == 4 * (regionCodeCount + 1)) || (colspanCount == 8 * (regionCodeCount + 1)))
            {
                colspanCount = colspanCount + 2 * (regionCodeCount + 1);
                sb.Append("<TH colspan = " + 2 * (regionCodeCount + 1) + ">" + childHeader[childHeaderCount] + "</TH>");
            }
            else
            {
                colspanCount = colspanCount + regionCodeCount + 1;
                sb.Append("<TH colspan = " + (regionCodeCount + 1) + ">" + childHeader[childHeaderCount] + "</TH>");
            }
            if (childHeaderCount < childHeader.Length - 1)
            {
                childHeaderCount++;
            }
            else
            {
                childHeaderCount = 0;
            }

        }
        sb.Append("</TR><TR>");
        colspanCount = 0;
        //colspanCount = parentHeader.Length*childHeader.Length;
        // childHeader2Count = 0;                        
        while (colspanCount < overAllCounter)
        {
            //colspanCount = colspanCount + 2 * (regionCodeCount + 1);
            if (colspanCount.Equals(1 + regionCodeCount) || colspanCount.Equals(1 + regionCodeCount + 4 * (regionCodeCount + 1)) || colspanCount.Equals(1 + regionCodeCount + 8 * (regionCodeCount + 1)))
            {
                sb.Append("<TH colspan = " + span + ">ILB</TH>");
                colspanCount = colspanCount + regionCodeCount + 1;
            }
            if (childHeader2[childHeaderCount].Equals("%"))
            {
                sb.Append("<TH rowspan = 3>" + childHeader2[childHeaderCount] + "</TH>");
                colspanCount = colspanCount + 1;
            }
            //    else if (childHeader2[childHeaderCount].Equals("ILB"))
            //    {
            //        sb.Append("<TH colspan = " + span + ">" + childHeader2[childHeaderCount] + "</TH>");
            //colspanCount = colspanCount + regionCodeCount+1;
            //    }
            else
            {
                sb.Append("<TH colspan = " + regionCodeCount + ">" + childHeader2[childHeaderCount] + "</TH>");
                colspanCount = colspanCount + regionCodeCount;
            }
            if (childHeaderCount < childHeader2.Length - 1)
            {
                childHeaderCount++;
            }
            else
            {
                childHeaderCount = 0;
            }
        }
        sb.Append("</TR><TR>");

        // colspanCount = 0;        
        int childHeader3Counter = parentHeader.Length * (4 * regionCodeCount + 1);
        //colspanCount = parentHeader.Length * childHeader.Length;
        colspanCount = 0;
        childHeaderCount = 0;
        while (colspanCount < childHeader3Counter)
        {
            if ((colspanCount < 2 * regionCodeCount + 2) || ((colspanCount > 4 * regionCodeCount + 1) && (colspanCount < 2 * regionCodeCount + 4 * regionCodeCount + 1)) || ((colspanCount > 2 * (4 * regionCodeCount + 1)) && colspanCount < 2 * regionCodeCount + 2 * (4 * regionCodeCount + 1)))
            {
                if (childHeader3[childHeaderCount].Equals("#"))
                {
                    sb.Append("<TH colspan =" + regionCodeCount + ">" + childHeader3[childHeaderCount] + "</TH>");
                    colspanCount = colspanCount + regionCodeCount;
                }
                else
                {
                    sb.Append("<TH rowspan = 2>" + childHeader3[childHeaderCount] + "</TH>");
                    colspanCount++;
                }
            }
            else if (!childHeader3[childHeaderCount].Equals("#") && !childHeader3[childHeaderCount].Equals("%"))
            {
                sb.Append("<TH rowspan = 2>" + childHeader3[childHeaderCount] + "</TH>");
                colspanCount++;
            }
            if (childHeaderCount < childHeader3.Length - 1)
            {
                childHeaderCount++;

            }
            else
            {
                childHeaderCount = 0;
            }
        }
        sb.Append("</TR><TR>");
        //colspanCount = parentHeader.Length * childHeader.Length * childHeader3.Length;
        colspanCount = 0;
        childHeaderCount = 0;
        while (colspanCount < 3 * split_RegionCode.Length)
        {
            sb.Append("<TH >" + split_RegionCode[childHeaderCount] + "</TH>");
            colspanCount++;
            if (childHeaderCount < split_RegionCode.Length - 1)
            {
                childHeaderCount++;
            }
            else
            {
                childHeaderCount = 0;
            }
        }

        sb.Append("</TR>");

        foreach (DataRow dr in dt.Rows)
        {
            sb.Append("<tr>");
            foreach (DataColumn dc in dt.Columns)
            {
                if (dr[dc].ToString().Contains(">"))
                {
                    Array a = dr[dc].ToString().Split('>');
                    string tempString = a.GetValue(1).ToString();
                    int indx = tempString.IndexOf('<');
                    tempString = tempString.Remove(indx);
                    sb.Append("<td>" + tempString + "</td>");
                }
                else
                {
                    sb.Append("<td>" + dr[dc].ToString() + "</td>");
                }

            }
            sb.Append("</tr>");
        }
        sb.Append("</table>");
        sb.Append("</body></html>");
        string fileName = string.Empty;
        fileName += "InputFormViewReport.xls";
        string exportContent = sb.ToString();
        page.Response.Clear();
        page.Response.ClearContent();
        page.Response.ClearHeaders();
        page.Response.Charset = string.Empty;
        page.Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
        page.Response.ContentType = "application/vnd.ms-excel;";
        //page.Response.ContentType = "application/vnd.ms-excel;charset=UTF-8";
        //Fix for CR 5109 - Prabhu R S
        page.Response.ContentEncoding = System.Text.Encoding.UTF8;      //page.Response.AppendHeader("Content-Length", System.Text.Encoding.UTF8.GetByteCount(exportContent).ToString());
        page.EnableViewState = false;
        page.Response.Write(exportContent);
        page.Response.End();

    }

    public static void ExportContainer(System.Data.DataTable dt, Page page, string regionCode, string BizName, string InputFormName)
    {
        // string contains html code to export
        // The multi-header is implemented here in a row by row basic
        // the Infragistics Initialize layout was done in column by column basis
        System.Text.StringBuilder sb = new System.Text.StringBuilder(string.Empty);
        string[] split_RegionCode = regionCode.Split(',');
        string[] childHeader3 = new string[split_RegionCode.Length + 2];
        int i;
        for (i = 0; i < split_RegionCode.Length; i++)
        {
            childHeader3[i] = split_RegionCode[i];
        }
        childHeader3[split_RegionCode.Length] = "%";
        childHeader3[split_RegionCode.Length + 1] = "#";
        int regionCodeCount = split_RegionCode.Length;
        sb.Append("<html><head>");
        //   sb.Append("<meta http-equiv='Content-Type' content='text/html; charset=" + page.Request.ContentEncoding.WebName+ "'>");
        sb.Append("</head><body style='font-size: 14; font-family:Arial Unicode MS'>");
        // item name
        sb.Append("<table style='font-size: 14; font-family:Arial Unicode MS' border = '1'>");
        //InputFormName = InputFormName.Trim();
        sb.Append("ContainerViewReport");
        // sb.Append("</TR><TR>");
        sb.Append("<br>" + BizName);


        if (InputFormName != null)
        {
            sb.Append("<br>" + InputFormName);
        }
        //sb.Append("</TR><TR>");
        sb.Append(("<br>" + "Exported On:" + SessionState.User.FormatUtcDate(DateTime.UtcNow, true, "MM/dd/yyyy:HH:mm:ss")));
        sb.Append("<br> " +"Generated By:" + SessionState.User.FullName);
        sb.Append("<TR><TH rowspan='4'>Container Name</th>");
        /* Alternate for CR 5096(Removal of rejection functionality)--start
        string[] childHeader = new string[] { "Final  Mandatory Chunks", "Draft  Mandatory Chunks", "Rejected/Missing  Mandatory Chunks" }; */
        string[] childHeader = new string[] { "Final  Mandatory Chunks", "Draft  Mandatory Chunks", "Missing  Mandatory Chunks" };
        // Alternate for CR 5096(Removal of rejection functionality)--end

        string[] childHeader2 = new string[] { "%", "#" };
        int imgCount = 0;
        int parentHeaderOriginX = 0;
        int parentHeaderCount = 0;
        int childHeaderCount;
        int overAllCounter = 4 * (regionCodeCount + 1);
        //sb.Append("<TR>");
        int colspanCount = 0;
        childHeaderCount = 0;
        int span = regionCodeCount + 1;
        while (colspanCount < overAllCounter)
        {
            //childHeaderCount = 0;
            // parentHeaderOriginX = parentHeaderOriginX + 6 * (regionCodeCount + 1);
            if (colspanCount == 0)
            {
                colspanCount = colspanCount + 2 * (regionCodeCount + 1);
                sb.Append("<TH colspan = " + 2 * (regionCodeCount + 1) + ">" + childHeader[childHeaderCount] + "</TH>");
            }
            else
            {
                colspanCount = colspanCount + regionCodeCount + 1;
                sb.Append("<TH colspan = " + (regionCodeCount + 1) + ">" + childHeader[childHeaderCount] + "</TH>");
            }
            if (childHeaderCount < childHeader.Length - 1)
            {
                childHeaderCount++;
            }
            //else
            //{
            //    childHeaderCount = 0;
            //}

        }
        sb.Append("</TR><TR>");
        colspanCount = 0;
        childHeaderCount = 0;
        //colspanCount = parentHeader.Length*childHeader.Length;
        // childHeader2Count = 0;                        
        while (colspanCount < overAllCounter)
        {
            //colspanCount = colspanCount + 2 * (regionCodeCount + 1);
            if (colspanCount.Equals(1 + regionCodeCount))
            {
                sb.Append("<TH colspan = " + span + ">ILB</TH>");
                colspanCount = colspanCount + regionCodeCount + 1;
            }
            if (childHeader2[childHeaderCount].Equals("%"))
            {
                sb.Append("<TH rowspan = 3>" + childHeader2[childHeaderCount] + "</TH>");
                colspanCount = colspanCount + 1;
            }
            //else if ((childHeader2[childHeaderCount].Equals("%")) && (colspanCount = 2+regionCodeCount))
            //{
            //    sb.Append("<TH colspan = " + span + ">" + childHeader2[childHeaderCount] + "</TH>");
            //    colspanCount = colspanCount + regionCodeCount + 1;
            //}
            else
            {
                sb.Append("<TH colspan = " + regionCodeCount + ">" + childHeader2[childHeaderCount] + "</TH>");
                colspanCount = colspanCount + regionCodeCount;
            }
            if (childHeaderCount < childHeader2.Length - 1)
            {
                childHeaderCount++;
            }
            else
            {
                childHeaderCount = 0;
                //   // string[] childHeader2 = new string[] { "%", "#"};
                //      //  = { "%", "#"};// Since the first round for the Fianl Mandatory CHunk is complete
            }
        }
        sb.Append("</TR><TR>");
        int childHeader3Counter = 4 * regionCodeCount + 1;
        colspanCount = 0;
        // colspanCount = childHeader.Length;
        childHeaderCount = 0;
        while (colspanCount < childHeader3Counter)
        {
            if (colspanCount < 2 * regionCodeCount + 2)
            {
                if (childHeader3[childHeaderCount].Equals("#"))
                {
                    sb.Append("<TH colspan =" + regionCodeCount + ">" + childHeader3[childHeaderCount] + "</TH>");
                    colspanCount = colspanCount + regionCodeCount;
                }
                else
                {
                    sb.Append("<TH rowspan = 2>" + childHeader3[childHeaderCount] + "</TH>");
                    colspanCount++;
                }
            }
            else if (!childHeader3[childHeaderCount].Equals("#") && !childHeader3[childHeaderCount].Equals("%"))
            {
                sb.Append("<TH rowspan = 2>" + childHeader3[childHeaderCount] + "</TH>");
                colspanCount++;
            }
            if (childHeaderCount < childHeader3.Length - 1)
            {
                childHeaderCount++;

            }
            else
            {
                childHeaderCount = 0;
            }
        }
        sb.Append("</TR><TR>");
        colspanCount = childHeader.Length * childHeader3.Length;
        childHeaderCount = 0;
        //while (colspanCount < overAllCounter)
        //{
        while (childHeaderCount < split_RegionCode.Length)
        {
            sb.Append("<TH >" + split_RegionCode[childHeaderCount] + "</TH>");
            colspanCount++;
            //if (childHeaderCount < split_RegionCode.Length - 1)
            //{
            childHeaderCount++;
            //}
            //else
            //{
            //    childHeaderCount = 0;
            //}
        }

        sb.Append("</TR>");
        foreach (DataRow dr in dt.Rows)
        {
            sb.Append("<tr>");
            foreach (DataColumn dc in dt.Columns)
            {
                sb.Append("<td>" + dr[dc].ToString() + "</td>");
            }
            sb.Append("</tr>");
        }
        sb.Append("</table>");
        sb.Append("</body></html>");
        string fileName = string.Empty;
        fileName += "ContainerViewReport.xls";
        string exportContent = sb.ToString();
        page.Response.Clear();
        page.Response.ClearContent();
        page.Response.ClearHeaders();
        page.Response.Charset = string.Empty;
        page.Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
        page.Response.ContentType = "application/vnd.ms-excel;";
        //page.Response.ContentType = "application/vnd.ms-excel;charset=UTF-8";
        //Fix for CR 5109 - Prabhu R S
        page.Response.ContentEncoding = System.Text.Encoding.UTF8;      //page.Response.AppendHeader("Content-Length", System.Text.Encoding.UTF8.GetByteCount(exportContent).ToString());
        page.EnableViewState = false;
        page.Response.Write(exportContent);
        page.Response.End();
    }

    // This method is used by almost all reports
    // when a ListBox is multi-selected then all the selections are returns as a comma seprated string by this method
    // If 'All' option is selected then all the entries in the ListBox, whether selected or not
    // will be returned as a comma seperated string
    public string getSelectedValues(ListBox lstBox)
    {
        string selValues = string.Empty;

        if (lstBox.SelectedValue == "All")
        {
            for (int i = 1; i <= lstBox.Items.Count - 1; i++)
            {
                selValues += lstBox.Items[i].Value.Trim() + ",";
            }
        }
        else
        {
            for (int i = 1; i <= lstBox.Items.Count - 1; i++)
            {
                if (lstBox.Items[i].Selected)
                    selValues += lstBox.Items[i].Value.Trim() + ",";
            }
        }

        if (selValues.Length == 0)
        {
            return null;
        }
        else
        {

            selValues = selValues.Remove(selValues.Length - 1, 1);

            return selValues.Trim();
        }


    }

    //<Summary>
    //The following code initializes the UltraWebGrid for both the Single Container View and Multi-container View
    //<summary>
    public void ContainerViewUltraGrid_Initialize(Infragistics.WebUI.UltraWebGrid.LayoutEventArgs e, string regionCode)
    {
        string[] split_RegionCode = regionCode.Split(',');
        int counter = 0;
        int headerCount = 0;
        int regionCodeCount = split_RegionCode.Length;
        foreach (Infragistics.WebUI.UltraWebGrid.UltraGridColumn c in e.Layout.Bands[0].Columns)
        {
            c.Header.RowLayoutColumnInfo.OriginY = 3;
        }

        //string[] header = new string[] { "Final", "Final#", "FinalILB", "FinalILB#", "Draft", "Draft#", "DraftILB", "DraftILB#", "MisRej", "MisRej#", "MisRejILB", "MisRejILB#" };
        //string[] header = new string[] { "/hc_v4/img/status2.gif", "#", "ILB", "#", "/hc_v4/img/status0.gif", "#", "ILB", "#", "/hc_v4/img/status-1.gif", "#", "ILB", "#" };
        string[] header = new string[] { "/hc_v4/img/status2.gif", "#", "ILB", "#", "/hc_v4/img/status0.gif", "#", "/hc_v4/img/status-1.gif", "#" };

        /* Alternate for CR 5096(Removal of rejection functionality)--start
        string[] toolTip = new string[] { "Final Mandatory Chunks", "Draft Mandatory Chunks", "Rejected/Missing Mandatory Chunks" }; */
        string[] toolTip = new string[] { "Final Mandatory Chunks", "Draft Mandatory Chunks", "Missing Mandatory Chunks" };
        // Alternate for CR 5096(Removal of rejection functionality)--end

        int imgCount = 0;
        while (headerCount < header.Length)
        {
            Infragistics.WebUI.UltraWebGrid.ColumnHeader ch = new ColumnHeader(true);

            //ch.Caption = header[headerCount];
            ch.Image.Url = header[headerCount];
            ch.Style.HorizontalAlign = HorizontalAlign.Center;
            ch.Style.VerticalAlign = VerticalAlign.Middle;
            ch.Style.Font.Bold = true;
            if (ch.HasImage)
            {
                ch.Image.AlternateText = toolTip[imgCount];
                imgCount++;
            }
            //ch.Title = "tested123";
            ch.RowLayoutColumnInfo.OriginY = 0;

            ch.RowLayoutColumnInfo.OriginX = counter + 1;

            if (headerCount == 0)
            {
                ch.RowLayoutColumnInfo.SpanX = 2 * (regionCodeCount + 1);
            }
            else
            {
                ch.RowLayoutColumnInfo.SpanX = regionCodeCount + 1;
            }
            counter = counter + ch.RowLayoutColumnInfo.SpanX;

            e.Layout.Bands[0].HeaderLayout.Add(ch);

            headerCount++;
            Infragistics.WebUI.UltraWebGrid.ColumnHeader ch1 = new ColumnHeader(true);
            ch1.Caption = header[headerCount];
            ch1.Style.HorizontalAlign = HorizontalAlign.Center;
            ch1.Style.VerticalAlign = VerticalAlign.Middle;
            ch1.Style.Font.Bold = true;

            ch1.RowLayoutColumnInfo.OriginY = 1;

            ch1.RowLayoutColumnInfo.OriginX = ch.RowLayoutColumnInfo.OriginX + 1;

            ch1.RowLayoutColumnInfo.SpanX = regionCodeCount;


            e.Layout.Bands[0].HeaderLayout.Add(ch1);
            headerCount++;
            if (headerCount < header.Length - 1)
            {
                if (header[headerCount].Equals("ILB"))
                {
                    Infragistics.WebUI.UltraWebGrid.ColumnHeader ch2 = new ColumnHeader(true);
                    ch2.Caption = header[headerCount];
                    ch2.Style.HorizontalAlign = HorizontalAlign.Center;
                    ch2.Style.VerticalAlign = VerticalAlign.Middle;
                    ch2.Style.Font.Bold = true;
                    ch2.RowLayoutColumnInfo.OriginY = 1;
                    ch2.RowLayoutColumnInfo.OriginX = ch1.RowLayoutColumnInfo.OriginX + regionCodeCount;
                    ch2.RowLayoutColumnInfo.SpanX = regionCodeCount + 1;
                    // counter = counter + ch1.RowLayoutColumnInfo.OriginX + ch1.RowLayoutColumnInfo.SpanX + 1;
                    e.Layout.Bands[0].HeaderLayout.Add(ch2);

                    headerCount++;

                    Infragistics.WebUI.UltraWebGrid.ColumnHeader ch3 = new ColumnHeader(true);
                    ch3.Caption = header[headerCount];
                    ch3.Style.HorizontalAlign = HorizontalAlign.Center;
                    ch3.Style.VerticalAlign = VerticalAlign.Middle;
                    ch3.Style.Font.Bold = true;
                    ch3.RowLayoutColumnInfo.OriginY = 2;
                    ch3.RowLayoutColumnInfo.OriginX = ch2.RowLayoutColumnInfo.OriginX + 1;
                    ch3.RowLayoutColumnInfo.SpanX = regionCodeCount;
                    // counter = counter + ch1.RowLayoutColumnInfo.OriginX + ch1.RowLayoutColumnInfo.SpanX + 1;
                    e.Layout.Bands[0].HeaderLayout.Add(ch3);
                    headerCount++;
                    //headerCount = header.Length;
                }
            }
        }
        foreach (Infragistics.WebUI.UltraWebGrid.UltraGridColumn c in e.Layout.Bands[0].Columns)
        {
            //if (c.Key != "A02" && c.Key != "A03" && c.Key != "A04" && c.Key != "A05" && c.Key != "A06" && c.Key != "A07" && c.Key != "A08")
            if (c.Key.Contains("Container"))
            {
                c.Header.RowLayoutColumnInfo.OriginY = 0;
                c.Header.RowLayoutColumnInfo.SpanY = 4;
                //Response.Write("here we are");
            }
            else if ((c.Key.Contains("ILB") && !c.Key.Contains("~")) || (!c.Key.Contains("ILB") && c.Key.Contains("~")))
            {
                c.Header.RowLayoutColumnInfo.OriginY = 2;
                c.Header.RowLayoutColumnInfo.SpanY = 2;
                //Response.Write("here we are");
            }
            //else if (!c.Key.Contains("~") && c.Key.Contains("ILB"))
            //{
            else if (c.Key.Contains("#"))
            {
                c.Header.RowLayoutColumnInfo.OriginY = 1;
                c.Header.RowLayoutColumnInfo.SpanY = 1;
            }
            else if (c.Key.Contains("Per") && (!c.Key.Contains("ILB")))
            {
                c.Header.RowLayoutColumnInfo.OriginY = 1;
                c.Header.RowLayoutColumnInfo.SpanY = 3;

            }
            if (c.Key.Contains("~"))
            {

                c.Header.Caption = c.Key.Split('~')[1].Split('-')[0].ToUpper();
            }
            if (c.Key.Contains("Per"))
            {
                c.CellStyle.Font.Bold = true;
                c.Header.Caption = "%";
            }
            //img.src = "/hc_v4/img/status2.gif";

            if (c.Key.Contains("~"))
            {
                c.CellStyle.ForeColor = System.Drawing.Color.Green;
            }

            c.Width = Unit.Pixel(40);
            c.Header.ClickAction = HeaderClickAction.Select;
            c.Header.Style.HorizontalAlign = HorizontalAlign.Center;
            c.Header.Style.VerticalAlign = VerticalAlign.Middle;
            c.Header.Style.Font.Bold = true;
            c.AllowResize = AllowSizing.Fixed;
            c.AllowUpdate = AllowUpdate.No;
            if (c.Key.Contains("ContainerName"))
            {
                c.CellStyle.BackColor = System.Drawing.Color.LightGray;
            }
        }
    }

    //<Summary>
    // This code will give the list of selected Biz/ Region
    // This will be used in the excel export showing the user selected
    // <Params>
    // isBiz 0 - Region 1- Biz
    // shortCode - comma seperated code
    // if Biz then ShortCode will be the BizCode (as seen in BPL Table)
    // else shortCode will be the regionShortCode
    // int Code wil be used only if IsBiz = 1
    //  0-returns active OrgName
    // 1-returns active GroupName
    // 2-returns active GBUName
    //</params>
    //</Summary>
    public string GetSelectedBiz_Reg(int isBiz, string shortCode, int code)
    {
        Database dbObj = new Database(HyperCatalog.Business.ApplicationSettings.Components["Datawarehouse_DB"].ConnectionString);
        DataSet dsNames = new DataSet();
        if (isBiz.Equals(1))
        {

            dsNames = dbObj.RunSPReturnDataSet("_GetSelectedBusinessNames", new SqlParameter("@BizCode", shortCode), new SqlParameter("@Code", code));
        }
        else
        {
            //dsNames = dbObj.RunSQLReturnDataSet("select distinct CultureName from Cultures,Split('" + shortCode + "',',')  where CountryCode  = Value and LanguageCode = 'EN'");
            dsNames = dbObj.RunSQLReturnDataSet("select distinct CultureName from Cultures,Split('" + @shortCode + "',',')  where CountryCode  = Value and LanguageCode = 'EN'", HyperComponents.Data.dbAccess.Database.NewSqlParameter("@shortCode", System.Data.SqlDbType.Text, shortCode.Length, shortCode));

            // dsNames = dbObj.RunSPReturnDataSet("_GetSelectedRegions",newSqlParameter("@shortCode", shortCode));

        }

        string list = "";
        foreach (DataRow r in dsNames.Tables[0].Rows)
        {

            list += r[0].ToString() + ",";
        }
        list = list.Remove(list.Length - 1, 1);
        return list;
    }
}



