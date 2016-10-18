#region Copyright (c) 1996-2005 HyperObjects, Inc. All Rights Reserved
/* ---------------------------------------------------------------------*
*                           HyperObjects, Inc.                          *
*              Copyright (c) 1996-2005 All Rights reserved              *
*        THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE.       *
* --------------------------------------------------------------------- *
*/
#endregion
#region using
using System.Xml;
using System;
using System.Collections;
using System.Collections.Specialized;
using HyperCatalog.DataAccessLayer;
#endregion

namespace HyperCatalog.Business
{
  // Created by Philippe on 11/11/2005
  /// <summary>  /// The Template concept is a HOT file that will allow user to create Previews from a content extract
  /// </summary>
  [Serializable()]
  public class Template :BaseObject
  {

    #region "Private members"

    private string _Name = String.Empty;
    #endregion
    
    #region "Public accessors"
    /// <summary>
    /// Template name
    /// </summary>
    public string Name
    {
      get { return _Name; }
      set { _Name = value; }
    }


    #endregion
    
    #region "Constructors"

    public Template(){}
    public Template(string name) 
    {
      _Name = name;
    }

    #endregion
    #region "Static methods"

    public static TemplateList GetAll() 
    {
      HyperCatalog.WebServices.DAMWS.DAMWebService templateRepository = HCPage.WSDam;
      Business.Debug.Trace("WEBUI", "DAM uri = " + templateRepository.Url, DebugSeverity.Low);
      XmlDocument xmlTemplates =  new XmlDocument();
      xmlTemplates.LoadXml(templateRepository.ResourceGetAll("Templates"));
      XmlNodeList childNodes = xmlTemplates.DocumentElement.ChildNodes;
      System.Collections.Specialized.StringCollection templates = new System.Collections.Specialized.StringCollection();
      TemplateList tList = new TemplateList();
      foreach (XmlNode n in childNodes)
      {
        tList.Add(new Template(n.Attributes["name"].InnerText));
      }
      return tList;
    }

    #endregion         
    
  }
  #region "Object list"
  [Serializable()]
  public class TemplateList:SortableCollectionBase, IEnumerable
  {
    // Method implementation from the CollectionBase class
    public void Add(Template aTemplate)
    {     
      List.Add(aTemplate);      
    }       

    // Method implementation from the CollectionBase class
    public void Remove(int index)
    {
      // Check to see if there is a Template at the supplied index.
      if (index > Count - 1 || index < 0) 
      {
        // Handle the error that occurs if the valid page index is       
        // not supplied.    
        // This exception will be written to the calling function             
        throw new Exception("index out of bounds");            
      }        
      List.RemoveAt(index);       
    } 
    // Method implementation from the CollectionBase class
    public Template this[int index]
    {
      get { return (Template) this.InnerList[index]; }
      set { this.InnerList[index] = value; }
    }   

    // Implement the method GetEnumerator from the longerface IEnumerable       
    public new TemplateEnumerator GetEnumerator() 
    {           
      return new TemplateEnumerator(this);        
    }        
    // Implement the GetEnumerator() method:        
    IEnumerator IEnumerable.GetEnumerator() 
    {            
      return GetEnumerator();        
    }

  }

  /*Define the Enumeration class to return */       
  public class TemplateEnumerator: IEnumerator 
  { 
    int nIndex;            
    TemplateList collection;            
    public TemplateEnumerator(TemplateList coll) 
    {                
      collection = coll;                
      nIndex = -1;            
    }
    // Sets the enumerator to its initial position, which is
    // before the first element in the collection.
    public void Reset() 
    { 
      nIndex = -1;
    }
    // Advances the enumerator to the next element of the 
    // collection.            
    public bool MoveNext() 
    {
      nIndex++;                
      return(nIndex < collection.Count);            
    }
    // Gets the current element in the collection.
    public Template Current 
    {
      get 
      {                    
        return((Template)collection[nIndex]);                
      }            
    }            
    // The current property on the IEnumerator longerface: 
    object IEnumerator.Current 
    {                
      get 
      {                    
        return(Current);                
      }            
    }     
  } // End of class TemplateEnumeration
  #endregion
}
