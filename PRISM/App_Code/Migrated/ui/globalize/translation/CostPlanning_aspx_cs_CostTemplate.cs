//====================================================================
// This file is generated as part of Web project conversion.
// The extra class 'CostTemplate' in the code behind file in 'ui\globalize\translation\CostPlanning.aspx.cs' is moved to this file.
//====================================================================


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


namespace HyperCatalog.UI.Globalize.Translation
 {


  public class CostTemplate : ITemplate
  {
    public string DataField;
    public bool Editable = false;
    public CostTemplate(string dataField, bool editable)
    {
      DataField = dataField;
      Editable = editable;
    }


    #region Membres de ITemplate

    public void InstantiateIn(Control container)
    {
      if (Editable)
      {
        Infragistics.WebUI.WebDataInput.WebNumericEdit numEdit = new Infragistics.WebUI.WebDataInput.WebNumericEdit();
        numEdit.MinValue = 0;
        numEdit.Width = new Unit("25px");
        numEdit.ClientSideEvents.ValueChange = "valueChangeInEdit";
        //      numEdit.ValueChange += new Infragistics.WebUI.WebDataInput.ValueChangeHandler(numEdit_ValueChange);
        numEdit.DataBinding += new EventHandler(onDataBinding);
        container.Controls.Add(numEdit);
      }
      else
      {
        Label num = new Label();
        num.DataBinding += new EventHandler(onDataBinding);
        container.Controls.Add(num);
      }
    }

    #endregion

    public delegate void ValueChangedHandler(object sender, CostTemplate template, Infragistics.WebUI.WebDataInput.ValueChangeEventArgs e);
    public ValueChangedHandler ValueChanged;

    private void onDataBinding(object sender, EventArgs e)
    {
      int value = Convert.ToInt32(((DataRowView)((DataGridItem)((Control)sender).NamingContainer).DataItem)[DataField]);
      if (sender is Infragistics.WebUI.WebDataInput.WebNumericEdit)
        ((Infragistics.WebUI.WebDataInput.WebNumericEdit)sender).ValueInt = value;
      else if (sender is Label)
        ((Label)sender).Text = value.ToString();

    }

    private void numEdit_ValueChange(object sender, Infragistics.WebUI.WebDataInput.ValueChangeEventArgs e)
    {
      if (ValueChanged!=null)
        ValueChanged(sender,this,e);
    }
  }

}