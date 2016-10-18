
Namespace HyperHelp

  Public Module AddComponents

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
          dg.Width = System.Web.UI.WebControls.Unit.Percentage(w)
        End If
        If wType = "p" Then
          dg.Width = System.Web.UI.WebControls.Unit.Percentage(w)
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

    '  Public Function CreateChart(ByVal Title As String, ByVal w As Integer, ByVal h As Integer, ByVal legend As Boolean) As C1.Web.C1WebChart.C1WebChart
    '      Dim c As New C1.Web.C1WebChart.C1WebChart
    '      c.Header.Text = Title
    '      ' style
    '      c.ChartStyle.BackColor = Color.White
    '      c.BorderStyle = BorderStyle.Solid
    '      c.BorderColor = Color.Black
    '      c.BorderWidth = c.BorderWidth.Pixel(1)
    '      ' Dimension
    '      c.Width = c.Width.Pixel(w)
    '      c.Height = c.Height.Pixel(h)
    '      If legend Then
    '          ' Legend
    '          c.Legend.Visible = True
    '          c.Legend.Compass = C1.Win.C1Chart.CompassEnum.South
    '          c.Legend.Style.Border.BorderStyle = C1.Win.C1Chart.BorderStyleEnum.RaisedBevel
    '          c.Legend.Style.BackColor = Color.WhiteSmoke
    '      Else
    '          c.Legend.Visible = False
    '      End If

    '      Return c
    '  End Function
  End Module

End Namespace
