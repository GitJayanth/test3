Namespace HyperHelp

Public Module ChartsUtils
  '---------------------------------------------------------------------------------------------
  ' CHARTS UTILS -- OWC Component --
  '
  '
  '0=Clustered Column
  '1=Stacked Column
  '2=100% Stacked Column
  '3=Clustered Bar
  '4=Stacked Bar
  '5=100% Stacked Bar
  '6=Line
  '8=Stacked Line
  '10=100% Stacked Line
  '7=Line with Markers
  '9=Stacked Line with Markers
  '11=100% Stacked Line with Markers
  '12=Smooth Line
  '14=Stacked Smooth Line
  '16=100% Stacked Smooth Line
  '13=Smooth Line with Markers
  '15=Stacked Smooth Line with Markers
  '17=100% Stacked Smooth Line with Markers
  '18=Pie
  '19=Pie Exploded
  '20=Stacked Pie
  '21=Scatter
  '25=Scatter with Lines
  '24=Scatter with Markers and Lines
  '26=Filled Scatter
  '23=Scatter with Smooth Lines
  '22=Scatter with Markers and Smooth Lines
  '27=Bubble
  '28=Bubble with Lines
  '29=Area
  '30=Stacked Area
  '31=100% Stacked Area
  '32=Doughnut
  '33=Exploded Doughnut
  '34=Radar with Lines
  '35=Radar with Lines and Markers
  '36=Filled Radar
  '37=Radar with Smooth Lines
  '38=Radar with Smooth Lines and Markers
  '39=High-Low-Close
  '40=Open-High-Low-Close
  '41=Polar
  '42=Polar with Lines
  '43=Polar with Lines and Markers
  '44=Polar with Smooth Lines
  '45=Polar with Smooth Lines and Markers
  '---------------------------------------------------------------------------------------------

  Public Function InitChart(ByVal ShowLegend, ByVal ShowTitle, ByVal Title)
    Dim c, ChartSpace, oChart

    ChartSpace = CreateObject("OWC.chart")
    c = ChartSpace.Constants

    ' -- Clear the Chartspace --
    ChartSpace.Clear()

    ' -- Add a Fisrt Chart
    AddChart(ChartSpace, 0)
    oChart = ChartSpace.Charts(0)

    ' -- set the graph legend --
    oChart.HasLegend = False
    If ShowLegend = True Then
      oChart.HasLegend = True
      oChart.Legend.Position = c.chLegendPositionBottom
    End If

    ' -- set the graph title --
    oChart.HasTitle = False
    If ShowTitle = True Then
      oChart.HasTitle = True
      oChart.Title.Caption = Title
    End If

    InitChart = ChartSpace
  End Function

  Public Sub AddChart(ByRef ChartSpace, ByVal ChartIndex)
    Dim oChart

    ' -- Add a new chart --
    oChart = ChartSpace.Charts.Add(ChartIndex)
  End Sub

  Public Sub AddSerieToChart(ByRef ChartSpace, ByVal graphIndex, ByVal Serie1, ByVal Serie2, ByVal Serie3, ByVal Serie4, ByVal Serie5, ByVal chartType, ByVal Legend, ByVal absMin, ByVal absMax)
      Dim c, oChart, oLabels, nChartType
    c = ChartSpace.Constants
    oChart = ChartSpace.Charts(graphIndex)

    If absMin <> "" And absMax <> "" Then
      '-- Set the value axis minimum and maximum --
      oChart.Axes(Int(c.chAxisPositionBottom)).MajorUnit = 1
      oChart.Axes(Int(c.chAxisPositionBottom)).Scaling.Minimum = absMin
      oChart.Axes(Int(c.chAxisPositionBottom)).Scaling.Maximum = absMax
    End If

    '-- Set the value axis minimum and maximum --
    oChart.Axes(Int(c.chAxisPositionLeft)).Scaling.Minimum = 0

    ' -- Set the Chart type --
    oChart.Type = chartType

    ' -- Set up the series on the chart based on the chart type that was selected --
    Select Case chartType

      '===================================================================================
      ' Chart Type: Bar, Column, Stacked Pie, Doughnut, Line (and Variants of those types)
      '
      ' Each series has Categories (absAxe) and Values (ordAxe)
      '====================================================================================

    Case 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 20, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38

        'Add the first series and specify the categories and values
        With oChart.SeriesCollection.Add
          .Caption = Legend
          .SetData(c.chDimCategories, c.chDataLiteral, Serie1)
          .SetData(c.chDimValues, c.chDataLiteral, Serie2)
        End With

        '===================================================================================
        ' Chart Type: Pie and Exploded Pie
        '
        ' Only create a single series for the chart.
        ' The series has Categories (absAxe) and Values (ordAxe)
        ' Also display data labels for each pie slice as percentages
        '====================================================================================
      Case 18, 19

        With oChart.SeriesCollection.Add
          .SetData(c.chDimCategories, c.chDataLiteral, Serie1)
          .SetData(c.chDimValues, c.chDataLiteral, Serie2)
          oLabels = .DataLabelsCollection.Add
          oLabels.HasValue = False
          oLabels.HasPercentage = True
        End With

        '===================================================================================
        ' Chart Type: Scatter and Bubble (and Variants of those Types)
        '
        ' The series has XValues (absAxe) and YValues (ordAxe)
        '====================================================================================

      Case 21, 22, 23, 24, 25, 26, 27, 28

        ' -- Add a series and specify the XValues and YValues --
        With oChart.SeriesCollection.Add
          .Caption = Legend
          .SetData(Int(c.chDimXValues), Int(c.chDataLiteral), Serie1)
          .SetData(Int(c.chDimYValues), Int(c.chDataLiteral), Serie2)
        End With

        ' -- Add Bubble Values for Bubble type charts only --
        If nChartType = 27 Or nChartType = 28 Then
          oChart.SeriesCollection(0).SetData(Int(c.chDimBubbleValues), Int(c.chDataLiteral), Serie1)
          oChart.SeriesCollection(1).SetData(Int(c.chDimBubbleValues), Int(c.chDataLiteral), Serie2)
        End If

        '===================================================================================
        ' Chart Type: High-Low-Close (HLC) and Open-High-Low-Close (OHLC)
        '
        ' Create one series for the chart
        ' HLC charts have Categories (Serie1), High Values (Serie2), Low Values (Serie3) and Close Values (Serie4)
        ' OHLC has those same values plus Open Values (Serie5)
        '====================================================================================
      Case 39, 40

        With oChart.SeriesCollection.Add
          .Caption = Legend
          .SetData(c.chDimCategories, c.chDataLiteral, Serie1)
          .SetData(c.chDimHighValues, c.chDataLiteral, Serie2)
          .SetData(c.chDimLowValues, c.chDataLiteral, Serie3)
          .SetData(c.chDimCloseValues, c.chDataLiteral, Serie4)

          'Add Open values for OHLC chart type
          If nChartType = 40 Then
            .SetData(c.chDimOpenValues, c.chDataLiteral, Serie5)
          End If

        End With

        '===================================================================================
        ' Chart Type: Polar
        '
        ' Create one series for the chart
        ' The series contains R (Serie1) and Theta (Serie2) Values
        '====================================================================================

      Case 41, 42, 43, 44, 45

        With oChart.SeriesCollection.Add
          .SetData(c.chDimRValues, c.chDataLiteral, Serie1)
          .SetData(c.chDimThetaValues, c.chDataLiteral, Serie2)
        End With


    End Select

  End Sub


  Public Sub CreateGanttChart(ByRef ChartSpace, ByVal graphIndex, ByVal absMin, ByVal absMax, ByVal absUnit, ByVal absFormat, ByVal Ordonnee, ByVal Serie1, ByVal Serie2)
    Dim c, oChart
    c = ChartSpace.Constants
    oChart = ChartSpace.Charts(graphIndex)


    '-- Set the Chart type --
    oChart.Type = c.chChartTypeBarStacked

    oChart.SetData(c.chDimCategories, c.chDataLiteral, Ordonnee)

    '-- Add the first series to the chart --
    With oChart.SeriesCollection.Add
      .SetData(c.chDimValues, c.chDataLiteral, Serie1)
      .Interior.Color = c.chColorNone
      .Border.Color = c.chColorNone
    End With

    '-- Add the second series to the chart --
    With oChart.SeriesCollection.Add
      .SetData(c.chDimValues, c.chDataLiteral, Serie2)
      .Interior.Color = RGB(51, 102, 102)
    End With

    If absMin <> "" And absMax <> "" Then
      '-- Format the category axis to show dates in intervals and remove the gridlines --
      '-- Set the value axis minimum and maximum --
      With oChart.Axes(c.chAxisPositionBottom)
        .NumberFormat = absFormat
        .Scaling.Minimum = absMin
        .Scaling.Maximum = absMax
        .MajorUnit = absUnit
      End With
    End If
  End Sub

  Public Sub SaveChartAsImg(ByRef chartSpace, ByVal graphIndex, ByVal Filename, ByVal cWidth, ByVal cHeight)
    ' -- Create a GIF image of the Chart --
    chartSpace.ExportPicture(Filename & ".gif", "GIF", cWidth, cHeight)
  End Sub

End Module

End Namespace
