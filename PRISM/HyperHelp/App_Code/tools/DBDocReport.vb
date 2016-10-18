Imports System.Data
Imports System.Data.SqlClient
Imports HyperComponents.Data.dbAccess


Namespace HyperHelp


Public Module DBDocReport

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
    td.Controls.Add(AddComponents.CreateLabel(title, "hh_titlereport", 100))

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
    td.Controls.Add(AddComponents.CreateLabel("<hr><i>Created on " & Today.ToShortDateString.ToString & "</i>", "", 100))
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
      t.Width = Unit.Percentage(100)
    t.CellPadding = -1
    t.CellSpacing = -1

    tr = New TableRow
    td = New TableCell
    td.ColumnSpan = 2
    td.Controls.Add(AddComponents.CreateLabel(title, "hh_subtitlereport", 100))
    tr.Cells.Add(td)
    t.Rows.Add(tr)

    sql = "SELECT Name, Dependence, [Function] as objFunction, Description, IsTemporaryTable " & _
          "FROM Objects " & _
          "WHERE Type='" & type & "' AND ApplicationId=" & applicationId & " " & _
          "ORDER BY [Name]"

    rs = conn.RunSQLReturnRs(sql)

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
          td.Controls.Add(AddComponents.CreateLabel(rs("Name") & "</br></br>", "hh_labelreport", 0))
          If rs("IsTemporaryTable") = "Y" Then
            td.Controls.Add(AddComponents.CreateLabel("<i>Temporary table</i></br>", "", 0))
          End If
          td.Controls.Add(AddComponents.CreateLabel("FUNCTION: " & rs("objFunction") & "</br>", "", 0))
          td.Controls.Add(AddComponents.CreateLabel("DESCRIPTION: " & rs("Description") & "</br>", "", 0))
          tr.Cells.Add(td)
          t.Rows.Add(tr)

          tr = New TableRow
          ' show table structure
          If showtbStructure Then
            td = New TableCell
              td.Width = Unit.Percentage(35)
            td.VerticalAlign = VerticalAlign.Top
            td.HorizontalAlign = HorizontalAlign.Left

            sql = "_DB_GetFieldsList '" & rs("Name") & "', " & applicationId
            Dim ds As DataSet = conn.RunSQLReturnDataSet(sql, "tableStructure")

            If ds.Tables("tableStructure").Rows.Count > 0 Then
              td.Controls.Add(AddComponents.CreateLabel("Fields", "hh_subTitle", 100))
              Dim grid As DataGrid = AddComponents.CreateGrid(300, "p", True)
              grid.DataSource = ds.Tables("tableStructure").DefaultView
              grid.DataBind()
              td.Controls.Add(grid)
            End If
            tr.Cells.Add(td)
          End If

          ' show DBObject dependance
          If showDependance Then

            Dim grid2 As DataGrid = AddComponents.CreateGrid(100, "%", False)
            grid2.DataSource = DBobjDep(rs("Name"), conn, applicationName, applicationId).Tables("objDepend1")
            grid2.DataBind()

            Dim grid3 As DataGrid = AddComponents.CreateGrid(100, "%", False)
            grid3.DataSource = DBobjDep(rs("Name"), conn, applicationName, applicationId).Tables("objDepend2")
            grid3.DataBind()

            If (grid2.Items.Count <> 0) Or (grid3.Items.Count <> 0) Then
              td = New TableCell
              td.VerticalAlign = VerticalAlign.Top
              td.HorizontalAlign = HorizontalAlign.Left
              td.Controls.Add(AddComponents.CreateLabel("Dependence", "hh_subTitle", 100))

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
                td2.Controls.Add(AddComponents.CreateLabel("depends on</br>", "hh_gridHeader", 100))
                td2.Controls.Add(grid2)
                tr2.Cells.Add(td2)
                td2 = New TableCell
                td2.Width = td2.Width.Pixel(20)
                td2.Controls.Add(AddComponents.CreateLabel("", "", 100))
                tr2.Cells.Add(td2)
              End If

              If grid3.Items.Count <> 0 Then
                td2 = New TableCell
                td2.Width = td2.Width.Pixel(400)
                td2.VerticalAlign = VerticalAlign.Top
                td2.HorizontalAlign = HorizontalAlign.Center
                td2.Controls.Add(AddComponents.CreateLabel("objects depending on</br>", "hh_gridHeader", 100))
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
          td.Controls.Add(AddComponents.CreateLabel("<hr>", "", 0))
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
      t.Width = Unit.Percentage(100)
    t.CellPadding = -1
    t.CellSpacing = -1

    tr = New TableRow
    td = New TableCell
    td.ColumnSpan = 2
    td.Controls.Add(AddComponents.CreateLabel(title, "sectionTitle", 100))
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
      Dim grid As DataGrid = AddComponents.CreateGrid(100, "%", True)
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
End Module

End Namespace
