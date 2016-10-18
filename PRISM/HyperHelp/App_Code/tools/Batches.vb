Imports System.Data
Imports System.Data.SqlClient
Imports HyperComponents.Data.dbAccess


Namespace HyperHelp

'Imports C1.Win.C1Chart
'Imports C1.Web.C1WebChart


Public Module Batches

  'Public Function NullToString(ByVal s) As String
  '  If s Is DBNull.Value Then Return String.Empty
  '  Return s
  'End Function

  'Public Function NullToInteger(ByVal s) As Integer
  '  If s Is DBNull.Value Then Return 0
  '  Return s
  'End Function

  'Public Function BuildGantt(ByVal conn As Database, ByVal aDate As String, ByVal ApplicationName As String) As DataSet
  '  Dim BatchList As String
  '  Dim sql As String
  '  Dim rs As SqlDataReader

  '  ' Create a DataSet and a DataTable
  '  Dim ds As DataSet = New DataSet
  '  Dim table As DataTable = New DataTable("Gantt")
  '  Dim col As DataColumn
  '  ' Create & Add DataTable columns
  '  ' BatchName
  '  col = New DataColumn
  '  col.DataType = System.Type.GetType("System.String")
  '  col.ColumnName = "BatchName"
  '  col.ReadOnly = True
  '  col.Unique = False
  '  table.Columns.Add(col)
  '  ' Start
  '  col = New DataColumn
  '  col.DataType = System.Type.GetType("System.DateTime")
  '  col.ColumnName = "Start"
  '  col.ReadOnly = True
  '  col.Unique = True
  '  table.Columns.Add(col)
  '  ' Finish
  '  col = New DataColumn
  '  col.DataType = System.Type.GetType("System.DateTime")
  '  col.ColumnName = "Finish"
  '  col.ReadOnly = True
  '  col.Unique = True
  '  table.Columns.Add(col)

  '  sql = "SELECT DISTINCT B.BatchName, L.Start, L.Finish " & _
  '        "FROM Batches B, " & ApplicationName & ".dbo.Logs L " & _
  '        "WHERE B.BatchName = L.ProcessName " & _
  '        "  AND B.ApplicationName = '" & ApplicationName & "'" & _
  '        "  AND Start >= CONVERT(Datetime, '" & aDate & " 00:00:00') " & _
  '        "  AND Start < CONVERT(DateTime, '" & DateAdd("D", 1, aDate) & " 00:00:00') " & _
  '        "ORDER BY Start asc"

  '  rs = conn.RunSQLReturnRs(sql)

  '  If conn.LastError = "" Then
  '    If Not ds Is Nothing Then
  '      BatchList = ""
  '      While rs.Read()
  '        If InStr(BatchList, rs("BatchName")) = 0 Then
  '          Dim row = table.NewRow()
  '          row("BatchName") = rs("BatchName")
  '          row("Start") = rs("Start")
  '          row("Finish") = rs("Finish")
  '          table.Rows.Add(row)
  '          BatchList = BatchList & "," & rs("BatchName")
  '        End If
  '      End While
  '      ' Add DataTable to DataSet
  '      ds.Tables.Add(table)
  '    End If
  '  End If

  '  rs.Close()
  '  rs = Nothing
  '  Return ds
  'End Function


  'Public Function BuildMonthReport(ByVal conn As Database, ByVal aBatchID As Integer, ByVal aBatchName As String, ByVal aDescription As String, ByVal aMetric As String, ByVal aMonth As Integer, ByVal ApplicationName As String) As DataSet
  '  Dim sql As String
  '  Dim ds As DataSet
  '  Dim d, h, m, s As Integer

  '  ' Create a DataSet and a DataTable
  '  Dim resultds As DataSet = New DataSet
  '  Dim table As DataTable = New DataTable("BatchMonth")
  '  Dim col As DataColumn
  '  Dim dsRow As DataRow
  '  ' Create & Add DataTable columns
  '  ' Start
  '  col = New DataColumn
  '  col.DataType = System.Type.GetType("System.String")
  '  col.ColumnName = "Start"
  '  col.ReadOnly = True
  '  col.Unique = False
  '  table.Columns.Add(col)
  '  ' Duration
  '  col = New DataColumn
  '  col.DataType = System.Type.GetType("System.String")
  '  col.ColumnName = "Duration"
  '  col.ReadOnly = True
  '  col.Unique = False
  '  table.Columns.Add(col)
  '  ' Duration in seconde
  '  col = New DataColumn
  '  col.DataType = System.Type.GetType("System.String")
  '  col.ColumnName = "DurationSec"
  '  col.ReadOnly = True
  '  col.Unique = False
  '  table.Columns.Add(col)
  '  ' Metric
  '  col = New DataColumn
  '  col.DataType = System.Type.GetType("System.String")
  '  col.ColumnName = "Metric"
  '  col.ReadOnly = True
  '  col.Unique = False
  '  table.Columns.Add(col)

  '  '-- Select DATAS to display
  '  sql = "SELECT Start, Finish, Result, Datediff(ss, Start, Finish) as Time " & _
  '        "FROM " & ApplicationName & ".dbo.Logs WHERE ProcessName='" & aBatchName & _
  '        "' AND B.ApplicationName = '" & ApplicationName & _
  '        "' AND month(START) = '" & aMonth & "' AND Result <> '' " & _
  '        "ORDER BY Start asc"

  '  ds = conn.RunSQLReturnDataSet(sql, "dsBatch")

  '  If conn.LastError = "" Then
  '    If Not ds Is Nothing Then
  '      For Each dsRow In ds.Tables(0).Rows
  '        d = DateDiff("s", dsRow("Start"), dsRow("Finish"))
  '        h = d \ 3600
  '        If Len(h) = 1 Then
  '          h = "0" & h
  '        End If
  '        m = (d Mod 3600) \ 60
  '        If Len(m) = 1 Then
  '          m = "0" & m
  '        End If
  '        s = d Mod 60
  '        If Len(s) = 1 Then
  '          s = "0" & s
  '        End If

  '        Dim row = table.NewRow()
  '        row("Start") = Left(dsRow("Start"), 10)
  '        row("Duration") = h & ":" & m & ":" & s
  '        row("DurationSec") = d
  '        row("Metric") = dsRow("Result")
  '        table.Rows.Add(row)
  '      Next
  '      ' Add DataTable to DataSet
  '      resultds.Tables.Add(table)
  '    End If
  '  End If

  '  ds = Nothing
  '  Return resultds

  'End Function

  'Public Function ShowGantt(ByVal d As String, ByVal isReport As Boolean, ByVal conn As Database, ByVal ApplicationName As String) As Panel
  '  Dim p As New Panel

  '  Dim t As New Table
  '  Dim tr As TableRow
  '  Dim td As TableCell
  '  t.Width = t.Width.Percentage(100)
  '  t.CellPadding = -1
  '  t.CellSpacing = -1

  '  If isReport Then
  '    tr = New TableRow
  '    td = New TableCell
  '    td.ColumnSpan = 2
  '    td.Controls.Add(AddComponents.CreateLabel("Gantt chart for batches", "sectionTitle", 100))
  '    tr.Cells.Add(td)
  '    t.Rows.Add(tr)
  '  End If

  '  Dim ds As DataSet = BuildGantt(conn, d, ApplicationName)
  '  If Not ds Is Nothing Then
  '    If ds.Tables(0).Rows.Count > 0 Then

  '      tr = New TableRow
  '      td = New TableCell
  '      td.VerticalAlign = VerticalAlign.Top
  '      td.HorizontalAlign = HorizontalAlign.Center
  '      Dim grid As DataGrid = AddComponents.CreateGrid(0, "", True)
  '      grid.DataSource() = ds
  '      grid.DataBind()
  '      td.Controls.Add(grid)
  '      tr.Cells.Add(td)

  '      Dim chart1 As C1.Web.C1WebChart.C1WebChart
  '      If isReport Then
  '        chart1 = AddComponents.CreateChart("", 400, 300, False)
  '      Else
  '        chart1 = AddComponents.CreateChart("", 500, 400, False)
  '      End If
  '      td = New TableCell
  '      td.VerticalAlign = VerticalAlign.Top
  '      td.HorizontalAlign = HorizontalAlign.Center
  '      td.Controls.Add(chart1)
  '      tr.Cells.Add(td)
  '      t.Rows.Add(tr)

  '      ' Gantt Chart
  '      Dim i As Integer
  '      Dim x As Integer()
  '      Dim xlabels As String()
  '      Dim y As Double()
  '      Dim y1 As Double()
  '      ReDim x(ds.Tables(0).Rows.Count - 1), xlabels(ds.Tables(0).Rows.Count - 1)
  '      ReDim y(ds.Tables(0).Rows.Count - 1), y1(ds.Tables(0).Rows.Count - 1)
  '      For i = 0 To ds.Tables(0).Rows.Count - 1
  '        x(i) = i
  '        xlabels(i) = ds.Tables(0).Rows(i)("BatchName")
  '        y(i) = ds.Tables(0).Rows(i)("Start").ToOADate
  '        y1(i) = ds.Tables(0).Rows(i)("Finish").ToOADate
  '      Next
  '      chart1.ChartGroups(0).ChartData.SeriesList.Clear()
  '      Dim cds1 As ChartDataSeries = chart1.ChartGroups(0).ChartData.SeriesList.AddNewSeries()
  '      cds1.X.CopyDataIn(x)
  '      cds1.Y.CopyDataIn(y)
  '      cds1.Y1.CopyDataIn(y1)
  '      cds1.LineStyle.Color = Color.FromArgb(205, 225, 243)
  '      cds1.LineStyle.Thickness = 25
  '      chart1.ChartArea.AxisY.Min = DateAndTime.DateValue(d & " 00:00:00").ToOADate
  '      chart1.ChartArea.AxisY2.Max = DateAndTime.DateValue(DateAdd("D", 1, d) & " 00:00:00").ToOADate
  '      For i = 0 To xlabels.Length - 1
  '        chart1.ChartArea.AxisX.ValueLabels.Add(i, xlabels(i))
  '      Next
  '      With chart1.ChartArea
  '        .AxisX.AnnoMethod = AnnotationMethodEnum.ValueLabels
  '        .AxisX.TickMajor = TickMarksEnum.None
  '        .AxisX.TickMinor = TickMarksEnum.None

  '        .AxisY.AnnoFormat = FormatEnum.DateManual
  '        .AxisY.AnnoFormatString = "HH"
  '        .AxisY.GridMajor.Visible = True
  '        .AxisY.GridMajor.Color = Color.Black
  '        .AxisY.GridMajor.Pattern = LinePatternEnum.Solid
  '        .AxisY.TickMinor = TickMarksEnum.None

  '        .PlotArea.BackColor = Color.White
  '        .PlotArea.Boxed = True

  '        .Inverted = True
  '      End With
  '      chart1.BackColor = Color.White
  '      chart1.ChartGroups(0).ChartType = Chart2DTypeEnum.HiLo

  '    Else
  '      tr = New TableRow
  '      td = New TableCell
  '      td.VerticalAlign = VerticalAlign.Top
  '      td.HorizontalAlign = HorizontalAlign.Left
  '      td.Controls.Add(AddComponents.CreateLabel("No log for this selected date ...", "errormsg", 0))
  '      tr.Cells.Add(td)
  '      t.Controls.Add(tr)
  '    End If

  '  End If
  '  p.Controls.Add(t)
  '  Return p
  'End Function

  'Public Function ShowBatches(ByVal list As DropDownList, ByVal BatchName As String, ByVal isReport As Boolean, ByVal conn As Database, ByVal ApplicationName As String) As Panel
  '  Dim p As New Panel
  '  Dim month As String
  '  Dim sql As String
  '  Dim rs As SqlDataReader

  '  Dim t As New Table
  '  Dim tr As TableRow
  '  Dim td As TableCell
  '  t.Width = t.Width.Percentage(100)
  '  t.CellPadding = -1
  '  t.CellSpacing = -1

  '  sql = "SELECT * FROM Batches WHERE B.ApplicationName = '" & ApplicationName & "'"
  '  If BatchName <> "All" Then
  '    sql += " AND BatchName='" & BatchName & "'"
  '  End If

  '  rs = conn.RunSQLReturnRs(sql)

  '  If conn.LastError = "" Then
  '    While rs.Read
  '      If isReport Then
  '        tr = New TableRow
  '        td = New TableCell
  '        td.ColumnSpan = 2
  '        td.Controls.Add(AddComponents.CreateLabel("Logs for " & rs("BatchName"), "sectionTitle", 100))
  '        tr.Cells.Add(td)
  '        t.Rows.Add(tr)
  '      End If

  '      tr = New TableRow
  '      td = New TableCell
  '      td.VerticalAlign = VerticalAlign.Middle
  '      td.HorizontalAlign = HorizontalAlign.Center
  '      td.ColumnSpan = 2
  '      Dim p1 As New Panel
  '      p1.Width = p1.Width.Percentage(100)
  '      p1.Height = p1.Height.Percentage(100)
  '      p1.CssClass = "div"
  '      p1.Controls.Add(AddComponents.CreateLabel(NullToString(rs("Description")) & "<BR>", "", 0))
  '      p1.Controls.Add(AddComponents.CreateLabel("<b>Metric:</b> " & NullToString(rs("Metric")) & "<BR>", "", 0))
  '      p1.Controls.Add(AddComponents.CreateLabel("<b>Scheduled:</b> " & NullToString(rs("Scheduled")), "", 0))
  '      td.Controls.Add(p1)
  '      tr.Cells.Add(td)
  '      t.Rows.Add(tr)

  '      month = Left(list.SelectedItem.Text, InStr(list.SelectedItem.Text, "/") - 1)
  '      Dim ds As DataSet = Batches.BuildMonthReport(conn, rs("BatchID"), rs("BatchName"), NullToString(rs("Description")), NullToString(rs("Metric")), month, ApplicationName)
  '      If ds.Tables(0).Rows.Count > 0 Then

  '        'bind datagrid
  '        Dim grid As DataGrid = AddComponents.CreateGrid(0, "", True)
  '        grid.DataSource = ds
  '        grid.DataBind()

  '        'add datagrid to the page
  '        tr = New TableRow
  '        td = New TableCell
  '        td.VerticalAlign = VerticalAlign.Top
  '        td.HorizontalAlign = HorizontalAlign.Center
  '        td.Height = td.Height.Percentage(100)
  '        td.Controls.Add(grid)
  '        tr.Cells.Add(td)
  '        t.Rows.Add(tr)

  '        Dim chart1 As C1.Web.C1WebChart.C1WebChart
  '        If isReport Then
  '          td = New TableCell
  '          td.VerticalAlign = VerticalAlign.Top
  '          td.HorizontalAlign = HorizontalAlign.Center
  '          chart1 = AddComponents.CreateChart("", 400, 300, True)
  '          td.Controls.Add(chart1)
  '          tr.Cells.Add(td)
  '          t.Rows.Add(tr)
  '        Else
  '          tr = New TableRow
  '          td = New TableCell
  '          td.VerticalAlign = VerticalAlign.Top
  '          td.HorizontalAlign = HorizontalAlign.Center
  '          chart1 = AddComponents.CreateChart("", 500, 400, True)
  '          td.Controls.Add(chart1)
  '          tr.Cells.Add(td)
  '          t.Rows.Add(tr)
  '        End If

  '        Dim i As Integer
  '        Dim x As DateTime()
  '        Dim y As Integer()
  '        Dim z As Integer()
  '        ReDim x(ds.Tables(0).Rows.Count - 1), y(ds.Tables(0).Rows.Count - 1), z(ds.Tables(0).Rows.Count - 1)
  '        chart1.ChartGroups.Group0.ChartData.SeriesList.Clear()
  '        chart1.ChartGroups.Group1.ChartData.SeriesList.Clear()
  '        For i = 0 To ds.Tables(0).Rows.Count - 1
  '          x(i) = ds.Tables(0).Rows(i)("Start")
  '          y(i) = ds.Tables(0).Rows(i)("Metric")
  '          z(i) = ds.Tables(0).Rows(i)("DurationSec")
  '        Next
  '        chart1.ImageAreas.GetByName("ChartData").Tooltip = "({#XVAL},{#YVAL})"
  '        Dim cds1 As ChartDataSeries = chart1.ChartGroups.Group0.ChartData.SeriesList.InsertNewSeries(0)
  '        chart1.ChartGroups.Group0.ChartType = Chart2DTypeEnum.XYPlot
  '        cds1.Label = "Metric"
  '        cds1.X.CopyDataIn(x)
  '        cds1.Y.CopyDataIn(y)
  '        cds1.LineStyle.Color = Color.FromArgb(27, 27, 117)
  '        cds1.SymbolStyle.Shape = SymbolShapeEnum.None
  '        Dim cds2 As ChartDataSeries = chart1.ChartGroups.Group1.ChartData.SeriesList.InsertNewSeries(0)
  '        chart1.ChartGroups.Group1.ChartType = Chart2DTypeEnum.XYPlot
  '        cds2.Label = "Time"
  '        cds2.X.CopyDataIn(x)
  '        cds2.Y.CopyDataIn(z)
  '        cds2.LineStyle.Color = Color.FromArgb(205, 225, 243)
  '        cds2.SymbolStyle.Shape = SymbolShapeEnum.None
  '        cds2.LegendEntry = True
  '        With chart1.ChartArea
  '          ' Axe X
  '          .AxisX.AnnoFormat = FormatEnum.DateShort
  '          .AxisX.TickMinor = TickMarksEnum.Cross
  '          .AxisX.TickMinor = TickMarksEnum.None
  '          '.AxisX.Min = x(0).ToOADate
  '          '.AxisX.Max = x(UBound(x)).ToOADate
  '          ' Axe Y
  '          .AxisY.Text = "Metric"
  '          .AxisY.Min = 0
  '          .AxisY.TickMinor = TickMarksEnum.Cross
  '          .AxisY.TickMinor = TickMarksEnum.None
  '          .AxisY.ForeColor = Color.Black()
  '          ' Axe Y2
  '          .AxisY2.Text = "Duration (s)"
  '          .AxisY2.Min = 0
  '          .AxisY2.TickMinor = TickMarksEnum.Cross
  '          .AxisY2.TickMinor = TickMarksEnum.None
  '          .AxisY2.ForeColor = Color.Black()
  '        End With

  '      Else
  '        tr = New TableRow
  '        td = New TableCell
  '        td.VerticalAlign = VerticalAlign.Middle
  '        td.HorizontalAlign = HorizontalAlign.Left
  '        td.Controls.Add(AddComponents.CreateLabel("Nothing to display ...", "errormsg", 0))
  '        tr.Cells.Add(td)
  '        t.Controls.Add(tr)
  '      End If
  '    End While
  '  End If
  '  p.Controls.Add(t)
  '  Return p
  'End Function

End Module

End Namespace
