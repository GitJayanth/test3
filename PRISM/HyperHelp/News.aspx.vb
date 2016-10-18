#Region "Uses"
Imports HyperComponents.Data.dbAccess
Imports System.Data.SqlClient
Imports HyperCatalog.Business
Imports HyperCatalog.Business.Settings
Imports System.Configuration.ConfigurationManager
#End Region

Namespace HyperHelp

  Partial Class News
    Inherits System.Web.UI.Page

#Region " Code généré par le Concepteur Web Form "

    'Cet appel est requis par le Concepteur Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents btCancel As System.Web.UI.WebControls.Button
    Protected WithEvents btnSave As System.Web.UI.WebControls.Button
    Protected WithEvents btnDelete As System.Web.UI.WebControls.Button
    Protected WithEvents lbSave As System.Web.UI.WebControls.Label

    'REMARQUE : la déclaration d'espace réservé suivante est requise par le Concepteur Web Form.
    'Ne pas supprimer ou déplacer.

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
      'CODEGEN : cet appel de méthode est requis par le Concepteur Web Form
      'Ne le modifiez pas en utilisant l'éditeur de code.
      InitializeComponent()
    End Sub

#End Region

#Region "global variables"

    Private HyperHelpConn As Database
    Public applicationName As String
    Public UserId As String

    Public nId As String = String.Empty
    Public author As String = String.Empty
    Public pageTitle As String = String.Empty
    Public shortDescription As String = String.Empty
    Public description As String = String.Empty
    Public applicationId As String

