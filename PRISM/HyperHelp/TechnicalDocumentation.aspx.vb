#Region "Uses"
Imports HyperComponents
Imports HyperComponents.Data.dbAccess
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Data
#End Region

Namespace HyperHelp

  Partial Class TechnicalDocumentation
    Inherits System.Web.UI.Page

#Region " Code généré par le Concepteur Web Form "

    'Cet appel est requis par le Concepteur Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lErrorReport As System.Web.UI.WebControls.Label


    'REMARQUE : la déclaration d'espace réservé suivante est requise par le Concepteur Web Form.
    'Ne pas supprimer ou déplacer.

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
      'CODEGEN : cet appel de méthode est requis par le Concepteur Web Form
      'Ne le modifiez pas en utilisant l'éditeur de code.
      InitializeComponent()
    End Sub

#End Region

    Private HyperHelpConn As Database
    Public applicationName As String
    Public applicationId As String

    Public Sub LoadEnvironment()
      Dim instanceName As String = HttpContext.Current.Request.ApplicationPath.Split("/")(1).ToUpper()
      If (Not System.Configuration.ConfigurationManager.AppSettings(instanceName + "_ConnectionString") Is Nothing) Then
        HyperCatalog.Business.Settings.ConnectionString = System.Configuration.ConfigurationManager.AppSettings(instanceName + "_ConnectionString")
      End If
      HyperCatalog.Business.Settings.Values = New HyperCatalog.Business.Settings.WebConfigSettingValues()
      If (Not HttpContext.Current.Request("t") Is Nothing And HttpContext.Current.Request("t") <> HttpContext.Current.Session("userToken")) Then
        If (Not HttpContext.Current.Session Is Nothing) Then
          If ((HttpContext.Current.Session("UserId") Is Nothing) And (Not HttpContext.Current.Request("t") Is Nothing)) Then
            HttpContext.Current.Session("userToken") = HttpContext.Current.Request("t")
                        Dim CurrentUser As HyperCatalog.Business.User = HyperCatalog.Business.User.GetByToken(New Guid(HttpContext.Current.Request("t")))
            If (Not CurrentUser Is Nothing) Then
              HttpContext.Current.Session("UserId") = CurrentUser.Id
              Dim canManageNews As String = "0"
              If (CurrentUser.HasCapability(HyperCatalog.Business.CapabilitiesEnum.MANAGE_NEWS)) Then
                canManageNews = "1"
              End If
                            HttpContext.Current.Session("CanManageNews") = canManageNews
                            ' Check whether the User role has ManageHelp capability. 
                            ' ManageHelp capability has to be assigned only to System users.
                            ' Fixed by Prabhu (Bugid - 67321)
                            Dim canManageHelp As String = "0"
                            If (CurrentUser.HasCapability(HyperCatalog.Business.CapabilitiesEnum.MANAGE_HELP)) Then
                                canManageHelp = "1"
                            End If
                            HttpContext.Current.Session("canManageHelp") = canManageHelp
            End If
          End If
        End If
      End If
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
      LoadEnvironment()
      HyperHelpConn = New Database(HyperCatalog.Business.ApplicationSettings.Components("HyperHelp_DB").ConnectionString)
      applicationName = HyperCatalog.Business.Settings.Values("applicationname")

      Dim rs As SqlDataReader
      ' Get the applicationId 
      rs = HyperHelpConn.RunSQLReturnRS("SELECT * FROM Applications WHERE ApplicationName='" + applicationName + "'")
      If (HyperHelpConn.LastError = String.Empty) Then
        If rs.HasRows Then
          rs.Read()
          applicationId = rs("ApplicationId")
        End If
        rs.Close()
        rs = Nothing
      Else
        Response.Write("Error" + HyperHelpConn.LastError)
      End If

      If Not Page.IsPostBack Then
        RefreshDBObjects(HyperHelpConn, applicationName, applicationId)
        ShowPanel(pObjectsList)
        ShowObjectsList()
        uwToolBar.Items.FromKeyLabel("lbResult").DefaultStyle.Width = Unit.Pixel(0)
      End If
      Dim sql As String = "SELECT Report FROM ObjCategory WHERE Type='" + Trim(dpObjectsList.SelectedValue) + "' AND ApplicationId=" + applicationId + " ORDER BY Sort"
      rs = HyperHelpConn.RunSQLReturnRS(sql)
      If HyperHelpConn.LastError = String.Empty Then
        rs.Read()
        If rs("Report") = "Y" Then
          uwToolBar.Items.FromKeyButton("Report").Visible = True
        Else
          uwToolBar.Items.FromKeyButton("Report").Visible = False
        End If
            End If
            ' If the role does not have the capability to manage help, then remove Save button
            ' and set read-only property.
            ' Fixed by Prabhu (Bugid - 67321) 
            If HttpContext.Current.Session("canManageHelp") = 0 Then
                uwToolBarObjDetail.Items.FromKeyButton("Save").Visible = False
                uwToolBarObjDetail.Items.FromKeySeparator("Separator1").Width = Unit.Percentage(0)
                uwToolBarObjDetail.Items.FromKeySeparator("Separator2").Width = Unit.Percentage(0)
                txtdescription.Enabled = False
                lboxFunction.Enabled = False
                cboxSecondarytable.Enabled = False
            End If
    End Sub

    ' GET THE LIST OF ALL OBJECTS' CATEGORIES
    Private Sub ShowObjectsList()
      Dim sql As String = "SELECT Name, Type FROM ObjCategory WHERE ApplicationId= " + applicationId + " ORDER BY Sort"
      Dim ds As DataSet = HyperHelpConn.RunSQLReturnDataSet(sql, "Objects")

      If ds.Tables("Objects").Rows.Count > 0 Then
        dpObjectsList.DataSource = ds
        dpObjectsList.DataBind()
        dpObjectsList.SelectedIndex = 0
        ShowDetail(Trim(dpObjectsList.Items(0).Value))
      Else
        uwToolBar.Items.FromKeyLabel("lbResult").Text = "No objects found !"
        uwToolBar.Items.FromKeyLabel("lbResult").DefaultStyle.Width = uwToolBar.Items.FromKeyLabel("lbResult").ToolBar.ItemWidthDefault
      End If
    End Sub

    ' SHOW OBJECTS LIST BY TYPE
    Private Sub ShowDetail(ByVal objectType As String)
      Dim sql As String
      Dim ds As DataSet

      '------------------------------------------------------------------------------
      '-- objectType: Tables, views, Stored Procedure, Triggers or User Functions ---
      '------------------------------------------------------------------------------
      If (objectType = "U") Or (objectType = "V") Or (objectType = "P") Or (objectType = "TR") Or (objectType = "FN") Then
        sql = "SELECT Name, Dependence, [Function] as objFunction, Description " & _
              "FROM Objects " & _
              "WHERE Type='" & objectType & "' " & _
              "  AND ApplicationId=" & applicationId & _
              "ORDER BY [Name]"
        ds = HyperHelpConn.RunSQLReturnDataSet(sql, "ObjectType")
        gridDBObject.DataSource = ds.Tables("ObjectType").DefaultView
        gridDBObject.DataBind()

        ShowSubPanel(pDBObjects)
      End If

      '------------------------------------------------------------------------------
      '-- objectType: Jobs ----------------------------------------------------------
      '------------------------------------------------------------------------------
      If (objectType = "J") Then
        sql = "SELECT J.name as Name, JServer.last_outcome_message as Status, " & _
              "J.enabled as Activate, " & _
              "SUBSTRING(CONVERT(varchar, JStep.last_run_date), 1, 4) + '/' + " & _
              "SUBSTRING(CONVERT(varchar, JStep.last_run_date), 5, 2) + '/' + " & _
              "SUBSTRING(CONVERT(varchar, JStep.last_run_date), 7, 2) + ' ' + " & _
              "SUBSTRING(CONVERT(varchar, JStep.last_run_time), 1, 2) + ':' + " & _
              "SUBSTRING(CONVERT(varchar, JStep.last_run_time), 3, 2) + ':' + " & _
              "SUBSTRING(CONVERT(varchar, JStep.last_run_time), 5, 2) as 'Lastrun', " & _
              "SUBSTRING(CONVERT(varchar, JS.next_run_date), 1, 4) + '/' + " & _
              "SUBSTRING(CONVERT(varchar, JS.next_run_date), 5, 2) + '/' + " & _
              "SUBSTRING(CONVERT(varchar, JS.next_run_date), 7, 2) + ' ' + " & _
              "SUBSTRING(CONVERT(varchar, JS.next_run_time), 1, 2) + ':' + " & _
              "SUBSTRING(CONVERT(varchar, JS.next_run_time), 3, 2) + ':' + " & _
              "SUBSTRING(CONVERT(varchar, JS.next_run_time), 5, 2) as 'Nextrun' " & _
              "FROM msdb.dbo.sysjobs J, msdb.dbo.sysjobschedules JS, msdb.dbo.sysjobsteps JStep, msdb.dbo.sysjobservers JServer " & _
              "WHERE J.job_id=JS.job_id " & _
              "  AND J.job_id=JStep.job_id " & _
              "  AND J.job_id=JServer.job_id " & _
              "  AND UPPER(J.name) LIKE '%" & applicationName.ToUpper() & "%'" & _
              "ORDER BY J.name"
        ds = HyperHelpConn.RunSQLReturnDataSet(sql, "ObjectType")
        gridJobs.DataSource = ds.Tables("ObjectType").DefaultView
        gridJobs.DataBind()

        ShowSubPanel(pJobs)
      End If

      '------------------------------------------------------------------------------
      '-- objectType: sources Files -------------------------------------------------
      '------------------------------------------------------------------------------
      If (objectType = "F") Then

        sql = "SELECT Name, [Function] as objFunction, Description " & _
              "FROM Objects " & _
              "WHERE Type='" & objectType & "' " & _
              "  AND ApplicationId=" & applicationId & _
              "ORDER BY Name"
        ds = HyperHelpConn.RunSQLReturnDataSet(sql, "ObjectType")
        gridFiles.DataSource = ds.Tables("ObjectType").DefaultView
        gridFiles.DataBind()

        ShowSubPanel(pFiles)
      End If

      '------------------------------------------------------------------------------
      '-- objectType: Dlls ----------------------------------------------------------
      '------------------------------------------------------------------------------
      If (objectType = "D") Then
        sql = "SELECT Name, [Function] as objFunction, Description " & _
              "FROM Objects " & _
              "WHERE Type='" & objectType & "' " & _
              "  AND ApplicationId=" & applicationId & _
              "ORDER BY Name"
        ds = HyperHelpConn.RunSQLReturnDataSet(sql, "ObjectType")
        gridDlls.DataSource = ds.Tables("ObjectType").DefaultView
        gridDlls.DataBind()

        ShowSubPanel(pDlls)
      End If

    End Sub

    ' SHOW THE DETAIL OF THE OBJECT SELECTED
    Private Sub GetDetailsObj(ByVal obj As String)
      Dim sql As String
      Dim rs As SqlDataReader
      Dim objtype As String
      objtype = String.Empty


      LoadCategoryList()
      cboxSecondarytable.Visible = lTempTable.Visible = False

      sql = "SELECT * FROM Objects WHERE Name='" & obj & "' AND ApplicationId=" & applicationId
      rs = HyperHelpConn.RunSQLReturnRS(sql)

      If HyperHelpConn.LastError = String.Empty Then
        While rs.Read
          txtName.Text = rs("Name")
          objtype = Trim(rs("Type"))
          Select Case objtype
            Case "U" : txtType.Text = "Table"
            Case "P" : txtType.Text = "Stored Procedure"
            Case "V" : txtType.Text = "View"
            Case "TR" : txtType.Text = "Trigger"
            Case "FN" : txtType.Text = "User Function"
          End Select
          If IsDBNull(rs("Description")) Then
            txtdescription.Text = String.Empty
          Else
            txtdescription.Text = rs("Description")
          End If
          If IsDBNull(rs("Function")) Then
            lboxFunction.SelectedValue = String.Empty
          Else
            lboxFunction.SelectedValue = rs("Function")
          End If
          If objtype = "U" Then
            If rs("IsTemporaryTable") = "Y" Then
              cboxSecondarytable.Checked = True
            Else
              cboxSecondarytable.Checked = False
            End If
            cboxSecondarytable.Visible = True
            lTempTable.Visible = True
          End If
        End While
      End If
      rs.Close()
      rs = Nothing

      '------------------------------------------------------------------------------
      '-- Fields section ------------------------------------------------------------
      '------------------------------------------------------------------------------
      pFields.Visible = False
      ' show fields only for Table Objects
      If objtype = "U" Then
        sql = "_DB_GetFieldsList '" & obj & "', " & applicationId

        Dim ds As DataSet = HyperHelpConn.RunSQLReturnDataSet(sql, "tableStructure")

        If ds.Tables("tableStructure").Rows.Count > 0 Then
          gridStruture.DataSource = ds.Tables("tableStructure").DefaultView
          gridStruture.DataBind()
        End If
        pFields.Visible = True
      End If

      '------------------------------------------------------------------------------
      '-- Dependence section --------------------------------------------------------
      '------------------------------------------------------------------------------

      gridDBObjDep1.DataSource = DBobjDep(obj, HyperHelpConn, applicationName, applicationId).Tables("objDepend1")
      gridDBObjDep1.DataBind()
      If gridDBObjDep1.Rows.Count = 0 Then
        pGridDBObjDep1.Visible = False
      Else
        pGridDBObjDep1.Visible = True
      End If

      gridDBObjDep2.DataSource = DBobjDep(obj, HyperHelpConn, applicationName, applicationId).Tables("objDepend2")
      gridDBObjDep2.DataBind()
      If gridDBObjDep2.Rows.Count = 0 Then
        pGridDBObjDep2.Visible = False
      Else
        pGridDBObjDep2.Visible = True
      End If

      If ((gridDBObjDep1.Rows.Count = 0) And (gridDBObjDep2.Rows.Count = 0)) Then
        lDependence.Visible = False
      Else
        lDependence.Visible = True
      End If

      title.Text = "Database objects documentation - """ & obj & """ " & txtType.Text
    End Sub

    ' GET THE LIST OF CATEGORIES FUNCTION FOR AN OBJECT
    Private Sub LoadCategoryList()
      Dim sql As String
      Dim rs As SqlDataReader

      sql = "SELECT * FROM objFunction WHERE ApplicationId=" & applicationId & " ORDER BY [Name] asc"
      rs = HyperHelpConn.RunSQLReturnRS(sql)

      lboxFunction.Items.Clear()
      lboxFunction.Items.Add(String.Empty)
      If HyperHelpConn.LastError = String.Empty Then
        While rs.Read
          lboxFunction.Items.Add(rs("Name"))
        End While
      End If


    End Sub

    ' SELECTION IN GRIDDBOBJECT
    ' "Name" Link Button event handler
    Protected Sub UpdateGridItem(ByVal sender As Object, ByVal e As System.EventArgs)
      Dim cellItem As Infragistics.WebUI.UltraWebGrid.CellItem
      cellItem = CType((CType(sender, System.Web.UI.WebControls.LinkButton).Parent), Infragistics.WebUI.UltraWebGrid.CellItem)
      GetDetailsObj(cellItem.Cell.Row.Cells.FromKey("Name").Text)
      ShowPanel(pObjectDetails)
      uwToolBarObjDetail.Items.FromKeyLabel("lbResultObj").DefaultStyle.Width = Unit.Pixel(0)
    End Sub

    ' SAVE THE OBJECT INFORMATION
    Private Sub SaveObjDetail()
      Dim IsTempTable As String
      Dim sql As String

      If cboxSecondarytable.Checked Then
        IsTempTable = "Y"
      Else
        IsTempTable = "N"
      End If
      Dim retVal As Int32 = HyperHelpConn.RunSPReturnInteger("_DB_UpdateObject", New SqlParameter("@Name", txtName.Text), New SqlParameter("@ApplicationId", applicationId), New SqlParameter("@Description", txtdescription.Text), New SqlParameter("@IsTemporaryTable", IsTempTable), New SqlParameter("@Function", lboxFunction.SelectedItem.Text))

      If retVal = 1 Then
        uwToolBarObjDetail.Items.FromKeyLabel("lbResultObj").Text = "Data saved"
        uwToolBarObjDetail.Items.FromKeyLabel("lbResultObj").DefaultStyle.ForeColor = Color.Green
      Else
        uwToolBarObjDetail.Items.FromKeyLabel("lbResultObj").Text = "[error] Not saved"
        uwToolBarObjDetail.Items.FromKeyLabel("lbResultObj").DefaultStyle.ForeColor = Color.Red
      End If
      uwToolBarObjDetail.Items.FromKeyLabel("lbResultObj").DefaultStyle.Width = uwToolBarObjDetail.Items.FromKeyLabel("lbResultObj").ToolBar.ItemWidthDefault

      sql = "SELECT Type FROM Objects WHERE Name='" & txtName.Text & "' AND ApplicationId=" & applicationId
      Dim rs As SqlDataReader = HyperHelpConn.RunSQLReturnRS(sql)
      rs.Read()
      ShowDetail(Trim(rs("Type")))
    End Sub

    ' DISPLAY PANEL
    Private Sub ShowPanel(ByVal p As Panel)
      pReport.Visible = False
      pObjectsList.Visible = False
      pObjectDetails.Visible = False
      pDBObjects.Visible = False
      pJobs.Visible = False
      pFiles.Visible = False
      pDlls.Visible = False
      Select Case p.ID
        Case "pObjectsList" : p.Visible = True
        Case "pReport" : p.Visible = True
        Case "pObjectDetails" : p.Visible = True
        Case "pDBObjects" : p.Visible = True
        Case "pJobs" : p.Visible = True
        Case "pFiles" : p.Visible = True
        Case "pDlls" : p.Visible = True
      End Select
    End Sub

    ' DISPLAY SUBPANEL
    Private Sub ShowSubPanel(ByVal p As Panel)
      pDBObjects.Visible = False
      pJobs.Visible = False
      pFiles.Visible = False
      pDlls.Visible = False
      Select Case p.ID
        Case "pDBObjects" : p.Visible = True
        Case "pJobs" : p.Visible = True
        Case "pFiles" : p.Visible = True
        Case "pDlls" : p.Visible = True
      End Select
    End Sub

    ' Back To First Panel
    Private Sub BackToList()
      uwToolBar.Items.FromKeyLabel("lbResult").DefaultStyle.Width = Unit.Pixel(0)
      If pReport.Visible Then
        ShowPanel(pObjectsList)
        ShowSubPanel(pDBObjects)
      End If
      If pObjectDetails.Visible Then
        ShowPanel(pObjectsList)
        ShowSubPanel(pDBObjects)
      End If
    End Sub

    ' Open report panel
    Private Sub OpenReport()
      ShowPanel(pReport)
      Dim objtype = Trim(dpObjectsList.SelectedValue)
      Select Case objtype
        Case "U" : REPORT.Controls.Add(ShowDBObjects("Tables details", "U", True, True, HyperHelpConn, applicationName, applicationId))
        Case "TR" : REPORT.Controls.Add(ShowDBObjects("Triggers details", "TR", False, True, HyperHelpConn, applicationName, applicationId))
        Case "V" : REPORT.Controls.Add(ShowDBObjects("Views details", "V", False, True, HyperHelpConn, applicationName, applicationId))
        Case "P" : REPORT.Controls.Add(ShowDBObjects("Stored procedures details", "P", False, True, HyperHelpConn, applicationName, applicationId))
        Case "FN" : REPORT.Controls.Add(ShowDBObjects("User Functions details", "FN", False, True, HyperHelpConn, applicationName, applicationId))
      End Select
      REPORT.Controls.Add(ShowEndReport)
    End Sub


    ' SHOW DETAIL INFORMATION ABOUT OBJECT SELECTED
    Public Sub OnSelChange(ByVal sender As System.Object, ByVal e As System.EventArgs)
      Dim objtype = Trim(dpObjectsList.SelectedValue)
      ShowDetail(objtype)
    End Sub

    Private Sub uwToolBar_ButtonClicked(ByVal sender As Object, ByVal be As Infragistics.WebUI.UltraWebToolbar.ButtonEvent) Handles uwToolBar.ButtonClicked
      Dim btn As String = be.Button.Key.ToLower()
      If (btn = "report") Then
        OpenReport()
      End If
    End Sub

    Private Sub uwToolBarReport_ButtonClicked(ByVal sender As Object, ByVal be As Infragistics.WebUI.UltraWebToolbar.ButtonEvent) Handles uwToolBarReport.ButtonClicked
      Dim btn As String = be.Button.Key.ToLower()
      If (btn = "list") Then
        BackToList()
      End If
    End Sub

    Private Sub uwToolBarObjDetail_ButtonClicked(ByVal sender As Object, ByVal be As Infragistics.WebUI.UltraWebToolbar.ButtonEvent) Handles uwToolBarObjDetail.ButtonClicked
      Dim btn As String = be.Button.Key.ToLower()
      If (btn = "save") Then
        SaveObjDetail()
      End If
      If (btn = "list") Then
        BackToList()
      End If
    End Sub


#Region "ObjectsUtils"
    Public Function InitVar(ByVal v As String) As String
      Return v
    End Function

    Public Sub Debug(ByVal s As String)
      If (HttpContext.Current.Trace.IsEnabled) Then
        HttpContext.Current.Trace.Warn(s)
      End If
    End Sub

    ' -----------------------------------------------------------------------------------------------------
    ' -- REFRESH OBJECTS ----------------------------------------------------------------------------------
    ' -----------------------------------------------------------------------------------------------------

    'Refresh DBObjects List (Table, View, Sproc, Trigger) 
    '1/ delete from DBObjects table the Objects that does not exist anymore in the current directory
    '2/ add the new Objects that are not in the DBObjects table	
    Public Sub RefreshDBObjects(ByRef conn As HyperComponents.Data.dbAccess.Database, ByVal ApplicationName As String, ByVal ApplicationId As String)
      Dim sql As String

      ' DELETE 
      sql = "DELETE Objects " & _
            "WHERE ([Name] COLLATE French_CI_AS not IN (" & _
            "SELECT [Name] FROM " & ApplicationName & ".dbo.sysobjects " & _
            "WHERE (xtype='U' OR xtype='P' OR xtype='V' OR xtype='TR' OR xtype='FN') AND status >= 0)) " & _
            "  AND Type <> 'F' AND Type <> 'D' AND Type <> 'J' AND Type <> 'B' " & _
            "  AND ApplicationId = " & ApplicationId
      conn.RunSQLReturnRS(sql)

      ' ADD
      sql = "INSERT INTO Objects(Name, Type, ApplicationId) " & _
            "SELECT [name], xtype, " + ApplicationId + " FROM " & ApplicationName & ".dbo.sysobjects " & _
            "WHERE (xtype = 'V' OR xtype = 'U' OR xtype = 'P' OR xtype = 'TR' OR xtype='FN') " & _
            "  AND status >= 0  AND [Name] COLLATE French_CI_AS Not IN (SELECT [Name] FROM Objects WHERE ApplicationId = " & ApplicationId + ")"
      conn.RunSQLReturnRS(sql)

    End Sub

    Public Function FindFiles(ByVal path As String, ByVal listExistFiles As String, ByVal sql As String) As String
      Dim rSql = sql

      Dim fso = CreateObject("Scripting.FileSystemObject")
      Dim f = fso.GetFolder(path)
      Dim SubFolderList = f.SubFolders
      Dim SubfolderItem
      For Each SubfolderItem In SubFolderList
        Dim f1 = fso.GetFolder(SubfolderItem.path)
        If (f1.Attributes = 16) Then 'directory
          rSql = FindFiles(f1.path, listExistFiles, rSql)
        End If
      Next
      Dim filelist = f.Files
      Dim fn
      For Each fn In filelist
        If (Right(fn.Name, 5).ToLower() = ".aspx") Or (Right(fn.Name, 8).ToLower() = ".aspx.vb") Or _
           (Right(fn.Name, 5).ToLower() = ".ascx") Or (Right(fn.Name, 8).ToLower() = ".ascx.vb") Or _
           (Right(fn.Name, 8).ToLower() = ".ascx.cs") Or (Right(fn.Name, 8).ToLower() = ".aspx.cs") Then
          ' string that contains all asp pages of the current directory
          rSql = rSql & "|" & fn.Name
        End If
      Next
      fso = Nothing
      Return rSql
    End Function




    Public Function DBobjDep(ByVal obj As String, ByVal dbconn As HyperComponents.Data.dbAccess.Database, ByVal applicationName As String, ByVal ApplicationId As String) As DataSet

      ' Create a DataSet and a DataTable
      Dim ds As DataSet = New DataSet
      Dim table1 As DataTable = New DataTable("objDepend1")
      Dim col As DataColumn
      Dim row As DataRow
      ' Create & Add DataTable columns
      ' img
      col = New DataColumn
      col.DataType = System.Type.GetType("System.String")
      col.ColumnName = "img"
      col.ReadOnly = True
      col.Unique = False
      table1.Columns.Add(col)
      'obj name
      col = New DataColumn
      col.DataType = System.Type.GetType("System.String")
      col.ColumnName = "DBobj"
      col.ReadOnly = True
      col.Unique = False
      table1.Columns.Add(col)

      ' Create & Add DataTable rows
      Dim rs As SqlDataReader
      rs = dbconn.RunSPReturnRS("_DB_Depends", New SqlParameter("@objName", obj), New SqlParameter("@ApplicationId", ApplicationId))
      rs.NextResult()
      While rs.Read()
        row = table1.NewRow()
        Select Case rs("Type")
          Case "U" : row("img") = "<IMG SRC=img/tableObj.gif border=0>"
          Case "P" : row("img") = "<IMG SRC=img/spObj.gif border=0>"
          Case "V" : row("img") = "<IMG SRC=img/viewObj.gif border=0>"
          Case "T" : row("img") = "<IMG SRC=img/triggerObj.gif border=0>"
          Case "F" : row("img") = "<IMG SRC=img/fctObj.gif border=0>"
        End Select
        row("DBobj") = rs("Name")
        table1.Rows.Add(row)
      End While
      ' Add DataTable to DataSet
      ds.Tables.Add(table1)

      Dim table2 As DataTable = New DataTable("objDepend2")
      ' Create & Add DataTable columns
      ' img
      col = New DataColumn
      col.DataType = System.Type.GetType("System.String")
      col.ColumnName = "img"
      col.ReadOnly = True
      col.Unique = False
      table2.Columns.Add(col)
      'obj name
      col = New DataColumn
      col.DataType = System.Type.GetType("System.String")
      col.ColumnName = "DBobj"
      col.ReadOnly = True
      col.Unique = False
      table2.Columns.Add(col)
      ' Create & Add DataTable rows

      rs.NextResult()
      If rs.HasRows() Then
        While rs.Read()
          row = table2.NewRow()
          Select Case rs("Type")
            Case "U" : row("img") = "<IMG SRC=img/tableObj.gif border=0>"
            Case "P" : row("img") = "<IMG SRC=img/spObj.gif border=0>"
            Case "V" : row("img") = "<IMG SRC=img/viewObj.gif border=0>"
            Case "T" : row("img") = "<IMG SRC=img/triggerObj.gif border=0>"
            Case "F" : row("img") = "<IMG SRC=img/fctObj.gif border=0>"
          End Select
          row("DBobj") = rs("Name")
          table2.Rows.Add(row)
        End While
      End If
      ' Add DataTable to DataSet
      ds.Tables.Add(table2)

      rs.Close()
      rs = Nothing
      Return ds
    End Function

#End Region
#Region "DBDocReport"
    Public Function ShowTitleReport(ByVal title As String) As Panel
      Dim p As New Panel

      Dim t As New Table
      Dim tr As TableRow
      Dim td As TableCell
      t.Width = t.Width.Percentage(100)
      t.CellPadding = -1
      t.CellSpacing = -1

      tr = New TableRow
      td = New TableCell
      td.Controls.Add(CreateLabel(title, "hh_titlereport", 100))

      tr.Cells.Add(td)
      t.Controls.Add(tr)

      p.Controls.Add(t)

      Return p
    End Function

    Public Function ShowEndReport() As Panel
      Dim p As New Panel

      Dim t As New Table
      Dim tr As TableRow
      Dim td As TableCell
      t.Width = t.Width.Percentage(100)
      t.CellPadding = -1
      t.CellSpacing = -1

      tr = New TableRow
      td = New TableCell
      td.Controls.Add(CreateLabel("<hr><i>Created on " & Today.ToShortDateString.ToString & "</i>", "", 100))
      tr.Cells.Add(td)
      t.Controls.Add(tr)

      p.Controls.Add(t)

      Return p
    End Function


    Public Function ShowDBObjects(ByVal title As String, ByVal type As String, ByVal showtbStructure As Boolean, ByVal showDependance As Boolean, ByVal conn As HyperComponents.Data.dbAccess.Database, ByVal applicationName As String, ByVal applicationId As String) As Panel
      Dim p As New Panel
      Dim sql As String
      Dim rs As SqlDataReader

      Dim t As New Table
      Dim t2 As Table
      Dim tr, tr2 As TableRow
      Dim td, td2 As TableCell
      t.Width = t.Width.Percentage(100)
      t.CellPadding = -1
      t.CellSpacing = -1

      tr = New TableRow
      td = New TableCell
      td.ColumnSpan = 2
      td.Controls.Add(CreateLabel(title, "hh_subtitlereport", 100))
      tr.Cells.Add(td)
      t.Rows.Add(tr)

      sql = "SELECT Name, Dependence, [Function] as objFunction, Description, IsTemporaryTable " & _
            "FROM Objects " & _
            "WHERE Type='" & type & "' AND ApplicationId=" & applicationId & " " & _
            "ORDER BY [Name]"

      rs = conn.RunSQLReturnRS(sql)

      If conn.LastError = String.Empty Then
        If Not rs Is Nothing Then
          While rs.Read

            tr = New TableRow
            td = New TableCell
            td.VerticalAlign = VerticalAlign.Top
            td.HorizontalAlign = HorizontalAlign.Left
            If Not showtbStructure Then
              td.ColumnSpan = 2
            End If
            td.Controls.Add(CreateLabel(rs("Name") & "</br></br>", "hh_labelreport", 0))
            If rs("IsTemporaryTable") = "Y" Then
              td.Controls.Add(CreateLabel("<i>Temporary table</i></br>", "", 0))
            End If
            td.Controls.Add(CreateLabel("FUNCTION: " & rs("objFunction") & "</br>", "", 0))
            td.Controls.Add(CreateLabel("DESCRIPTION: " & rs("Description") & "</br>", "", 0))
            tr.Cells.Add(td)
            t.Rows.Add(tr)

            tr = New TableRow
            ' show table structure
            If showtbStructure Then
              td = New TableCell
              td.Width = td.Width.Percentage(35)
              td.VerticalAlign = VerticalAlign.Top
              td.HorizontalAlign = HorizontalAlign.Left

              sql = "EXEC _DB_GetFieldsList '" & rs("Name") & "', " & applicationId
              Dim ds As DataSet = conn.RunSQLReturnDataSet(sql, "tableStructure")

              If ds.Tables("tableStructure").Rows.Count > 0 Then
                td.Controls.Add(CreateLabel("Fields", "hh_subTitle", 100))
                Dim grid As DataGrid = CreateGrid(300, "p", True)
                grid.DataSource = ds.Tables("tableStructure")
                grid.DataBind()
                td.Controls.Add(grid)
              End If
              tr.Cells.Add(td)
            End If

            ' show DBObject dependance
            If showDependance Then

              Dim grid2 As DataGrid = CreateGrid(100, "%", False)
              grid2.DataSource = DBobjDep(rs("Name"), conn, applicationName, applicationId).Tables("objDepend1")
              grid2.DataBind()

              Dim grid3 As DataGrid = CreateGrid(100, "%", False)
              grid3.DataSource = DBobjDep(rs("Name"), conn, applicationName, applicationId).Tables("objDepend2")
              grid3.DataBind()

              If (grid2.Items.Count <> 0) Or (grid3.Items.Count <> 0) Then
                td = New TableCell
                td.VerticalAlign = VerticalAlign.Top
                td.HorizontalAlign = HorizontalAlign.Left
                td.Controls.Add(CreateLabel("Dependence", "hh_subTitle", 100))

                t2 = New Table
                t2.CellPadding = 0
                t2.CellSpacing = 0
                t2.Width = t2.Width.Percentage(100)
                tr2 = New TableRow
                td2 = New TableCell

                If grid2.Items.Count <> 0 Then
                  td2 = New TableCell
                  td2.Width = td2.Width.Pixel(400)
                  td2.VerticalAlign = VerticalAlign.Top
                  td2.HorizontalAlign = HorizontalAlign.Center
                  td2.Controls.Add(CreateLabel("depends on</br>", "hh_gridHeader", 100))
                  td2.Controls.Add(grid2)
                  tr2.Cells.Add(td2)
                  td2 = New TableCell
                  td2.Width = td2.Width.Pixel(20)
                  td2.Controls.Add(CreateLabel("", "", 100))
                  tr2.Cells.Add(td2)
                End If

                If grid3.Items.Count <> 0 Then
                  td2 = New TableCell
                  td2.Width = td2.Width.Pixel(400)
                  td2.VerticalAlign = VerticalAlign.Top
                  td2.HorizontalAlign = HorizontalAlign.Center
                  td2.Controls.Add(CreateLabel("objects depending on</br>", "hh_gridHeader", 100))
                  td2.Controls.Add(grid3)
                  tr2.Cells.Add(td2)
                End If

                t2.Controls.Add(tr2)

                td.Controls.Add(t2)
                tr.Cells.Add(td)
                t.Rows.Add(tr)

              End If
            End If

            tr = New TableRow
            td = New TableCell
            td.ColumnSpan = 2
            td.Controls.Add(CreateLabel("<hr>", "", 0))
            tr.Cells.Add(td)
            t.Controls.Add(tr)

          End While
        End If
        p.Controls.Add(t)
      End If

      p.Controls.Add(t)
      Return p
    End Function


    Public Function ShowJobs(ByVal title As String, ByVal conn As HyperComponents.Data.dbAccess.Database, ByVal applicationName As String) As Panel
      Dim p As New Panel
      Dim sql As String

      Dim t As New Table
      Dim tr As TableRow
      Dim td As TableCell
      t.Width = t.Width.Percentage(100)
      t.CellPadding = -1
      t.CellSpacing = -1

      tr = New TableRow
      td = New TableCell
      td.ColumnSpan = 2
      td.Controls.Add(CreateLabel(title, "sectionTitle", 100))
      tr.Cells.Add(td)
      t.Rows.Add(tr)

      sql = "SELECT J.name, JServer.last_outcome_message as Status, " & _
                  "J.enabled as Activate, JS.enabled as Planified, " & _
                  "SUBSTRING(CONVERT(varchar, JStep.last_run_date), 1, 4) + '/' + " & _
                  "SUBSTRING(CONVERT(varchar, JStep.last_run_date), 5, 2) + '/' + " & _
                  "SUBSTRING(CONVERT(varchar, JStep.last_run_date), 7, 2) + ' ' + " & _
                  "SUBSTRING(CONVERT(varchar, JStep.last_run_time), 1, 2) + ':' + " & _
                  "SUBSTRING(CONVERT(varchar, JStep.last_run_time), 3, 2) + ':' + " & _
                  "SUBSTRING(CONVERT(varchar, JStep.last_run_time), 5, 2) as 'Lastrun', " & _
                  "SUBSTRING(CONVERT(varchar, JS.next_run_date), 1, 4) + '/' + " & _
                  "SUBSTRING(CONVERT(varchar, JS.next_run_date), 5, 2) + '/' + " & _
                  "SUBSTRING(CONVERT(varchar, JS.next_run_date), 7, 2) + ' ' + " & _
                  "SUBSTRING(CONVERT(varchar, JS.next_run_time), 1, 2) + ':' + " & _
                  "SUBSTRING(CONVERT(varchar, JS.next_run_time), 3, 2) + ':' + " & _
                  "SUBSTRING(CONVERT(varchar, JS.next_run_time), 5, 2) as 'Nextrun' " & _
                  "FROM msdb.dbo.sysjobs J, msdb.dbo.sysjobschedules JS, msdb.dbo.sysjobsteps JStep, msdb.dbo.sysjobservers JServer " & _
                  "WHERE J.job_id=JS.job_id " & _
                  "  AND J.job_id=JStep.job_id " & _
                  "  AND J.job_id=JServer.job_id " & _
                  "  AND j.Name LIKE '%" & applicationName & "%'" & _
                  "ORDER BY J.name"
      Dim ds As DataSet = conn.RunSQLReturnDataSet(sql, "tableJobs")

      If conn.LastError = "" Then
        tr = New TableRow
        td = New TableCell
        td.VerticalAlign = VerticalAlign.Top
        td.HorizontalAlign = HorizontalAlign.Left
        Dim grid As DataGrid = CreateGrid(100, "%", True)
        grid.DataSource = ds.Tables("tableJobs").DefaultView
        grid.DataBind()
        td.Controls.Add(grid)
        tr.Cells.Add(td)
        t.Rows.Add(tr)
        p.Controls.Add(t)
      End If

      p.Controls.Add(t)
      Return p
    End Function
#End Region

#Region "AddComponents"

    Public Function CreateGrid(ByVal w As Integer, ByVal wType As String, ByVal showhd As Boolean) As DataGrid
      'declare a new datagrid and set properties
      Dim dg As New DataGrid
      dg.CssClass = "hh_dataTable"
      dg.ShowHeader = showhd
      dg.HeaderStyle.CssClass = "hh_gridHeader"
      dg.ItemStyle.CssClass = "hh_gridCell"
      dg.SelectedItemStyle.CssClass = "hh_gridSelected"
      dg.AlternatingItemStyle.CssClass = "hh_gridCellAlter"
      If w <> 0 Then
        If wType = "%" Then
          dg.Width = dg.Width.Percentage(w)
        End If
        If wType = "p" Then
          dg.Width = dg.Width.Pixel(w)
        End If
      End If

      Return dg
    End Function

    Public Function CreateLabel(ByVal text As String, ByVal cssClass As String, ByVal w As Integer) As Label
      Dim lb As New Label
      lb.Text = text
      lb.CssClass = cssClass
      If w <> 0 Then
        lb.Width = lb.Width.Percentage(w)
      End If
      Return lb
    End Function
#End Region
  End Class

End Namespace
