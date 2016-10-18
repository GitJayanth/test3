using System;
using HyperCatalog.Business;
using HyperCatalog.Shared;
using System.Web.UI;
using System.Data;
//Added for Enhancement for the BUG#70135
using Infragistics.WebUI.UltraWebGrid;
//Added for Enhancement for the BUG#70135
namespace HyperCatalog.UI
{
	/// <summary>
	/// this class allows exporting the grid of frame content
	/// </summary>
  public class ExportUtils
  {
    /// <summary>
    /// Default constructor
    /// </summary>
    public ExportUtils() { }

    /// <summary>
    /// Export the data in Excel file
    /// </summary>
    /// <param name="itemId">Id of the item</param>
    /// <param name="inputFormId">Id of the input form</param>
    /// <param name="viewMandatory">Display mandatory or not</param>
    /// <param name="filter">Filter in the grid</param>
    /// <param name="page">Current page</param>



    //Added for Enhancement for the BUG#70135

    private static string GetProperShortDate(DateTime? d)
    {
        return d.HasValue ? d.Value.ToString(SessionState.User.FormatDate) : string.Empty;
    }
    #region Export Content Tab
    public static void ExportGridOfFrameContentCountry(System.Int64 itemId, bool Localizable, bool SingleView, string filter, Page page, UltraWebGrid dgUPC)
    {

        #region Declaration
        Item item = Item.GetByKey(itemId);
        int inputFormId = -1;
        int Local = 0;
        int sView = 0;
        #endregion

        if (Localizable)
        {
            Local = 1;
        }
        if (SingleView)
        {
            sView = 1;
        }
        try
        {
            #region Containers
            InputForm inputForm = InputForm.GetByKey(inputFormId);
            if (item != null)
            {
                // Retrieve list of input form chunk
                InputFormChunkList chunkList = InputFormChunk.GetChunksByCountry(itemId, SessionState.Culture.Code, Local, sView);

                if (chunkList != null && chunkList.Count > 0)
                {
                    // apply filter if necessary
                    int i = chunkList.Count - 1;
                    while (i >= 0)
                    {
                        // mandatory view
                        InputFormChunk chunk = chunkList[i];
                        if (!chunk.Mandatory)
                        {
                            chunkList.RemoveAt(i);
                        }
                        else
                        {
                            if (Localizable && !chunk.Localizable)
                            {
                                chunkList.RemoveAt(i);
                            }
                            // filter (container name or value)
                            if (filter.Length > 0)
                            {
                                filter = filter.ToLower();
                                string contName = chunk.ContainerName.ToLower();
                                string chunkText = chunk.Text.ToLower();
                                if (!(contName.IndexOf(filter) > 0))
                                {
                                    if (!chunk.IsResource && !(chunkText.IndexOf(filter) > 0))
                                    {
                                        chunkList.RemoveAt(i);
                                    }
                                }
                            }
                        }
                        i--;
                    }
                    // string contains html code to export
                    System.Text.StringBuilder sb = new System.Text.StringBuilder(string.Empty);

                    sb.Append("<html><head>");
                    sb.Append("<meta http-equiv='Content-Type' content='text/html; charset=utf-8'>");
                    sb.Append("</head><body>");

                    #region "Header"
                    // item name
                    sb.Append("<table border='1'><tr><td colspan='2' style='font-size: 14; font-weight: bold; background-color: lightgrey'>Node name</td><td style='font-size: 12' colspan='5'>");
                    if (item.Sku.Length > 0)
                    {
                        sb.Append("[");
                        sb.Append(item.Sku);
                        sb.Append("] ");
                    }
                    sb.Append(item.Name);
                    sb.Append(" (#");
                    sb.Append(item.Id);
                    sb.Append(", ");
                    sb.Append(item.Level.Name);
                    sb.Append(")");
                    sb.Append("</td></tr>");

                    // Input form name
                    if (inputForm != null)
                    {
                        sb.Append("<tr><td colspan='2' style='font-size: 14; font-weight: bold; background-color: lightgrey'>Input form name</td><td style='font-size: 12' colspan='5'>");
                        sb.Append(inputForm.Name);
                        sb.Append(", Total: ");
                        sb.Append(inputForm.Containers.Count);
                        sb.Append("   Mandatory: ");
                        int nbMandatoryChunks = 0;
                        foreach (InputFormContainer ifc in inputForm.Containers)
                        {
                            if (ifc.Mandatory)
                                nbMandatoryChunks++;
                        }
                        sb.Append(nbMandatoryChunks.ToString());
                        sb.Append("</td></tr>");
                    }

                    // Export by
                    sb.Append("<tr><td colspan='2' style='font-size: 14; font-weight: bold; background-color: lightgrey'>Exported by</td><td colspan='5'>");
                    sb.Append(SessionState.User.Email);
                    sb.Append(" - <i>");
                    sb.Append(SessionState.User.FormatUtcDate(DateTime.UtcNow, true, SessionState.User.FormatDate));
                    sb.Append(" at ");
                    sb.Append(SessionState.User.FormatUtcDate(DateTime.UtcNow, true, SessionState.User.FormatTime));
                    sb.Append(" (");
                    sb.Append(SessionState.CacheParams["AppName"].Value.ToString());
                    sb.Append(")");
                    sb.Append("</i></td></tr>");

                    // List type
                    sb.Append("<tr><td colspan='2' style='font-size: 14; font-weight: bold; background-color: lightgrey'>Container list</td><td colspan='5'>");
                    if (filter.Length > 0)
                        sb.Append("Partial");
                    // else if (viewMandatory)
                    //   sb.Append("Mandatory only");
                    else
                        sb.Append("Complete");
                    sb.Append("</td></tr></table>");

                    sb.Append("<br>");
                    #endregion

                    #region "Column header"
                    sb.Append("<table border='1'>");
                    sb.Append("<tr style='font-weight: bold; background-color: lightgrey'>");
                    sb.Append("<td>Mandatory</td>");
                    sb.Append("<td>Inherited</td>");
                    sb.Append("<td>Read only</td>");
                    sb.Append("<td>Status</td>");
                    sb.Append("<td>Container name</td>");
                    sb.Append("<td>Value</td>");
                    sb.Append("<td>Comment</td>");
                    sb.Append("</tr>");
                    #endregion

                    #region "Rows"
                    /* Alternate for CR 5096(Removal of rejection functionality)--start
                    int nbMandatory, nbDraft, nbDraftInh, nbFinal, nbFinalInh, nbRejected, nbRejectedInh, nbMissing,
                      nbDraftM, nbFinalM, nbMissingM, nbRejectedM, nbDraftInhM, nbFinalInhM, nbRejectedInhM, nbTotal;
                    nbMandatory = nbDraft = nbDraftInh = nbFinal = nbFinalInh = nbRejected = nbRejectedInh = nbMissing = 0;
                    nbDraftM = nbFinalM = nbMissingM = nbRejectedM = nbDraftInhM = nbFinalInhM = nbRejectedInhM = nbTotal = 0;
                     */
                    int nbMandatory, nbDraft, nbDraftInh, nbFinal, nbFinalInh, nbMissing, nbDraftM, nbFinalM, nbMissingM,                       nbDraftInhM, nbFinalInhM, nbTotal;
                    nbMandatory = nbDraft = nbDraftInh = nbFinal = nbFinalInh = nbMissing = 0;
                    nbDraftM = nbFinalM = nbMissingM = nbDraftInhM = nbFinalInhM = nbTotal = 0;
                    // Alternate for CR 5096(Removal of rejection functionality)--end

                    string path = string.Empty;
                    nbTotal = chunkList.Count;
                    foreach (InputFormChunk ifc in chunkList)
                    {
                        // add group
                        if (ifc.Path != path)
                        {
                            path = ifc.Path;
                            sb.Append("<tr><td style='font-weight:bold; color:white; background-color: #003366' colspan='7'>");
                            sb.Append(path);
                            sb.Append("</td></tr>");
                        }

                        sb.Append("<tr>");

                        // add mandatory or not
                        if (ifc.Mandatory)
                        {
                            sb.Append("<td align='center' style='background-color: #D0D0D0'><font size='5' color='red'>*</font></td>");
                            nbMandatory++; // update mandatory count
                        }
                        else
                            sb.Append("<td style='background-color: #D0D0D0'></td>");

                        // add inherited or not
                        if (ifc.Inherited)
                            sb.Append("<td style='background-color: #D0D0D0' align='center'><b>X</b></td>");
                        else
                            sb.Append("<td style='background-color: #D0D0D0'></td>");

                        // add read only container
                        if (ifc.ReadOnly)
                            sb.Append("<td style='background-color: #D0D0D0' align='center'><b>X</b></td>");
                        else
                            sb.Append("<td style='background-color: #D0D0D0'></td>");

                        // add status
                        string fontColor = string.Empty;
                        switch (ifc.Status)
                        {
                            case ChunkStatus.Draft: fontColor = "orange"; break;
                            case ChunkStatus.Final: fontColor = "green"; break;
                            //case ChunkStatus.Rejected: fontColor = "red"; break; --Commented for alternate for CR 5096
                            case ChunkStatus.Missing: fontColor = "blue"; break;
                        }
                        sb.Append("<td align='center' style='background-color: #D0D0D0'>");
                        sb.Append("<font color='");
                        sb.Append(fontColor);
                        sb.Append("'>");
                        sb.Append((ifc.Status.ToString())[0]);
                        sb.Append("</font>");
                        sb.Append("</td>");

                        // add container name
                        sb.Append("<td style='background-color: #D0D0D0'>");
                        sb.Append(ifc.ContainerName);
                        sb.Append("</td>");

                        // add value
                        if (ifc.Rtl)
                            sb.Append("<td style='direction: rtl'>"); // if Rtl
                        else
                            sb.Append("<td>");

                        if (ifc.IsBool)
                        {
                            if (ifc.Text == "1")
                                sb.Append("Yes");
                            else
                                sb.Append("No");
                        }
                        else if (ifc.Text == HyperCatalog.Business.Chunk.BlankValue)
                            sb.Append(HyperCatalog.Business.Chunk.BlankText);
                        else
                            sb.Append(ifc.Text);

                        sb.Append("</td>");

                        // Comment
                        sb.Append("<td>");
                        sb.Append(ifc.Comment);
                        sb.Append("</td>");

                        sb.Append("</tr>");

                        // Update count
                        switch (ifc.Status)
                        {
                            case ChunkStatus.Draft: nbDraft++;
                                if (ifc.Inherited) nbDraftInh++;
                                if (ifc.Mandatory) nbDraftM++;
                                if (ifc.Inherited && ifc.Mandatory) nbDraftInhM++;
                                break;
                            case ChunkStatus.Final: nbFinal++;
                                if (ifc.Inherited) nbFinalInh++;
                                if (ifc.Mandatory) nbFinalM++;
                                if (ifc.Inherited && ifc.Mandatory) nbFinalInhM++;
                                break;
                            /* Alternate for CR 5096(Removal of rejection functionality)
                            case ChunkStatus.Rejected: nbRejected++;
                                if (ifc.Inherited) nbRejectedInh++;
                                if (ifc.Mandatory) nbRejectedM++;
                                if (ifc.Inherited && ifc.Mandatory) nbRejectedInhM++;
                                break;
                             */
                            case ChunkStatus.Missing: nbMissing++;
                                if (ifc.Mandatory) nbMissingM++;
                                break;
                        }
                    }

                    sb.Append("</table>");
                    sb.Append("<br>");
                    #endregion

                    #region "Count"
                    int optional = 0;
                    int specific = 0;
                    int specificM = 0;

                    // Add summary for status counts
                    sb.Append("<table border='1'><tr><td colspan='7' style='background-color: #D0D0D0; font-size: 14; font-weight:bold'>Summary</td></tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='background-color: #D0D0D0'></td>");
                    sb.Append("<td colspan='2' style='background-color: #D0D0D0'><b>Total</b></td>");
                    sb.Append("<td colspan='2' style='background-color: #D0D0D0'><b>Inherited</b></td>");
                    sb.Append("<td colspan='2' style='background-color: #D0D0D0'><b>At current level</b></td>");
                    sb.Append("</tr>");

                    // Total chunks
                    sb.Append("<tr>");
                    sb.Append("<td style='background-color: #D0D0D0'><b>Total containers</b></td>");
                    sb.Append("<td colspan='2'>" + nbTotal.ToString() + "</td>");
                    sb.Append("<td colspan='2' style='background-color: #D0D0D0'></td>");
                    sb.Append("<td colspan='2' style='background-color: #D0D0D0'></td>");
                    sb.Append("</tr>");

                    // Mandatory chunks
                    sb.Append("<tr>");
                    sb.Append("<td style='background-color: #D0D0D0'><b>Mandatory containers</b></td>");
                    sb.Append("<td colspan='2'>" + nbMandatory.ToString() + "</td>");
                    sb.Append("<td colspan='2' style='background-color: #D0D0D0'></td>");
                    sb.Append("<td colspan='2' style='background-color: #D0D0D0'></td>");
                    sb.Append("</tr>");

                    // Final chunks
                    sb.Append("<tr>");
                    sb.Append("<td style='background-color: #D0D0D0'><font color='green'><b>Final</b></font></td>");
                    optional = nbFinal - nbFinalM;
                    sb.Append("<td colspan='2'>" + nbFinal.ToString() + " (mandatory: " + nbFinalM.ToString() + ", optional: " + optional.ToString() + ")</td>");
                    optional = nbFinalInh - nbFinalInhM;
                    sb.Append("<td colspan='2'>" + nbFinalInh.ToString() + " (mandatory: " + nbFinalInhM.ToString() + ", optional: " + optional.ToString() + ")</td>");
                    specific = nbFinal - nbFinalInh;
                    specificM = nbFinalM - nbFinalInhM;
                    optional = specific - specificM;
                    sb.Append("<td colspan='2'>" + specific.ToString() + " (mandatory: " + specificM.ToString() + ", optional: " + optional.ToString() + ")</td>");
                    sb.Append("</tr>");

                    // Draft chunks
                    sb.Append("<tr>");
                    sb.Append("<td style='background-color: #D0D0D0'><font color='orange'><b>Draft</b></font></td>");
                    optional = nbDraft - nbDraftM;
                    sb.Append("<td colspan='2'>" + nbDraft.ToString() + " (mandatory: " + nbDraftM.ToString() + ", optional: " + optional.ToString() + ")</td>");
                    optional = nbDraftInh - nbDraftInhM;
                    sb.Append("<td colspan='2'>" + nbDraftInh.ToString() + " (mandatory: " + nbDraftInhM.ToString() + ", optional: " + optional.ToString() + ")</td>");
                    specific = nbDraft - nbDraftInh;
                    specificM = nbDraftM - nbDraftInhM;
                    optional = specific - specificM;
                    sb.Append("<td colspan='2'>" + specific.ToString() + " (mandatory: " + specificM.ToString() + ", optional: " + optional.ToString() + ")</td>");
                    sb.Append("</tr>");

                    /* Alternate for CR 5096(Removal of rejection functionality)
                    // Rejected chunks
                    sb.Append("<tr>");
                    sb.Append("<td style='background-color: #D0D0D0'><font color='red'><b>Rejected</b></font></td>");
                    optional = nbRejected - nbRejectedM;
                    sb.Append("<td colspan='2'>" + nbRejected.ToString() + " (mandatory: " + nbRejectedM.ToString() + ", optional: " + optional.ToString() + ")</td>");
                    optional = nbRejectedInh - nbRejectedInhM;
                    sb.Append("<td colspan='2'>" + nbRejectedInh.ToString() + " (mandatory: " + nbRejectedInhM.ToString() + ", optional: " + optional.ToString() + ")</td>");
                    specific = nbRejected - nbRejectedInh;
                    specificM = nbRejectedM - nbRejectedInhM;
                    optional = specific - specificM;
                    sb.Append("<td colspan='2'>" + specific.ToString() + " (mandatory: " + specificM.ToString() + ", optional: " + optional.ToString() + ")</td>");
                    sb.Append("</tr>");
                     */

                    // Missing chunks
                    sb.Append("<tr>");
                    sb.Append("<td style='background-color: #D0D0D0'><font color='blue'><b>Missing</b></font></td>");
                    optional = nbMissing - nbMissingM;
                    sb.Append("<td colspan='2'>" + nbMissing.ToString() + " (mandatory: " + nbMissingM.ToString() + ", optional: " + optional.ToString() + ")</td>");
                    sb.Append("<td colspan='2' style='background-color: #D0D0D0'></td>");
                    sb.Append("<td colspan='2' style='background-color: #D0D0D0'></td>");
                    sb.Append("</tr>");
                    #endregion

                    sb.Append("</table><br>");

            #endregion



                    // Available Options
                    #region Available Options
                    if (dgUPC.Rows.Count > 0 && dgUPC.Rows.Count != null)
                    {
                        sb.Append("<table border='1'>");
                        sb.Append("<tr style='font-weight: bold; background-color: lightgrey'>");
                        sb.Append("<td>Available options</td>");
                        sb.Append("</tr></table>");
                        sb.Append("<br>");

                        sb.Append("<table border='1'>");
                        sb.Append("<tr style='font-weight: bold; background-color: lightgrey'>");

                        foreach (UltraGridColumn col in dgUPC.Columns)
                        {


                            if (!col.Hidden)
                            {

                                if (col.Key == "UPCCode" || col.Key == "OptionCode" || col.Key == "OptionDescription")
                                {
                                    sb.Append("<td>" + col.Header.Caption + "</td>");
                                }

                            }


                        }

                        sb.Append("</tr>");
                        string strExport2 = string.Empty;

                        for (int row = 0; row < dgUPC.Rows.Count; row++)
                        {
                            sb.Append("<tr>");
                            for (int col = 0; col < dgUPC.Columns.Count; col++)
                            {

                                if (!dgUPC.Columns[col].Hidden)
                                {


                                    if (dgUPC.Rows[row].Cells[col].Key == "UPCCode" || dgUPC.Rows[row].Cells[col].Key == "OptionCode" || dgUPC.Rows[row].Cells[col].Key == "OptionDescription")
                                    {
                                        strExport2 = (dgUPC.Rows[row].Cells[col].Value + "").Trim();
                                        sb.Append("<td>");
                                        sb.Append(strExport2);
                                        sb.Append("</td>");
                                    }
                                }
                            }
                            sb.Append("</tr>");
                        }

                        sb.Append("</table>");
                        sb.Append("<br>");
                    }
                    else
                    {
                        sb.Append("<table border='1'>");
                        sb.Append("<tr style='font-weight: bold; background-color: lightgrey'>");
                        sb.Append("<td>Available options</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td>None</td>");
                        sb.Append("</tr></table>");

                    }

                    #endregion
                    //Available Options
                    #region Excel File name

                    sb.Append("</body></html>");

                    string fileName = string.Empty;
                    if (item.Sku.Length > 0)
                    {
                        fileName += "[" + item.Sku + "] ";
                    }
                    fileName += item.Name;
                    if (inputForm != null)
                    {
                        fileName += " (" + inputForm.Name + ")";
                    }

                    // Handling Special Characters for the Bug# 71578
                    int j = 178;
                    char chSquare = (char)j;
                    int k = 179;
                    char chCube = (char)k;
                    int l = 130;
                    char chComma = (char)l;
                    int m = 132;
                    char chDoubleQuotes = (char)m;
                    int n = 153;
                    char chTM = (char)n;
                    int o = 169;
                    char chCopyright = (char)o;
                    int p = 174;
                    char chRegistered = (char)p;



                    fileName = fileName.Replace("/", "#").Replace("\\", "#").Replace(" ", "#").Replace(":", "#").Replace("*", "#").Replace("\"", "#").Replace("|", "#").Replace("<", "#").Replace(">", "#").Replace(".", "#").Replace(chSquare, '#').Replace(chComma, '#').Replace(chDoubleQuotes, '#').Replace(chTM, '#').Replace(chCopyright, '#').Replace(chRegistered, '#').Replace(chCube, '#');

                    // Handling Special Characters for the Bug# 71578



                    fileName += ".xls";
                    #endregion

                    string exportContent = sb.ToString();
                    page.Response.Clear();
                    page.Response.ClearContent();
                    page.Response.ClearHeaders();
                    page.Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                    page.Response.ContentType = "application/vnd.ms-excel";
                    //Fix for CR 5109 - Prabhu R S
                    page.Response.ContentEncoding = System.Text.Encoding.UTF8;
                    page.Response.AppendHeader("Content-Length", System.Text.Encoding.UTF8.GetByteCount(exportContent).ToString());
                    page.EnableViewState = false;
                    page.Response.Write(exportContent);
                    page.Response.End();
                }
            }

        }
        catch (Exception ex)
        {
            Exception exception = null;
            exception = ex;

        }

    }
    #endregion Export Content Tab


