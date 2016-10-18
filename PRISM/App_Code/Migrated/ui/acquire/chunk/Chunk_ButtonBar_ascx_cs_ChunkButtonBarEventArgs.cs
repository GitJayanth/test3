//====================================================================
// This file is generated as part of Web project conversion.
// The extra class 'ChunkButtonBarEventArgs' in the code behind file in 'ui\acquire\chunk\Chunk_ButtonBar.ascx.cs' is moved to this file.
//====================================================================




namespace HyperCatalog.UI.Acquire.Chunk
 {

  using System;
  using System.Data;
  using System.Drawing;
  using System.Web;
  using System.Web.UI.WebControls;
  using System.Web.UI.HtmlControls;
  using HyperCatalog.Business;
  using HyperCatalog.Shared;

  public class ChunkButtonBarEventArgs : EventArgs
  {
    private bool _LockTranslations = false;
    public bool LockTranslations
    {
      get { return _LockTranslations;}
      set { _LockTranslations = value;}
    }
    public ChunkButtonBarEventArgs(bool lockTranslations)
    {
      _LockTranslations = lockTranslations;
    }
  }

}