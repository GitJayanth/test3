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

public partial class HOLoadingPage : System.Web.UI.UserControl
{
  private string text = "loading...";
  private string loadHTML1 = "<SCRIPT LANGUAGE='JavaScript'> if(document.getElementById) {var upLevel = true; }	else if(document.layers) {	var ns4 = true;	}else if(document.all) { var ie4 = true;}function showObject(obj) {	if (ns4) {	obj.visibility = 'show';}else if (ie4 || upLevel) {	obj.style.visibility = 'visible';}} function hideObject(obj) {	if (ns4) {	obj.visibility = 'hide';}	if (ie4 || upLevel) {obj.style.visibility = 'hidden';}	}</SCRIPT>";
  private string loadHTML2 = string.Empty;
  private string loadHTML3 = "<script language='javascript'>	if(upLevel)	{	var load = document.getElementById('loadingScreen');	}	else if(ns4)	{		var load = document.loadingScreen;	}	else if(ie4)	{	var load = document.all.loadingScreen;	}	hideObject(load);</script>";
  private string sourceImage = "/hc_v4/img/ed_wait.gif";
  private bool _NoProgressOnPostBack = false;

  public string Text
  {
    get
    {
      return text;
    }
    set
    {
      text = value;
    }
  }

  public string SrcImage
  {
    get
    {
      return sourceImage;
    }
    set
    {
      sourceImage = value;
    }
  }
  public bool NoProgressOnPostBack
  {
    get
    {
      return _NoProgressOnPostBack;
    }
    set
    {
      _NoProgressOnPostBack = value;
    }
  }
  public HOLoadingPage()
  {
    this.Init += new EventHandler(Init1);
    this.Load += new EventHandler(Load1);
  }

  protected void Init1(Object sender, System.EventArgs e)
  {
    if (!_NoProgressOnPostBack || !Page.IsPostBack)
    {
      System.Web.UI.UserControl tmp = sender as System.Web.UI.UserControl;
      loadHTML2 = "<div id='loadingScreen' style='POSITION: absolute;Z-INDEX:5; LEFT: 5%; TOP: 5%;'><table border=0 height=100% width=100%><tr valign=middle>  <td style='font-family: Verdana, Tahoma,arial,sans-serif;font-size: xx-small;font-weight:bold;'><center><img src='{0}'/>&nbsp;<b>" + text + "</b></td></tr></center></table></div>";
      loadHTML2 = string.Format(loadHTML2, sourceImage);
      tmp.Page.Response.Clear();
      tmp.Page.Response.Write(loadHTML1 + loadHTML2);
      tmp.Page.Response.Flush();
    }
  }

  protected void Load1(Object sender, System.EventArgs e)
  {
    if (!_NoProgressOnPostBack || !Page.IsPostBack)
    {
      System.Web.UI.UserControl tmp = sender as System.Web.UI.UserControl;
      tmp.Page.Response.Write(loadHTML3);
    }
  }
}