    //Overview Tab
    #region Overview Content Tab

    public static void ExportGridOfFrameOverview(System.Int64 itemId, string CountryCode, string fallback1CountryCode, string fallback2CountryCode, bool Localizable, bool SingleView, string filter, Page page, UltraWebGrid dg, UltraWebGrid dgUPC, UltraWebGrid dgCrossSell)
    {
        #region Declaration
        Item item = Item.GetByKey(itemId);
        int inputFormId = -1;
        InputForm inputForm = InputForm.GetByKey(inputFormId);

        //Market Segments
        string appMS = string.Empty;
        string appCountry = string.Empty;
        //Market Segments

        //Publisher
        string sPublishers = string.Empty;
        string sCountry = string.Empty;
        //Publisher

        //Content
        int Local = 0;
        int sView = 0;
        if (Localizable)
        {
            Local = 1;

        }

        if (SingleView)
        {
            sView = 1;
        }
        //Content

        #endregion

        System.Text.StringBuilder sb = new System.Text.StringBuilder(string.Empty);

        sb.Append("<html><head>");
        sb.Append("<meta http-equiv='Content-Type' content='text/html; charset=utf-8'>");
        sb.Append("</head><body>");

        // item name
        sb.Append("<table border='1'><tr><td colspan='2' style='font-size: 14; font-weight: bold; background-color: lightgrey'>Node name</td><td style='font-size: 12' colspan='5'>");
        if (item.Sku.Length > 0)
        {
            sb.Append("[");
            sb.Append(item.Sku);
            sb.Append("] ");
        }
        sb.Append(item.Name);
        sb.Append(" (#");
        sb.Append(item.Id);
        sb.Append(", ");
        sb.Append(item.Level.Name);
        sb.Append(")");
        sb.Append("</td></tr>");



        // Export by
        sb.Append("<tr><td colspan='2' style='font-size: 14; font-weight: bold; background-color: lightgrey'>Exported by</td><td colspan='5'>");
        sb.Append(SessionState.User.Email);
        sb.Append(" - <i>");
        sb.Append(SessionState.User.FormatUtcDate(DateTime.UtcNow, true, SessionState.User.FormatDate));
        sb.Append(" at ");
        sb.Append(SessionState.User.FormatUtcDate(DateTime.UtcNow, true, SessionState.User.FormatTime));
        sb.Append(" (");
        sb.Append(SessionState.CacheParams["AppName"].Value.ToString());
        sb.Append(")");
        sb.Append("</i></td></tr>");

        // List type
        sb.Append("<tr><td colspan='2' style='font-size: 14; font-weight: bold; background-color: lightgrey'>Container list</td><td colspan='5'>");
        if (filter.Length > 0)
            sb.Append("Partial");
        else
            sb.Append("Complete");

        sb.Append("</td></tr>");

        sb.Append("<tr><td colspan='2' style='font-size: 14; font-weight: bold; background-color: lightgrey'>Culture Code</td><td colspan='5'>");
        string Country = SessionState.Culture.Code;
        if (Country != string.Empty)
            sb.Append(Country);
        else
            sb.Append("None");

        sb.Append("</td></tr></table>");

        sb.Append("<br>");


        //PLC
        #region PLC
        try
        {
            PLC itemPLC = PLC.GetByKey(itemId, CountryCode);

            if (itemPLC != null)
            {
                sb.Append("<table border='1'>");
                sb.Append("<tr style='font-weight: bold; background-color: lightgrey'>");
                sb.Append("<td>Product Life Cycle</td>");
                sb.Append("</tr></table>");
                sb.Append("<br>");

                sb.Append("<table border='1'>");
                sb.Append("<tr style='font-weight: bold; background-color: lightgrey'>");
                sb.Append("<td>General Availability</td>");
                sb.Append("<td>End of Sales/Marketing</td>");
                sb.Append("<td>Selective Availability</td>");
                sb.Append("<td>Announcement Date</td>");
                sb.Append("<td>Removal Date</td>");
                sb.Append("<td>End Division Support Life</td>");
                sb.Append("<td>Planned End of Manufacturing</td>");
                sb.Append("<td>End of Manufacturing</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");

                sb.Append("<td>");
                sb.Append(GetProperShortDate(itemPLC.FullDate));
                sb.Append("</td>");


                sb.Append("<td>");
                sb.Append(GetProperShortDate(itemPLC.ObsoleteDate));
                sb.Append("</td>");


                sb.Append("<td>");
                sb.Append(GetProperShortDate(itemPLC.BlindDate));
                sb.Append("</td>");

                sb.Append("<td>");
                sb.Append(GetProperShortDate(itemPLC.AnnouncementDate));
                sb.Append("</td>");

                sb.Append("<td>");
                sb.Append(GetProperShortDate(itemPLC.RemovalDate));
                sb.Append("</td>");

                sb.Append("<td>");
                sb.Append(GetProperShortDate(itemPLC.EndOfSupportDate));
                sb.Append("</td>");


                sb.Append("<td>");
                sb.Append(GetProperShortDate(itemPLC.EndOfLifeDate));
                sb.Append("</td>");

                sb.Append("<td>");
                sb.Append(GetProperShortDate(itemPLC.DiscontinueDate));
                sb.Append("</td>");

                sb.Append("</tr></table>");
                sb.Append("<br>");

            }
            else
            {
                sb.Append("<table border='1'>");
                sb.Append("<tr style='font-weight: bold; background-color: lightgrey'>");
                sb.Append("<td>Product Life Cycle</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td>None</td>");
                sb.Append("</tr>");
                sb.Append("</table><br>");

            }
        }
        catch (Exception ex)
        {
            Exception exception = null;
            exception = ex;
        }
        #endregion
        //PLC

        //Market Segments
        //countryCode = SessionState.Culture.CountryCode;
        #region Market Segments

        try
        {
            using (ItemMarketSegmentList appliedItemMarketSegments = ItemMarketSegment.GetAppliedByItem(itemId, CountryCode))
            {

                if (appliedItemMarketSegments != null)
                {

                    sb.Append("<table border='1'>");
                    sb.Append("<tr style='font-weight: bold; background-color: lightgrey'>");
                    sb.Append("<td>Market Segments</td>");
                    sb.Append("</tr></table>");
                    sb.Append("<br>");


                    sb.Append("<table border='1'>");
                    sb.Append("<tr style='font-weight: bold; background-color: lightgrey'>");
                    sb.Append("<td colspan=2>Applied Market Segments</td></tr>");
                    sb.Append("<tr style='font-weight: bold; background-color: lightgrey'>");
                    sb.Append("<td>Segment</td>");
                    sb.Append("<td>Country</td>");
                    sb.Append("</tr>");

                    foreach (ItemMarketSegment ims in appliedItemMarketSegments)
                    {

                        if (ims.CountryCode == CountryCode)
                        {
                            appMS = ims.MarketSegment.Name;
                            appCountry = ims.CountryCode;
                        }
                        else if (ims.CountryCode == fallback1CountryCode)
                        {
                            appMS = ims.MarketSegment.Name;
                            appCountry = ims.CountryCode;

                        }
                        else if (ims.CountryCode == fallback2CountryCode)
                        {
                            appMS = ims.MarketSegment.Name;
                            appCountry = ims.CountryCode;

                        }


                        sb.Append("<tr><td>");
                        sb.Append(appMS);
                        sb.Append("</td>");

                        sb.Append("<td>");
                        sb.Append(appCountry);
                        sb.Append("</td></tr>");


                    }

                    sb.Append("</table>");
                    sb.Append("<br>");



                }
                else
                {


                    sb.Append("<table border='1'>");
                    sb.Append("<tr style='font-weight: bold; background-color: lightgrey'>");
                    sb.Append("<td>Market Segments</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr style='font-weight: bold; background-color: lightgrey'>");
                    sb.Append("<td>Applied Market Segments</td></tr>");
                    sb.Append("<tr><td>");
                    sb.Append("None");
                    sb.Append("</td></tr>");

                    sb.Append("</table>");
                    sb.Append("<br>");

                }



            }
        }
        catch (Exception ex)
        {
            Exception exception = null;
            exception = ex;
        }

        #endregion
        //Market Segments

        //Publisher
        #region Publisher
        try
        {
            using (ItemPublisherList ipl = ItemPublisher.GetPublishersByItemId(item.Id, CountryCode))
            {
                int pCount = ipl.Count;
                if (pCount > 0)
                {
                    if (ipl != null)
                    {
                        sb.Append("<table border='1'>");
                        sb.Append("<tr style='font-weight: bold; background-color: lightgrey'>");
                        sb.Append("<td>Exclusive Publishers</td>");
                        sb.Append("</tr></table>");
                        sb.Append("<br>");


                        sb.Append("<table border='1'>");
                        sb.Append("<tr style='font-weight: bold; background-color: lightgrey'>");
                        sb.Append("<td>Publisher</td>");
                        sb.Append("<td>Country</td>");
                        sb.Append("</tr>");


                        string name = string.Empty;
                        string regionCode = string.Empty;
                        foreach (ItemPublisher ip in ipl)
                        {
                            if (ip != null)
                            {
                                if (ip.Publisher != null)
                                {
                                    name = ip.Publisher.Name;
                                    regionCode = ip.CountryCode;


                                    if (regionCode == CountryCode)
                                    {
                                        sPublishers = name;
                                        sCountry = regionCode;
                                    }
                                    else if (regionCode == fallback1CountryCode)
                                    {
                                        sPublishers = name;
                                        sCountry = regionCode;
                                    }
                                    else if (regionCode == fallback2CountryCode)
                                    {
                                        sPublishers = name;
                                        sCountry = regionCode;
                                    }

                                }
                            }

                            sb.Append("<tr><td>");
                            sb.Append(sPublishers);
                            sb.Append("</td>");
                            sb.Append("<td>");
                            sb.Append(sCountry);
                            sb.Append("</td></tr>");
                        }

                        sb.Append("</tr></table>");
                    }

                }
                else
                {

                    sb.Append("<table border='1'>");
                    sb.Append("<tr style='font-weight: bold; background-color: lightgrey'>");
                    sb.Append("<td>Exclusive Publishers</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr><td>");
                    sb.Append("None");
                    sb.Append("</td></tr>");
                    sb.Append("</table>");
                    sb.Append("<br>");
                }
            }

        }
        catch (Exception ex)
        {
            Exception exception = null;
            exception = ex;
        }
        #endregion
        // Publisher

        //Containers 
        #region Containers
        try
        {

            if (item != null)
            {
                // Retrieve list of input form chunk
                InputFormChunkList chunkList = InputFormChunk.GetChunksByCountry(itemId, SessionState.Culture.Code, Local, sView);

                if (chunkList != null && chunkList.Count > 0)
                {
                    // apply filter if necessary
                    int i = chunkList.Count - 1;
                    while (i >= 0)
                    {
                        // mandatory view
                        InputFormChunk chunk = chunkList[i];
                        if (!chunk.Mandatory)
                        {
                            chunkList.RemoveAt(i);
                        }
                        else
                        {
                            if (Localizable && !chunk.Localizable)
                            {
                                chunkList.RemoveAt(i);
                            }
                            // filter (container name or value)
                            if (filter.Length > 0)
                            {
                                filter = filter.ToLower();
                                string contName = chunk.ContainerName.ToLower();
                                string chunkText = chunk.Text.ToLower();
                                if (!(contName.IndexOf(filter) > 0))
                                {
                                    if (!chunk.IsResource && !(chunkText.IndexOf(filter) > 0))
                                    {
                                        chunkList.RemoveAt(i);
                                    }
                                }
                            }
                        }
                        i--;
                    }


                    // List type

                    sb.Append("<table border='1'>");
                    sb.Append("<tr style='font-weight: bold; background-color: lightgrey'>");
                    sb.Append("<td>Contents</td>");
                    sb.Append("</tr></table>");
                    sb.Append("<br>");

                    #region "Column header"
                    sb.Append("<table border='1'>");
                    sb.Append("<tr style='font-weight: bold; background-color: lightgrey'>");
                    sb.Append("<td>Mandatory</td>");
                    sb.Append("<td>Inherited</td>");
                    sb.Append("<td>Read only</td>");
                    sb.Append("<td>Status</td>");
                    sb.Append("<td>Container name</td>");
                    sb.Append("<td>Value</td>");
                    sb.Append("<td>Comment</td>");
                    sb.Append("</tr>");
                    #endregion

                    #region "Rows"
                    /* Alternate for CR 5096(Removal of rejection functionality)--start
                    int nbMandatory, nbDraft, nbDraftInh, nbFinal, nbFinalInh, nbRejected, nbRejectedInh, nbMissing,
                      nbDraftM, nbFinalM, nbMissingM, nbRejectedM, nbDraftInhM, nbFinalInhM, nbRejectedInhM, nbTotal;
                    nbMandatory = nbDraft = nbDraftInh = nbFinal = nbFinalInh = nbRejected = nbRejectedInh = nbMissing = 0;
                    nbDraftM = nbFinalM = nbMissingM = nbRejectedM = nbDraftInhM = nbFinalInhM = nbRejectedInhM = nbTotal = 0;
                     */
                    int nbMandatory, nbDraft, nbDraftInh, nbFinal, nbFinalInh, nbMissing,
                      nbDraftM, nbFinalM, nbMissingM, nbDraftInhM, nbFinalInhM, nbTotal;
                    nbMandatory = nbDraft = nbDraftInh = nbFinal = nbFinalInh = nbMissing = 0;
                    nbDraftM = nbFinalM = nbMissingM = nbDraftInhM = nbFinalInhM = nbTotal = 0;
                    // Alternate for CR 5096(Removal of rejection functionality)--end

                    string path = string.Empty;
                    nbTotal = chunkList.Count;
                    foreach (InputFormChunk ifc in chunkList)
                    {
                        // add group
                        if (ifc.Path != path)
                        {
                            path = ifc.Path;
                            sb.Append("<tr><td style='font-weight:bold; color:white; background-color: #003366' colspan='7'>");
                            sb.Append(path);
                            sb.Append("</td></tr>");
                        }

                        sb.Append("<tr>");

                        // add mandatory or not
                        if (ifc.Mandatory)
                        {
                            sb.Append("<td align='center' style='background-color: #D0D0D0'><font size='5' color='red'>*</font></td>");
                            nbMandatory++; // update mandatory count
                        }
                        else
                            sb.Append("<td style='background-color: #D0D0D0'></td>");

                        // add inherited or not
                        if (ifc.Inherited)
                            sb.Append("<td style='background-color: #D0D0D0' align='center'><b>X</b></td>");
                        else
                            sb.Append("<td style='background-color: #D0D0D0'></td>");

                        // add read only container
                        if (ifc.ReadOnly)
                            sb.Append("<td style='background-color: #D0D0D0' align='center'><b>X</b></td>");
                        else
                            sb.Append("<td style='background-color: #D0D0D0'></td>");

                        // add status
                        string fontColor = string.Empty;
                        switch (ifc.Status)
                        {
                            case ChunkStatus.Draft: fontColor = "orange"; break;
                            case ChunkStatus.Final: fontColor = "green"; break;
                            //case ChunkStatus.Rejected: fontColor = "red"; break; --Commented for alternate for CR 5096
                            case ChunkStatus.Missing: fontColor = "blue"; break;
                        }
                        sb.Append("<td align='center' style='background-color: #D0D0D0'>");
                        sb.Append("<font color='");
                        sb.Append(fontColor);
                        sb.Append("'>");
                        sb.Append((ifc.Status.ToString())[0]);
                        sb.Append("</font>");
                        sb.Append("</td>");

                        // add container name
                        sb.Append("<td style='background-color: #D0D0D0'>");
                        sb.Append(ifc.ContainerName);
                        sb.Append("</td>");

                        // add value
                        if (ifc.Rtl)
                            sb.Append("<td style='direction: rtl'>"); // if Rtl
                        else
                            sb.Append("<td>");

                        if (ifc.IsBool)
                        {
                            if (ifc.Text == "1")
                                sb.Append("Yes");
                            else
                                sb.Append("No");
                        }
                        else if (ifc.Text == HyperCatalog.Business.Chunk.BlankValue)
                            sb.Append(HyperCatalog.Business.Chunk.BlankText);
                        else
                            sb.Append(ifc.Text);

                        sb.Append("</td>");

                        // Comment
                        sb.Append("<td>");
                        sb.Append(ifc.Comment);
                        sb.Append("</td>");

                        sb.Append("</tr>");

                        // Update count
                        switch (ifc.Status)
                        {
                            case ChunkStatus.Draft: nbDraft++;
                                if (ifc.Inherited) nbDraftInh++;
                                if (ifc.Mandatory) nbDraftM++;
                                if (ifc.Inherited && ifc.Mandatory) nbDraftInhM++;
                                break;
                            case ChunkStatus.Final: nbFinal++;
                                if (ifc.Inherited) nbFinalInh++;
                                if (ifc.Mandatory) nbFinalM++;
                                if (ifc.Inherited && ifc.Mandatory) nbFinalInhM++;
                                break;
                            /* Alternate for CR 5096(Removal of rejection functionality)
                            case ChunkStatus.Rejected: nbRejected++;
                                if (ifc.Inherited) nbRejectedInh++;
                                if (ifc.Mandatory) nbRejectedM++;
                                if (ifc.Inherited && ifc.Mandatory) nbRejectedInhM++;
                                break; */
                            case ChunkStatus.Missing: nbMissing++;
                                if (ifc.Mandatory) nbMissingM++;
                                break;
                        }
                    }

                    sb.Append("</table>");
                    sb.Append("<br>");
                    #endregion

                    #region "Count"
                    int optional = 0;
                    int specific = 0;
                    int specificM = 0;

                    // Add summary for status counts
                    sb.Append("<table border='1'><tr><td colspan='7' style='background-color: #D0D0D0; font-size: 14; font-weight:bold'>Summary</td></tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='background-color: #D0D0D0'></td>");
                    sb.Append("<td colspan='2' style='background-color: #D0D0D0'><b>Total</b></td>");
                    sb.Append("<td colspan='2' style='background-color: #D0D0D0'><b>Inherited</b></td>");
                    sb.Append("<td colspan='2' style='background-color: #D0D0D0'><b>At current level</b></td>");
                    sb.Append("</tr>");

                    // Total chunks
                    sb.Append("<tr>");
                    sb.Append("<td style='background-color: #D0D0D0'><b>Total containers</b></td>");
                    sb.Append("<td colspan='2'>" + nbTotal.ToString() + "</td>");
                    sb.Append("<td colspan='2' style='background-color: #D0D0D0'></td>");
                    sb.Append("<td colspan='2' style='background-color: #D0D0D0'></td>");
                    sb.Append("</tr>");

                    // Mandatory chunks
                    sb.Append("<tr>");
                    sb.Append("<td style='background-color: #D0D0D0'><b>Mandatory containers</b></td>");
                    sb.Append("<td colspan='2'>" + nbMandatory.ToString() + "</td>");
                    sb.Append("<td colspan='2' style='background-color: #D0D0D0'></td>");
                    sb.Append("<td colspan='2' style='background-color: #D0D0D0'></td>");
                    sb.Append("</tr>");

                    // Final chunks
                    sb.Append("<tr>");
                    sb.Append("<td style='background-color: #D0D0D0'><font color='green'><b>Final</b></font></td>");
                    optional = nbFinal - nbFinalM;
                    sb.Append("<td colspan='2'>" + nbFinal.ToString() + " (mandatory: " + nbFinalM.ToString() + ", optional: " + optional.ToString() + ")</td>");
                    optional = nbFinalInh - nbFinalInhM;
                    sb.Append("<td colspan='2'>" + nbFinalInh.ToString() + " (mandatory: " + nbFinalInhM.ToString() + ", optional: " + optional.ToString() + ")</td>");
                    specific = nbFinal - nbFinalInh;
                    specificM = nbFinalM - nbFinalInhM;
                    optional = specific - specificM;
                    sb.Append("<td colspan='2'>" + specific.ToString() + " (mandatory: " + specificM.ToString() + ", optional: " + optional.ToString() + ")</td>");
                    sb.Append("</tr>");

                    // Draft chunks
                    sb.Append("<tr>");
                    sb.Append("<td style='background-color: #D0D0D0'><font color='orange'><b>Draft</b></font></td>");
                    optional = nbDraft - nbDraftM;
                    sb.Append("<td colspan='2'>" + nbDraft.ToString() + " (mandatory: " + nbDraftM.ToString() + ", optional: " + optional.ToString() + ")</td>");
                    optional = nbDraftInh - nbDraftInhM;
                    sb.Append("<td colspan='2'>" + nbDraftInh.ToString() + " (mandatory: " + nbDraftInhM.ToString() + ", optional: " + optional.ToString() + ")</td>");
                    specific = nbDraft - nbDraftInh;
                    specificM = nbDraftM - nbDraftInhM;
                    optional = specific - specificM;
                    sb.Append("<td colspan='2'>" + specific.ToString() + " (mandatory: " + specificM.ToString() + ", optional: " + optional.ToString() + ")</td>");
                    sb.Append("</tr>");

                    /* Alternate for CR 5096(Removal of rejection functionality)
                    // Rejected chunks
                    sb.Append("<tr>");
                    sb.Append("<td style='background-color: #D0D0D0'><font color='red'><b>Rejected</b></font></td>");
                    optional = nbRejected - nbRejectedM;
                    sb.Append("<td colspan='2'>" + nbRejected.ToString() + " (mandatory: " + nbRejectedM.ToString() + ", optional: " + optional.ToString() + ")</td>");
                    optional = nbRejectedInh - nbRejectedInhM;
                    sb.Append("<td colspan='2'>" + nbRejectedInh.ToString() + " (mandatory: " + nbRejectedInhM.ToString() + ", optional: " + optional.ToString() + ")</td>");
                    specific = nbRejected - nbRejectedInh;
                    specificM = nbRejectedM - nbRejectedInhM;
                    optional = specific - specificM;
                    sb.Append("<td colspan='2'>" + specific.ToString() + " (mandatory: " + specificM.ToString() + ", optional: " + optional.ToString() + ")</td>");
                    sb.Append("</tr>");
                     */

                    // Missing chunks
                    sb.Append("<tr>");
                    sb.Append("<td style='background-color: #D0D0D0'><font color='blue'><b>Missing</b></font></td>");
                    optional = nbMissing - nbMissingM;
                    sb.Append("<td colspan='2'>" + nbMissing.ToString() + " (mandatory: " + nbMissingM.ToString() + ", optional: " + optional.ToString() + ")</td>");
                    sb.Append("<td colspan='2' style='background-color: #D0D0D0'></td>");
                    sb.Append("<td colspan='2' style='background-color: #D0D0D0'></td>");
                    sb.Append("</tr>");
                    #endregion

                    sb.Append("</table><br>");



                }
            }
        }
        catch (Exception ex)
        {
            Exception exception;
            exception = ex;

        }
        #endregion

        //Content

        // Available Options
        #region Available Options
        try
        {

            if (dgUPC.Rows.Count > 0 && dgUPC.Rows.Count != null)
            {
                sb.Append("<table border='1'>");
                sb.Append("<tr style='font-weight: bold; background-color: lightgrey'>");
                sb.Append("<td>Available options</td>");
                sb.Append("</tr></table>");
                sb.Append("<br>");

                sb.Append("<table border='1'>");
                sb.Append("<tr style='font-weight: bold; background-color: lightgrey'>");

                foreach (UltraGridColumn col in dgUPC.Columns)
                {


                    if (!col.Hidden)
                    {

                        if (col.Key == "UPCCode" || col.Key == "OptionCode" || col.Key == "OptionDescription")
                        {
                            sb.Append("<td>" + col.Header.Caption + "</td>");
                        }

                    }


                }

                sb.Append("</tr>");
                string strExport2 = string.Empty;

                for (int row = 0; row < dgUPC.Rows.Count; row++)
                {
                    sb.Append("<tr>");
                    for (int col = 0; col < dgUPC.Columns.Count; col++)
                    {

                        if (!dgUPC.Columns[col].Hidden)
                        {


                            if (dgUPC.Rows[row].Cells[col].Key == "UPCCode" || dgUPC.Rows[row].Cells[col].Key == "OptionCode" || dgUPC.Rows[row].Cells[col].Key == "OptionDescription")
                            {
                                strExport2 = (dgUPC.Rows[row].Cells[col].Value + "").Trim();
                                sb.Append("<td>");
                                sb.Append(strExport2);
                                sb.Append("</td>");
                            }
                        }
                    }
                    sb.Append("</tr>");
                }

                sb.Append("</table>");
                sb.Append("<br>");
            }
            else
            {
                sb.Append("<table border='1'>");
                sb.Append("<tr style='font-weight: bold; background-color: lightgrey'>");
                sb.Append("<td>Available options</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td>None</td>");
                sb.Append("</tr></table><br>");

            }
        }
        catch (Exception ex)
        {
            Exception exception = null;
            exception = ex;
        }
        #endregion
        //Available Options

        //Links
        #region Links
        try
        {
            if (dg.Rows.Count > 0 && dg.Rows.Count != null)
            {
                sb.Append("<table border='1'>");
                sb.Append("<tr style='font-weight: bold; background-color: lightgrey'>");
                sb.Append("<td>Links</td>");
                sb.Append("</tr></table>");
                sb.Append("<br>");

                sb.Append("<table border='1'>");
                sb.Append("<tr style='font-weight: bold; background-color: lightgrey'>");
                foreach (UltraGridColumn col in dg.Columns)
                {
                    if (!col.Hidden)
                    {
                        if (col.Key == "Class" || col.Key == "SKU" || col.Key == "Name")
                        {
                            sb.Append("<td>" + col.Header.Caption + "</td>");
                        }

                    }

                }

                sb.Append("</tr>");
                string strExport = string.Empty;
                string cellStyle = string.Empty;


                for (int row = 0; row < dg.Rows.Count; row++)
                {
                    sb.Append("<tr>");
                    for (int col = 0; col < dg.Columns.Count; col++)
                    {

                        if (!dg.Columns[col].Hidden)
                        {
                            if (dg.Rows[row].Cells[col].Key == "Class" || dg.Rows[row].Cells[col].Key == "SKU" || dg.Rows[row].Cells[col].Key == "Name")
                            {
                                strExport = (dg.Rows[row].Cells[col].Value + "").Trim();
                                sb.Append("<td>");
                                sb.Append(strExport);
                                sb.Append("</td>");
                            }
                        }
                    }
                    sb.Append("</tr>");

                }
                sb.Append("</table><br>");
            }

            else
            {
                sb.Append("<table border='1'>");
                sb.Append("<tr style='font-weight: bold; background-color: lightgrey'>");
                sb.Append("<td>Links</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td>None</td>");
                sb.Append("</tr></table>");
                sb.Append("<br>");
            }
        }
        catch (Exception ex)
        {
            Exception exception = null;
            exception = ex;
        }
        #endregion
        //Links

        //Cross Sell
        #region Cross Sell
        try
        {


            if (dgCrossSell.Rows.Count > 0 && dgCrossSell.Rows.Count != null)
            {
                sb.Append("<table border='1'>");
                sb.Append("<tr style='font-weight: bold; background-color: lightgrey'>");
                sb.Append("<td>Cross Sell</td>");
                sb.Append("</tr></table>");
                sb.Append("<br>");

                sb.Append("<table border='1'>");
                sb.Append("<tr style='font-weight: bold; background-color: lightgrey'>");

                foreach (UltraGridColumn col in dgCrossSell.Columns)
                {


                    if (!col.Hidden)
                    {

                        if (col.Key == "Class" || col.Key == "Family" || col.Key == "ItemSku" || col.Key == "ItemName")
                        {
                            sb.Append("<td>" + col.Header.Caption + "</td>");
                        }

                    }


                }

                sb.Append("</tr>");
                string strExport1 = string.Empty;

                for (int row = 0; row < dgCrossSell.Rows.Count; row++)
                {
                    sb.Append("<tr>");
                    for (int col = 0; col < dgCrossSell.Columns.Count; col++)
                    {

                        if (!dgCrossSell.Columns[col].Hidden)
                        {


                            if (dgCrossSell.Rows[row].Cells[col].Key == "Class" || dgCrossSell.Rows[row].Cells[col].Key == "Family" || dgCrossSell.Rows[row].Cells[col].Key == "ItemSku" || dgCrossSell.Rows[row].Cells[col].Key == "ItemName")
                            {
                                strExport1 = (dgCrossSell.Rows[row].Cells[col].Value + "").Trim();
                                sb.Append("<td>");
                                sb.Append(strExport1);
                                sb.Append("</td>");
                            }
                        }
                    }
                    sb.Append("</tr>");
                }

                sb.Append("</table>");
            }
            else
            {
                sb.Append("<table border='1'>");
                sb.Append("<tr style='font-weight: bold; background-color: lightgrey'>");
                sb.Append("<td>Cross Sell</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td>None</td>");
                sb.Append("</tr></table>");

            }
        }
        catch (Exception ex)
        {
            Exception exception = null;
            exception = ex;
        }
        #endregion
        //Cross Sell


        sb.Append("</body></html>");
        #region Excel File Name
        string fileName = string.Empty;
        if (item.Sku.Length > 0)
        {
            fileName += "[" + item.Sku + "] ";
        }
        fileName += item.Name;
        if (inputForm != null)
        {
            fileName += " (" + inputForm.Name + ")";
        }

        fileName += " - " + Country;

        // Handling Special Characters for the Bug# 71578
        int j = 178;
        char chSquare = (char)j;
        int k = 179;
        char chCube = (char)k;
        int l = 130;
        char chComma = (char)l;
        int m = 132;
        char chDoubleQuotes = (char)m;
        int n = 153;
        char chTM = (char)n;
        int o = 169;
        char chCopyright = (char)o;
        int p = 174;
        char chRegistered = (char)p;



        fileName = fileName.Replace("/", "#").Replace("\\", "#").Replace(" ", "#").Replace(":", "#").Replace("*", "#").Replace("\"", "#").Replace("|", "#").Replace("<", "#").Replace(">", "#").Replace(".", "#").Replace(chSquare, '#').Replace(chComma, '#').Replace(chDoubleQuotes, '#').Replace(chTM, '#').Replace(chCopyright, '#').Replace(chRegistered, '#').Replace(chCube, '#');

        // Handling Special Characters for the Bug# 71578



        fileName += ".xls";
        #endregion

        string exportContent = sb.ToString();
        page.Response.Clear();
        page.Response.ClearContent();
        page.Response.ClearHeaders();
        page.Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
        page.Response.ContentType = "application/vnd.ms-excel";
        //Fix for CR 5109 - Prabhu R S
        page.Response.ContentEncoding = System.Text.Encoding.UTF8;
        page.Response.AppendHeader("Content-Length", System.Text.Encoding.UTF8.GetByteCount(exportContent).ToString());

        page.EnableViewState = false;
        page.Response.Write(exportContent);
        page.Response.End();



    }



