using Infragistics.UltraChart.Core.Layers;
using Infragistics.UltraChart.Resources;
using Infragistics.UltraChart.Core;
using Infragistics.UltraChart.Core.Primitives;
using Infragistics.UltraChart.Shared.Styles;
using Infragistics.UltraChart.Resources.Appearance;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using Infragistics.UltraChart.Data;
public class FloatingBarGanttLayer : ILayer 
{


	private IChartData data;
	public IChartData Data
	{
		get
		{
			return this.data;
		}
		set
		{
			this.data = value;
		}
	}
	private int startDateTimeColumnNumber = -1;
	protected int StartDateTimeColumnNumber
	{
		get
		{
			return this.startDateTimeColumnNumber;
		}
		set
		{
			this.startDateTimeColumnNumber = value;
		}
	}
	private int endDateTimeColumnNumber = -1;
	protected int EndDateTimeColumnNumber
	{
		get
		{
			return this.endDateTimeColumnNumber;
		}
		set
		{
			this.endDateTimeColumnNumber = value;
		}
	}

	protected void InitializeColumnNumbers()
	{
		this.InitializeDateTimeColumnNumbers();
		this.InitializeLabelColumnNumbers();
	}
	protected void InitializeDateTimeColumnNumbers()
	{
		this.StartDateTimeColumnNumber = -1;
		this.EndDateTimeColumnNumber = -1;
		int columnCount=this.Data.GetColumnCount();
		for (int c=0;c<columnCount;c++)
		{
			if (this.Data.IsColumnDateTime(c))
			{
				if (this.StartDateTimeColumnNumber == -1)
				{
					this.StartDateTimeColumnNumber = c;
				}
				else
				{
					this.EndDateTimeColumnNumber = c;
					return;
				}
			}
		}
	}
	protected void InitializeLabelColumnNumbers()
	{
		int columnCount=this.Data.GetColumnCount();
		for (int c=0;c<columnCount;c++)
		{
			if (this.Data.IsColumnString(c))
			{
				this.ItemLabelColumnNumber = c;
				return;	
			}
		}
	}
	protected void InitializeMinAndMaxDates()
	{
		int rowCount = this.Data.GetRowCount();
		DateTime minDateTime = DateTime.MaxValue;
		DateTime maxDateTime = DateTime.MinValue;
		for (int r=0;r<rowCount;r++)
		{
			DateTime currentStartDateTime = (DateTime)this.Data.GetObjectValue(r, this.StartDateTimeColumnNumber);
			DateTime currentEndDateTime   = (DateTime)this.Data.GetObjectValue(r, this.EndDateTimeColumnNumber);
			if (currentStartDateTime < minDateTime)
				minDateTime = currentStartDateTime;
			if (currentStartDateTime > maxDateTime)
				maxDateTime = currentStartDateTime;
			if (currentEndDateTime < minDateTime)
				minDateTime = currentEndDateTime;
			if (currentEndDateTime > maxDateTime)
				maxDateTime = currentEndDateTime;
		}
	}
	
	private DateTime minDateTime = DateTime.MaxValue;
	private DateTime maxDateTime = DateTime.MinValue;
	protected DateTime MinDateTime
	{
		get
		{
			return this.minDateTime;
		}
		set
		{
			this.minDateTime = value;
		}
	}
	protected DateTime MaxDateTime
	{
		get
		{
			return this.maxDateTime;
		}
		set
		{
			this.maxDateTime = value;
		}
	}

	private int seriesLabelColumnNumber = -1;
	private int itemLabelColumnNumber = -1;
	protected int SeriesLabelColumnNumber
	{
		get
		{
			return this.seriesLabelColumnNumber;
		}
		set
		{
			this.seriesLabelColumnNumber=value;
		}
	}
	protected int ItemLabelColumnNumber
	{
		get
		{
			return this.itemLabelColumnNumber;
		}
		set
		{
			this.itemLabelColumnNumber=value;
		}
	}
	protected void InitializeColumns()
	{
		
		this.InitializeColumnNumbers();
		this.InitializeMinAndMaxDates();
	}

