using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using Infragistics.WebUI.UltraWebNavigator;
using System.Xml.Xsl;

public partial class UI_Localize_TRList : System.Web.UI.Page
{
  string _filter = "*.xml";
  string _path = null;//(string)SessionState.CacheParams["TRPhysicalPath"].Value;
  string[] Country = new string[200];
  Node SelectedNode = null;
  
  protected void Page_Load(object sender, EventArgs e)
  {
    _path = System.IO.Path.Combine(this.Server.MapPath("."), "TR");
    if (!IsPostBack)
    {
        MakeFileList();
        TreeView.Nodes[0].Nodes[0].Selected = true;
    }
  }

  #region Code généré par le Concepteur Web Form
  override protected void OnInit(EventArgs e)
  {
    //
    // CODEGEN : Cet appel est requis par le Concepteur Web Form ASP.NET.
    //
    InitializeComponent();
    base.OnInit(e);
  }

  /// <summary>
  /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
  /// le contenu de cette méthode avec l'éditeur de code.
  /// </summary>
  private void InitializeComponent()
  {
    mainToolBar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(mainToolBar_ButtonClicked);
  }
  #endregion

  private void MakeFileList()
  {
     
    System.IO.DirectoryInfo dir = new DirectoryInfo(_path);
    if (dir.Exists)
    {
        if (TreeView.SelectedNode!=null)
        {
            SelectedNode = TreeView.SelectedNode;
        }
        TreeView.ClearAll();
        int i=0;
        foreach (FileInfo f in dir.GetFiles(_filter))
        {
            bool ExistCountry = false;
            for (int j = 0; j <= i-1; j++)
            {
                if (f.Name.Split('_')[3].Split('.')[0] == Country[j])
                    ExistCountry = true;
            }
            if (!ExistCountry)
            {
                Country[i] = f.Name.Split('_')[3].Split('.')[0];
            }
            i++;
        }
        foreach (string Parent in Country)
        {
            Node Nd = new Node();
            Nd.Text = Parent;
            foreach (FileInfo f in dir.GetFiles(_filter))
            {
                if (f.Name.Split('_')[3].Split('.')[0] == Parent)
                {
                    Node SubNd = new Node();
                    SubNd.Text = f.Name;
                    SubNd.DataKey = f.Name;
                    SubNd.TargetUrl = "TranslateContent.aspx?FileName=" + f.Name;
                    SubNd.TargetFrame = "main";
                    SubNd.ToolTip = "Size: " + convertBytes(f.Length) + ". Last access time: " + f.LastAccessTime.ToString();
                    Nd.Nodes.Add(SubNd);
                }
            }
            TreeView.Nodes.Add(Nd);
        }
        if (SelectedNode != null)
        {
            if (SelectedNode.Nodes.Count == 0)
                TreeView.Nodes.SearchByTargetURL("TranslateContent.aspx?FileName=" + SelectedNode.Text, TreeView.Nodes[0], false).Selected = true;
            else
                foreach (Node Nd in TreeView.Nodes)
                {
                    if (Nd.Text == SelectedNode.Text)
                    {
                        Nd.Selected = true;
                        break;
                    }
                }
        }
        
       }
    else
    { }
  }

  void mainToolBar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
  {
      switch (be.Button.Key)
      {
          case "refresh":
              MakeFileList();
              break;
      }
    
  }
    public string convertBytes(long bytes)
    {
        //Converts bytes into a readable 
        if (bytes >= 1073741824)
            return String.Format(Convert.ToString(bytes / 1024 / 1024 / 1024), "#0.00") + " GB";
        else if (bytes >= 1048576)
            return String.Format(Convert.ToString(bytes / 1024 / 1024), "#0.00") + " MB";
        else if (bytes >= 1024)
            return String.Format(Convert.ToString(bytes / 1024), "#0.00") + " KB";
        else if (bytes > 0 && bytes < 1024)
            return bytes + " Bytes";
        else
            return "0 Byte";
    }
}
