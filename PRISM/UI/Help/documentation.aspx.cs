using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using HyperCatalog.Business.DAM;
using HyperCatalog.Shared;
using System.Text;


namespace HyperCatalog.UI.Help
{
    /// <summary>
    /// Description résumée de Documentation.
    /// </summary>
    public partial class Documentation : HCPage
    {
        private Business.DAM.Library _CurrentLibrary;
        protected Business.DAM.Library CurrentLibrary
        {
            get
            {
                if (_CurrentLibrary == null)
                    _CurrentLibrary = HyperCatalog.Business.DAM.Library.GetByKey(SessionState.CacheParams["Help_DAMDocumentationLibrary"].Value.ToString());
                return _CurrentLibrary;
            }
        }
        private ResourceType[] _resourceTypes;
        protected ResourceType[] resourceTypes
        {
            get
            {
                if (_resourceTypes == null)
                    _resourceTypes = CurrentLibrary != null ? ResourceType.GetAllForLibrary(_CurrentLibrary.Id) : ResourceType.GetAll();
                return _resourceTypes;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Shared.SessionState.User.HasCapability(Business.CapabilitiesEnum.MANAGE_RESOURCES))
                {
                    UITools.ShowToolBarButton(mainToolBar.Items.FromKeyButton("Add"));
                    UITools.ShowToolBarButton(mainToolBar.Items.FromKeyButton("Delete"));
                }
                else
                {
                    UITools.HideToolBarButton(mainToolBar.Items.FromKeyButton("Add"));
                    UITools.HideToolBarButton(mainToolBar.Items.FromKeyButton("Delete"));
                }
                LoadGrid();
            }
            base.OnLoad(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            //DataBind();
            base.OnPreRender(e);
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
            pTb.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(pTb_ButtonClicked);
        }
        #endregion

        private void LoadGrid()
        {
            Business.DAM.Library lib = CurrentLibrary;
            if (lib != null)
            {
                txtResourceTypeValue.DataSource = resourceTypes;
                txtResourceTypeValue.DataBind();
                documents.DataSource = lib.Resources;
                documents.DataBind();
            }
            mainToolBar.Items.FromKeyButton("Add").Pressed(false);
        }


        protected void pTb_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
        {
            switch (be.Button.Key.ToLower())
            {
                case "save":
                    if (Shared.SessionState.User.HasCapability(Business.CapabilitiesEnum.MANAGE_RESOURCES))
                    {
                        if (Shared.SessionState.User.IsReadOnly)
                            return;

                        if (txtResourceNameValue.Text == null || txtResourceNameValue.Text.Length == 0)
                        {
                            SetMessage(propertiesMsgLbl, "Resource must have a name", MessageLevel.Error);
                            break;
                        }
                        string[][] attrs = new string[1][];

                        try
                        {
                            HyperCatalog.Business.DAM.Resource newResource = CurrentLibrary.AddResource(txtResourceNameValue.Text + Path.GetExtension(txtFileBinaryValue.PostedFile.FileName), txtResourceDescriptionValue.Value, Convert.ToInt32(txtResourceTypeValue.SelectedValue), HyperCatalog.Shared.SessionState.MasterCulture.Code, txtFileBinaryValue.PostedFile.InputStream, attrs);
                            LoadGrid();

                        }
                        catch (Exception ex)
                        {
                            SetMessage(propertiesMsgLbl, "InValid file format. Please select the correct file.", MessageLevel.Error);
                            LoadGrid();
                            break;
                        }

                    }
                    break;
            }
        }

        private enum MessageLevel : int
        {
            Information = 0,
            Warning = 1,
            Error = 2
        }

        private static string[] cssClassLevels = { "lbl_msgInformation", "lbl_msgWarning", "lbl_msgError" };

        private static void SetMessage(Label label, string text, MessageLevel level)
        {
            if (label.ID == "propertiesMsgLbl")
            {
                label.Page.ClientScript.RegisterStartupScript(label.Page.GetType(), "start", "document.getElementById('addPanel').style.display='none';", true);
            }
            label.Text = text;
            label.CssClass = cssClassLevels[(int)level];
            label.Visible = true;
        }

        protected void mainToolBar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
        {
            switch (be.Button.Key.ToLower())
            {
                case "delete":
                    DeleteSelectedItems();
                    break;
            }
        }

        public void DeleteSelectedItems()
        {
            int nbDeletedRows = 0;
            int nbSelectedRows = 0;
            for (int i = 0; i < documents.Items.Count; i++)
            {
                CheckBox cb = (CheckBox)documents.Items[i].Cells[0].FindControl("g_sd");
                if (cb.Checked)
                {
                    HyperCatalog.Business.DAM.Resource CurrentResource = HyperCatalog.Business.DAM.Resource.GetByKey(_CurrentLibrary.Id, Convert.ToInt32(documents.DataKeys[i]));
                    if (SessionState.User.HasCapability(Business.CapabilitiesEnum.MANAGE_RESOURCES) && CurrentResource != null)
                    {
                        //if (SessionState.User.IsReadOnly || CurrentResource.Delete())
                        //{
                        //    //SetMessage(propertiesMsgLbl, "Resource \"" + CurrentResource.Name + "\" deleted", MessageLevel.Information);
                        //    LoadGrid();
                        //}
                        //else
                        //{
                        //    //SetMessage(propertiesMsgLbl, "Resource \"" + CurrentResource.Name + "\" could not be deleted", MessageLevel.Error);
                        //    LoadGrid();
                        //}
                        CurrentResource.Delete();
                    }
                }
            }
            LoadGrid();
        }
    }
}
