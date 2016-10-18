Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports HyperComponents.Data.dbAccess


Namespace HyperHelp


Public Module ObjectsUtils

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
    conn.RunSQLReturnRs(sql)

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

  'Refresh Source Pages
  '1/ add the new asp pages that are not in the DBObjects table
  '2/ delete from DBObjects table the asp pages that does not exist anymore in the current directory
  Public Sub RefreshPages(ByVal conn As HyperComponents.Data.dbAccess.Database, ByVal sourcespath As String, ByVal ApplicationId As String)
    Dim sql As String = String.Empty
    Dim str1, str2 As String
    Dim rs As SqlDataReader

    ' list of all asp pages in the table DBObjects
    sql = "SELECT * FROM Objects WHERE ApplicationId = " & ApplicationId & " AND Type='F' ORDER BY Name"
    rs = conn.RunSQLReturnRs(sql)

    str1 = String.Empty
    If rs.HasRows Then
      While rs.Read()
        ' create a string that contains all the pages of the table DBObjects
        str1 = str1 & "|" & rs("Name")
      End While
    End If
    rs.Close()
    rs = Nothing

    ' list of all asp pages in the current directory
    str2 = FindFiles(sourcespath, str1, String.Empty)

    sql = String.Empty
    Dim arNewFiles, nbNewFiles, i
    arNewFiles = Split(str2, "|")
    nbNewFiles = UBound(arNewFiles)
    For i = 0 To nbNewFiles
      If InStr(str1, arNewFiles(i)) = 0 Then
        ' if the asp pages contained in the current repertory is not
        ' contained in the first string (str1) then, add it to the insert query sql
        sql = sql & " INSERT INTO Objects(Name, Type, ApplicationId) VALUES('" & arNewFiles(i) & "', 'F', " & ApplicationId & ") "
      End If
    Next

    ' If there are new asp pages filenames in the current repertory
    If sql <> "" Then
      conn.RunSQLReturnRs(sql)
    End If

    sql = ""
    Dim nbFile, arFilenamesFromDB, k
    arFilenamesFromDB = Split(str1, "|")
    nbFile = UBound(arFilenamesFromDB)

    For k = 0 To nbFile
      If InStr(str2, arFilenamesFromDB(k)) = 0 Then
        sql = sql & " DELETE FROM Objects WHERE ApplicationId=" & ApplicationId & " AND Type='F' AND Name='" & arFilenamesFromDB(k) & "' "
      End If
    Next

    ' If there are "obsolete" asp pages in the Objects table
    If sql <> "" Then
      conn.RunSQLReturnRs(sql)
    End If


  End Sub

  'Refresh Dlls
  '1/ add the new Dlls that are not in the DBObjects table
  '2/ delete from DBObjects table the Dlls that does not exist anymore in the current directory
  Public Sub RefreshDLLs(ByVal conn As HyperComponents.Data.dbAccess.Database, ByVal dllspath As String, ByVal ApplicationId As String)
    Dim sql As String
    Dim str1, str2 As String
    Dim rs As SqlDataReader

    ' list of all asp pages in the table DBObjects
    sql = "SELECT * FROM Objects WHERE ApplicationId=" & ApplicationId & " AND Type='D' ORDER BY Name"
    rs = conn.RunSQLReturnRs(sql)

    str1 = ""
    If rs.HasRows() Then
      While rs.Read()
        ' create a string that contains all the asp pages of the table DBObjects
        str1 = str1 & "|" & rs("Name")
      End While
    End If
    rs.Close()
    rs = Nothing

    ' list of all asp pages in the current directory
    sql = ""
    str2 = ""
    Dim fso = CreateObject("Scripting.FileSystemObject")
    Dim f = fso.GetFolder(dllspath)
    Dim filelist = f.Files
    Dim fn
    For Each fn In filelist
      If (Right(fn.Name, 4) = ".dll") Or (Right(fn.Name, 4) = ".DLL") Then
        If InStr(str1, fn.Name) = 0 Then
          ' if the asp pages contained in the current repertory is not
          ' contained in the first string (str1) then, add it to the insert query sql
          sql = sql & " INSERT INTO Objects(Name, Type, ApplicationId) VALUES('" + fn.Name + "', 'D', " & ApplicationId & ") "
        End If
        ' string that contains all asp pages of the current directory
        str2 = str2 & "|" & fn.Name
      End If
    Next

    ' If there are new asp pages filenames in the current repertory
    If sql <> "" Then
      conn.RunSQLReturnRs(sql)
    End If

    sql = ""
    Dim arFilenamesFromDB = Split(str1, "|")
    Dim nbFile = UBound(arFilenamesFromDB)
    Dim k

    For k = 0 To nbFile
      If InStr(str2, arFilenamesFromDB(k)) = 0 Then
        sql = sql & " DELETE FROM Objects WHERE ApplicationId=" & ApplicationId & " AND Type='D' AND Name='" & arFilenamesFromDB(k) & "' "
      End If
    Next

    ' If there are "obsolete" asp pages in the DBObjects table
    If sql <> "" Then
      conn.RunSQLReturnRs(sql)
    End If
    fso = Nothing

  End Sub

  'Refresh Jobs
  '1/ add the new Jobs that are not in the DBObjects table
  '2/ delete from DBObjects table the Jobs that does not exist anymore in the current directory
  Public Sub RefreshJobs(ByVal conn As HyperComponents.Data.dbAccess.Database, ByVal applicationName As String, ByVal ApplicationId As String)
    Dim sql As String
    Dim rs As SqlDataReader

    sql = "SELECT J.name " & _
          "FROM msdb.dbo.sysjobs J, msdb.dbo.sysjobschedules JS, " & _
          "msdb.dbo.sysjobsteps JStep, msdb.dbo.sysjobservers JServer " & _
          "WHERE J.job_id=JS.job_id AND J.job_id=JStep.job_id AND J.job_id=JServer.job_id"
    rs = conn.RunSQLReturnRs(sql)

    If rs.HasRows() Then
      While rs.Read()

        ' the job name must contain the application name
        If InStr(rs("Name"), UCase(applicationName), vbTextCompare) > 0 Then
          ' ADD
          sql = "INSERT INTO Objects(Name, Type, ApplicationId) " & _
                "SELECT '" & rs("Name") & "', 'J' , " & ApplicationId & _
                " WHERE '" & rs("Name") & "' Not IN (SELECT Name FROM Objects WHERE ApplicationId=" & ApplicationId & ")"
          conn.RunSQLReturnRs(sql)
        End If
      End While
    End If
    rs.Close()
    rs = Nothing

  End Sub
  ' -----------------------------------------------------------------------------------------------------
  ' -- DEPENDENCE ---------------------------------------------------------------------------------------
  ' -----------------------------------------------------------------------------------------------------

  'Refresh Dependence for objects List (Table, View, Sproc, Trigger)
  '1/ update Dependence in other object 
  '2/ update Dependence in asp pages
  Public Function RefreshDependence(ByVal conn As HyperComponents.Data.dbAccess.Database, ByVal sourcespath As String, ByVal extensionslist As String, ByVal applicationName As String, ByVal ApplicationId As String) As String
    Dim nb, nbObj As Integer
    Dim sql, r As String
    Dim rs, rsDep As SqlDataReader

    Try
      nbObj = 0
      sql = "SELECT Name FROM Objects WHERE ApplicationId=" & ApplicationId & " AND (Type='U' OR Type='P' OR Type='V' OR Type='TR')"
      rs = conn.RunSQLReturnRs(sql)

      Dim dbIndexer As New DBObjects.IndexerClass
      dbIndexer.debugMode = True
      dbIndexer.ASPMode = False
      dbIndexer.OpenIndex(sourcespath, extensionslist)

      If conn.LastError = String.Empty Then
        While rs.Read
          nbObj += 1
          rsDep = conn.RunSPReturnRS("_DB_Depends", New SqlParameter("@objName", rs("Name")), New SqlParameter("@ApplicationId", ApplicationId))
          While rsDep.Read
            nb = nb + rsDep("nb")
          End While
          rsDep.Close()

          ' find dependence object in asp page
          dbIndexer.Search(rs("Name"))
          If dbIndexer.SearchResultCount > 0 Then
            nb = nb + dbIndexer.SearchResultCount
          End If

          ' Update in DBObjects table the Dependence Field by object
          sql = "UPDATE Objects SET Dependence = " & nb & _
                " WHERE ApplicationId=" & ApplicationId & " AND Name = '" & rs("Name") & "'"
          conn.RunSQLQuery(sql)

          nb = 0
        End While
      End If

      rs.Close()
      rs = Nothing
      r = "Dependence: " + nbObj.ToString + " objects updated."
    Catch ex As Exception
      r = "[Error] Dependence Process - " + ex.Message + " -"
    End Try

    Return r
  End Function

  Public Function SourceFilesDep(ByVal obj As String, ByVal sourcespath As String, ByVal extensionslist As String, ByVal applicationName As String) As DataSet

    ' Create a DataSet and a DataTable
    Dim ds As DataSet = New DataSet
    Dim table As DataTable = New DataTable("FilesDependence")
    Dim col As DataColumn
    ' Create & Add DataTable columns
    ' img
    col = New DataColumn
    col.DataType = System.Type.GetType("System.String")
    col.ColumnName = "img"
    col.ReadOnly = True
    col.Unique = False
    table.Columns.Add(col)
    ' aspx file name
    col = New DataColumn
    col.DataType = System.Type.GetType("System.String")
    col.ColumnName = "fileName"
    col.ReadOnly = True
    col.Unique = False
    table.Columns.Add(col)
    ' Create & Add DataTable rows

    Dim dbIndexer As New DBObjects.IndexerClass
    Dim j As Integer
    Dim page As String

    dbIndexer.debugMode = False
    dbIndexer.ASPMode = False
    dbIndexer.OpenIndex(sourcespath, extensionslist)
    dbIndexer.Search(obj)
    If dbIndexer.SearchResultCount > 0 Then
      For j = 0 To dbIndexer.SearchResultCount - 1
        page = dbIndexer.SearchResults(j)
        page = Replace(page, sourcespath, "")
        Dim row = table.NewRow()
        If (InStr(page, ".ascx", vbTextCompare) > 0) Or (InStr(page, ".aspx", vbTextCompare) > 0) Then
          row("img") = "<IMG SRC=img/otherFile.gif border=0>"
        End If
        If InStr(page, "vb", vbTextCompare) > 0 Then
          row("img") = "<IMG SRC=img/vbFile.gif border=0>"
        End If
        If InStr(page, ".cs", vbTextCompare) > 0 Then
          row("img") = "<IMG SRC=img/csFile.gif border=0>"
        End If
        row("fileName") = page
        table.Rows.Add(row)
      Next
    End If
    dbIndexer.CloseIndex()

    dbIndexer = Nothing
    ' Add DataTable to DataSet
    ds.Tables.Add(table)
    ' Return DataSet
    Return ds
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

  ' -----------------------------------------------------------------------------------------------------
  ' -- INDEX --------------------------------------------------------------------------------------------
  ' -----------------------------------------------------------------------------------------------------

  'Re-Index Pages
  Public Function ReIndex(ByVal sourcespath As String, ByVal extlist As String) As String
    Dim r As String
    Dim nb As Integer

    Try
      nb = 0
      Dim dbIndexer As New DBObjects.IndexerClass
      If (dbIndexer Is Nothing) Then
        r = "[Error] Indexing does not work !"
      Else
        dbIndexer.debugMode = False
        dbIndexer.ASPMode = False
        dbIndexer.OpenIndex(sourcespath, extlist)
        dbIndexer.reIndex()
        nb = dbIndexer.SearchResultCount
        dbIndexer.CloseIndex()
        dbIndexer = Nothing
        r = "Indexing process finish."
      End If
    Catch ex As Exception
      r = "[Error] Indexing Process - " + ex.Message + " -"
    End Try

    Return r

  End Function

End Module

End Namespace