    #endregion


    // Added for Enhancement for the BUG#70135





    public static void ExportGridOfFrameContent(System.Int64 itemId, int inputFormId, bool viewMandatory, bool viewRegionalizable, string filter, Page page)
    {
      Item item = Item.GetByKey(itemId);
      InputForm inputForm = InputForm.GetByKey(inputFormId);
      if (item != null)
      {
        // Retrieve list of input form chunk
        InputFormChunkList chunkList = InputFormChunk.GetByInputForm(itemId, inputFormId, SessionState.Culture.Code);

        if (chunkList != null && chunkList.Count > 0)
        {
          // apply filter if necessary
          int i = chunkList.Count - 1;
          while (i >= 0)
          {
            // mandatory view
            InputFormChunk chunk = chunkList[i];
            if (viewMandatory && !chunk.Mandatory)
            {
              chunkList.RemoveAt(i);
            }
            else
            {
              if (viewRegionalizable && !chunk.Regionalizable)
              {
                chunkList.RemoveAt(i);
              }
              // filter (container name or value)
              if (filter.Length > 0)
              {
                filter = filter.ToLower();
                string contName = chunk.ContainerName.ToLower();
                string chunkText = chunk.Text.ToLower();
                if (!(contName.IndexOf(filter) > 0))
                {
                  if (!chunk.IsResource && !(chunkText.IndexOf(filter) > 0))
                  {
                    chunkList.RemoveAt(i);
                  }
                }
              }
            }
            i--;
          }

          // string contains html code to export
          System.Text.StringBuilder sb = new System.Text.StringBuilder(string.Empty);

          sb.Append("<html><head>");
          sb.Append("<meta http-equiv='Content-Type' content='text/html; charset=utf-8'>");
          sb.Append("</head><body>");

          #region "Header"
          // item name
          sb.Append("<table border='1'><tr><td colspan='2' style='font-size: 14; font-weight: bold; background-color: lightgrey'>Node name</td><td style='font-size: 12' colspan='5'>");
          if (item.Sku.Length > 0)
          {
            sb.Append("[");
            sb.Append(item.Sku);
            sb.Append("] ");
          }
          sb.Append(item.Name);
          sb.Append(" (#");
          sb.Append(item.Id);
          sb.Append(", ");
          sb.Append(item.Level.Name);
          sb.Append(")");
          sb.Append("</td></tr>");

          // Input form name
          if (inputForm != null)
          {
            sb.Append("<tr><td colspan='2' style='font-size: 14; font-weight: bold; background-color: lightgrey'>Input form name</td><td style='font-size: 12' colspan='5'>");
            sb.Append(inputForm.Name);
            sb.Append(", Total: ");
            sb.Append(inputForm.Containers.Count);
            sb.Append("   Mandatory: ");
            int nbMandatoryChunks = 0;
            foreach (InputFormContainer ifc in inputForm.Containers)
            {
              if (ifc.Mandatory)
                nbMandatoryChunks++;
            }
            sb.Append(nbMandatoryChunks.ToString());
            sb.Append("</td></tr>");
          }

          // Export by
          sb.Append("<tr><td colspan='2' style='font-size: 14; font-weight: bold; background-color: lightgrey'>Exported by</td><td colspan='5'>");
          sb.Append(SessionState.User.Email);
          sb.Append(" - <i>");
          sb.Append(SessionState.User.FormatUtcDate(DateTime.UtcNow, true, SessionState.User.FormatDate));
          sb.Append(" at ");
          sb.Append(SessionState.User.FormatUtcDate(DateTime.UtcNow, true, SessionState.User.FormatTime));
          sb.Append(" (");
          sb.Append(SessionState.CacheParams["AppName"].Value.ToString());
          sb.Append(")");
          sb.Append("</i></td></tr>");

          // List type
          sb.Append("<tr><td colspan='2' style='font-size: 14; font-weight: bold; background-color: lightgrey'>Container list</td><td colspan='5'>");
          if (filter.Length > 0)
            sb.Append("Partial");
          else if (viewMandatory)
            sb.Append("Mandatory only");
          else
            sb.Append("Complete");
          sb.Append("</td></tr></table>");

          sb.Append("<br>");
          #endregion

          #region "Column header"
          sb.Append("<table border='1'>");
          sb.Append("<tr style='font-weight: bold; background-color: lightgrey'>");
          sb.Append("<td>Mandatory</td>");
          sb.Append("<td>Inherited</td>");
          sb.Append("<td>Read only</td>");
          sb.Append("<td>Status</td>");
          sb.Append("<td>Container name</td>");
          sb.Append("<td>Value</td>");
          sb.Append("<td>Comment</td>");
          sb.Append("</tr>");
          #endregion

          #region "Rows"
          /* Alternate for CR 5096(Removal of rejection functionality)--start
          int nbMandatory, nbDraft, nbDraftInh, nbFinal, nbFinalInh, nbRejected, nbRejectedInh, nbMissing,
            nbDraftM, nbFinalM, nbMissingM, nbRejectedM, nbDraftInhM, nbFinalInhM, nbRejectedInhM, nbTotal;
          nbMandatory = nbDraft = nbDraftInh = nbFinal = nbFinalInh = nbRejected = nbRejectedInh = nbMissing = 0;
          nbDraftM = nbFinalM = nbMissingM = nbRejectedM = nbDraftInhM = nbFinalInhM = nbRejectedInhM = nbTotal = 0;
           */
          int nbMandatory, nbDraft, nbDraftInh, nbFinal, nbFinalInh, nbMissing,
            nbDraftM, nbFinalM, nbMissingM, nbDraftInhM, nbFinalInhM, nbTotal;
          nbMandatory = nbDraft = nbDraftInh = nbFinal = nbFinalInh = nbMissing = 0;
          nbDraftM = nbFinalM = nbMissingM = nbDraftInhM = nbFinalInhM = nbTotal = 0;
          //Alternate for CR 5096(Removal of rejection functionality)--end

          string path = string.Empty;
          nbTotal = chunkList.Count;
          foreach (InputFormChunk ifc in chunkList)
          {
            // add group
            if (ifc.Path != path)
            {
              path = ifc.Path;
              sb.Append("<tr><td style='font-weight:bold; color:white; background-color: #003366' colspan='7'>");
              sb.Append(path);
              sb.Append("</td></tr>");
            }

            sb.Append("<tr>");

            // add mandatory or not
            if (ifc.Mandatory)
            {
              sb.Append("<td align='center' style='background-color: #D0D0D0'><font size='5' color='red'>*</font></td>");
              nbMandatory++; // update mandatory count
            }
            else
              sb.Append("<td style='background-color: #D0D0D0'></td>");

            // add inherited or not
            if (ifc.Inherited)
              sb.Append("<td style='background-color: #D0D0D0' align='center'><b>X</b></td>");
            else
              sb.Append("<td style='background-color: #D0D0D0'></td>");

            // add read only container
            if (ifc.ReadOnly)
              sb.Append("<td style='background-color: #D0D0D0' align='center'><b>X</b></td>");
            else
              sb.Append("<td style='background-color: #D0D0D0'></td>");

            // add status
            string fontColor = string.Empty;
            switch (ifc.Status)
            {
              case ChunkStatus.Draft: fontColor = "orange"; break;
              case ChunkStatus.Final: fontColor = "green"; break;
              //case ChunkStatus.Rejected: fontColor = "red"; break; --Commented for alternate for CR 5096
              case ChunkStatus.Missing: fontColor = "blue"; break;
            }
            sb.Append("<td align='center' style='background-color: #D0D0D0'>");
            sb.Append("<font color='");
            sb.Append(fontColor);
            sb.Append("'>");
            sb.Append((ifc.Status.ToString())[0]);
            sb.Append("</font>");
            sb.Append("</td>");

            // add container name
            sb.Append("<td style='background-color: #D0D0D0'>");
            sb.Append(ifc.ContainerName);
            sb.Append("</td>");

            // add value
            if (ifc.Rtl)
              sb.Append("<td style='direction: rtl'>"); // if Rtl
            else
              sb.Append("<td>");

            if (ifc.IsBool)
            {
              if (ifc.Text == "1")
                sb.Append("Yes");
              else
                sb.Append("No");
            }
            else if (ifc.Text == HyperCatalog.Business.Chunk.BlankValue)
              sb.Append(HyperCatalog.Business.Chunk.BlankText);
            else
              sb.Append(ifc.Text);

            sb.Append("</td>");

            // Comment
            sb.Append("<td>");
            sb.Append(ifc.Comment);
            sb.Append("</td>");

            sb.Append("</tr>");

            // Update count
            switch (ifc.Status)
            {
              case ChunkStatus.Draft: nbDraft++;
                if (ifc.Inherited) nbDraftInh++;
                if (ifc.Mandatory) nbDraftM++;
                if (ifc.Inherited && ifc.Mandatory) nbDraftInhM++;
                break;
              case ChunkStatus.Final: nbFinal++;
                if (ifc.Inherited) nbFinalInh++;
                if (ifc.Mandatory) nbFinalM++;
                if (ifc.Inherited && ifc.Mandatory) nbFinalInhM++;
                break;
              /* Alternate for CR 5096(Removal of rejection functionality)
              case ChunkStatus.Rejected: nbRejected++;
                if (ifc.Inherited) nbRejectedInh++;
                if (ifc.Mandatory) nbRejectedM++;
                if (ifc.Inherited && ifc.Mandatory) nbRejectedInhM++;
                break; */
              case ChunkStatus.Missing: nbMissing++;
                if (ifc.Mandatory) nbMissingM++;
                break;
            }
          }

          sb.Append("</table>");
          sb.Append("<br>");
          #endregion

          #region "Count"
          int optional = 0;
          int specific = 0;
          int specificM = 0;

          // Add summary for status counts
          sb.Append("<table border='1'><tr><td colspan='7' style='background-color: #D0D0D0; font-size: 14; font-weight:bold'>Summary</td></tr>");
          sb.Append("<tr>");
          sb.Append("<td style='background-color: #D0D0D0'></td>");
          sb.Append("<td colspan='2' style='background-color: #D0D0D0'><b>Total</b></td>");
          sb.Append("<td colspan='2' style='background-color: #D0D0D0'><b>Inherited</b></td>");
          sb.Append("<td colspan='2' style='background-color: #D0D0D0'><b>At current level</b></td>");
          sb.Append("</tr>");

          // Total chunks
          sb.Append("<tr>");
          sb.Append("<td style='background-color: #D0D0D0'><b>Total containers</b></td>");
          sb.Append("<td colspan='2'>" + nbTotal.ToString() + "</td>");
          sb.Append("<td colspan='2' style='background-color: #D0D0D0'></td>");
          sb.Append("<td colspan='2' style='background-color: #D0D0D0'></td>");
          sb.Append("</tr>");

          // Mandatory chunks
          sb.Append("<tr>");
          sb.Append("<td style='background-color: #D0D0D0'><b>Mandatory containers</b></td>");
          sb.Append("<td colspan='2'>" + nbMandatory.ToString() + "</td>");
          sb.Append("<td colspan='2' style='background-color: #D0D0D0'></td>");
          sb.Append("<td colspan='2' style='background-color: #D0D0D0'></td>");
          sb.Append("</tr>");

          // Final chunks
          sb.Append("<tr>");
          sb.Append("<td style='background-color: #D0D0D0'><font color='green'><b>Final</b></font></td>");
          optional = nbFinal - nbFinalM;
          sb.Append("<td colspan='2'>" + nbFinal.ToString() + " (mandatory: " + nbFinalM.ToString() + ", optional: " + optional.ToString() + ")</td>");
          optional = nbFinalInh - nbFinalInhM;
          sb.Append("<td colspan='2'>" + nbFinalInh.ToString() + " (mandatory: " + nbFinalInhM.ToString() + ", optional: " + optional.ToString() + ")</td>");
          specific = nbFinal - nbFinalInh;
          specificM = nbFinalM - nbFinalInhM;
          optional = specific - specificM;
          sb.Append("<td colspan='2'>" + specific.ToString() + " (mandatory: " + specificM.ToString() + ", optional: " + optional.ToString() + ")</td>");
          sb.Append("</tr>");

          // Draft chunks
          sb.Append("<tr>");
          sb.Append("<td style='background-color: #D0D0D0'><font color='orange'><b>Draft</b></font></td>");
          optional = nbDraft - nbDraftM;
          sb.Append("<td colspan='2'>" + nbDraft.ToString() + " (mandatory: " + nbDraftM.ToString() + ", optional: " + optional.ToString() + ")</td>");
          optional = nbDraftInh - nbDraftInhM;
          sb.Append("<td colspan='2'>" + nbDraftInh.ToString() + " (mandatory: " + nbDraftInhM.ToString() + ", optional: " + optional.ToString() + ")</td>");
          specific = nbDraft - nbDraftInh;
          specificM = nbDraftM - nbDraftInhM;
          optional = specific - specificM;
          sb.Append("<td colspan='2'>" + specific.ToString() + " (mandatory: " + specificM.ToString() + ", optional: " + optional.ToString() + ")</td>");
          sb.Append("</tr>");

          /* Alternate for CR 5096(Removal of rejection functionality)
          // Rejected chunks
          sb.Append("<tr>");
          sb.Append("<td style='background-color: #D0D0D0'><font color='red'><b>Rejected</b></font></td>");
          optional = nbRejected - nbRejectedM;
          sb.Append("<td colspan='2'>" + nbRejected.ToString() + " (mandatory: " + nbRejectedM.ToString() + ", optional: " + optional.ToString() + ")</td>");
          optional = nbRejectedInh - nbRejectedInhM;
          sb.Append("<td colspan='2'>" + nbRejectedInh.ToString() + " (mandatory: " + nbRejectedInhM.ToString() + ", optional: " + optional.ToString() + ")</td>");
          specific = nbRejected - nbRejectedInh;
          specificM = nbRejectedM - nbRejectedInhM;
          optional = specific - specificM;
          sb.Append("<td colspan='2'>" + specific.ToString() + " (mandatory: " + specificM.ToString() + ", optional: " + optional.ToString() + ")</td>");
          sb.Append("</tr>");
           */

          // Missing chunks
          sb.Append("<tr>");
          sb.Append("<td style='background-color: #D0D0D0'><font color='blue'><b>Missing</b></font></td>");
          optional = nbMissing - nbMissingM;
          sb.Append("<td colspan='2'>" + nbMissing.ToString() + " (mandatory: " + nbMissingM.ToString() + ", optional: " + optional.ToString() + ")</td>");
          sb.Append("<td colspan='2' style='background-color: #D0D0D0'></td>");
          sb.Append("<td colspan='2' style='background-color: #D0D0D0'></td>");
          sb.Append("</tr>");
          #endregion

          sb.Append("</table>");

          sb.Append("</body></html>");

          string fileName = string.Empty;
          if (item.Sku.Length > 0)
          {
            fileName += "[" + item.Sku + "] ";
          }
          fileName += item.Name;
          if (inputForm != null)
          {
            fileName += " (" + inputForm.Name + ")";
          }
          fileName += ".xls";

          string exportContent = sb.ToString();
          page.Response.Clear();
          page.Response.ClearContent();
          page.Response.ClearHeaders();
          page.Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
          page.Response.ContentType = "application/vnd.ms-excel";
          //Fix for CR 5109 - Prabhu R S
          page.Response.ContentEncoding = System.Text.Encoding.UTF8;
          page.Response.AppendHeader("Content-Length", System.Text.Encoding.UTF8.GetByteCount(exportContent).ToString());

          page.EnableViewState = false;
          page.Response.Write(exportContent);
          page.Response.End();
        }
      }
    }