	private TimeAxis xAxis;
	public TimeAxis XAxis
	{
		get
		{
			return this.xAxis;
		}
		set
		{
			this.xAxis=value;
		}
	}
	private SetLabelAxis yAxis;
	public SetLabelAxis YAxis
	{
		get
		{
			return this.yAxis;
		}
		set
		{
			this.yAxis=value;
		}
	}
	// FillSceneGraph is where you draw on your layer or manipulate the existing (graphics) Primitives.
	public void FillSceneGraph(SceneGraph scene) 
	{
		#region reset bounds to entire chart area
		this.OuterBound=this.ChartCore.GetBorderLayer().GetOuterBounds();
		this.ChartCore.GetAdornmentLayer().OuterBound=this.OuterBound;
		GraphicsContext gC = new GraphicsContext();
		gC.ResetClip();
		scene.Add(gC);
		#endregion
		#region include columns which would have been excluded in a typical ChartDataFilter
		IChartDataFilter filter = this.Data as IChartDataFilter;
		int colCount = filter.GetRawData().GetColumnCount();
		for (int c=0;c<colCount;c++)
		{
			filter.IncludeColumn(c,true);
		}
		#endregion
		foreach (Primitive p in scene)
		{
			#region remove existing primitives
			Text t = p as Text;
			if (t != null) // labels
			{
				t.SetTextString("");
				t.bounds=new Rectangle(-1,-1,-1,-1);
			}
			else
			{
				Line l = p as Line;
				if (l != null)
				{
					l.p1=new Point(-1,-1);
					l.p2=new Point(-1,-1);
				}
			}
			
			#endregion
		}
		this.InitializeColumns();
		this.InitializeAxes();
		this.FillSceneGraphXAxis(scene);
		this.FillSceneGraphYAxis(scene);
		this.FillSceneGraphFloatingBars(scene);
	}
	protected void InitializeAxes()
	{
		this.XAxis = new TimeAxis(this.ChartComponent, this.ChartCore, AxisNumber.X_Axis);
		this.YAxis = new SetLabelAxis(this.ChartComponent, this.ChartCore, AxisNumber.Y_Axis, SetLabelAxisType.ContinuousData);
		Axis oldX = (Axis)this.Grid["X"];
		Axis oldY = (Axis)this.Grid["Y"];
		#region setup data for X axis
		DataTable allDates = new DataTable();
		allDates.Columns.Add("DateTime", typeof(DateTime));
		int rowCount = this.Data.GetRowCount();
		for (int r=0;r<rowCount;r++)
		{
			allDates.Rows.Add(new object[] {this.Data.GetObjectValue(r, this.StartDateTimeColumnNumber)});
			allDates.Rows.Add(new object[] {this.Data.GetObjectValue(r, this.EndDateTimeColumnNumber)});
		}
		Infragistics.UltraChart.Data.DataTableToChartAdapter dateAdapter = new Infragistics.UltraChart.Data.DataTableToChartAdapter(allDates);
		Infragistics.UltraChart.Data.ChartDataFilter dateFilter = new Infragistics.UltraChart.Data.ChartDataFilter(dateAdapter);
		#endregion
		AxisAppearance yAppearance = (AxisAppearance)oldY.GetAppearance();
		this.XAxis.SetBounds(yAppearance.Extent, this.OuterBound.Y+this.OuterBound.Height, this.ChartCore.GetBorderLayer().OuterBound.Width - this.OuterBound.Width, this.OuterBound.Height);
		this.XAxis.Minimum = this.MinDateTime;
		this.XAxis.Maximum = this.MaxDateTime;
		this.XAxis.MapMinimum = this.OuterBound.X+yAppearance.Extent;
		this.XAxis.MapMaximum = this.OuterBound.X+this.OuterBound.Width-20;

		AxisAppearance xAppearance = (AxisAppearance)oldX.GetAppearance();		
		this.XAxis.startPoint = new Point(this.OuterBound.X, this.OuterBound.Y+this.OuterBound.Height-xAppearance.Extent);
		this.XAxis.endPoint   = new Point(this.OuterBound.X + this.OuterBound.Width, this.OuterBound.Y+this.OuterBound.Height-xAppearance.Extent);
		this.XAxis.drawingStyle = oldX.drawingStyle;
		//		this.XAxis.MajorLabelStyle=oldX.MajorLabelStyle;
		this.XAxis.SetData(dateFilter);
		
		this.YAxis.SetBounds(0,0, this.ChartCore.GetBorderLayer().OuterBound.Width - this.OuterBound.Width, this.OuterBound.Height);
		this.YAxis.Minimum=0;
		this.YAxis.Maximum=this.Data.GetRowCount();
		this.YAxis.MapMinimum = 20;
		this.YAxis.MapMaximum = this.XAxis.startPoint.Y;
		this.XAxis.span_1=(int)this.YAxis.MapMinimum;
		this.XAxis.span_2=(int)this.YAxis.MapMaximum;
	
	}
	protected void FillSceneGraphXAxis(SceneGraph scene)
	{
		#region X axis
		this.XAxis.FillSceneGraph(scene);
		Line l = new Line(new Point((int)XAxis.MapMinimum, (int)YAxis.MapMinimum), new Point((int)XAxis.MapMinimum, (int)YAxis.MapMaximum), new LineStyle());
		scene.Add(l);
		#endregion
	}
	protected void FillSceneGraphYAxis(SceneGraph scene)
	{

		
		int rowCount=this.Data.GetRowCount();
		int labelWidth = (int)this.XAxis.MapMinimum;
		double labelHeight = (this.YAxis.MapMaximum - this.YAxis.MapMinimum) / rowCount;//(YAxis.MapMinimum / YAxis.MapMaximum)*rowCount;
		for (int r=0;r<rowCount;r++)
		{
			Rectangle labelRect = new Rectangle(YAxis.OuterBound.X, (int)YAxis.Map(r), labelWidth, (int)labelHeight);
			Text t1 = new Text(labelRect, (string)this.Data.GetObjectValue(r, this.ItemLabelColumnNumber), new LabelStyle());
			t1.labelStyle.VerticalAlign=StringAlignment.Center;
			scene.Add(t1);
		}
		Line l = new Line(new Point((int)XAxis.MapMinimum, (int)YAxis.MapMaximum), new Point((int)XAxis.MapMaximum, (int)YAxis.MapMaximum), new LineStyle());		
		scene.Add(l);

	}
	protected void FillSceneGraphFloatingBars(SceneGraph scene)
	{
		int rowCount = this.Data.GetRowCount();
		double boxHeight = ((this.YAxis.MapMaximum - this.YAxis.MapMinimum) / rowCount) / 2;
		for (int r=0;r<rowCount;r++)
		{
			DateTime start = (DateTime)this.Data.GetObjectValue(r, this.StartDateTimeColumnNumber);
			DateTime end   = (DateTime)this.Data.GetObjectValue(r, this.EndDateTimeColumnNumber);
			int boxX = (int)this.XAxis.Map(start);
			int boxY = (int)this.YAxis.Map(r) + (int)(boxHeight/2);
			int boxWidth = (int)(this.XAxis.Map(end) - this.XAxis.Map(start));
			Rectangle boxRect = new Rectangle(boxX, boxY, boxWidth, (int)boxHeight);
			Box b = new Box(boxRect);
			b.PE.Fill = this.ChartColorModel.getFillColor(r, r, (double)start.Ticks);

			b.Caps = PCaps.HitTest | PCaps.Skin | PCaps.Tooltip;
			b.Column=this.StartDateTimeColumnNumber;
			b.Row=r;
			b.Chart=ChartType.BarChart;
			b.Layer = this.ChartCore.GetChartLayer();
			b.Value = 12345;
			scene.Add(b);
		}
	}
	#region Standard ILayer Implementation
	#region Properties
	// private fields for storage of properties
	private Infragistics.UltraChart.Core.ColorModel.IColorModel _chartColorModel;
	private IChartComponent _chartComponent;
	private ChartCore _chartCore;
	private Infragistics.UltraChart.Data.IChartData _chartData;
	private Hashtable _grid;
	private string _layerID;
	private Rectangle _outerBound;
	private bool _visible;