#End Region

    Public Sub LoadEnvironment()
            Dim instanceName As String = HttpContext.Current.Request.ApplicationPath.Split("/")(1).ToUpper()
            Dim ConnectionString As String
      If (Not AppSettings(instanceName + "_ConnectionString") Is Nothing) Then
        ConnectionString = AppSettings(instanceName + "_ConnectionString")
      End If
      Values = New WebConfigSettingValues()
      Dim newToken As String = String.Empty
      Dim curToken As String = String.Empty
      If (Not HttpContext.Current.Request("t") Is Nothing) Then
        newToken = HttpContext.Current.Request("t").ToString()
      End If
      If (Not HttpContext.Current.Session("userToken") Is Nothing) Then
        curToken = HttpContext.Current.Session("userToken").ToString()
      End If

      Dim canManageNews As String = "0"
      If (newToken <> curToken) Then
        HttpContext.Current.Session("userToken") = newToken
        Dim CurrentUser As HyperCatalog.Business.User = HyperCatalog.Business.User.GetByToken(New Guid(newToken))
        If (Not CurrentUser Is Nothing) Then
          HttpContext.Current.Session("UserId") = CurrentUser.Id
          If (CurrentUser.HasCapability(HyperCatalog.Business.CapabilitiesEnum.MANAGE_NEWS)) Then
            canManageNews = "1"
          End If
        End If
      End If
      HttpContext.Current.Session("CanManageNews") = canManageNews
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
      If Not Page.IsPostBack Then
        LoadEnvironment()
      End If
      HyperHelpConn = New Database(HyperCatalog.Business.ApplicationSettings.Components("HyperHelp_DB").ConnectionString)
      applicationName = HyperCatalog.Business.Settings.Values("applicationname")

      If (HttpContext.Current.Session("UserId") = Nothing) And (HttpContext.Current.Session("RoleId") = Nothing) Then
        lbErrorNews.Text = "[ERROR] Your are not logged, please contact the support team"
        lbErrorNews.Visible = True
        uwToolBar.Items.FromKey("Add").DefaultStyle.Width = Unit.Pixel(0)
        dgNews.Visible = False
      Else
        UserId = HttpContext.Current.Session("UserId").ToString()
        lbErrorNews.Visible = False
        lbNoNews.Visible = False
        'Retrieve the ApplicationId
        Dim rs As SqlDataReader = HyperHelpConn.RunSQLReturnRS("SELECT ApplicationId FROM Applications WHERE ApplicationName='" + applicationName + "'")
        If (HyperHelpConn.LastError = String.Empty) Then
          If rs.HasRows Then
            rs.Read()
            applicationId = rs("ApplicationId").ToString()
          End If
          rs.Close()
          rs = Nothing
        Else
          lbErrorNews.Text = "This application name " + applicationName + " does not exist, please contact the support team"
          lbErrorNews.Visible = True
        End If
        If (Not Page.IsPostBack) Then
          If (Not Request("n") Is Nothing) Then
            UpdateDataEdit(Request("n").ToString())
          Else
            ShowData()
          End If
        End If
      End If
      Dim RoleId As String = "0"
      If (Not HttpContext.Current.Session("CanManageNews") Is Nothing) Then
        RoleId = HttpContext.Current.Session("CanManageNews").ToString()
      End If
      If (RoleId = 0) Then
        uwToolBar.Enabled = False
        dgNews.Visible = False
        lbErrorNews.Text = "Sorry, you don't have the permission to manage news!"
        If (HttpContext.Current.Session("CanManageNews") Is Nothing) Then
          lbErrorNews.Text = lbErrorNews.Text + " (issue in transferring session values)"
        End If
        lbErrorNews.Visible = True
      End If
      lbError.Visible = False
    End Sub

    Private Sub ShowData()
      Dim sql As String = "SELECT * FROM News WHERE ApplicationId=" + applicationId + " ORDER BY Date"
            Dim ds As Data.DataSet
            ds = HyperHelpConn.RunSQLReturnDataSet(sql, "News")
      If (HyperHelpConn.LastError = String.Empty) Then
        If ds.Tables(0).Rows.Count > 0 Then
          dgNews.Visible = True
          dgNews.DataSource = ds.Tables("News").DefaultView()
          dgNews.DataBind()
        Else
          dgNews.Visible = False
          lbNoNews.Visible = True
        End If
      End If
    End Sub

    Private Sub UpdateDataEdit(ByVal NewId As String)
      Dim RoleId As String = "0"
      If (Not HttpContext.Current.Session("CanManageNews") Is Nothing) Then
        RoleId = HttpContext.Current.Session("CanManageNews").ToString()
      End If
      If (RoleId = 0) Then
        uwToolBar2.Enabled = False
      End If
      pnNews.Visible = False
        pnEditNews.Visible = True
        nId = NewId
        If NewId = "-1" Then
          uwToolBar2.Items.FromKey("Delete").DefaultStyle.Width = Unit.Pixel(0)
          uwToolBar2.Items.FromKey("DeleteSep").Width = Unit.Pixel(0)
          Page.DataBind()
        Else
          Dim sql As String = "SELECT * FROM News WHERE NewsId=" & NewId
          Dim rs As SqlDataReader = HyperHelpConn.RunSQLReturnRS(sql)
          If rs.HasRows Then
            rs.Read()
            pageTitle = rs("Title")
            shortDescription = rs("ShortDescription")
            description = rs("Description")
            author = "By " + rs("Author")
            uwToolBar2.Items.FromKey("Delete").DefaultStyle.Width = uwToolBar2.Items.FromKey("Delete").ToolBar.ItemWidthDefault
            uwToolBar2.Items.FromKey("DeleteSep").Width = Unit.Pixel(8)
            Page.DataBind()
            rs.Close()
            rs = Nothing
          Else
            lbError.Text = "[Error] display news failed"
          End If
      End If
    End Sub

    Private Function getAuthor()
      author = HyperCatalog.Business.User.GetByKey(CType(HttpContext.Current.Session("UserId"), Integer)).FullName
      Return author
    End Function

    Private Sub Save()
      Dim r As Integer
      If txtTitle.Text.Trim <> "" And txtShortdescription.Text.Trim <> "" And txtdescription.Text.Trim <> "" Then
        If (txtNewsID.Text <> "-1") Then
          ' update a news
          HyperHelpConn.RunSPQuery("_News_Upd", New SqlParameter("@ID", txtNewsID.Text), New SqlParameter("@Author", getAuthor()), New SqlParameter("@Title", txtTitle.Text.Trim), New SqlParameter("@ShortDescription", txtShortdescription.Text.Trim), New SqlParameter("@Description", txtdescription.Text.Trim))
        Else
          ' add a news
          HyperHelpConn.RunSPQuery("_News_Add", New SqlParameter("@ApplicationId", applicationId), New SqlParameter("@Author", getAuthor()), New SqlParameter("@Title", txtTitle.Text.Trim), New SqlParameter("@ShortDescription", txtShortdescription.Text.Trim), New SqlParameter("@Description", txtdescription.Text.Trim))
        End If
        If (HyperHelpConn.LastError <> String.Empty And r < 0) Then
          lbError.Text = "[Error] save news failed - " + HyperHelpConn.LastError.ToString()
          lbError.Visible = True
        Else
          pnNews.Visible = True
          pnEditNews.Visible = False
          ShowData()
        End If
      Else
        lbError.Text = " all fields must be filled !"
        lbError.Visible = True
      End If
    End Sub

    ' Delete a news
    Private Sub Delete()
      Dim rs As SqlDataReader = HyperHelpConn.RunSQLReturnRS("_News_Del " & txtNewsID.Text)
      pnNews.Visible = True
      pnEditNews.Visible = False
      ShowData()
    End Sub

    ' SELECTION IN GRIDNEWS
    ' "Title" Link Button event handler
    Protected Sub UpdateGridItem(ByVal sender As Object, ByVal e As System.EventArgs)
      Dim cellItem As Infragistics.WebUI.UltraWebGrid.CellItem
      cellItem = CType((CType(sender, System.Web.UI.WebControls.LinkButton).Parent), Infragistics.WebUI.UltraWebGrid.CellItem)
      UpdateDataEdit(cellItem.Cell.Row.Cells.FromKey("Id").Text)
    End Sub

    Private Sub uwToolBar2_ButtonClicked(ByVal sender As Object, ByVal be As Infragistics.WebUI.UltraWebToolbar.ButtonEvent) Handles uwToolBar2.ButtonClicked
      Dim btn As String = be.Button.Key.ToLower()
      If (btn = "list") Then
        pnNews.Visible = True
        pnEditNews.Visible = False
        ShowData()
      End If
      If (btn = "save") Then
        Save()
      End If
      If (btn = "delete") Then
        Delete()
      End If
    End Sub

    Private Sub uwToolBar_ButtonClicked(ByVal sender As Object, ByVal be As Infragistics.WebUI.UltraWebToolbar.ButtonEvent) Handles uwToolBar.ButtonClicked
      Dim btn As String = be.Button.Key.ToLower()
      If (btn = "add") Then
        UpdateDataEdit("-1")
      End If
    End Sub
  End Class

End Namespace