    /// <summary>
    /// Export the data in Excel file
    /// </summary>
    /// <param name="ds">DataSet object</param>
    /// <param name="page">Current page</param>
    public static void ExportReportOfMoveStatus(DataSet ds, Int64 itemId, int inputFormId, Page page)
    {
      using (Item item = Item.GetByKey(itemId))
      {
        using (InputForm inputForm = InputForm.GetByKey(inputFormId))
        {
          if (item != null)
          {
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
              // string contains html code to export
              System.Text.StringBuilder sb = new System.Text.StringBuilder(string.Empty);

              sb.Append("<html><head>");
              sb.Append("<meta http-equiv='Content-Type' content='text/html; charset=utf-8'>");
              sb.Append("</head><body>");

              #region "Header"
              // item name
              sb.Append("<table border='1'><tr><td colspan='2' style='font-size: 14; font-weight: bold; background-color: lightgrey'>Name</td><td style='font-size: 12' colspan='5'>");
              if (item.Sku.Length > 0)
              {
                sb.Append("[");
                sb.Append(item.Sku);
                sb.Append("] ");
              }
              sb.Append(item.Name);
              sb.Append(" (#");
              sb.Append(item.Id);
              sb.Append(", ");
              sb.Append(item.Level.Name);
              sb.Append(")");
              sb.Append("</td></tr>");

              // Input form name
              if (inputForm != null)
              {
                sb.Append("<tr><td colspan='2' style='font-size: 14; font-weight: bold; background-color: lightgrey'>Input form name</td><td style='font-size: 12' colspan='5'>");
                sb.Append(inputForm.Name);
                sb.Append("</td></tr>");
              }

              // Export by
              sb.Append("<tr><td colspan='2' style='font-size: 14; font-weight: bold; background-color: lightgrey'>Exported by</td><td colspan='5'>");
              sb.Append(SessionState.User.Email);
              sb.Append(" - <i>");
              sb.Append(SessionState.User.FormatUtcDate(DateTime.UtcNow, true, SessionState.User.FormatDate));
              sb.Append(" at ");
              sb.Append(SessionState.User.FormatUtcDate(DateTime.UtcNow, true, SessionState.User.FormatTime));
              sb.Append(" (");
              sb.Append(SessionState.CacheParams["AppName"].Value.ToString());
              sb.Append(")");
              sb.Append("</i></td></tr>");

              sb.Append("</table>");
              sb.Append("<br>");
              #endregion

              #region "Column header"
              sb.Append("<table border='1'>");
              sb.Append("<tr style='font-weight: bold; background-color: lightgrey'>");
              sb.Append("<td>Item name</td>");
              sb.Append("<td>Container group</td>");
              sb.Append("<td>Container name</td>");
              sb.Append("<td>Culture name</td>");
              sb.Append("<td>Status</td>");
              sb.Append("<td>Value</td>");
              sb.Append("</tr>");
              #endregion

              #region "Rows"
              foreach (DataRow r in ds.Tables[0].Rows)
              {
                Int64 currentItemId = -1;
                int currentContainerId = -1;
                string currentCultureCode = string.Empty;
                if (r["ItemId"] != null)
                  currentItemId = Convert.ToInt64(r["ItemId"]);
                if (r["ItemId"] != null)
                  currentContainerId = Convert.ToInt32(r["ContainerId"]);
                if (r["ItemId"] != null)
                  currentCultureCode = r["CultureCode"].ToString();

                sb.Append("<tr>");

                // add item name
                sb.Append("<td style='background-color: #D0D0D0'><b>");
                sb.Append(r["ItemName"].ToString());
                sb.Append("</b></td>");

                // add container group name
                sb.Append("<td style='background-color: #D0D0D0'>");
                sb.Append(r["ContainerGroup"].ToString());
                sb.Append("</td>");
                // add container name
                sb.Append("<td style='background-color: #D0D0D0'>");
                sb.Append(r["ContainerName"].ToString());
                sb.Append("</td>");

                // add culture
                sb.Append("<td style='background-color: #D0D0D0'>");
                sb.Append(r["CultureCode"].ToString());
                sb.Append("</td>");

                // add status
                string fontColor = string.Empty;
                ChunkStatus status = Chunk.GetStatusFromString(r["ChunkStatus"].ToString());
                switch (status)
                {
                  case ChunkStatus.Draft: fontColor = "orange"; break;
                  case ChunkStatus.Final: fontColor = "green"; break;
                  //case ChunkStatus.Rejected: fontColor = "red"; break; --Commented for alternate for CR 5096
                  case ChunkStatus.Missing: fontColor = "blue"; break;
                }
                sb.Append("<td align='center' style='background-color: #D0D0D0'>");
                sb.Append("<font color='");
                sb.Append(fontColor);
                sb.Append("'>");
                sb.Append(status.ToString().Substring(0,1));
                sb.Append("</font>");
                sb.Append("</td>");

                // add value
                if ((bool)r["rtl"])
                  sb.Append("<td style='direction: rtl'>"); // if Rtl
                else
                  sb.Append("<td>");
                if (r["ChunkValue"].ToString() == HyperCatalog.Business.Chunk.BlankValue)
                  sb.Append(HyperCatalog.Business.Chunk.BlankText);
                else
                  sb.Append(r["ChunkValue"].ToString());
                sb.Append("</td>");
              }

              if (ds != null)
              {
                ds.Dispose();
              }

              sb.Append("</table>");
              #endregion

              sb.Append("</table>");
              sb.Append("</body></html>");


              string fileName = string.Empty;
              fileName += "MoveStatus - ";
              if (item.Sku.Length > 0)
              {
                fileName += "[" + item.Sku + "] ";
              }
              fileName += item.Name;
              if (inputForm != null)
              {
                fileName += " (" + inputForm.Name + ")";
              }
              fileName += ".xls";

              string exportContent = sb.ToString();
              page.Response.Clear();
              page.Response.ClearContent();
              page.Response.ClearHeaders();
              page.Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
              page.Response.ContentType = "application/vnd.ms-excel";
              //Fix for CR 5109 - Prabhu R S
              page.Response.ContentEncoding = System.Text.Encoding.UTF8;
              page.Response.AppendHeader("Content-Length", System.Text.Encoding.UTF8.GetByteCount(exportContent).ToString());

              page.EnableViewState = false;
              page.Response.Write(exportContent);
              page.Response.End();
            }
          }

          if (ds != null)
          {
            ds.Dispose();
          }
        }
      }
    }
  }
}
