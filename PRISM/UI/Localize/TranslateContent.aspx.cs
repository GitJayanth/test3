using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using Infragistics.WebUI.UltraWebGrid;
using Infragistics.WebUI.Shared;
using System.Drawing;
using System.Xml.Xsl;
using System.Xml;
using System.Xml.XPath;
using System.IO;
public partial class UI_Localize_TranslateContent : System.Web.UI.Page
{
  DataSet MyDataSet;
  public string FileName;
  string[] Itemid = new string[1000];
  string[] Containerid = new string[1000];
  string[] TranslatedValue = new string[1000];

  private void Page_Init(object sender, EventArgs e)
  {
    GetData();
    MyWebGrid.DisplayLayout.ViewType = ViewType.Flat;
    MyWebGrid.DisplayLayout.AllowSortingDefault = AllowSorting.No;
    MyWebGrid.Bands[0].FilterOptions.AllowRowFiltering = RowFiltering.No;
    MyWebGrid.DisplayLayout.AllowColumnMovingDefault = AllowColumnMoving.None;
    MyWebGrid.EnableViewState = false;
    MyWebGrid.InitializeDataSource += new InitializeDataSourceEventHandler(MyWebGrid_InitializeDataSource);
    MyWebGrid.InitializeLayout+=new InitializeLayoutEventHandler(MyWebGrid_InitializeLayout);
    MyWebGrid.UpdateRowBatch += new UpdateRowBatchEventHandler(MyWebGrid_UpdateRowBatch);
    MyWebGrid.DataBind();
    mainToolBar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(mainToolBar_ButtonClicked);
    //mainToolBar.Items.AddLabel(" ", "CurrentFile");

  }

