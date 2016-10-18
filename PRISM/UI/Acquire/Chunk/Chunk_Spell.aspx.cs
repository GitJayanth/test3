using System;
using System.IO;
using System.Collections;
using System.Web.Caching;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HyperCatalog.SpellChecker;
/*using NetSpell.SpellChecker;
using NetSpell.SpellChecker.Dictionary;*/

namespace HyperCatalog.UI.Main.UI.Acquire.Chunk
{
	/// <summary>
	/// Description résumée de Chunk_Spell.
	/// </summary>
	public partial class Chunk_Spell : HCPage
	{
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
    }
    #endregion

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (IsUserCallback())
                return;
        }

    private bool IsUserCallback()
    {
      if (Request.QueryString["callback"] != null)
      {
        string text = Request.QueryString["value"].ToString();
        string culture = HyperCatalog.Shared.SessionState.MasterCulture.LanguageCode;//Request.QueryString["culture"].ToString();
        try
        {
      /*    SpellChecker.SpellTester spellTest;
          if (HttpContext.Current.Cache["WordDictionary_" + culture]==null)
          {
            NetSpell.SpellChecker.Dictionary.WordDictionary wordDictionary = new NetSpell.SpellChecker.Dictionary.WordDictionary();
            wordDictionary.EnableUserFile = false;

            //getting folder for dictionaries
            string folderName = ConfigurationManager.AppSettings["DictionaryFolder"];

            folderName = Server.MapPath(folderName);
            wordDictionary.DictionaryFolder = folderName;
            wordDictionary.DictionaryFile = HyperCatalog.Business.Culture.GetByKey(culture).DictionaryName + "."+SessionState.CacheParams["DictionaryFileExt"].Value.ToString();
            //load and initialize the dictionary
            wordDictionary.Initialize();

            // Store the Dictionary in cache
            HttpContext.Current.Cache.Insert("WordDictionary_" + culture, wordDictionary, new CacheDependency(Path.Combine(folderName, wordDictionary.DictionaryFile)));
          }
          spellTest = new SpellTester((WordDictionary)HttpContext.Current.Cache["WordDictionary_" + culture]);
          spellTest.HasErrors(text.ToString());
          Response.Write(spellTest.NbMispelled);*/
        }
        catch
        {
          Response.Write("0");
        }
        Response.Flush();
        Response.End();
        return true;
      }

      return false;
    }
	}
}