	public Infragistics.UltraChart.Core.ColorModel.IColorModel ChartColorModel 
	{
		get 
		{
			return _chartColorModel;
		}
		set 
		{
			_chartColorModel = value;
		}
	}
	public IChartComponent ChartComponent 
	{
		get 
		{
			return _chartComponent;
		}
		set 
		{
			_chartComponent = value;
		}
	}
	public ChartCore ChartCore 
	{
		get 
		{
			return _chartCore;
		}
		set 
		{
			_chartCore = value;
		}
	}
	public Infragistics.UltraChart.Data.IChartData ChartData 
	{
		get
		{
			return _chartData;
		}
		set 
		{
			_chartData = value;
		}
	}
	public string GetDataInvalidMessage() 
	{
		return "Invalid Data.";
	}
	public bool Visible 
	{
		get 
		{
			return _visible;
		}
		set 
		{
			_visible = value;
		}
	}
	public System.Collections.Hashtable Grid 
	{
		get 
		{
			return _grid;
		}
		set 
		{
			_grid = value;
		}
	}
	public string LayerID 
	{
		get 
		{
			return _layerID;
		}
		set 
		{
			_layerID = value;
		}
	}
	public System.Drawing.Rectangle OuterBound 
	{
		get 
		{
			return _outerBound;
		}
		set 
		{
			_outerBound = value;
		}
	}
	#endregion
	public System.Drawing.Rectangle GetInnerBounds() 
	{
		return _outerBound;
	}
	#endregion
}