  void mainToolBar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
  {
    switch (be.Button.Key)
    {
      case "back":
        Response.Redirect("./TRList.aspx");
        break;
      case "submit":
        MyDataSet.WriteXml(Server.MapPath(".") + @"/Result1.xml");
        ModifyXML();
        break;
    }
  }
  protected void Page_Load(object sender, EventArgs e)
  {
      System.IO.DirectoryInfo dir = new DirectoryInfo(Server.MapPath(".") + @"/TR");
      if (dir.Exists)
      {
          FileName = dir.GetFiles("*.xml")[0].Name;
      }
  }
  public void GetData()
  {
    MyDataSet = new DataSet();
    // Load the style sheet.
    XslCompiledTransform xslt = new XslCompiledTransform();
    xslt.Load(Server.MapPath(".") + @"/Transform.xsl");
    // Execute the transform and output the results to a file.
    System.IO.MemoryStream stream = new System.IO.MemoryStream();
    if (Request["FileName"] != null)
    {
        xslt.Transform(Server.MapPath(".") + @"/TR/" + Request["FileName"].ToString(), null, stream);
        mainToolBar.Items.FromKeyLabel("CurrentFile").Text = "Selected file: " + Request["FileName"].ToString();
    }
    else
    {
        System.IO.DirectoryInfo dir = new DirectoryInfo(Server.MapPath(".") + @"/TR");
        if (dir.Exists)
        {
            FileName = dir.GetFiles("*.xml")[0].Name;
            mainToolBar.Items.FromKeyLabel("CurrentFile").Text = "Selected file: " + FileName;
            xslt.Transform(Server.MapPath(".") + @"/TR/" + FileName, null, stream);
        }
    }
    //xslt.Transform(Server.MapPath(".") + @"/TR/" + Request["FileName"].ToString(), Server.MapPath(".") + @"/Result1.xml");
    //Load DataSet With Trasformed XML
    stream.Position = 0;
    MyDataSet.ReadXml(stream);
    //MyDataSet.ReadXml(Server.MapPath(".") + @"/Result1.xml");
    MyDataSet.Tables[0].PrimaryKey = new DataColumn[] { MyDataSet.Tables[0].Columns[0], MyDataSet.Tables[0].Columns[1] };
  }
  protected void MyWebGrid_InitializeDataSource(object sender, UltraGridEventArgs e)
  {
    MyWebGrid.DataSource = MyDataSet;
    MyWebGrid.DataBind();
    MyWebGrid.Width = Unit.Percentage(100);
    for (int i = 0; i < 3; i++)
    {
        MyWebGrid.Columns[i].AllowUpdate = AllowUpdate.No;
        MyWebGrid.Columns[i].CellMultiline = CellMultiline.Yes;
        MyWebGrid.Columns[i].CellStyle.Wrap = true;
    }
    MyWebGrid.Columns[3].CellStyle.Wrap = true;
    MyWebGrid.Columns[3].CellMultiline = CellMultiline.Yes;
    MyWebGrid.Columns[0].Width = Unit.Percentage(10);
    MyWebGrid.Columns[1].Width = Unit.Percentage(10);
    MyWebGrid.Columns[2].Width = Unit.Percentage(40);
    MyWebGrid.Columns[3].Width = Unit.Percentage(40);
  }
    protected void MyWebGrid_InitializeLayout(object sender, LayoutEventArgs e)
    {
        foreach (UltraGridColumn Column in MyWebGrid.Columns)
        {
            if (Column.Header.Caption == "Translated")
            {
                Column.Type = ColumnType.Custom;
                Column.EditorControlID = "EditTranslated";
            }
        }
    }
  private void MyWebGrid_UpdateRowBatch(object sender, RowEventArgs e)
  {
    string DataKey = e.Row.DataKey.ToString();
    if (e.Row.DataChanged == DataChanged.Modified)
    {
      if (DataKey != null)
      {
        DataRow DataRow = MyDataSet.Tables[0].Rows.Find(new object[] { e.Row.Cells[0], e.Row.Cells[1] });
        if (DataRow != null)
          foreach (UltraGridCell Cell in e.Row.Cells)
          {
            if (!DataRow[Cell.Column.Key].Equals(Cell.Value))
            {
              DataRow[Cell.Column.Key] = Cell.Value;
            }
          }
      }
    }
  }
  public void GetIdsAndTranslated()
  {
    string XmlFile = Server.MapPath(".") + @"/Result1.xml";
    XmlTextReader XmlReader = new XmlTextReader(XmlFile);
    XmlReader.Read();
    int Index = 0;
    while (XmlReader.Read())
    {
      if (XmlReader.Name.ToLower().Trim() == "itemid")
      {
        XmlReader.Read();
        Itemid[Index] = XmlReader.Value;
        XmlReader.Read();
        XmlReader.Read();
        XmlReader.Read();
        XmlReader.Read();
        Containerid[Index] = XmlReader.Value;
        XmlReader.Read();
        XmlReader.Read();
        XmlReader.Read();
        XmlReader.Read();
        XmlReader.Read();
        XmlReader.Read();
        XmlReader.Read();
        XmlReader.Read();
        TranslatedValue[Index] = XmlReader.Value.Trim();
        Index++;
      }
    }
  }
  public void ModifyXML()
  {
    GetIdsAndTranslated();
    XmlDocument DocXml = new XmlDocument();
    //Open xml File
      if (Request["FileName"] != null)
    DocXml.Load(Server.MapPath(".") + @"/TR/" + Request["FileName"].ToString());
      else
    DocXml.Load(Server.MapPath(".") + @"/TR/" + FileName);
    //Get the root
    XmlElement Root = DocXml.DocumentElement;
    //Point on each container Node
    for (int i = 0; i < Itemid.Length; i++)
    {
      if (Itemid[i] == null)
        break;
      XmlNode Element = Root.SelectSingleNode("containers/group/container[@itemid='" + Itemid[i] + "' and @containerid='" + Containerid[i] + "']");
      //if the container does't have a Translated node, move to the next container
      if (Element.ChildNodes.Count < 2)
        continue;
      //Select the "translated" node
      XmlNode Translated = Element.SelectSingleNode("translated");
      //Update "translated" node's value from the created xml file.
      Translated.InnerText = TranslatedValue[i];
    }
    //Save xml file
    DocXml.Save(Server.MapPath(".") + @"/TR/" + FileName);
  }
}
